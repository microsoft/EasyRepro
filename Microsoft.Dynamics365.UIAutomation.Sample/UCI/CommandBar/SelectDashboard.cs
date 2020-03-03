// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class SelectDashboard : TestsBase
    {
        [TestMethod]
        public void UCITestSelectDashboard()
        {
            using (var xrmApp = CreateApp())
            {
                NavigateTo(UCIAppName.Sales);

                xrmApp.Navigation.ClickQuickLaunchButton("Dashboards");

                xrmApp.Dashboard.SelectDashboard("My Knowledge Dashboard");
            }
        }

        [TestMethod]
        public void UCITestSelectDashboard_SalesDashboard()
        {
            using (var xrmApp = CreateApp())
            {
                NavigateTo(UCIAppName.Sales, "Sales", "Dashboards");
                
                xrmApp.Dashboard.SelectDashboard("Sales Dashboard");
                
                xrmApp.ThinkTime(5000);
            }
        }
    }
}
