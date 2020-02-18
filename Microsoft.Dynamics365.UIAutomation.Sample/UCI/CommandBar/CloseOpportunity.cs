// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class CloseOpportunity : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.Sales, "Sales", "Opportunities");

        public override void SetOptions(BrowserOptions options)
        {
            options.PrivateMode = false;
            options.UCIPerformanceMode = true;
        }

        [TestMethod]
        public void UCITestCloseOpportunity()
        {
            _xrmApp.Grid.SwitchView("Open Opportunities");
            
            _xrmApp.Grid.OpenRecord(0);

            _xrmApp.CommandBar.ClickCommand("Close as Won");

            _xrmApp.Dialogs.CloseOpportunity(123.45, DateTime.Now, "test");
        }
    }
}
