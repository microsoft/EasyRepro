using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class OpenApp
    {

        [TestMethod]
        public void TestOpenApp()
        {
            var username = ConfigurationManager.AppSettings["OnlineUsername"];
            var password = ConfigurationManager.AppSettings["OnlinePassword"];
            var uri = new System.Uri("https://web.powerapps.com");
            var appId = ConfigurationManager.AppSettings["AppId"];

            using (var appCanvas = new PowerAppBrowser(TestSettings.Options))
            {
                appCanvas.OnlineLogin.Login(uri, username.ToSecureString(), password.ToSecureString());

                //Cold Load
                appCanvas.Player.OpenApp(appId);
                appCanvas.ThinkTime(3000);
            }
        }
    }
}
