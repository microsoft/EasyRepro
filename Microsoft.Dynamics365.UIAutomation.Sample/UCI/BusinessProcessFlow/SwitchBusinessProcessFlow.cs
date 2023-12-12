// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class SwitchBusinessProcessFlowUCI : TestsBase
    {
        [TestCategory("BusinessProcessFlow")]
        [TestMethod]
        public void UCITestSwitchBusinessProcessFlow()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.OpenRecord(0);

                //xrmApp.Entity.SwitchProcess("AccountEventingProcess");
                xrmApp.Entity.SwitchProcess("Lead to Opportunity Sales Process");
            }
        }
    }
}
