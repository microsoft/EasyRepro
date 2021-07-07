// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class GetValueUci
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly SecureString _mfaSecretKey = System.Configuration.ConfigurationManager.AppSettings["MfaSecretKey"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestGetValueFromOptionSet()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.SwitchView("Active Contacts");

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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(2000);
                string lookupValue = xrmApp.Entity.GetValue(new LookupItem { Name = "primarycontactid" });
            }
        }

        [TestMethod]
        public void UCITestGetValueFromLookup_MultiAndSingle_ReturnsTheSameResult()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(2000);

                var primaryContactLookupItem = new LookupItem { Name = "primarycontactid" };

                string lookupValue = xrmApp.Entity.GetValue(primaryContactLookupItem);
                Debug.WriteLine($"Single-Value: {lookupValue ?? "null"}");

                string[] lookupValues = xrmApp.Entity.GetValue(new[] { primaryContactLookupItem });
                Assert.IsNotNull(lookupValues);
                Assert.IsTrue(lookupValues.Length == 0 && lookupValue == string.Empty || string.Equals(lookupValue, lookupValues[0]));

                Debug.WriteLine($"Multi-Value: {lookupValues.FirstOrDefault() ?? "null"}");
            }
        }

        [TestMethod]
        public void UCITestActivityPartyGetValue()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("My Work", "Activities");

                xrmApp.Grid.SwitchView("All Phone Calls");
                xrmApp.ThinkTime(500);

                xrmApp.Grid.OpenRecord(0);
                xrmApp.ThinkTime(500);

                var to = xrmApp.Entity.GetValue(new[] { new LookupItem { Name = "to" } });
                Assert.IsNotNull(to);
                xrmApp.ThinkTime(500);
            }
        }

        [TestMethod]
        public void UCITestGetValueFromDateTime()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");
                
                xrmApp.ThinkTime(500);

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue("name", TestSettings.GetRandomString(5, 10));

                var dateTime = DateTime.Today;
                xrmApp.Entity.SetHeaderValue("estimatedclosedate", dateTime);
                xrmApp.ThinkTime(500);

                var estimatedclosedate = xrmApp.Entity.GetHeaderValue(new DateTimeControl("estimatedclosedate"));
                Assert.AreEqual(dateTime, estimatedclosedate);
                xrmApp.ThinkTime(500);
            }
        }
    }
}
