using System;
using System.Configuration;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class Demo_TestsClass {

        readonly Uri _xrmUri = new Uri(ConfigurationManager.AppSettings["OnlineCrmUrl"]);
        readonly SecureString _username = ConfigurationManager.AppSettings["OnlineUsername"]?.ToSecureString();
        readonly SecureString _password = ConfigurationManager.AppSettings["OnlinePassword"]?.ToSecureString();
        readonly SecureString _mfaSecrectKey = ConfigurationManager.AppSettings["MfaSecrectKey"]?.ToSecureString();
        readonly bool _usePrivateMode = Convert.ToBoolean(ConfigurationManager.AppSettings["UsePrivateMode"] ?? bool.TrueString);

        [TestMethod]
        public void ThisTestDoNotUseTheBaseClass()
        {
            var options = TestSettings.Options;
            options.PrivateMode = _usePrivateMode;
            options.UCIPerformanceMode = false; // <= you can also change other settings here, for this tests only

            var client = new WebClient(options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey); // <= You can use different credentials here, but ensure to convert this ToSecureString
                
                xrmApp.Navigation.OpenApp(UCIAppName.Sales); // <= change this parameters to navigate to another app

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts"); // <= change this parameters to navigate to another area 

                Assert.IsNotNull("Replace this line with your test code");  

            }  // Note: that here get the Browser closed, xrmApp get disposed
        }
        
        [TestMethod]
        public void ThisTestDoNotUseTheBaseClass_GoingToCases_InCustomerServicesApp()
        {
            var options = TestSettings.Options;
            options.PrivateMode = false; // <= this test is not in private mode, ignore config

            var client = new WebClient(options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, "anton@contoso.com".ToSecureString(), "2xTanTan!".ToSecureString(), "WhereIsMyKey?".ToSecureString()); // <= this tests use other credentials, ignore config

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService); // <= navigate to another app

                xrmApp.Navigation.OpenSubArea("Service", "Cases"); // <= navigate to another area 

                Assert.IsNotNull("Replace this line with your test code");  
            } 
        } 
    }
}