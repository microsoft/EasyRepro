using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class ValidateVersion
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestGetVersion()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.OpenAbout();

                xrmBrowser.Driver.LastWindow().SwitchTo().ActiveElement();

                var dbVersion = xrmBrowser.Document.getElementByXPath("id(\"PageBody\")/table/tbody/tr[1]/td[2]/div/bdo[3]").Text;
                var appVersion = xrmBrowser.Document.getElementByXPath("id(\"PageBody\")/table/tbody/tr[1]/td[2]/div/bdo[2]").Text.Replace("(","").Replace(")","");

                Console.WriteLine("App Version: {0} , DB Version: {1}", appVersion, dbVersion);
            }
        }
    }
}
