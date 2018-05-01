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
    public class UpdateCase
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestUpdateCase()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Service", "Cases");

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.Grid.SwitchView("Active Cases");

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.Grid.OpenRecord(0);               

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.Entity.SetValue(new OptionSet { Name = "caseorigincode", Value = "Email" });
                xrmBrowser.Entity.Save();
                xrmBrowser.ThinkTime(10000);



            }
        }
    }
}
