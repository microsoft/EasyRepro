// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class SubGrid
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestOpenSubGridRecord()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.Grid.SwitchView("Active Accounts");

                xrmBrowser.Grid.OpenRecord(0);

                //This uses the unique name (not label) from the subgrid properties
                xrmBrowser.Entity.OpenSubGridRecord("CONTACTS", 0);

                xrmBrowser.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void WEBTestDeleteSubGridRecord()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.Grid.SwitchView("Active Accounts");

                xrmBrowser.Grid.OpenRecord(0);

                //This uses the unique name (not label) from the subgrid properties
                xrmBrowser.Entity.DeleteSubGridRecord("CONTACTS", 0);

                xrmBrowser.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void WEBTestGetSubGridItems()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.Grid.SwitchView("Active Accounts");

                xrmBrowser.Grid.OpenRecord(0);

                //This uses the unique name (not label) from the subgrid properties
                List<GridItem> contacts = xrmBrowser.Entity.GetSubGridItems("CONTACTS");

                //This uses the unique name (not label) from the subgrid properties
                var contactCount = xrmBrowser.Entity.GetSubGridItemsCount("CONTACTS");

                xrmBrowser.ThinkTime(2000);
            }
        }
    }
}
