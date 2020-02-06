// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class GetValueUci: TestsBase
    {
        [TestMethod]
        public void UCITestGetValueFromOptionSet()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                var options = xrmApp.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode" });
            }

        }

        [TestMethod]
        public void UCITestGetValueFromLookup()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(2000);
                string lookupValue = xrmApp.Entity.GetValue(new LookupItem { Name = "primarycontactid" });
            }
        }

        [TestMethod]
        public void UCITestActivityPartyGetValue()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                var to = xrmApp.Entity.GetValue(new LookupItem[] { new LookupItem { Name = "to" } });
                xrmApp.ThinkTime(500);
            }
        }

        [TestMethod]
        public void UCITestGetValueFromDateTime()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");
                xrmApp.ThinkTime(500);

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue("name", "Test EasyRepro Opportunity");

                xrmApp.Entity.SetValue("estimatedclosedate", DateTime.Now, "M/d/yyyy h:mm tt");
                xrmApp.ThinkTime(500);

                xrmApp.Entity.Save();

                xrmApp.ThinkTime(2000);            

                var estimatedclosedate = xrmApp.Entity.GetValue("estimatedclosedate");
                xrmApp.ThinkTime(500);
            }
        }
    }
}
