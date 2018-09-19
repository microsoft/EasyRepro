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

                xrmBrowser.Navigation.OpenSubArea("Sales", "Reports");

                xrmBrowser.Grid.Search("Account Overview");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.Report.RunReport(null);

                xrmBrowser.ThinkTime(2000);

            }
        }

        [TestMethod]
        public void TestAccountOverviewFromGrid()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.CommandBar.ClickCommand("Run Report", "Account Overview");

                xrmBrowser.Dialogs.RunReport(XrmDialogPage.ReportRecords.AllRecords);

                xrmBrowser.ThinkTime(2000);

            }
        }
        [TestMethod]
        public void TestAccountOverviewFromRecord()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.CommandBar.ClickCommand("Run Report", "Account Overview", true);


                xrmBrowser.ThinkTime(2000);

            }
        }

        [TestMethod]
        public void TestAccountOverviewWithCriteriaReport()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Reports");

                xrmBrowser.Grid.Search("Account Overview");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.Report.RunReport(SetFilterCriteria);

                xrmBrowser.ThinkTime(2000);

            }
        }

        public void SetFilterCriteria(ReportEventArgs args)
        {
            //Modify Report Filter to change the default "Modified On" of 30 days to 15 days
            var criteria = args.Driver.FindElement(By.XPath("id(\"CRM_FilteredAccountEFGRP0FFLD0CCVALLBL\")"));
            criteria.Click();

            var input = args.Driver.FindElement(By.XPath("id(\"CRM_FilteredAccountEFGRP0FFLD0CCVALCTL\")"));
            input.SendKeys("15", true);

        }
    }
}