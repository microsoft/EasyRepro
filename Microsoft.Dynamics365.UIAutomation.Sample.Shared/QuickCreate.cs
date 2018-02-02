using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Sample.Shared;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class QuickCreate: CrmTestBase
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestOpenCreate()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.QuickCreate(Reference.Localization.Contact);

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.QuickCreate.SetValue("lastname", "Contact");
                xrmBrowser.QuickCreate.SetValue("firstname", "Test Quick Create");

                xrmBrowser.QuickCreate.Save();
                xrmBrowser.ThinkTime(5000);
            }
        }
    }
}
