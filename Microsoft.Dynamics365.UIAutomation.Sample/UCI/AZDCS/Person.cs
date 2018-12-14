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
    public class Person
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

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

                MultiValueOptionSet mcshhs_race = new MultiValueOptionSet();
                mcshhs_race.Name = "mcshhs_race";
                mcshhs_race.Values = new string[]{ "American Indian or Alaska Native"};
                xrmApp.Entity.SetValue(mcshhs_race);
                //xrmApp.Entity.SetValue("mcshhs_armedservicesinformation", "uaiosuiouio aipaos aspoi asipo ipoias poiasi pia yuiysiu asu uias ");

                OptionSet mcshhs_hispanicorlatinoorigin = new OptionSet();
                mcshhs_hispanicorlatinoorigin.Name = "mcshhs_hispanicorlatinoorigin";
                mcshhs_hispanicorlatinoorigin.Value = "No";
                xrmApp.Entity.SetValue(mcshhs_hispanicorlatinoorigin);

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
                xrmApp.CommandBar.ClickSubGridCommand("Add New Person Address");
                xrmApp.ThinkTime(10000);

                LookupItem mcshhs_addressstoreid = new LookupItem();
                mcshhs_addressstoreid.Name = "mcshhs_addressstoreid";
                mcshhs_addressstoreid.Value = "3555 S Perry Park Rd";
                xrmApp.QuickCreate.SetValue(mcshhs_addressstoreid);

                MultiValueOptionSet mcshhs_addresstype = new MultiValueOptionSet();
                mcshhs_addresstype.Name = "mcshhs_addresstype";
                mcshhs_addresstype.Values = new string[]{ "Home"};
                xrmApp.QuickCreate.SetValue(mcshhs_addresstype);

                xrmApp.QuickCreate.SetValue("mcshhs_addressstartdate", DateTime.Now, "MM/dd/yyyy");

                xrmApp.QuickCreate.Save();

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Tracking Information");

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Notes");

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("ID's");

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Validation");

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Related", "Person Substances");
                xrmApp.CommandBar.ClickRelatedGridCommand("Add New Person Substance");

                LookupItem mcshhs_substance = new LookupItem();
                mcshhs_substance.Name = "mcshhs_substance";
                mcshhs_substance.Value = "Alcohol";
                xrmApp.QuickCreate.SetValue(mcshhs_substance);

                xrmApp.QuickCreate.SetValue("mcshhs_datetested", DateTime.Now, "MM/dd/yyyy");

                OptionSet mcshhs_typeoftest = new OptionSet();
                mcshhs_typeoftest.Name = "mcshhs_typeoftest";
                mcshhs_typeoftest.Value = "Urine";
                xrmApp.QuickCreate.SetValue(mcshhs_typeoftest);

                xrmApp.QuickCreate.Save();

                xrmApp.ThinkTime(10000);
                xrmApp.Entity.SelectTab("Person Information");
                //xrmApp.Entity.

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
    }
}
