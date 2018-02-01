using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class Views
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestSortGridRow()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Konta");
                xrmBrowser.Grid.SwitchView("Aktywne konta");
                xrmBrowser.Grid.Sort("Nazwa konta");
                xrmBrowser.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void TestFilterGridByLetter()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Konta");
                xrmBrowser.Grid.SwitchView("Aktywne konta");
                xrmBrowser.Grid.FilterByLetter('A');
                xrmBrowser.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void TestFilterGridByAll()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Potencjalni klienci");
                xrmBrowser.Grid.SwitchView("Otwarci potencjalni klienci");
                xrmBrowser.Grid.FilterByAll();
                xrmBrowser.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void TestEnableFilter()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Potencjalni klienci");
                xrmBrowser.Grid.SwitchView("Otwarci potencjalni klienci");
                xrmBrowser.Grid.EnableFilter();
                xrmBrowser.ThinkTime(2000);
            }
        }
    }
}