using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass] // NUnit =>  [TestFixture] 
    public class Lab_TestsBase_QuickStart : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest(); // here is _xrmApp initialized before each tests

        [TestCleanup]
        public override void FinishTest() => base.FinishTest(); // <= // here get Browser closed, xrmApp get disposed, after each tests

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.Sales, "Sales", "Accounts"); // => NavigateTo( App, Area, Sub Area ) in all tests
       
        // Optional: this will override the default parameters values & from  this app.config
        public override void SetOptions(BrowserOptions options) => options.UCIPerformanceMode = false; // => set options for all tests in this class

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