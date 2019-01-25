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
    public class QuickCreateAccount
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestQuickCreateNewAccount()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.QuickCreate("Account");

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.QuickCreate.SetValue("name", "Test API Account"); //Text field
                var qcNameValue = xrmBrowser.QuickCreate.GetValue("name").Value;
                xrmBrowser.QuickCreate.ClearValue("name");
                xrmBrowser.QuickCreate.SetValue("name", "Test API Account"); //Text field


                xrmBrowser.QuickCreate.SetValue(new LookupItem { Name = "primarycontactid", Index = 0 }); //Lookup field
                var qcLookupValue = xrmBrowser.QuickCreate.GetValue(new LookupItem { Name = "primarycontactid" }).Value;
                xrmBrowser.QuickCreate.ClearValue(new LookupItem { Name = "primarycontactid" });

                xrmBrowser.ThinkTime(1000);

                xrmBrowser.QuickCreate.SelectLookup(new LookupItem { Name = "primarycontactid"}); //Lookup field

                xrmBrowser.Lookup.Search("test");

                xrmBrowser.Lookup.SelectItem(1);

                xrmBrowser.Lookup.Add();

                xrmBrowser.QuickCreate.Save();

                xrmBrowser.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void WEBTestQuickCreateNewLead()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.ThinkTime(500);

                xrmBrowser.Navigation.QuickCreate("Lead");

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.QuickCreate.SetValue("subject", "Test API Lead"); //Text field
                xrmBrowser.QuickCreate.SetValue("firstname", "Test"); //Text field
                xrmBrowser.QuickCreate.SetValue("lastname", "API Lead"); //Text field

                xrmBrowser.QuickCreate.SetValue(new OptionSet { Name = "leadsourcecode", Value = "External Referral" }); //OptionSet field
                var qcOptionSetValue = xrmBrowser.QuickCreate.GetValue(new OptionSet { Name = "leadsourcecode" }).Value;
                xrmBrowser.QuickCreate.ClearValue(new OptionSet { Name = "leadsourcecode" });

                xrmBrowser.ThinkTime(1000);

                xrmBrowser.QuickCreate.Save();

                xrmBrowser.ThinkTime(2000);
            }
        }
    }
}