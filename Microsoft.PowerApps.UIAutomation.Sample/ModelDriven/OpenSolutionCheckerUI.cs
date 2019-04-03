// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.PowerApps.UIAutomation.Sample.ModelDriven
{
    [TestClass]
    public class OpenSolutionCheckerUI
    {
        private static string _username = "";
        private static string _password = "";
        private static BrowserType _browserType;
        private static Uri _xrmUri;
        public TestContext TestContext { get; set; }

        private static TestContext _testContext;

        [ClassInitialize]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;

            _username = _testContext.Properties["OnlineUsername"].ToString();
            _password = _testContext.Properties["OnlinePassword"].ToString();
            _xrmUri = new System.Uri(_testContext.Properties["OnlineUrl"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());

        }

        [TestMethod]
        public void TestOpenSolutionCheckerUI()
        {
            using (var appBrowser = new PowerAppBrowser(TestSettings.Options))
            {

                appBrowser.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());

                appBrowser.ThinkTime(10000);

                appBrowser.Navigation.ChangeEnvironment("Customization Advisor Development");

                appBrowser.SideBar.ChangeDesignMode("Model-driven");

                appBrowser.ThinkTime(5000);

                
            }
        }

        [TestMethod]
        public void TestLogin()
        {
            using (var appBrowser = new PowerAppBrowser(TestSettings.Options))
            {

                appBrowser.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());

                appBrowser.ThinkTime(5000);


            }
        }
    }
}
