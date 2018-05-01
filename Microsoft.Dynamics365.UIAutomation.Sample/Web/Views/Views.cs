// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class Views
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestSwitchView()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {

                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");

            }
        }


        [TestMethod]
        public void WEBTestGetGridItems()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {

                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");
                xrmBrowser.Grid.GetGridItems();
                xrmBrowser.ThinkTime(1000);

            }
        }

        [TestMethod]
        public void WEBTestOpenGridRecord()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");
                xrmBrowser.Grid.OpenRecord(0);

            }
        }

        [TestMethod]
        public void WEBTestSortGridRow()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");
                xrmBrowser.Grid.Sort("Account Name");

                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestSelectRecord()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");

                xrmBrowser.ThinkTime(200);

                xrmBrowser.Grid.SelectRecord(1);
                xrmBrowser.ThinkTime(5000);

            }
        }

        [TestMethod]
        public void WEBTestSelectAllRecords()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");


                xrmBrowser.Grid.SelectAllRecords();
                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestFilterGridByLetter()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");
                xrmBrowser.Grid.FilterByLetter('A');
                xrmBrowser.ThinkTime(5000);

            }
        }

        [TestMethod]
        public void WEBTestFilterGridByAll()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                xrmBrowser.Grid.SwitchView("Open Leads");
                xrmBrowser.Grid.FilterByAll();
                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestEnableFilter()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                xrmBrowser.Grid.SwitchView("Open Leads");
                xrmBrowser.Grid.EnableFilter();
                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestGridNextPage()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                xrmBrowser.Grid.SwitchView("All Leads");

                xrmBrowser.Grid.NextPage();
                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestGridPreviousPage()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                xrmBrowser.Grid.SwitchView("Open Leads");
                xrmBrowser.Grid.PreviousPage();

                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestGridFirstPage()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                xrmBrowser.Grid.SwitchView("Open Leads");

                xrmBrowser.Grid.FirstPage();
                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestOpenChart()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                xrmBrowser.Grid.SwitchView("Open Leads");
                xrmBrowser.Grid.OpenChart();

                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestSwitchChart()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");
                xrmBrowser.Grid.OpenChart();
                xrmBrowser.Grid.SwitchChart("Accounts by Owner");

                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestRefreshGrid()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                xrmBrowser.Grid.SwitchView("Active Accounts");
                xrmBrowser.Grid.Refresh();

                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestCloseChart()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                xrmBrowser.Grid.SwitchView("Open Leads");
                xrmBrowser.Grid.OpenChart();
                xrmBrowser.Grid.CloseChart();

                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestQuickFindSearch()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                xrmBrowser.Grid.SwitchView("Open Leads");

                xrmBrowser.Grid.Search("Test");

                xrmBrowser.ThinkTime(10000);

            }
        }

        [TestMethod]
        public void WEBTestPinDefaultView()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                xrmBrowser.Grid.SwitchView("Open Leads");
                xrmBrowser.Grid.Pin();

                xrmBrowser.ThinkTime(10000);
            }
        }
    }
}