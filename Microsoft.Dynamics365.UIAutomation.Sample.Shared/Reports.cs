using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class Reports
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestAccountOverviewReport()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Raporty");

                xrmBrowser.Grid.Search("Przegląd klientów");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.Report.RunReport(null);
            }
        }

        [TestMethod]
        public void TestAccountOverviewFromGrid()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Konta");

                xrmBrowser.CommandBar.ClickCommand("Uruchom raport", "Przegląd klientów");

                xrmBrowser.Dialogs.RunReport(XrmDialogPage.ReportRecords.AllRecords);
            }
        }
        [TestMethod]
        public void TestAccountOverviewFromRecord()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Konta");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.CommandBar.ClickCommand("Uruchom raport", "Przegląd klientów", true);
            }
        }

        [TestMethod]
        public void TestAccountOverviewWithCriteriaReport()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sprzedaż", "Raporty");

                xrmBrowser.Grid.Search("Przegląd klientów");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.Report.RunReport(SetFilterCriteria);

                xrmBrowser.ThinkTime(2000);

            }
        }

        private static void SetFilterCriteria(ReportEventArgs args)
        {
            //Modify Report Filter to change the default "Modified On" of 30 days to 15 days
            var criteria = args.Driver.FindElement(By.XPath("id(\"CRM_FilteredAccountEFGRP0FFLD0CCVALLBL\")"));
            criteria.Click();

            var input = args.Driver.FindElement(By.XPath("id(\"CRM_FilteredAccountEFGRP0FFLD0CCVALCTL\")"));
            input.SendKeys("15", true);

        }
    }
}