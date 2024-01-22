// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class Grid : TestsBase
    {
        [TestMethod]
        public void GridSort()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.Sort("Main Phone", "Sort A to Z");
            }
        }

        [TestMethod]
        public void GridOpenRecord()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Customers", "Accounts");

                xrmApp.Grid.SwitchView("All Accounts");

                xrmApp.Grid.OpenRecord(0);
            }
        }

        [TestMethod]
        public void GridOpenRecordWithHierarchialRelationship()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("My Active Accounts");

                xrmApp.Grid.OpenRecord(1);
            }
        }
        [TestCategory("SubGrid")]
        [TestMethod]
        public void TestOpenSubGridRecordOnAccount()
        {
            var client = new WebClient(TestSettings.Options);

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.OpenRecord(0);

                // Reference schema name of the SubGrid
                xrmApp.Entity.SubGrid.GetSubGridItems("Contacts");

                xrmApp.ThinkTime(3000);
            }
        }
    }
}