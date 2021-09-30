// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class RunWorkFlow
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestRunWorkFlow()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {


                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();
                
                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                
                xrmBrowser.Grid.SwitchView("Active Accounts");
                
                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.CommandBar.ClickCommand("Run Workflow", "", true);

                // This test is a sample use case only. Uncomment and update the display name of the workflow you wish to run from the grid in the line below
                // xrmBrowser.Dialogs.RunWorkflow("Account Set Phone Number");

                xrmBrowser.ThinkTime(10000);
            }
        }
    }
}