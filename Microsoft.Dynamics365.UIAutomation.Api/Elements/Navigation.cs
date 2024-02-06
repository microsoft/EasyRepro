// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class Navigation 
    {
        #region DTO
        public class ApplicationReference
        {
            public const string Application = "Application";
            private string _shell = "//*[@id=\"ApplicationShell\"]";
            public string Shell { get => _shell; set { _shell = value; } }
        }
        public class NavigationReference
        {
            public const string Navigation = "Navigation";
            #region private
            private string _AreaButton = "//button[@id='areaSwitcherId']";
            private string _AreaMenu = "//*[@data-lp-id='sitemap-area-switcher-flyout']";
            private string _AreaMoreMenu = "//ul[@role=\"menubar\"]";
            private string _SubAreaContainer = "//*[@data-id=\"navbar-container\"]/div/ul";
            private string _WebAppMenuButton = "//*[@id=\"TabArrowDivider\"]/a";
            private string _AppMenuButton = "//a[@data-id=\"appBreadCrumb\"]";
            private string _SiteMapLauncherButton = "//button[@data-lp-id=\"sitemap-launcher\"]";
            private string _SiteMapLauncherCloseButton = "//button[@data-id='navbutton']";
            private string _SiteMapAreaMoreButton = "//li[translate(@data-text,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = '[NAME]']";
            private string _SiteMapSingleArea = "//*[@id=\"taskpane-scroll-container\"]";
            private string _AppMenuContainer = "//button[@data-id='[NAME]Launcher']";
            private string _SettingsLauncherBar = "//button[@data-id='[NAME]Launcher']";
            private string _SettingsLauncher = "//ul[@data-id='[NAME]Launcher']";
            //private string SettingsLauncher = "//button[@data-id='[NAME]Launcher']";
            private string _AccountManagerButton = "//*[@id=\"mectrl_main_trigger\"]";
            private string _AccountManagerSignOutButton = "//*[@id=\"mectrl_body_signOut\"]";
            private string _GuidedHelp = "//*[@id=\"helpLauncher\"]/button";
            private string _AdminPortal = "//*[contains(@data-id,'officewaffle')]";
            private string _AdminPortalButton = "//*[@id=(\"O365_AppTile_Admin\")]";
            private string _SearchButton = "//*[@id=\"searchLauncher\"]/button";
            private string _Search = "//*[@id=\"categorizedSearchInputAndButton\"]";
            private string _QuickLaunchActivities = "//button[contains(@data-id, 'quickCreateMenuButton_Activities')]";
            private string _QuickLaunchMenu = "//div[contains(@data-id,'quick-launch-bar')]";
            private string _QuickLaunchButton = "//li[contains(@title, '[NAME]')]";
            private string _QuickCreateButton = "//button[contains(@data-id,'quickCreateLauncher')]";
            private string _QuickCreateMenuList = "//ul[contains(@id,'MenuSectionItemsquickCreate')]";
            private string _QuickCreateMenuItems = "//button[@role='menuitem']";
            private string _PinnedSitemapEntity = "//li[contains(@data-id,'sitemap-entity-Pinned') and contains(@role,'treeitem')]";
            private string _SitemapMenuGroup = "//ul[@role=\"group\"]";
            private string _SitemapMenuItems = "//li[contains(@data-id,'sitemap-entity')]";
            private string _SitemapSwitcherButton = "//button[contains(@data-id,'sitemap-areaSwitcher-expand-btn')]";
            private string _SitemapSwitcherFlyout = "//div[contains(@data-lp-id,'sitemap-area-switcher-flyout')]";
            private string _AppContainer = "//div[@id='AppLandingPageContentContainer']";
            private string _AppTile = "//div[@data-type='app-title' and @title='[NAME]']";
            private string _GoBack = "//button[@title='Go back']";
            #endregion
            public string AreaButton { get => _AreaButton; set { _AreaButton = value; } }
            public string AreaMenu { get => _AreaMenu; set { _AreaMenu = value; } }
            public string AreaMoreMenu { get => _AreaMoreMenu; set { _AreaMoreMenu = value; } }
            public string SubAreaContainer { get => _SubAreaContainer; set { _SubAreaContainer = value; } }
            public string WebAppMenuButton { get => _WebAppMenuButton; set { _WebAppMenuButton = value; } }
            public string AppMenuButton { get => _AppMenuButton; set { _AppMenuButton = value; } }
            public string SiteMapLauncherButton { get => _SiteMapLauncherButton; set { _SiteMapLauncherButton = value; } }
            public string SiteMapLauncherCloseButton { get => _SiteMapLauncherCloseButton; set { _SiteMapLauncherCloseButton = value; } }
            public string SiteMapAreaMoreButton { get => _SiteMapAreaMoreButton; set { _SiteMapAreaMoreButton = value; } }
            public string SiteMapSingleArea { get => _SiteMapSingleArea; set { _SiteMapSingleArea = value; } }
            public string AppMenuContainer { get => _AppMenuContainer; set { _AppMenuContainer = value; } }
            public string SettingsLauncherBar { get => _SettingsLauncherBar; set { _SettingsLauncherBar = value; } }
            public string SettingsLauncher { get => _SettingsLauncher; set { _SettingsLauncher = value; } }
            //public string SettingsLauncher = "//button[@data-id='[NAME]Launcher']";
            public string AccountManagerButton { get => _AccountManagerButton; set { _AccountManagerButton = value; } }
            public string AccountManagerSignOutButton { get => _AccountManagerSignOutButton; set { _AccountManagerSignOutButton = value; } }
            public string GuidedHelp { get => _GuidedHelp; set { _GuidedHelp = value; } }
            public string AdminPortal { get => _AdminPortal; set { _AdminPortal = value; } }
            public string AdminPortalButton { get => _AdminPortalButton; set { _AdminPortalButton = value; } }
            public string SearchButton { get => _SearchButton; set { _SearchButton = value; } }
            public string Search { get => _Search; set { _Search = value; } }

            public string QuickLaunchActivities { get => _QuickLaunchActivities; set { _QuickLaunchActivities = value; } }
            public string QuickLaunchMenu { get => _QuickLaunchMenu; set { _QuickLaunchMenu = value; } }
            public string QuickLaunchButton { get => _QuickLaunchButton; set { _QuickLaunchButton = value; } }
            public string QuickCreateButton { get => _QuickCreateButton; set { _QuickCreateButton = value; } }
            public string QuickCreateMenuList { get => _QuickCreateMenuList; set { _QuickCreateMenuList = value; } }
            public string QuickCreateMenuItems { get => _QuickCreateMenuItems; set { _QuickCreateMenuItems = value; } }
            public string PinnedSitemapEntity { get => _PinnedSitemapEntity; set { _PinnedSitemapEntity = value; } }
            public string SitemapMenuGroup { get => _SitemapMenuGroup; set { _SitemapMenuGroup = value; } }
            public string SitemapMenuItems { get => _SitemapMenuItems; set { _SitemapMenuItems = value; } }
            public string SitemapSwitcherButton { get => _SitemapSwitcherButton; set { _SitemapSwitcherButton = value; } }
            public string SitemapSwitcherFlyout { get => _SitemapSwitcherFlyout; set { _SitemapSwitcherFlyout = value; } }
            public string AppContainer { get => _AppContainer; set { _AppContainer = value; } }
            public string AppTile { get => _AppTile; set { _AppTile = value; } }
            public string GoBack { get => _GoBack; set { _GoBack = value; } }

            public static class MenuRelatedReference
            {
                public const string MenuRelated = "MenuRelated";
                private static string _Related = "//li[contains(@data-id,\"tablist-tab_related\")]";
                private static string _CommonActivities = "//div[contains(@data-id,\"form-tab-relatedEntity-navActivities\")]";
                public static string Related { get => _Related; set { _Related = value; } }
                public static string CommonActivities { get => _CommonActivities; set { _CommonActivities = value; } }
            }
        }
        #endregion
        private readonly WebClient _client;
        private InteractiveBrowser _browser;
        #region public
        public Navigation(WebClient client)
        {
            _browser = client.Browser;
            _client = client;
        }
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
        /// Opens the Admin Portal
        /// </summary>
        public void OpenPortalAdmin()
        {
            this.OpenAdminPortal();
        }
        /// <summary>
        /// Clicks the Search button (magnifying glass icon)
        /// </summary>
        //public void OpenGlobalSearch()
        //{
        //    this.OpenGlobalSearch();
        //}

        /// <summary>
        /// This method will open and click on any Menu and Menuitem provided to it.
        /// </summary>
        /// <param name="menuName">This is the name of the menu reference in the XPath Dictionary in the References Class</param>
        /// <param name="menuItemName">This is the name of the menu item reference in the XPath Dictionary in the References Class</param>
        public void OpenMenu(string menuName, string menuItemName)
        {
            OpenAndClickPopoutMenu(menuName, menuItemName);
        }

        #endregion
        #region Public BrowserCommandResult
        public BrowserCommandResult<bool> GoBack()
        {
            return _client.Execute(_client.GetOptions("Go Back"), driver =>
            {
                driver.Wait();

                var IElement = driver.ClickWhenAvailable(_client.ElementMapper.NavigationReference.GoBack);

                driver.Wait();
                return IElement != null;
            });
        }
        public BrowserCommandResult<bool> QuickCreate(string entityName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Quick Create: {entityName}"), driver =>
            {
                //Click the + button in the ribbon
                var quickCreateButton = driver.FindElement(_client.ElementMapper.NavigationReference.QuickCreateButton);
                quickCreateButton.Click(_client);

                //Find the entity name in the list
                var entityMenuList = driver.FindElement(_client.ElementMapper.NavigationReference.QuickCreateMenuList);
                var entityMenuItems = driver.FindElements(entityMenuList.Locator + _client.ElementMapper.NavigationReference.QuickCreateMenuItems);
                var entitybutton = entityMenuItems.FirstOrDefault(e => e.Text.Contains(entityName, StringComparison.OrdinalIgnoreCase));

                //Text could be in Activities. Expand and verify
                if (driver.HasElement(_client.ElementMapper.NavigationReference.QuickLaunchActivities))
                {
                    var activityExpander = driver.FindElement(_client.ElementMapper.NavigationReference.QuickLaunchActivities);
                    activityExpander.Click(_client);

                    if (driver.HasElement("//button[@data-id=\"quickCreateMenuButton_[NAME]\"]".Replace("[NAME]", entityName)))
                    {
                        driver.FindElement("//button[@data-id=\"quickCreateMenuButton_[NAME]\"]".Replace("[NAME]", entityName)).Click(_client);
                    }
                    return true;
                }

                if (entitybutton == null)
                    throw new Exception(String.Format("{0} not found in Quick Create list.", entityName));

                //Click the entity name
                entitybutton.Click(_client);

                driver.Wait();

                return true;
            });
        }
        public BrowserCommandResult<bool> ClickQuickLaunchButton(string toolTip, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Quick Launch: {toolTip}"), driver =>
            {
                driver.WaitUntilAvailable(_client.ElementMapper.NavigationReference.QuickLaunchMenu);

                //Text could be in the crumb bar.  Find the Quick launch bar buttons and click that one.
                var buttons = driver.FindElement(_client.ElementMapper.NavigationReference.QuickLaunchMenu);
                var launchButton = driver.FindElement(buttons.Locator + _client.ElementMapper.NavigationReference.QuickLaunchButton.Replace("[NAME]", toolTip));
                launchButton.Click(_client);

                //Text could be in Activities. Expand and verify
                if (driver.HasElement(_client.ElementMapper.NavigationReference.QuickLaunchActivities))
                {
                    var activityExpander = driver.FindElement(_client.ElementMapper.NavigationReference.QuickLaunchActivities);
                    activityExpander.Click(_client);

                    if (driver.HasElement("//button[@data-id=\"quickCreateMenuButton_[NAME]\"]".Replace("[NAME]", toolTip))){
                        driver.FindElement("//button[@data-id=\"quickCreateMenuButton_[NAME]\"]".Replace("[NAME]", toolTip)).Click(_client);
                    }
                }

                return true;
            });
        }
        public BrowserCommandResult<bool> SignOut()
        {
            return _client.Execute(_client.GetOptions("Sign out"), driver =>
            {
                driver.WaitUntilAvailable(_client.ElementMapper.NavigationReference.AccountManagerButton).Click(_client);
                driver.WaitUntilAvailable(_client.ElementMapper.NavigationReference.AccountManagerSignOutButton).Click(_client);

                driver.Wait(PageEvent.Load);
                return true;
            });
        }
        public BrowserCommandResult<bool> OpenApp(string appName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Open App {appName}"), driver =>
            {
                driver.Wait(PageEvent.Load);
                driver.SwitchToFrame("0");

                var query = GetUrlQueryParams(driver.Url);
                bool isSomeAppOpen = query.Get("appid") != null || query.Get("app") != null;

                bool success = false;
                if (!isSomeAppOpen)
                    success = TryToClickInAppTile(appName, driver);
                else
                    success = TryOpenAppFromMenu(driver, appName, _client.ElementMapper.NavigationReference.AppMenuButton) ||
                              TryOpenAppFromMenu(driver, appName, _client.ElementMapper.NavigationReference.WebAppMenuButton);

                if (!success)
                    throw new InvalidOperationException($"App Name {appName} not found.");

                OnlineLogin login = new OnlineLogin(_client);
                login.InitializeModes();

                // Wait for app page IElements to be visible (shell and sitemapLauncherButton)
                var shell = driver.WaitUntilAvailable(_client.ElementMapper.ApplicationReference.Shell);
                var sitemapLauncherButton = driver.WaitUntilAvailable(_client.ElementMapper.NavigationReference.SiteMapLauncherButton);

                success = shell != null && sitemapLauncherButton != null;

                if (!success)
                    throw new InvalidOperationException($"App '{appName}' was found but app page was not loaded.");

                _client.CloseTeachingBubbles(driver);

                return true;
            });
        }
        public BrowserCommandResult<bool> OpenArea(string subarea)
        {
            return _client.Execute(_client.GetOptions("Open Unified Interface Area"), driver =>
            {
                var success = TryOpenArea(subarea);
                this.WaitForLoadArea(driver);
                return success;
            });
        }
        /// <summary>
        /// Open Global Search
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenGlobalSearch();</example>
        public BrowserCommandResult<bool> OpenGlobalSearch(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Open Global Search"), driver =>
            {
            driver.Wait();

            if (driver.HasElement("//*[@id='GlobalSearchBox']"));
                {
                    return true;
                }

                driver.ClickWhenAvailable(
                    _client.ElementMapper.NavigationReference.SearchButton,
                    5.Seconds(),
                    "The Global Search button is not available.");

                driver.Wait();

                return true;
            });
        }
        public BrowserCommandResult<bool> OpenGroupSubArea(string group, string subarea, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Open Group Sub Area"), driver =>
            {
                //Make sure the sitemap-launcher is expanded - 9.1
                if (driver.HasElement(_client.ElementMapper.NavigationReference.SiteMapLauncherButton))
                {
                    var expanded = bool.Parse(driver.FindElement(_client.ElementMapper.NavigationReference.SiteMapLauncherButton).GetAttribute(_client,"aria-expanded"));

                    if (!expanded)
                        driver.ClickWhenAvailable(_client.ElementMapper.NavigationReference.SiteMapLauncherButton);
                }

                var groups = driver.FindElements(_client.ElementMapper.NavigationReference.SitemapMenuGroup);
                var groupList = groups.FirstOrDefault(g => g.GetAttribute(_client,"aria-label").ToLowerString() == group.ToLowerString());
                if (groupList == null)
                {
                    throw new KeyNotFoundException($"No group with the name '{group}' exists");
                }

                var subAreaItems = driver.FindElements(groupList.Locator + _client.ElementMapper.NavigationReference.SitemapMenuItems);
                var subAreaItem = subAreaItems.FirstOrDefault(a => a.GetAttribute(_client, "data-text").ToLowerString() == subarea.ToLowerString());
                if (subAreaItem == null)
                {
                    throw new KeyNotFoundException($"No subarea with the name '{subarea}' exists inside of '{group}'");
                }

                subAreaItem.Click(_client);

                this.WaitForLoadArea(driver);
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
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Open Guided Help"), driver =>
            {
                driver.ClickWhenAvailable(_client.ElementMapper.NavigationReference.GuidedHelp);

                return true;
            });
        }
        public BrowserCommandResult<bool> OpenSubArea(string subarea)
        {
            return _client.Execute(_client.GetOptions("Open Unified Interface Sub-Area"), driver =>
            {
                var success = TryOpenSubArea(driver, subarea);
                this.WaitForLoadArea(driver);
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

                this.WaitForLoadArea(driver);
                return true;
            });
        }
        public void WaitForLoadArea(IWebBrowser driver)
        {
            driver.Wait(PageEvent.Load);
            driver.Wait();
        }
        #endregion
        #region private
        private void AddMenuItems(IElement menu, Dictionary<string, IElement> dictionary)
        {
            var menuItems = _client.Browser.Browser.FindElements(menu.Locator + "//li");
            foreach (var item in menuItems)
            {
                string key = item.Text.ToLowerString();
                if (dictionary.ContainsKey(key))
                    continue;
                dictionary.Add(key, item);
            }
        }
        private void AddMenuItemsFrom(IWebBrowser driver, string referenceToMenuItemsContainer, Dictionary<string, IElement> dictionary)
        {
            Trace.TraceInformation("Attempting to collect menu items using XPath:" + referenceToMenuItemsContainer);
            var menu = driver.WaitUntilAvailable(referenceToMenuItemsContainer,
                TimeSpan.FromSeconds(2), "The Main Menu is not available.");
            AddMenuItems(menu, dictionary);
                
        }
        private Dictionary<string, IElement> GetMenuItemsFrom(IWebBrowser driver, string referenceToMenuItemsContainer)
        {
            var result = new Dictionary<string, IElement>();
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
                driver.WaitUntilAvailable(_client.ElementMapper.ApplicationReference.Shell);
                driver.FindElement(_client.ElementMapper.NavigationReference.AdminPortal)?.Click(_client);
                driver.FindElement(_client.ElementMapper.NavigationReference.AdminPortalButton)?.Click(_client);
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
        internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(string menuName, string menuItemName, int thinkTime = Constants.DefaultThinkTime)
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
                    // IElement is stale reference is thrown here since the HTML components 
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
        //internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(string popoutName, string popoutItemName, int thinkTime = Constants.DefaultThinkTime)
        //{
        //    return this.OpenAndClickPopoutMenu(popoutName, popoutItemName, thinkTime);
        //}
        internal bool OpenAppFromMenu(IWebBrowser driver, string appName)
        {
            var container = driver.WaitUntilAvailable(_client.ElementMapper.NavigationReference.AppMenuContainer.Replace("[NAME]", appName));
            var xpathToButton = "//nav[@aria-hidden='false']//button//*[text()='[TEXT]']".Replace("[TEXT]", appName);
            var button = driver.ClickWhenAvailable(container.Locator + xpathToButton,
                                TimeSpan.FromSeconds(1), "Cannot click button to open app from menu. XPath: '" + container.Locator + xpathToButton + "'"
                            );

            var success = (button != null);
            if (!success)
                Trace.TraceWarning($"App Name '{appName}' not found.");

            return success;
        }
        private Dictionary<string, IElement> OpenAreas(string area, int thinkTime = Constants.DefaultThinkTime)
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
        public Dictionary<string, IElement> OpenMenu(int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions("Open Menu"), driver =>
            {
                driver.ClickWhenAvailable(_client.ElementMapper.NavigationReference.AreaButton);

                var result = GetMenuItemsFrom(driver, _client.ElementMapper.NavigationReference.AreaMenu);
                return result;
            });
        }
        public Dictionary<string, IElement> OpenMenuFallback(string area, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions("Open Menu"), driver =>
            {
                //Make sure the sitemap-launcher is expanded - 9.1
                var xpathSiteMapLauncherButton = _client.ElementMapper.NavigationReference.SiteMapLauncherButton;
                bool success = driver.HasElement(xpathSiteMapLauncherButton);
                if (success)
                {
                    
                    IElement launcherButton = driver.FindElement(xpathSiteMapLauncherButton);
                    bool expanded = bool.Parse(launcherButton.GetAttribute(_client, "aria-expanded"));
                    if (!expanded)
                        driver.ClickWhenAvailable(xpathSiteMapLauncherButton);
                }

                var dictionary = new Dictionary<string, IElement>();

                //Is this the sitemap with enableunifiedinterfaceshellrefresh?
                var xpathSitemapSwitcherButton = _client.ElementMapper.NavigationReference.SitemapSwitcherButton;
                success = driver.HasElement(xpathSitemapSwitcherButton);
                if (success)
                {
                    IElement switcherButton = driver.FindElement(xpathSitemapSwitcherButton);
                    switcherButton.Click(_client);
                    driver.Wait();
                    AddMenuItemsFrom(driver, _client.ElementMapper.NavigationReference.SitemapSwitcherFlyout, dictionary);
                }

                var xpathSiteMapAreaMoreButton = _client.ElementMapper.NavigationReference.SiteMapAreaMoreButton;
                success = driver.HasElement(xpathSiteMapAreaMoreButton);
                if (!success)
                    return dictionary;

                IElement moreButton = driver.FindElement(xpathSiteMapAreaMoreButton);
                bool isVisible = moreButton.IsAvailable;
                if (isVisible)
                {
                    moreButton.Click(_client);
                    AddMenuItemsFrom(driver, _client.ElementMapper.NavigationReference.AreaMoreMenu, dictionary);
                }
                else
                {
                    var singleItem = driver.FindElement(_client.ElementMapper.NavigationReference.SiteMapSingleArea.Replace("[NAME]", area));
                    dictionary.Add(singleItem.Text.ToLowerString(), singleItem);
                }

                return dictionary;
            });
        }
        internal BrowserCommandResult<bool> OpenSettingsOption(string command, string dataId, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Open " + command + " " + dataId), driver =>
            {
                var xpathFlyout = _client.ElementMapper.NavigationReference.SettingsLauncher.Replace("[NAME]", command);
                var xpathToFlyoutButton = _client.ElementMapper.NavigationReference.SettingsLauncherBar.Replace("[NAME]", command);
        //                    public static string SettingsLauncherBar = "//button[@data-id='[NAME]Launcher']";
        //public static string SettingsLauncher = "//ul[@data-id='[NAME]Launcher']";
                IElement flyout;
                bool success = driver.HasElement(xpathFlyout);
                if (success)
                {
                    flyout = driver.FindElement(xpathFlyout);
                }
                if (!success)
                {
                    driver.ClickWhenAvailable(xpathToFlyoutButton, new TimeSpan(0,0,5), $"No command button exists that match with: {command}.");
                    //flyout = driver.WaitUntilVisible(xpathFlyout, "Flyout menu did not became visible");
                }

                var menuItems = driver.FindElements("//button[contains(@data-id,'SettingsMenu')]");
                //var button = menuItems.FirstOrDefault(x => x.GetAttribute("data-id").Contains(dataId));
                var button = driver.FindElement("//button[contains(@data-id,'" + dataId + "')]");
                if (button != null)
                {
                    button.Click(_client);
                    return true;
                }

                throw new InvalidOperationException($"No command with data-id: {dataId} exists inside of the command menu {command}");
            });
        }
        private bool TryOpenArea(string area)
        {
            area = area.ToLowerString();
            var areas = OpenAreas(area);

            IElement menuItem;
            bool found = areas.TryGetValue(area, out menuItem);
            if (found)
            {
                var strSelected = menuItem.GetAttribute(_client,"aria-checked");
                bool selected;
                bool.TryParse(strSelected, out selected);
                if (!selected)
                    menuItem.Click(_client);
            }
            return found;
        }
        public Dictionary<string, IElement> GetSubAreaMenuItems(IWebBrowser driver)
        {
            var dictionary = new Dictionary<string, IElement>();

            //Sitemap without enableunifiedinterfaceshellrefresh
            var hasPinnedSitemapEntity = driver.HasElement(_client.ElementMapper.NavigationReference.PinnedSitemapEntity);
            if (!hasPinnedSitemapEntity)
            {
                // Close SiteMap launcher since it is open
                var xpathToLauncherCloseButton = _client.ElementMapper.NavigationReference.SiteMapLauncherCloseButton;
                driver.ClickWhenAvailable(xpathToLauncherCloseButton);

                driver.ClickWhenAvailable(_client.ElementMapper.NavigationReference.SiteMapLauncherButton);

                var menuContainer = driver.WaitUntilAvailable(_client.ElementMapper.NavigationReference.SubAreaContainer);

                var subItems = driver.FindElements(menuContainer.Locator + "//li");

                foreach (var subItem in subItems)
                {
                    // Check 'Id' attribute, NULL value == Group Header
                    var id = subItem.GetAttribute(_client, "id");
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
            var menuShell = driver.FindElements(_client.ElementMapper.NavigationReference.SubAreaContainer);

            //The menu is broke into multiple sections. Gather all items.
            foreach (IElement menuSection in menuShell)
            {
                var menuItems = driver.FindElements(menuSection.Locator + _client.ElementMapper.NavigationReference.SitemapMenuItems);

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
        private bool TryOpenAppFromMenu(IWebBrowser driver, string appName, string appMenuButton)
        {
            bool found = false;
            var xpathToAppMenu = _client.ElementMapper.NavigationReference.AppMenuButton;
            driver.Wait(new TimeSpan(0, 0, 5));
            var appClicked = driver.ClickWhenAvailable(xpathToAppMenu);
            //if (appClicked) driver.FindElement(xpathToAppMenu).Click(_client);
            //app.Click(_client);
            found = TryToClickInAppTile(appName, driver) || OpenAppFromMenu(driver, appName);
            //driver.WaitUntilClickable(xpathToAppMenu, 5.Seconds(),
            //            appMenu =>
            //            {
            //                appMenu.Click(true);
            //                found = TryToClickInAppTile(appName, driver) || OpenAppFromMenu(driver, appName);
            //            });
            return found;
        }
        private bool TryOpenSubArea(IWebBrowser driver, string subarea)
        {
            subarea = subarea.ToLowerString();
            var navSubAreas = GetSubAreaMenuItems(driver);

            var found = navSubAreas.TryGetValue(subarea, out var IElement);
            if (found)
            {
                var strSelected = IElement.GetAttribute(_client,"aria-selected");
                bool.TryParse(strSelected, out var selected);
                if (!selected)
                {
                    IElement.Locator = "//*[@id='" + IElement.Id + "']";
                    IElement.Click(_client);
                }
                else
                {
                    // This will result in navigating back to the desired subArea -- even if already selected.
                    // Example: If context is an Account entity record, then a call to OpenSubArea("Sales", "Accounts"),
                    // this will click on the Accounts subArea and go back to the grid view
                    IElement.Click(_client);
                }
            }
            return found;
        }
        private bool TryToClickInAppTile(string appName, IWebBrowser driver)
        {
            string message = "Frame AppLandingPage is not loaded.";
            driver.SwitchToFrame("AppLandingPage");
            //driver.WaitUntil(
            //    d =>
            //    {
            //        try
            //        {
            //            driver.SwitchTo().Frame("AppLandingPage");
            //        }
            //        catch (NoSuchFrameException ex)
            //        {
            //            message = $"{message} Exception: {ex.Message}";
            //            Trace.TraceWarning(message);
            //            return false;
            //        }
            //        return true;
            //    },
            //    5.Seconds()
            //    );

            var xpathToAppContainer = _client.ElementMapper.NavigationReference.AppContainer;
            var xpathToappTile = _client.ElementMapper.NavigationReference.AppTile.Replace("[NAME]", appName);

            bool success = false;
            var appContainer = driver.WaitUntilAvailable(xpathToAppContainer);
            success = driver.ClickWhenAvailable(appContainer.Locator + xpathToappTile, TimeSpan.FromSeconds(5)) != null;
            //driver.WaitUntilVisible(xpathToAppContainer, TimeSpan.FromSeconds(5),
            //    appContainer => success = appContainer.ClickWhenAvailable(xpathToappTile, TimeSpan.FromSeconds(5)) != null
            //    );

            if (!success)
                Trace.TraceWarning(message);

            return success;
        }
        #endregion
    }
}
