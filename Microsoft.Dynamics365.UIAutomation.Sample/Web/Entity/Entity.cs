// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Entity : TestBase
    {
        [TestInitialize]
        public override void TestSetup()
        {
            XrmTestBrowser.ThinkTime(500);
            OpenEntity("Sales", "Contacts", "Active Contacts");
        }

        [TestMethod]
        public void WEBTestNavigateUp()
        {
            XrmTestBrowser.Grid.OpenRecord(1);
            XrmTestBrowser.Entity.NavigateUp();
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestNavigateDown()
        {
            XrmTestBrowser.Grid.OpenRecord(0);
            XrmTestBrowser.Entity.NavigateDown();
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestGetRecordGuid()
        {
            XrmTestBrowser.Grid.OpenRecord(0);
            var recordGuid = XrmTestBrowser.Entity.GetRecordGuid().Value;
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestClearRecordValues()
        {
            XrmTestBrowser.Grid.OpenRecord(0);

            //XrmTestBrowser.Entity.SetValue("") //Text Field
            //XrmTestBrowser.Entity.ClearValue(""); //Text field

            XrmTestBrowser.ThinkTime(5000);
        }
    }
}