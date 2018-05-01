// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class BusinessProcessFlow
    {
       
        private readonly SecureString  _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestLeadToOpportunityBPF()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp(5000);
                
                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");

                xrmBrowser.Grid.SwitchView("Open Leads");

                xrmBrowser.CommandBar.ClickCommand("New");

                List<Field> fields = new List<Field>
                {
                    new Field() {Id = "firstname", Value = "Test"},
                    new Field() {Id = "lastname", Value = "Lead"}
                };

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.Entity.SetValue("subject", "Test API Lead BPF");
                xrmBrowser.Entity.SetValue(new CompositeControl() { Id = "fullname", Fields = fields });
                xrmBrowser.Entity.SetValue("mobilephone", "555-555-5555");
                xrmBrowser.Entity.SetValue("description", "Test lead creation with API commands");

                xrmBrowser.CommandBar.ClickCommand("Save");

                xrmBrowser.ThinkTime(3000);

                xrmBrowser.CommandBar.ClickCommand("Qualify");

                xrmBrowser.Dialogs.QualifyLead(true, 4000);

                xrmBrowser.ThinkTime(10000);

                xrmBrowser.BusinessProcessFlow.NextStage();

                xrmBrowser.ThinkTime(3000);

                xrmBrowser.BusinessProcessFlow.PreviousStage();

                xrmBrowser.ThinkTime(3000);

                xrmBrowser.BusinessProcessFlow.Hide();

                xrmBrowser.ThinkTime(3000);

                xrmBrowser.BusinessProcessFlow.SelectStage(0, 4000);

                xrmBrowser.ThinkTime(3000);
            }
        }
        [TestMethod]
        public void WEBTestBusinessProcessFlowNextStage()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmBrowser.Grid.SwitchView("Open Opportunities");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.BusinessProcessFlow.NextStage();

            }
        }
        [TestMethod]
        public void WEBTestBusinessProcessFlowPreviousStage()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmBrowser.Grid.SwitchView("Open Opportunities");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.BusinessProcessFlow.NextStage();

                xrmBrowser.BusinessProcessFlow.PreviousStage();

            }
        }
        [TestMethod]
        public void WEBTestBusinessProcessFlowHide()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmBrowser.Grid.SwitchView("Open Opportunities");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.BusinessProcessFlow.Hide();

            }
        }

        [TestMethod]
        public void WEBTestBusinessProcessFlowSelectStage()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmBrowser.Grid.SwitchView("Open Opportunities");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.BusinessProcessFlow.Hide();

                xrmBrowser.BusinessProcessFlow.SelectStage(0);
            }
        }


        [TestMethod]
        public void WEBTestBusinessProcessFlowSetActive()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Sales", "Opportunities");

                xrmBrowser.Grid.SwitchView("Open Opportunities");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.BusinessProcessFlow.Hide();

                xrmBrowser.BusinessProcessFlow.SelectStage(0);

                xrmBrowser.BusinessProcessFlow.SetActive();
            }
        }

    }
}