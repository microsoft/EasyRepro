using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Api;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class UpdateOpportunity
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestUpdateOpportunity()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Szanse sprzedaży");

                xrmBrowser.ThinkTime(200);
                xrmBrowser.Grid.SwitchView("Otwarte szanse sprzedaży");


                xrmBrowser.ThinkTime(1000);
                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.Entity.SetValue("description", "Testing the update api for Opportunity");

                xrmBrowser.Entity.Save();
                xrmBrowser.ThinkTime(2000);
            }
        }
    }
}
