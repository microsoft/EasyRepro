// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Navigation : Element
    {
        private readonly WebClient _client;

        public Navigation(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Opens the App supplied
        /// </summary>
        /// <param name="appName">Name of the app to open</param>
        public void OpenApp(string appName)
        {
            _client.OpenApp(appName);
        }

        /// <summary>
        /// Opens a sub area from a group in the active app &amp; area
        /// This can be used to navigate within the active app/area or when the app only has a single area
        /// It will not navigate to a different app or area within the app
        /// </summary>
        /// <param name="group">Name of the group</param>
        /// <param name="subarea">Name of the subarea</param>
        /// <example>xrmApp.Navigation.OpenGroupSubArea("Customers", "Accounts");</example>
        public void OpenGroupSubArea(string group, string subarea)
        {
            _client.OpenGroupSubArea(group, subarea);
        }

        /// <summary>
        /// Opens a area in the unified client
        /// </summary>
        /// <param name="area">Name of the area</param>
        public void OpenArea(string area)
        {
            _client.OpenArea(area);
        }

        /// <summary>
        /// Opens a sub area in the unified client
        /// </summary>
        /// <param name="area">Name of the area</param>
        public void OpenSubArea(string area)
        {
            _client.OpenSubArea(area);
        }

        /// <summary>
        /// Opens a sub area in the unified client
        /// </summary>
        /// <param name="area">Name of the area</param>
        /// <param name="subarea">Name of the subarea</param>
        public void OpenSubArea(string area, string subarea)
        {
            _client.OpenSubArea(area, subarea);
        }

        /// <summary>
        /// Opens the Personalization Settings menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenOptions()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.PersonalSettings");
        }

        /// <summary>
        /// Opens the About menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenAbout()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.About");
        }

        /// <summary>
        /// Opens the Privacy and Cookies menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenPrivacy()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.PrivacyStatement");
        }

        /// <summary>
        /// Opens the Learning Path menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenOptInForLearningPath()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.LpOptIn-buttoncontainer");
        }

        /// <summary>
        /// Opens the Software license terms menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenSoftwareLicensing()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.SoftwareLicenseTerms");
        }

        /// <summary>
        /// Opens the Toast Notification Display Time terms menu item in the Options menu (gear icon)
        /// </summary>
        public void OpenToastNotifications()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.ToastNotificationSettings");
        }

        /// <summary>
        /// Clicks the Sign Out button
        /// </summary>
        public void SignOut()
        {
            _client.SignOut();
        }

        /// <summary>
        /// Clicks the Help button
        /// </summary>
        public void OpenGuidedHelp()
        {
            _client.OpenGuidedHelp();
        }

        /// <summary>
        /// Opens the Admin Portal
        /// </summary>
        public void OpenPortalAdmin()
        {
            _client.OpenAdminPortal();
        }

        /// <summary>
        /// Clicks the Search button (magnifying glass icon)
        /// </summary>
        public void OpenGlobalSearch()
        {
            _client.OpenGlobalSearch();
        }

        /// <summary>
        /// This method will open and click on any Menu and Menuitem provided to it.
        /// </summary>
        /// <param name="menuName">This is the name of the menu reference in the XPath Dictionary in the References Class</param>
        /// <param name="menuItemName">This is the name of the menu item reference in the XPath Dictionary in the References Class</param>
        public void OpenMenu(string menuName, string menuItemName)
        {
            _client.OpenAndClickPopoutMenu(menuName, menuItemName);
        }

        /// <summary>
        /// Clicks the quick create button (+ icon)
        /// </summary>
        /// <param name="entityName"></param>
        public void QuickCreate(string entityName)
        {
            _client.QuickCreate(entityName);
        }

        /// <summary>
        /// Opens the quick launch bar on the left hand side of the window
        /// </summary>
        /// <param name="toolTip">Tooltip to select</param>
        public void ClickQuickLaunchButton(string toolTip)
        {
            _client.ClickQuickLaunchButton(toolTip);
        }
    }
}
