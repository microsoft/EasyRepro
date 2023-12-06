// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Navigation : Element
    {
        private readonly WebClient _client;
        private InteractiveBrowser _browser;
        #region public
        public Navigation(WebClient client)
        {
            _browser = client.Browser;
            _client = client;
        }

        /// <summary>
        /// Opens the App supplied
        /// </summary>
        /// <param name="appName">Name of the app to open</param>
        //public void OpenApp(string appName)
        //{
        //    _client.OpenApp(appName);
        //}

        /// <summary>
        /// Opens a sub area from a group in the active app &amp; area
        /// This can be used to navigate within the active app/area or when the app only has a single area
        /// It will not navigate to a different app or area within the app
        /// </summary>
        /// <param name="group">Name of the group</param>
        /// <param name="subarea">Name of the subarea</param>
        /// <example>xrmApp.Navigation.OpenGroupSubArea("Customers", "Accounts");</example>
        //public void OpenGroupSubArea(string group, string subarea)
        //{
        //    _client.OpenGroupSubArea(group, subarea);
        //}

        /// <summary>
        /// Opens a area in the unified client
        /// </summary>
        /// <param name="area">Name of the area</param>
        //public void OpenArea(string area)
        //{
        //    _client.OpenArea(area);
        //}

        /// <summary>
        /// Opens a sub area in the unified client
        /// </summary>
        /// <param name="area">Name of the area</param>
        //public void OpenSubArea(string area)
        //{
        //    _client.OpenSubArea(area);
        //}

        /// <summary>
        /// Opens a sub area in the unified client
        /// </summary>
        /// <param name="area">Name of the area</param>
        /// <param name="subarea">Name of the subarea</param>
        //public void OpenSubArea(string area, string subarea)
        //{
        //    _client.OpenSubArea(area, subarea);
        //}

        /// <summary>
        /// Opens the Personalization Settings menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenOptions()
        {
            this.OpenSettingsOption("personalSettings", "SettingsMenu.PersonalSettings");
        }

        /// <summary>
        /// Opens the About menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenAbout()
        {
            this.OpenSettingsOption("personalSettings", "SettingsMenu.About");
        }

        /// <summary>
        /// Opens the Privacy and Cookies menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenPrivacy()
        {
            this.OpenSettingsOption("personalSettings", "SettingsMenu.PrivacyStatement");
        }

        /// <summary>
        /// Opens the Learning Path menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenOptInForLearningPath()
        {
            this.OpenSettingsOption("personalSettings", "SettingsMenu.LpOptIn-buttoncontainer");
        }

        /// <summary>
        /// Opens the Software license terms menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenSoftwareLicensing()
        {
            this.OpenSettingsOption("personalSettings", "SettingsMenu.SoftwareLicenseTerms");
        }

        /// <summary>
        /// Opens the Toast Notification Display Time terms menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenToastNotifications()
        {
           this.OpenSettingsOption("personalSettings", "SettingsMenu.ToastNotificationSettings");
        }

        /// <summary>
        /// Clicks the Sign Out button
        /// </summary>
        //public void SignOut()
        //{
        //    _client.SignOut();
        //}

        /// <summary>
        /// Clicks the Help button
        /// </summary>
        public void OpenGuidedHelp()
        {
            this.OpenGuidedHelp();
        }

        /// <summary>
        /// Opens the Admin Portal
        /// </summary>
        public void OpenPortalAdmin()
        {
            this.OpenAdminPortal();
        }

        /// <summary>
        /// Clicks the Search button (magnifying glass icon)
        /// </summary>
        public void OpenGlobalSearch()
        {
            this.OpenGlobalSearch();
        }

        /// <summary>
        /// This method will open and click on any Menu and Menuitem provided to it.
        /// </summary>
        /// <param name="menuName">This is the name of the menu reference in the XPath Dictionary in the References Class</param>
        /// <param name="menuItemName">This is the name of the menu item reference in the XPath Dictionary in the References Class</param>
        public void OpenMenu(string menuName, string menuItemName)
        {
            this.OpenAndClickPopoutMenu(menuName, menuItemName);
        }

        /// <summary>
        /// Clicks the quick create button (+ icon)
        /// </summary>
        /// <param name="entityName"></param>
        //public void QuickCreate(string entityName)
        //{
        //    _client.QuickCreate(entityName);
        //}

        /// <summary>
        /// Opens the quick launch bar on the left hand side of the window
        /// </summary>
        /// <param name="toolTip">Tooltip to select</param>
        //public void ClickQuickLaunchButton(string toolTip)
        //{
        //    this.ClickQuickLaunchButton(toolTip);
        //}

        // <summary>
        /// Go back
        /// </summary>
        /// <example>xrmApp.Navigation.GoBack();</example>
        //public void GoBack()
        //{
        //    this.GoBack();
        //}
        #endregion
        #region Public BrowserCommandResult
        public BrowserCommandResult<bool> GoBack()
        {
            return _client.Execute(_client.GetOptions("Go Back"), driver =>
            {
                driver.WaitForTransaction();

                var element = driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.GoBack]));

                driver.WaitForTransaction();
                return element != null;
            });
        }
        public BrowserCommandResult<bool> QuickCreate(string entityName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Quick Create: {entityName}"), driver =>
            {
                //Click the + button in the ribbon
                var quickCreateButton = driver.FindElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.QuickCreateButton]));
                quickCreateButton.Click(true);

                //Find the entity name in the list
                var entityMenuList = driver.FindElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.QuickCreateMenuList]));
                var entityMenuItems = entityMenuList.FindElements(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.QuickCreateMenuItems]));
                var entitybutton = entityMenuItems.FirstOrDefault(e => e.Text.Contains(entityName, StringComparison.OrdinalIgnoreCase));

                if (entitybutton == null)
                    throw new Exception(String.Format("{0} not found in Quick Create list.", entityName));

                //Click the entity name
                entitybutton.Click(true);

                driver.WaitForTransaction();

                return true;
            });
        }
        public BrowserCommandResult<bool> ClickQuickLaunchButton(string toolTip, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Quick Launch: {toolTip}"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.QuickLaunchMenu]));

                //Text could be in the crumb bar.  Find the Quick launch bar buttons and click that one.
                var buttons = driver.FindElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.QuickLaunchMenu]));
                var launchButton = buttons.FindElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.QuickLaunchButton].Replace("[NAME]", toolTip)));
                launchButton.Click();

                return true;
            });
        }
        public BrowserCommandResult<bool> SignOut()
        {
            return _client.Execute(_client.GetOptions("Sign out"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.AccountManagerButton])).Click();
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.AccountManagerSignOutButton])).Click();

                return driver.WaitForPageToLoad();
            });
        }
        public BrowserCommandResult<bool> OpenApp(string appName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Open App {appName}"), driver =>
            {
                driver.WaitForPageToLoad();
                driver.SwitchTo().DefaultContent();

                var query = GetUrlQueryParams(driver.Url);
                bool isSomeAppOpen = query.Get("appid") != null || query.Get("app") != null;

                bool success = false;
                if (!isSomeAppOpen)
                    success = TryToClickInAppTile(appName, driver);
                else
                    success = TryOpenAppFromMenu(driver, appName, DTO.ElementReferences.Navigation.UCIAppMenuButton) ||
                              TryOpenAppFromMenu(driver, appName, DTO.ElementReferences.Navigation.WebAppMenuButton);

                if (!success)
                    throw new InvalidOperationException($"App Name {appName} not found.");

                _client.InitializeModes();

                // Wait for app page elements to be visible (shell and sitemapLauncherButton)
                var shell = driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Application.Shell]));
                var sitemapLauncherButton = driver.WaitUntilVisible(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SiteMapLauncherButton]));

                success = shell != null && sitemapLauncherButton != null;

                if (!success)
                    throw new InvalidOperationException($"App '{appName}' was found but app page was not loaded.");

                return true;
            });
        }
        public BrowserCommandResult<bool> OpenArea(string subarea)
        {
            return _client.Execute(_client.GetOptions("Open Unified Interface Area"), driver =>
            {
                var success = TryOpenArea(subarea);
                _client.WaitForLoadArea(driver);
                return success;
            });
        }
        public BrowserCommandResult<bool> OpenGroupSubArea(string group, string subarea, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Open Group Sub Area"), driver =>
            {
                //Make sure the sitemap-launcher is expanded - 9.1
                if (driver.HasElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SiteMapLauncherButton])))
                {
                    var expanded = bool.Parse(driver.FindElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SiteMapLauncherButton])).GetAttribute("aria-expanded"));

                    if (!expanded)
                        driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SiteMapLauncherButton]));
                }

                var groups = driver.FindElements(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SitemapMenuGroup]));
                var groupList = groups.FirstOrDefault(g => g.GetAttribute("aria-label").ToLowerString() == group.ToLowerString());
                if (groupList == null)
                {
                    throw new NotFoundException($"No group with the name '{group}' exists");
                }

                var subAreaItems = groupList.FindElements(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SitemapMenuItems]));
                var subAreaItem = subAreaItems.FirstOrDefault(a => a.GetAttribute("data-text").ToLowerString() == subarea.ToLowerString());
                if (subAreaItem == null)
                {
                    throw new NotFoundException($"No subarea with the name '{subarea}' exists inside of '{group}'");
                }

                subAreaItem.Click(true);

                _client.WaitForLoadArea(driver);
                return true;
            });
        }
        public BrowserCommandResult<bool> OpenSubArea(string subarea)
        {
            return _client.Execute(_client.GetOptions("Open Unified Interface Sub-Area"), driver =>
            {
                var success = TryOpenSubArea(driver, subarea);
                _client.WaitForLoadArea(driver);
                return success;
            });
        }
        public BrowserCommandResult<bool> OpenSubArea(string area, string subarea)
        {
            return _client.Execute(_client.GetOptions("Open Sub Area"), driver =>
            {
                //If the subarea is already in the left hand nav, click it
                var success = TryOpenSubArea(driver, subarea);
                if (!success)
                {
                    success = TryOpenArea(area);
                    if (!success)
                        throw new InvalidOperationException($"Area with the name '{area}' not found. ");

                    success = TryOpenSubArea(driver, subarea);
                    if (!success)
                        throw new InvalidOperationException($"No subarea with the name '{subarea}' exists inside of '{area}'.");
                }

                _client.WaitForLoadArea(driver);
                return true;
            });
        }
        #endregion
        #region private
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
        private static void AddMenuItemsFrom(IWebDriver driver, string referenceToMenuItemsContainer, Dictionary<string, IWebElement> dictionary)
        {
            driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[referenceToMenuItemsContainer]),
                TimeSpan.FromSeconds(2),
                menu => AddMenuItems(menu, dictionary),
                "The Main Menu is not available."
            );
        }
        private static Dictionary<string, IWebElement> GetMenuItemsFrom(IWebDriver driver, string referenceToMenuItemsContainer)
        {
            var result = new Dictionary<string, IWebElement>();
            AddMenuItemsFrom(driver, referenceToMenuItemsContainer, result);
            return result;
        }
        /// <summary>
        /// This method opens the popout menus in the Dynamics 365 pages. 
        /// This method uses a thinktime since after the page loads, it takes some time for the 
        /// widgets to load before the method can find and popout the menu.
        /// </summary>
        /// <param name="popoutName">The By Object of the Popout menu</param>
        /// <param name="popoutItemName">The By Object of the Popout Item name in the popout menu</param>
        /// <param name="thinkTime">Amount of time(milliseconds) to wait before this method will click on the "+" popout menu.</param>
        /// <returns>True on success, False on failure to invoke any action</returns>
        /// <summary>
        /// Opens the Admin Portal
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenAdminPortal();</example>
        internal BrowserCommandResult<bool> OpenAdminPortal(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);
            return _client.Execute(_client.GetOptions("Open Admin Portal"), driver =>
            {
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Application.Shell]));
                driver.FindElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.AdminPortal]))?.Click();
                driver.FindElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.AdminPortalButton]))?.Click();
                return true;
            });
        }
        internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(By menuName, By menuItemName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute($"Open menu", driver =>
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
        internal bool OpenAppFromMenu(IWebDriver driver, string appName)
        {
            var container = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.AppMenuContainer]));
            var xpathToButton = "//nav[@aria-hidden='false']//button//*[text()='[TEXT]']".Replace("[TEXT]", appName);
            var button = container.ClickWhenAvailable(By.XPath(xpathToButton),
                                TimeSpan.FromSeconds(1)
                            );

            var success = (button != null);
            if (!success)
                Trace.TraceWarning($"App Name '{appName}' not found.");

            return success;
        }
        private Dictionary<string, IWebElement> OpenAreas(string area, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions("Open Unified Interface Area"), driver =>
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
            return _client.Execute(_client.GetOptions("Open Menu"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.AreaButton]));

                var result = GetMenuItemsFrom(driver, DTO.ElementReferences.Navigation.AreaMenu);
                return result;
            });
        }

        public Dictionary<string, IWebElement> OpenMenuFallback(string area, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions("Open Menu"), driver =>
            {
                //Make sure the sitemap-launcher is expanded - 9.1
                var xpathSiteMapLauncherButton = By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SiteMapLauncherButton]);
                bool success = driver.TryFindElement(xpathSiteMapLauncherButton, out IWebElement launcherButton);
                if (success)
                {
                    bool expanded = bool.Parse(launcherButton.GetAttribute("aria-expanded"));
                    if (!expanded)
                        driver.ClickWhenAvailable(xpathSiteMapLauncherButton);
                }

                var dictionary = new Dictionary<string, IWebElement>();

                //Is this the sitemap with enableunifiedinterfaceshellrefresh?
                var xpathSitemapSwitcherButton = By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SitemapSwitcherButton]);
                success = driver.TryFindElement(xpathSitemapSwitcherButton, out IWebElement switcherButton);
                if (success)
                {
                    switcherButton.Click(true);
                    driver.WaitForTransaction();

                    AddMenuItemsFrom(driver, DTO.ElementReferences.Navigation.SitemapSwitcherFlyout, dictionary);
                }

                var xpathSiteMapAreaMoreButton = By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SiteMapAreaMoreButton]);
                success = driver.TryFindElement(xpathSiteMapAreaMoreButton, out IWebElement moreButton);
                if (!success)
                    return dictionary;

                bool isVisible = moreButton.IsVisible();
                if (isVisible)
                {
                    moreButton.Click();
                    AddMenuItemsFrom(driver, DTO.ElementReferences.Navigation.AreaMoreMenu, dictionary);
                }
                else
                {
                    var singleItem = driver.FindElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SiteMapSingleArea].Replace("[NAME]", area)));
                    dictionary.Add(singleItem.Text.ToLowerString(), singleItem);
                }

                return dictionary;
            });
        }
        internal BrowserCommandResult<bool> OpenSettingsOption(string command, string dataId, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Open " + command + " " + dataId), driver =>
            {
                var xpathFlyout = By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SettingsLauncher].Replace("[NAME]", command));
                var xpathToFlyoutButton = By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SettingsLauncherBar].Replace("[NAME]", command));

                IWebElement flyout;
                bool success = driver.TryFindElement(xpathFlyout, out flyout);
                if (!success || !flyout.Displayed)
                {
                    driver.ClickWhenAvailable(xpathToFlyoutButton, $"No command button exists that match with: {command}.");
                    flyout = driver.WaitUntilVisible(xpathFlyout, "Flyout menu did not became visible");
                }

                var menuItems = flyout.FindElements(By.TagName("button"));
                var button = menuItems.FirstOrDefault(x => x.GetAttribute("data-id").Contains(dataId));
                if (button != null)
                {
                    button.Click();
                    return true;
                }

                throw new InvalidOperationException($"No command with data-id: {dataId} exists inside of the command menu {command}");
            });
        }
        private bool TryOpenArea(string area)
        {
            area = area.ToLowerString();
            var areas = OpenAreas(area);

            IWebElement menuItem;
            bool found = areas.TryGetValue(area, out menuItem);
            if (found)
            {
                var strSelected = menuItem.GetAttribute("aria-checked");
                bool selected;
                bool.TryParse(strSelected, out selected);
                if (!selected)
                    menuItem.Click(true);
            }
            return found;
        }
        public Dictionary<string, IWebElement> GetSubAreaMenuItems(IWebDriver driver)
        {
            var dictionary = new Dictionary<string, IWebElement>();

            //Sitemap without enableunifiedinterfaceshellrefresh
            var hasPinnedSitemapEntity = driver.HasElement(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.PinnedSitemapEntity]));
            if (!hasPinnedSitemapEntity)
            {
                // Close SiteMap launcher since it is open
                var xpathToLauncherCloseButton = By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SiteMapLauncherCloseButton]);
                driver.ClickWhenAvailable(xpathToLauncherCloseButton);

                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SiteMapLauncherButton]));

                var menuContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SubAreaContainer]));

                var subItems = menuContainer.FindElements(By.TagName("li"));

                foreach (var subItem in subItems)
                {
                    // Check 'Id' attribute, NULL value == Group Header
                    var id = subItem.GetAttribute("id");
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
            var menuShell = driver.FindElements(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SubAreaContainer]));

            //The menu is broke into multiple sections. Gather all items.
            foreach (IWebElement menuSection in menuShell)
            {
                var menuItems = menuSection.FindElements(By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.SitemapMenuItems]));

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

        private bool TryOpenAppFromMenu(IWebDriver driver, string appName, string appMenuButton)
        {
            bool found = false;
            var xpathToAppMenu = By.XPath(AppElements.Xpath[appMenuButton]);
            driver.WaitUntilClickable(xpathToAppMenu, 5.Seconds(),
                        appMenu =>
                        {
                            appMenu.Click(true);
                            found = TryToClickInAppTile(appName, driver) || OpenAppFromMenu(driver, appName);
                        });
            return found;
        }

        private bool TryOpenSubArea(IWebDriver driver, string subarea)
        {
            subarea = subarea.ToLowerString();
            var navSubAreas = GetSubAreaMenuItems(driver);

            var found = navSubAreas.TryGetValue(subarea, out var element);
            if (found)
            {
                var strSelected = element.GetAttribute("aria-selected");
                bool.TryParse(strSelected, out var selected);
                if (!selected)
                {
                    element.Click(true);
                }
                else
                {
                    // This will result in navigating back to the desired subArea -- even if already selected.
                    // Example: If context is an Account entity record, then a call to OpenSubArea("Sales", "Accounts"),
                    // this will click on the Accounts subArea and go back to the grid view
                    element.Click(true);
                }
            }
            return found;
        }

        private static bool TryToClickInAppTile(string appName, IWebDriver driver)
        {
            string message = "Frame AppLandingPage is not loaded.";
            driver.WaitUntil(
                d =>
                {
                    try
                    {
                        driver.SwitchTo().Frame("AppLandingPage");
                    }
                    catch (NoSuchFrameException ex)
                    {
                        message = $"{message} Exception: {ex.Message}";
                        Trace.TraceWarning(message);
                        return false;
                    }
                    return true;
                },
                5.Seconds()
                );

            var xpathToAppContainer = By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.UCIAppContainer]);
            var xpathToappTile = By.XPath(AppElements.Xpath[DTO.ElementReferences.Navigation.UCIAppTile].Replace("[NAME]", appName));

            bool success = false;
            driver.WaitUntilVisible(xpathToAppContainer, TimeSpan.FromSeconds(5),
                appContainer => success = appContainer.ClickWhenAvailable(xpathToappTile, TimeSpan.FromSeconds(5)) != null
                );

            if (!success)
                Trace.TraceWarning(message);

            return success;
        }
        #endregion
    }
}
