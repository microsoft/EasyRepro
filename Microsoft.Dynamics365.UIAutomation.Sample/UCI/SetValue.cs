// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class SetValueUci : TestsBase
    {
        [TestMethod]
        public void UCITestSetValue()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.SetValue(new LookupItem { Name = "primarycontactid", Value = "Nancy Anderson (sample)" });
                xrmApp.ThinkTime(500);
            }
        }

        [TestMethod]
        public void UCITestActivityPartySetValue()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.SetValue(new LookupItem[] {
                    new LookupItem { Name = "to", Value = "Adventure Works", Index = 0 },
                    new LookupItem { Name = "to", Value = "", Index = 0 } });
                xrmApp.ThinkTime(500);

                xrmApp.Entity.SetValue(new LookupItem[] {
                    new LookupItem { Name = "from", Value = "Rene Valdes (sample)", Index = 0 } });
                xrmApp.ThinkTime(500);

                xrmApp.Entity.Save();
            }
        }

        [TestMethod]
        public void UCITestActivityPartySetValue_CaseSensitive()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.SetValue(new [] {
                    new LookupItem { Name = "to", Value = "Adventure Works", Index = 0 },
                    new LookupItem { Name = "to", Value = "Nana Bule", Index = 0 } });
                xrmApp.ThinkTime(500);

                string toValue = xrmApp.Entity.GetValue(new LookupItem { Name = "to" });
                Assert.IsTrue(toValue.Contains("Adventure Works"));
                Assert.IsFalse(toValue.Contains("adventure works"));

                Assert.IsTrue(toValue.Contains("Nana Bule"));
                Assert.IsFalse(toValue.Contains("nana bule"));
            }
        }

        [TestMethod]
        public void UCITestActivityPartyAddValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.AddValues(new LookupItem[] {
                    new LookupItem { Name = "to", Value = "Adventure Works", Index = 0 },
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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.RemoveValues(new LookupItem[] {
                    new LookupItem { Name = "to", Value = "Adventure Works", Index = 0 },
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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");
                xrmApp.ThinkTime(500);

                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(500);

                xrmApp.Entity.SetValue("name", "Test EasyRepro Opportunity");

                xrmApp.Entity.SetHeaderValue("estimatedclosedate", DateTime.Now);
                xrmApp.ThinkTime(500);

                xrmApp.Entity.Save();

                xrmApp.ThinkTime(2000);
            }
        }
    }
}