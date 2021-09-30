// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class UpdateLead
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestUpdateLead()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");

                xrmBrowser.CommandBar.ClickCommand("New");

                var testLeadName = "Test EasyRepro Topic";
                List<Field> fields = new List<Field>
                {
                    new Field() {Id = "firstname", Value = "EasyRepro"},
                    new Field() {Id = "lastname", Value = "Lead Test"}
                };


                xrmBrowser.Entity.SetValue("subject", testLeadName);
                xrmBrowser.Entity.SetValue(new CompositeControl { Id = "fullname", Fields = fields });

                xrmBrowser.Entity.Save();
                xrmBrowser.Entity.CloseEntity();

                xrmBrowser.Grid.SwitchView("All Leads");

                xrmBrowser.Grid.Search(testLeadName);

                xrmBrowser.Grid.OpenRecord(0);
               
                xrmBrowser.Entity.SetValue("subject", "Update Test EasyRepro Topic");
                xrmBrowser.Entity.SetValue("description", "Test lead updated with API commands");

                xrmBrowser.Entity.Save();
            }
        }
    }
}
