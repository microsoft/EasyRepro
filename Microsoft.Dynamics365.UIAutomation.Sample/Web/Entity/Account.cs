// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class TestAccount : TestBase
    {
        [TestInitialize]
        public override void TestSetup()
        {
            XrmTestBrowser.ThinkTime(500);
            OpenEntity("Sales", "Accounts", "Active Accounts");
            HasData = OpenEntityGrid();
        }

        [TestMethod]
        public void WEBTestCollapseTab()
        {
            if (!HasData) return;

            var tabState = XrmTestBrowser.Entity.GetTabState("Summary");

            if (tabState.Value != "Collapsed")
                XrmTestBrowser.Entity.CollapseTab("Summary");


            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestPopOutForm()
        {
            if (!HasData) return;
            XrmTestBrowser.Entity.Popout();
            XrmTestBrowser.ThinkTime(10000);
        }

        [TestMethod]
        public void WEBTestOpenLookup()
        {
            if (!HasData) return;
            XrmTestBrowser.Entity.SelectLookup(TestSettings.LookupValues, true, false);
            //XrmTestBrowser.Entity.SelectLookup(TestSettings.LookupField, 0);
        }

        [TestMethod]
        public void WEBTestSearchLookup()
        {
            if (!HasData) return;
            XrmTestBrowser.Entity.SetValue(TestSettings.LookupValues);

            XrmTestBrowser.ThinkTime(5000);

            XrmTestBrowser.Entity.SelectLookup(TestSettings.LookupValues, true, false);

            XrmTestBrowser.ThinkTime(5000);
            //XrmTestBrowser.Entity.SelectLookup(TestSettings.LookupField, 0);
        }

        [TestMethod]
        public void WEBTestSaveEntity()
        {
            if (!HasData) return;
            XrmTestBrowser.Entity.Save();
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestCloseEntity()
        {
            if (!HasData) return;
            XrmTestBrowser.Entity.CloseEntity();
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestExpandTab()
        {
            if (!HasData) return;

            var tabState = XrmTestBrowser.Entity.GetTabState("Scheduling");

            if (tabState != "Expanded")
                XrmTestBrowser.Entity.ExpandTab("Scheduling");

            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestSelectTab()
        {
            if (!HasData) return;
            XrmTestBrowser.Entity.SelectTab("Summary");
            XrmTestBrowser.Entity.SelectTab("Details");
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestSelectSectionInForm()
        {
            if (!HasData) return;
            XrmTestBrowser.Entity.SelectForm("Details");
            XrmTestBrowser.ThinkTime(5000);
        }
    }
}