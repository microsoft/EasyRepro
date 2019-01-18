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
    public class CreateCaseUCI
    {

       
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestCreateCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.CommandBar.ClickCommand("New Case");

                xrmApp.ThinkTime(5000);

                xrmApp.Entity.SetValue("title", TestSettings.GetRandomString(5,10));

                LookupItem customer = new LookupItem { Name = "customerid", Value = "Test Lead" };
                xrmApp.Entity.SetValue(customer);

                xrmApp.Entity.Save();
            }
            
        }
    }
}