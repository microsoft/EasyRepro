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
    public class OfferManagementTesting
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

                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, client.MSFTLoginAction);

                xrmApp.Navigation.OpenApp("Offer Management");

                xrmApp.Navigation.OpenSubArea("New Area", "Spa Offers");
                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue("name", "Test Offer 123");

                xrmApp.ThinkTime(3000);

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
