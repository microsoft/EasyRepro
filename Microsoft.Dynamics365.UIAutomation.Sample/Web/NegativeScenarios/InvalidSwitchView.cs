// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class InvalidSwitchView
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestInvalidSwitchView()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(100);

                try
                {
                    xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                    xrmBrowser.Grid.SwitchView("Open Accounts");
                    Assert.Fail("Acounts is an invalid Id for SubArea and hence should have failed");
                }
                catch (Exception e)
                {
                    Assert.IsTrue(true, String.Format("Exception expected: {0}", e.Message));
                }
                xrmBrowser.ThinkTime(1000);
            }
        }

    }
}
