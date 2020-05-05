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
    public class UpdateCaseUCI
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly SecureString _mfaSecretKey = System.Configuration.ConfigurationManager.AppSettings["MfaSecretKey"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestUpdateActiveCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Entity.SetValue("description", TestSettings.GetRandomString(10,15));

                xrmApp.Entity.Save();

            }
            
        }

        [TestMethod]
        public void UCITestUpdateActiveCaseSetHeaderValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.RelatedGrid.OpenGridRow(0);

                OptionSet priorityCode = new OptionSet() { Name = "prioritycode", Value="Low" };
                xrmApp.Entity.SetHeaderValue(priorityCode);

                xrmApp.Entity.Save();

                xrmApp.ThinkTime(2000);

            }
        }
    }
}