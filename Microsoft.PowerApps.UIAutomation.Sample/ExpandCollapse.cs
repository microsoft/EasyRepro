using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                var username = "easyrepro@pfecrmonline.onmicrosoft.com";
                var password = "R@ngers2017";

                appCanvas.OnlineLogin.Login(new System.Uri("https://web.powerapps.com"), username.ToSecureString(), password.ToSecureString());

                appCanvas.SideBar.ExpandCollapse(500);
            }
        }
    }
}
