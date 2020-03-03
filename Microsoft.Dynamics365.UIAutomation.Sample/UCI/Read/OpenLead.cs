// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenLeadUCI: TestsBase
    {
        [TestMethod]
        public void UCITestOpenActiveLead()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");
                
                xrmApp.Grid.Search("David");

                xrmApp.Grid.OpenRecord(0);
                
            }
            
        }

        [TestMethod]
        public void UCITestRetrieveBPFFields()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.OpenRecord(0);

                LookupItem contact = new LookupItem() { Name = "parentcontactid" };
                LookupItem account = new LookupItem() { Name = "parentaccountid" };

                xrmApp.BusinessProcessFlow.SelectStage("Qualify");

                string contactName = xrmApp.BusinessProcessFlow.GetValue(contact);
                string acctName = xrmApp.BusinessProcessFlow.GetValue(account);
                string budgetAmt = xrmApp.BusinessProcessFlow.GetValue("budgetamount");

                string status = xrmApp.Entity.GetFooterStatusValue();

                xrmApp.ThinkTime(1000);
            }
        }

        [TestMethod]
        public void UCITestPinBPFStage()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.BusinessProcessFlow.SelectStage("Qualify");

                xrmApp.BusinessProcessFlow.Pin("Qualify");

                xrmApp.ThinkTime(500);
            }
        }

        [TestMethod]
        public void UCITestCloseBPFStage()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.BusinessProcessFlow.SelectStage("Qualify");

                xrmApp.BusinessProcessFlow.Close("Qualify");

                xrmApp.ThinkTime(500);
            }
        }
    }
}