using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using System;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Sample.Shared;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class CreateAccountTest : CrmTestBase
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"]);

        [TestMethod]
        public void TestCreateNewAccount()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea(Reference.Localization.Sales, Reference.Localization.Accounts);

                xrmBrowser.ThinkTime(2000);
                xrmBrowser.Grid.SwitchView(Reference.Localization.ActiveAccounts);

                xrmBrowser.ThinkTime(1000);
                xrmBrowser.CommandBar.ClickCommand(Reference.Localization.New);

                xrmBrowser.ThinkTime(4000);
                xrmBrowser.Entity.SetValue("name", "Test API Account " + DateTime.Now);
                xrmBrowser.Entity.SetValue("telephone1", "555-555-5555");
                xrmBrowser.Entity.SetValue("websiteurl", "https://easyrepro.crm.dynamics.com");

                xrmBrowser.CommandBar.ClickCommand(Reference.Localization.SaveClose);
                xrmBrowser.ThinkTime(2000);
            }
        }
    }
}