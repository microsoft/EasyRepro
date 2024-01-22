// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class OpenContact : TestsBase
    {

        [TestCategory("Grid")]
        [TestMethod]
        public void TestOpenActiveContact()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.Search("Patrick");

                xrmApp.Grid.OpenRecord(0);

            }

        }

        [TestCategory("Grid")]
        [TestCategory("OpenRecordSetNavigator")]
        [TestCategory("RecordSetNavigator")]
        [TestMethod]
        public void TestOpenRecordSetNavigator()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.SwitchView("Active Contacts");

                xrmApp.Grid.OpenRecord(0);

                // open record set navigator and go to 3rd record
                xrmApp.Entity.OpenRecordSetNavigator(2);

                xrmApp.Entity.CloseRecordSetNavigator();

                // open record set navigator and go to 1st record
                xrmApp.Entity.OpenRecordSetNavigator();

                // without closing the record set navigator go to 2nd record
                xrmApp.Entity.OpenRecordSetNavigator(1);

                //xrmApp.Entity.CloseRecordSetNavigator();
            }
        }

        [TestCategory("Grid")]
        [TestCategory("SubGrid")]
        [TestMethod]
        [TestCategory("Fail - Bug")]
        public void TestOpenSubGridRecord()
        {
            var client = new WebClient(TestSettings.Options);

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                List<GridItem> rows = xrmApp.Entity.SubGrid.GetSubGridItems("contactopportunitiesgrid");

                int rowCount = xrmApp.Entity.SubGrid.GetSubGridItemsCount("contactcasessgrid");

                xrmApp.Entity.SubGrid.OpenSubGridRecord("contactopportunitiesgrid", 0);

                xrmApp.ThinkTime(500);
            }
        }

        [TestCategory("Grid")]
        [TestCategory("PowerAppsGridControl")]
        [TestCategory("Lookup")]
        [TestCategory("LookupSelectedRelatedEntity")]
        [TestCategory("LookupSwitchView")]
        [TestCategory("LookupOpenRecord")]
        [TestCategory("LookupNew")]
        [TestMethod]
        [TestCategory("RegressionTests")]
        public void TestLookupSearch()
        {
            var client = new WebClient(TestSettings.Options);

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                LookupItem parentCustomerId = new LookupItem { Name = "parentcustomerid" };
                xrmApp.Entity.SelectLookup(parentCustomerId);

                xrmApp.Lookup.SelectRelatedEntity("Accounts");
               
                xrmApp.Lookup.SwitchView("My Active Accounts");

                xrmApp.Lookup.OpenRecord(0);

                xrmApp.ThinkTime(3000);
            }
        }

        [TestCategory("Grid")]
        [TestMethod]
        public void TestLookupNew()
        {
            var client = new WebClient(TestSettings.Options);

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                LookupItem parentCustomerId = new LookupItem { Name = "parentcustomerid" };
                xrmApp.Entity.SelectLookup(parentCustomerId);

                xrmApp.Lookup.SelectRelatedEntity("Accounts");

                xrmApp.Lookup.New();

                xrmApp.ThinkTime(3000);
            }
        }

        [TestCategory("RelatedGrid")]
        [TestMethod]
        public void TestOpenContactRetrieveHeaderValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Contacts");

                xrmApp.RelatedGrid.OpenGridRow(0);

                LookupItem ownerId = new LookupItem() { Name = "ownerid" };
                string ownerIdValue = xrmApp.Entity.GetHeaderValue(ownerId);

                string emailAddress = xrmApp.Entity.GetHeaderValue("emailaddress1");

                xrmApp.ThinkTime(2000);
            }
        }

        [TestCategory("Grid")]
        [TestCategory("Fail - Bug")]
        [TestMethod]
        public void TestOpenContactRelatedEntity()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Entity.SelectTab("Related", "Leads");

                // Bug: OpenQA.Selenium.NotFoundException: Excel Templates button not found. Button names are case sensitive. Please check for proper casing of button name.
                xrmApp.RelatedGrid.ClickCommand("Excel Templates", "View All My Templates");

                xrmApp.ThinkTime(2000);

            }
        }
    }
}