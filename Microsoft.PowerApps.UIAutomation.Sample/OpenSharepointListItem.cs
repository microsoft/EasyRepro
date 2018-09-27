using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Configuration;
using System;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class OpenSharepointListItem
    {
        [TestMethod]
        public void TestOpenSharepointListItem()
        {
            var username = "admin@a830edad9050849822e18041200.onmicrosoft.com";
            var password = "Cl@!ms!2";
            var uri = new System.Uri("https://a830edad9050849822e18041200.sharepoint.com/PowerAppTest");

            using (var browser = new PowerAppBrowser(TestSettings.Options))
            {
                //Login
                browser.OnlineLogin.Login(uri, username.ToSecureString(), password.ToSecureString());

                //Click "Custom Form test"
                browser.Sharepoint.ClickLeftNavigationItem("Custom Form test");

                //Click "Testing this"
                browser.Sharepoint.ClickGridItem("Testing this");
               
            }
        }
    }
}

