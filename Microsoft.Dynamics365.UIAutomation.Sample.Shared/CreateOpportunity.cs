using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class CreateOpportunity
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestCreateNewOpportunity()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Szanse sprzedaży");

                xrmBrowser.ThinkTime(1000);
                xrmBrowser.CommandBar.ClickCommand("Nowy");

                xrmBrowser.ThinkTime(5000);

                xrmBrowser.Entity.SetValue("name", "Test API Opportunity");
                xrmBrowser.Entity.SetValue("description", "Testing the create api for Opportunity");

                xrmBrowser.CommandBar.ClickCommand("Zapisz");
            }
        }
    }
}