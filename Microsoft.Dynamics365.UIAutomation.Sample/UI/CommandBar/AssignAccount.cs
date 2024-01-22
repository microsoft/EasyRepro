// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;


namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class AssignAccount : TestsBase
    {

        [TestCategory("Fail - DynamicData")]
        [TestCategory("Dialogs")]
        [TestCategory("CommandBar")]
        [TestMethod]
        public void TestAssignAccount()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(2000);

                xrmApp.CommandBar.ClickCommand("Assign");

                // Fail - DynamicData: Need to set an actual user here
                xrmApp.Dialogs.Assign(Dialogs.AssignTo.User, "Grant");

            }
        }
    }
}
