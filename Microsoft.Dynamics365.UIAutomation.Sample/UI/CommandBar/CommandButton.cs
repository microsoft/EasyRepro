// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class CommandButton : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => NavigateTo(AppName.Sales, "Sales", "Accounts");

        [TestCategory("CommandBar")]
        [TestMethod]
        public void TestNewCommandBarButton()
        {
            _xrmApp.CommandBar.ClickCommand("New");
            _xrmApp.ThinkTime(2000);
        }

        [TestCategory("CommandBar")]
        [TestMethod]
        public void TestRetrieveCommandBarValues()
        {
            var commandValues = _xrmApp.CommandBar.GetCommandValues().Value;
            int commandCount = commandValues.Count;

            var includeMoreCommandValues = _xrmApp.CommandBar.GetCommandValues(true).Value;
            int totalCommandCount = includeMoreCommandValues.Count;

            Assert.IsTrue(commandCount <= totalCommandCount);
            _xrmApp.ThinkTime(2000);
        }
    }
}