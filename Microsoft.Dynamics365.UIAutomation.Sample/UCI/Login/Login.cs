using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class Login : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.Sales, "Sales", "Accounts");

        [TestMethod]
        public void MultiFactorLogin()
        {
            _xrmApp.Grid.SwitchView("All Accounts");

            _xrmApp.CommandBar.ClickCommand("New");

            _xrmApp.Entity.SetValue("name", "Test API Account" + TestSettings.GetRandomString(5,5) );
            _xrmApp.Entity.SetValue("telephone1", "555-555-5555");
        }
    }
}