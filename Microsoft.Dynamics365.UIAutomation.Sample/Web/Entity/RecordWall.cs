// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Dynamics365.UIAutomation.Api;

    [TestClass]
    public class RecordWall : TestBase
    {
        [TestInitialize]
        public override void TestSetup()
        {
            XrmTestBrowser.ThinkTime(500);
            OpenEntity("Sales", "Accounts", "Active Accounts");
            HasData = OpenEntityGrid();
        }

        [TestMethod]
        public void WEBTestSelectTab()
        {
            if (!HasData) return;
            XrmTestBrowser.ActivityFeed.SelectTab(Api.Pages.ActivityFeed.Tab.Activities);
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestAddPost()
        {
            if (!HasData) return;
            XrmTestBrowser.ActivityFeed.SelectTab(Api.Pages.ActivityFeed.Tab.Posts);
            XrmTestBrowser.ActivityFeed.AddPost("Test Add Post");
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestAddActivity()
        {
            if (!HasData) return;
            XrmTestBrowser.ActivityFeed.SelectTab(Api.Pages.ActivityFeed.Tab.Activities);
            XrmTestBrowser.ActivityFeed.AddPhoneCall("Test Phone call Description", false);
            XrmTestBrowser.ThinkTime(5000);
            
        }

        [TestMethod]
        public void WEBTestAddNote()
        {
            if (!HasData) return;
            XrmTestBrowser.ActivityFeed.SelectTab(Api.Pages.ActivityFeed.Tab.Notes);
            XrmTestBrowser.ActivityFeed.AddNote("Test Add Note");
            XrmTestBrowser.ThinkTime(5000);
        }

        [TestMethod]
        public void WEBTestAddAppointment()
        {
            if (!HasData) return;

            XrmTestBrowser.ActivityFeed.SelectTab(Api.Pages.ActivityFeed.Tab.Activities);
            XrmTestBrowser.ActivityFeed.AddAppointment();
            XrmTestBrowser.Entity.SetValue("subject", "Add Appointment");
            XrmTestBrowser.CommandBar.ClickCommand("Save");
            XrmTestBrowser.ThinkTime(2000);
        }

        [TestMethod]
        public void WEBTestAddEmail()
        {
            if (!HasData) return;

            XrmTestBrowser.ActivityFeed.SelectTab(Api.Pages.ActivityFeed.Tab.Activities);
            XrmTestBrowser.ActivityFeed.AddEmail();
            XrmTestBrowser.Entity.SetValue("subject", "Test Mail");
            XrmTestBrowser.ThinkTime(2000);
         
        }

        [TestMethod]
        public void WEBTestAddTask()
        {
            if (!HasData) return;
            XrmTestBrowser.ActivityFeed.SelectTab(Api.Pages.ActivityFeed.Tab.Activities);
            XrmTestBrowser.ActivityFeed.AddTask("Schedule an appointment", "Capture preliminary customer and product information.", DateTime.Now, new OptionSet { Name = "quickCreateActivity4212controlId_prioritycode_d", Value = "Normal" });
            XrmTestBrowser.ThinkTime(4000); 
        }

        [TestMethod]
        public void WEBTestFilterActivitiesByStatus()
        {
            if (!HasData) return;
            XrmTestBrowser.ActivityFeed.SelectTab(Api.Pages.ActivityFeed.Tab.Activities);
            XrmTestBrowser.ActivityFeed.FilterActivitiesByStatus(Api.Pages.ActivityFeed.Status.Overdue);
            XrmTestBrowser.ThinkTime(3000);
        }
    }
}