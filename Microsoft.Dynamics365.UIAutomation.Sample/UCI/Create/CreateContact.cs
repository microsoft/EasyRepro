// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class CreateContactUCI : TestsBase
    {
        [TestMethod]
        public void UCITestCreateContact()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue("firstname", TestSettings.GetRandomString(5,10));
                xrmApp.Entity.SetValue("lastname", TestSettings.GetRandomString(5,10));

                xrmApp.Entity.Save();

            }
            
        }
    }
}