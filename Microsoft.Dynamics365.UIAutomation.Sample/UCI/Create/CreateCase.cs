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
        private readonly SecureString _mfaSecretKey = System.Configuration.ConfigurationManager.AppSettings["MfaSecretKey"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestCreateCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.CommandBar.ClickCommand("New Case");

                xrmApp.ThinkTime(4000);

                xrmApp.Entity.SetValue("title", TestSettings.GetRandomString(5,10));

                // To work around the bug below, clearvalue first to initalize the field and then set it
                xrmApp.Entity.ClearValue(new LookupItem { Name = "customerid"});
                
                // Bug - System.InvalidOperationException: No Results Matching maria campbell Were Found. #769
                xrmApp.Entity.SetValue(new LookupItem { Name = "customerid", Value = "maria campbell", Index = 0 });
                
                xrmApp.ThinkTime(5000);

                xrmApp.Entity.Save();
            }
            
        }
    }
}