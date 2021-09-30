// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class UpdateAccount
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestUpdateAccount()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp(5000);
                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.ThinkTime(2000);
                xrmBrowser.Grid.SwitchView("Active Accounts");

                xrmBrowser.ThinkTime(1000);
                xrmBrowser.Grid.OpenRecord(0);

                var parentAccountId = new LookupItem { Name = "parentaccountid", Value = "Litware, Inc. (sample)", Index = 0 };

                // Field Has No Data
                xrmBrowser.Entity.SelectLookup(parentAccountId);
                xrmBrowser.Entity.Save();
                xrmBrowser.ThinkTime(2000);

                // Field Has Existing Data
                parentAccountId.Index = 1; // Set Index to 1 to select the 2nd record
                xrmBrowser.Entity.SelectLookup(parentAccountId);
                xrmBrowser.Entity.Save();
                xrmBrowser.ThinkTime(2000);

                // Remove value for SetValue tests
                xrmBrowser.Entity.ClearValue(parentAccountId);
                xrmBrowser.Entity.Save();
                xrmBrowser.ThinkTime(2000);

                // Field Has No Data
                xrmBrowser.Entity.SetValue(parentAccountId);
                xrmBrowser.Entity.Save();
                xrmBrowser.ThinkTime(2000);

                // Field Has Existing Data
                parentAccountId.Value = "Fabrikam, Inc."; // Set the value of parentAccountId to select a different record.
                xrmBrowser.Entity.SetValue(parentAccountId);
                xrmBrowser.Entity.Save();

                xrmBrowser.ThinkTime(2000);
            }
        }
    }
}
