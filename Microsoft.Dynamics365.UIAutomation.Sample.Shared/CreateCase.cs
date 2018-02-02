using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Sample.Shared;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class CreateCase : CrmTestBase
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestCreateNewCase()
        {

            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea(Reference.Localization.Sales, Reference.Localization.Accounts);

                xrmBrowser.ThinkTime(3000);
                xrmBrowser.Grid.OpenRecord(0);
                xrmBrowser.Navigation.OpenRelated(Reference.Localization.Cases);

                xrmBrowser.Related.SwitchView(Reference.Localization.ActiveCases);
                xrmBrowser.ThinkTime(2000);

                xrmBrowser.Related.ClickCommand(Reference.Localization.AddNewCase);
                xrmBrowser.ThinkTime(2000);


                xrmBrowser.QuickCreate.SetValue("title", "Test API Case");
                xrmBrowser.QuickCreate.Save();
                xrmBrowser.ThinkTime(5000);
            }
        }
    }
}
