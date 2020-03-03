// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class RandomAccountTest: TestsBase
    {
        [TestMethod]
        public void UCIRandomAccountTest()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {

                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue("name", ("Test Account Creation" + " " + TestSettings.GetRandomString(5,10)));

                var accountName = xrmApp.Entity.GetValue("name");

                xrmApp.CommandBar.ClickCommand("Save & Close");

                xrmApp.ThinkTime(2000);

                xrmApp.Grid.Search(accountName);

                xrmApp.ThinkTime(2000);

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(5000);

                xrmApp.Entity.Delete();

                xrmApp.ThinkTime(5000);

            }

        }
    }
}
