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
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Service", "Cases");

                xrmApp.CommandBar.ClickCommand("New Case");

                xrmApp.ThinkTime(2500);

                xrmApp.Entity.SetValue("title", "Test Case "+ TestSettings.GetRandomString(5,10));
                
                xrmApp.ThinkTime(2500);

                LookupItem customer = new LookupItem { Name = "customerid", Value = "David", Index = 0};
                xrmApp.Entity.SetValue(customer);

                var customerName = xrmApp.Entity.GetValue(customer);
                Assert.IsNotNull(customerName);
                Assert.IsTrue(customerName.Contains("David"));

                xrmApp.Entity.Save();
                xrmApp.ThinkTime(2500);
            }
        }
    }
}