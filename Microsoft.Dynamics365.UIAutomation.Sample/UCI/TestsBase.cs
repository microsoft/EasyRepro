// Created by: Rodriguez Mustelier Angel (rodang)
// Modify On: 2020-01-23 02:51

using System;
using System.Configuration;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    public class TestsBase
    {
        protected readonly Uri _xrmUri = new Uri(ConfigurationManager.AppSettings["OnlineCrmUrl"]);
        protected readonly SecureString _username = ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        protected readonly SecureString _password = ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        protected readonly SecureString _mfaSecrectKey = ConfigurationManager.AppSettings["MfaSecrectKey"].ToSecureString();
        protected readonly bool _usePrivateMode = Convert.ToBoolean(ConfigurationManager.AppSettings["UsePrivateMode"]);

        protected XrmApp _xrmApp;
        protected string _timed(string value) => $"{value} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";

        public virtual void InitTest()
        {
            try
            {  
                CreateApp();
                NavigateToHomePage();
            }
            catch
            {
                CloseApp();
                throw;
            }
        }

        public virtual void FinishTest()
        {
            CloseApp();
        }

        public void CreateApp(bool privateMode = true)
        {
            BrowserOptions options = TestSettings.Options;
            options.PrivateMode = privateMode || _usePrivateMode;
            options.UCIPerformanceMode = false;

            var client = new WebClient(options);
            _xrmApp = new XrmApp(client);

            _xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);
            _xrmApp.Navigation.OpenApp(UCIAppName.Sales);
        }

        public void CloseApp()
        {
            _xrmApp.Dispose();
            _xrmApp = null;
        }

        public virtual void NavigateToHomePage() { }
    }
}