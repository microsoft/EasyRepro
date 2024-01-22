// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class IsFieldRequired: TestsBase
    {
        [TestCategory("Entity")]
        [TestMethod]
        public void TestConfirmFieldIsNotRequired()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.Search("Adventure");

                xrmApp.Grid.OpenRecord(0);

                Field Name = xrmApp.Entity.GetField("industrycode");

                Assert.IsFalse(Name.IsRequired);

                xrmApp.ThinkTime(3000);

            }
        }

        [TestMethod]
        public void TestConfirmFieldIsRequired()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.Search("Adventure");

                xrmApp.Grid.OpenRecord(0);

                Field Name = xrmApp.Entity.GetField("name");

                Assert.IsTrue(Name.IsRequired);

                xrmApp.ThinkTime(3000);

            }
        }
    }
}