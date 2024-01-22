using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class GoBack : TestsBase
    {
        [TestCategory("Navigation")]
        [TestMethod]
        public void TestOpenSubArea_GoBack()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                
                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales","Dashboards");
                xrmApp.Dashboard.SelectDashboard("Sales Dashboard"); // Check: Dashboards SubArea is Open

                xrmApp.Navigation.OpenSubArea("Accounts");
                xrmApp.Grid.SwitchView("Active Accounts"); // Check: Accounts SubArea is Open

                xrmApp.Navigation.GoBack();

                xrmApp.Dashboard.SelectDashboard("Sales Activity Dashboard"); // Check: Dashboards SubArea is Open
            }
        }

        [TestCategory("Navigation")]
        [TestMethod]
        public void TestOpenRecord_GoBack()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

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