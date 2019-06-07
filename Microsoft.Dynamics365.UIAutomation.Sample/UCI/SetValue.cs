// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class SetValueUci
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestSetValue()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                
                
                xrmApp.Grid.SwitchView("My Active Accounts");
                xrmApp.Grid.OpenRecord(0);
                //xrmApp.Entity.SetValue("name", "Margaret");
            }
        }

        [TestMethod]
        public void UCITestSelectOptionSetValue()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(3000);

                xrmApp.Entity.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });
            }
        }

        [TestMethod]
        public void UCITestOpenLookupSetValue()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.SetValue(new LookupItem { Name = "primarycontactid", Value = "Test" });
                xrmApp.ThinkTime(500);
            }
        }

        [TestMethod]
        public void UCITestActivityPartySetValue()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.SetValue(new LookupItem[] {
                    new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)", Index = 0 },
                    new LookupItem { Name = "to", Value = "", Index = 0 } });
                xrmApp.ThinkTime(500);

                xrmApp.Entity.SetValue(new LookupItem[] {
                    new LookupItem { Name = "from", Value = "Rene Valdes (sample)", Index = 0 } });
                xrmApp.ThinkTime(500);

                xrmApp.Entity.Save();
            }
        }

        [TestMethod]
        public void UCITestActivityPartyAddValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.AddValues(new LookupItem[] {
                    new LookupItem { Name = "to", Value = "Adventure Works (sample)", Index = 0 },
                    new LookupItem { Name = "to", Value = "", Index = 1 } });
                xrmApp.ThinkTime(500);

                xrmApp.Entity.Save();
            }
        }

        [TestMethod]
        public void UCITestActivityPartyRemoveValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.RemoveValues(new LookupItem[] {
                    new LookupItem { Name = "to", Value = "Adventure Works (sample)", Index = 0 },
                    new LookupItem { Name = "to", Value = "", Index = 0 } });
                xrmApp.ThinkTime(500);

                xrmApp.Entity.Save();
            }
        }

        [TestMethod]
        public void UCITestActivityClearValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.ClearValue(new LookupItem { Name = "to" });
                xrmApp.ThinkTime(500);

                xrmApp.Entity.ClearValue(new LookupItem { Name = "from" });
                xrmApp.ThinkTime(500);
            }
        }

        [TestMethod]
        public void UCITestDateTimeSetValue()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.SetValue("lastonholdtime", DateTime.Now, "M/d/yyyy h:mm tt");
                xrmApp.ThinkTime(500);
            }
        }
    }
}