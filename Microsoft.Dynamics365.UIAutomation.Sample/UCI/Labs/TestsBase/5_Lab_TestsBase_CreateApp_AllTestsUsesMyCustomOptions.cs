using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class Lab_TestsBase_CreateApp_AllTestsUsesMyCustomOptions : TestsBase
    {
        public override void SetOptions(BrowserOptions options)
        {
            // <= test in this class use my custom options
            options.PrivateMode = false;
            options.UCIPerformanceMode = false;
        }

        [TestCategory("Labs - TestsBase")]
        [TestMethod]
        public void UseTheBaseClass()
        {
            using (var xrmApp = CreateApp()) // <= CreateApp is now calling Login for you, running with your options
            {
                xrmApp.Navigation.OpenApp(UCIAppName.Sales); // <= change this parameters to navigate to another app

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts"); // <= change this parameters to navigate to another area 

                Assert.IsNotNull("Replace this line with your test code");  

            } // Note: that here get the Browser closed, CreateApp
        }

        [TestCategory("Labs - TestsBase")]
        [TestMethod]
        public void UseTheBaseClass_GoToCases_InCustomerServicesApp()
        {
            using (var xrmApp = CreateApp())
            {
                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService); // <= navigate to another app

                xrmApp.Navigation.OpenSubArea("Service", "Cases"); // <= navigate to another area 

                Assert.IsNotNull("Replace this line with your test code");
            }
        } 
    }
}