using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class GoBackUCI : TestsBase
    {
        [TestMethod]
        public void UCITestOpenSubArea_GoBack()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales","Dashboards");
                xrmApp.Dashboard.SelectDashboard("Sales Dashboard"); // Check: Dashboards SubArea is Open

                xrmApp.Navigation.OpenSubArea("Accounts");
                xrmApp.Grid.SwitchView("Active Accounts"); // Check: Accounts SubArea is Open

                xrmApp.Navigation.GoBack();

                xrmApp.Dashboard.SelectDashboard("Sales Activity Dashboard"); // Check: Dashboards SubArea is Open
            }
        }

        [TestMethod]
        public void UCITestOpenRecord_GoBack()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.SwitchView("Active Accounts"); // Check: Grid is Open

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Entity.SelectForm("Account for Interactive experience");  // Check: Form is Open

                xrmApp.Navigation.GoBack();
                
                xrmApp.Grid.SwitchView("My Active Accounts"); // Check: Grid is Open
            }
        }
    }
}