// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class Reports
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestAccountOverviewReport()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
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
        public void WEBTestAccountOverviewFromGrid()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.CommandBar.ClickCommand("Run Report", "Account Overview");

                xrmBrowser.Dialogs.RunReport(Dialog.ReportRecords.AllRecords);

                xrmBrowser.ThinkTime(2000);

            }
        }

        [TestMethod]
        public void WEBTestAccountOverviewFromRecord()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.CommandBar.ClickCommand("Run Report", "Account Overview",true);
                

                xrmBrowser.ThinkTime(2000);

            }
        }

        [TestMethod]
        public void WEBTestAccountOverviewWithCriteriaReport()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Reports");

                xrmBrowser.Grid.Search("Account Overview");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.Report.RunReport(this.SetFilterCriteria);

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