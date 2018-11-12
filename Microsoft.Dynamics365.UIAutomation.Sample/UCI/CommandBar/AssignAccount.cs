// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class AssignAccountUCI
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestAssignAccount()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                xrmApp.Navigation.OpenApp("UCI");

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(2000);

                xrmApp.CommandBar.ClickCommand("Assign");

                xrmApp.Dialogs.Assign(Dialogs.AssignTo.User, "Grant");

            }
        }


        [TestMethod]
        public void CreatePerson()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Person", "Persons");
                xrmApp.ThinkTime(2000);
                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.ThinkTime(2000);

                //xrmApp.Entity.SetValue("name", "Test Account Creation");
                xrmApp.Entity.SetValue("mcshhs_firstname", Guid.NewGuid().ToString().Substring(10));
                xrmApp.Entity.SetValue("mcshhs_lastname", Guid.NewGuid().ToString().Substring(20));
                xrmApp.Entity.SetValue("mcshhs_middlename", Guid.NewGuid().ToString().Substring(25));
                OptionSet suffix = new OptionSet();
                suffix.Name = "mcshhs_suffix";
                suffix.Value = "I";
                xrmApp.Entity.SetValue(suffix);
                xrmApp.Entity.SetValue("mcshhs_dateofbirth", DateTime.Now.AddYears(-31), "MM/dd/yyyy");
                xrmApp.Entity.SetValue("mcshhs_cityofbirth", "Test City");
                LookupItem mcshhs_countyofbirth = new LookupItem();
                mcshhs_countyofbirth.Name = "mcshhs_countyofbirth";
                mcshhs_countyofbirth.Value = "Mohave";
                xrmApp.Entity.SetValue(mcshhs_countyofbirth);
                LookupItem mcshhs_stateofbirth = new LookupItem();
                mcshhs_stateofbirth.Name = "mcshhs_stateofbirth";
                mcshhs_stateofbirth.Value = "Colorado";
                xrmApp.Entity.SetValue(mcshhs_stateofbirth);
                LookupItem mcshhs_countryofbirth = new LookupItem();
                mcshhs_countryofbirth.Name = "mcshhs_countryofbirth";
                mcshhs_countryofbirth.Value = "Brazil";
                xrmApp.Entity.SetValue(mcshhs_countryofbirth);
                LookupItem mcshhs_maritalstatus = new LookupItem();
                mcshhs_maritalstatus.Name = "mcshhs_maritalstatus";
                mcshhs_maritalstatus.Value = "Single";
                xrmApp.Entity.SetValue(mcshhs_maritalstatus);

                LookupItem mcshhs_title = new LookupItem();
                mcshhs_title.Name = "mcshhs_title";
                mcshhs_title.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_title);


                //xrmApp.Entity.SetValue("mcshhs_armedservicesinformation", "uaiosuiouio aipaos aspoi asipo ipoias poiasi pia yuiysiu asu uias ");


                OptionSet mcshhs_sexualorientation = new OptionSet();
                mcshhs_sexualorientation.Name = "mcshhs_sexualorientation";
                mcshhs_sexualorientation.Value = "Decline";
                xrmApp.Entity.SetValue(mcshhs_sexualorientation);

                OptionSet mcshhs_gender = new OptionSet();
                mcshhs_gender.Name = "mcshhs_gender";
                mcshhs_gender.Value = "Male";
                xrmApp.Entity.SetValue(mcshhs_gender);

                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Contact Info");
                OptionSet mcshhs_preferredmethodofcommunication = new OptionSet();
                mcshhs_preferredmethodofcommunication.Name = "mcshhs_preferredmethodofcommunication";
                mcshhs_preferredmethodofcommunication.Value = "Mail";
                xrmApp.Entity.SetValue(mcshhs_preferredmethodofcommunication);

                LookupItem mcshhs_primarylanguage = new LookupItem();
                mcshhs_primarylanguage.Name = "mcshhs_primarylanguage";
                mcshhs_primarylanguage.Value = "English";
                xrmApp.Entity.SetValue(mcshhs_primarylanguage);
                //xrmApp.Entity.Save();


                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Related Addresses");

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Tracking Information");

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Notes");

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("ID's");

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Validation");

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Related", "Person Addresses");



                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Person Information");
                //xrmApp.Entity.

            }
        }

        [TestMethod]
        public void CreateIntake()
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

                xrmApp.Entity.SetValue("mcshhs_incidentdate", DateTime.Now, "MM/dd/yyyy");

                LookupItem mcshhs_intaketype = new LookupItem();
                mcshhs_intaketype.Name = "mcshhs_intaketype";
                mcshhs_intaketype.Value = "Action Request - Courtesy Assessment";
                xrmApp.Entity.SetValue(mcshhs_intaketype);

                //MultiValueOptionSet mcshhs_workersafetyissues = new MultiValueOptionSet();
                //mcshhs_workersafetyissues.Name = "mcshhs_workersafetyissues";
                //mcshhs_workersafetyissues.Values = new string[]{ "Access to/Possession of Weapons", "Gang Affiliation", "History of Violence" };
                //xrmApp.Entity.SetValue(mcshhs_workersafetyissues);

                xrmApp.Entity.SetValue("mcshhs_highpriorityacknowledgeddatetime", DateTime.Now, "MM/dd/yyyy");

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

                xrmApp.Entity.SetValue("mcshhs_sourcefirstname", Guid.NewGuid().ToString());
                xrmApp.Entity.SetValue("mcshhs_sourcelastname", Guid.NewGuid().ToString());

                LookupItem mcshhs_sourcetitle = new LookupItem();
                mcshhs_sourcetitle.Name = "mcshhs_sourcetitle";
                mcshhs_sourcetitle.Value = "Nurse";
                xrmApp.Entity.SetValue(mcshhs_sourcetitle);

                LookupItem mcshhs_sourcechanneltype = new LookupItem();
                mcshhs_sourcechanneltype.Name = "mcshhs_sourcechanneltype";
                mcshhs_sourcechanneltype.Value = "Fax";
                xrmApp.Entity.SetValue(mcshhs_sourcechanneltype);

                LookupItem mcshhs_sourcesetting = new LookupItem();
                mcshhs_sourcesetting.Name = "mcshhs_sourcesetting";
                mcshhs_sourcesetting.Value = "Hospital";
                xrmApp.Entity.SetValue(mcshhs_sourcesetting);

                LookupItem mcshhs_sourcetype = new LookupItem();
                mcshhs_sourcetype.Name = "mcshhs_sourcetype";
                mcshhs_sourcetype.Value = "Doctor";
                xrmApp.Entity.SetValue(mcshhs_sourcetype);

                xrmApp.Entity.SetValue("mcshhs_sourcephone", "123456789");
                xrmApp.Entity.Save();

                xrmApp.ThinkTime(10000);

                xrmApp.Entity.SelectTab("Narrative");

                xrmApp.Entity.SetValue("mcshhs_narrative", "This is Narrative description");

                xrmApp.Entity.SelectTab("Family Information");

                xrmApp.Entity.SetValue("mcshhs_familycomposition", "Family Composition Value");
                xrmApp.Entity.SetValue("mcshhs_currentlocationofchild", "Curret Location of Child Value");
                xrmApp.Entity.SetValue("mcshhs_specialneeds", "Special Needs Value");

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
        public void Dashboard()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                xrmApp.ThinkTime(10000);

                xrmApp.Navigation.OpenApp("GuardianAPP");
                xrmApp.ThinkTime(2000);
                xrmApp.Navigation.OpenSubArea("Person", "Dashboards");
                xrmApp.ThinkTime(2000);

                xrmApp.Dashboard.SelectDashboard("Active Person Dashboard");
                xrmApp.ThinkTime(2000);

                xrmApp.Dashboard.SelectDashboard("Draft Intake Dashboard");
                xrmApp.ThinkTime(2000);

                xrmApp.Dashboard.SelectDashboard("Licensing Issue Intake Dashboard");
                xrmApp.ThinkTime(2000);

                xrmApp.Dashboard.SelectDashboard("Screened out Intake Dashboard");
                xrmApp.ThinkTime(2000);

                xrmApp.Dashboard.SelectDashboard("Active Person Dashboard");
                xrmApp.ThinkTime(2000);

                xrmApp.Grid.OpenRecord(1);
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
                List<GridItem>  allitems = xrmApp.Grid.GetGridItems(true);
                Console.WriteLine(allitems.Count);

                xrmApp.Grid.SwitchView("Intake - All Drafts");

                allitems =  xrmApp.Grid.GetGridItems(true);
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
            }
        }
    }
}