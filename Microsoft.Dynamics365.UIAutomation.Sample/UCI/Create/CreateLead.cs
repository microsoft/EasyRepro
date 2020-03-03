// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class CreateLeadUCI : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.Sales, "Sales", "Leads");

        [TestMethod]
        public void UCITestCreateLead()
        {
            _xrmApp.CommandBar.ClickCommand("New");

            _xrmApp.ThinkTime(5000);

            _xrmApp.Entity.SetValue("subject", TestSettings.GetRandomString(5, 15));
            _xrmApp.Entity.SetValue("firstname", TestSettings.GetRandomString(5, 10));
            _xrmApp.Entity.SetValue("lastname", TestSettings.GetRandomString(5, 10));
        }

        [TestMethod]
        public void UCITestCreateLead_SetHeaderStatus()
        {
            _xrmApp.CommandBar.ClickCommand("New");

            _xrmApp.ThinkTime(5000);

            _xrmApp.Entity.SetValue("subject", TestSettings.GetRandomString(5, 15));
            _xrmApp.Entity.SetValue("firstname", TestSettings.GetRandomString(5, 10));
            _xrmApp.Entity.SetValue("lastname", TestSettings.GetRandomString(5, 10));

            var status = new OptionSet { Name = "statuscode", Value = "Contacted" };
            _xrmApp.Entity.SetHeaderValue(status);

            string value = _xrmApp.Entity.GetHeaderValue(status);
            Assert.AreEqual(status.Value,  value);
        }
    }
}

