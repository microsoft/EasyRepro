// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Globalization;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class CreateActivityUCI
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestCreateActivity_SetDateTimes()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("My Work", "Activities");

                xrmApp.CommandBar.ClickCommand("Appointment");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("subject", "Appointment "+ TestSettings.GetRandomString(5,10));
                
                DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                DateTime expectedEndDate = expectedDate.AddHours(2);
             
                xrmApp.Entity.SetValue("scheduledstart", expectedDate);
                xrmApp.Entity.SetValue("scheduledend",  expectedEndDate);

                DateTime date = xrmApp.Entity.GetValue(new DateTimeControl("scheduledstart"));
                Assert.AreEqual(expectedDate, date);
                
                date = xrmApp.Entity.GetValue(new DateTimeControl("scheduledend"));
                Assert.AreEqual(expectedEndDate, date);
            }
        }
    }
}