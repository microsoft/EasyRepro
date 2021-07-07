using System;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class Lab_TestsBase_ReadConfigParameters : TestsBase {

        // Read Config Parameters are now defined in TestsBase everything else still the same
        [TestCategory("Labs - TestsBase")]
        [TestMethod]
        public void UseTheBaseClass()
        {
            var options = TestSettings.Options;
            options.UCIPerformanceMode = false; // <= you can also change other settings here

            var client = new WebClient(options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey); // <= this are now comming from TestsBase, but you can change it for some specific test

                xrmApp.Navigation.OpenApp(UCIAppName.Sales); // <= change this parameters to navigate to another app

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts"); // <= change this parameters to navigate to another area 

                Assert.IsNotNull("Replace this line with your test code");  
               
            } // Note: that here get the Browser closed, xrmApp get disposed
        }

        [TestCategory("Labs - TestsBase")]
        [TestMethod, ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void UseTheBaseClass_GoToCases_InCustomerServicesApp()
        {
            var options = TestSettings.Options;
            options.PrivateMode = false; // <= this test is not in private mode, ignore config

            var client = new WebClient(options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, "anton@contoso.com".ToSecureString(), "2xTanTan!".ToSecureString(), "WhereIsMySecretKey?".ToSecureString()); // <= this tests use other credentials, ignore config

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService); // <= navigate to another app

                xrmApp.Navigation.OpenSubArea("Service", "Cases"); // <= navigate to another area 

                Assert.IsNotNull("Replace this line with your test code");
            } 
        } 
    }
}