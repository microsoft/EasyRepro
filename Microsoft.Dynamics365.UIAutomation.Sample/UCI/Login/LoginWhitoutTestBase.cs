using System;
using System.Configuration;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class LoginWhitoutTestBase
    {
        private readonly SecureString _username = ConfigurationManager.AppSettings["OnlineUsername"]?.ToSecureString();
        private readonly SecureString _password = ConfigurationManager.AppSettings["OnlinePassword"]?.ToSecureString();
        private readonly SecureString _mfaSecretKey = ConfigurationManager.AppSettings["MfaSecretKey"]?.ToSecureString();
        private readonly Uri _xrmUri = new Uri(ConfigurationManager.AppSettings["OnlineCrmUrl"]);

        [TestMethod]
        public void LoginFail_DontPassMfaSecretKey()
        {
            var requireMfa = ConfigurationManager.AppSettings["MfaSecretKey"] != null;
            if(!requireMfa) 
                return;
            
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                try
                {
                    xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);
                }
                catch (InvalidOperationException e)
                {
                    var errorMessage = "The application is wait for the OTC but your MFA-SecretKey is not set.";
                    Assert.IsTrue(e.Message.Contains(errorMessage));
                }
            }
        }
        
        [TestMethod]
        public void LoginSuccess_PassMfaSecretKey()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);
            }
        }
    }
}