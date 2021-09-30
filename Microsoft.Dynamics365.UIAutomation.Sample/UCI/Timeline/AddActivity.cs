using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class AddActivity
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly SecureString _mfaSecretKey = System.Configuration.ConfigurationManager.AppSettings["MfaSecretKey"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void UCITestAccountAddAppointment()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.AddAppointment("Entry into Zoo", "Microsoft", "45 minutes", "Entertainment");

                xrmApp.Timeline.SaveAndCloseAppointment();

                xrmApp.ThinkTime(3000);
            }
        }

        [TestMethod]
        public void UCITestAccountAddEmail()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

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
        public void UCITestAccountGetMultiSelectEmail()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

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
                //    Name = Reference.Timeline.EmailTo,
                //    Values = new string[] { "Test Contact", "Jay Zee3" },
                //});

                var multiselectedItems = xrmApp.Timeline.GetEmail(
                    new MultiValueOptionSet()
                    {
                        Name = Elements.ElementId[Reference.Timeline.EmailTo],
                    });

                xrmApp.ThinkTime(3000);
            }
        }

        [TestMethod]
        [TestCategory("Fail - Bug")]
        public void UCITestAccountRemoveMultiSelectEmail()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.ClickEmailMenuItem();
                xrmApp.ThinkTime(4000);

                xrmApp.Timeline.AddEmailSubject("Request admission to butterfly section in zoo");

                xrmApp.Timeline.AddEmailContacts(CreateCcLooupItemsFor("Jim Glynn (sample)", "Nancy Anderson (sample)"));

                // This fails with the exception of OpenQA.Selenium.ElementNotInteractableException: element not interactable
                var success = xrmApp.Timeline.RemoveEmail(
                    new MultiValueOptionSet()
                    {
                        Name = Elements.ElementId[Reference.Timeline.EmailCC],
                        Values = new string[] { "Jim Glynn (sample)", "Nancy Anderson (sample)" },
                    });

                xrmApp.ThinkTime(3000);
            }
        }


        [TestMethod]
        public void UCITestAccountAddPhoneCall()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.AddPhoneCall("Call Zoo primates section", "425-425-1235", "Specifically call the primate that can handle a phone !", "15 minutes");

                xrmApp.Timeline.SaveAndClosePhoneCall();

                xrmApp.ThinkTime(3000);
            }
        }

        [TestMethod]
        public void UCITestAccountAddTask()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

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
            var lookupItemList = new List<LookupItem>();
            foreach (var lookupName in lookupNames)
            {
                lookupItemList.Add(CreateLookupItem(Reference.Timeline.EmailBcc, lookupName));
            }
            return lookupItemList.ToArray();
        }

        private LookupItem[] CreateCcLooupItemsFor(params string[] v1)
        {
            var lookupItemList = new List<LookupItem>();
            foreach (var item in v1)
            {
                lookupItemList.Add(CreateLookupItem(Reference.Timeline.EmailCC, item));
            }
            return lookupItemList.ToArray();
        }

        private LookupItem CreateLookupItem(string name, string value)
        {
            return new LookupItem
            {
                Name = Elements.ElementId[name],
                Value = value
            };
        }
    }
}
