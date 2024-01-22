// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class CreateAccount : TestsBase
    {
        [TestCategory("Entity")]
        [TestMethod]
        public void TestCreateAccount()
        {
            //var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var settings = configFile.AppSettings.Settings;
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue("name", TestSettings.GetRandomString(5,15));

                xrmApp.Entity.Save();
                
            }
            
        }
    }
}