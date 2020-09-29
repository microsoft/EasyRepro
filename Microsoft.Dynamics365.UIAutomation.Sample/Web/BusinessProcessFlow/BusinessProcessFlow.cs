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

                xrmBrowser.BusinessProcessFlow.NextStage();

                xrmBrowser.ThinkTime(3000);

                xrmBrowser.BusinessProcessFlow.Finish();

                xrmBrowser.ThinkTime(3000);
            }
        }

        [TestMethod]
        public void WEBTestBusinessProcessFlowNextPreviousStage()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Service", "Cases");

                xrmBrowser.Grid.SwitchView("Active Cases");

                xrmBrowser.Grid.Search("Faulty product catalog");

                xrmBrowser.Grid.OpenRecord(0);

                // If using on an entity that requires selection of a record, use NextStage(int index);
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

                xrmBrowser.BusinessProcessFlow.SelectStage(1);
            }
        }

        [TestMethod]
        public void WEBTestBusinessProcessFlowSetActive()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                xrmBrowser.Navigation.OpenSubArea("Service", "Cases");

                xrmBrowser.Grid.SwitchView("Active Cases");

                xrmBrowser.Grid.OpenRecord(0);

                xrmBrowser.BusinessProcessFlow.NextStage();

                xrmBrowser.BusinessProcessFlow.SelectStage(0);

                xrmBrowser.BusinessProcessFlow.SetActive();

            }
        }

        [TestMethod]
        public void WEBTestLeadToOpportunityBPFClearAndSetValue()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.GuidedHelp.CloseGuidedHelp();

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

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.BusinessProcessFlow.SetValue(new LookupItem { Name = "parentaccountid", Value = "Adventure Works" });

                xrmBrowser.ThinkTime(2000);

                var lookupValue = xrmBrowser.BusinessProcessFlow.GetValue(new LookupItem { Name = "parentaccountid" }).Value;

                Assert.IsNotNull(lookupValue);

                xrmBrowser.BusinessProcessFlow.ClearValue(new LookupItem { Name = "parentaccountid" });

                lookupValue = xrmBrowser.BusinessProcessFlow.GetValue(new LookupItem { Name = "parentaccountid" }).Value;

                Assert.AreEqual("click to enter", lookupValue.ToLower());

                // Set Value of a Textbox field in a Business Process Flow
                xrmBrowser.BusinessProcessFlow.SetValue("Description", "Test description value in BPF");

                xrmBrowser.ThinkTime(2000);

                var textValue = xrmBrowser.BusinessProcessFlow.GetValue("description").Value;

                Assert.IsNotNull(textValue);

                xrmBrowser.BusinessProcessFlow.ClearValue("description");

                textValue = xrmBrowser.BusinessProcessFlow.GetValue("description").Value;
                
                Assert.AreEqual("", textValue);

                xrmBrowser.BusinessProcessFlow.SetValue(new OptionSet { Name = "purchaseprocess", Value = "Committee" });

                xrmBrowser.ThinkTime(2000);

                var optionValue = xrmBrowser.BusinessProcessFlow.GetValue(new OptionSet { Name = "purchaseprocess" }).Value;

                Assert.IsNotNull(optionValue);

                xrmBrowser.BusinessProcessFlow.ClearValue(new OptionSet { Name = "purchaseprocess" });

                optionValue = xrmBrowser.BusinessProcessFlow.GetValue(new OptionSet { Name = "purchaseprocess" }).Value;

                Assert.AreEqual("click to enter", optionValue.ToLower());

                xrmBrowser.CommandBar.ClickCommand("Save");

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.CommandBar.ClickCommand("Qualify");

                xrmBrowser.Dialogs.QualifyLead(true, 4000);

                xrmBrowser.ThinkTime(10000);

                xrmBrowser.BusinessProcessFlow.PreviousStage();

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.BusinessProcessFlow.NextStage(0, 2000);

                xrmBrowser.ThinkTime(2000);

                // Set Value on a TwoOption field in a Business Process Flow
                xrmBrowser.BusinessProcessFlow.SetValue(new TwoOption { Name = "identifycustomercontacts", Value = "Completed"});

                xrmBrowser.ThinkTime(1000);

                var checkBoxValue = xrmBrowser.BusinessProcessFlow.GetValue(new TwoOption { Name = "identifycustomercontacts" }).Value;

                Assert.IsTrue(checkBoxValue);

                xrmBrowser.BusinessProcessFlow.ClearValue(new TwoOption { Name = "identifycustomercontacts" });

                checkBoxValue = xrmBrowser.BusinessProcessFlow.GetValue(new TwoOption { Name = "identifycustomercontacts" }).Value;

                Assert.IsFalse(checkBoxValue);

                xrmBrowser.BusinessProcessFlow.NextStage();

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.BusinessProcessFlow.NextStage();

                xrmBrowser.ThinkTime(2000);

                xrmBrowser.BusinessProcessFlow.SetValue(new DateTimeControl { Name = "finaldecisiondate", Value = DateTime.Parse("01/15/2019") });

                var dateTimeValue = xrmBrowser.BusinessProcessFlow.GetValue(new DateTimeControl { Name = "finaldecisiondate" }).Value;

                Assert.IsNotNull(dateTimeValue);

                xrmBrowser.BusinessProcessFlow.ClearValue(new DateTimeControl { Name = "finaldecisiondate"});

                dateTimeValue = xrmBrowser.BusinessProcessFlow.GetValue(new DateTimeControl { Name = "finaldecisiondate" }).Value;

                Assert.AreEqual("--",dateTimeValue);

                xrmBrowser.BusinessProcessFlow.Finish();

                xrmBrowser.BusinessProcessFlow.Hide();

                xrmBrowser.ThinkTime(3000);
            }
        }
    }
}