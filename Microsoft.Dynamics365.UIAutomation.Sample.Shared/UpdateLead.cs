using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Api;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Sample.Shared;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class UpdateLead: CrmTestBase
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestUpdateLead()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea(Reference.Localization.Sales, Reference.Localization.Leads);

                xrmBrowser.Grid.SwitchView(Reference.Localization.AllLeads);

                xrmBrowser.Grid.OpenRecord(0);

               
                xrmBrowser.Entity.SetValue("subject", "Update test API Lead");
                xrmBrowser.Entity.SetValue("description", "Test lead updation with API commands");

                xrmBrowser.Entity.Save();
                xrmBrowser.ThinkTime(5000);
            }
        }
    }
}
