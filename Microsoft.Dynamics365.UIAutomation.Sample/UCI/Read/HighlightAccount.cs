// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class HighlightAccount: TestsBase
    {
        [TestMethod]
        public void UCITestHighlightActiveAccount()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.Search("Adventure");

                xrmApp.Grid.HighLightRecord(0); //Ticks the box, allowing you to Edit / Delete (Command) if you so desire

                xrmApp.CommandBar.ClickCommand("Edit");

                xrmApp.ThinkTime(3000);
            }
        }
    }
}