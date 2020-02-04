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
    public class OpenCaseUCI
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestOpenActiveCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.Search("Contacted");

                xrmApp.Grid.OpenRecord(0);
            }
        }

        [TestMethod]
        public void UCITestOpenCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.OpenRecord(0);

                Field ticketNumber = xrmApp.Entity.GetField("ticketnumber");
                Field subject = xrmApp.Entity.GetField("subjectid");
                Field description = xrmApp.Entity.GetField("description");
                Field mobilePhone = xrmApp.Entity.GetField("mobilephone");
            }
        }

        [TestMethod]
        public void UCITestOpenCaseById()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                // For proper test usage, please update the recordId below to a valid Case recordId
                Guid recordId = new Guid("ba9a3931-675d-e911-a852-000d3a372393");

                xrmApp.Entity.OpenEntity("incident", recordId);
            }
        }

        [TestMethod]
        public void UCITestOpenCaseRetrieveHeaderValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);

                LookupItem ownerId = new LookupItem { Name = "ownerid" };
                string ownerIdValue = xrmApp.Entity.GetHeaderValue(ownerId);

                OptionSet priorityCode = new OptionSet { Name = "prioritycode" };
                string priorityCodeValue = xrmApp.Entity.GetHeaderValue(priorityCode);

                xrmApp.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void UCITestOpenCaseRetrieveHeaderValues_SetLookup()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);

                LookupItem ownerId = new LookupItem { Name = "ownerid" };
                string ownerIdValue = xrmApp.Entity.GetHeaderValue(ownerId);

                client.Browser.Driver.ClearFocus();

                ownerId.Value = "Angel Rodriguez";
                xrmApp.Entity.SetHeaderValue(ownerId);

                ownerIdValue = xrmApp.Entity.GetHeaderValue(ownerId);
                Assert.AreEqual(ownerId.Value, ownerIdValue);

                xrmApp.ThinkTime(2000);
            }
        }


        [TestMethod]
        public void UCITestOpenCaseRetrieveHeaderValues_SetOptionSet()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);

                OptionSet priorityCode = new OptionSet { Name = "prioritycode" };
                string priorityCodeValue = xrmApp.Entity.GetHeaderValue(priorityCode);

                priorityCode.Value = "High";
                xrmApp.Entity.SetHeaderValue(priorityCode);

                priorityCodeValue = xrmApp.Entity.GetHeaderValue(priorityCode);
                Assert.AreEqual(priorityCode.Value, priorityCodeValue);

                xrmApp.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void UCITestOpenCase_SetOptionSet()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);

                OptionSet priorityCode = new OptionSet { Name = "prioritycode" };
                string priorityCodeValue = xrmApp.Entity.GetValue(priorityCode);

                priorityCode.Value = "High";
                xrmApp.Entity.SetValue(priorityCode);

                priorityCodeValue = xrmApp.Entity.GetValue(priorityCode);
                Assert.AreEqual(priorityCode.Value, priorityCodeValue);

                xrmApp.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void UCITestOpenCaseRetrieveHeaderValues_SetFields()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);

                LookupItem ownerId = new LookupItem { Name = "ownerid" };
                string ownerIdValue = xrmApp.Entity.GetHeaderValue(ownerId);

                client.Browser.Driver.ClearFocus();

                ownerId.Value = "Angel Rodriguez";
                xrmApp.Entity.SetHeaderValue(ownerId);

                ownerIdValue = xrmApp.Entity.GetHeaderValue(ownerId);
                Assert.AreEqual(ownerId.Value, ownerIdValue);

                OptionSet priorityCode = new OptionSet { Name = "prioritycode" };
                string priorityCodeValue = xrmApp.Entity.GetHeaderValue(priorityCode);

                priorityCode.Value = "High";
                xrmApp.Entity.SetHeaderValue(priorityCode);
                priorityCodeValue = xrmApp.Entity.GetHeaderValue(priorityCode);
                Assert.AreEqual(priorityCode.Value, priorityCodeValue);

                xrmApp.ThinkTime(2000);

            }

        }
    }
}