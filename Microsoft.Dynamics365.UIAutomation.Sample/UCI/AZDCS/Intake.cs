// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using System.Collections.Generic;
namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI.AZDCS
{
    [TestClass]
    public class Intake
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TC_30588()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                List<string> allActionRequestintakeTypes = new List<string>();
                allActionRequestintakeTypes.Add("Action Request - Court Ordered Investigation");
                allActionRequestintakeTypes.Add("Action Request - Court Ordered Pickup");
                allActionRequestintakeTypes.Add("Action Request - Courtesy Assessment");
                allActionRequestintakeTypes.Add("Action Request - Courtesy Placement");
                allActionRequestintakeTypes.Add("Action Request - Successor of Permanent Guardian");
                foreach (string actionrequesttype in allActionRequestintakeTypes)
                {
                    xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                    xrmApp.ThinkTime(2000);
                    xrmApp.CommandBar.ClickCommand("New");
                    xrmApp.ThinkTime(10000);

                    //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                    LookupItem mcshhs_intaketype = new LookupItem();
                    mcshhs_intaketype.Name = "mcshhs_intaketype";
                    mcshhs_intaketype.Value = actionrequesttype;
                    xrmApp.Entity.SetValue(mcshhs_intaketype);

                    xrmApp.Dialogs.ConfirmationDialog(true);

                    xrmApp.ThinkTime(2000);
                    //Validate Mandatory Fields
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.ThinkTime(2000);
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                    //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                    xrmApp.Entity.SelectTab("General");

                    MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                    mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                    mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                    xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                    OptionSet mcshhs_highprioritystatus = new OptionSet();
                    mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                    mcshhs_highprioritystatus.Value = "Acknowledged";
                    //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                    OptionSet mcshhs_priority = new OptionSet();
                    mcshhs_priority.Name = "mcshhs_priority";
                    mcshhs_priority.Value = "1";
                    xrmApp.Entity.SetValue(mcshhs_priority);

                    BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                    mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                    mcshhs_addsourcetointakeperson.Value = true;
                    xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                    xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                    LookupItem mcshhs_sourcetitle = new LookupItem();
                    mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                    mcshhs_sourcetitle.Value = "Nurse";
                    xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                    LookupItem mcshhs_sourcechanneltype = new LookupItem();
                    mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                    mcshhs_sourcechanneltype.Value = "Fax";
                    xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                    //LookupItem mcshhs_sourcesetting = new LookupItem();
                    //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                    //mcshhs_sourcesetting.Value = "Hospital";
                    //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                    LookupItem mcshhs_sourcetype = new LookupItem();
                    mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                    mcshhs_sourcetype.Value = "Doctor";
                    //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                    xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                    xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                    xrmApp.Entity.SelectTab("General");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                    //xrmApp.Entity.SelectTab("Narrative");
                    //xrmApp.ThinkTime(2000);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                    xrmApp.ThinkTime(1000);
                    xrmApp.Entity.Save();

                    xrmApp.ThinkTime(10000);


                    xrmApp.Entity.SelectTab("Additional Information");

                    xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                    xrmApp.ThinkTime(10000);

                    xrmApp.Entity.SelectTab("Incident Demographic Information");
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                    LookupItem mcshhs_incidentstate = new LookupItem();
                    mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                    mcshhs_incidentstate.Value = "Alaska";
                    xrmApp.Entity.SetValue(mcshhs_incidentstate);

                    xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                    LookupItem mcshhs_incidentcounty = new LookupItem();
                    mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                    mcshhs_incidentcounty.Value = "Gila County";
                    xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                    LookupItem mcshhs_incidentcountry = new LookupItem();
                    mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                    mcshhs_incidentcountry.Value = "Bahrain";
                    xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                    xrmApp.ThinkTime(10000);

                    //xrmApp.Entity.SelectTab("After Hours");

                    //LookupItem mcshhs_afterhoursoncallteam = new LookupItem();
                    //mcshhs_afterhoursoncallteam.Name = "mcshhs_afterhoursoncallteam";
                    //mcshhs_afterhoursoncallteam.Value = "DCS Specialist";
                    //xrmApp.Entity.SetValue(mcshhs_afterhoursoncallteam);

                    //xrmApp.ThinkTime(10000);

                    xrmApp.Entity.SelectTab("General");
                }
            }
        }

        [TestMethod]
        public void TC_30589()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Intake Addendum";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);
                xrmApp.ThinkTime(2000);

                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                //OptionSet mcshhs_highprioritystatus = new OptionSet();
                //mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                //mcshhs_highprioritystatus.Value = "Acknowledged";
                ////xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                //LookupItem mcshhs_sourcetype = new LookupItem();
                //mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                //mcshhs_sourcetype.Value = "Doctor";
                ////xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.ThinkTime(2000);
                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                //xrmApp.ThinkTime(2000);
                //xrmApp.Entity.SelectTab("General");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.ThinkTime(2000);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                Console.WriteLine(xrmApp.Entity.GetValue("mcshhs_sourcefirstname"));
                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.Entity.Save();
                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.Entity.Save();
                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("General");
            }
        }

        [TestMethod]
        public void TC_30590()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                List<string> alertintakeTypes = new List<string>();
                alertintakeTypes.Add("Alert - Notification from Law Enforcement");
                alertintakeTypes.Add("Alert - Notification from other state/Child Welfare Agency");
                alertintakeTypes.Add("Alert - Other (needed by the Field)");
                alertintakeTypes.Add("Alert - Potential Placement on Non-Open Case/Report");
                foreach (string alertintaketype in alertintakeTypes)
                {
                    xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                    xrmApp.ThinkTime(2000);
                    xrmApp.CommandBar.ClickCommand("New");
                    xrmApp.ThinkTime(10000);

                    //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                    LookupItem mcshhs_intaketype = new LookupItem();
                    mcshhs_intaketype.Name = "mcshhs_intaketype";
                    mcshhs_intaketype.Value = alertintaketype;
                    xrmApp.Entity.SetValue(mcshhs_intaketype);

                    xrmApp.Dialogs.ConfirmationDialog(true);

                    xrmApp.ThinkTime(2000);
                    //Validate Mandatory Fields
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.ThinkTime(2000);
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                    //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                    xrmApp.Entity.SelectTab("General");

                    MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                    mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                    mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                    xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                    OptionSet mcshhs_highprioritystatus = new OptionSet();
                    mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                    mcshhs_highprioritystatus.Value = "Acknowledged";
                    //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                    OptionSet mcshhs_priority = new OptionSet();
                    mcshhs_priority.Name = "mcshhs_priority";
                    mcshhs_priority.Value = "1";
                    xrmApp.Entity.SetValue(mcshhs_priority);

                    BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                    mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                    mcshhs_addsourcetointakeperson.Value = true;
                    xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                    xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                    LookupItem mcshhs_sourcetitle = new LookupItem();
                    mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                    mcshhs_sourcetitle.Value = "Nurse";
                    xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                    LookupItem mcshhs_sourcechanneltype = new LookupItem();
                    mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                    mcshhs_sourcechanneltype.Value = "Fax";
                    xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                    //LookupItem mcshhs_sourcesetting = new LookupItem();
                    //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                    //mcshhs_sourcesetting.Value = "Hospital";
                    //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                    LookupItem mcshhs_sourcetype = new LookupItem();
                    mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                    mcshhs_sourcetype.Value = "Doctor";
                    //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                    xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                    xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                    //xrmApp.Entity.SelectTab("General");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                    //xrmApp.Entity.SelectTab("Narrative");
                    //xrmApp.ThinkTime(2000);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                    xrmApp.ThinkTime(1000);
                    xrmApp.Entity.Save();

                    xrmApp.ThinkTime(10000);


                    xrmApp.Entity.SelectTab("Additional Information");
                    xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                    xrmApp.ThinkTime(10000);

                    xrmApp.Entity.SelectTab("Incident Demographic Information");
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                    LookupItem mcshhs_incidentstate = new LookupItem();
                    mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                    mcshhs_incidentstate.Value = "Alaska";
                    xrmApp.Entity.SetValue(mcshhs_incidentstate);

                    xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                    LookupItem mcshhs_incidentcounty = new LookupItem();
                    mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                    mcshhs_incidentcounty.Value = "Gila County";
                    xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                    LookupItem mcshhs_incidentcountry = new LookupItem();
                    mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                    mcshhs_incidentcountry.Value = "Bahrain";
                    xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                    xrmApp.ThinkTime(10000);
                    xrmApp.Entity.SelectTab("General");
                }
            }
        }

        [TestMethod]
        public void TC_30597()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Community Inquiry";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                //xrmApp.Entity.SelectTab("General");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.ThinkTime(2000);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());

                //xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("General");
            }
        }

        [TestMethod]
        public void TC_30591()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);

                List<string> dcsintakeTypes = new List<string>();
                dcsintakeTypes.Add("DCS History Request - Child Welfare Agency");
                dcsintakeTypes.Add("DCS History Request - Law Enforcement");
                foreach (string dcsintaketype in dcsintakeTypes)
                {
                    xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                    xrmApp.ThinkTime(2000);
                    xrmApp.CommandBar.ClickCommand("New");
                    xrmApp.ThinkTime(10000);

                    //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                    LookupItem mcshhs_intaketype = new LookupItem();
                    mcshhs_intaketype.Name = "mcshhs_intaketype";
                    mcshhs_intaketype.Value = dcsintaketype;
                    xrmApp.Entity.SetValue(mcshhs_intaketype);

                    xrmApp.Dialogs.ConfirmationDialog(true);

                    xrmApp.ThinkTime(2000);
                    //Validate Mandatory Fields
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.ThinkTime(2000);
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                    //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                    xrmApp.Entity.SelectTab("General");

                    MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                    mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                    mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                    xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                    OptionSet mcshhs_highprioritystatus = new OptionSet();
                    mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                    mcshhs_highprioritystatus.Value = "Acknowledged";
                    //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                    OptionSet mcshhs_priority = new OptionSet();
                    mcshhs_priority.Name = "mcshhs_priority";
                    mcshhs_priority.Value = "1";
                    xrmApp.Entity.SetValue(mcshhs_priority);

                    BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                    mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                    mcshhs_addsourcetointakeperson.Value = true;
                    xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                    xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                    LookupItem mcshhs_sourcetitle = new LookupItem();
                    mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                    mcshhs_sourcetitle.Value = "Nurse";
                    xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                    LookupItem mcshhs_sourcechanneltype = new LookupItem();
                    mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                    mcshhs_sourcechanneltype.Value = "Fax";
                    xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                    //LookupItem mcshhs_sourcesetting = new LookupItem();
                    //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                    //mcshhs_sourcesetting.Value = "Hospital";
                    //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                    LookupItem mcshhs_sourcetype = new LookupItem();
                    mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                    mcshhs_sourcetype.Value = "Doctor";
                    //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                    xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                    xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                    //xrmApp.Entity.SelectTab("General");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                    //xrmApp.Entity.SelectTab("Narrative");
                    //xrmApp.ThinkTime(2000);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                    xrmApp.ThinkTime(1000);
                    xrmApp.Entity.Save();

                    xrmApp.ThinkTime(10000);


                    xrmApp.Entity.SelectTab("Additional Information");

                    xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                    //xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                    xrmApp.ThinkTime(10000);

                    xrmApp.Entity.SelectTab("Incident Demographic Information");
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                    LookupItem mcshhs_incidentstate = new LookupItem();
                    mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                    mcshhs_incidentstate.Value = "Alaska";
                    xrmApp.Entity.SetValue(mcshhs_incidentstate);

                    xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                    LookupItem mcshhs_incidentcounty = new LookupItem();
                    mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                    mcshhs_incidentcounty.Value = "Gila County";
                    xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                    LookupItem mcshhs_incidentcountry = new LookupItem();
                    mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                    mcshhs_incidentcountry.Value = "Bahrain";
                    xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                    xrmApp.ThinkTime(10000);

                    xrmApp.Entity.SelectTab("General");
                }
            }
        }

        [TestMethod]
        public void TC_30596()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Employee Report";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                //xrmApp.Entity.SelectTab("General");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.ThinkTime(2000);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("General");
            }
        }
        [TestMethod]
        public void TC_30592()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Entered in Error";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                //xrmApp.Entity.SelectTab("General");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.ThinkTime(2000);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("General");
            }
        }

        [TestMethod]
        public void TC_30593()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Screened Out Intake";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                //xrmApp.Entity.SelectTab("General");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.ThinkTime(2000);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("General");
            }
        }

        [TestMethod]
        public void TC_30594()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Licensing Issue";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                //xrmApp.Entity.SelectTab("General");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.ThinkTime(2000);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("General");
            }
        }

        [TestMethod]
        public void TC_30595()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Report";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                //xrmApp.Entity.SelectTab("General");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.ThinkTime(2000);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("General");
            }
        }

        [TestMethod]
        public void TC_30598()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                List<string> statuscommunicationintakeTypes = new List<string>();
                statuscommunicationintakeTypes.Add("Status Communication - AWOL");
                statuscommunicationintakeTypes.Add("Status Communication - DCS History Request (Law Enforcement)");
                statuscommunicationintakeTypes.Add("Status Communication - Medical Consent");
                statuscommunicationintakeTypes.Add("Status Communication - Non-Report");
                statuscommunicationintakeTypes.Add("Status Communication - Pick Up of Court Ward");
                statuscommunicationintakeTypes.Add("Status Communication - Placement Disruption");
                statuscommunicationintakeTypes.Add("Status Communication - Request for Case Manager Contact");
                statuscommunicationintakeTypes.Add("Status Communication - Request to be Placement");
                statuscommunicationintakeTypes.Add("Status Communication-DCS History Request (Child Welfare Agency)");
                statuscommunicationintakeTypes.Add("Status Communication-Other");
                foreach (string statuscommunicationintakeType in statuscommunicationintakeTypes)
                {
                    xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                    xrmApp.ThinkTime(2000);
                    xrmApp.CommandBar.ClickCommand("New");
                    xrmApp.ThinkTime(10000);

                    //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                    LookupItem mcshhs_intaketype = new LookupItem();
                    mcshhs_intaketype.Name = "mcshhs_intaketype";
                    mcshhs_intaketype.Value = statuscommunicationintakeType;
                    xrmApp.Entity.SetValue(mcshhs_intaketype);
                    xrmApp.ThinkTime(2000);
                    xrmApp.Dialogs.ConfirmationDialog(true);

                    xrmApp.ThinkTime(2000);
                    //Validate Mandatory Fields
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.ThinkTime(2000);
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                    //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                    xrmApp.Entity.SelectTab("General");

                    MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                    mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                    mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                    xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                    OptionSet mcshhs_highprioritystatus = new OptionSet();
                    mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                    mcshhs_highprioritystatus.Value = "Acknowledged";
                    //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                    OptionSet mcshhs_priority = new OptionSet();
                    mcshhs_priority.Name = "mcshhs_priority";
                    mcshhs_priority.Value = "1";
                    xrmApp.Entity.SetValue(mcshhs_priority);

                    BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                    mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                    mcshhs_addsourcetointakeperson.Value = true;
                    xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                    xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                    LookupItem mcshhs_sourcetitle = new LookupItem();
                    mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                    mcshhs_sourcetitle.Value = "Nurse";
                    xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                    LookupItem mcshhs_sourcechanneltype = new LookupItem();
                    mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                    mcshhs_sourcechanneltype.Value = "Fax";
                    xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                    //LookupItem mcshhs_sourcesetting = new LookupItem();
                    //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                    //mcshhs_sourcesetting.Value = "Hospital";
                    //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                    LookupItem mcshhs_sourcetype = new LookupItem();
                    mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                    mcshhs_sourcetype.Value = "Doctor";
                    //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                    xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                    xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                    //xrmApp.Entity.SelectTab("General");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                    //xrmApp.Entity.SelectTab("Narrative");
                    //xrmApp.ThinkTime(2000);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                    xrmApp.ThinkTime(1000);
                    xrmApp.Entity.Save();

                    xrmApp.ThinkTime(10000);


                    xrmApp.Entity.SelectTab("Additional Information");

                    xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                    xrmApp.ThinkTime(10000);

                    xrmApp.Entity.SelectTab("Incident Demographic Information");
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                    LookupItem mcshhs_incidentstate = new LookupItem();
                    mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                    mcshhs_incidentstate.Value = "Alaska";
                    xrmApp.Entity.SetValue(mcshhs_incidentstate);

                    xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                    LookupItem mcshhs_incidentcounty = new LookupItem();
                    mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                    mcshhs_incidentcounty.Value = "Gila County";
                    xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                    LookupItem mcshhs_incidentcountry = new LookupItem();
                    mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                    mcshhs_incidentcountry.Value = "Bahrain";
                    xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                    xrmApp.ThinkTime(10000);

                    xrmApp.Entity.SelectTab("General");
                }
            }
        }

        [TestMethod]
        public void TC_30599()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Unborn Concern";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                //xrmApp.Entity.SelectTab("General");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.ThinkTime(2000);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("After Hours");

                LookupItem mcshhs_afterhoursoncallteam = new LookupItem();
                mcshhs_afterhoursoncallteam.Name = "mcshhs_afterhoursoncallteam";
                mcshhs_afterhoursoncallteam.Value = "DCS Specialist";
                xrmApp.Entity.SetValue(mcshhs_afterhoursoncallteam);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("General");
            }
        }

        [TestMethod]
        public void CreateIntake_ValidateMandatoryWithAnonymous()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Unborn Concern";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcefirstname"), false, "Source First Name is Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcemiddlename"), false, "Source Middle Name is Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcelastname"), false, "Source Last Name is Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcetitle"), false, "Source Title is Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcechanneltype"), false, "Source Channel Type is Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcetype"), false, "Source Type is Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourceemployer"), false, "Source Employer is Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourceemail"), false, "Source Email is Locked");

                xrmApp.ThinkTime(2000);
                BooleanItem mcshhs_anonymousreportingsource = new BooleanItem();
                mcshhs_anonymousreportingsource.Name = "mcshhs_anonymousreportingsource";
                mcshhs_anonymousreportingsource.Value = true;
                xrmApp.Entity.SetValue(mcshhs_anonymousreportingsource);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //Validate the fileds are locked
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcefirstname"), true, "Source First Name is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcemiddlename"), true, "Source Middle Name is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcelastname"), true, "Source Last Name is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcetitle"), true, "Source Title is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcechanneltype"), true, "Source Channel Type is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcetype"), true, "Source Type is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourceemployer"), true, "Source Employer is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourceemail"), true, "Source Email is Not Locked");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");


                xrmApp.Entity.SelectTab("General");
                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                //BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                //mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                //mcshhs_addsourcetointakeperson.Value = true;
                //xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                //xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                //xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                //xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                //xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                //LookupItem mcshhs_sourcetitle = new LookupItem();
                //mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                //mcshhs_sourcetitle.Value = "Nurse";
                //xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                //LookupItem mcshhs_sourcechanneltype = new LookupItem();
                //mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                //mcshhs_sourcechanneltype.Value = "Fax";
                //xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                //LookupItem mcshhs_sourcetype = new LookupItem();
                //mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                //mcshhs_sourcetype.Value = "Doctor";
                ////xrmApp.Entity.SetValue(mcshhs_sourcetype);

                //xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                //xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                //xrmApp.Entity.SelectTab("General");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                //xrmApp.Entity.SelectTab("Narrative");
                //xrmApp.ThinkTime(2000);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("After Hours");

                LookupItem mcshhs_afterhoursoncallteam = new LookupItem();
                mcshhs_afterhoursoncallteam.Name = "mcshhs_afterhoursoncallteam";
                mcshhs_afterhoursoncallteam.Value = "DCS Specialist";
                xrmApp.Entity.SetValue(mcshhs_afterhoursoncallteam);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("General");
            }
        }

        [TestMethod]
        public void CreateIntake_ValidateMandatoryWithAnonymous1()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);
                BooleanItem mcshhs_anonymousreportingsource = new BooleanItem();
                mcshhs_anonymousreportingsource.Name = "mcshhs_anonymousreportingsource";
                mcshhs_anonymousreportingsource.Value = true;
                xrmApp.Entity.SetValue(mcshhs_anonymousreportingsource);
                xrmApp.ThinkTime(2000);
                //Validate the fileds are locked
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcefirstname"), true, "Source First Name is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcemiddlename"), true, "Source Middle Name is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcelastname"), true, "Source Last Name is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcetitle"), true, "Source Title is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcechanneltype"), true, "Source Channel Type is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcetype"), true, "Source Type is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourceemployer"), true, "Source Employer is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourceemail"), true, "Source Email is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");
                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");
                xrmApp.Entity.Save();

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");
                xrmApp.Entity.SelectTab("General");
                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Unborn Concern";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields

                //Validate the fileds are locked
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcefirstname"), true, "Source First Name is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcemiddlename"), true, "Source Middle Name is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcelastname"), true, "Source Last Name is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcetitle"), true, "Source Title is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcechanneltype"), true, "Source Channel Type is Not Locked");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourcetype"), true, "Source Type is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourceemployer"), true, "Source Employer is Not Locked");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldLocked("mcshhs_sourceemail"), true, "Source Email is Not Locked");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                xrmApp.Entity.SelectTab("General");
                xrmApp.ThinkTime(2000);
                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                xrmApp.Entity.SelectTab("General");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("After Hours");

                LookupItem mcshhs_afterhoursoncallteam = new LookupItem();
                mcshhs_afterhoursoncallteam.Name = "mcshhs_afterhoursoncallteam";
                mcshhs_afterhoursoncallteam.Value = "DCS Specialist";
                xrmApp.Entity.SetValue(mcshhs_afterhoursoncallteam);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("General");
            }
        }

        [TestMethod]
        public void CreateIntake_UnbornConcern_CriminalConduct()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Unborn Concern";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                xrmApp.Entity.SelectTab("General");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");
                xrmApp.ThinkTime(2000);
                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Incident Demographic Information");
                xrmApp.ThinkTime(2000);
                xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.Entity.SetValue(mcshhs_incidentstate);

                xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                //xrmApp.Entity.SelectTab("After Hours");
                //xrmApp.ThinkTime(2000);
                //LookupItem mcshhs_afterhoursoncallteam = new LookupItem();
                //mcshhs_afterhoursoncallteam.Name = "mcshhs_afterhoursoncallteam";
                //mcshhs_afterhoursoncallteam.Value = "DCS Specialist";
                //xrmApp.Entity.SetValue(mcshhs_afterhoursoncallteam);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Related", "Criminal Conduct Screenings");

                xrmApp.ThinkTime(3000);
                xrmApp.CommandBar.ClickRelatedGridCommand("Add New Criminal Conduct Screening");

                xrmApp.ThinkTime(3000);

                xrmApp.Entity.SetValue("mcshhs_criminalconductid", Guid.NewGuid().ToString());
                xrmApp.Entity.SelectTab("Screening");
                xrmApp.ThinkTime(3000);
                List<string> alloptions = new List<string>();
                alloptions.Add("mcshhs_unexpecteddeath");
                alloptions.Add("mcshhs_threatenedwithweapon");
                alloptions.Add("mcshhs_sexualabuse");
                alloptions.Add("mcshhs_suspectednonaccidentalunexplainedinjuri");
                alloptions.Add("mcshhs_mechanizedinjury");
                alloptions.Add("mcshhs_abusiveconduct");
                alloptions.Add("mcshhs_unreasonableconfinement");
                alloptions.Add("mcshhs_weaponviolencewithinproximity");
                alloptions.Add("mcshhs_domesticviolencewithinproximity");
                alloptions.Add("mcshhs_drugmanufacturingwithinproximity");
                alloptions.Add("mcshhs_fleeingfromhome");
                alloptions.Add("mcshhs_aloneinvehicle");
                alloptions.Add("mcshhs_usedfordrugdistribution");
                alloptions.Add("mcshhs_illegaldrugusepermitted");
                alloptions.Add("mcshhs_drivenunderinfluence");
                alloptions.Add("mcshhs_intentionaldrowning");
                alloptions.Add("mcshhs_unrestrainedinvehicularaccident");
                alloptions.Add("mcshhs_lawenforcementfelonyreport");
                foreach (string s in alloptions)
                {
                    OptionSet eachquestion = new OptionSet();
                    eachquestion.Name = s;
                    eachquestion.Value = "Exists";
                    xrmApp.Entity.SetValue(eachquestion);
                    xrmApp.ThinkTime(1000);
                }
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


            }
        }

        [TestMethod]
        public void CreateIntake_UnbornConcern_IntakePerson()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Unborn Concern";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");

                MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                OptionSet mcshhs_highprioritystatus = new OptionSet();
                mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                mcshhs_highprioritystatus.Value = "Acknowledged";
                //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                OptionSet mcshhs_priority = new OptionSet();
                mcshhs_priority.Name = "mcshhs_priority";
                mcshhs_priority.Value = "1";
                xrmApp.Entity.SetValue(mcshhs_priority);

                BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                mcshhs_addsourcetointakeperson.Value = true;
                xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                //LookupItem mcshhs_sourcesetting = new LookupItem();
                //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                //mcshhs_sourcesetting.Value = "Hospital";
                //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                xrmApp.Entity.SelectTab("General");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                //xrmApp.Entity.SelectTab("Additional Information");
                //xrmApp.ThinkTime(2000);
                //xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                ////xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                ////xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                //xrmApp.ThinkTime(10000);

                //xrmApp.Entity.SelectTab("Incident Demographic Information");
                //xrmApp.ThinkTime(2000);
                //xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                //LookupItem mcshhs_incidentstate = new LookupItem();
                //mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                //mcshhs_incidentstate.Value = "Alaska";
                //xrmApp.Entity.SetValue(mcshhs_incidentstate);

                //xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                //LookupItem mcshhs_incidentcounty = new LookupItem();
                //mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                //mcshhs_incidentcounty.Value = "Gila County";
                //xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                //LookupItem mcshhs_incidentcountry = new LookupItem();
                //mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                //mcshhs_incidentcountry.Value = "Bahrain";
                //xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                //xrmApp.Entity.SelectTab("After Hours");
                //xrmApp.ThinkTime(2000);
                //LookupItem mcshhs_afterhoursoncallteam = new LookupItem();
                //mcshhs_afterhoursoncallteam.Name = "mcshhs_afterhoursoncallteam";
                //mcshhs_afterhoursoncallteam.Value = "DCS Specialist";
                //xrmApp.Entity.SetValue(mcshhs_afterhoursoncallteam);

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Persons");

                xrmApp.ThinkTime(3000);
                xrmApp.CommandBar.ClickSubGridCommand("Add New Intake Person");

                xrmApp.ThinkTime(3000);

                BooleanItem mcshhs_primarycaretaker = new BooleanItem();
                mcshhs_primarycaretaker.Name = "mcshhs_primarycaretaker";
                mcshhs_primarycaretaker.Value = true;
                xrmApp.QuickCreate.SetValue(mcshhs_primarycaretaker);

                Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_intakepersontype"), true, "Intake Person Type is not set Mandatory");
                Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_street1"), true, "Street 1 is not set Mandatory");
                Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_street2"), true, "Street 2 is not set Mandatory");
                Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_city"), true, "City is not set Mandatory");
                Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_state"), true, "State is not set Mandatory");
                Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_crossroads"), true, "Cross Roads is not set Mandatory");
                Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_county"), true, "County is not set Mandatory");
                Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_country"), true, "Country is not set Mandatory");
                Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_zipcode"), true, "Zip Code is not set Mandatory");

                xrmApp.QuickCreate.SetValue("mcshhs_firstname", Guid.NewGuid().ToString());
                xrmApp.QuickCreate.SetValue("mcshhs_lastname", Guid.NewGuid().ToString());
                xrmApp.QuickCreate.SetValue("mcshhs_middlename", Guid.NewGuid().ToString());

                LookupItem mcshhs_intakepersontype = new LookupItem();
                mcshhs_intakepersontype.Name = "mcshhs_intakepersontype";
                mcshhs_intakepersontype.Value = "Alleged Perpetrator";
                xrmApp.QuickCreate.SetValue(mcshhs_intakepersontype);

                xrmApp.QuickCreate.SetValue("mcshhs_street1", Guid.NewGuid().ToString());
                xrmApp.QuickCreate.SetValue("mcshhs_street2", Guid.NewGuid().ToString());
                xrmApp.QuickCreate.SetValue("mcshhs_crossroads", Guid.NewGuid().ToString());
                xrmApp.QuickCreate.SetValue("mcshhs_city", Guid.NewGuid().ToString());
                xrmApp.QuickCreate.SetValue("mcshhs_employer", Guid.NewGuid().ToString());

                LookupItem mcshhs_incidentstate = new LookupItem();
                mcshhs_incidentstate.Name = "mcshhs_state";
                mcshhs_incidentstate.Value = "Alaska";
                xrmApp.QuickCreate.SetValue(mcshhs_incidentstate);

                xrmApp.QuickCreate.SetValue("mcshhs_zipcode", "52010");

                LookupItem mcshhs_incidentcounty = new LookupItem();
                mcshhs_incidentcounty.Name = "mcshhs_county";
                mcshhs_incidentcounty.Value = "Gila County";
                xrmApp.QuickCreate.SetValue(mcshhs_incidentcounty);


                LookupItem mcshhs_incidentcountry = new LookupItem();
                mcshhs_incidentcountry.Name = "mcshhs_country";
                mcshhs_incidentcountry.Value = "Bahrain";
                xrmApp.QuickCreate.SetValue(mcshhs_incidentcountry);

                xrmApp.QuickCreate.Save();

                xrmApp.ThinkTime(10000);


            }
        }

        [TestMethod]
        public void VariousColumnSortings()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);


                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");

                xrmApp.Grid.SwitchView("Intake - All Drafts");

                xrmApp.Grid.Search("4000044");

                xrmApp.ThinkTime(3000);
                List<GridItem> allitems = xrmApp.Grid.GetGridItems(true);
                Console.WriteLine(allitems.Count);

                xrmApp.Grid.SwitchView("Intake - All Drafts");

                allitems = xrmApp.Grid.GetGridItems(true);
                Console.WriteLine(allitems.Count);
                xrmApp.Grid.Sort("Created On");
                xrmApp.ThinkTime(3000);
                xrmApp.Grid.Sort("ID");
                xrmApp.ThinkTime(3000);
                xrmApp.Grid.Sort("Incident Date");
                xrmApp.ThinkTime(3000);
                xrmApp.Grid.Sort("Intake Type");
                xrmApp.ThinkTime(3000);
                xrmApp.Grid.Sort("Created By");
                xrmApp.ThinkTime(3000);
                xrmApp.Grid.OpenRecord(4);

                xrmApp.ThinkTime(3000);

                xrmApp.Entity.SelectTab("Narrative");

                xrmApp.Entity.SelectTab("Related", "Criminal Conduct Screenings");

                xrmApp.ThinkTime(3000);
                xrmApp.CommandBar.ClickRelatedGridCommand("Add New Criminal Conduct Screening");

                xrmApp.ThinkTime(3000);

                xrmApp.Entity.SetValue("mcshhs_criminalconductid", Guid.NewGuid().ToString());
                xrmApp.Entity.SelectTab("Screening");
                List<string> alloptions = new List<string>();
                alloptions.Add("mcshhs_unexpecteddeath");
                alloptions.Add("mcshhs_threatenedwithweapon");
                alloptions.Add("mcshhs_sexualabuse");
                alloptions.Add("mcshhs_suspectednonaccidentalunexplainedinjuri");
                alloptions.Add("mcshhs_mechanizedinjury");
                alloptions.Add("mcshhs_abusiveconduct");
                alloptions.Add("mcshhs_unreasonableconfinement");
                alloptions.Add("mcshhs_weaponviolencewithinproximity");
                alloptions.Add("mcshhs_domesticviolencewithinproximity");
                alloptions.Add("mcshhs_drugmanufacturingwithinproximity");
                alloptions.Add("mcshhs_fleeingfromhome");
                alloptions.Add("mcshhs_aloneinvehicle");
                alloptions.Add("mcshhs_usedfordrugdistribution");
                alloptions.Add("mcshhs_illegaldrugusepermitted");
                alloptions.Add("mcshhs_drivenunderinfluence");
                alloptions.Add("mcshhs_intentionaldrowning");
                alloptions.Add("mcshhs_unrestrainedinvehicularaccident");
                alloptions.Add("mcshhs_lawenforcementfelonyreport");
                foreach (string s in alloptions)
                {
                    OptionSet eachquestion = new OptionSet();
                    eachquestion.Name = s;
                    eachquestion.Value = "Exists";
                    xrmApp.Entity.SetValue(eachquestion);
                    xrmApp.ThinkTime(1000);
                }
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);
            }
        }

        [TestMethod]
        public void TC_30588_1()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                List<string> allActionRequestintakeTypes = new List<string>();
                allActionRequestintakeTypes.Add("Action Request - Court Ordered Investigation");
                allActionRequestintakeTypes.Add("Action Request - Court Ordered Pickup");
                allActionRequestintakeTypes.Add("Action Request - Courtesy Assessment");
                allActionRequestintakeTypes.Add("Action Request - Courtesy Placement");
                allActionRequestintakeTypes.Add("Action Request - Successor of Permanent Guardian");
                foreach (string actionrequesttype in allActionRequestintakeTypes)
                {
                    xrmApp.Navigation.OpenSubArea("Intake/Hotline", "Intake");
                    xrmApp.ThinkTime(2000);
                    xrmApp.CommandBar.ClickCommand("New");
                    xrmApp.ThinkTime(10000);

                    //xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                    LookupItem mcshhs_intaketype = new LookupItem();
                    mcshhs_intaketype.Name = "mcshhs_intaketype";
                    mcshhs_intaketype.Value = actionrequesttype;
                    xrmApp.Entity.SetValue(mcshhs_intaketype);

                    xrmApp.Dialogs.ConfirmationDialog(true);

                    xrmApp.ThinkTime(2000);
                    //Validate Mandatory Fields
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true, "Worker Safety issues is not set Mandatory");

                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true, "Source Last Name is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.ThinkTime(2000);
                    Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                    //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                    xrmApp.Entity.SelectTab("General");

                    MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                    mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                    mcshhs_workersafetyissues.Values = new string[] { "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                    xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                    OptionSet mcshhs_highprioritystatus = new OptionSet();
                    mcshhs_highprioritystatus.Name = "mcshhs_highprioritystatus";
                    mcshhs_highprioritystatus.Value = "Acknowledged";
                    //xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

                    OptionSet mcshhs_priority = new OptionSet();
                    mcshhs_priority.Name = "mcshhs_priority";
                    mcshhs_priority.Value = "1";
                    xrmApp.Entity.SetValue(mcshhs_priority);

                    BooleanItem mcshhs_addsourcetointakeperson = new BooleanItem();
                    mcshhs_addsourcetointakeperson.Name = "mcshhs_addsourcetointakeperson";
                    mcshhs_addsourcetointakeperson.Value = true;
                    xrmApp.Entity.SetValue(mcshhs_addsourcetointakeperson);

                    xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcemiddlename", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString().Substring(10));
                    xrmApp.Entity.SetValue("mcshhs_sourceemployer", Guid.NewGuid().ToString().Substring(10));

                    LookupItem mcshhs_sourcetitle = new LookupItem();
                    mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                    mcshhs_sourcetitle.Value = "Nurse";
                    xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                    LookupItem mcshhs_sourcechanneltype = new LookupItem();
                    mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                    mcshhs_sourcechanneltype.Value = "Fax";
                    xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                    //LookupItem mcshhs_sourcesetting = new LookupItem();
                    //mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                    //mcshhs_sourcesetting.Value = "Hospital";
                    //xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                    LookupItem mcshhs_sourcetype = new LookupItem();
                    mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                    mcshhs_sourcetype.Value = "Doctor";
                    //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                    xrmApp.Entity.SetValue("mcshhs_sourcephone", "1234567809");
                    xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

                    xrmApp.Entity.SelectTab("Narrative");
                    xrmApp.Entity.SetValue("mcshhs_narrative", Guid.NewGuid().ToString());

                    xrmApp.Entity.SelectTab("General");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), false, "Worker Safety issues is set Mandatory");

                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), false, "Source First Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), false, "Source Middle Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), false, "Source Last Name is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), false, "Source Title is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), false, "Source Channel Type is set Mandatory");
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                    ////Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                    //xrmApp.Entity.SelectTab("Narrative");
                    //xrmApp.ThinkTime(2000);
                    //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                    xrmApp.Entity.Save();

                    xrmApp.ThinkTime(10000);


                    xrmApp.Entity.SelectTab("Additional Information");

                    xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                    xrmApp.Entity.Save();

                    xrmApp.ThinkTime(10000);


                    xrmApp.Entity.SelectTab("Incident Demographic Information");
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet1", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentstreet2", Guid.NewGuid().ToString());
                    xrmApp.Entity.SetValue("mcshhs_incidentcity", Guid.NewGuid().ToString());

                    LookupItem mcshhs_incidentstate = new LookupItem();
                    mcshhs_incidentstate.Name = "mcshhs_incidentstate";
                    mcshhs_incidentstate.Value = "Alaska";
                    xrmApp.Entity.SetValue(mcshhs_incidentstate);

                    xrmApp.Entity.SetValue("mcshhs_incidentzipcode", "52010");

                    LookupItem mcshhs_incidentcounty = new LookupItem();
                    mcshhs_incidentcounty.Name = "mcshhs_incidentcounty";
                    mcshhs_incidentcounty.Value = "Gila County";
                    xrmApp.Entity.SetValue(mcshhs_incidentcounty);


                    LookupItem mcshhs_incidentcountry = new LookupItem();
                    mcshhs_incidentcountry.Name = "mcshhs_incidentcountry";
                    mcshhs_incidentcountry.Value = "Bahrain";
                    xrmApp.Entity.SetValue(mcshhs_incidentcountry);

                    xrmApp.Entity.Save();

                    xrmApp.ThinkTime(10000);


                    xrmApp.Entity.SelectTab("Persons");

                    for (int i = 0; i < 2; i++)
                    {
                        xrmApp.ThinkTime(3000);
                        xrmApp.CommandBar.ClickSubGridCommand("Add New Intake Person");

                        xrmApp.ThinkTime(3000);

                        xrmApp.QuickCreate.SetValue("mcshhs_firstname", Guid.NewGuid().ToString());
                        xrmApp.QuickCreate.SetValue("mcshhs_lastname", Guid.NewGuid().ToString());
                        xrmApp.QuickCreate.SetValue("mcshhs_middlename", Guid.NewGuid().ToString());

                        if (i == 0)
                        {
                            BooleanItem mcshhs_primarycaretaker = new BooleanItem();
                            mcshhs_primarycaretaker.Name = "mcshhs_primarycaretaker";
                            mcshhs_primarycaretaker.Value = true;
                            xrmApp.QuickCreate.SetValue(mcshhs_primarycaretaker);
                            xrmApp.ThinkTime(3000);

                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_intakepersontype"), true, "Intake Person Type is not set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_street1"), true, "Street 1 is not set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_street2"), true, "Street 2 is not set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_city"), true, "City is not set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_state"), true, "State is not set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_crossroads"), true, "Cross Roads is not set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_county"), true, "County is not set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_country"), true, "Country is not set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_zipcode"), true, "Zip Code is not set Mandatory");



                            xrmApp.QuickCreate.SetValue("mcshhs_street1", Guid.NewGuid().ToString());
                            xrmApp.QuickCreate.SetValue("mcshhs_street2", Guid.NewGuid().ToString());
                            xrmApp.QuickCreate.SetValue("mcshhs_crossroads", Guid.NewGuid().ToString());
                            xrmApp.QuickCreate.SetValue("mcshhs_city", Guid.NewGuid().ToString());
                            xrmApp.QuickCreate.SetValue("mcshhs_employer", Guid.NewGuid().ToString());

                            LookupItem mcshhs_state = new LookupItem();
                            mcshhs_incidentstate.Name = "mcshhs_state";
                            mcshhs_incidentstate.Value = "Alaska";
                            xrmApp.QuickCreate.SetValue(mcshhs_incidentstate);

                            xrmApp.QuickCreate.SetValue("mcshhs_zipcode", "52010");

                            LookupItem mcshhs_county = new LookupItem();
                            mcshhs_incidentcounty.Name = "mcshhs_county";
                            mcshhs_incidentcounty.Value = "Gila County";
                            xrmApp.QuickCreate.SetValue(mcshhs_incidentcounty);


                            LookupItem mcshhs_country = new LookupItem();
                            mcshhs_incidentcountry.Name = "mcshhs_country";
                            mcshhs_incidentcountry.Value = "Bahrain";
                            xrmApp.QuickCreate.SetValue(mcshhs_incidentcountry);

                            LookupItem mcshhs_intakepersontype = new LookupItem();
                            mcshhs_intakepersontype.Name = "mcshhs_intakepersontype";
                            mcshhs_intakepersontype.Value = "Alleged Perpetrator";
                            xrmApp.QuickCreate.SetValue(mcshhs_intakepersontype);
                        }
                        else
                        {
                            LookupItem mcshhs_intakepersontype = new LookupItem();
                            mcshhs_intakepersontype.Name = "mcshhs_intakepersontype";
                            mcshhs_intakepersontype.Value = "Alleged Victim";
                            xrmApp.QuickCreate.SetValue(mcshhs_intakepersontype);

                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_intakepersontype"), true, "Intake Person Type is not set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_street1"), false, "Street 1 is set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_street2"), false, "Street 2 is set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_city"), false, "City is set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_state"), false, "State is set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_crossroads"), false, "Cross Roads is set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_county"), false, "County is set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_country"), false, "Country is set Mandatory");
                            Assert.AreEqual(xrmApp.QuickCreate.ValidateFieldMandatory("mcshhs_zipcode"), false, "Zip Code is set Mandatory");
                        }

                        xrmApp.QuickCreate.Save();

                        xrmApp.ThinkTime(10000);
                    }
                    xrmApp.Entity.NavigateBrowserback();
                    
                }
            }
        }
    }
}