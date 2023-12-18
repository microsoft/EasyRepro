// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenCaseUCI : TestsBase
    {
        [TestCategory("Grid")]
        [TestMethod]
        public void UCITestOpenActiveCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.Search("Service Requested");

                xrmApp.Grid.OpenRecord(0);
            }
        }

        [TestCategory("Grid")]
        [TestMethod]
        public void UCITestOpenCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);
                xrmApp.Entity.GetValue("");
            }
        }

        [TestCategory("Grid")]
        [TestMethod]
        [TestCategory("Fail - Bug")]
        public void UCITestOpenCaseRetrieveHeaderValues()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);

                OptionSet priorityCode = new OptionSet() { Name = "prioritycode" };
                string priorityCodeValue = xrmApp.Entity.GetHeaderValue(priorityCode);

                // Bug: Fails to resolve ownerid
                // OpenQA.Selenium.NoSuchElementException: no such element: Unable to locate element: {"method":"xpath","selector":"//div[@data-id='header_ownerId.fieldControl-Lookup_ownerId']"}
                LookupItem ownerId = new LookupItem() { Name = "ownerid" };
                string ownerIdValue = xrmApp.Entity.GetHeaderValue(ownerId);

                xrmApp.ThinkTime(2000);

            }
        }
    }
}