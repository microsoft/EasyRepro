// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class GetValue
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestGetValueFromOpenActiveLead()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");

                xrmBrowser.Grid.SwitchView("All Leads");

                xrmBrowser.Grid.OpenRecord(0);

                string subject = xrmBrowser.Entity.GetValue("subject");
                string mobilePhone = xrmBrowser.Entity.GetValue("mobilephone");
            }
        }

        public void WEBTestGetValueFromCompositeControl()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");

                xrmBrowser.Grid.SwitchView("All Leads");

                xrmBrowser.Grid.OpenRecord(0);

                List<Field> fields = new List<Field>
                {
                    new Field() {Id = "firstname"},
                    new Field() {Id = "lastname"}
                };
                string fullName = xrmBrowser.Entity.GetValue(new CompositeControl() { Id = "fullname", Fields = fields });
            }
        }


        [TestMethod]
        public void WEBTestGetValueFromOptionSet()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Sales", "Contacts");

                xrmBrowser.ThinkTime(2000);
                xrmBrowser.Grid.SwitchView("Active Contacts");

                xrmBrowser.ThinkTime(5000);
                xrmBrowser.Grid.OpenRecord(0);

                string birthDate = xrmBrowser.Entity.GetValue("birthdate");
                string options = xrmBrowser.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"});
                

            }
        }

        [TestMethod]
        public void WEBTestGetValueFromLookup()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.ThinkTime(2000);
                xrmBrowser.Grid.SwitchView("Active Accounts");

                xrmBrowser.ThinkTime(5000);
                xrmBrowser.Grid.OpenRecord(0);

                string lookupValue = xrmBrowser.Entity.GetValue(new LookupItem { Name = "primarycontactid" });

            }
        }
    }
}