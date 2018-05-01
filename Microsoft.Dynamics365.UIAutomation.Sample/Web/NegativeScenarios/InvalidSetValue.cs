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
    public class InvalidSetValue
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestInvalidSetValue()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");

                xrmBrowser.CommandBar.ClickCommand("New");

                try
                {
                    xrmBrowser.Entity.SetValue("subject1", "Test API to Create leads");
                    Assert.Fail("subject1 is an invalid Id for Setvalue and hence should have failed");
                }
                catch (Exception e)
                {
                    Assert.IsTrue(true, String.Format("Exception expected: {0}", e.Message));
                }
            }
        }

        [TestMethod]
        public void WEBTestInvalidOptionSetValue()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");

                xrmBrowser.CommandBar.ClickCommand("New");

                try
                {
                    xrmBrowser.Entity.SetValue(new OptionSet { Name = "Invalidcontactmethodcode", Value = "Email" });
                    Assert.Fail("prrimarycontactid is an invalid Id for OptionSet and hence should have failed");
                }
                catch (Exception e)
                {
                    Assert.IsTrue(true, String.Format("Exception expected: {0}", e.Message));
                }

            }
        }

        [TestMethod]
        public void WEBTestInvalidOpenLookupSetValue()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.CommandBar.ClickCommand("New");

                try
                {
                    xrmBrowser.Entity.SetValue(new LookupItem { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });
                    Assert.Fail("prrimarycontactid is an invalid Id for lookup and hence should have failed");
                }
                catch (Exception e)
                {
                    Assert.IsTrue(true, String.Format("Exception expected: {0}", e.Message));
                }
            }
        }

    }
}
