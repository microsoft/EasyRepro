using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class AddActivity: TestsBase
    {
        [TestMethod]
        public void UCITestAccountAddAppointment()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

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

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.ClickEmailMenuItem();
                xrmApp.ThinkTime(4000);

                xrmApp.Timeline.AddEmailSubject("Request admission to butterfly section in zoo");
                xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                {
                    Name = Reference.Timeline.EmailBcc,
                    Values = new string[] { "Test Contact", "Jay Zee3" },
                });
                xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                {
                    Name = Reference.Timeline.EmailCC,
                    Values = new string[] { "Test Contact", "Jay Zee3" },
                });
                xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                {
                    Name = Reference.Timeline.EmailTo,
                    Values = new string[] { "Test Contact", "Jay Zee3" },
                });

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

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.ClickEmailMenuItem();
                xrmApp.ThinkTime(4000);

                xrmApp.Timeline.AddEmailSubject("Request admission to butterfly section in zoo");
                xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                {
                    Name = Reference.Timeline.EmailBcc,
                    Values = new string[] { "Test Contact", "Jay Zee3" },
                });
                xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                {
                    Name = Reference.Timeline.EmailCC,
                    Values = new string[] { "Test Contact", "Jay Zee3" },
                });
                xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                {
                    Name = Reference.Timeline.EmailTo,
                    Values = new string[] { "Test Contact", "Jay Zee3" },
                });

                var multiselectedItems = xrmApp.Timeline.GetEmail(
                    new MultiValueOptionSet()
                    {
                        Name = Reference.Timeline.EmailTo,
                    });

                xrmApp.ThinkTime(3000);
            }
        }

        [TestMethod]
        public void UCITestAccountRemoveMultiSelectEmail()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.ClickEmailMenuItem();
                xrmApp.ThinkTime(4000);

                xrmApp.Timeline.AddEmailSubject("Request admission to butterfly section in zoo");
                xrmApp.Timeline.AddEmailContacts(new MultiValueOptionSet()
                {
                    Name = Reference.Timeline.EmailTo,
                    Values = new string[] { "Test Contact", "Jay Zee3" },
                });

                var success = xrmApp.Timeline.RemoveEmail(
                    new MultiValueOptionSet()
                    {
                        Name = Reference.Timeline.EmailTo,
                        Values = new string[] { "Test Contact", "Jay Zee3" },
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

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Timeline.AddTask("Feed the Animals", "Make sure no one is looking while feeding", "5 minutes");

                xrmApp.Timeline.SaveAndCloseTask();

                xrmApp.ThinkTime(3000);
            }
        }
    }
}
