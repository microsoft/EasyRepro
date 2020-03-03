// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class UpdateContactUCI: TestsBase
    {
        [TestMethod]
        public void UCITestUpdateActiveContact()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(3000);

                xrmApp.Entity.SetValue("firstname", TestSettings.GetRandomString(5,10));
                xrmApp.Entity.SetValue("lastname", TestSettings.GetRandomString(5,10));

                xrmApp.Entity.Save();

            }
            
        }

        [TestMethod]
        public void UCITestUpdateActiveContactSetHeaderValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Contacts");

                xrmApp.RelatedGrid.OpenGridRow(0);

                //xrmApp.Entity.SetHeaderValue("emailaddress1", String.Format("{0}@{1}.com", TestSettings.GetRandomString(8, 8), TestSettings.GetRandomString(5, 7)));
                xrmApp.Entity.SetHeaderValue("telephone1", "555-555-5555");

                xrmApp.Entity.Save();

                xrmApp.ThinkTime(2000);

            }
        }

        [TestMethod]
        public void UCITestUpdateClearFields()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Contacts");

                xrmApp.RelatedGrid.OpenGridRow(0);

                xrmApp.Entity.ClearValue("telephone1");

                LookupItem account = new LookupItem() { Name = "parentcustomerid" };
                xrmApp.Entity.ClearValue(account);

                OptionSet preferredContact = new OptionSet() { Name = "preferredcontactmethodcode" };
                xrmApp.Entity.ClearValue(preferredContact);
                
                xrmApp.Entity.SetValue("telephone1", "555-555-5555");

                preferredContact.Value = "Email";
                xrmApp.Entity.SetValue(preferredContact);

                xrmApp.Entity.Save();

                

                xrmApp.ThinkTime(2000);

            }
        }
    }
}