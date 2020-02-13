// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Threading;
using System.Web;
using OpenQA.Selenium.Interactions;
using OtpNet;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class WebClient : BrowserPage
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
                var uri = driver.Url;
                var queryParams = "&flags=easyreproautomation=true";

                if (Browser.Options.UCITestMode) queryParams += ",testmode=true";
                if (Browser.Options.UCIPerformanceMode) queryParams += "&perf=true";

                if (!uri.Contains(queryParams) && !uri.Contains(HttpUtility.UrlEncode(queryParams)))
                {
                    var testModeUri = uri + queryParams;

                    driver.Navigate().GoToUrl(testModeUri);
                }

                WaitForMainPage();

                return true;
            });
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
                                  _ => {
                                      bool isUCI = driver.HasElement(By.XPath(Elements.Xpath[Reference.Login.CrmUCIMainPage]));
                                      if (isUCI)
                                          driver.WaitForTransaction();
                                  });
         
            var xpathToMainPage = By.XPath(Elements.Xpath[Reference.Login.CrmMainPage]);
            var element = driver.WaitUntilVisible(xpathToMainPage, timeout, successCallback, failureCallback);
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

        internal BrowserCommandResult<LoginResult> Login(Uri orgUri, SecureString username, SecureString password, SecureString mfaSecrectKey = null, Action<LoginRedirectEventArgs> redirectAction = null)
        {
            return Execute(GetOptions("Login"), Login, orgUri, username, password, mfaSecrectKey, redirectAction);
        }

        private LoginResult Login(IWebDriver driver, Uri uri, SecureString username, SecureString password, SecureString mfaSecrectKey = null, Action<LoginRedirectEventArgs> redirectAction = null)
        {
            bool online = !(OnlineDomains != null && !OnlineDomains.Any(d => uri.Host.EndsWith(d)));
            driver.Navigate().GoToUrl(uri);

            if (!online)
                return LoginResult.Success;

            driver.ClickIfVisible(By.Id("use_another_account_link"));

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
                entered = EnterOneTimeCode(driver, mfaSecrectKey);
                success = ClickStaySignedIn(driver) || IsUserAlreadyLogged();
                attempts++;
            }
            while (!success && attempts <= Constants.DefaultRetryAttempts); // retry to enter the otc-code, if its fail & it is requested again 
           
            if (entered && !success)
                throw new InvalidOperationException("Somethig got wrong entering the OTC. Please check the MFA-SecrectKey in configuration.");

            return success ? LoginResult.Success : LoginResult.Failure;
        }

        private bool IsUserAlreadyLogged() => WaitForMainPage(2.Seconds());

        private static string GenerateOneTimeCode(SecureString mfaSecrectKey)
        {
            // credits:
            // https://dev.to/j_sakamoto/selenium-testing---how-to-sign-in-to-two-factor-authentication-2joi
            // https://www.nuget.org/packages/Otp.NET/
            string key = mfaSecrectKey?.ToUnsecureString(); // <- this 2FA secret key.

            byte[] base32Bytes = Base32Encoding.ToBytes(key);

            var totp = new Totp(base32Bytes);
            var result = totp.ComputeTotp(); // <- got 2FA coed at this time!
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

        private bool EnterOneTimeCode(IWebDriver driver, SecureString mfaSecrectKey)
        {
            try
            {
                IWebElement input = GetOtcInput(driver); // wait for the dialog, even if key is null, to print the right error
                if (input == null)
                    return true;

                if (mfaSecrectKey == null)
                    throw new InvalidOperationException("The application is wait for the OTC but your MFA-SecrectKey is not set. Please check your configuration.");

                var oneTimeCode = GenerateOneTimeCode(mfaSecrectKey);
                SetInputValue(driver, input, oneTimeCode, 1.Seconds());
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
            var element = driver.ClickIfVisible(xpath, 5.Seconds());
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
                    () => new InvalidOperationException("Load Main Page Fail.")
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

        #region Navigation

        internal BrowserCommandResult<bool> OpenApp(string appName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return Execute(GetOptions($"Open App {appName}"), driver =>
            {
                driver.WaitForPageToLoad();
                driver.SwitchTo().DefaultContent();

                //Handle left hand Nav in Web Client
                var success = TryToClickInAppTile(appName, driver) ||
                              TryOpenAppFromMenu(driver, appName, AppReference.Navigation.WebAppMenuButton) ||
                              TryOpenAppFromMenu(driver, appName, AppReference.Navigation.UCIAppMenuButton);

                if (!success)
                    throw new InvalidOperationException($"App Name {appName} not found.");

                Thread.Sleep(1000);
                WaitForMainPage();
                InitializeModes();
                return true;
            });
        }

        private bool TryOpenAppFromMenu(IWebDriver driver, string appName, string appMenuButton)
        {
            bool found = false;
            var xpathToAppMenu = By.XPath(AppElements.Xpath[appMenuButton]);
            driver.WaitUntilClickable(xpathToAppMenu, TimeSpan.FromSeconds(5),
                        appMenu =>
                        {
                            appMenu.Click(true);
                            OpenAppFromMenu(driver, appName);
                            found = true;
                        });
            return found;
        }

        internal void OpenAppFromMenu(IWebDriver driver, string appName)
        {
            var container = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.AppMenuContainer]));
            var xpathToButton = "//nav[@aria-hidden='false']//button//*[text()='[TEXT]']".Replace("[TEXT]", appName);
            container.ClickWhenAvailable(By.XPath(xpathToButton),
                    TimeSpan.FromSeconds(1),
                    $"App Name {appName} not found."
                );

            driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Application.Shell]));
            driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapLauncherButton]));
        }

        private static bool TryToClickInAppTile(string appName, IWebDriver driver)
        {
            string message = null;
            driver.WaitUntil(
                d =>
                {
                    try
                    {
                        driver.SwitchTo().Frame("AppLandingPage");
                    }
                    catch (NoSuchFrameException ex)
                    {
                        message = $"Frame AppLandingPage is not loaded. Exception: {ex.Message}";
                        Trace.TraceWarning(message);
                        return false;
                    }
                    return true;
                },
                TimeSpan.FromSeconds(30),
                failureCallback: () => throw new InvalidOperationException(message)
                );

            var xpathToAppContainer = By.XPath(AppElements.Xpath[AppReference.Navigation.UCIAppContainer]);
            var xpathToappTile = By.XPath(AppElements.Xpath[AppReference.Navigation.UCIAppTile].Replace("[NAME]", appName));

            bool success = false;
            driver.WaitUntilVisible(xpathToAppContainer, TimeSpan.FromSeconds(5),
                appContainer => success = appContainer.ClickWhenAvailable(xpathToappTile, TimeSpan.FromSeconds(5)) != null
                );

            return success;
        }

        internal BrowserCommandResult<bool> OpenGroupSubArea(string group, string subarea, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Group Sub Area"), driver =>
            {
                //Make sure the sitemap-launcher is expanded - 9.1
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapLauncherButton])))
                {
                    var expanded = bool.Parse(driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapLauncherButton])).GetAttribute("aria-expanded"));

                    if (!expanded)
                        driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapLauncherButton]));
                }

                var groups = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Navigation.SitemapMenuGroup]));
                var groupList = groups.FirstOrDefault(g => g.GetAttribute("aria-label").ToLowerString() == group.ToLowerString());
                if (groupList == null)
                {
                    throw new NotFoundException($"No group with the name '{group}' exists");
                }

                var subAreaItems = groupList.FindElements(By.XPath(AppElements.Xpath[AppReference.Navigation.SitemapMenuItems]));
                var subAreaItem = subAreaItems.FirstOrDefault(a => a.GetAttribute("data-text").ToLowerString() == subarea.ToLowerString());
                if (subAreaItem == null)
                {
                    throw new NotFoundException($"No subarea with the name '{subarea}' exists inside of '{group}'");
                }

                subAreaItem.Click(true);

                WaitForLoadArea(driver);
                return true;
            });
        }

        internal BrowserCommandResult<bool> OpenSubArea(string area, string subarea, int thinkTime = Constants.DefaultThinkTime)
        {
            return Execute(GetOptions("Open Sub Area"), driver =>
            {
                //If the subarea is already in the left hand nav, click it
                var success = TryOpenSubArea(subarea);
                if (!success)
                {
                    success = TryOpenArea(area);
                    if (!success)
                        throw new InvalidOperationException($"Area with the name '{area}' not found. ");

                    success = TryOpenSubArea(subarea);
                    if (!success)
                        throw new InvalidOperationException($"No subarea with the name '{subarea}' exists inside of '{area}'.");
                }

                WaitForLoadArea(driver);
                return true;
            });
        }

        private static void WaitForLoadArea(IWebDriver driver)
        {
            driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));
            driver.WaitForPageToLoad();
            driver.WaitForTransaction();
        }

        public BrowserCommandResult<bool> OpenSubArea(string subarea)
        {
            return Execute(GetOptions("Open Unified Interface Sub-Area"), driver =>
            {
                var success = TryOpenSubArea(subarea);
                WaitForLoadArea(driver);
                return success;
            });
        }

        private bool TryOpenSubArea(string subarea)
        {
            subarea = subarea.ToLowerString();
            var navSubAreas = OpenSubMenu(subarea).Value;

            var found = navSubAreas.TryGetValue(subarea, out var element);
            if (found)
                element.Click(true);

            return found;
        }

        public BrowserCommandResult<bool> OpenArea(string subarea)
        {
            return Execute(GetOptions("Open Unified Interface Area"), driver =>
            {
                var success = TryOpenArea(subarea);
                WaitForLoadArea(driver);
                return success;
            });
        }

        private bool TryOpenArea(string area)
        {
            area = area.ToLowerString();
            var areas = OpenAreas(area).Value;

            IWebElement menuItem = null;
            bool foundMenuItem = areas.TryGetValue(area, out menuItem);

            if (foundMenuItem)
                menuItem.Click(true);

            return foundMenuItem;
        }

        public BrowserCommandResult<Dictionary<string, IWebElement>> OpenAreas(string area, int thinkTime = Constants.DefaultThinkTime)
        {
            return Execute(GetOptions("Open Unified Interface Area"), driver =>
            {
                //  9.1 ?? 9.0.2 <- inverted order (fallback first) run quickly
                var areas = OpenMenuFallback(area) ?? OpenMenu();

                if (!areas.ContainsKey(area))
                    throw new InvalidOperationException($"No area with the name '{area}' exists.");

                return areas;
            });
        }

        public Dictionary<string, IWebElement> OpenMenu(int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions("Open Menu"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.AreaButton]));

                var result = GetMenuItemsFrom(driver, AppReference.Navigation.AreaMenu);
                return result;
            });
        }

        public Dictionary<string, IWebElement> OpenMenuFallback(string area, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions("Open Menu"), driver =>
            {
                //Make sure the sitemap-launcher is expanded - 9.1
                var xpathSiteMapLauncherButton = By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapLauncherButton]);
                bool success = driver.TryFindElement(xpathSiteMapLauncherButton, out IWebElement launcherButton);
                if (success)
                {
                    bool expanded = bool.Parse(launcherButton.GetAttribute("aria-expanded"));
                    if (!expanded)
                        driver.ClickWhenAvailable(xpathSiteMapLauncherButton);
                }

                var dictionary = new Dictionary<string, IWebElement>();

                //Is this the sitemap with enableunifiedinterfaceshellrefresh?
                var xpathSitemapSwitcherButton = By.XPath(AppElements.Xpath[AppReference.Navigation.SitemapSwitcherButton]);
                success = driver.TryFindElement(xpathSitemapSwitcherButton, out IWebElement switcherButton);
                if (success)
                {
                    switcherButton.Click(true);
                    driver.WaitForTransaction();

                    AddMenuItemsFrom(driver, AppReference.Navigation.SitemapSwitcherFlyout, dictionary);
                }

                var xpathSiteMapAreaMoreButton = By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapAreaMoreButton]);
                success = driver.TryFindElement(xpathSiteMapAreaMoreButton, out IWebElement moreButton);
                if (!success)
                    return dictionary;

                bool isVisible = moreButton.IsVisible();
                if (isVisible)
                {
                    moreButton.Click();
                    AddMenuItemsFrom(driver, AppReference.Navigation.AreaMoreMenu, dictionary);
                }
                else
                {
                    var singleItem = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapSingleArea].Replace("[NAME]", area)));
                    char[] trimCharacters =
                    {
                        '', '\r', '\n', '',
                        '', ''
                    };

                    dictionary.Add(singleItem.Text.Trim(trimCharacters).ToLowerString(), singleItem);
                }

                return dictionary;
            });
        }

        private static Dictionary<string, IWebElement> GetMenuItemsFrom(IWebDriver driver, string referenceToMenuItemsContainer)
        {
            var result = new Dictionary<string, IWebElement>();
            AddMenuItemsFrom(driver, referenceToMenuItemsContainer, result);
            return result;
        }

        private static void AddMenuItemsFrom(IWebDriver driver, string referenceToMenuItemsContainer, Dictionary<string, IWebElement> dictionary)
        {
            driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[referenceToMenuItemsContainer]),
                TimeSpan.FromSeconds(2),
                menu => AddMenuItems(menu, dictionary),
                "The Main Menu is not available."
            );
        }

        private static Dictionary<string, IWebElement> GetMenuItems(IWebElement menu)
        {
            var result = new Dictionary<string, IWebElement>();
            AddMenuItems(menu, result);
            return result;
        }

        private static void AddMenuItems(IWebElement menu, Dictionary<string, IWebElement> dictionary)
        {
            var menuItems = menu.FindElements(By.TagName("li"));
            foreach (var item in menuItems)
            {
                string key = item.Text.ToLowerString();
                if (dictionary.ContainsKey(key))
                    continue;
                dictionary.Add(key, item);
            }
        }

        internal BrowserCommandResult<Dictionary<string, IWebElement>> OpenSubMenu(string subarea)
        {
            return this.Execute(GetOptions($"Open Sub Menu: {subarea}"), driver =>
            {
                var dictionary = new Dictionary<string, IWebElement>();

                //Sitemap without enableunifiedinterfaceshellrefresh
                var hasPinnedSitemapEntity = driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Navigation.PinnedSitemapEntity]));
                if (!hasPinnedSitemapEntity)
                {
                    // Close SiteMap launcher since it is open
                    var xpathToLauncherCloseButton = By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapLauncherCloseButton]);
                    driver.ClickWhenAvailable(xpathToLauncherCloseButton);

                    driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapLauncherButton]));

                    var menuContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.SubAreaContainer]));

                    var subItems = menuContainer.FindElements(By.TagName("li"));

                    foreach (var subItem in subItems)
                    {
                        // Check 'Id' attribute, NULL value == Group Header
                        var id = subItem.GetAttribute("Id");
                        if (string.IsNullOrEmpty(id))
                            continue;

                        // Filter out duplicate entity keys - click the first one in the list
                        var key = subItem.Text.ToLowerString();
                        if (!dictionary.ContainsKey(key))
                            dictionary.Add(key, subItem);
                    }

                    return dictionary;
                }

                //Sitemap with enableunifiedinterfaceshellrefresh enabled
                var menuShell = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Navigation.SubAreaContainer]));

                //The menu is broke into multiple sections. Gather all items.
                foreach (IWebElement menuSection in menuShell)
                {
                    var menuItems = menuSection.FindElements(By.XPath(AppElements.Xpath[AppReference.Navigation.SitemapMenuItems]));

                    foreach (var menuItem in menuItems)
                    {
                        var text = menuItem.Text.ToLowerString();
                        if (string.IsNullOrEmpty(text))
                            continue;

                        if (!dictionary.ContainsKey(text))
                            dictionary.Add(text, menuItem);
                    }
                }

                return dictionary;
            });
        }

        internal BrowserCommandResult<bool> OpenSettingsOption(string command, string dataId, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions($"Open " + command + " " + dataId), driver =>
            {
                var cmdButtonBar = AppElements.Xpath[AppReference.Navigation.SettingsLauncherBar].Replace("[NAME]", command);
                var cmdLauncher = AppElements.Xpath[AppReference.Navigation.SettingsLauncher].Replace("[NAME]", command);

                if (!driver.IsVisible(By.XPath(cmdLauncher)))
                {
                    driver.ClickWhenAvailable(By.XPath(cmdButtonBar));

                    Thread.Sleep(1000);

                    driver.SetVisible(By.XPath(cmdLauncher), true);
                    driver.WaitUntilVisible(By.XPath(cmdLauncher));
                }

                var menuContainer = driver.FindElement(By.XPath(cmdLauncher));
                var menuItems = menuContainer.FindElements(By.TagName("button"));
                var button = menuItems.FirstOrDefault(x => x.GetAttribute("data-id").Contains(dataId));

                if (button != null)
                {
                    button.Click();
                }
                else
                {
                    throw new InvalidOperationException($"No command with the exists inside of the Command Bar.");
                }

                return true;
            });
        }

        /// <summary>
        /// Opens the Guided Help
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenGuidedHelp();</example>
        public BrowserCommandResult<bool> OpenGuidedHelp(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Guided Help"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.GuidedHelp]));

                return true;
            });
        }

        /// <summary>
        /// Opens the Admin Portal
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenAdminPortal();</example>
        internal BrowserCommandResult<bool> OpenAdminPortal(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);
            return this.Execute(GetOptions("Open Admin Portal"), driver =>
            {
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Application.Shell]));
                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.AdminPortal]))?.Click();
                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.AdminPortalButton]))?.Click();
                return true;
            });
        }

        /// <summary>
        /// Open Global Search
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenGlobalSearch();</example>
        internal BrowserCommandResult<bool> OpenGlobalSearch(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Global Search"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Navigation.SearchButton]),
                    TimeSpan.FromSeconds(5),
                    d => { driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.SearchButton])); },
                    "The Global Search button is not available."
                );
                return true;
            });
        }

        internal BrowserCommandResult<bool> ClickQuickLaunchButton(string toolTip, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Quick Launch: {toolTip}"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Navigation.QuickLaunchMenu]));

                //Text could be in the crumb bar.  Find the Quick launch bar buttons and click that one.
                var buttons = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.QuickLaunchMenu]));
                var launchButton = buttons.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.QuickLaunchButton].Replace("[NAME]", toolTip)));
                launchButton.Click();

                return true;
            });
        }

        internal BrowserCommandResult<bool> QuickCreate(string entityName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Quick Create: {entityName}"), driver =>
            {
                //Click the + button in the ribbon
                var quickCreateButton = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.QuickCreateButton]));
                quickCreateButton.Click(true);

                //Find the entity name in the list
                var entityMenuList = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.QuickCreateMenuList]));
                var entityMenuItems = entityMenuList.FindElements(By.XPath(AppElements.Xpath[AppReference.Navigation.QuickCreateMenuItems]));
                var entitybutton = entityMenuItems.FirstOrDefault(e => e.Text.Contains(entityName, StringComparison.OrdinalIgnoreCase));

                if (entitybutton == null)
                    throw new Exception(String.Format("{0} not found in Quick Create list.", entityName));

                //Click the entity name
                entitybutton.Click(true);

                driver.WaitForTransaction();

                return true;
            });
        }

        #endregion

        #region Dialogs

        internal bool SwitchToDialog(int frameIndex = 0)
        {
            var index = "";
            if (frameIndex > 0)
                index = frameIndex.ToString();

            Browser.Driver.SwitchTo().DefaultContent();

            // Check to see if dialog is InlineDialog or popup
            var inlineDialog = Browser.Driver.HasElement(By.XPath(Elements.Xpath[Reference.Frames.DialogFrame].Replace("[INDEX]", index)));
            if (inlineDialog)
            {
                //wait for the content panel to render
                Browser.Driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.DialogFrame].Replace("[INDEX]", index)),
                    TimeSpan.FromSeconds(2),
                    d => { Browser.Driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.DialogFrameId].Replace("[INDEX]", index)); });
                return true;
            }
            else
            {
                // need to add this functionality
                //SwitchToPopup();
            }

            return true;
        }

        internal BrowserCommandResult<bool> CloseWarningDialog()
        {
            return this.Execute(GetOptions($"Close Warning Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.WarningFooter]));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(Elements.Xpath[Reference.Dialogs.WarningCloseButton])).Count >
                          0)) return true;
                    var closeBtn = dialogFooter.FindElement(By.XPath(Elements.Xpath[Reference.Dialogs.WarningCloseButton]));
                    closeBtn.Click();
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> ConfirmationDialog(bool ClickConfirmButton)
        {
            //Passing true clicks the confirm button.  Passing false clicks the Cancel button.
            return this.Execute(GetOptions($"Confirm or Cancel Confirmation Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    //Wait until the buttons are available to click
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.ConfirmButton]));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(AppElements.Xpath[AppReference.Dialogs.ConfirmButton])).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    if (ClickConfirmButton)
                        buttonToClick = dialogFooter.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.ConfirmButton]));
                    else
                        buttonToClick = dialogFooter.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.CancelButton]));

                    buttonToClick.Click();
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> AssignDialog(Dialogs.AssignTo to, string userOrTeamName = null)
        {
            userOrTeamName = userOrTeamName?.Trim() ?? string.Empty;
            return this.Execute(GetOptions($"Assign to User or Team Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (!inlineDialog)
                    return false;

                //Click the Option to Assign to User Or Team
                var xpathToToggleButton = By.XPath(AppElements.Xpath[AppReference.Dialogs.AssignDialogToggle]);
                var toggleButton = driver.WaitUntilClickable(xpathToToggleButton, "Me/UserTeam toggle button unavailable");

                if (to == Dialogs.AssignTo.Me)
                {
                    if (toggleButton.Text != "Me")
                        toggleButton.Click();
                }
                else
                {
                    if (toggleButton.Text == "Me")
                        toggleButton.Click();

                    //Set the User Or Team
                    var userOrTeamField = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookup]), "User field unavailable");
                    var input = userOrTeamField.ClickWhenAvailable(By.TagName("input"), "User field unavailable");
                    input.SendKeys(userOrTeamName, true);
                    
                    ThinkTime(2000);

                    //Pick the User from the list
                    var container = driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.AssignDialogUserTeamLookupResults]));
                    container.WaitUntil(
                        c => c.FindElements(By.TagName("li")).FirstOrDefault(r => r.Text.StartsWith(userOrTeamName, StringComparison.OrdinalIgnoreCase)),
                        successCallback: e => e.Click(true),
                        failureCallback: () => throw new InvalidOperationException($"None {to} found which match with '{userOrTeamName}'"));
                }

                //Click Assign
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.AssignDialogOKButton]), TimeSpan.FromSeconds(5),
                    "Unable to click the OK button in the assign dialog");

                return true;
            });
        }

        internal BrowserCommandResult<bool> SwitchProcessDialog(string processToSwitchTo)
        {
            return this.Execute(GetOptions($"Switch Process Dialog"), driver =>
            {
                //Wait for the Grid to load
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.ActiveProcessGridControlContainer]));

                //Select the Process
                var popup = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.SwitchProcessContainer]));
                var labels = popup.FindElements(By.TagName("label"));
                foreach (var label in labels)
                {
                    if (label.Text.Equals(processToSwitchTo, StringComparison.OrdinalIgnoreCase))
                    {
                        label.Click();
                        break;
                    }
                }

                //Click the OK button
                var okBtn = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.SwitchProcessDialogOK]));
                okBtn.Click();

                return true;
            });
        }

        internal BrowserCommandResult<bool> CloseOpportunityDialog(bool clickOK)
        {
            return this.Execute(GetOptions($"Close Opportunity Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();

                if (inlineDialog)
                {
                    //Close Opportunity
                    var xPath = AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok];

                    //Cancel
                    if (!clickOK)
                        xPath = AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok];

                    driver.ClickWhenAvailable(By.XPath(xPath), TimeSpan.FromSeconds(5), "The Close Opportunity dialog is not available.");
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> HandleSaveDialog()
        {
            //If you click save and something happens, handle it.  Duplicate Detection/Errors/etc...
            //Check for Dialog and figure out which type it is and return the dialog type.

            //Introduce think time to avoid timing issues on save dialog
            ThinkTime(1000);

            return this.Execute(GetOptions($"Validate Save"), driver =>
            {
                //Is it Duplicate Detection?
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionWindowMarker])))
                {
                    if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows])))
                    {
                        //Select the first record in the grid
                        driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows]))[0].Click(true);

                        //Click Ignore and Save
                        driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton])).Click(true);
                        driver.WaitForTransaction();
                    }
                }

                //Is it an Error?
                if (driver.HasElement(By.XPath("//div[contains(@data-id,'errorDialogdialog')]")))
                {
                    var errorDialog = driver.FindElement(By.XPath("//div[contains(@data-id,'errorDialogdialog')]"));

                    var errorDetails = errorDialog.FindElement(By.XPath(".//*[contains(@data-id,'errorDialog_subtitle')]"));

                    if (!String.IsNullOrEmpty(errorDetails.Text))
                        throw new InvalidOperationException(errorDetails.Text);
                }


                return true;
            });
        }

        /// <summary>
        /// Opens the dialog
        /// </summary>
        /// <param name="dialog"></param>
        internal List<ListItem> GetListItems(IWebElement dialog)
        {
            var titlePath = By.XPath(".//label/span");
            var elementPath = By.XPath(".//div");

            var result = GetListItems(dialog, titlePath, elementPath);
            return result;
        }

        private static List<ListItem> GetListItems(IWebElement dialog, By titlePath, By elementPath)
        {
            var list = new List<ListItem>();
            var dialogItems = dialog.FindElements(By.XPath(".//li"));
            foreach (var dialogItem in dialogItems)
            {
                var titleLinks = dialogItem.FindElements(titlePath);
                if (titleLinks == null || titleLinks.Count == 0)
                    continue;

                var divLinks = dialogItem.FindElements(elementPath);
                if (divLinks == null || divLinks.Count == 0)
                    continue;

                var element = divLinks[0];
                var id = element.GetAttribute("id");
                var title = titleLinks[0].GetAttribute("innerText");

                list.Add(new ListItem
                {
                    Id = id,
                    Title = title,
                    Element = element
                });
            }

            return list;
        }

        #endregion

        #region CommandBar

        internal BrowserCommandResult<bool> ClickCommand(string name, string subname = "", bool moreCommands = false, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Click Command"), driver =>
            {
                IWebElement ribbon = null;

                //Find the button in the CommandBar
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container])))
                    ribbon = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container]));

                if (ribbon == null)
                {
                    if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid])))
                        ribbon = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid]));
                    else
                        throw new InvalidOperationException("Unable to find the ribbon.");
                }

                //Get the CommandBar buttons
                var items = ribbon.FindElements(By.TagName("li"));

                //Is the button in the ribbon?
                if (items.Any(x => x.GetAttribute("aria-label").Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    items.FirstOrDefault(x => x.GetAttribute("aria-label").Equals(name, StringComparison.OrdinalIgnoreCase)).Click(true);
                    driver.WaitForTransaction();
                }
                else
                {
                    //Is the button in More Commands?
                    if (items.Any(x => x.GetAttribute("aria-label").Equals("More Commands", StringComparison.OrdinalIgnoreCase)))
                    {
                        //Click More Commands
                        items.FirstOrDefault(x => x.GetAttribute("aria-label").Equals("More Commands", StringComparison.OrdinalIgnoreCase)).Click(true);
                        driver.WaitForTransaction();

                        //Click the button
                        if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Button].Replace("[NAME]", name))))
                        {
                            driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Button].Replace("[NAME]", name))).Click(true);
                            driver.WaitForTransaction();
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");
                    }
                    else
                        throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");
                }

                if (!string.IsNullOrEmpty(subname))
                {
                    var submenu = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.MoreCommandsMenu]));

                    var subbutton = submenu.FindElements(By.TagName("button")).FirstOrDefault(x => x.Text == subname);

                    if (subbutton != null)
                    {
                        subbutton.Click(true);
                    }
                    else
                        throw new InvalidOperationException($"No sub command with the name '{subname}' exists inside of Commandbar.");
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        /// <summary>
        /// Returns the values of CommandBar objects
        /// </summary>
        /// <param name="moreCommands">The moreCommands</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Related.ClickCommand("ADD NEW CASE");</example>
        internal BrowserCommandResult<List<string>> GetCommandValues(bool includeMoreCommandsValues = false, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Get CommandBar Command Count"), driver =>
            {
                IWebElement ribbon = null;
                List<string> commandValues = new List<string>();

                //Find the button in the CommandBar
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container])))
                    ribbon = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container]));

                if (ribbon == null)
                {
                    if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid])))
                        ribbon = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid]));
                    else
                        throw new InvalidOperationException("Unable to find the ribbon.");
                }

                //Get the CommandBar buttons
                var commandBarItems = ribbon.FindElements(By.TagName("li"));

                foreach (var value in commandBarItems)
                {
                    if (value.Text != "")
                    {
                        string commandText = value.Text.ToString();

                        if (commandText.Contains("\r\n"))
                        {
                            commandText = commandText.Substring(0, commandText.IndexOf("\r\n"));
                        }

                        if (!commandValues.Contains(value.Text))
                        {
                            commandValues.Add(commandText);
                        }
                    }
                }

                if (includeMoreCommandsValues)
                {
                    if (commandBarItems.Any(x => x.GetAttribute("aria-label").Equals("More Commands", StringComparison.OrdinalIgnoreCase)))
                    {
                        //Click More Commands Button
                        commandBarItems.FirstOrDefault(x => x.GetAttribute("aria-label").Equals("More Commands", StringComparison.OrdinalIgnoreCase)).Click(true);
                        driver.WaitForTransaction();

                        //Click the button
                        var moreCommandsMenu = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.MoreCommandsMenu]));

                        if (moreCommandsMenu != null)
                        {
                            var moreCommandsItems = moreCommandsMenu.FindElements(By.TagName("li"));

                            foreach (var value in moreCommandsItems)
                            {
                                if (value.Text != "")
                                {
                                    string commandText = value.Text.ToString();

                                    if (commandText.Contains("\r\n"))
                                    {
                                        commandText = commandText.Substring(0, commandText.IndexOf("\r\n"));
                                    }

                                    if (!commandValues.Contains(value.Text))
                                    {
                                        commandValues.Add(commandText);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("Unable to locate the 'More Commands' menu");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("No button matching 'More Commands' exists in the CommandBar");
                    }
                }

                return commandValues;
            });
        }

        #endregion

        #region Grid

        public BrowserCommandResult<Dictionary<string, string>> OpenViewPicker(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open View Picker"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelector]),
                    TimeSpan.FromSeconds(20),
                    "Unable to click the View Picker"
                );

                var viewContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.ViewContainer]));
                var viewItems = viewContainer.FindElements(By.TagName("li"));
                var dictionary = new Dictionary<string, string>();

                foreach (var viewItem in viewItems)
                {
                    var role = viewItem.GetAttribute("role");
                    if (role == "option")
                        dictionary.Add(viewItem.Text, viewItem.GetAttribute("id"));
                }

                return dictionary;
            });
        }

        internal BrowserCommandResult<bool> SwitchView(string viewName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Switch View"), driver =>
            {
                var views = OpenViewPicker().Value;
                Thread.Sleep(500);
                if (!views.ContainsKey(viewName))
                {
                    throw new InvalidOperationException($"No view with the name '{viewName}' exists.");
                }

                var viewId = views[viewName];
                driver.ClickWhenAvailable(By.Id(viewId));

                return true;
            });
        }

        internal BrowserCommandResult<bool> OpenRecord(int index, int thinkTime = Constants.DefaultThinkTime, bool checkRecord = false)
        {
            ThinkTime(thinkTime);

            return Execute(GetOptions("Open Grid Record"), driver =>
            {
                IWebElement control = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));

                var xpathToFind = checkRecord
                    ? $"//div[@data-id='cell-{index}-1']"
                    : $"//div[contains(@data-id, 'cell-{index}')]//a";
                control.ClickWhenAvailable(By.XPath(xpathToFind), "An error occur trying to open the record at position {index}");

                // Logic equivalent to fix #746 (by @rswafford) 
                //var xpathToFind = $"//div[@data-id='cell-{index}-1']";
                //control.WaitUntilClickable(By.XPath(xpathToFind),
                //    e =>
                //    {
                //        e.Click();
                //        if (!checkRecord)
                //           driver.DoubleClick(e);
                //    },
                //    $"An error occur trying to open the record at position {index}"
                //    );

                driver.WaitForTransaction();
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

        internal BrowserCommandResult<List<GridItem>> GetGridItems(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Get Grid Items"), driver =>
            {
                var returnList = new List<GridItem>();

                driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));

                var rows = driver.FindElements(By.ClassName("wj-row"));
                var columnGroup = driver.FindElement(By.ClassName("wj-colheaders"));

                foreach (var row in rows)
                {
                    if (!string.IsNullOrEmpty(row.GetAttribute("data-lp-id")) && !string.IsNullOrEmpty(row.GetAttribute("role")))
                    {
                        //MscrmControls.Grid.ReadOnlyGrid|entity_control|account|00000000-0000-0000-00aa-000010001001|account|cc-grid|grid-cell-container
                        var datalpid = row.GetAttribute("data-lp-id").Split('|');
                        var cells = row.FindElements(By.ClassName("wj-cell"));
                        var currentindex = 0;
                        var link =
                            $"{new Uri(driver.Url).Scheme}://{new Uri(driver.Url).Authority}/main.aspx?etn={datalpid[2]}&pagetype=entityrecord&id=%7B{datalpid[3]}%7D";

                        var item = new GridItem
                        {
                            EntityName = datalpid[2],
                            Url = new Uri(link)
                        };

                        foreach (var column in columnGroup.FindElements(By.ClassName("wj-row")))
                        {
                            var rowHeaders = column.FindElements(By.TagName("div"))
                                .Where(c => !string.IsNullOrEmpty(c.GetAttribute("title")) && !string.IsNullOrEmpty(c.GetAttribute("id")));

                            foreach (var header in rowHeaders)
                            {
                                var id = header.GetAttribute("id");
                                var className = header.GetAttribute("class");
                                var cellData = cells[currentindex + 1].GetAttribute("title");

                                if (!string.IsNullOrEmpty(id)
                                    && className.Contains("wj-cell")
                                    && !string.IsNullOrEmpty(cellData)
                                    && cells.Count > currentindex
                                )
                                {
                                    item[id] = cellData.Replace("-", "");
                                }

                                currentindex++;
                            }

                            returnList.Add(item);
                        }
                    }
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

        public BrowserCommandResult<bool> Sort(string columnName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Sort by {columnName}"), driver =>
            {
                var sortCol = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.GridSortColumn].Replace("[COLNAME]", columnName)));

                if (sortCol == null)
                    throw new InvalidOperationException($"Column: {columnName} Does not exist");
                else
                    sortCol.Click(true);

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
        public BrowserCommandResult<bool> OpenGridRow(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Grid Item"), driver =>
            {
                var grid = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));
                var gridCellContainer = grid.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.CellContainer]));
                var rowCount = gridCellContainer.GetAttribute("data-row-count");
                var count = 0;

                if (rowCount == null || !int.TryParse(rowCount, out count) || count <= 0) return true;
                var link =
                    gridCellContainer.FindElement(
                        By.XPath("//div[@role='gridcell'][@header-row-number='" + index + "']/following::div"));

                if (link == null)
                    throw new InvalidOperationException($"No record with the index '{index}' exists.");

                link.Click();

                driver.WaitForTransaction();
                return true;
            });
        }

        public BrowserCommandResult<bool> ClickRelatedCommand(string name, string subName = null)
        {
            return this.Execute(GetOptions("Click Related Tab Command"), driver =>
            {
                if (!driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarButton].Replace("[NAME]", name))))
                    throw new NotFoundException($"{name} button not found. Button names are case sensitive. Please check for proper casing of button name.");

                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarButton].Replace("[NAME]", name))).Click(true);

                driver.WaitForTransaction();

                if (subName != null)
                {
                    if (!driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))))
                        throw new NotFoundException($"{subName} button not found");

                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarSubButton].Replace("[NAME]", subName))).Click(true);

                    driver.WaitForTransaction();
                }

                return true;
            });
        }

        #endregion

        #region Subgrid

        public BrowserCommandResult<bool> ClickSubgridAddButton(string subgridName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Click add button of subgrid: {subgridName}"), driver =>
            {
                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridAddButton].Replace("[NAME]", subgridName)))?.Click();

                return true;
            });
        }

        #endregion

        #region Entity

        internal BrowserCommandResult<bool> CancelQuickCreate(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Cancel Quick Create"), driver =>
            {
                var save = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.CancelButton]),
                    "Quick Create Cancel Button is not available");
                save?.Click(true);

                driver.WaitForTransaction();

                return true;
            });
        }

        /// <summary>
        /// Open Entity
        /// </summary>
        /// <param name="entityName">The entity name</param>
        /// <param name="id">The Id</param>
        /// <param name="thinkTime">The think time</param>
        internal BrowserCommandResult<bool> OpenEntity(string entityName, Guid id, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open: {entityName} {id}"), driver =>
            {
                //https:///main.aspx?appid=98d1cf55-fc47-e911-a97c-000d3ae05a70&pagetype=entityrecord&etn=lead&id=ed975ea3-531c-e511-80d8-3863bb3ce2c8
                var uri = new Uri(this.Browser.Driver.Url);
                var qs = HttpUtility.ParseQueryString(uri.Query.ToLower());
                var appId = qs.Get("appid");
                var link = $"{uri.Scheme}://{uri.Authority}/main.aspx?appid={appId}&etn={entityName}&pagetype=entityrecord&id={id}";

                if (Browser.Options.UCITestMode)
                {
                    link += "&flags=testmode=true";
                }

                driver.Navigate().GoToUrl(link);

                //SwitchToContent();
                driver.WaitForPageToLoad();
                driver.WaitForTransaction();
                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Entity.Form]),
                    TimeSpan.FromSeconds(30),
                    "CRM Record is Unavailable or not finished loading. Timeout Exceeded"
                );

                return true;
            });
        }

        /// <summary>
        /// Saves the entity
        /// </summary>
        /// <param name="thinkTime"></param>
        internal BrowserCommandResult<bool> Save(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Save"), driver =>
            {
                var save = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.Save]),
                    "Save Buttton is not available");

                save?.Click();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SaveQuickCreate(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"SaveQuickCreate"), driver =>
            {
                var save = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.SaveAndCloseButton]),
                    "Quick Create Save Button is not available");
                save?.Click(true);

                driver.WaitForTransaction();

                return true;
            });
        }

        /// <summary>
        /// Open record set and navigate record index.
        /// This method supersedes Navigate Up and Navigate Down outside of UCI 
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.OpenRecordSetNavigator();</example>
        public BrowserCommandResult<bool> OpenRecordSetNavigator(int index = 0, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Record Set Navigator"), driver =>
            {
                // check if record set navigator parent div is set to open
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavigatorOpen])))
                {
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavigator])).Click();
                }

                var navList = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavList]));
                var links = navList.FindElements(By.TagName("li"));
                try
                {
                    links[index].Click();
                }
                catch
                {
                    throw new InvalidOperationException($"No record with the index '{index}' exists.");
                }

                driver.WaitForPageToLoad();

                return true;
            });
        }

        /// <summary>
        /// Close Record Set Navigator
        /// </summary>
        /// <param name="thinkTime"></param>
        /// <example>xrmApp.Entity.CloseRecordSetNavigator();</example>
        public BrowserCommandResult<bool> CloseRecordSetNavigator(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Close Record Set Navigator"), driver =>
            {
                var closeSpan = driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavCollapseIcon]));
                if (closeSpan)
                {
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavCollapseIconParent])).Click();
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
        internal BrowserCommandResult<bool> SetValue(string field, string value)
        {
            return Execute(GetOptions("Set Value"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));

                IWebElement input;
                bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);

                if (!found)
                    found = fieldContainer.TryFindElement(By.TagName("textarea"), out input);

                if (!found)
                    throw new NoSuchElementException($"Field with name {field} does not exist.");

                input.Click();
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Backspace);

                if (!string.IsNullOrWhiteSpace(value))
                    input.SendKeys(value, true);

                // Needed to transfer focus out of special fields (email or phone)
                var label = fieldContainer.ClickIfVisible(By.TagName("label"));
                if (label == null)
                    driver.ClearFocus();

                return true;
            });
        }

        private void SetInputValue(IWebDriver driver, IWebElement input, string value, TimeSpan? thinktime = null)
        {
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Backspace);
            driver.WaitForTransaction();

            if (string.IsNullOrWhiteSpace(value))
                return;

            input.SendKeys(value, true);
            driver.WaitForTransaction();
            ThinkTime(thinktime ?? 3.Seconds());
        }
        /// <summary>
        /// Sets the value of a Lookup, Customer, Owner or ActivityParty Lookup which accepts only a single value.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        /// The default index position is 0, which will be the first result record in the lookup results window. Suppy a value > 0 to select a different record if multiple are present.
        internal BrowserCommandResult<bool> SetValue(LookupItem control, int index = 0)
        {
            return Execute(GetOptions($"Set Lookup Value: {control.Name}"), driver =>
            {
                driver.WaitForTransaction(TimeSpan.FromSeconds(5));

                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupFieldContainer].Replace("[NAME]", control.Name)));

                TryRemoveLookupValue(driver, fieldContainer, control);
                TrySetValue(fieldContainer, control, index);

                return true;
            });
        }

        private void TrySetValue(IWebElement fieldContainer, LookupItem control, int index)
        {
            IWebElement input;
            bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);
            string value = control.Value?.Trim();
            if (found)
            {
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Backspace);
                input.SendKeys(value, true);
            }

            TrySetValue(fieldContainer, control, value, index);
        }

        /// <summary>
        /// Sets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new Lookup[] { Name = "to", Value = "Rene Valdes (sample)" }, { Name = "to", Value = "Alpine Ski House (sample)" } );</example>
        /// The default index position is 0, which will be the first result record in the lookup results window. Suppy a value > 0 to select a different record if multiple are present.
        internal BrowserCommandResult<bool> SetValue(LookupItem[] controls, int index = 0, bool clearFirst = true)
        {
            var control = controls.First();
            var controlName = control.Name;
            return Execute(GetOptions($"Set ActivityParty Lookup Value: {controlName}"), driver =>
            {
                driver.WaitForTransaction(TimeSpan.FromSeconds(5));

                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupFieldContainer].Replace("[NAME]", controlName)));

                if (clearFirst)
                    TryRemoveLookupValue(driver, fieldContainer, control);

                TryToSetValue(fieldContainer, controls, index);
                return true;
            });
        }

        private void TryToSetValue(ISearchContext fieldContainer, LookupItem[] controls, int index)
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
                        input.SendKeys(Keys.Tab);
                        input.SendKeys(Keys.Enter);
                    }
                }

                TrySetValue(fieldContainer, control, value, index);
            }

            input.SendKeys(Keys.Escape); // IE wants to keep the flyout open on multi-value fields, this makes sure it closes
        }

        private void TrySetValue(ISearchContext container, LookupItem control, string value, int index)
        {
            if (value == null)
                throw new InvalidOperationException($"No value has been provided for the LookupItem {control.Name}. Please provide a value or an empty string and try again.");

            if (value == string.Empty)
                SetLookupByIndex(container, control, index);

            SetLookUpByValue(container, control, index);
        }

        private void SetLookUpByValue(ISearchContext container, LookupItem control, int index)
        {
            var controlName = control.Name;
            var xpathToText = AppElements.Xpath[AppReference.Entity.LookupFieldNoRecordsText].Replace("[NAME]", controlName);
            var xpathToResultList = AppElements.Xpath[AppReference.Entity.LookupFieldResultList].Replace("[NAME]", controlName);
            var byPath = By.XPath(xpathToText + "|" + xpathToResultList);

            container.WaitUntilAvailable(byPath, TimeSpan.FromSeconds(10));

            var byPathToMenu = By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupMenu].Replace("[NAME]", controlName));
            var flyoutDialog = container.WaitUntilVisible(byPathToMenu);

            var xpathResultListItem = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldResultListItem].Replace("[NAME]", controlName));
            container.WaitUntilAvailable(xpathResultListItem, $"No Results Matching {control.Value} Were Found.");

            List<ListItem> items = GetListItems(flyoutDialog);

            if (items.Count == 0)
                throw new InvalidOperationException($"List does not contain a record with the name:  {control.Value}");

            if (index >= items.Count)
                throw new InvalidOperationException($"List does not contain {index + 1} records. Please provide an index value less than {items.Count} ");

            var selectedItem = items[index];
            selectedItem.Element.Click(true);
        }

        private void SetLookupByIndex(ISearchContext container, LookupItem control, int index)
        {
            var controlName = control.Name;
            var xpathToControl = By.XPath(AppElements.Xpath[AppReference.Entity.LookupResultsDropdown].Replace("[NAME]", controlName));
            var lookupResultsDialog = container.WaitUntilVisible(xpathToControl);

            var xpathFieldResultListItem = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldResultListItem].Replace("[NAME]", controlName));
            container.WaitUntil(d => d.FindElements(xpathFieldResultListItem).Count > 0);

            var items = GetListItems(lookupResultsDialog);
            if (items.Count == 0)
                throw new InvalidOperationException($"No results exist in the Recently Viewed flyout menu. Please provide a text value for {controlName}");

            if (index >= items.Count)
                throw new InvalidOperationException($"Recently Viewed list does not contain {index} records. Please provide an index value less than {items.Count}");

            var selectedItem = items[index];
            selectedItem.Element.Click(true);
        }

        /// <summary>
        /// Sets the value of a picklist or status field.
        /// </summary>
        /// <param name="control">The option you want to set.</param>
        /// <example>xrmApp.Entity.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public BrowserCommandResult<bool> SetValue(OptionSet control)
        {
            var controlName = control.Name;
            return Execute(GetOptions($"Set OptionSet Value: {controlName}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.OptionSetFieldContainer].Replace("[NAME]", controlName)));
                TrySetValue(fieldContainer, control);
                return true;
            });
        }

        private static void TrySetValue(IWebElement fieldContainer, OptionSet control)
        {
            var value = control.Value;
            bool success = fieldContainer.TryFindElement(By.TagName("select"), out IWebElement select);
            if (success)
            {
                var options = select.FindElements(By.TagName("option"));
                SelectOption(options, value);
                return;
            }

            var name = control.Name;
            var hasStatusCombo = fieldContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusCombo].Replace("[NAME]", name)));
            if (hasStatusCombo)
            {
                // This is for statuscode (type = status) that should act like an optionset doesn't doesn't follow the same pattern when rendered
                fieldContainer.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusComboButton].Replace("[NAME]", name)));

                var listBox = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusComboList].Replace("[NAME]", name)));

                var options = listBox.FindElements(By.TagName("li"));
                SelectOption(options, value);

                return;
            }

            throw new InvalidOperationException($"OptionSet Field: '{name}' does not exist");
        }

        private static void SelectOption(ReadOnlyCollection<IWebElement> options, string value)
        {
            var selectedOption = options.FirstOrDefault(op => op.Text == value || op.GetAttribute("value") == value);
            selectedOption.Click(true);
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        /// <example>xrmApp.Entity.SetValue(new BooleanItem { Name = "donotemail", Value = true });</example>
        public BrowserCommandResult<bool> SetValue(BooleanItem option)
        {
            return this.Execute(GetOptions($"Set BooleanItem Value: {option.Name}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", option.Name)));

                var hasRadio = fieldContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldRadioContainer].Replace("[NAME]", option.Name)));
                var hasCheckbox = fieldContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldCheckbox].Replace("[NAME]", option.Name)));
                var hasList = fieldContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldList].Replace("[NAME]", option.Name)));

                if (hasRadio)
                {
                    var trueRadio = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldRadioTrue].Replace("[NAME]", option.Name)));
                    var falseRadio = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldRadioFalse].Replace("[NAME]", option.Name)));

                    if (option.Value && bool.Parse(falseRadio.GetAttribute("aria-checked")) || !option.Value && bool.Parse(trueRadio.GetAttribute("aria-checked")))
                    {
                        driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldRadioContainer].Replace("[NAME]", option.Name)));
                    }
                }
                else if (hasCheckbox)
                {
                    var checkbox = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldCheckbox].Replace("[NAME]", option.Name)));

                    if (option.Value && !checkbox.Selected || !option.Value && checkbox.Selected)
                    {
                        driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldCheckboxContainer].Replace("[NAME]", option.Name)));
                    }
                }
                else if (hasList)
                {
                    var list = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldList].Replace("[NAME]", option.Name)));
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
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");


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
        public BrowserCommandResult<bool> SetValue(string field, DateTime value, string formatDate = null, string formatTime = null)
        {
            var control = new DateTimeControl(field)
            {
                Value = value,
                DateFormat = formatDate,
                TimeFormat = formatTime
            };
            return SetValue(control);
        }

        public BrowserCommandResult<bool> SetValue(DateTimeControl control)
            => Execute(GetOptions($"Set Date/Time Value: {control.Name}"),
                driver => TrySetValue(driver, container: driver, control: control));

        private bool TrySetValue(IWebDriver driver, ISearchContext container, DateTimeControl control)
        {
            TrySetDateValue(driver, container, control);
            TrySetTime(driver, container, control);

            if (container is IWebElement parent)
                parent.Click(true);
            else
                driver.ClearFocus();

            return true;
        }

        private void TrySetDateValue(IWebDriver driver, ISearchContext container, DateTimeControl control)
        {
            string controlName = control.Name;
            var xpathToInput = By.XPath(AppElements.Xpath[AppReference.Entity.FieldControlDateTimeInputUCI].Replace("[FIELD]", controlName));
            var dateField = container.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");
            TrySetDateValue(driver, dateField, control.DateAsString);
        }

        private void TrySetDateValue(IWebDriver driver, IWebElement dateField, string date)
        {
            var strExpanded = dateField.GetAttribute("aria-expanded");
            bool success = bool.TryParse(strExpanded, out var isCalendarExpanded);
            if (success && isCalendarExpanded)
                dateField.Click(); // close calendar

            driver.RepeatUntil(() =>
                {
                    ClearFieldValue(dateField);
                    if (date != null)
                        dateField.SendKeys(date);
                },
                d => dateField.GetAttribute("value").IsValueEqualsTo(date),
                TimeSpan.FromSeconds(9), 3,
                failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {date}. Actual: {dateField.GetAttribute("value")}")
            );
        }

        private void ClearFieldValue(IWebElement field)
        {
            if (field.GetAttribute("value").Length > 0)
            {
                field.SendKeys(Keys.Control + "a");
                field.SendKeys(Keys.Backspace);
            }

            ThinkTime(500);
        }

        private static void TrySetTime(IWebDriver driver, ISearchContext container, DateTimeControl control)
        {
            By timeFieldXPath = By.XPath(AppElements.Xpath[AppReference.Entity.FieldControlDateTimeTimeInputUCI].Replace("[FIELD]", control.Name));
            var success = container.TryFindElement(timeFieldXPath, out var timeField);
            if (success)
                TrySetTime(driver, timeField, control.TimeAsString);
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
                },
                d => timeField.GetAttribute("value").IsValueEqualsTo(time),
                TimeSpan.FromSeconds(9), 3,
                failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {time}. Actual: {timeField.GetAttribute("value")}")
            );
        }


        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        /// <returns>True on success</returns>
        internal BrowserCommandResult<bool> SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Set MultiValueOptionSet Value: {option.Name}"), driver =>
            {
                if (removeExistingValues)
                {
                    RemoveMultiOptions(option);
                }
                else
                {
                    AddMultiOptions(option);
                }

                return true;
            });
        }

        /// <summary>
        /// Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be removed</param>
        /// <returns></returns>
        private BrowserCommandResult<bool> RemoveMultiOptions(MultiValueOptionSet option)
        {
            return this.Execute(GetOptions($"Remove Multi Select Value: {option.Name}"), driver =>
            {
                string xpath = AppElements.Xpath[AppReference.MultiSelect.SelectedRecord].Replace("[NAME]", Elements.ElementId[option.Name]);
                // If there is already some pre-selected items in the div then we must determine if it
                // actually exists and simulate a set focus event on that div so that the input textbox
                // becomes visible.
                var listItems = driver.FindElements(By.XPath(xpath));
                if (listItems.Any())
                {
                    listItems.First().SendKeys("");
                }

                // If there are large number of options selected then a small expand collapse 
                // button needs to be clicked to expose all the list elements.
                xpath = AppElements.Xpath[AppReference.MultiSelect.ExpandCollapseButton].Replace("[NAME]", Elements.ElementId[option.Name]);
                var expandCollapseButtons = driver.FindElements(By.XPath(xpath));
                if (expandCollapseButtons.Any())
                {
                    expandCollapseButtons.First().Click(true);
                }

                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.InputSearch].Replace("[NAME]", Elements.ElementId[option.Name])));
                foreach (var optionValue in option.Values)
                {
                    xpath = String.Format(AppElements.Xpath[AppReference.MultiSelect.SelectedRecordButton].Replace("[NAME]", Elements.ElementId[option.Name]), optionValue);
                    var listItemObjects = driver.FindElements(By.XPath(xpath));
                    var loopCounts = listItemObjects.Any() ? listItemObjects.Count : 0;

                    for (int i = 0; i < loopCounts; i++)
                    {
                        // With every click of the button, the underlying DOM changes and the
                        // entire collection becomes stale, hence we only click the first occurance of
                        // the button and loop back to again find the elements and anyother occurance
                        driver.FindElements(By.XPath(xpath)).First().Click(true);
                    }
                }

                return true;
            });
        }

        /// <summary>
        /// Sets the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set</param>
        /// <returns></returns>
        private BrowserCommandResult<bool> AddMultiOptions(MultiValueOptionSet option)
        {
            return this.Execute(GetOptions($"Add Multi Select Value: {option.Name}"), driver =>
            {
                string xpath = AppElements.Xpath[AppReference.MultiSelect.SelectedRecord].Replace("[NAME]", Elements.ElementId[option.Name]);
                // If there is already some pre-selected items in the div then we must determine if it
                // actually exists and simulate a set focus event on that div so that the input textbox
                // becomes visible.
                var listItems = driver.FindElements(By.XPath(xpath));
                if (listItems.Any())
                {
                    listItems.First().SendKeys("");
                }

                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.InputSearch].Replace("[NAME]", Elements.ElementId[option.Name])));
                foreach (var optionValue in option.Values)
                {
                    xpath = String.Format(AppElements.Xpath[AppReference.MultiSelect.FlyoutList].Replace("[NAME]", Elements.ElementId[option.Name]), optionValue);
                    var flyout = driver.FindElements(By.XPath(xpath));
                    if (flyout.Any())
                    {
                        flyout.First().Click(true);
                    }
                }

                // Click on the div containing textbox so that the floyout collapses or else the flyout
                // will interfere in finding the next multiselect control which by chance will be lying
                // behind the flyout control.
                //driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", Elements.ElementId[option.Name])));
                xpath = AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", Elements.ElementId[option.Name]);
                var divElements = driver.FindElements(By.XPath(xpath));
                if (divElements.Any())
                {
                    divElements.First().Click(true);
                }

                return true;
            });
        }

        internal BrowserCommandResult<Field> GetField(string field)
        {
            return this.Execute(GetOptions($"Get Field"), driver =>
            {
                Field returnField = new Field(driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field))));
                returnField.Name = field;

                return returnField;
            });
        }

        internal BrowserCommandResult<string> GetValue(string field)
        {
            return this.Execute(GetOptions($"Get Value"), driver =>
            {
                string text = string.Empty;
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));

                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    var input = fieldContainer.FindElement(By.TagName("input"));
                    if (input != null)
                    {
                        IWebElement fieldValue = input.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldValue].Replace("[NAME]", field)));
                        text = fieldValue.GetAttribute("value").ToString();

                        // Needed if getting a date field which also displays time as there isn't a date specifc GetValue method
                        var timefields = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Entity.FieldControlDateTimeTimeInputUCI].Replace("[FIELD]", field)));
                        if (timefields.Any())
                        {
                            text = $" {timefields.First().GetAttribute("value")}";
                        }
                    }
                }
                else if (fieldContainer.FindElements(By.TagName("textarea")).Count > 0)
                {
                    text = fieldContainer.FindElement(By.TagName("textarea")).GetAttribute("value");
                }
                else
                {
                    throw new Exception($"Field with name {field} does not exist.");
                }

                return text;
            });
        }

        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new Lookup { Name = "primarycontactid" });</example>
        public BrowserCommandResult<string> GetValue(LookupItem control)
        {
            var controlName = control.Name;
            return Execute($"Get Lookup Value: {controlName}", driver =>
            {
                var xpathToContainer = AppElements.Xpath[AppReference.Entity.TextFieldLookupFieldContainer].Replace("[NAME]", controlName);
                IWebElement fieldContainer = driver.WaitUntilAvailable(By.XPath(xpathToContainer));
                string lookupValue = TryGetValue(fieldContainer, control);
                return lookupValue;
            });
        }

        private static string TryGetValue(IWebElement fieldContainer, LookupItem control)
        {
            Exception ex = null;
            try
            {
                bool found = fieldContainer.TryFindElement(By.TagName("input"), out var input);
                if (found)
                {
                    string lookupValue = input.GetAttribute("value");
                    return lookupValue;
                }

                found = fieldContainer.TryFindElement(By.XPath(".//label"), out var label);
                if (found)
                {
                    string lookupValue = label.GetAttribute("innerText");
                    return lookupValue;
                }
            }
            catch (Exception e)
            {
                ex = e;
            }

            throw new InvalidOperationException($"Field: {control.Name} Does not exist", ex);
        }

        /// <summary>
        /// Gets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The lookup field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new LookupItem[] { new LookupItem { Name = "to" } });</example>
        public BrowserCommandResult<string[]> GetValue(LookupItem[] controls)
        {
            var controlName = controls.First().Name;
            return Execute($"Get ActivityParty Lookup Value: {controlName}", driver =>
            {
                var xpathToContainer = By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupFieldContainer].Replace("[NAME]", controlName));
                var fieldContainer = driver.WaitUntilAvailable(xpathToContainer);
                string[] lookupValues = TryGetValue(fieldContainer, controls);
                return lookupValues;
            });
        }

        public BrowserCommandResult<string[]> TryGetValue(IWebElement fieldContainer, LookupItem[] controls)
        {
            var controlName = controls.First().Name;
            var xpathToExistingValues = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldExistingValue].Replace("[NAME]", controlName));
            var existingValues = fieldContainer.FindElements(xpathToExistingValues);

            var xpathToExpandButton = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldExpandCollapseButton].Replace("[NAME]", controlName));
            bool expandButtonFound = fieldContainer.TryFindElement(xpathToExpandButton, out var collapseButton);
            if (expandButtonFound)
            {
                collapseButton.Click(true);

                int count = existingValues.Count;
                fieldContainer.WaitUntil(fc => fc.FindElements(xpathToExistingValues).Count > count);

                existingValues = fieldContainer.FindElements(xpathToExistingValues);
            }

            Exception ex = null;
            try
            {
                if (existingValues.Count > 0)
                {
                    char[] trimCharacters =
                    {
                        '', '\r', '\n', '',
                        '', ''
                    }; //IE can return line breaks
                    string[] lookupValues = existingValues.Select(v => v.GetAttribute("innerText").Trim(trimCharacters)).ToArray();
                    return lookupValues;
                }

                if (fieldContainer.FindElements(By.TagName("input")).Any())
                    return null;
            }
            catch (Exception e)
            {
                ex = e;
            }

            throw new InvalidOperationException($"Field: {controlName} Does not exist", ex);
        }

        /// <summary>
        /// Gets the value of a picklist or status field.
        /// </summary>
        /// <param name="control">The option you want to set.</param>
        /// <example>xrmApp.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        internal BrowserCommandResult<string> GetValue(OptionSet control)
        {
            var controlName = control.Name;
            return this.Execute($"Get OptionSet Value: {controlName}", driver =>
            {
                var xpathToFieldContainer = AppElements.Xpath[AppReference.Entity.OptionSetFieldContainer].Replace("[NAME]", controlName);
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(xpathToFieldContainer));
                string result = TryGetValue(fieldContainer, control);
                return result;
            });
        }

        private static string TryGetValue(IWebElement fieldContainer, OptionSet control)
        {
            bool success = fieldContainer.TryFindElement(By.TagName("select"), out IWebElement select);
            if (success)
            {
                var options = select.FindElements(By.TagName("option"));
                string result = GetSelectedOption(options);
                return result;
            }

            var name = control.Name;
            var hasStatusCombo = fieldContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusCombo].Replace("[NAME]", name)));
            if (hasStatusCombo)
            {
                // This is for statuscode (type = status) that should act like an optionset doesn't doesn't follow the same pattern when rendered
                var valueSpan = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusTextValue].Replace("[NAME]", name)));
                return valueSpan.Text;
            }

            throw new InvalidOperationException($"OptionSet Field: '{name}' does not exist");
        }

        private static string GetSelectedOption(ReadOnlyCollection<IWebElement> options)
        {
            var selectedOption = options.FirstOrDefault(op => op.Selected);
            return selectedOption?.Text ?? string.Empty;
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        /// <example>xrmApp.Entity.GetValue(new BooleanItem { Name = "creditonhold" });</example>
        internal BrowserCommandResult<bool> GetValue(BooleanItem option)
        {
            return this.Execute($"Get BooleanItem Value: {option.Name}", driver =>
            {
                var check = false;

                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", option.Name)));

                var hasRadio = fieldContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldRadioContainer].Replace("[NAME]", option.Name)));
                var hasCheckbox = fieldContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldCheckbox].Replace("[NAME]", option.Name)));
                var hasList = fieldContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldList].Replace("[NAME]", option.Name)));

                if (hasRadio)
                {
                    var trueRadio = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldRadioTrue].Replace("[NAME]", option.Name)));

                    check = bool.Parse(trueRadio.GetAttribute("aria-checked"));
                }
                else if (hasCheckbox)
                {
                    var checkbox = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldCheckbox].Replace("[NAME]", option.Name)));

                    check = bool.Parse(checkbox.GetAttribute("aria-checked"));
                }
                else if (hasList)
                {
                    var list = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityBooleanFieldList].Replace("[NAME]", option.Name)));
                    var options = list.FindElements(By.TagName("option"));
                    var selectedOption = options.FirstOrDefault(a => a.HasAttribute("data-selected") && bool.Parse(a.GetAttribute("data-selected")));

                    if (selectedOption != null)
                    {
                        check = int.Parse(selectedOption.GetAttribute("value")) == 1;
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                return check;
            });
        }

        /// <summary>
        /// Gets the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field</param>
        /// <returns>MultiValueOptionSet object where the values field contains all the contact names</returns>
        internal BrowserCommandResult<MultiValueOptionSet> GetValue(MultiValueOptionSet option)
        {
            return this.Execute(GetOptions($"Get Multi Select Value: {option.Name}"), driver =>
            {
                // If there are large number of options selected then a small expand collapse 
                // button needs to be clicked to expose all the list elements.
                string xpath = AppElements.Xpath[AppReference.MultiSelect.ExpandCollapseButton].Replace("[NAME]", Elements.ElementId[option.Name]);
                var expandCollapseButtons = driver.FindElements(By.XPath(xpath));
                if (expandCollapseButtons.Any())
                {
                    expandCollapseButtons.First().Click(true);
                }

                var returnValue = new MultiValueOptionSet {Name = option.Name};

                xpath = AppElements.Xpath[AppReference.MultiSelect.SelectedRecordLabel].Replace("[NAME]", Elements.ElementId[option.Name]);
                var labelItems = driver.FindElements(By.XPath(xpath));
                if (labelItems.Any())
                {
                    returnValue.Values = labelItems.Select(x => x.Text).ToArray();
                }

                return returnValue;
            });
        }


        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new DateTimeControl { Name = "scheduledstart" });</example>
        public BrowserCommandResult<DateTime?> GetValue(DateTimeControl control)
            => Execute($"Get DateTime Value: {control.Name}", driver => TryGetValue(driver, container: driver, control: control));

        private static DateTime? TryGetValue(IWebDriver driver, ISearchContext container, DateTimeControl control)
        {
            string field = control.Name;
            driver.WaitForTransaction();

            var xpathToDateField = By.XPath(AppElements.Xpath[AppReference.Entity.FieldControlDateTimeInputUCI].Replace("[FIELD]", field));

            var dateField = container.WaitUntilAvailable(xpathToDateField, $"Field: {field} Does not exist");
            string strDate = dateField.GetAttribute("value");
            if (strDate.IsEmptyValue())
                return null;

            var date = DateTime.Parse(strDate);

            // Try get Time
            var timeFieldXPath = By.XPath(AppElements.Xpath[AppReference.Entity.FieldControlDateTimeTimeInputUCI].Replace("[FIELD]", field));
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

        /// <summary>
        /// Returns the ObjectId of the entity
        /// </summary>
        /// <returns>Guid of the Entity</returns>
        internal BrowserCommandResult<Guid> GetObjectId(int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions($"Get Object Id"), driver =>
            {
                var objectId = driver.ExecuteScript("return Xrm.Page.data.entity.getId();");

                Guid oId;
                if (!Guid.TryParse(objectId.ToString(), out oId))
                    throw new NotFoundException("Unable to retrieve object Id for this entity");

                return oId;
            });
        }

        /// <summary>
        /// Returns the Entity Name of the entity
        /// </summary>
        /// <returns>Entity Name of the Entity</returns>
        internal BrowserCommandResult<string> GetEntityName(int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions($"Get Entity Name"), driver =>
            {
                var entityName = driver.ExecuteScript("return Xrm.Page.data.entity.getEntityName();").ToString();

                if (string.IsNullOrEmpty(entityName))
                {
                    throw new NotFoundException("Unable to retrieve Entity Name for this entity");
                }

                return entityName;
            });
        }

        internal BrowserCommandResult<List<GridItem>> GetSubGridItems(string subgridName)
        {
            return this.Execute(GetOptions($"Get Subgrid Items for Subgrid {subgridName}"), driver =>
            {
                List<GridItem> subGridRows = new List<GridItem>();

                if (!driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridTitle].Replace("[NAME]", subgridName))))
                    throw new NotFoundException($"{subgridName} subgrid not found. Subgrid names are case sensitive.  Please make sure casing is the same.");

                //Find the subgrid contents
                var subGrid = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridContents].Replace("[NAME]", subgridName)));

                //Find the columns
                List<string> columns = new List<string>();

                var headerCells = subGrid.FindElements(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridHeaders]));

                foreach (IWebElement headerCell in headerCells)
                {
                    columns.Add(headerCell.Text);
                }

                //Find the rows
                var rows = subGrid.FindElements(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridRows]));

                //Process each row
                foreach (IWebElement row in rows)
                {
                    List<string> cellValues = new List<string>();
                    GridItem item = new GridItem();

                    //Get the entityId and entity Type
                    if (row.GetAttribute("data-lp-id") != null)
                    {
                        var rowAttributes = row.GetAttribute("data-lp-id").Split('|');
                        item.Id = Guid.Parse(rowAttributes[3]);
                        item.EntityName = rowAttributes[4];
                    }

                    var cells = row.FindElements(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridCells]));

                    if (cells.Count > 0)
                    {
                        foreach (IWebElement thisCell in cells)
                            cellValues.Add(thisCell.Text);

                        for (int i = 0; i < columns.Count; i++)
                        {
                            //The first cell is always a checkbox for the record.  Ignore the checkbox.
                            item[columns[i]] = cellValues[i + 1];
                        }

                        subGridRows.Add(item);
                    }
                }

                return subGridRows;
            });
        }

        internal BrowserCommandResult<bool> OpenSubGridRecord(string subgridName, int index = 0)
        {
            return this.Execute(GetOptions($"Open Subgrid record for subgrid {subgridName}"), driver =>
            {
                //Find the Grid
                var subGrid = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridContents].Replace("[NAME]", subgridName)));

                //Get the GridName
                string subGridName = subGrid.GetAttribute("data-id").Replace("dataSetRoot_", String.Empty);

                //cell-0 is the checkbox for each record
                var checkBox = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridRecordCheckbox].Replace("[INDEX]", index.ToString()).Replace("[NAME]", subGridName)));

                driver.DoubleClick(checkBox);

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<int> GetSubGridItemsCount(string subgridName)
        {
            return this.Execute(GetOptions($"Get Subgrid Items Count for subgrid {subgridName}"), driver =>
            {
                List<GridItem> rows = GetSubGridItems(subgridName);
                return rows.Count;
            });
        }

        /// <summary>
        /// Click the magnifying glass icon for the lookup control supplied
        /// </summary>
        /// <param name="control">The LookupItem field on the form</param>
        /// <returns></returns>
        internal BrowserCommandResult<bool> SelectLookup(LookupItem control)
        {
            return this.Execute(GetOptions($"Select Lookup Field {control.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.FieldLookupButton].Replace("[NAME]", control.Name))))
                {
                    var lookupButton = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.FieldLookupButton].Replace("[NAME]", control.Name)));

                    lookupButton.Hover(driver);

                    driver.WaitForTransaction();

                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.SearchButtonIcon])).Click(true);
                }
                else
                    throw new NotFoundException($"Lookup field {control.Name} not found");

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<string> GetHeaderValue(LookupItem control)
        {
            var controlName = control.Name;
            return Execute(GetOptions($"Get Header LookupItem Value {controlName}"), driver =>
            {
                var xpathToContainer = AppElements.Xpath[AppReference.Entity.Header.LookupFieldContainer].Replace("[NAME]", controlName);
                string lookupValue = ExecuteInHeaderContainer(driver, xpathToContainer, container => TryGetValue(container, control));
                return lookupValue;
            });
        }

        internal BrowserCommandResult<string[]> GetHeaderValue(LookupItem[] controls)
        {
            var controlName = controls.First().Name;
            var xpathToContainer = AppElements.Xpath[AppReference.Entity.Header.LookupFieldContainer].Replace("[NAME]", controlName);
            return Execute(GetOptions($"Get Header Activityparty LookupItem Value {controlName}"), driver =>
            {
                string[] lookupValues = ExecuteInHeaderContainer(driver, xpathToContainer, container => TryGetValue(container, controls));
                return lookupValues;
            });
        }

        internal BrowserCommandResult<string> GetHeaderValue(string control)
        {
            return this.Execute(GetOptions($"Get Header Value {control}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                return GetValue(control);
            });
        }

        internal BrowserCommandResult<MultiValueOptionSet> GetHeaderValue(MultiValueOptionSet control)
        {
            return this.Execute(GetOptions($"Get Header MultiValueOptionSet Value {control.Name}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                return GetValue(control);
            });
        }

        internal BrowserCommandResult<string> GetHeaderValue(OptionSet control)
        {
            var controlName = control.Name;
            var xpathToContainer = AppElements.Xpath[AppReference.Entity.Header.OptionSetFieldContainer].Replace("[NAME]", controlName);
            return Execute(GetOptions($"Get Header OptionSet Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer, container => TryGetValue(container, control))
            );
        }

        internal BrowserCommandResult<bool> GetHeaderValue(BooleanItem control)
        {
            return this.Execute(GetOptions($"Get Header BooleanItem Value {control}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                return GetValue(control);
            });
        }

        internal BrowserCommandResult<DateTime?> GetHeaderValue(DateTimeControl control)
        {
            var xpathToContainer = AppElements.Xpath[AppReference.Entity.Header.DateTimeFieldContainer].Replace("[NAME]", control.Name);
            return Execute(GetOptions($"Get Header DateTime Value {control.Name}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    container => TryGetValue(driver, container, control)));
        }

        internal BrowserCommandResult<string> GetStatusFromFooter()
        {
            return this.Execute(GetOptions($"Get Status value from footer"), driver =>
            {
                if (!driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityFooter])))
                    throw new NotFoundException("Unable to find footer on the form");

                var footer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityFooter]));

                var status = footer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.FooterStatusValue]));

                if (String.IsNullOrEmpty(status.Text))
                    return "unknown";

                return status.Text;
            });
        }

        internal BrowserCommandResult<bool> SetHeaderValue(string field, string value)
        {
            return this.Execute(GetOptions($"Set Header Value {field}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                SetValue(field, value);

                return true;
            });
        }

        internal BrowserCommandResult<bool> SetHeaderValue(LookupItem control, int index = 0)
        {
            var controlName = control.Name;
            var xpathToContainer = AppElements.Xpath[AppReference.Entity.Header.LookupFieldContainer].Replace("[NAME]", controlName);
            return Execute(GetOptions($"Set Header LookupItem Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    fieldContainer =>
                    {
                        TryRemoveLookupValue(driver, fieldContainer, control);
                        TrySetValue(fieldContainer, control, index);
                        return true;
                    }));
        }

        internal BrowserCommandResult<bool> SetHeaderValue(LookupItem[] controls, int index = 0, bool clearFirst = true)
        {
            var control = controls.First();
            var controlName = control.Name;
            var xpathToContainer = AppElements.Xpath[AppReference.Entity.Header.LookupFieldContainer].Replace("[NAME]", controlName);
            return Execute(GetOptions($"Set Header Activityparty LookupItem Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    container =>
                    {
                        if (clearFirst)
                            TryRemoveLookupValue(driver, container, control);

                        TryToSetValue(container, controls, index);
                        return true;
                    }));
        }

        internal BrowserCommandResult<bool> SetHeaderValue(OptionSet control)
        {
            var controlName = control.Name;
            var xpathToContainer = AppElements.Xpath[AppReference.Entity.Header.OptionSetFieldContainer].Replace("[NAME]", controlName);
            return Execute(GetOptions($"Set Header OptionSet Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    container =>
                    {
                        TrySetValue(container, control);
                        return true;
                    }));
        }

        internal BrowserCommandResult<bool> SetHeaderValue(MultiValueOptionSet control)
        {
            return this.Execute(GetOptions($"Set Header MultiValueOptionSet Value {control.Name}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                SetValue(control);

                return true;
            });
        }

        internal BrowserCommandResult<bool> SetHeaderValue(BooleanItem control)
        {
            return this.Execute(GetOptions($"Set Header BooleanItem Value {control.Name}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                SetValue(control);

                return true;
            });
        }

        internal BrowserCommandResult<bool> SetHeaderValue(string field, DateTime value, string formatDate = null, string formatTime = null)
        {
            var control = new DateTimeControl(field)
            {
                Value = value,
                DateFormat = formatDate,
                TimeFormat = formatTime
            };
            return SetHeaderValue(control);
        }

        internal BrowserCommandResult<bool> SetHeaderValue(DateTimeControl control)
            => Execute(GetOptions($"Set Header Date/Time Value: {control.Name}"), driver => TrySetHeaderValue(driver, control));

        internal BrowserCommandResult<bool> ClearHeaderValue(DateTimeControl control)
        {
            var controlName = control.Name;
            return Execute(GetOptions($"Clear Header Date/Time Value: {controlName}"),
                driver => TrySetHeaderValue(driver, new DateTimeControl(controlName)));
        }

        private bool TrySetHeaderValue(IWebDriver driver, DateTimeControl control)
        {
            var xpathToContainer = AppElements.Xpath[AppReference.Entity.Header.DateTimeFieldContainer].Replace("[NAME]", control.Name);
            return ExecuteInHeaderContainer(driver, xpathToContainer,
                container => TrySetValue(driver, container, control));
        }

        internal BrowserCommandResult<bool> ClearValue(DateTimeControl control)
            => Execute(GetOptions($"Clear Field: {control.Name}"),
                driver => TrySetValue(driver, container: driver, control: new DateTimeControl(control.Name))); // Pass an empty control

        internal BrowserCommandResult<bool> ClearValue(string fieldName)
        {
            return this.Execute(GetOptions($"Clear Field {fieldName}"), driver =>
            {
                SetValue(fieldName, string.Empty);

                return true;
            });
        }

        internal BrowserCommandResult<bool> ClearValue(LookupItem control, bool removeAll = true)
        {
            var controlName = control.Name;
            return Execute(GetOptions($"Clear Field {controlName}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupFieldContainer].Replace("[NAME]", controlName)));
                TryRemoveLookupValue(driver, fieldContainer, control, removeAll);
                return true;
            });
        }

        private static void TryRemoveLookupValue(IWebDriver driver, IWebElement fieldContainer, LookupItem control, bool removeAll = true)
        {
            var controlName = control.Name;
            fieldContainer.Hover(driver);

            var xpathDeleteExistingValues = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldDeleteExistingValue].Replace("[NAME]", controlName));
            var existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);

            var xpathToExpandButton = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldExpandCollapseButton].Replace("[NAME]", controlName));
            bool success = fieldContainer.TryFindElement(xpathToExpandButton, out var expandButton);
            if (success)
            {
                expandButton.Click(true);

                var count = existingValues.Count;
                fieldContainer.WaitUntil(x => x.FindElements(xpathDeleteExistingValues).Count > count);
            }
            else
            {
                var xpathToHoveExistingValue = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldHoverExistingValue].Replace("[NAME]", controlName));
                var found = fieldContainer.TryFindElement(xpathToHoveExistingValue, out var existingList);
                if (found)
                    existingList.SendKeys(Keys.Clear);
            }

            fieldContainer.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupSearchButton].Replace("[NAME]", controlName)));

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
        }

        internal BrowserCommandResult<bool> ClearValue(OptionSet control)
        {
            return this.Execute(GetOptions($"Clear Field {control.Name}"), driver =>
            {
                control.Value = "-1";
                SetValue(control);

                return true;
            });
        }

        internal BrowserCommandResult<bool> ClearValue(MultiValueOptionSet control)
        {
            return this.Execute(GetOptions($"Clear Field {control.Name}"), driver =>
            {
                RemoveMultiOptions(control);

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectForm(string formName)
        {
            return this.Execute(GetOptions($"Select Form {formName}"), driver =>
            {
                driver.WaitForTransaction();

                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.FormSelector])))
                    throw new NotFoundException("Unable to find form selector on the form");

                var formSelector = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.FormSelector]));
                // Click didn't work with IE
                formSelector.SendKeys(Keys.Enter);

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Entity.FormSelectorFlyout]));

                var flyout = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.FormSelectorFlyout]));
                var forms = flyout.FindElements(By.XPath(Elements.Xpath[Reference.Entity.FormSelectorItem]));

                var form = forms.FirstOrDefault(a => a.GetAttribute("data-text").EndsWith(formName, StringComparison.OrdinalIgnoreCase));
                if (form == null)
                    throw new NotFoundException($"Form {formName} is not in the form selector");

                driver.ClickWhenAvailable(By.Id(form.GetAttribute("id")));

                driver.WaitForPageToLoad();

                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Entity.Form]),
                    TimeSpan.FromSeconds(30),
                    "CRM Record is Unavailable or not finished loading. Timeout Exceeded"
                );
                return true;
            });
        }

        internal BrowserCommandResult<bool> AddValues(LookupItem[] controls, int index = 0)
        {
            return this.Execute(GetOptions($"Add values {controls.First().Name}"), driver =>
            {
                SetValue(controls, index, false);

                return true;
            });
        }

        internal BrowserCommandResult<bool> RemoveValues(LookupItem[] controls)
        {
            return this.Execute(GetOptions($"Remove values {controls.First().Name}"), driver =>
            {
                foreach (var control in controls)
                {
                    ClearValue(control, false);
                }

                return true;
            });
        }

        internal TResult ExecuteInHeaderContainer<TResult>(IWebDriver driver, string xpathToContainer, Func<IWebElement, TResult> function)
        {
            TResult lookupValue = default(TResult);

            TryExpandHeaderFlyout(driver);

            var xpathToFlyout = AppElements.Xpath[AppReference.Entity.Header.Flyout];
            driver.WaitUntilVisible(By.XPath(xpathToFlyout), TimeSpan.FromSeconds(5),
                flyout =>
                {
                    IWebElement container = flyout.FindElement(By.XPath(xpathToContainer));
                    lookupValue = function(container);
                });

            return lookupValue;
        }

        internal void TryExpandHeaderFlyout(IWebDriver driver)
        {
            bool hasHeader = driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.Header.Container]));
            if (!hasHeader)
                throw new NotFoundException("Unable to find header on the form");

            var xPath = By.XPath(AppElements.Xpath[AppReference.Entity.Header.FlyoutButton]);
            var headerFlyoutButton = driver.FindElement(xPath);
            bool expanded = bool.Parse(headerFlyoutButton.GetAttribute("aria-expanded"));

            if (!expanded)
                headerFlyoutButton.Click(true);
        }

        #endregion

        #region Lookup 

        internal BrowserCommandResult<bool> OpenLookupRecord(int index)
        {
            return this.Execute(GetOptions("Select Lookup Record"), driver =>
            {
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Lookup.LookupResultRows])))
                {
                    var rows = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Lookup.LookupResultRows]));

                    if (rows.Count > 0)
                        rows.FirstOrDefault().Click(true);
                }
                else
                    throw new NotFoundException("No rows found");

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SearchLookupField(LookupItem control, string searchCriteria)
        {
            return this.Execute(GetOptions("Search Lookup Record"), driver =>
            {
                //Click in the field and enter values
                control.Value = searchCriteria;
                SetValue(control);

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupRelatedEntity(string entityName)
        {
            //Click the Related Entity on the Lookup Flyout
            return this.Execute(GetOptions($"Select Lookup Related Entity {entityName}"), driver =>
            {
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Lookup.RelatedEntityLabel].Replace("[NAME]", entityName))))
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Lookup.RelatedEntityLabel].Replace("[NAME]", entityName))).Click(true);
                else
                    throw new NotFoundException($"Lookup Entity {entityName} not found");

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SwitchLookupView(string viewName)
        {
            return Execute(GetOptions($"Select Lookup View {viewName}"), driver =>
            {
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Lookup.ChangeViewButton])))
                {
                    //Click Change View 
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Lookup.ChangeViewButton])).Click(true);

                    driver.WaitForTransaction();

                    //Click View Requested 
                    var rows = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Lookup.ViewRows]));
                    if (rows.Any(x => x.Text.Equals(viewName, StringComparison.OrdinalIgnoreCase)))
                        rows.First(x => x.Text.Equals(viewName, StringComparison.OrdinalIgnoreCase)).Click(true);
                    else
                        throw new NotFoundException($"View {viewName} not found");
                }

                else
                    throw new NotFoundException("Lookup menu not visible");

                driver.WaitForTransaction();
                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupNewButton()
        {
            return this.Execute(GetOptions("Click New Lookup Button"), driver =>
            {
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

                driver.WaitForTransaction();

                return true;
            });
        }

        #endregion

        #region Timeline

        /// <summary>
        /// This method opens the popout menus in the Dynamics 365 pages. 
        /// This method uses a thinktime since after the page loads, it takes some time for the 
        /// widgets to load before the method can find and popout the menu.
        /// </summary>
        /// <param name="popoutName">The By Object of the Popout menu</param>
        /// <param name="popoutItemName">The By Object of the Popout Item name in the popout menu</param>
        /// <param name="thinkTime">Amount of time(milliseconds) to wait before this method will click on the "+" popout menu.</param>
        /// <returns>True on success, False on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(By menuName, By menuItemName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute($"Open menu", driver =>
            {
                driver.ClickWhenAvailable(menuName);
                try
                {
                    driver.ClickWhenAvailable(menuItemName);
                }
                catch
                {
                    // Element is stale reference is thrown here since the HTML components 
                    // get destroyed and thus leaving the references null. 
                    // It is expected that the components will be destroyed and the next 
                    // action should take place after it and hence it is ignored.
                    return false;
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> Delete(int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Delete Entity"), driver =>
            {
                var deleteBtn = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.Delete]),
                    "Delete Button is not available");

                deleteBtn?.Click();
                ConfirmationDialog(true);

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> Assign(string userOrTeamToAssign, int thinkTime = Constants.DefaultThinkTime)
        {
            //Click the Assign Button on the Entity Record
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Assign Entity"), driver =>
            {
                var assignBtn = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.Assign]),
                    "Assign Button is not available");

                assignBtn?.Click();
                AssignDialog(Dialogs.AssignTo.User, userOrTeamToAssign);

                return true;
            });
        }

        internal BrowserCommandResult<bool> SwitchProcess(string processToSwitchTo, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Switch BusinessProcessFlow"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.ProcessButton]), TimeSpan.FromSeconds(5));

                driver.ClickWhenAvailable(
                    By.XPath(AppElements.Xpath[AppReference.Entity.SwitchProcess]),
                    TimeSpan.FromSeconds(5),
                    "The Switch Process Button is not available."
                );

                return true;
            });
        }

        internal BrowserCommandResult<bool> CloseOpportunity(bool closeAsWon, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            var xPathQuery = closeAsWon
                ? AppElements.Xpath[AppReference.Entity.CloseOpportunityWin]
                : AppElements.Xpath[AppReference.Entity.CloseOpportunityLoss];

            return this.Execute(GetOptions($"Close Opportunity"), driver =>
            {
                var closeBtn = driver.WaitUntilAvailable(By.XPath(xPathQuery), "Opportunity Close Button is not available");

                closeBtn?.Click();
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]));
                CloseOpportunityDialog(true);

                return true;
            });
        }

        internal BrowserCommandResult<bool> CloseOpportunity(double revenue, DateTime closeDate, string description, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Close Opportunity"), driver =>
            {
                SetValue(Elements.ElementId[AppReference.Dialogs.CloseOpportunity.ActualRevenueId], revenue.ToString(CultureInfo.CurrentCulture));
                SetValue(Elements.ElementId[AppReference.Dialogs.CloseOpportunity.CloseDateId], closeDate);
                SetValue(Elements.ElementId[AppReference.Dialogs.CloseOpportunity.DescriptionId], description);

                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]),
                    TimeSpan.FromSeconds(5),
                    "The Close Opportunity dialog is not available."
                );

                return true;
            });
        }

        /// <summary>
        /// This method opens the popout menus in the Dynamics 365 pages. 
        /// This method uses a thinktime since after the page loads, it takes some time for the 
        /// widgets to load before the method can find and popout the menu.
        /// </summary>
        /// <param name="popoutName">The name of the Popout menu</param>
        /// <param name="popoutItemName">The name of the Popout Item name in the popout menu</param>
        /// <param name="thinkTime">Amount of time(milliseconds) to wait before this method will click on the "+" popout menu.</param>
        /// <returns>True on success, False on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(string popoutName, string popoutItemName, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.OpenAndClickPopoutMenu(By.XPath(Elements.Xpath[popoutName]), By.XPath(Elements.Xpath[popoutItemName]), thinkTime);
        }


        /// <summary>
        /// Provided a By object which represents a HTML Button object, this method will
        /// find it and click it.
        /// </summary>
        /// <param name="by">The object of Type By which represents a HTML Button object</param>
        /// <returns>True on success, False/Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> ClickButton(By by)
        {
            return this.Execute($"Open Timeline Add Post Popout", driver =>
            {
                var button = driver.WaitUntilAvailable(by);
                if (button.TagName.Equals("button"))
                {
                    try
                    {
                        driver.ClickWhenAvailable(by);
                    }
                    catch
                    {
                        // Element is stale reference is thrown here since the HTML components 
                        // get destroyed and thus leaving the references null. 
                        // It is expected that the components will be destroyed and the next 
                        // action should take place after it and hence it is ignored.
                    }

                    return true;
                }
                else if (button.FindElements(By.TagName("button")).Any())
                {
                    button.FindElements(By.TagName("button")).First().Click();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"Control does not exist");
                }
            });
        }

        /// <summary>
        /// Provided a fieldname as a XPath which represents a HTML Button object, this method will
        /// find it and click it.
        /// </summary>
        /// <param name="fieldNameXpath">The field as a XPath which represents a HTML Button object</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> ClickButton(string fieldNameXpath)
        {
            try
            {
                return ClickButton(By.XPath(fieldNameXpath));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Field: {fieldNameXpath} with Does not exist", e);
            }
        }

        /// <summary>
        /// Generic method to help click on any item which is clickable or uniquely discoverable with a By object.
        /// </summary>
        /// <param name="by">The xpath of the HTML item as a By object</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> SelectTab(string tabName, string subTabName = "", int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute($"Select Tab", driver =>
            {
                IWebElement tabList = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TabList]));

                ClickTab(tabList, AppElements.Xpath[AppReference.Entity.Tab], tabName);

                //Click Sub Tab if provided
                if (!String.IsNullOrEmpty(subTabName))
                {
                    ClickTab(tabList, AppElements.Xpath[AppReference.Entity.SubTab], subTabName);
                }

                driver.WaitForTransaction();
                return true;
            });
        }

        internal void ClickTab(IWebElement tabList, string xpath, string name)
        {
            IWebElement moreTabsButton;
            IWebElement listItem;
            // Look for the tab in the tab list, else in the more tabs menu
            IWebElement searchScope = null;
            if (tabList.HasElement(By.XPath(string.Format(xpath, name))))
            {
                searchScope = tabList;
            }
            else if (tabList.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Entity.MoreTabs]), out moreTabsButton))
            {
                moreTabsButton.Click();
                searchScope = Browser.Driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.MoreTabsMenu]));
            }


            if (searchScope.TryFindElement(By.XPath(string.Format(xpath, name)), out listItem))
            {
                listItem.Click(true);
            }
            else
            {
                throw new Exception($"The tab with name: {name} does not exist");
            }
        }

        /// <summary>
        /// A generic setter method which will find the HTML Textbox/Textarea object and populate
        /// it with the value provided. The expected tag name is to make sure that it hits
        /// the expected tag and not some other object with the similar fieldname.
        /// </summary>
        /// <param name="fieldName">The name of the field representing the HTML Textbox/Textarea object</param>
        /// <param name="value">The string value which will be populated in the HTML Textbox/Textarea</param>
        /// <param name="expectedTagName">Expected values - textbox/textarea</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> SetValue(string fieldName, string value, string expectedTagName)
        {
            return this.Execute($"SetValue (Generic)", driver =>
            {
                var inputbox = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[fieldName]));
                if (expectedTagName.Equals(inputbox.TagName, StringComparison.InvariantCultureIgnoreCase))
                {
                    inputbox.Click();
                    inputbox.Clear();
                    inputbox.SendKeys(value);
                    return true;
                }

                throw new InvalidOperationException($"Field: {fieldName} with tagname {expectedTagName} Does not exist");
            });
        }

        #endregion

        #region BusinessProcessFlow

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.BusinessProcessFlow.SetValue("firstname", "Test");</example>
        internal BrowserCommandResult<bool> BPFSetValue(string field, string value)
        {
            return this.Execute(GetOptions($"Set BPF Value"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", field)));

                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    var input = fieldContainer.FindElement(By.TagName("input"));
                    if (input != null)
                    {
                        input.Click(true);
                        input.Clear();
                        input.SendKeys(value, true);
                        input.SendKeys(Keys.Tab);
                    }
                }
                else if (fieldContainer.FindElements(By.TagName("textarea")).Count > 0)
                {
                    var textarea = fieldContainer.FindElement(By.TagName("textarea"));
                    textarea.Click();
                    textarea.Clear();
                    textarea.SendKeys(value);
                }
                else
                {
                    throw new Exception($"Field with name {field} does not exist.");
                }

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public BrowserCommandResult<bool> BPFSetValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Set BPF Value: {option.Name}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", option.Name)));

                if (fieldContainer.FindElements(By.TagName("select")).Count > 0)
                {
                    var select = fieldContainer.FindElement(By.TagName("select"));
                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.Text != option.Value && op.GetAttribute("value") != option.Value) continue;
                        op.Click(true);
                        break;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");
                }

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue(new BooleanItem { Name = "preferredcontactmethodcode"});</example>
        public BrowserCommandResult<bool> BPFSetValue(BooleanItem option)
        {
            return this.Execute(GetOptions($"Set BPF Value: {option.Name}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.BooleanFieldContainer].Replace("[NAME]", option.Name)));
                if (!option.Value)
                {
                    if (!fieldContainer.Selected)
                    {
                        fieldContainer.Click(true);
                    }
                    else
                    {
                        fieldContainer.Click(true);
                    }
                }
                else
                {
                    if (fieldContainer.Selected)
                    {
                        fieldContainer.Click(true);
                    }
                    else
                    {
                        fieldContainer.Click(true);
                    }
                }

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="format">DateTime format</param>
        /// <example> xrmBrowser.BusinessProcessFlow.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        public BrowserCommandResult<bool> BPFSetValue(string field, DateTime date, string format = "MM dd yyyy")
        {
            return this.Execute(GetOptions($"Set BPF Value: {field}"), driver =>
            {
                var dateField = AppElements.Xpath[AppReference.BusinessProcessFlow.DateTimeFieldContainer].Replace("[FIELD]", field);

                if (driver.HasElement(By.XPath(dateField)))
                {
                    var fieldElement = driver.ClickWhenAvailable(By.XPath(dateField));

                    if (fieldElement.GetAttribute("value").Length > 0)
                    {
                        //fieldElement.Click();
                        //fieldElement.SendKeys(date.ToString(format));
                        //fieldElement.SendKeys(Keys.Enter);

                        fieldElement.Click();
                        ThinkTime(250);
                        fieldElement.Click();
                        ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        ThinkTime(250);
                        fieldElement.SendKeys(date.ToString(format), true);
                        ThinkTime(500);
                        fieldElement.SendKeys(Keys.Tab);
                        ThinkTime(250);
                    }
                    else
                    {
                        fieldElement.Click();
                        ThinkTime(250);
                        fieldElement.Click();
                        ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        ThinkTime(250);
                        fieldElement.SendKeys(date.ToString(format));
                        ThinkTime(250);
                        fieldElement.SendKeys(Keys.Tab);
                        ThinkTime(250);
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        internal BrowserCommandResult<bool> NextStage(string stageName, Field businessProcessFlowField = null, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Next Stage"), driver =>
            {
                //Find the Business Process Stages
                var processStages = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.NextStage_UCI]));

                if (processStages.Count == 0)
                    return true;

                foreach (var processStage in processStages)
                {
                    var labels = processStage.FindElements(By.TagName("label"));

                    //Click the Label of the Process Stage if found
                    foreach (var label in labels)
                    {
                        if (label.Text.Equals(stageName, StringComparison.OrdinalIgnoreCase))
                        {
                            label.Click();
                        }
                    }
                }

                var flyoutFooterControls = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.Flyout_UCI]));

                foreach (var control in flyoutFooterControls)
                {
                    //If there's a field to enter, fill it out
                    if (businessProcessFlowField != null)
                    {
                        var bpfField = control.FindElement(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.BusinessProcessFlowFieldName].Replace("[NAME]", businessProcessFlowField.Name)));

                        if (bpfField != null)
                        {
                            bpfField.Click();
                            for (int i = 0; i < businessProcessFlowField.Value.Length; i++)
                            {
                                bpfField.SendKeys(businessProcessFlowField.Value.Substring(i, 1));
                            }
                        }
                    }

                    //Click the Next Stage Button
                    var nextButton = control.FindElement(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.NextStageButton]));
                    nextButton.Click();
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectStage(string stageName, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions($"Select Stage: {stageName}"), driver =>
            {
                //Find the Business Process Stages
                var processStages = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.NextStage_UCI]));

                foreach (var processStage in processStages)
                {
                    var labels = processStage.FindElements(By.TagName("label"));

                    //Click the Label of the Process Stage if found
                    foreach (var label in labels)
                    {
                        if (label.Text.Equals(stageName, StringComparison.OrdinalIgnoreCase))
                        {
                            label.Click();
                        }
                    }
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SetActive(string stageName = "", int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Set Active Stage: {stageName}"), driver =>
            {
                if (!String.IsNullOrEmpty(stageName))
                {
                    SelectStage(stageName);

                    if (!driver.HasElement(By.XPath("//button[contains(@data-id,'setActiveButton')]")))
                        throw new NotFoundException($"Unable to find the Set Active button. Please verify the stage name {stageName} is correct.");

                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.SetActiveButton])).Click(true);

                    driver.WaitForTransaction();
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFPin(string stageName, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions($"Pin BPF: {stageName}"), driver =>
            {
                //Click the BPF Stage
                SelectStage(stageName, 0);
                driver.WaitForTransaction();

                //Pin the Stage
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.PinStageButton])))
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.PinStageButton])).Click();
                else
                    throw new NotFoundException($"Pin button for stage {stageName} not found.");

                driver.WaitForTransaction();
                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFClose(string stageName, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions($"Close BPF: {stageName}"), driver =>
            {
                //Click the BPF Stage
                SelectStage(stageName, 0);
                driver.WaitForTransaction();

                //Pin the Stage
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.CloseStageButton])))
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.CloseStageButton])).Click(true);
                else
                    throw new NotFoundException($"Close button for stage {stageName} not found.");

                driver.WaitForTransaction();
                return true;
            });
        }

        #endregion

        #region GlobalSearch

        /// <summary>
        /// Searches for the specified criteria in Global Search.
        /// </summary>
        /// <param name="criteria">Search criteria.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param> time.</param>
        /// <example>xrmBrowser.GlobalSearch.Search("Contoso");</example>
        internal BrowserCommandResult<bool> GlobalSearch(string criteria, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Global Search: {criteria}"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.SearchButton]),
                    TimeSpan.FromSeconds(5),
                    "The Global Search button is not available.");

                var input = driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Text]), "The Global Search text field is not available.");

                string reference = null;
                driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Type]),
                    e =>
                    {
                        var searchType = e.GetAttribute("value");
                        reference =
                            searchType == "0" ? AppReference.GlobalSearch.RelevanceSearchButton :
                            searchType == "1" ? AppReference.GlobalSearch.CategorizedSearchButton :
                            throw new InvalidOperationException("The Global Search type is not available.");
                    },
                    "The Global Search type is not available."
                );

                IWebElement button = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[reference]), "The Global Search Button is not available.");

                input.SendKeys(criteria, true);
                button.Click(true);
                return true;
            });
        }

        /// <summary>
        /// Filter by entity in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to filter with.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.GlobalSearch.FilterWith("Account");</example>
        public BrowserCommandResult<bool> FilterWith(string entity, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Filter With: {entity}"), driver =>
            {
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Filter]),
                    TimeSpan.FromSeconds(10),
                    picklist =>
                    {
                        var options = picklist.FindElements(By.TagName("option"));
                        var option = options.FirstOrDefault(x => x.Text == entity);
                        if (option == null)
                            throw new InvalidOperationException($"Entity '{entity}' does not exist in the Filter options.");

                        picklist.Click();
                        option.Click();
                    },
                    "Filter With picklist is not available. The timeout period elapsed waiting for the picklist to be available."
                );


                return true;
            });
        }

        /// <summary>
        /// Filter by group and value in the Global Search Results.
        /// </summary>
        /// <param name="filterby">The Group that contains the filter you want to use.</param>
        /// <param name="value">The value listed in the group by area.</param>
        /// <example>xrmBrowser.GlobalSearch.Filter("Record Type", "Accounts");</example>
        public BrowserCommandResult<bool> Filter(string filterBy, string value, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Filter With: {value}"), driver =>
            {
                var xpathToContainer = By.XPath(AppElements.Xpath[AppReference.GlobalSearch.GroupContainer].Replace("[NAME]", filterBy));
                var xpathToValue = By.XPath(AppElements.Xpath[AppReference.GlobalSearch.FilterValue].Replace("[NAME]", value));
                driver.WaitUntilVisible(xpathToContainer,
                    TimeSpan.FromSeconds(10),
                    groupContainer => groupContainer.ClickWhenAvailable(xpathToValue, $"Filter By Value '{value}' does not exist in the Filter options."),
                    "Filter With picklist is not available. The timeout period elapsed waiting for the picklist to be available."
                );
                return true;
            });
        }

        /// <summary>
        /// Opens the specified record in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to open a record.</param>
        /// <param name="index">The index of the record you want to open.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param> time.</param>
        /// <example>xrmBrowser.GlobalSearch.OpenRecord("Accounts",0);</example>
        public BrowserCommandResult<bool> OpenGlobalSearchRecord(string entity, int index, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Global Search Record"), driver =>
            {
                var searchTypeElement = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Type]), "The Global Search type is not available.");
                var searchType = searchTypeElement.GetAttribute("value");

                if (searchType == "1") //Categorized Search
                {
                    var resultsContainer = driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Container]),
                        Constants.DefaultTimeout,
                        "Search Results is not available"
                    );

                    var entityContainer = resultsContainer.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.EntityContainer].Replace("[NAME]", entity)),
                        $"Entity {entity} was not found in the results"
                    );

                    var records = entityContainer.FindElements(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Records]));
                    if (records == null || records.Count == 0)
                        throw new InvalidOperationException($"No records found for entity {entity}");

                    if (index >= records.Count)
                        throw new InvalidOperationException($"There was less than {index} records in your the search result.");

                    records[index].Click(true);

                    driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Entity.Form]),
                        TimeSpan.FromSeconds(30),
                        "CRM Record is Unavailable or not finished loading. Timeout Exceeded"
                    );
                    return true;
                }

                if (searchType == "0") //Relevance Search
                {
                    var resultsContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.RelevanceResultsContainer]));
                    var records = resultsContainer.FindElements(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.RelevanceResults].Replace("[ENTITY]", entity.ToUpper())));

                    if (index >= records.Count)
                        throw new InvalidOperationException($"There was less than {index} records in your the search result.");

                    records[index].Click(true);
                    return true;
                }

                return false;
            });
        }


        /// <summary>
        /// Changes the search type used for global search
        /// </summary>
        /// <param name="type">The type of search that you want to do.</param>
        /// <example>xrmBrowser.GlobalSearch.ChangeSearchType("Categorized");</example>
        public BrowserCommandResult<bool> ChangeSearchType(string type, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions("Change Search Type"), driver =>
            {
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Type]),
                    Constants.DefaultTimeout,
                    select =>
                    {
                        var options = select.FindElements(By.TagName("option"));
                        var option = options.FirstOrDefault(x => x.Text.Trim() == type);
                        if (option == null)
                            return;

                        select.Click(true);
                        option.Click(true);
                    },
                    "Search Results is not available");
                return true;
            });
        }

        #endregion

        #region Dashboard

        internal BrowserCommandResult<bool> SelectDashboard(string dashboardName, int thinkTime = Constants.DefaultThinkTime)
        {
            ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Select Dashboard"), driver =>
            {
                //Click the drop-down arrow
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dashboard.DashboardSelector]));
                //Select the dashboard
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dashboard.DashboardItemUCI].Replace("[NAME]", dashboardName)));

                return true;
            });
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

        internal void ThinkTime(int milliseconds)
        {
            Browser.ThinkTime(milliseconds);
        }

        internal void ThinkTime(TimeSpan timespan)
        {
            ThinkTime((int)timespan.TotalMilliseconds);
        }

        internal void Dispose()
        {
            Browser.Dispose();
        }
    }
}