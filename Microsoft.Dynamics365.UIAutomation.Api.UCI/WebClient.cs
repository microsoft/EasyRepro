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
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.Grid;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.OnlineLogin;

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
                                      bool isUCI = driver.HasElement(By.XPath(LoginReference.CrmUCIMainPage));
                                      if (isUCI)
                                          driver.WaitForTransaction();
                                  });

            var xpathToMainPage = By.XPath(LoginReference.CrmMainPage);
            var element = driver.WaitUntilAvailable(xpathToMainPage, timeout, successCallback, failureCallback);
            return element != null;
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
