using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class Lab_TestsBase_CreateApp_NavigateToSameApp_InAllTests : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest(); // here is xrmApp initialized before each tests

        [TestCleanup]
        public override void FinishTest() => base.FinishTest(); // <= here get Browser closed, xrmApp get disposed, after each tests

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.Sales); // => going to Sale Hub App

        [TestCategory("Labs - TestsBase")]
        [TestMethod]
        public void UseTheBaseClass()
        {
            // _xrmApp is now an instance variable, instead local

            _xrmApp.Navigation.OpenSubArea("Sales", "Accounts"); // <= change this parameters to navigate to another area 

            Assert.IsNotNull("Replace this line with your test code");
        }

        [TestCategory("Labs - TestsBase")]
        [TestMethod]
        public void UseTheBaseClass_GoToCases_InCustomerServicesApp()
        {
            // all test are going now to the same app

            _xrmApp.Navigation.OpenSubArea("Sales", "Opportunities"); // <= navigate to another area 

            Assert.IsNotNull("Replace this line with your test code");
        }
    }
}