// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class CreateCaseUCI : TestsBase
    {
        [TestMethod]
        public void UCITestCreateCase()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.CommandBar.ClickCommand("New Case");

                xrmApp.ThinkTime(5000);

                xrmApp.Entity.SetValue("title", TestSettings.GetRandomString(5,10));

                LookupItem customer = new LookupItem { Name = "customerid", Value = "Test Lead" };
                xrmApp.Entity.SetValue(customer);

                xrmApp.Entity.Save();
            }
        }
    }
}