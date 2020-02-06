// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenCaseUCI: TestsBase
    {
        [TestMethod]
        public void UCITestOpenActiveCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);

                LookupItem ownerId = new LookupItem() { Name = "ownerid" };
                string ownerIdValue = xrmApp.Entity.GetHeaderValue(ownerId);

                OptionSet priorityCode = new OptionSet() { Name = "prioritycode" };
                string priorityCodeValue = xrmApp.Entity.GetHeaderValue(priorityCode);

                xrmApp.ThinkTime(2000);

            }
        }
    }
}