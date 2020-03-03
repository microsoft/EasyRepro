// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenAccountUCI : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.Sales, "Sales", "Accounts");

        [TestMethod]
        public void UCITestOpenActiveAccount()
        {
            _xrmApp.Grid.Search("Adventure");

            _xrmApp.Grid.OpenRecord(0);
            
            _xrmApp.ThinkTime(1000);

            string value = _xrmApp.Entity.GetValue("name");
            Assert.IsTrue(value.StartsWith("Adventure"));
        }

        [TestMethod]
        public void UCITestGetActiveGridItems()
        {
            _xrmApp.Grid.GetGridItems();

            _xrmApp.Grid.Sort("Account Name");

            _xrmApp.ThinkTime(3000);
        }

        [TestMethod]
        public void UCITestOpenTabDetails()
        {
            _xrmApp.Grid.SwitchView("All Accounts");

            _xrmApp.Grid.OpenRecord(0);

            _xrmApp.ThinkTime(3000);

            _xrmApp.Entity.SelectTab("Details");

            _xrmApp.Entity.SelectTab("Related", "Contacts");
            
            _xrmApp.ThinkTime(3000);
        }

        [TestMethod]
        public void UCITestGetObjectId()
        {
            _xrmApp.Grid.OpenRecord(0);

            Guid objectId = _xrmApp.Entity.GetObjectId();
            Assert.AreNotEqual(Guid.Empty, objectId);
            _xrmApp.ThinkTime(3000);
        }

        [TestMethod]
        public void UCITestOpenSubGridRecord()
        {
            _xrmApp.Grid.OpenRecord(0);

            _xrmApp.Entity.GetSubGridItems("CONTACTS");

            _xrmApp.ThinkTime(3000);
        }
    }
}