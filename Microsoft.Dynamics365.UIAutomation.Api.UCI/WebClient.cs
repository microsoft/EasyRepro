// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OtpNet;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.BusinessProcessFlow;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.Entity;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class WebClient : BrowserPage, IDisposable
    {
        public List<ICommandResult> CommandResults => Browser.CommandResults;
        public Guid ClientSessionId;

        public WebClient(BrowserOptions options)
        {
            Browser = new InteractiveBrowser(options);
            OnlineDomains = Constants.Xrm.XrmDomains;
            ClientSessionId = Guid.NewGuid();
        }

        internal BrowserCommandOptions GetOptions(string commandName)
        {
            return new BrowserCommandOptions(Constants.DefaultTraceSource,
                commandName,
                Constants.DefaultRetryAttempts,
                Constants.DefaultRetryDelay,
                null,
                true,
                typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        internal BrowserCommandResult<bool> InitializeModes()
        {
            return this.Execute(GetOptions("Initialize Unified Interface Modes"), driver =>
            {
                driver.SwitchTo().DefaultContent();

                // Wait for main page to load before attempting this. If you don't do this it might still be authenticating and the URL will be wrong
                WaitForMainPage();

                string uri = driver.Url;
                if (string.IsNullOrEmpty(uri))
                    return false;

                var prevQuery = GetUrlQueryParams(uri);
                bool requireRedirect = false;
                string queryParams = "";
                if (prevQuery.Get("flags") == null)
                {
                    queryParams += "&flags=easyreproautomation=true";
                    if (Browser.Options.UCITestMode)
                        queryParams += ",testmode=true";
                    requireRedirect = true;
                }

                if (Browser.Options.UCIPerformanceMode && prevQuery.Get("perf") == null)
                {
                    queryParams += "&perf=true";
                    requireRedirect = true;
                }

                if (!requireRedirect)
                    return true;

                var testModeUri = uri + queryParams;
                driver.Navigate().GoToUrl(testModeUri);

                // Again wait for loading
                WaitForMainPage();
                return true;
            });
        }

        private NameValueCollection GetUrlQueryParams(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            Uri uri = new Uri(url);
            var query = uri.Query.ToLower();
            NameValueCollection result = HttpUtility.ParseQueryString(query);
            return result;
        }


        public string[] OnlineDomains { get; set; }

        #region PageWaits
        internal bool WaitForMainPage(TimeSpan timeout, string errorMessage)
            => WaitForMainPage(timeout, null, () => throw new InvalidOperationException(errorMessage));

        internal bool WaitForMainPage(TimeSpan? timeout = null, Action<IWebElement> successCallback = null, Action failureCallback = null)
        {
            IWebDriver driver = Browser.Driver;
            timeout = timeout ?? Constants.DefaultTimeout;
            successCallback = successCallback ?? (
                                  _ =>
                                  {
                                      bool isUCI = driver.HasElement(By.XPath(Elements.Xpath[Reference.Login.CrmUCIMainPage]));
                                      if (isUCI)
                                          driver.WaitForTransaction();
                                  });

            var xpathToMainPage = By.XPath(Elements.Xpath[Reference.Login.CrmMainPage]);
            var element = driver.WaitUntilAvailable(xpathToMainPage, timeout, successCallback, failureCallback);
            return element != null;
        }

        #endregion

        #region Login

        internal BrowserCommandResult<LoginResult> Login(Uri uri)
        {
            var username = Browser.Options.Credentials.Username;
            if (username == null)
                return PassThroughLogin(uri);

            var password = Browser.Options.Credentials.Password;
            return Login(uri, username, password);
        }

        internal BrowserCommandResult<LoginResult> Login(Uri orgUri, SecureString username, SecureString password, SecureString mfaSecretKey = null, Action<LoginRedirectEventArgs> redirectAction = null)
        {
            return Execute(GetOptions("Login"), Login, orgUri, username, password, mfaSecretKey, redirectAction);
        }

        private LoginResult Login(IWebDriver driver, Uri uri, SecureString username, SecureString password, SecureString mfaSecretKey = null, Action<LoginRedirectEventArgs> redirectAction = null)
        {
            bool online = !(OnlineDomains != null && !OnlineDomains.Any(d => uri.Host.EndsWith(d)));
            driver.Navigate().GoToUrl(uri);

            if (!online)
                return LoginResult.Success;

            driver.ClickIfVisible(By.Id(Elements.ElementId[Reference.Login.UseAnotherAccount]));

            bool waitingForOtc = false;
            bool success = EnterUserName(driver, username);
            if (!success)
            {
                var isUserAlreadyLogged = IsUserAlreadyLogged();
                if (isUserAlreadyLogged)
                {
                    SwitchToDefaultContent(driver);
                    return LoginResult.Success;
                }

                ThinkTime(1000);
                waitingForOtc = GetOtcInput(driver) != null;

                if (!waitingForOtc)
                    throw new Exception($"Login page failed. {Reference.Login.UserId} not found.");
            }

            if (!waitingForOtc)
            {
                driver.ClickIfVisible(By.Id("aadTile"));
                ThinkTime(1000);

                //If expecting redirect then wait for redirect to trigger
                if (redirectAction != null)
                {
                    //Wait for redirect to occur.
                    ThinkTime(3000);

                    redirectAction.Invoke(new LoginRedirectEventArgs(username, password, driver));
                    return LoginResult.Redirect;
                }

                EnterPassword(driver, password);
                ThinkTime(1000);
            }

            int attempts = 0;
            bool entered;
            do
            {
                entered = EnterOneTimeCode(driver, mfaSecretKey);
                success = ClickStaySignedIn(driver) || IsUserAlreadyLogged();
                attempts++;
            }
            while (!success && attempts <= Constants.DefaultRetryAttempts); // retry to enter the otc-code, if its fail & it is requested again 

            if (entered && !success)
                throw new InvalidOperationException("Something went wrong entering the OTC. Please check the MFA-SecretKey in configuration.");

            return success ? LoginResult.Success : LoginResult.Failure;
        }

        private bool IsUserAlreadyLogged() => WaitForMainPage(2.Seconds());

        private static string GenerateOneTimeCode(string key)
        {
            // credits:
            // https://dev.to/j_sakamoto/selenium-testing---how-to-sign-in-to-two-factor-authentication-2joi
            // https://www.nuget.org/packages/Otp.NET/
            byte[] base32Bytes = Base32Encoding.ToBytes(key);

            var totp = new Totp(base32Bytes);
            var result = totp.ComputeTotp(); // <- got 2FA code at this time!
            return result;
        }

        private bool EnterUserName(IWebDriver driver, SecureString username)
        {
            var input = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]), new TimeSpan(0, 0, 30));
            if (input == null)
                return false;

            input.SendKeys(username.ToUnsecureString());
            input.SendKeys(Keys.Enter);
            return true;
        }

        private static void EnterPassword(IWebDriver driver, SecureString password)
        {
            var input = driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword]));
            input.SendKeys(password.ToUnsecureString());
            input.Submit();
        }

        private bool EnterOneTimeCode(IWebDriver driver, SecureString mfaSecretKey)
        {
            try
            {
                IWebElement input = GetOtcInput(driver); // wait for the dialog, even if key is null, to print the right error
                if (input == null)
                    return true;

                string key = mfaSecretKey?.ToUnsecureString(); // <- this 2FA secret key.
                if (string.IsNullOrWhiteSpace(key))
                    throw new InvalidOperationException("The application is wait for the OTC but your MFA-SecretKey is not set. Please check your configuration.");

                var oneTimeCode = GenerateOneTimeCode(key);
                this.SetInputValue(driver, input, oneTimeCode, 1.Seconds());
                input.Submit();
                return true; // input found & code was entered
            }
            catch (Exception e)
            {
                var message = $"An Error occur entering OTC. Exception: {e.Message}";
                Trace.TraceInformation(message);
                throw new InvalidOperationException(message, e);
            }
        }


        private static IWebElement GetOtcInput(IWebDriver driver)
            => driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.OneTimeCode]), TimeSpan.FromSeconds(2));

        private static bool ClickStaySignedIn(IWebDriver driver)
        {
            var xpath = By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]);
            var element = driver.ClickIfVisible(xpath, 2.Seconds());
            return element != null;
        }

        private static void SwitchToDefaultContent(IWebDriver driver)
        {
            SwitchToMainFrame(driver);

            //Switch Back to Default Content for Navigation Steps
            driver.SwitchTo().DefaultContent();
        }

        private static void SwitchToMainFrame(IWebDriver driver)
        {
            driver.WaitForPageToLoad();
            driver.SwitchTo().Frame(0);
            driver.WaitForPageToLoad();
        }

        internal BrowserCommandResult<LoginResult> PassThroughLogin(Uri uri)
        {
            return this.Execute(GetOptions("Pass Through Login"), driver =>
            {
                driver.Navigate().GoToUrl(uri);

                WaitForMainPage(60.Seconds(),
                    _ =>
                    {
                        //determine if we landed on the Unified Client Main page
                        var isUCI = driver.HasElement(By.XPath(Elements.Xpath[Reference.Login.CrmUCIMainPage]));
                        if (isUCI)
                        {
                            driver.WaitForPageToLoad();
                            driver.WaitForTransaction();
                        }
                        else
                            //else we landed on the Web Client main page or app picker page
                            SwitchToDefaultContent(driver);
                    },
                    () => throw new InvalidOperationException("Load Main Page Fail.")
                );

                return LoginResult.Success;
            });
        }


        public void ADFSLoginAction(LoginRedirectEventArgs args)

        {
            //Login Page details go here.  You will need to find out the id of the password field on the form as well as the submit button. 
            //You will also need to add a reference to the Selenium Webdriver to use the base driver. 
            //Example

            var driver = args.Driver;

            driver.FindElement(By.Id("passwordInput")).SendKeys(args.Password.ToUnsecureString());
            driver.ClickWhenAvailable(By.Id("submitButton"), TimeSpan.FromSeconds(2));

            //Insert any additional code as required for the SSO scenario

            //Wait for CRM Page to load
            WaitForMainPage(TimeSpan.FromSeconds(60), "Login page failed.");
            SwitchToMainFrame(driver);
        }

        public void MSFTLoginAction(LoginRedirectEventArgs args)

        {
            //Login Page details go here.  You will need to find out the id of the password field on the form as well as the submit button. 
            //You will also need to add a reference to the Selenium Webdriver to use the base driver. 
            //Example

            var driver = args.Driver;

            //d.FindElement(By.Id("passwordInput")).SendKeys(args.Password.ToUnsecureString());
            //d.ClickWhenAvailable(By.Id("submitButton"), TimeSpan.FromSeconds(2));

            //This method expects single sign-on

            ThinkTime(5000);

            driver.WaitUntilVisible(By.XPath("//div[@id=\"mfaGreetingDescription\"]"));

            var azureMFA = driver.FindElement(By.XPath("//a[@id=\"WindowsAzureMultiFactorAuthentication\"]"));
            azureMFA.Click(true);

            Thread.Sleep(20000);

            //Insert any additional code as required for the SSO scenario

            //Wait for CRM Page to load
            WaitForMainPage(TimeSpan.FromSeconds(60), "Login page failed.");
            SwitchToMainFrame(driver);
        }

        #endregion

        public void WaitForLoadArea(IWebDriver driver)
        {
            driver.WaitForPageToLoad();
            driver.WaitForTransaction();
        }

        #region FormContextType

        public void SetInputValue(IWebDriver driver, IWebElement input, string value, TimeSpan? thinktime = null)
        {
            // Repeat set value if expected value is not set
            // Do this to ensure that the static placeholder '---' is removed 
            driver.RepeatUntil(() =>
            {
                input.Clear();
                input.Click();
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Backspace);
                input.SendKeys(value);
                driver.WaitForTransaction();
            },
                d => input.GetAttribute("value").IsValueEqualsTo(value),
                TimeSpan.FromSeconds(9), 3,
                failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {value}. Actual: {input.GetAttribute("value")}")
            );

            driver.WaitForTransaction();
        }
        public static void TryRemoveLookupValue(IWebDriver driver, IWebElement fieldContainer, LookupItem control, bool removeAll = true, bool isHeader = false)
        {
            var controlName = control.Name;
            fieldContainer.Hover(driver);

            var xpathDeleteExistingValues = By.XPath(Entity.EntityReference.LookupFieldDeleteExistingValue.Replace("[NAME]", controlName));
            var existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);

            var xpathToExpandButton = By.XPath(Entity.EntityReference.LookupFieldExpandCollapseButton.Replace("[NAME]", controlName));
            bool success = fieldContainer.TryFindElement(xpathToExpandButton, out var expandButton);
            if (success)
            {
                expandButton.Click(true);

                var count = existingValues.Count;
                fieldContainer.WaitUntil(x => x.FindElements(xpathDeleteExistingValues).Count > count);
            }

            fieldContainer.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldLookupSearchButton.Replace("[NAME]", controlName)));

            existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);
            if (existingValues.Count == 0)
                return;

            if (removeAll)
            {
                // Removes all selected items

                while (existingValues.Count > 0)
                {
                    foreach (var v in existingValues)
                        v.Click(true);

                    existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);
                }

                return;
            }

            // Removes an individual item by value or index
            var value = control.Value;
            if (value == null)
                throw new InvalidOperationException($"No value or index has been provided for the LookupItem {controlName}. Please provide an value or an empty string or an index and try again.");

            if (value == string.Empty)
            {
                var index = control.Index;
                if (index >= existingValues.Count)
                    throw new InvalidOperationException($"Field '{controlName}' does not contain {index + 1} records. Please provide an index value less than {existingValues.Count}");

                existingValues[index].Click(true);
                return;
            }

            var existingValue = existingValues.FirstOrDefault(v => v.GetAttribute("aria-label").EndsWith(value));
            if (existingValue == null)
                throw new InvalidOperationException($"Field '{controlName}' does not contain a record with the name:  {value}");

            existingValue.Click(true);
            driver.WaitForTransaction();
        }
        public static DateTime? TryGetValue(IWebDriver driver, ISearchContext container, DateTimeControl control)
        {
            string field = control.Name;
            driver.WaitForTransaction();

            var xpathToDateField = By.XPath(EntityReference.FieldControlDateTimeInputUCI.Replace("[FIELD]", field));

            var dateField = container.WaitUntilAvailable(xpathToDateField, $"Field: {field} Does not exist");
            string strDate = dateField.GetAttribute("value");
            if (strDate.IsEmptyValue())
                return null;

            var date = DateTime.Parse(strDate);

            // Try get Time
            var timeFieldXPath = By.XPath(EntityReference.FieldControlDateTimeTimeInputUCI.Replace("[FIELD]", field));
            bool success = container.TryFindElement(timeFieldXPath, out var timeField);
            if (!success || timeField == null)
                return date;

            string strTime = timeField.GetAttribute("value");
            if (strTime.IsEmptyValue())
                return date;

            var time = DateTime.Parse(strTime);

            var result = date.AddHours(time.Hour).AddMinutes(time.Minute).AddSeconds(time.Second);

            return result;
        }
        private static void TrySetTime(IWebDriver driver, IWebElement timeField, string time)
        {
            // click & wait until the time get updated after change/clear the date
            timeField.Click();
            driver.WaitForTransaction();

            driver.RepeatUntil(() =>
            {
                timeField.Clear();
                timeField.Click();
                timeField.SendKeys(time);
                timeField.SendKeys(Keys.Tab);
                driver.WaitForTransaction();
            },
            d => timeField.GetAttribute("value").IsValueEqualsTo(time),
            TimeSpan.FromSeconds(9), 3,
            failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {time}. Actual: {timeField.GetAttribute("value")}")
            );
        }
        public void TrySetValue(IWebDriver driver, IWebElement fieldContainer, LookupItem control)
        {
            IWebElement input;
            bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);
            string value = control.Value?.Trim();
            if (found)
                this.SetInputValue(driver, input, value);

            TrySetValue(driver, control);
        }

        public void TryToSetValue(IWebDriver driver, ISearchContext fieldContainer, LookupItem[] controls)
        {
            IWebElement input;
            bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);

            foreach (var control in controls)
            {
                var value = control.Value?.Trim();
                if (found)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        input.Click();
                    else
                    {
                        input.SendKeys(value, true);
                        driver.WaitForTransaction();
                        this.ThinkTime(3.Seconds());
                        input.SendKeys(Keys.Tab);
                        input.SendKeys(Keys.Enter);
                    }
                }

                TrySetValue(fieldContainer, control);
            }

            input.SendKeys(Keys.Escape); // IE wants to keep the flyout open on multi-value fields, this makes sure it closes
        }

        public void TrySetValue(ISearchContext container, LookupItem control)
        {
            string value = control.Value;
            if (value == null)
                control.Value = string.Empty;
            // throw new InvalidOperationException($"No value has been provided for the LookupItem {control.Name}. Please provide a value or an empty string and try again.");

            if (control.Value == string.Empty)
                SetLookupByIndex(container, control);
            else
                SetLookUpByValue(container, control);
        }
        private void SetLookUpByValue(ISearchContext container, LookupItem control)
        {
            var controlName = control.Name;
            var xpathToText = EntityReference.LookupFieldNoRecordsText.Replace("[NAME]", controlName);
            var xpathToResultList = EntityReference.LookupFieldResultList.Replace("[NAME]", controlName);
            var bypathResultList = By.XPath(xpathToText + "|" + xpathToResultList);

            container.WaitUntilAvailable(bypathResultList, TimeSpan.FromSeconds(10));

            var byPathToFlyout = By.XPath(EntityReference.TextFieldLookupMenu.Replace("[NAME]", controlName));
            var flyoutDialog = container.WaitUntilClickable(byPathToFlyout);

            var items = GetListItems(flyoutDialog, control);

            if (items.Count == 0)
                throw new InvalidOperationException($"List does not contain a record with the name:  {control.Value}");

            int index = control.Index;
            if (index >= items.Count)
                throw new InvalidOperationException($"List does not contain {index + 1} records. Please provide an index value less than {items.Count} ");

            var selectedItem = items.ElementAt(index);
            selectedItem.Click(true);
        }
        private void SetLookupByIndex(ISearchContext container, LookupItem control)
        {
            var controlName = control.Name;
            var xpathToControl = By.XPath(EntityReference.LookupResultsDropdown.Replace("[NAME]", controlName));
            var lookupResultsDialog = container.WaitUntilVisible(xpathToControl);

            var xpathFieldResultListItem = By.XPath(EntityReference.LookupFieldResultListItem.Replace("[NAME]", controlName));
            container.WaitUntil(d => d.FindElements(xpathFieldResultListItem).Count > 0);


            var items = GetListItems(lookupResultsDialog, control);
            if (items.Count == 0)
                throw new InvalidOperationException($"No results exist in the Recently Viewed flyout menu. Please provide a text value for {controlName}");

            int index = control.Index;
            if (index >= items.Count)
                throw new InvalidOperationException($"Recently Viewed list does not contain {index} records. Please provide an index value less than {items.Count}");

            var selectedItem = items.ElementAt(index);
            selectedItem.Click(true);
        }

        /// <summary>
        /// Sets the value of a Lookup, Customer, Owner or ActivityParty Lookup which accepts only a single value.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        /// The default index position is 0, which will be the first result record in the lookup results window. Suppy a value > 0 to select a different record if multiple are present.
        internal BrowserCommandResult<bool> SetValue(LookupItem control, FormContextType formContextType)
        {
            return this.Execute(this.GetOptions($"Set Lookup Value: {control.Name}"), driver =>
            {
                driver.WaitForTransaction();

                IWebElement fieldContainer = null;
                fieldContainer = ValidateFormContext(driver, formContextType, control.Name, fieldContainer);

                TryRemoveLookupValue(driver, fieldContainer, control);
                TrySetValue(driver, fieldContainer, control);

                return true;
            });
        }
        /// <summary>
        /// Sets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new Lookup[] { Name = "to", Value = "Rene Valdes (sample)" }, { Name = "to", Value = "Alpine Ski House (sample)" } );</example>
        /// The default index position is 0, which will be the first result record in the lookup results window. Suppy a value > 0 to select a different record if multiple are present.
        internal BrowserCommandResult<bool> SetValue(LookupItem[] controls, FormContextType formContextType = FormContextType.Entity, bool clearFirst = true)
        {
            var control = controls.First();
            var controlName = control.Name;
            return this.Execute(this.GetOptions($"Set ActivityParty Lookup Value: {controlName}"), driver =>
            {
                driver.WaitForTransaction();

                IWebElement fieldContainer = null;
                fieldContainer = ValidateFormContext(driver, formContextType, controlName, fieldContainer);

                if (clearFirst)
                    TryRemoveLookupValue(driver, fieldContainer, control);

                TryToSetValue(driver, fieldContainer, controls);

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a picklist or status field.
        /// </summary>
        /// <param name="control">The option you want to set.</param>
        /// <example>xrmApp.Entity.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public BrowserCommandResult<bool> SetValue(OptionSet control, FormContextType formContextType)
        {
            var controlName = control.Name;
            return this.Execute(this.GetOptions($"Set OptionSet Value: {controlName}"), driver =>
            {
                IWebElement fieldContainer = null;
                fieldContainer = ValidateFormContext(driver, formContextType, controlName, fieldContainer);

                TrySetValue(fieldContainer, control);
                driver.WaitForTransaction();
                return true;
            });
        }
        internal BrowserCommandResult<bool> ClearValue(string fieldName, FormContextType formContextType)
        {
            return this.Execute(this.GetOptions($"Clear Field {fieldName}"), driver =>
            {
                SetValue(fieldName, string.Empty, formContextType);

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        /// <example>xrmApp.Entity.SetValue(new BooleanItem { Name = "donotemail", Value = true });</example>
        public BrowserCommandResult<bool> SetValue(BooleanItem option, FormContextType formContextType)
        {
            return this.Execute(this.GetOptions($"Set BooleanItem Value: {option.Name}"), driver =>
            {
                // ensure that the option.Name value is lowercase -- will cause XPath lookup issues
                option.Name = option.Name.ToLowerInvariant();

                IWebElement fieldContainer = null;
                fieldContainer = ValidateFormContext(driver, formContextType, option.Name, fieldContainer);

                var hasRadio = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name)));
                var hasCheckbox = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name)));
                var hasList = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name)));
                var hasToggle = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name)));
                var hasFlipSwitch = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldFlipSwitchLink.Replace("[NAME]", option.Name)));

                // Need to validate whether control is FlipSwitch or Button
                IWebElement flipSwitchContainer = null;
                var flipSwitch = hasFlipSwitch ? fieldContainer.TryFindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldFlipSwitchContainer.Replace("[NAME]", option.Name)), out flipSwitchContainer) : false;
                var hasButton = flipSwitchContainer != null ? flipSwitchContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldButtonTrue)) : false;
                hasFlipSwitch = hasButton ? false : hasFlipSwitch; //flipSwitch and button have the same container reference, so if it has a button it is not a flipSwitch
                hasFlipSwitch = hasToggle ? false : hasFlipSwitch; //flipSwitch and Toggle have the same container reference, so if it has a Toggle it is not a flipSwitch

                if (hasRadio)
                {
                    var trueRadio = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldRadioTrue.Replace("[NAME]", option.Name)));
                    var falseRadio = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldRadioFalse.Replace("[NAME]", option.Name)));

                    if (option.Value && bool.Parse(falseRadio.GetAttribute("aria-checked")) || !option.Value && bool.Parse(trueRadio.GetAttribute("aria-checked")))
                    {
                        driver.ClickWhenAvailable(By.XPath(Entity.EntityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name)));
                    }
                }
                else if (hasCheckbox)
                {
                    var checkbox = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name)));

                    if (option.Value && !checkbox.Selected || !option.Value && checkbox.Selected)
                    {
                        driver.ClickWhenAvailable(By.XPath(Entity.EntityReference.EntityBooleanFieldCheckboxContainer.Replace("[NAME]", option.Name)));
                    }
                }
                else if (hasList)
                {
                    var list = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name)));
                    var options = list.FindElements(By.TagName("option"));
                    var selectedOption = options.FirstOrDefault(a => a.HasAttribute("data-selected") && bool.Parse(a.GetAttribute("data-selected")));
                    var unselectedOption = options.FirstOrDefault(a => !a.HasAttribute("data-selected"));

                    var trueOptionSelected = false;
                    if (selectedOption != null)
                    {
                        trueOptionSelected = selectedOption.GetAttribute("value") == "1";
                    }

                    if (option.Value && !trueOptionSelected || !option.Value && trueOptionSelected)
                    {
                        if (unselectedOption != null)
                        {
                            driver.ClickWhenAvailable(By.Id(unselectedOption.GetAttribute("id")));
                        }
                    }
                }
                else if (hasToggle)
                {
                    var toggle = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name)));
                    var link = toggle.FindElement(By.TagName("button"));
                    var value = bool.Parse(link.GetAttribute("aria-checked"));

                    if (value != option.Value)
                    {
                        link.Click();
                    }
                }
                else if (hasFlipSwitch)
                {
                    // flipSwitchContainer should exist based on earlier TryFindElement logic
                    var link = flipSwitchContainer.FindElement(By.TagName("a"));
                    var value = bool.Parse(link.GetAttribute("aria-checked"));

                    if (value != option.Value)
                    {
                        link.Click();
                    }
                }
                else if (hasButton)
                {
                    var container = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldButtonContainer.Replace("[NAME]", option.Name)));
                    var trueButton = container.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldButtonTrue));
                    var falseButton = container.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldButtonFalse));

                    if (option.Value)
                    {
                        trueButton.Click();
                    }
                    else
                    {
                        falseButton.Click();
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");


                return true;
            });
        }

        // Used by SetValue methods to determine the field context
        private IWebElement ValidateFormContext(IWebDriver driver, FormContextType formContextType, string field, IWebElement fieldContainer)
        {
            if (formContextType == FormContextType.QuickCreate)
            {
                // Initialize the quick create form context
                // If this is not done -- element input will go to the main form due to new flyout design
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.QuickCreateFormContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.Entity)
            {
                // Initialize the entity form context
                var formContext = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormContext));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.BusinessProcessFlow)
            {
                // Initialize the Business Process Flow context
                var formContext = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFormContext));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.TextFieldContainer.Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.Header)
            {
                // Initialize the Header context
                var formContext = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.HeaderContext));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.Dialog)
            {
                // Initialize the Dialog context
                driver.WaitForTransaction();
                var formContext = driver
                    .FindElements(By.XPath(Dialogs.DialogsReference.DialogContext))
                    .LastOrDefault() ?? throw new NotFoundException("Unable to find a dialog.");
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", field)));
            }

            return fieldContainer;
        }

        internal BrowserCommandResult<bool> AddValues(LookupItem[] controls)
        {
            return this.Execute(this.GetOptions($"Add values {controls.First().Name}"), driver =>
            {
                this.SetValue(controls, FormContextType.Entity, false);

                return true;
            });
        }

        internal BrowserCommandResult<bool> RemoveValues(LookupItem[] controls)
        {
            return this.Execute(this.GetOptions($"Remove values {controls.First().Name}"), driver =>
            {
                foreach (var control in controls)
                    ClearValue(control, FormContextType.Entity, false);

                return true;
            });
        }

        internal BrowserCommandResult<bool> ClearValue(OptionSet control, FormContextType formContextType)
        {
            return this.Execute(this.GetOptions($"Clear Field {control.Name}"), driver =>
            {
                control.Value = "-1";
                SetValue(control, formContextType);

                return true;
            });
        }

        internal BrowserCommandResult<bool> ClearValue(MultiValueOptionSet control, FormContextType formContextType)
        {
            return this.Execute(this.GetOptions($"Clear Field {control.Name}"), driver =>
            {
                this.RemoveMultiOptions(control, formContextType);

                return true;
            });
        }

        internal BrowserCommandResult<bool> ClearValue(DateTimeControl control, FormContextType formContextType)
    => this.Execute(this.GetOptions($"Clear Field: {control.Name}"),
        driver => TrySetValue(driver, container: driver, control: new DateTimeControl(control.Name), formContextType)); // Pass an empty control


        internal BrowserCommandResult<bool> ClearValue(LookupItem control, FormContextType formContextType, bool removeAll = true)
        {
            var controlName = control.Name;
            return this.Execute(this.GetOptions($"Clear Field {controlName}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldLookupFieldContainer.Replace("[NAME]", controlName)));
                TryRemoveLookupValue(driver, fieldContainer, control, removeAll);
                return true;
            });
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        /// <returns>True on success</returns>
        internal BrowserCommandResult<bool> SetValue(MultiValueOptionSet option, FormContextType formContextType = FormContextType.Entity, bool removeExistingValues = false)
        {
            return this.Execute(this.GetOptions($"Set MultiValueOptionSet Value: {option.Name}"), driver =>
            {
                if (removeExistingValues)
                {
                    RemoveMultiOptions(option, formContextType);
                }


                AddMultiOptions(option, formContextType);

                return true;
            });
        }

        /// <summary>
        /// Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be removed</param>
        /// <returns></returns>
        private BrowserCommandResult<bool> RemoveMultiOptions(MultiValueOptionSet option, FormContextType formContextType)
        {
            return this.Execute(this.GetOptions($"Remove Multi Select Value: {option.Name}"), driver =>
            {
                IWebElement fieldContainer = null;

                if (formContextType == FormContextType.QuickCreate)
                {
                    // Initialize the quick create form context
                    // If this is not done -- element input will go to the main form due to new flyout design
                    var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.QuickCreateFormContext]));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.Entity)
                {
                    // Initialize the entity form context
                    var formContext = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.BusinessProcessFlow)
                {
                    // Initialize the Business Process Flow context
                    var formContext = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFormContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.Header)
                {
                    // Initialize the Header context
                    var formContext = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.HeaderContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.Dialog)
                {
                    // Initialize the Header context
                    var formContext = driver.WaitUntilAvailable(By.XPath(Dialogs.DialogsReference.DialogContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name)));
                }

                fieldContainer.Hover(driver, true);

                var selectedRecordXPath = By.XPath(AppElements.Xpath[AppReference.MultiSelect.SelectedRecord]);
                //change to .//li
                var selectedRecords = fieldContainer.FindElements(selectedRecordXPath);

                var initialCountOfSelectedOptions = selectedRecords.Count;
                var deleteButtonXpath = By.XPath(AppElements.Xpath[AppReference.MultiSelect.SelectedOptionDeleteButton]);
                //button[contains(@data-id, 'delete')]
                for (int i = 0; i < initialCountOfSelectedOptions; i++)
                {
                    // With every click of the button, the underlying DOM changes and the
                    // entire collection becomes stale, hence we only click the first occurance of
                    // the button and loop back to again find the elements and anyother occurance
                    selectedRecords[0].FindElement(deleteButtonXpath).Click(true);
                    driver.WaitForTransaction();
                    selectedRecords = fieldContainer.FindElements(selectedRecordXPath);
                }

                return true;
            });
        }

        /// <summary>
        /// Sets the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set</param>
        /// <returns></returns>
        private BrowserCommandResult<bool> AddMultiOptions(MultiValueOptionSet option, FormContextType formContextType)
        {
            return this.Execute(this.GetOptions($"Add Multi Select Value: {option.Name}"), driver =>
            {
                IWebElement fieldContainer = null;

                if (formContextType == FormContextType.QuickCreate)
                {
                    // Initialize the quick create form context
                    // If this is not done -- element input will go to the main form due to new flyout design
                    var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.QuickCreateFormContext]));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.Entity)
                {
                    // Initialize the entity form context
                    var formContext = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.BusinessProcessFlow)
                {
                    // Initialize the Business Process Flow context
                    var formContext = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFormContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.Header)
                {
                    // Initialize the Header context
                    var formContext = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.HeaderContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.Dialog)
                {
                    // Initialize the Header context
                    var formContext = driver.WaitUntilAvailable(By.XPath(Dialogs.DialogsReference.DialogContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(Dialogs.DialogsReference.DialogContext.Replace("[NAME]", option.Name)));
                }

                var inputXPath = By.XPath(AppElements.Xpath[AppReference.MultiSelect.InputSearch]);
                fieldContainer.FindElement(inputXPath).SendKeys(string.Empty);

                var flyoutCaretXPath = By.XPath(AppElements.Xpath[AppReference.MultiSelect.FlyoutCaret]);
                fieldContainer.FindElement(flyoutCaretXPath).Click();

                foreach (var optionValue in option.Values)
                {
                    var flyoutOptionXPath = By.XPath(AppElements.Xpath[AppReference.MultiSelect.FlyoutOption].Replace("[NAME]", optionValue));
                    if (fieldContainer.TryFindElement(flyoutOptionXPath, out var flyoutOption))
                    {
                        var ariaSelected = flyoutOption.GetAttribute<string>("aria-selected");
                        var selected = !string.IsNullOrEmpty(ariaSelected) && bool.Parse(ariaSelected);

                        if (!selected)
                        {
                            flyoutOption.Click();
                        }
                    }
                }

                return true;
            });
        }

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.Entity.SetValue("firstname", "Test");</example>
        internal BrowserCommandResult<bool> SetValue(string field, string value, FormContextType formContextType = FormContextType.Entity)
        {
            return this.Execute(this.GetOptions("Set Value"), driver =>
            {
                IWebElement fieldContainer = null;
                fieldContainer = ValidateFormContext(driver, formContextType, field, fieldContainer);

                IWebElement input;
                bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);

                if (!found)
                    found = fieldContainer.TryFindElement(By.TagName("textarea"), out input);

                if (!found)
                    throw new NoSuchElementException($"Field with name {field} does not exist.");

                SetInputValue(driver, input, value);

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="value">DateTime value.</param>
        /// <param name="formatDate">Datetime format matching Short Date formatting personal options.</param>
        /// <param name="formatTime">Datetime format matching Short Time formatting personal options.</param>
        /// <example>xrmApp.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        /// <example>xrmApp.Entity.SetValue("new_actualclosedatetime", DateTime.Now, "MM/dd/yyyy", "hh:mm tt");</example>
        /// <example>xrmApp.Entity.SetValue("estimatedclosedate", DateTime.Now);</example>
        public BrowserCommandResult<bool> SetValue(string field, DateTime value, FormContextType formContext, string formatDate = null, string formatTime = null)
        {
            var control = new DateTimeControl(field)
            {
                Value = value,
                DateFormat = formatDate,
                TimeFormat = formatTime
            };
            return SetValue(control, formContext);
        }

        public BrowserCommandResult<bool> SetValue(DateTimeControl control, FormContextType formContext)
            => this.Execute(this.GetOptions($"Set Date/Time Value: {control.Name}"),
                driver => TrySetValue(driver, container: driver, control: control, formContext));

        public bool TrySetValue(IWebDriver driver, ISearchContext container, DateTimeControl control, FormContextType formContext)
        {
            TrySetDateValue(driver, container, control, formContext);
            TrySetTime(driver, container, control, formContext);

            if (formContext == FormContextType.Header)
            {
                Entity entity = new Entity(this);
                entity.TryCloseHeaderFlyout(driver);
            }

            return true;
        }

        private void TrySetDateValue(IWebDriver driver, ISearchContext container, DateTimeControl control, FormContextType formContextType)
        {
            string controlName = control.Name;
            IWebElement fieldContainer = null;
            var xpathToInput = By.XPath(Entity.EntityReference.FieldControlDateTimeInputUCI.Replace("[FIELD]", controlName));

            if (formContextType == FormContextType.QuickCreate)
            {
                // Initialize the quick create form context
                // If this is not done -- element input will go to the main form due to new flyout design
                var formContext = container.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.QuickCreateFormContext]));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }
            }
            else if (formContextType == FormContextType.Entity)
            {
                // Initialize the entity form context
                var formContext = container.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormContext));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }
            }
            else if (formContextType == FormContextType.BusinessProcessFlow)
            {
                // Initialize the Business Process Flow context
                var formContext = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFormContext));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }
            }
            else if (formContextType == FormContextType.Header)
            {
                // Initialize the Header context
                var formContext = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.HeaderContext));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }
            }
            else if (formContextType == FormContextType.Dialog)
            {
                // Initialize the Dialog context
                var formContext = driver.WaitUntilAvailable(By.XPath(Dialogs.DialogsReference.DialogContext));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }

            }

            TrySetDateValue(driver, fieldContainer, control.DateAsString, formContextType);
        }
        private void ClearFieldValue(IWebElement field)
        {
            if (field.GetAttribute("value").Length > 0)
            {
                field.SendKeys(Keys.Control + "a");
                field.SendKeys(Keys.Backspace);
            }

            this.ThinkTime(500);
        }

        public static void TrySetValue(IWebElement fieldContainer, OptionSet control)
        {
            var value = control.Value;
            bool success = fieldContainer.TryFindElement(By.TagName("select"), out IWebElement select);
            if (success)
            {
                fieldContainer.WaitUntilAvailable(By.TagName("select"));
                var options = select.FindElements(By.TagName("option"));
                SelectOption(options, value);
                return;
            }

            var name = control.Name;
            var hasStatusCombo = fieldContainer.HasElement(By.XPath(EntityReference.EntityOptionsetStatusCombo.Replace("[NAME]", name)));
            if (hasStatusCombo)
            {
                // This is for statuscode (type = status) that should act like an optionset doesn't doesn't follow the same pattern when rendered
                fieldContainer.ClickWhenAvailable(By.XPath(EntityReference.EntityOptionsetStatusComboButton.Replace("[NAME]", name)));

                var listBox = fieldContainer.FindElement(By.XPath(EntityReference.EntityOptionsetStatusComboList.Replace("[NAME]", name)));

                var options = listBox.FindElements(By.TagName("li"));
                SelectOption(options, value);
                return;
            }

            throw new InvalidOperationException($"OptionSet Field: '{name}' does not exist");
        }
        private void TrySetDateValue(IWebDriver driver, IWebElement dateField, string date, FormContextType formContextType)
        {
            var strExpanded = dateField.GetAttribute("aria-expanded");

            if (strExpanded != null)
            {
                bool success = bool.TryParse(strExpanded, out var isCalendarExpanded);
                if (success && isCalendarExpanded)
                    dateField.Click(); // close calendar

                driver.RepeatUntil(() =>
                {
                    ClearFieldValue(dateField);
                    if (date != null)
                    {
                        dateField.SendKeys(date);
                        dateField.SendKeys(Keys.Tab);
                    }
                },
                d => dateField.GetAttribute("value").IsValueEqualsTo(date),
                TimeSpan.FromSeconds(9), 3,
                failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {date}. Actual: {dateField.GetAttribute("value")}")
                );
            }
            else
            {
                driver.RepeatUntil(() =>
                {
                    dateField.Click(true);
                    if (date != null)
                    {
                        dateField = dateField.FindElement(By.TagName("input"));

                        // Only send Keys.Escape to avoid element not interactable exceptions with calendar flyout on forms.
                        // This can cause the Header or BPF flyouts to close unexpectedly
                        if (formContextType == FormContextType.Entity || formContextType == FormContextType.QuickCreate)
                        {
                            dateField.SendKeys(Keys.Escape);
                        }

                        ClearFieldValue(dateField);
                        dateField.SendKeys(date);
                    }
                },
                    d => dateField.GetAttribute("value").IsValueEqualsTo(date),
                    TimeSpan.FromSeconds(9), 3,
                    failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {date}. Actual: {dateField.GetAttribute("value")}")
                );
            }
        }


        private static void TrySetTime(IWebDriver driver, ISearchContext container, DateTimeControl control, FormContextType formContextType)
        {
            By timeFieldXPath = By.XPath(Entity.EntityReference.FieldControlDateTimeTimeInputUCI.Replace("[FIELD]", control.Name));

            IWebElement formContext = null;

            if (formContextType == FormContextType.QuickCreate)
            {
                //IWebDriver formContext;
                // Initialize the quick create form context
                // If this is not done -- element input will go to the main form due to new flyout design
                formContext = container.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.QuickCreateFormContext]), new TimeSpan(0, 0, 1));
            }
            else if (formContextType == FormContextType.Entity)
            {
                // Initialize the entity form context
                formContext = container.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormContext), new TimeSpan(0, 0, 1));
            }
            else if (formContextType == FormContextType.BusinessProcessFlow)
            {
                // Initialize the Business Process Flow context
                formContext = container.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFormContext), new TimeSpan(0, 0, 1));
            }
            else if (formContextType == FormContextType.Header)
            {
                // Initialize the Header context
                formContext = container as IWebElement;
            }
            else if (formContextType == FormContextType.Dialog)
            {
                // Initialize the Header context
                formContext = container.WaitUntilAvailable(By.XPath(Dialogs.DialogsReference.DialogContext), new TimeSpan(0, 0, 1));
            }

            driver.WaitForTransaction();

            if (formContext.TryFindElement(timeFieldXPath, out var timeField))
            {
                TrySetTime(driver, timeField, control.TimeAsString);
            }
        }
        #endregion
        
        #region Grid
        public BrowserCommandResult<string> GetGridControl()
        {

            return Execute(GetOptions($"Get Grid Control"), driver =>
            {
                var gridContainer = driver.FindElement(By.XPath("//div[contains(@data-lp-id,'MscrmControls.Grid')]"));

                return gridContainer.GetAttribute("innerHTML");
            });
        }
        
        public BrowserCommandResult<Dictionary<string, IWebElement>> OpenViewPicker(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return Execute(GetOptions("Open View Picker"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelector]),
                    TimeSpan.FromSeconds(20),
                    "Unable to click the View Picker"
                );

                var viewContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.ViewContainer]));
                var viewItems = viewContainer.FindElements(By.TagName("li"));

                var result = new Dictionary<string, IWebElement>();
                foreach (var viewItem in viewItems)
                {
                    var role = viewItem.GetAttribute("role");

                    if (role != "presentation")
                        continue;

                    //var key = viewItem.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelectorMenuItem])).Text.ToLowerString();
                    var key = viewItem.Text.ToLowerString();
                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    if (!result.ContainsKey(key))
                        result.Add(key, viewItem);
                }
                return result;
            });
        }

        internal BrowserCommandResult<bool> SwitchView(string viewName, string subViewName = null, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return Execute(GetOptions($"Switch View"), driver =>
            {
                var views = OpenViewPicker().Value;
                Thread.Sleep(500);
                var key = viewName.ToLowerString();
                bool success = views.TryGetValue(key, out var view);
                if (!success)
                    throw new InvalidOperationException($"No view with the name '{key}' exists.");

                view.Click(true);

                if (subViewName != null)
                {
                    // TBD
                }

                driver.WaitForTransaction();

                return true;
            });
        }
        private static int ClickGridAndPageDown(IWebDriver driver, IWebElement grid, int lastKnownFloor, Grid.GridType gridType)
        {
            Actions actions = new Actions(driver);
            By rowGroupLocator = null;
            By topRowLocator = null;
            switch (gridType)
            {
                case Grid.GridType.LegacyReadOnlyGrid:
                    rowGroupLocator = By.XPath(AppElements.Xpath[AppReference.Grid.LegacyReadOnlyRows]);
                    topRowLocator = By.XPath(AppElements.Xpath[AppReference.Grid.Rows]);
                    break;
                case Grid.GridType.ReadOnlyGrid:
                    rowGroupLocator = By.XPath(AppElements.Xpath[AppReference.Grid.Rows]);
                    topRowLocator = By.XPath(AppElements.Xpath[AppReference.Grid.Rows]);
                    break;
                case Grid.GridType.PowerAppsGridControl:
                    rowGroupLocator = By.XPath(AppElements.Xpath[AppReference.Grid.Rows]);
                    topRowLocator = By.XPath(AppElements.Xpath[AppReference.Grid.Rows]);
                    break;
                default:
                    break;
            }
            var CurrentRows = driver.FindElements(rowGroupLocator);
            var lastFloor = CurrentRows.Where(x => Convert.ToInt32(x.GetAttribute("row-index")) == lastKnownFloor).First();
            //var topRow = driver.FindElement(topRowLocator);
            var topRow = CurrentRows.First();
            var firstCell = lastFloor.FindElement(By.XPath("//div[@aria-colindex='1']"));
            lastFloor.Click();
            actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Perform();
            return Convert.ToInt32(driver.FindElements(rowGroupLocator).Last().GetAttribute("row-index"));
        }

        internal BrowserCommandResult<bool> OpenRecord(int index, int thinkTime = Constants.DefaultThinkTime, bool checkRecord = false)
        {
            ThinkTime(thinkTime);
            return Execute(GetOptions("Open Grid Record"), driver =>
            {
                var grid = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.PcfContainer]));
                bool lastRow = false;
                IWebElement gridRow = null;
                Grid.GridType gridType = Grid.GridType.PowerAppsGridControl;
                int lastRowInCurrentView = 0;
                while (!lastRow)
                {
                    //determine which grid
                    if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Grid.Rows]))){
                        gridType = Grid.GridType.PowerAppsGridControl;
                        Trace.WriteLine("Found Power Apps Grid.");
                    }
                    else if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Grid.LegacyReadOnlyRows])))
                    {
                        gridType = Grid.GridType.LegacyReadOnlyGrid;
                        Trace.WriteLine("Found Legacy Read Only Grid.");
                    }


                    if (!driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Grid.Row].Replace("[INDEX]", (index).ToString()))))
                    {
                        lastRowInCurrentView = ClickGridAndPageDown(driver, grid, lastRowInCurrentView, gridType);
                    }
                    else
                    {
                        gridRow = driver.FindElement(By.XPath
                        (AppElements.Xpath[AppReference.Grid.Row].Replace("[INDEX]", index.ToString())));
                        lastRow = true;
                    }
                    if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Grid.LastRow])))
                    {
                        lastRow = true;
                    }
                }
                if (gridRow == null) throw new NotFoundException($"Grid Row {index} not found.");
                var xpathToGrid = By.XPath("//div[contains(@data-id,'DataSetHostContainer')]");
                IWebElement control = driver.WaitUntilAvailable(xpathToGrid);

                Func<Actions, Actions> action;
                if (checkRecord)
                    action = e => e.Click();
                else
                    action = e => e.DoubleClick();
                var xpathToCell = By.XPath(AppElements.Xpath[AppReference.Grid.Row].Replace("[INDEX]", index.ToString()));
                control.WaitUntilClickable(xpathToCell,
                    cell =>
                    {
                        var emptyDiv = cell.FindElement(By.TagName("div"));
                        switch (gridType)
                        {
                            case Grid.GridType.LegacyReadOnlyGrid:
                                driver.Perform(action, emptyDiv, null);
                                break;
                            case Grid.GridType.ReadOnlyGrid:
                                driver.Perform(action, emptyDiv, null);
                                break;
                            case Grid.GridType.PowerAppsGridControl:
                                cell.FindElement(By.XPath("//a[contains(@aria-label,'Read only')]")).Click();
                                break;
                            default: throw new InvalidSelectorException("Did not find Read Only or Power Apps Grid.");
                        }
                        Trace.WriteLine("Clicked record.");
                    },
                    $"An error occur trying to open the record at position {index}"
                );

                driver.WaitForTransaction();
                Trace.WriteLine("Click Record transaction complete.");
                return true;
            });
        }

        internal BrowserCommandResult<bool> Search(string searchCriteria, bool clearByDefault = true, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Search"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Grid.QuickFind]));

                if (clearByDefault)
                {
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.QuickFind])).Clear();
                }

                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.QuickFind])).SendKeys(searchCriteria);
                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.QuickFind])).SendKeys(Keys.Enter);

                //driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> ClearSearch(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Clear Search"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Grid.QuickFind]));

                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.QuickFind])).Clear();

                return true;
            });
        }
        private static string GetGridQueryKey(IWebDriver driver, string dataSetName = null)
        {
            Dictionary<string, object> pages = (Dictionary<string, object>)driver.ExecuteScript($"return window[Object.keys(window).find(i => !i.indexOf(\"__store$\"))].getState().pages");
            //This is the current view
            Dictionary<string, object> pageData = (Dictionary<string, object>)pages.Last().Value;
            IList<KeyValuePair<string, object>> datasets = pageData.Where(i => i.Key == "datasets").ToList();
            //Get Entity From Page List
            Dictionary<string, object> entityName = null;
            if (dataSetName != null)
            {
                foreach (KeyValuePair<string, object> dataset in datasets)
                {
                    foreach (KeyValuePair<string, object> datasetList in (Dictionary<string, object>)dataset.Value)
                    {
                        if (datasetList.Key == dataSetName)
                        {
                            entityName = (Dictionary<string, object>)datasetList.Value;
                            return (string)entityName["queryKey"];
                        }
                    }

                }
                throw new Exception("Invalid DataSet Name");
            }
            else
            {
                entityName = (Dictionary<string, object>)datasets[0].Value;
                Dictionary<string, object> entityQueryListList = (Dictionary<string, object>)entityName.First().Value;
                return (string)entityQueryListList["queryKey"];
            }
            if (entityName == null) throw new Exception("Invalid DataSet Name");
        }
        private static void ProcessGridRowAttributes(Dictionary<string, object> attributes, GridItem gridItem)
        {
            foreach (string attributeKey in attributes.Keys)
            {

                var serializedString = JsonConvert.SerializeObject(attributes[attributeKey]);
                var deserializedRecord = JsonConvert.DeserializeObject<SerializedGridItem>(serializedString);
                if (deserializedRecord.value != null)
                {
                    gridItem[attributeKey] = deserializedRecord.value;
                }
                else if (deserializedRecord.label != null)
                {
                    gridItem[attributeKey] = deserializedRecord.label;
                }
                else if (deserializedRecord.id != null)
                {
                    gridItem[attributeKey] = deserializedRecord.id.guid;
                }
                else if (deserializedRecord.reference != null)
                {
                    gridItem[attributeKey] = deserializedRecord.reference.id.guid;
                }
            }
        }
        internal BrowserCommandResult<List<GridItem>> GetGridItems(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Get Grid Items"), driver =>
            {
                var returnList = new List<GridItem>();
                //#1294
                var gridContainer = driver.FindElement(By.XPath("//div[contains(@data-id,'data-set-body-container')]/div"));
                string[] gridDataId = gridContainer.GetAttribute("data-lp-id").Split('|');
                Dictionary<string, object> WindowStateData = (Dictionary<string, object>)driver.ExecuteScript($"return window[Object.keys(window).find(i => !i.indexOf(\"__store$\"))].getState().data");
                string keyForData = GetGridQueryKey(driver, null);
                //Get Data Store

                Dictionary<string, object> WindowStateDataLists = (Dictionary<string, object>)WindowStateData["lists"];

                //Find Data by Key
                Dictionary<string, object> WindowStateDataKeyedForData = (Dictionary<string, object>)WindowStateDataLists[keyForData];
                //Find Record Ids for Key Data Set
                ReadOnlyCollection<object> WindowStateDataKeyedForDataRecordsIds = (ReadOnlyCollection<object>)WindowStateDataKeyedForData["records"];

                //Get Data
                Dictionary<string, object> WindowStateEntityData = (Dictionary<string, object>)WindowStateData["entities"];

                if (!WindowStateEntityData.ContainsKey(gridDataId[2]))
                {
                    return returnList;

                }
                Dictionary<string, object> WindowStateEntityDataEntity = (Dictionary<string, object>)WindowStateEntityData[gridDataId[2]];
                foreach (Dictionary<string, object> record in WindowStateDataKeyedForDataRecordsIds)
                {
                    Dictionary<string, object> recordId = (Dictionary<string, object>)record["id"];
                    Dictionary<string, object> definedRecord = (Dictionary<string, object>)WindowStateEntityDataEntity[(string)recordId["guid"]];
                    Dictionary<string, object> attributes = (Dictionary<string, object>)definedRecord["fields"];
                    GridItem gridItem = new GridItem()
                    {
                        EntityName = gridDataId[2],
                        Id = new Guid((string)recordId["guid"])
                    };
                    ProcessGridRowAttributes(attributes, gridItem);
                    returnList.Add(gridItem);
                }
                return returnList;
            });
        }

        internal BrowserCommandResult<bool> NextPage(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Next Page"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.NextPage]));

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> PreviousPage(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Previous Page"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.PreviousPage]));

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> FirstPage(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"First Page"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.FirstPage]));

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectAll(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Select All"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.SelectAll]));

                driver.WaitForTransaction();

                return true;
            });
        }

        public BrowserCommandResult<bool> ShowChart(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Show Chart"), driver =>
            {
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Grid.ShowChart])))
                {
                    driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.ShowChart]));

                    driver.WaitForTransaction();
                }
                else
                {
                    throw new Exception("The Show Chart button does not exist.");
                }

                return true;
            });
        }

        public BrowserCommandResult<bool> HideChart(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Hide Chart"), driver =>
            {
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Grid.HideChart])))
                {
                    driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.HideChart]));

                    driver.WaitForTransaction();
                }
                else
                {
                    throw new Exception("The Hide Chart button does not exist.");
                }

                return true;
            });
        }

        public BrowserCommandResult<bool> FilterByLetter(char filter, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            if (!Char.IsLetter(filter) && filter != '#')
                throw new InvalidOperationException("Filter criteria is not valid.");

            return this.Execute(GetOptions("Filter by Letter"), driver =>
            {
                var jumpBar = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.JumpBar]));
                var link = jumpBar.FindElement(By.Id(filter + "_link"));

                if (link != null)
                {
                    link.Click();

                    driver.WaitForTransaction();
                }
                else
                {
                    throw new Exception($"Filter with letter: {filter} link does not exist");
                }

                return true;
            });
        }

        public BrowserCommandResult<bool> FilterByAll(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Filter by All Records"), driver =>
            {
                var jumpBar = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.JumpBar]));
                var link = jumpBar.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.FilterByAll]));

                if (link != null)
                {
                    link.Click();

                    driver.WaitForTransaction();
                }
                else
                {
                    throw new Exception($"Filter by All link does not exist");
                }

                return true;
            });
        }

        public BrowserCommandResult<bool> SelectRecord(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Select Grid Record"), driver =>
            {
                var container = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.RowsContainer]), "Grid Container does not exist.");

                var row = container.FindElement(By.Id("id-cell-" + index + "-1"));
                if (row == null)
                    throw new Exception($"Row with index: {index} does not exist.");

                row.Click();
                return true;
            });
        }

        public BrowserCommandResult<bool> SwitchChart(string chartName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            if (!Browser.Driver.IsVisible(By.XPath(AppElements.Xpath[AppReference.Grid.ChartSelector])))
                ShowChart();

            ThinkTime(1000);

            return this.Execute(GetOptions("Switch Chart"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.ChartSelector]));

                var list = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.ChartViewList]));

                driver.ClickWhenAvailable(By.XPath("//li[contains(@title,'" + chartName + "')]"));

                return true;
            });
        }

        public BrowserCommandResult<bool> Sort(string columnName, string sortOptionButtonText, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Sort by {columnName}"), driver =>
            {
                var sortCol = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.GridSortColumn].Replace("[COLNAME]", columnName)));

                if (sortCol == null)
                    throw new InvalidOperationException($"Column: {columnName} Does not exist");
                else
                {
                    sortCol.Click(true);
                    driver.WaitUntilClickable(By.XPath($@"//button[@name='{sortOptionButtonText}']")).Click(true);
                }

                driver.WaitForTransaction();
                return true;
            });
        }

        #endregion

        #region RelatedGrid

        /// <summary>
        /// Opens the grid record.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> OpenRelatedGridRow(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Grid Item"), driver =>
            {
                var grid = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));
                var rows = grid
                    .FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.CellContainer]))
                    .FindElements(By.XPath(AppElements.Xpath[AppReference.Grid.Rows]));

                if (rows.Count <= 0)
                {
                    return true;
                }
                else if (index + 1 > rows.Count)
                {
                    throw new IndexOutOfRangeException($"Grid record count: {rows.Count}. Expected: {index + 1}");
                }

                var row = rows.ElementAt(index + 1);
                var cell = row.FindElements(By.XPath(Entity.EntityReference.SubGridCells)).ElementAt(1);

                new Actions(driver).DoubleClick(cell).Perform();
                driver.WaitForTransaction();

                return true;
            });
        }

        public BrowserCommandResult<bool> ClickRelatedCommand(string name, string subName = null, string subSecondName = null)
        {
            return this.Execute(GetOptions("Click Related Tab Command"), driver =>
            {
                // Locate Related Command Bar Button List
                var relatedCommandBarButtonList = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarButtonList]));

                // Validate list has provided command bar button
                if (relatedCommandBarButtonList.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarButton].Replace("[NAME]", name))))
                {
                    relatedCommandBarButtonList.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarButton].Replace("[NAME]", name))).Click(true);

                    driver.WaitForTransaction();

                    if (subName != null)
                    {
                        //Look for Overflow flyout
                        if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowContainer])))
                        {
                            var overFlowContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowContainer]));

                            if (!overFlowContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))))
                                throw new NotFoundException($"{subName} button not found");

                            overFlowContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))).Click(true);

                            driver.WaitForTransaction();
                        }

                        if (subSecondName != null)
                        {
                            //Look for Overflow flyout
                            if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowContainer])))
                            {
                                var overFlowContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowContainer]));

                                if (!overFlowContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))))
                                    throw new NotFoundException($"{subName} button not found");

                                overFlowContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))).Click(true);

                                driver.WaitForTransaction();
                            }
                        }
                    }

                    return true;
                }
                else
                {
                    // Button was not found, check if we should be looking under More Commands (OverflowButton)
                    var moreCommands = relatedCommandBarButtonList.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowButton]));

                    if (moreCommands)
                    {
                        var overFlowButton = relatedCommandBarButtonList.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowButton]));
                        overFlowButton.Click(true);

                        if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowContainer]))) //Look for Overflow
                        {
                            var overFlowContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowContainer]));

                            if (overFlowContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarButton].Replace("[NAME]", name))))
                            {
                                overFlowContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarButton].Replace("[NAME]", name))).Click(true);

                                driver.WaitForTransaction();

                                if (subName != null)
                                {
                                    overFlowContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowContainer]));

                                    if (!overFlowContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))))
                                        throw new NotFoundException($"{subName} button not found");

                                    overFlowContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))).Click(true);

                                    driver.WaitForTransaction();

                                    if (subSecondName != null)
                                    {
                                        overFlowContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowContainer]));

                                        if (!overFlowContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))))
                                            throw new NotFoundException($"{subName} button not found");

                                        overFlowContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))).Click(true);

                                        driver.WaitForTransaction();
                                    }
                                }

                                return true;
                            }
                        }
                        else
                        {
                            throw new NotFoundException($"{name} button not found in the More Commands container. Button names are case sensitive. Please check for proper casing of button name.");
                        }

                    }
                    else
                    {
                        throw new NotFoundException($"{name} button not found. Button names are case sensitive. Please check for proper casing of button name.");
                    }
                }

                return true;
            });
        }

        #endregion

        #region Subgrid
        public BrowserCommandResult<string> GetSubGridControl(string subGridName)
        {

            return Execute(GetOptions($"Get Sub Grid Control"), driver =>
            {
                var subGrid = driver.FindElement(By.XPath(Entity.EntityReference.SubGridContents.Replace("[NAME]", subGridName)));

                return subGrid.GetAttribute("innerHTML");
            });
        }
        internal BrowserCommandResult<bool> SwitchSubGridView(string subGridName, string viewName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return Execute(GetOptions($"Switch SubGrid View"), driver =>
            {
                // Initialize required variables
                IWebElement viewPicker = null;

                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(Entity.EntityReference.SubGridContents.Replace("[NAME]", subGridName)));

                var foundPicker = subGrid.TryFindElement(By.XPath(Entity.EntityReference.SubGridViewPickerButton), out viewPicker);

                if (foundPicker)
                {
                    viewPicker.Click(true);

                    // Locate the ViewSelector flyout
                    var viewPickerFlyout = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.SubGridViewPickerFlyout), new TimeSpan(0, 0, 2));

                    var viewItems = viewPickerFlyout.FindElements(By.TagName("li"));


                    //Is the button in the ribbon?
                    if (viewItems.Any(x => x.GetAttribute("aria-label").Equals(viewName, StringComparison.OrdinalIgnoreCase)))
                    {
                        viewItems.FirstOrDefault(x => x.GetAttribute("aria-label").Equals(viewName, StringComparison.OrdinalIgnoreCase)).Click(true);
                    }

                }
                else
                    throw new NotFoundException($"Unable to locate the viewPicker for SubGrid {subGridName}");

                driver.WaitForTransaction();

                return true;
            });
        }
        /// This method is obsolete. Do not use.
        public BrowserCommandResult<bool> ClickSubgridAddButton(string subgridName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Click add button of subgrid: {subgridName}"), driver =>
            {
                driver.FindElement(By.XPath(Entity.EntityReference.SubGridAddButton.Replace("[NAME]", subgridName)))?.Click();

                return true;
            });
        }

        public BrowserCommandResult<bool> ClickSubGridCommand(string subGridName, string name, string subName = null, string subSecondName = null)
        {
            return this.Execute(GetOptions("Click SubGrid Command"), driver =>
            {
                // Initialize required local variables
                IWebElement subGridCommandBar = null;

                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(Entity.EntityReference.SubGridContents.Replace("[NAME]", subGridName)));

                if (subGrid == null)
                    throw new NotFoundException($"Unable to locate subgrid contents for {subGridName} subgrid.");

                // Check if grid commandBar was found
                if (subGrid.TryFindElement(By.XPath(Entity.EntityReference.SubGridCommandBar.Replace("[NAME]", subGridName)), out subGridCommandBar))
                {
                    //Is the button in the ribbon?
                    if (subGridCommandBar.TryFindElement(By.XPath(Entity.EntityReference.SubGridCommandLabel.Replace("[NAME]", name)), out var command))
                    {
                        command.Click(true);
                        driver.WaitForTransaction();
                    }
                    else
                    {
                        // Is the button in More Commands overflow?
                        if (subGridCommandBar.TryFindElement(By.XPath(Entity.EntityReference.SubGridOverflowButton.Replace("[NAME]", "More commands")), out var moreCommands))
                        {
                            // Click More Commands
                            moreCommands.Click(true);
                            driver.WaitForTransaction();

                            // Locate the overflow button (More Commands flyout)
                            var overflowContainer = driver.FindElement(By.XPath(Entity.EntityReference.SubGridOverflowContainer));

                            //Click the primary button, if found
                            if (overflowContainer.TryFindElement(By.XPath(Entity.EntityReference.SubGridOverflowButton.Replace("[NAME]", name)), out var overflowCommand))
                            {
                                overflowCommand.Click(true);
                                driver.WaitForTransaction();
                            }
                            else
                                throw new InvalidOperationException($"No command with the name '{name}' exists inside of {subGridName} Commandbar.");
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{name}' exists inside of {subGridName} CommandBar.");
                    }

                    if (subName != null)
                    {
                        // Locate the sub-button flyout if subName present
                        var overflowContainer = driver.FindElement(By.XPath(Entity.EntityReference.SubGridOverflowContainer));

                        //Click the primary button, if found
                        if (overflowContainer.TryFindElement(By.XPath(Entity.EntityReference.SubGridOverflowButton.Replace("[NAME]", subName)), out var overflowButton))
                        {
                            overflowButton.Click(true);
                            driver.WaitForTransaction();
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{subName}' exists under the {name} command inside of {subGridName} Commandbar.");

                        // Check if we need to go to a 3rd level
                        if (subSecondName != null)
                        {
                            // Locate the sub-button flyout if subSecondName present
                            overflowContainer = driver.FindElement(By.XPath(Entity.EntityReference.SubGridOverflowContainer));

                            //Click the primary button, if found
                            if (overflowContainer.TryFindElement(By.XPath(Entity.EntityReference.SubGridOverflowButton.Replace("[NAME]", subSecondName)), out var secondOverflowCommand))
                            {
                                secondOverflowCommand.Click(true);
                                driver.WaitForTransaction();
                            }
                            else
                                throw new InvalidOperationException($"No command with the name '{subSecondName}' exists under the {subName} command inside of {name} on the {subGridName} SubGrid Commandbar.");
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate the Commandbar for the {subGrid} SubGrid.");

                return true;
            });
        }

        internal BrowserCommandResult<bool> ClickSubgridSelectAll(string subGridName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Click Select All Button on subgrid: {subGridName}"), driver =>
            {
                // Find the SubGrid
                var subGrid = driver.WaitUntilAvailable(
                    By.XPath(Entity.EntityReference.SubGridContents.Replace("[NAME]", subGridName)),
                    5.Seconds(),
                    $"Unable to find subgrid named {subGridName}.");

                subGrid.ClickWhenAvailable(By.XPath("//div[@role='columnheader']//span[@role='checkbox']"));
                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SearchSubGrid(string subGridName, string searchCriteria, bool clearByDefault = false)
        {
            return this.Execute(GetOptions($"Search SubGrid {subGridName}"), driver =>
            {
                IWebElement subGridSearchField = null;
                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(Entity.EntityReference.SubGridContents.Replace("[NAME]", subGridName)));
                if (subGrid != null)
                {
                    var foundSearchField = subGrid.TryFindElement(By.XPath(Entity.EntityReference.SubGridSearchBox), out subGridSearchField);
                    if (foundSearchField)
                    {
                        var inputElement = subGridSearchField.FindElement(By.TagName("input"));

                        if (clearByDefault)
                        {
                            inputElement.Clear();
                        }

                        inputElement.SendKeys(searchCriteria);

                        var startSearch = subGridSearchField.FindElement(By.TagName("button"));
                        startSearch.Click(true);

                        driver.WaitForTransaction();
                    }
                    else
                        throw new NotFoundException($"Unable to locate the search box for subgrid {subGridName}. Please validate that view search is enabled for this subgrid");
                }
                else
                    throw new NotFoundException($"Unable to locate subgrid with name {subGridName}");

                return true;
            });
        }
        internal BrowserCommandResult<List<GridItem>> GetSubGridItems(string subgridName)
        {
            return this.Execute(GetOptions($"Get Subgrid Items for Subgrid {subgridName}"), driver =>
            {
                // Initialize return object
                List<GridItem> subGridRows = new List<GridItem>();

                // Initialize required local variables
                IWebElement subGridRecordList = null;
                List<string> columns = new List<string>();
                List<string> cellValues = new List<string>();
                GridItem item = new GridItem();
                Dictionary<string, object> WindowStateData = (Dictionary<string, object>)driver.ExecuteScript($"return JSON.parse(JSON.stringify(window[Object.keys(window).find(i => !i.indexOf(\"__store$\"))].getState().data))");
                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(Entity.EntityReference.SubGridContents.Replace("[NAME]", subgridName)));

                if (subGrid == null)
                    throw new NotFoundException($"Unable to locate subgrid contents for {subgridName} subgrid.");
                // Check if ReadOnlyGrid was found
                if (subGrid.TryFindElement(By.XPath(Entity.EntityReference.SubGridListCells.Replace("[NAME]", subgridName)), out subGridRecordList))
                {

                    // Locate record list
                    var foundRecords = subGrid.TryFindElement(By.XPath(Entity.EntityReference.SubGridListCells.Replace("[NAME]", subgridName)), out subGridRecordList);

                    if (foundRecords)
                    {
                        var subGridRecordRows = subGrid.FindElements(By.XPath(Entity.EntityReference.SubGridRows.Replace("[NAME]", subgridName)));
                        var SubGridContainer = driver.FindElement(By.XPath(Entity.EntityReference.SubGridContents.Replace("[NAME]", subgridName)));
                        string[] gridDataId = SubGridContainer.FindElement(By.XPath($"//div[contains(@data-lp-id,'{subgridName}')]")).GetAttribute("data-lp-id").Split('|');
                        //Need to add entity name
                        string keyForData = GetGridQueryKey(driver, gridDataId[2] + ":" + subgridName);

                        Dictionary<string, object> WindowStateDataLists = (Dictionary<string, object>)WindowStateData["lists"];

                        //Find Data by Key
                        Dictionary<string, object> WindowStateDataKeyedForData = (Dictionary<string, object>)WindowStateDataLists[keyForData];
                        //Find Record Ids for Key Data Set
                        ReadOnlyCollection<object> WindowStateDataKeyedForDataRecordsIds = (ReadOnlyCollection<object>)WindowStateDataKeyedForData["records"];

                        //Get Data
                        Dictionary<string, object> WindowStateEntityData = (Dictionary<string, object>)WindowStateData["entities"];
                        Dictionary<string, object> WindowStateEntityDataEntity = (Dictionary<string, object>)WindowStateEntityData[gridDataId[2]];
                        foreach (Dictionary<string, object> record in WindowStateDataKeyedForDataRecordsIds)
                        {
                            Dictionary<string, object> recordId = (Dictionary<string, object>)record["id"];
                            //Dictionary<string, object> definedRecord = (Dictionary<string, object>)WindowStateEntityDataEntity[(string)recordId["guid"]];
                            //Dictionary<string, object> attributes = (Dictionary<string, object>)definedRecord["fields"];
                            GridItem gridItem = new GridItem()
                            {
                                EntityName = gridDataId[2],
                                Id = new Guid((string)recordId["guid"])
                            };
                            //ProcessGridRowAttributes(attributes, gridItem);
                            subGridRows.Add(gridItem);
                        }


                    }
                    else
                        throw new NotFoundException($"Unable to locate record list for subgrid {subgridName}");

                }
                // Attempt to locate the editable grid list
                else if (subGrid.TryFindElement(By.XPath(Entity.EntityReference.EditableSubGridList.Replace("[NAME]", subgridName)), out subGridRecordList))
                {
                    //Find the columns
                    var headerCells = subGrid.FindElements(By.XPath(Entity.EntityReference.SubGridHeadersEditable));

                    foreach (IWebElement headerCell in headerCells)
                    {
                        var headerTitle = headerCell.GetAttribute("title");
                        columns.Add(headerTitle);
                    }

                    //Find the rows
                    var rows = subGrid.FindElements(By.XPath(Entity.EntityReference.SubGridDataRowsEditable));

                    //Process each row
                    foreach (IWebElement row in rows)
                    {
                        var cells = row.FindElements(By.XPath(Entity.EntityReference.SubGridCells));

                        if (cells.Count > 0)
                        {
                            foreach (IWebElement thisCell in cells)
                                cellValues.Add(thisCell.Text);

                            for (int i = 0; i < columns.Count; i++)
                            {
                                //The first cell is always a checkbox for the record.  Ignore the checkbox.
                                if (i == 0)
                                {
                                    // Do Nothing
                                }
                                else
                                {
                                    item[columns[i]] = cellValues[i];
                                }
                            }

                            subGridRows.Add(item);

                            // Flush Item and Cell Values To Get New Rows
                            cellValues = new List<string>();
                            item = new GridItem();
                        }
                    }

                    return subGridRows;

                }
                // Special 'Related' high density grid control for entity forms
                else if (subGrid.TryFindElement(By.XPath(Entity.EntityReference.SubGridHighDensityList.Replace("[NAME]", subgridName)), out subGridRecordList))
                {
                    //Find the columns
                    var headerCells = subGrid.FindElements(By.XPath(Entity.EntityReference.SubGridHeadersHighDensity));

                    foreach (IWebElement headerCell in headerCells)
                    {
                        var headerTitle = headerCell.GetAttribute("data-id");
                        columns.Add(headerTitle);
                    }

                    //Find the rows
                    var rows = subGrid.FindElements(By.XPath(Entity.EntityReference.SubGridRowsHighDensity));

                    //Process each row
                    foreach (IWebElement row in rows)
                    {
                        //Get the entityId and entity Type
                        if (row.GetAttribute("data-lp-id") != null)
                        {
                            var rowAttributes = row.GetAttribute("data-lp-id").Split('|');
                            item.EntityName = rowAttributes[4];
                            //The row record IDs are not in the DOM. Must be retrieved via JavaScript
                            var getId = $"return Xrm.Page.getControl(\"{subgridName}\").getGrid().getRows().get({rows.IndexOf(row)}).getData().entity.getId()";
                            item.Id = new Guid((string)driver.ExecuteScript(getId));
                        }

                        var cells = row.FindElements(By.XPath(Entity.EntityReference.SubGridCells));

                        if (cells.Count > 0)
                        {
                            foreach (IWebElement thisCell in cells)
                                cellValues.Add(thisCell.Text);

                            for (int i = 0; i < columns.Count; i++)
                            {
                                //The first cell is always a checkbox for the record.  Ignore the checkbox.
                                if (i == 0)
                                {
                                    // Do Nothing
                                }
                                else
                                {
                                    item[columns[i]] = cellValues[i];
                                }

                            }

                            subGridRows.Add(item);

                            // Flush Item and Cell Values To Get New Rows
                            cellValues = new List<string>();
                            item = new GridItem();
                        }
                    }

                    return subGridRows;
                }

                // Return rows object
                return subGridRows;
            });
        }

        private static Actions ClickSubGridAndPageDown(IWebDriver driver, IWebElement grid)
        {
            Actions actions = new Actions(driver);
            //var topRow = driver.FindElement(By.XPath("//div[@data-id='entity_control-pcf_grid_control_container']//div[@ref='centerContainer']//div[@role='rowgroup']//div[@role='row']"));
            var topRow = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Rows]));
            //topRow.Click();
            //actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Perform();

            actions.MoveToElement(topRow.FindElement(By.XPath("//div[@role='listitem']//button"))).Perform();
            actions.KeyDown(OpenQA.Selenium.Keys.Alt).SendKeys(OpenQA.Selenium.Keys.ArrowDown).Build().Perform();
            return actions;

        }
        internal BrowserCommandResult<bool> OpenSubGridRecord(string subgridName, int index = 0)
        {
            return this.Execute(GetOptions($"Open Subgrid record for subgrid {subgridName}"), driver =>
            {
                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(Entity.EntityReference.SubGridContents.Replace("[NAME]", subgridName)));

                // Find list of SubGrid records
                IWebElement subGridRecordList = null;
                var foundGrid = subGrid.TryFindElement(By.XPath(Entity.EntityReference.SubGridListCells.Replace("[NAME]", subgridName)), out subGridRecordList);

                // Read Only Grid Found
                if (subGridRecordList != null && foundGrid)
                {
                    var subGridRecordRows = subGrid.FindElements(By.XPath(Entity.EntityReference.SubGridListCells.Replace("[NAME]", subgridName)));
                    if (subGridRecordRows == null)
                        throw new NoSuchElementException($"No records were found for subgrid {subgridName}");
                    Actions actions = new Actions(driver);
                    actions.MoveToElement(subGrid).Perform();

                    IWebElement gridRow = null;
                    if (index + 1 < subGridRecordRows.Count)
                    {
                        gridRow = subGridRecordRows[index];
                    }
                    else
                    {
                        var grid = driver.FindElement(By.XPath("//div[@ref='eViewport']"));
                        actions.DoubleClick(gridRow).Perform();
                        driver.WaitForTransaction();
                    }
                    if (gridRow == null)
                        throw new IndexOutOfRangeException($"Subgrid {subgridName} record count: {subGridRecordRows.Count}. Expected: {index + 1}");


                    actions.DoubleClick(gridRow).Perform();
                    driver.WaitForTransaction();
                    return true;
                }
                else if (!foundGrid)
                {
                    // Read Only Grid Not Found
                    var foundEditableGrid = subGrid.TryFindElement(By.XPath(Entity.EntityReference.EditableSubGridList.Replace("[NAME]", subgridName)), out subGridRecordList);

                    if (foundEditableGrid)
                    {
                        var editableGridListCells = subGridRecordList.FindElement(By.XPath(Entity.EntityReference.EditableSubGridListCells));

                        var editableGridCellRows = editableGridListCells.FindElements(By.XPath(Entity.EntityReference.EditableSubGridListCellRows));

                        var editableGridCellRow = editableGridCellRows[index + 1].FindElements(By.XPath("./div"));

                        Actions actions = new Actions(driver);
                        actions.DoubleClick(editableGridCellRow[0]).Perform();

                        driver.WaitForTransaction();

                        return true;
                    }
                    else
                    {
                        // Editable Grid Not Found
                        // Check for special 'Related' grid form control
                        // This opens a limited form view in-line on the grid

                        //Get the GridName
                        string subGridName = subGrid.GetAttribute("data-id").Replace("dataSetRoot_", String.Empty);

                        //cell-0 is the checkbox for each record
                        var checkBox = driver.FindElement(By.XPath(Entity.EntityReference.SubGridRecordCheckbox.Replace("[INDEX]", index.ToString()).Replace("[NAME]", subGridName)));

                        driver.DoubleClick(checkBox);

                        driver.WaitForTransaction();
                    }
                }

                return true;

            });
        }

        #endregion



        #region Lookup 

        internal BrowserCommandResult<bool> OpenLookupRecord(int index)
        {
            return this.Execute(GetOptions("Select Lookup Record"), driver =>
            {
                driver.WaitForTransaction();

                ReadOnlyCollection<IWebElement> rows = null;
                if (driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.Container]), out var advancedLookup))
                {
                    // Advanced Lookup
                    rows = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.ResultRows]));
                }
                else
                {
                    // Lookup
                    rows = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Lookup.LookupResultRows]));
                }

                if (rows == null || !rows.Any())
                {
                    throw new NotFoundException("No rows found");
                }

                var row = rows.ElementAt(index);

                if (advancedLookup == null)
                {
                    row.Click();
                }
                else
                {
                    if (!row.GetAttribute<bool?>("aria-selected").GetValueOrDefault())
                    {
                        row.Click();
                    }

                    advancedLookup.FindElement(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.DoneButton])).Click();
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SearchLookupField(LookupItem control, string searchCriteria)
        {
            return this.Execute(GetOptions("Search Lookup Record"), driver =>
            {
                driver.WaitForTransaction();

                if (driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.Container]), out var advancedLookup))
                {
                    // Advanced lookup
                    var search = advancedLookup.FindElement(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.SearchInput]));
                    search.Click();
                    search.SendKeys(Keys.Control + "a");
                    search.SendKeys(Keys.Backspace);
                    search.SendKeys(searchCriteria);

                    driver.WaitForTransaction();

                    OpenLookupRecord(0);
                }
                else
                {
                    // Lookup
                    control.Value = searchCriteria;
                    SetValue(control, FormContextType.Entity);
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupRelatedEntity(string entityName)
        {
            // Click the Related Entity on the Lookup Flyout
            return this.Execute(GetOptions($"Select Lookup Related Entity {entityName}"), driver =>
            {
                driver.WaitForTransaction();

                IWebElement relatedEntity = null;
                if (driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.Container]), out var advancedLookup))
                {
                    // Advanced lookup
                    relatedEntity = advancedLookup.WaitUntilAvailable(
                        By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.FilterTable].Replace("[NAME]", entityName)),
                        2.Seconds());
                }
                else
                {
                    // Lookup 
                    relatedEntity = driver.WaitUntilAvailable(
                        By.XPath(AppElements.Xpath[AppReference.Lookup.RelatedEntityLabel].Replace("[NAME]", entityName)),
                        2.Seconds());
                }

                if (relatedEntity == null)
                {
                    throw new NotFoundException($"Lookup Entity {entityName} not found.");
                }

                relatedEntity.Click();
                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SwitchLookupView(string viewName)
        {
            return Execute(GetOptions($"Select Lookup View {viewName}"), driver =>
            {
                var advancedLookup = driver.WaitUntilAvailable(
                    By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.Container]),
                    2.Seconds());
               
                if (advancedLookup == null)
                {
                    SelectLookupAdvancedLookupButton();
                    advancedLookup = driver.WaitUntilAvailable(
                        By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.Container]),
                        2.Seconds(),
                        "Expected Advanced Lookup dialog but it was not found.");
                }

                advancedLookup
                    .FindElement(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.ViewSelectorCaret]))
                    .Click();
                
                driver
                    .WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.ViewDropdownList]))
                    .ClickWhenAvailable(
                     By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.ViewDropdownListItem].Replace("[NAME]", viewName)),
                     2.Seconds(),
                     $"The '{viewName}' view isn't in the list of available lookup views.");

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupAdvancedLookupButton()
        {
            return this.Execute(GetOptions("Click Advanced Lookup Button"), driver =>
            {
                driver.ClickWhenAvailable(
                    By.XPath(AppElements.Xpath[AppReference.Lookup.AdvancedLookupButton]),
                    10.Seconds(),
                    "The 'Advanced Lookup' button was not found. Ensure a search has been performed in the lookup first.");

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupNewButton()
        {
            return this.Execute(GetOptions("Click New Lookup Button"), driver =>
            {
                driver.WaitForTransaction();

                if (driver.TryFindElement(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.Container]), out var advancedLookup))
                {
                    // Advanced lookup
                    if (advancedLookup.TryFindElement(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.AddNewRecordButton]), out var addNewRecordButton))
                    {
                        // Single table lookup
                        addNewRecordButton.Click();
                    }
                    else if (advancedLookup.TryFindElement(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.AddNewButton]), out var addNewButton))
                    {
                        // Composite lookup
                        var filterTables = advancedLookup.FindElements(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.FilterTables])).ToList();
                        var tableIndex = filterTables.FindIndex(t => t.HasAttribute("aria-current"));

                        addNewButton.Click();
                        driver.WaitForTransaction();

                        var addNewTables = advancedLookup.FindElements(By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.AddNewTables]));
                        addNewTables.ElementAt(tableIndex).Click();
                    }
                }
                else
                {
                    // Lookup
                    if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Lookup.NewButton])))
                    {
                        var newButton = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Lookup.NewButton]));

                        if (newButton.GetAttribute("disabled") == null)
                            driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Lookup.NewButton])).Click();
                        else
                            throw new ElementNotInteractableException("New button is not enabled.  If this is a mulit-entity lookup, please use SelectRelatedEntity first.");
                    }
                    else
                        throw new NotFoundException("New button not found.");
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<IReadOnlyList<FormNotification>> GetFormNotifications()
        {
            return Execute(GetOptions($"Get all form notifications"), driver =>
            {
                List<FormNotification> notifications = new List<FormNotification>();

                // Look for notificationMessageAndButtons bar
                var notificationMessage = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormMessageBar), TimeSpan.FromSeconds(2));

                if (notificationMessage != null)
                {
                    IWebElement icon = null;

                    try
                    {
                        icon = driver.FindElement(By.XPath(Entity.EntityReference.FormMessageBarTypeIcon));
                    }
                    catch (NoSuchElementException)
                    {
                        // Swallow the exception
                    }

                    if (icon != null)
                    {
                        var notification = new FormNotification
                        {
                            Message = notificationMessage?.Text
                        };
                        string classes = icon.GetAttribute("class");
                        notification.SetTypeFromClass(classes);
                        notifications.Add(notification);
                    }
                }

                // Look for the notification wrapper, if it doesn't exist there are no notificatios
                var notificationBar = driver.WaitUntilVisible(By.XPath(Entity.EntityReference.FormNotifcationBar), TimeSpan.FromSeconds(2));
                if (notificationBar == null)
                    return notifications;
                else
                {
                    // If there are multiple notifications, the notifications must be expanded first.
                    if (notificationBar.TryFindElement(By.XPath(Entity.EntityReference.FormNotifcationExpandButton), out var expandButton))
                    {
                        if (!Convert.ToBoolean(notificationBar.GetAttribute("aria-expanded")))
                            expandButton.Click();

                        // After expansion the list of notifications are now in a different element
                        notificationBar = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormNotifcationFlyoutRoot), TimeSpan.FromSeconds(2), "Failed to open the form notifications");
                    }

                    var notificationList = notificationBar.FindElement(By.XPath(Entity.EntityReference.FormNotifcationList));
                    var notificationListItems = notificationList.FindElements(By.TagName("li"));

                    foreach (var item in notificationListItems)
                    {
                        var icon = item.FindElement(By.XPath(Entity.EntityReference.FormNotifcationTypeIcon));

                        var notification = new FormNotification
                        {
                            Message = item.Text
                        };
                        string classes = icon.GetAttribute("class");
                        notification.SetTypeFromClass(classes);
                        notifications.Add(notification);
                    }

                    if (notificationBar != null)
                    {
                        notificationBar = driver.WaitUntilVisible(By.XPath(Entity.EntityReference.FormNotifcationBar), TimeSpan.FromSeconds(2));
                        notificationBar.Click(true); // Collapse the notification bar
                    }
                    return notifications;
                }

            }).Value;
        }

        #endregion



        #region PerformanceCenter

        internal void EnablePerformanceCenter()
        {
            Browser.Driver.Navigate().GoToUrl($"{Browser.Driver.Url}&perf=true");
            Browser.Driver.WaitForPageToLoad();
            Browser.Driver.WaitForTransaction();
        }

        #endregion

        #region PowerApp
        private bool _inPowerApps = false;
        internal IWebElement LocatePowerApp(IWebDriver driver, string appId)
        {
            IWebElement powerApp = null;
            Trace.WriteLine(String.Format("Locating {0} App", appId));
            if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.PowerApp.ModelFormContainer].Replace("[NAME]", appId))))
            {
                powerApp = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.PowerApp.ModelFormContainer].Replace("[NAME]", appId)));
                driver.SwitchTo().Frame(powerApp);
                powerApp = driver.FindElement(By.XPath("//iframe[@class='publishedAppIframe']"));
                driver.SwitchTo().Frame(powerApp);
                _inPowerApps = true;
            }
            else
            {
                throw new NotFoundException(String.Format("PowerApp with Id {0} not found.", appId));
            }
            return powerApp;
        }
        public BrowserCommandResult<bool> PowerAppSendCommand(string appId, string command)
        {
            return this.Execute(GetOptions("PowerApp Send Command"), driver =>
            {
                LocatePowerApp(driver, appId);
                return true;
            });
        }
        public BrowserCommandResult<bool> PowerAppSelect(string appId, string control)
        {

            return this.Execute(GetOptions("PowerApp Select"), driver =>
            {
                if(!_inPowerApps) LocatePowerApp(driver, appId);
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.PowerApp.Control].Replace("[NAME]", control))))
                {
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.PowerApp.Control].Replace("[NAME]", control))).Click();
                }
                else
                {
                    throw new NotFoundException(String.Format("Control {0} not found in Power App {1}", control, appId));
                }
                return true;
            });
        }
        public BrowserCommandResult<bool> PowerAppSetProperty(string appId, string control, string value)
        {

            return this.Execute(GetOptions("PowerApp Set Property"), driver =>
            {
                LocatePowerApp(driver, appId);
                return true;
            });
        }
        #endregion PowerApp
        internal void ThinkTime(int milliseconds)
        {
            Browser.ThinkTime(milliseconds);
        }

        internal void ThinkTime(TimeSpan timespan)
        {
            ThinkTime((int)timespan.TotalMilliseconds);
        }

        public void Dispose()
        {
            Browser.Dispose();
        }
    }
}
