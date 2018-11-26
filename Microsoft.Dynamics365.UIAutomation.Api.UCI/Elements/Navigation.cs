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

        public void OpenApp(string appName)
        {
            _client.OpenApp(appName);
        }
        public void OpenSubArea(string area, string subarea)
        {
            _client.OpenSubArea(area, subarea);
        }

        public void OpenOptions()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.PersonalSettings");
        }

        public void OpenAbout()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.About");
        }

        public void OpenPrivacy()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.PrivacyStatement");
        }

        public void OpenOptInForLearningPath()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.LpOptIn-buttoncontainer");
        }

        public void OpenSoftwareLicensing()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.SoftwareLicenseTerms");
        }
        public void OpenToastNotifications()
        {
            _client.OpenSettingsOption("personalSettings", "SettingsMenu.ToastNotificationSettings");
        }
        public void SignOut()
        {
            _client.OpenSettingsOption("userInformation", "UserInformationMenu.SignOut");
        }
        public void OpenGuidedHelp()
        {
            _client.OpenGuidedHelp();
        }

        public void OpenPortalAdmin()
        {
            _client.OpenAdminPortal();
        }
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
		
		public void QuickCreate(string entityName)
        {
            _client.QuickCreate(entityName);
        }

        public void ClickQuickLaunchButton(string toolTip)
        {
            _client.ClickQuickLaunchButton(toolTip);
        }
    }
}
