// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class ScreenShot
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestTakeScreenshot()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                ScreenshotImageFormat fileFormat = ScreenshotImageFormat.Tiff;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                string strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                xrmBrowser.TakeWindowScreenShot(strFileName, fileFormat);
                if(!File.Exists(strFileName))
                {
                    Assert.Fail(String.Format("Following file '{0}' was not found", strFileName));
                }
                else
                {
                    Console.WriteLine(Path.GetFullPath(strFileName));
                }
            }
        }
    }
}