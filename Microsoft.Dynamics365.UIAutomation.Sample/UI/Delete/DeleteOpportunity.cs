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
    public class DeleteOpportunity : TestsBase
    {

        [TestCategory("Dialogs")]
        [TestMethod]
        public void TestDeleteOpportunity()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.Grid.SwitchView("Open Opportunities");

                xrmApp.Grid.OpenRecord(0);

                // Click the Delete button from the command bar
                xrmApp.CommandBar.ClickCommand("Delete");

                // Need to be careful here as setting this value to true can be a destructive practice if we are not creating a record first.
                xrmApp.Dialogs.ConfirmationDialog(false); // Click OK on the Delete confirmation dialog (false to cancel)

                xrmApp.ThinkTime(3000);

            }

        }
    }
}
