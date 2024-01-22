// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api;
using OpenQA.Selenium.DevTools.V109.Storage;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class BusinessProcessFlowAction : TestsBase
    {

        /// <summary>
        ///  Business Process Flow - Open case record and call TestBusinessProcessFlowAction
        /// </summary>
        [TestCategory("BusinessProcessFlow")]
        [TestMethod]
        public void TestBusinessProcessFlowSelectStage()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
               xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                
                xrmApp.ThinkTime(3000);

                xrmApp.Navigation.OpenApp(AppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("My Active Cases");

                xrmApp.Grid.OpenRecord(0);

            }
        }

    }
}
