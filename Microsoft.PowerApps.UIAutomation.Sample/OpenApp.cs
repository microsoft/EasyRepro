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

            using (var appCanvas = new PowerAppBrowser(TestSettings.Options))
            {

                var parameters = new Dictionary<string, string>();

                appCanvas.OnlineLogin.Login(uri, username.ToSecureString(), password.ToSecureString());

                parameters.Add("loadtype", "cold");
                parameters.Add("origin", "performancebenchmark");

                //Cold Load
                //appCanvas.Player.OpenApp("892faefd-e1ed-4982-bc36-04a6ba2a7d2a", parameters);
                appCanvas.Player.OpenApp("2394b4e6-c62c-4b21-a880-18c1b31ba95f", parameters);

                appCanvas.ThinkTime(3000);

                //Navigate Away
                appCanvas.Navigation.Homepage();
                
                appCanvas.ThinkTime(3000);

                parameters["loadtype"] = "warm";
                
                //Warm Load
                appCanvas.Player.OpenApp("2394b4e6-c62c-4b21-a880-18c1b31ba95f", parameters);
            }
        }
    }
}
