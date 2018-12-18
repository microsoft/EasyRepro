// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OfferManagementTest
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestCreateNewOffer()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {

                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp("Offer Management");

                xrmApp.Navigation.OpenSubArea("New Area", "Spa Offers");
                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue("name", "Test Offer 1234");

                xrmApp.ThinkTime(3000);

                xrmApp.Entity.Save();

                xrmApp.ThinkTime(3000);

                xrmApp.BusinessProcessFlow.SelectStage("Details");

                xrmApp.BusinessProcessFlow.SetValue(new OptionSet { Name = "va_minimumtieravailability", Value = "Flying Club Gold" }); // Minimum Tier            

                xrmApp.BusinessProcessFlow.NextStage("Details");

                xrmApp.BusinessProcessFlow.SelectStage("Gold Member Costs");

                xrmApp.BusinessProcessFlow.SetValue("va_goldmemberprice", "500.00"); // Gold Member Price

                xrmApp.BusinessProcessFlow.SetValue("va_silvermemberprice", "200.00"); // Silver Member Price

                xrmApp.BusinessProcessFlow.NextStage("Gold Member Costs");

                xrmApp.BusinessProcessFlow.SelectStage("Sent To Approval");

                xrmApp.BusinessProcessFlow.SetValue(new OptionSet { Name = "va_sendforapproval", Value = "Send request" }); // Approval REquest

                 xrmApp.BusinessProcessFlow.NextStage("Sent To Approval");

                xrmApp.BusinessProcessFlow.SelectStage("Publish Offer");

                xrmApp.BusinessProcessFlow.SetValue("va_availableon", DateTime.Parse("01/12/2019")); ; // Available On

                xrmApp.ThinkTime(2000);

                xrmApp.BusinessProcessFlow.SetValue(new BooleanItem {Name = "va_isfeatured"}); // Featured On (Checkbox)

                xrmApp.ThinkTime(2000);

                xrmApp.Entity.Save();

                xrmApp.ThinkTime(2000);

            }

        }


        [TestMethod]
        public void UCITestOpenOffer()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp("Offer Management");

                xrmApp.Navigation.OpenSubArea("New Area", "Spa Offers");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(2000);
            }
        }
    }
}
