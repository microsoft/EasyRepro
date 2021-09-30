// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenNavigationUci
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly SecureString _mfaSecretKey = System.Configuration.ConfigurationManager.AppSettings["MfaSecretKey"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"]);

        [TestMethod]
        public void UCITestOpenOptions()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.UCIPerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);
                xrmApp.Navigation.OpenOptions();
                // xrmApp.Navigation.OpenOptInForLearningPath();
                // xrmApp.Navigation.OpenPrivacy();
                // xrmApp.Navigation.SignOut();
            }
        }

        [TestMethod]
        public void UCITestOpenGuidedHelp()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.UCIPerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);
                xrmApp.Navigation.OpenGuidedHelp();
            }
        }

        [TestMethod]
        public void UCITestOpenSoftwareLicensing()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.UCIPerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);
                xrmApp.Navigation.OpenSoftwareLicensing();
            }
        }

        [TestMethod]
        public void UCITestOpenToastNotifications()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.UCIPerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);
                xrmApp.Navigation.OpenToastNotifications();
            }
        }

        [TestMethod]
        public void UCITestOpenAbout()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.UCIPerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);
                xrmApp.Navigation.OpenAbout();
            }
        }

        [TestMethod]
        public void UCITestOpenRelatedCommonActivities()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Navigation.OpenMenu(Reference.MenuRelated.Related, Reference.MenuRelated.CommonActivities);
            }
        }
    }
}