// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class OpenOpportunityUCIc: TestsBase
    {

        [TestCategory("Grid")]
        [TestMethod]
        public void TestOpenActiveOpportunity()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.Grid.Search("Will be ordering");

                xrmApp.Grid.OpenRecord(0);

            }

        }

        [TestCategory("Grid")]
        [TestMethod]
        public void TestOpenOpportunityLookupAccount()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.Grid.SwitchView("Open Opportunities");

                xrmApp.Grid.OpenRecord(0);

                LookupItem parentAccountId = new LookupItem { Name = "parentaccountid" };
                xrmApp.Entity.SelectLookup(parentAccountId);

                xrmApp.ThinkTime(1000);
                xrmApp.Lookup.OpenRecord(0);

            }
        }

        [TestCategory("Grid")]
        [TestCategory("Lookup")]
        [TestCategory("LookupSearch")]
        [TestMethod]
        public void TestOpenOpportunitySearchLookupAccount()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.Grid.SwitchView("Open Opportunities");

                xrmApp.Grid.OpenRecord(0);

                LookupItem parentAccountId = new LookupItem { Name = "parentaccountid" };
                xrmApp.Lookup.Search(parentAccountId, "Adventure");

                xrmApp.ThinkTime(1000);
            }
        }
    }
}