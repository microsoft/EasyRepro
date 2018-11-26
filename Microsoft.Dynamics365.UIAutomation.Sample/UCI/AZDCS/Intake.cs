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
        public void CreateIntake_ActionRequestCourtOrderInvestigation()
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
                mcshhs_intaketype.Value = "Action Request - Court Ordered Investigation";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                xrmApp.Dialogs.ConfirmationDialog(true);

                xrmApp.ThinkTime(2000);
                //Validate Mandatory Fields
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_workersafetyissues"), true,"Worker Safety issues is not set Mandatory");

                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcefirstname"), true, "Source First Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcemiddlename"), true, "Source Middle Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcelastname"), true,"Source Last Name is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetitle"), true, "Source Title is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcechanneltype"), true, "Source Channel Type is not set Mandatory");
                //Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcesetting"), true);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_ActionRequestCourtOrderPickup()
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
                mcshhs_intaketype.Value = "Action Request - Court Ordered Pickup";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_ActionRequestCourtesyAssessment()
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
                mcshhs_intaketype.Value = "Action Request - Courtesy Assessment";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_ActionRequestCourtesyPlacement()
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
                mcshhs_intaketype.Value = "Action Request - Courtesy Placement";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_ActionRequestOther()
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
                mcshhs_intaketype.Value = "Action Request - Other";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_ActionRequestSuccesorofPermanentGuardian()
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
                mcshhs_intaketype.Value = "Action Request - Successor of Permanent Guardian";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_IntakeAddendum()
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_Alert_NotificationfromLawEnforcement()
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
                mcshhs_intaketype.Value = "Alert - Notification from Law Enforcement";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_Alert_NotificationfromotherstateChildWelfareAgency()
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
                mcshhs_intaketype.Value = "Alert - Notification from other state/Child Welfare Agency";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_Alert_OtherneededbytheField()
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
                mcshhs_intaketype.Value = "Alert - Other (needed by the Field)";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_Alert_PotentialPlacementonNonOpenCaseReport()
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
                mcshhs_intaketype.Value = "Alert - Potential Placement on Non-Open Case/Report";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_DCSHistoryRequest_ChildWelfareAgency()
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
                mcshhs_intaketype.Value = "DCS History Request - Child Welfare Agency";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_DCSHistoryRequest_LawEnforcement()
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
                mcshhs_intaketype.Value = "DCS History Request - Law Enforcement";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_EnteredinError()
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_ScreenedOutIntake()
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_LicensingIssue()
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_Report()
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_EmployeeReport()
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_CommunityInquiry()
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

                xrmApp.ThinkTime(1000);
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);


                xrmApp.Entity.SelectTab("Additional Information");

                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_schooldaycare", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_collateralcontactinformation", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_specialneeds", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_custodyvisitation", Guid.NewGuid().ToString());
                //xrmApp.Entity.SetValue("mcshhs_assistanceneededlawenforcement", Guid.NewGuid().ToString());

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_AWOL()
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
                mcshhs_intaketype.Value = "Status Communication - AWOL";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_DCSHistoryRequestLawEnforcement()
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
                mcshhs_intaketype.Value = "Status Communication - DCS History Request (Law Enforcement)";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_MedicalConsent()
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
                mcshhs_intaketype.Value = "Status Communication - Medical Consent";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_NonReport()
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
                mcshhs_intaketype.Value = "Status Communication - Non-Report";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_PickUpofCourtWard()
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
                mcshhs_intaketype.Value = "Status Communication - Pick Up of Court Ward";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_PlacementDisruption()
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
                mcshhs_intaketype.Value = "Status Communication - Placement Disruption";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_RequestforCaseManagerContact()
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
                mcshhs_intaketype.Value = "Status Communication - Request for Case Manager Contact";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_RequesttobePlacement()
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
                mcshhs_intaketype.Value = "Status Communication - Request to be Placement";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_DCSHistoryRequest_ChildWelfareAgency()
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
                mcshhs_intaketype.Value = "Status Communication-DCS History Request (Child Welfare Agency)";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_StatusCommunication_Other()
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
                mcshhs_intaketype.Value = "Status Communication-Other";
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
        public void CreateIntake_UnbornConcern()
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), true, "Source Employer is not set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), true, "Source Email is not set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), true, "Narrative is not set Mandatory");

                //xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");


                xrmApp.Entity.SelectTab("General");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                //xrmApp.Entity.SetValue(mcshhs_sourcetype);

                //xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
                //xrmApp.Entity.SetValue("mcshhs_sourceemail", Guid.NewGuid().ToString().Substring(10) + "@" + Guid.NewGuid().ToString().Substring(30) + ".com");

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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemployer"), false, "Source Employer is set Mandatory");
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourceemail"), false, "Source Email is set Mandatory");

                xrmApp.Entity.SelectTab("Narrative");
                xrmApp.ThinkTime(2000);
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_narrative"), false, "Narrative is set Mandatory");

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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), true, "Source Type is not set Mandatory");
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
                xrmApp.Entity.SetValue(mcshhs_highprioritystatus);

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
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
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
                Assert.AreEqual(xrmApp.Entity.ValidateFieldMandatory("mcshhs_sourcetype"), false, "Source Type is set Mandatory");
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

                xrmApp.Entity.SelectTab("Child Demographic Information");
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
    }
}
