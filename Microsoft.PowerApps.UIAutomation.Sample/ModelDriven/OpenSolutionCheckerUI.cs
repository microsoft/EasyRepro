using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Security;

namespace Microsoft.PowerApps.UIAutomation.Sample.ModelDriven
{
    [TestClass]
    public class OpenSolutionCheckerUI
    {
        private readonly SecureString _username = ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(ConfigurationManager.AppSettings["OnlineUrl"].ToString());

        [TestMethod]
        public void TestOpenSolutionCheckerUI()
        {
            using (var appBrowser = new PowerAppBrowser(TestSettings.Options))
            {

                appBrowser.OnlineLogin.Login(_xrmUri, _username, _password);

                appBrowser.ThinkTime(10000);

                appBrowser.SideBar.ChangeDesignMode("Model-driven");

                appBrowser.ThinkTime(5000);

            }
        }
    }
}
