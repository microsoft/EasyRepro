// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class CreateOpportunityUCI : TestsBase
    {
        [TestMethod]
        public void UCITestCreateOpportunity()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("name", TestSettings.GetRandomString(5,10));

                xrmApp.Entity.Save();
            }
        }

        [TestMethod]
        public void UCITestCreateOpportunity_SetHeaderDate()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("name", "Opporunity " + TestSettings.GetRandomString(5,10));

                DateTime expectedDate = DateTime.Today.AddDays(10);
             
                xrmApp.Entity.SetHeaderValue("estimatedclosedate", expectedDate);

                var commandResult = xrmApp.Entity.GetHeaderValue(new DateTimeControl("estimatedclosedate"));
                DateTime? date = commandResult;
                Assert.AreEqual(expectedDate, date);
            }
        }

        [TestMethod]
        public void UCITestCreateOpportunity_ClearHeaderDate()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SetValue("name", "Opporunity " + TestSettings.GetRandomString(5,10));

                DateTime expectedDate = DateTime.Today.AddDays(10);
             
                xrmApp.Entity.SetHeaderValue("estimatedclosedate", expectedDate);

                var control = new DateTimeControl("estimatedclosedate");
                DateTime? date = xrmApp.Entity.GetHeaderValue(control);
                Assert.AreEqual(expectedDate, date);


                xrmApp.Entity.ClearHeaderValue(control);
                date = xrmApp.Entity.GetHeaderValue(control);
                Assert.AreEqual(null, date);
            }
        }
    }
}