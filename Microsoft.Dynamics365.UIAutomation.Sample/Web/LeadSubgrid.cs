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
    public class LeadSubgrid
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestClickSubGridInLead()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();
                
                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");

                xrmBrowser.Grid.SwitchView("Open Leads");
                
                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.Entity.ClickSubgridAddButton("Stakeholders");
                xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", "Maria Campbell");
                xrmBrowser.Entity.ClickSubgridAddButton("Stakeholders");
                xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", 3);
                xrmBrowser.Entity.ClickSubgridAddButton("Stakeholders");
                xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", true);
                xrmBrowser.Lookup.SelectItem(0);
                xrmBrowser.Lookup.Select();
                xrmBrowser.Lookup.SelectItem("Maria Campbell");
                xrmBrowser.Lookup.Select();
                xrmBrowser.Lookup.Add();

            }
        }
    }
}