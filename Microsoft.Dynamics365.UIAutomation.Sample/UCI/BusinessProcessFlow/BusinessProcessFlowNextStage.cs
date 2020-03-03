// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class BusinessProcessFlowNextStage : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.Sales, "Sales", "Leads");

        [TestMethod]
        public void UCITestBusinessProcessFlowNextStage()
        {
            _xrmApp.Grid.OpenRecord(0);
            _xrmApp.BusinessProcessFlow.NextStage("Qualify");
        }
    }
}
