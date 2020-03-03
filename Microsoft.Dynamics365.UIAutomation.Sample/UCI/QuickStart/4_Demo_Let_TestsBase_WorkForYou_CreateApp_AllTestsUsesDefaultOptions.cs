using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class Demo_Let_TestsBase_WorkForYou_CreateApp_AllTestsUsesDefaultOptions : TestsBase {
        
        [TestMethod]
        public void UseTheBaseClass()
        {
            using (var xrmApp = CreateApp()) // <= CreateApp is now calling Login for you, with your options
            {
                xrmApp.Navigation.OpenApp(UCIAppName.Sales); // <= change this parameters to navigate to another app

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts"); // <= change this parameters to navigate to another area 

                Assert.IsNotNull("Replace this line with your test code");  

            } // Note: that here get the Browser closed, CreateApp
        }
        
        [TestMethod]
        public void UseTheBaseClass_GoToCases_InCustomerServicesApp()
        {
            var options = TestSettings.Options;
            options.PrivateMode = false; // <= this test still using my custom options

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