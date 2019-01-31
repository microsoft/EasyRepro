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
    public class QuickCreateContactUCI
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestQuickCreateContact()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.QuickCreate("contact");

                xrmApp.QuickCreate.SetValue("firstname", TestSettings.GetRandomString(5, 10));

                xrmApp.QuickCreate.SetValue("lastname", TestSettings.GetRandomString(5, 10));

                xrmApp.QuickCreate.SetValue(new LookupItem() { Name = "parentcustomerid", Value = "Test" });

                xrmApp.QuickCreate.Save();

            }
        }

        [TestMethod]
        public void UCITestQuickCreateCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.QuickCreate("case");

                xrmApp.QuickCreate.SetValue(new LookupItem { Name = "customerid", Value = "" });

                xrmApp.QuickCreate.SetValue("title", TestSettings.GetRandomString(5, 10));

                xrmApp.QuickCreate.SetValue(new OptionSet() { Name = "casetypecode", Value = "Problem" });

                xrmApp.QuickCreate.Save();

            }
        }

        [TestMethod]
        public void UCITestQuickCreateOpportunity()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.QuickCreate("opportunity");

                xrmApp.QuickCreate.Cancel();

                xrmApp.Navigation.QuickCreate("opportunity");

                xrmApp.QuickCreate.SetValue(new LookupItem { Name = "parentcontactid", Value = "Test" });

                xrmApp.QuickCreate.SetValue("name", TestSettings.GetRandomString(5, 10));

                xrmApp.QuickCreate.SetValue(new OptionSet() { Name = "msdyn_ordertype", Value = "Work based" });

                xrmApp.QuickCreate.SetValue("estimatedclosedate", DateTime.Now.AddDays(45), "MM/dd/yyyy");

                string name = xrmApp.QuickCreate.GetValue("name");

                string orderType = xrmApp.QuickCreate.GetValue(new OptionSet() { Name = "msdyn_ordertype" });

                string estimatedCloseDate = xrmApp.QuickCreate.GetValue("estimatedclosedate");

                string parentContact = xrmApp.QuickCreate.GetValue(new LookupItem() { Name = "parentcontactid" });

                xrmApp.QuickCreate.Save();

            }
        }
    }
}