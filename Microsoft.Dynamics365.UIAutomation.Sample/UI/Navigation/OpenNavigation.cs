// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class OpenNavigationUci : TestsBase
    {
        //private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        //private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        //private readonly SecureString _mfaSecretKey = System.Configuration.ConfigurationManager.AppSettings["MfaSecretKey"].ToSecureString();
        //private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"]);

        [TestCategory("Navigation")]
        [TestMethod]
        public void TestOpenOptions()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.PerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(AppName.Sales);
                xrmApp.Navigation.OpenOptions();
                // xrmApp.Navigation.OpenOptInForLearningPath();
                // xrmApp.Navigation.OpenPrivacy();
                // xrmApp.Navigation.SignOut();
            }
        }

        [TestCategory("Navigation")]
        [TestMethod]
        public void TestOpenGuidedHelp()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.PerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(AppName.Sales);
                xrmApp.Navigation.OpenGuidedHelp();
                trace.Log("UCITestOpenGuidedHelp browser command count : " + client.CommandResults.Count());
            }
        }
       
        [TestCategory("Navigation")]
        [TestMethod]
        public void TestOpenSoftwareLicensing()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.PerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(AppName.Sales);
                xrmApp.Navigation.OpenSoftwareLicensing();
            }
        }

        [TestCategory("Navigation")]
        [TestMethod]
        public void TestOpenToastNotifications()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.PerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(AppName.Sales);
                xrmApp.Navigation.OpenToastNotifications();
            }
        }

        [TestCategory("Navigation")]
        [TestMethod]
        public void TestOpenAbout()
        {
            var client = new WebClient(TestSettings.Options);

            // The performance widget overlays on top of these settings. In order for them to be clickable, you have to disable PerformanceMode on these tests.
            // Otherwise, you get the following error: OpenQA.Selenium.ElementClickInterceptedException: element click intercepted
            client.Browser.Options.PerformanceMode = false;

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(AppName.Sales);
                xrmApp.Navigation.OpenAbout();
            }
        }

        [TestCategory("Navigation")]
        [TestMethod]
        public void TestOpenRelatedCommonActivities()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.OpenRecord(0);
                xrmApp.Navigation.OpenMenu(Navigation.NavigationReference.MenuRelatedReference.Related, Navigation.NavigationReference.MenuRelatedReference.CommonActivities);
            }
        }
    }
}