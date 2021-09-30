// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenContactUCI
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly SecureString _mfaSecretKey = System.Configuration.ConfigurationManager.AppSettings["MfaSecretKey"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestOpenActiveContact()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.Search("Patrick");

                xrmApp.Grid.OpenRecord(0);
                
            }
            
        }

        [TestMethod]
        public void UCITestOpenRecordSetNavigator()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

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

        [TestMethod]
        [TestCategory("Fail - Bug")]
        public void UCITestOpenSubGridRecord()
        {
            var client = new WebClient(TestSettings.Options);

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                List<GridItem> rows = xrmApp.Entity.SubGrid.GetSubGridItems("contactopportunitiesgrid");

                int rowCount = xrmApp.Entity.SubGrid.GetSubGridItemsCount("contactcasessgrid");

                xrmApp.Entity.SubGrid.OpenSubGridRecord("contactopportunitiesgrid", 0);

                xrmApp.ThinkTime(500);
            }
        }

        [TestMethod]
        [TestCategory("Fail - Bug")]
        public void UCITestLookupSearch()
        {
            var client = new WebClient(TestSettings.Options);

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                LookupItem parentCustomerId = new LookupItem { Name = "parentcustomerid" };
                xrmApp.Entity.SelectLookup(parentCustomerId);

                // Bug: OpenQA.Selenium.NotFoundException: Lookup Entity Accounts not found
                xrmApp.Lookup.SelectRelatedEntity("Accounts");

                xrmApp.Lookup.SwitchView("My Active Accounts");

                xrmApp.Lookup.OpenRecord(0);

                xrmApp.ThinkTime(3000);
            }
        }

        [TestMethod]
        [TestCategory("Fail - Bug")]
        public void UCITestLookupNew()
        {
            var client = new WebClient(TestSettings.Options);

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                LookupItem parentCustomerId = new LookupItem { Name = "parentcustomerid" };
                xrmApp.Entity.SelectLookup(parentCustomerId);

                // Bug: OpenQA.Selenium.NotFoundException: Lookup Entity Accounts not found
                xrmApp.Lookup.SelectRelatedEntity("Accounts");

                xrmApp.Lookup.New();

                xrmApp.ThinkTime(3000);



            }
        }

        [TestCategory("Fail - Bug")]
        [TestMethod]
        public void UCITestOpenContactRetrieveHeaderValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Contacts");

                xrmApp.RelatedGrid.OpenGridRow(0);

                // Bug: Fails to resolve ownerid
                // OpenQA.Selenium.NoSuchElementException: no such element: Unable to locate element: {"method":"xpath","selector":"//div[@data-id='header_ownerId.fieldControl-Lookup_ownerId']"}
                LookupItem ownerId = new LookupItem() { Name = "ownerid" };
                string ownerIdValue = xrmApp.Entity.GetHeaderValue(ownerId);

                string emailAddress = xrmApp.Entity.GetHeaderValue("emailaddress1");

                xrmApp.ThinkTime(2000);

            }
        }

        [TestCategory("Fail - Bug")]
        [TestMethod]
        public void UCITestOpenContactRelatedEntity()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

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