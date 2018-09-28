using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class ExpandCollapse
    {
        [TestMethod]
        public void TestExpandCollapse()
        {
            using (var appCanvas = new PowerAppBrowser(TestSettings.Options))
            {
                var username = ConfigurationManager.AppSettings["OnlineUsername"];
                var password = ConfigurationManager.AppSettings["OnlinePassword"];

                appCanvas.OnlineLogin.Login(new System.Uri("https://web.powerapps.com"), username.ToSecureString(), password.ToSecureString());

                appCanvas.SideBar.ExpandCollapse(500);
            }
        }
    }
}
