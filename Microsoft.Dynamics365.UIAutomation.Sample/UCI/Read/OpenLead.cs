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
    public class OpenLeadUCI : TestsBase
    {
        [TestCategory("Grid")]
        [TestCategory("GridSearch")]
        [TestMethod]
        public void UCITestOpenActiveLead()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");
                
                xrmApp.Grid.SwitchView("Open Leads");

                xrmApp.Grid.Search("Debra");

                xrmApp.Grid.OpenRecord(0);
                
            }
            
        }

        [TestCategory("Grid")]
        [TestCategory("BusinessProcessFlow")]
        [TestMethod]
        [TestCategory("Fail - Bug")]
        public void UCITestRetrieveBPFFields()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.SwitchView("Open Leads");

                xrmApp.Grid.OpenRecord(0);

                LookupItem contact = new LookupItem() { Name = "parentcontactid" };
                LookupItem account = new LookupItem() { Name = "parentaccountid" };

                xrmApp.BusinessProcessFlow.SelectStage("Qualify");

                // Bug: Lookups fail to resolve:  System.NullReferenceException: Object reference not set to an instance of an object.
                string contactName = xrmApp.BusinessProcessFlow.GetValue(contact);
                string acctName = xrmApp.BusinessProcessFlow.GetValue(account);
                string budgetAmt = xrmApp.BusinessProcessFlow.GetValue("budgetamount");

                string status = xrmApp.Entity.GetFormState();

                xrmApp.ThinkTime(1000);
            }
        }

        [TestCategory("Grid")]
        [TestCategory("BusinessProcessFlowPin")]
        [TestMethod]
        public void UCITestPinBPFStage()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.SwitchView("Open Leads");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.BusinessProcessFlow.SelectStage("Qualify");

                xrmApp.BusinessProcessFlow.Pin("Qualify");

                xrmApp.ThinkTime(500);
            }
        }

        [TestCategory("BusinessProcessFlowClose")]
        [TestMethod]
        public void UCITestCloseBPFStage()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.SwitchView("Open Leads");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.BusinessProcessFlow.SelectStage("Qualify");

                xrmApp.BusinessProcessFlow.Close("Qualify");

                xrmApp.ThinkTime(500);
            }
        }
    }
}