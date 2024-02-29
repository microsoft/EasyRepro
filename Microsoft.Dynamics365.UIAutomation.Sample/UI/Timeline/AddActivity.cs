using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class AddActivity : TestsBase
    {

        [TestCategory("Timeline")]
        [TestCategory("Appointment")]
        [TestMethod]
        public void TestAccountAddAppointment()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.AddAppointment("Entry into Zoo", "Microsoft", "45 minutes", "<b>testing</b> rich text editor.");
                //xrmApp.Timeline.AddAppointment("Entry into Zoo", "Microsoft", "45 minutes", "Entertainment");

                xrmApp.Timeline.SaveAndCloseAppointment();

                xrmApp.ThinkTime(3000);
            }
        }

        [TestCategory("Timeline")]
        [TestCategory("Email")]
        [TestMethod]
        public void TestAccountAddEmail()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.ClickEmailMenuItem();
                xrmApp.ThinkTime(4000);

                xrmApp.Timeline.AddEmailSubject("Request admission to butterfly section in zoo");

                xrmApp.Timeline.AddEmailContacts(CreateBccLookupItemsFor("Jim Glynn (sample)", "Nancy Anderson (sample)"));
                xrmApp.Timeline.AddEmailContacts(CreateCcLooupItemsFor("Jim Glynn (sample)", "Nancy Anderson (sample)"));


                // This fails as it already has a value.
                //xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                //{
                //    Name = Elements.ElementId[Reference.Timeline.EmailTo],
                //    Values = new string[] { "Test Contact", "Jay Zee3" },
                //});

                xrmApp.Timeline.SaveAndCloseEmail();

                xrmApp.ThinkTime(3000);
            }
        }

        [TestMethod]
        [TestCategory("Timeline")]
        [TestCategory("Email")]
        [TestCategory("RegressionTests")]
        public void TestAccountGetMultiSelectEmail()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                //xrmApp.Timeline.ClickEmailMenuItem();
                //xrmApp.ThinkTime(4000);

                //xrmApp.Timeline.AddEmailSubject("Request admission to butterfly section in zoo");
                //xrmApp.Timeline.AddEmailContacts(CreateBccLookupItemsFor("Jim Glynn (sample)", "Nancy Anderson (sample)"), true);
                //xrmApp.Timeline.AddEmailContacts(CreateCcLooupItemsFor("Jim Glynn (sample)", "Nancy Anderson (sample)"), true);

                // This fails as it already has a value.
                //xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                //{
                //    Name = Reference.Timeline.EmailTo,
                //    Values = new string[] { "Test Contact", "Jay Zee3" },
                //});
                Timeline.TimelineReference emailReference = new Timeline.TimelineReference();
                
                var multiselectedItems = xrmApp.Timeline.GetEmail(
                    new MultiValueOptionSet()
                    {
                        
                        Name = emailReference.EmailTo,
                    }); 

                xrmApp.ThinkTime(3000);
            }
        }

        [TestMethod]
        [TestCategory("Timeline")]
        [TestCategory("Email")]
        [TestCategory("RegressionTests")]
        public void TestAccountGetMultiSelectEmail_Activities()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Activities");

                //xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.CommandBar.ClickCommand("Email");
                Timeline.TimelineReference emailReference = new Timeline.TimelineReference();
                xrmApp.Entity.SetValue(new MultiValueOptionSet()
                {
                    Name = "to",
                    Values = new[] { "Nancy Anderson (sample)", "Jim Glynn (sample)" }
                });

                //xrmApp.Timeline.ClickEmailMenuItem();
                //xrmApp.ThinkTime(4000);

                //xrmApp.Timeline.AddEmailSubject("Request admission to butterfly section in zoo");
                //xrmApp.Timeline.AddEmailContacts(CreateBccLookupItemsFor("Jim Glynn (sample)", "Nancy Anderson (sample)"), true);
                //xrmApp.Timeline.AddEmailContacts(CreateCcLooupItemsFor("Jim Glynn (sample)", "Nancy Anderson (sample)"), true);

                // This fails as it already has a value.
                //xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                //{
                //    Name = Reference.Timeline.EmailTo,
                //    Values = new string[] { "Test Contact", "Jay Zee3" },
                //});
                //Timeline.TimelineReference emailReference = new Timeline.TimelineReference();

                var multiselectedItems = xrmApp.Timeline.GetEmail(
                    new MultiValueOptionSet()
                    {

                        Name = emailReference.EmailTo,
                    });

                xrmApp.ThinkTime(3000);
            }
        }

        [TestMethod]
        [TestCategory("Timeline")]
        [TestCategory("Email")]
        [TestCategory("Fail - Bug")]
        [TestCategory("RegressionTests")]
        public void TestAccountRemoveMultiSelectEmail()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                
                //using (var xrmApp2 = new XrmApp(options))
                //{

                //}
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.ClickEmailMenuItem();
                xrmApp.ThinkTime(4000);

                xrmApp.Timeline.AddEmailSubject("Request admission to butterfly section in zoo");

                xrmApp.Timeline.AddEmailContacts(CreateCcLooupItemsFor("Jim Glynn (sample)", "Nancy Anderson (sample)"), true);
                Timeline.TimelineReference emailReference = new Timeline.TimelineReference();
                var success = xrmApp.Timeline.RemoveEmail(
                    new MultiValueOptionSet()
                    {
                        Name = emailReference.EmailTo,
                        Values = new string[] { "Jim Glynn (sample)", "Nancy Anderson (sample)" },
                    });

                xrmApp.ThinkTime(3000);
            }
        }

        [TestCategory("Timeline")]
        [TestCategory("PhoneCall")]
        [TestMethod]
        public void TestAccountAddPhoneCall()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.AddPhoneCall("Call Zoo primates section", "425-425-1235", "Specifically call the primate that can handle a phone !", "15 minutes");

                xrmApp.Timeline.SaveAndClosePhoneCall();

                xrmApp.ThinkTime(3000);
            }
        }

        [TestCategory("Timeline")]
        [TestCategory("Task")]
        [TestMethod]
        public void TestAccountAddTask()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.AddTask("Feed the Animals", "Make sure no one is looking while feeding", "5 minutes");

                xrmApp.Timeline.SaveAndCloseTask();

                xrmApp.ThinkTime(3000);
            }
        }


        private LookupItem[] CreateBccLookupItemsFor(params string[] lookupNames)
        {
            Timeline.TimelineReference emailReference = new Timeline.TimelineReference();
            var lookupItemList = new List<LookupItem>();
            foreach (var lookupName in lookupNames)
            {
                lookupItemList.Add(CreateLookupItem(emailReference.EmailBcc, lookupName));
            }
            return lookupItemList.ToArray();
        }

        private LookupItem[] CreateCcLooupItemsFor(params string[] v1)
        {
            Timeline.TimelineReference emailReference = new Timeline.TimelineReference();
            var lookupItemList = new List<LookupItem>();
            foreach (var item in v1)
            {
                lookupItemList.Add(CreateLookupItem(emailReference.EmailCC, item));
            }
            return lookupItemList.ToArray();
        }

        private LookupItem CreateLookupItem(string name, string value)
        {
            return new LookupItem
            {
                Name = name,
                Value = value
            };
        }
    }
}
