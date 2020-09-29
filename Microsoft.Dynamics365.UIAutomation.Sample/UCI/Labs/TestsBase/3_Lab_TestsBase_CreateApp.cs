using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class Lab_TestsBase_WorkForYou_CreateApp : TestsBase {

        [TestCategory("Labs - TestsBase")]
        [TestMethod]
        public void UseTheBaseClass()
        {
            var options = TestSettings.Options;
            options.UCIPerformanceMode = false; // <= you can also change other settings here

            using (var xrmApp = CreateApp(options)) // <= CreateApp is now calling Login for you, with your options
            {
                xrmApp.Navigation.OpenApp(UCIAppName.Sales); // <= change this parameters to navigate to another app

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts"); // <= change this parameters to navigate to another area 

                Assert.IsNotNull("Replace this line with your test code");  

            }  // Note: that here get the Browser closed, xrmApp get disposed
        }

        [TestCategory("Labs - TestsBase")]
        [TestMethod]
        public void UseTheBaseClass_GoToCases_InCustomerServicesApp()
        {
            var options = TestSettings.Options;
            using (var xrmApp = CreateApp(options))
            {
                // now all tests uses the same credentials

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService); // <= navigate to another app

                xrmApp.Navigation.OpenSubArea("Service", "Cases"); // <= navigate to another area 

                Assert.IsNotNull("Replace this line with your test code");
            }
        } 
    }
}