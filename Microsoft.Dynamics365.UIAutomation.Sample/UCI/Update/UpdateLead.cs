// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class UpdateLeadUCI: TestsBase
    {
        [TestMethod]
        public void UCITestUpdateActiveLead()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(3000);

                xrmApp.Entity.SetValue("subject", TestSettings.GetRandomString(10,15));

                xrmApp.Entity.Save();

            }
            
        }

        [TestMethod]
        public void UCITestOpenActiveLeadBPF()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.OpenRecord(0);

                LookupItem acct = new LookupItem();
                acct.Name = "parentaccountid";
                acct.Value = "Adventure Works";

                LookupItem contact = new LookupItem();
                contact.Name = "parentcontactid";
                contact.Value = "Nancy Anderson (sample)";

                xrmApp.BusinessProcessFlow.SelectStage("Qualify");

                xrmApp.BusinessProcessFlow.SetValue(acct);
                xrmApp.BusinessProcessFlow.SetValue(contact);
                xrmApp.BusinessProcessFlow.SetValue("budgetamount", "100.00");

                xrmApp.ThinkTime(3000);
            }
        }
    }
}