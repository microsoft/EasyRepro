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
    public class QuickCreateContact : TestsBase
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

        [TestCategory("MicrosoftInternal")]
        [TestCategory("Regression")]
        [TestCategory("SetValue")]
        [TestCategory("QuickCreate")]
        [TestCategory("TestHarness")]
        [TestMethod]
        public void TestQuickCreateTestHarness()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                
                xrmApp.Navigation.QuickCreate("test harness");

                trace.Log("[EasyRepro Test][QuickCreate] - email field validation.");
                xrmApp.QuickCreate.SetValue("pfe_approver1", "Regression@testing.com");

                //trace.Log("[EasyRepro Test][QuickCreate] - custom option set field validation.");
                //xrmApp.QuickCreate.SetValue("pfe_customoptionset", "TX");

                trace.Log("[EasyRepro Test][QuickCreate] - text field validation.");
                xrmApp.QuickCreate.SetValue("pfe_name", "Regression tests");

                trace.Log("[EasyRepro Test][QuickCreate] - multi value option set field validation.");
                xrmApp.QuickCreate.SetValue("pfe_multiselectoptionset", "Allowed");

                trace.Log("[EasyRepro Test][QuickCreate] - lookup field validation.");
                xrmApp.QuickCreate.SetValue("pfe_customer", "Contoso Software");

                trace.Log("[EasyRepro Test][QuickCreate] - two options ddl field validation.");
                xrmApp.QuickCreate.SetValue("pfe_twooptions", "Yes");

                trace.Log("[EasyRepro Test][QuickCreate] - save validation.");  
                xrmApp.QuickCreate.Save();

            }
        }
    }
}