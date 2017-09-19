using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Sample;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.DOCValidationTests
{
    [TestClass]
    public class ValidateVersion
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestValidateVersion()
        {
            var validAppVersion = "8.2.1.360";
            var validDBVersion = "8.2.1.360";

            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.OpenAbout();

                xrmBrowser.Driver.LastWindow().SwitchTo().ActiveElement();

                var dbVersion = xrmBrowser.Document.getElementByXPath("id(\"PageBody\")/table/tbody/tr[1]/td[2]/div/bdo[3]").Text;
                var appVersion = xrmBrowser.Document.getElementByXPath("id(\"PageBody\")/table/tbody/tr[1]/td[2]/div/bdo[2]").Text.Replace("(","").Replace(")","");


                if (dbVersion != validDBVersion) throw new Exception( $"The database verion ({dbVersion}) is not valid for this org. The version should be {validDBVersion}");
                if(appVersion != validAppVersion) throw new Exception($"The application verion ({appVersion}) is not valid for this org. The version should be {validAppVersion}");


            }
        }
    }
}
