using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class Lab_TestsBase_CreateApp_NavigateToSameArea_InAllTests : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest(); // <= here is xrmApp initialized before each tests, Login inclusive

        [TestCleanup]
        public override void FinishTest() => base.FinishTest(); // <= here get Browser closed, xrmApp get disposed, after each tests

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.Sales, "Sales", "Accounts"); // => going to Sale Hub App, Sales Area, Accounts Sub Area

        [TestCategory("Labs - TestsBase")]
        [TestMethod]
        public void TestCreateAccount()
        {
            // You can keep your focus in what is really important, TestsBase do the rest.
            _xrmApp.CommandBar.ClickCommand("New");

            Assert.IsNotNull("Replace this line with your test code");
        }

        [TestCategory("Labs - TestsBase")]
        [TestMethod]
        public void TestOpenAccount()
        {
            _xrmApp.Grid.OpenRecord(0); 

            Assert.IsNotNull("Replace this line with your test code");
        }
    }
}