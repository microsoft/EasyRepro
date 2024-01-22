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
    public class OpenAccountUCI : TestsBase
    {
        [TestCategory("Grid")]
        [TestCategory("EditableGrid")]
        [TestCategory("GridSwitchView")]
        [TestCategory("GridSearch")]
        [TestMethod]
        public void TestOpenActiveAccount()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                //xrmApp.Grid.Search("A. Datum");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(3000);

            }
        }

        [TestCategory("Grid")]
        [TestCategory("GridItems")]
        [TestCategory("GridSort")]
        [TestMethod]
        public void TestGetActiveGridItems()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.GetGridItems();

                xrmApp.Grid.Sort("Account Name", "Sort Z to A");

                xrmApp.ThinkTime(3000);
            }
        }

        [TestCategory("Grid")]
        [TestMethod]
        public void TestOpenTabDetails()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("All Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(3000);

                xrmApp.Entity.SelectTab("Details");

                xrmApp.Entity.SelectTab("Related","Contacts");

                xrmApp.ThinkTime(3000);
            }
        }

        [TestCategory("Grid")]
        [TestCategory("EntityGetObjectId")]
        [TestMethod]
        public void TestGetObjectId()
        {
            var client = new WebClient(TestSettings.Options);

            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                Guid objectId = xrmApp.Entity.GetObjectId();

                xrmApp.ThinkTime(3000);
            }
        }

        [TestCategory("Grid")]
        [TestCategory("SubGrid")]
        [TestMethod]
        public void TestOpenSubGridRecord()
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