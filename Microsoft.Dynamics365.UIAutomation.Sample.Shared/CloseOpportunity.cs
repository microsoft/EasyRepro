using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Sample.Shared;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class CloseOpportunity: CrmTestBase
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestCloseOpportunity()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea(Reference.Localization.Sales, Reference.Localization.Opportunities);

                xrmBrowser.ThinkTime(2000);
                xrmBrowser.Grid.SwitchView(Reference.Localization.OpenOpportunities);

                xrmBrowser.ThinkTime(1000);
                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.CommandBar.ClickCommand(Reference.Localization.CloseWon);

                xrmBrowser.Dialogs.CloseOpportunity(10000, DateTime.Now, "Testing the Close Opportunity API");
                xrmBrowser.ThinkTime(5000);
            }
        }
    }
}