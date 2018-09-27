
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class Sidebar
    {
        [TestMethod]
        public void TestSidebarNavigate()
        {

            using (var appCanvas = new PowerAppBrowser(TestSettings.Options))
            {
                var username = "easyrepro@pfecrmonline.onmicrosoft.com";
                var password = "R@ngers2017";

                appCanvas.OnlineLogin.Login(new System.Uri("https://web.powerapps.com"), username.ToSecureString(), password.ToSecureString());

                appCanvas.SideBar.ExpandCollapse(500);
                appCanvas.SideBar.Navigate("Apps");
            }
        }
    }
}
