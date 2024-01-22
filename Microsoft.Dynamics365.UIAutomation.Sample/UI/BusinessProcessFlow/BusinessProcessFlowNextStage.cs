// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class BusinessProcessFlowNextStageUCI : TestsBase
    {

        [TestCategory("BusinessProcessFlow")]
        [TestMethod]
        public void TestBusinessProcessFlowNextStage()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Leads");

                xrmApp.Grid.SwitchView("Open Leads");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.BusinessProcessFlow.NextStage("Qualify");
            }
        }
    }
}
