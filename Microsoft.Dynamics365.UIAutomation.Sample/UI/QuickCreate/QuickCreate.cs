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
    public class QuickCreateContactUCI : TestsBase
    {

        [TestCategory("QuickCreate")]
        [TestMethod]
        public void TestQuickCreateContact()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);
                
                xrmApp.Navigation.QuickCreate("contact");
                
                xrmApp.QuickCreate.SetValue("firstname", TestSettings.GetRandomString(5,10));
                
                xrmApp.QuickCreate.SetValue("lastname", TestSettings.GetRandomString(5,10));

                xrmApp.QuickCreate.SetValue(new LookupItem() { Name = "parentcustomerid", Value="Adventure" });
                
                xrmApp.QuickCreate.Save();
                
            }
        }
    }
}