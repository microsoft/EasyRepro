// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class Options
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestOpenOptions()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.OpenOptions();
                xrmBrowser.ThinkTime(5000);

            }
        }

        [TestMethod]
        public void WEBTestOpenPrintPreview()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");
                xrmBrowser.Grid.SelectRecord(1);
                xrmBrowser.Navigation.OpenPrintPreview();

                xrmBrowser.ThinkTime(1000);
            }
        }

        [TestMethod]
        public void WEBTestOpenAppsforDynamicsCRM()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.OpenAppsForDynamicsCRM();

                xrmBrowser.ThinkTime(1000);

            }
        }

        [TestMethod]
        public void WEBTestOpenSeeWelcomeScreen()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.OpenWelcomeScreen();

                xrmBrowser.ThinkTime(1000);

            }
        }

        [TestMethod]
        public void WEBTestOpenAbout()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.OpenAbout();

                xrmBrowser.Driver.LastWindow().Close();

                xrmBrowser.ThinkTime(1000);

            }
        }

        [TestMethod]
        public void WEBTestOpenOptOutOfLearningPath()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.OpenOptOutLearningPath();

                xrmBrowser.ThinkTime(1000);

            }
        }

        [TestMethod]
        public void WEBTestOpenPrivacyStatement()
        {
            //Only available on Admin Users
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");
                xrmBrowser.Grid.SelectRecord(1);

                xrmBrowser.Navigation.OpenPrivacyStatement();

                xrmBrowser.ThinkTime(1000);
            }
        }
    }
}