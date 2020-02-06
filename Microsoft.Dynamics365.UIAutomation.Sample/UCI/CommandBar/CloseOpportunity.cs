// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class CloseOpportunity : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => _xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

        [TestMethod]
        public void UCITestCloseOpportunity()
        {
            _xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

            _xrmApp.Grid.OpenRecord(0);

            _xrmApp.CommandBar.ClickCommand("Close as Won");

            _xrmApp.Dialogs.CloseOpportunity(123.45, DateTime.Now, "test");
        }
    }
}
