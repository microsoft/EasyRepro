// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class CreateActivityUCI : TestsBase
    {
        [TestMethod]
        public void UCITestCreateActivity_SetDateTimes()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.CommandBar.ClickCommand("Appointment");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("subject", "Appointment "+ TestSettings.GetRandomString(5,10));
                
                DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                DateTime expectedEndDate = expectedDate.AddHours(2);
             
                xrmApp.Entity.SetValue("scheduledstart", expectedDate);
                xrmApp.Entity.SetValue("scheduledend",  expectedEndDate);

                DateTime? date = xrmApp.Entity.GetValue(new DateTimeControl("scheduledstart"));
                Assert.AreEqual(expectedDate, date);
                
                date = xrmApp.Entity.GetValue(new DateTimeControl("scheduledend"));
                Assert.AreEqual(expectedEndDate, date);
            }
        }
        
        [TestMethod]
        public void UCITestCreateActivity_ClearDateTimes()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.CommandBar.ClickCommand("Appointment");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("subject", "Appointment "+ TestSettings.GetRandomString(5,10));
                
                DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                DateTime expectedEndDate = expectedDate.AddHours(2);
             
                //xrmApp.Entity.SetValue("scheduledstart",  expectedDate);
                var start = new DateTimeControl("scheduledstart") { Value =  expectedDate};
                xrmApp.Entity.SetValue(start);

                DateTime? date = xrmApp.Entity.GetValue(start);
                Assert.AreEqual(expectedDate, date);
                
                //xrmApp.Entity.SetValue("scheduledend",  expectedEndDate);
                var end = new DateTimeControl("scheduledend") {Value =  expectedEndDate};
                xrmApp.Entity.SetValue(end);

                date = xrmApp.Entity.GetValue(end);
                Assert.AreEqual(expectedEndDate, date);

                xrmApp.Entity.ClearValue(start);
                date = xrmApp.Entity.GetValue(start);
                Assert.AreEqual(null, date);

                xrmApp.Entity.ClearValue(end);
                date = xrmApp.Entity.GetValue(end);
                Assert.AreEqual(null, date);
            }
        }

        
        [TestMethod]
        public void UCITestCreateActivity_ClearHeaderDateTimes()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                xrmApp.CommandBar.ClickCommand("Appointment");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("subject", "Appointment "+ TestSettings.GetRandomString(5,10));
                
                DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                DateTime expectedEndDate = expectedDate.AddHours(2);

                var start = new DateTimeControl("scheduledstart") { Value =  expectedDate};
                xrmApp.Entity.SetValue(start);
             
                var end = new DateTimeControl("scheduledend") { Value =  expectedEndDate};
                xrmApp.Entity.SetHeaderValue(end);

                DateTime? date = xrmApp.Entity.GetHeaderValue(end);
                Assert.AreEqual(expectedEndDate, date);
                
                xrmApp.Entity.ClearValue(start);

                xrmApp.Entity.ClearHeaderValue(end);
                date = xrmApp.Entity.GetHeaderValue(end);
                Assert.AreEqual(null, date);
            }
        }
    }
}