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
    public class DuplicateDetection : TestsBase
    {
        [TestCategory("CommandBar")]
        [TestMethod]
        public void TestDuplicateDetection()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue("firstname", "EasyRepro");
                xrmApp.Entity.SetValue("lastname", "Duplicate");
                xrmApp.Entity.SetValue("emailaddress1", "jz3@jztest.com");

                xrmApp.Entity.Save();

            }
        }
    }
}
