// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenCaseUCI : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.CustomerService, "Service", "Cases");

        [TestMethod]
        public void UCITestOpenActiveCase()
        {
            _xrmApp.Grid.Search("*Service");

            _xrmApp.Grid.OpenRecord(0);
        }

        [TestMethod]
        public void UCITestOpenCase()
        {
            _xrmApp.Grid.SwitchView("Active Cases");
            _xrmApp.Grid.OpenRecord(0);

            Field ticketNumber = _xrmApp.Entity.GetField("ticketnumber");
            Assert.IsNotNull(ticketNumber);
            
            Field subject = _xrmApp.Entity.GetField("subjectid");
            Assert.IsNotNull(subject);

            Field description = _xrmApp.Entity.GetField("description");
            Assert.IsNotNull(description);

            Field mobilePhone = _xrmApp.Entity.GetField("mobilephone");
            Assert.IsNotNull(mobilePhone);
        }

        [TestMethod]
        public void UCITestOpenCaseById()
        {
            _xrmApp.Grid.SwitchView("Active Cases");
            _xrmApp.Grid.OpenRecord(0);
            string number = _xrmApp.Entity.GetValue("ticketnumber");
            Assert.IsNotNull(number);

            // For proper test usage, please update the recordId below to a valid Case recordId
            Guid recordId = _xrmApp.Entity.GetObjectId();

            _xrmApp.Navigation.OpenSubArea("Service", "Cases");

            string firstCaseNumber = number;
            _xrmApp.Entity.OpenEntity("incident", recordId);
            number = _xrmApp.Entity.GetValue("ticketnumber");
            Assert.IsNotNull(number);
            Assert.AreEqual(firstCaseNumber, number);
        }

        [TestMethod]
        public void UCITestOpenCaseRetrieveHeaderValues()
        {
            _xrmApp.Grid.SwitchView("Active Cases");

            _xrmApp.Grid.OpenRecord(0);

            LookupItem ownerId = new LookupItem {Name = "ownerid"};
            string ownerIdValue = _xrmApp.Entity.GetHeaderValue(ownerId);

            OptionSet priorityCode = new OptionSet {Name = "prioritycode"};
            string priorityCodeValue = _xrmApp.Entity.GetHeaderValue(priorityCode);

            _xrmApp.ThinkTime(2000);
        }

        [TestMethod]
        public void UCITestOpenCaseRetrieveHeaderValues_SetLookup()
        {
            _xrmApp.Grid.SwitchView("Active Cases");

            _xrmApp.Grid.OpenRecord(0);

            LookupItem ownerId = new LookupItem {Name = "ownerid"};
            string ownerIdValue = _xrmApp.Entity.GetHeaderValue(ownerId);

            _client.Browser.Driver.ClearFocus();

            ownerId.Value = "Angel Rodriguez";
            _xrmApp.Entity.SetHeaderValue(ownerId);

            ownerIdValue = _xrmApp.Entity.GetHeaderValue(ownerId);
            Assert.AreEqual(ownerId.Value, ownerIdValue);

            _xrmApp.ThinkTime(2000);
        }


        [TestMethod]
        public void UCITestOpenCaseRetrieveHeaderValues_SetOptionSet()
        {
            _xrmApp.Grid.SwitchView("Active Cases");

            _xrmApp.Grid.OpenRecord(0);

            OptionSet priorityCode = new OptionSet
            {
                Name = "prioritycode",
                Value = "High"
            };
            
            _xrmApp.Entity.SetHeaderValue(priorityCode);

           string priorityCodeValue = _xrmApp.Entity.GetHeaderValue(priorityCode);
            Assert.AreEqual(priorityCode.Value, priorityCodeValue);

            _xrmApp.ThinkTime(2000);
        }

        [TestMethod]
        public void UCITestOpenCase_SetOptionSet()
        {
            _xrmApp.Grid.SwitchView("Active Cases");

            _xrmApp.Grid.OpenRecord(0);

            OptionSet priorityCode = new OptionSet
            {
                Name = "prioritycode",
                Value = "High"
            };
            _xrmApp.Entity.SetValue(priorityCode);

            string priorityCodeValue = _xrmApp.Entity.GetValue(priorityCode);
            Assert.AreEqual(priorityCode.Value, priorityCodeValue);

            _xrmApp.ThinkTime(2000);
        }

        [TestMethod]
        public void UCITestOpenCase_SetHeaderValues()
        {
            _xrmApp.Grid.SwitchView("Active Cases");

            _xrmApp.Grid.OpenRecord(0);

            LookupItem ownerId = new LookupItem {Name = "ownerid"};
            ownerId.Value = _xrmApp.Entity.GetHeaderValue(ownerId);
            
            _client.Browser.Driver.ClearFocus();
            
            _xrmApp.Entity.SetHeaderValue(ownerId);

            var ownerIdValue = _xrmApp.Entity.GetHeaderValue(ownerId);
            Assert.AreEqual(ownerId.Value, ownerIdValue);

            OptionSet priorityCode = new OptionSet
            {
                Name = "prioritycode",
                Value = "High"
            };
            _xrmApp.Entity.SetHeaderValue(priorityCode);
            string priorityCodeValue = _xrmApp.Entity.GetHeaderValue(priorityCode);
            Assert.AreEqual(priorityCode.Value, priorityCodeValue);

            _xrmApp.ThinkTime(2000);
        }
    }
}