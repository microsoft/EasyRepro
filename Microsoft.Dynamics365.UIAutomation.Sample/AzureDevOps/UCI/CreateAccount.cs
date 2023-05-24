// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using OpenQA.Selenium;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Sample.AzureDevOps.UCI
{
    [TestClass]
    public class CreateAccountUCIDevOps
    {
        private static string _username = "";
        private static string _password = "";
        private static string _mfaSecret = "";
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
            _mfaSecret = _testContext.Properties["MfaSecretKey"].ToString();
            _xrmUri = new Uri(_testContext.Properties["OnlineCrmUrl"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
        }

        [TestCategory("EasyReproAutomation")]
        [TestMethod]
        public void UCIDevOpsCreateAccount()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {

                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString(), _mfaSecret.ToSecureString());
                TakeScreenshot(client, "Login with MFA");
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue("name", TestSettings.GetRandomString(5,15));

                xrmApp.Entity.Save();
                
            }
            
        }

        private string TakeScreenshot(WebClient client, string command)
        {
            ScreenshotImageFormat fileFormat = ScreenshotImageFormat.Bmp;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
            string strFileName = String.Format("{2}_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat, command.Replace(":", ""));
            client.Browser.TakeWindowScreenShot(strFileName, fileFormat);
            TestContext.AddResultFile(strFileName);
            return strFileName;
        }
    }
}