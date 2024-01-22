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
    public class CloseOpportunity : TestsBase
    {

        [TestCategory("Dialogs")]
        [TestCategory("CommandBar")]
        [TestMethod]
        public void TestCloseOpportunity()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmApp.Grid.SwitchView("Open Opportunities");

                // Need to be careful here as this is destructive practice if we are not creating a record first.
                xrmApp.Grid.OpenRecord(0);

                xrmApp.CommandBar.ClickCommand("Close as Won");

                xrmApp.Dialogs.CloseOpportunity(123.45, DateTime.Now, "test");

            }
        }
    }
}
