// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class SelectDashboard : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        [TestMethod]
        public void UCITestSelectDashboard()
        {
            _xrmApp.Navigation.ClickQuickLaunchButton("Dashboards");

            _xrmApp.Dashboard.SelectDashboard("My Knowledge Dashboard");
        }
    }
}
