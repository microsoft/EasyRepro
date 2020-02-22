using System;
using System.Configuration;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Browser.Logs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    public class TestsBase
    {
        protected readonly Uri _xrmUri = new Uri(ConfigurationManager.AppSettings["OnlineCrmUrl"]);
        protected readonly SecureString _username = ConfigurationManager.AppSettings["OnlineUsername"]?.ToSecureString();
        protected readonly SecureString _password = ConfigurationManager.AppSettings["OnlinePassword"]?.ToSecureString();
        protected readonly SecureString _mfaSecrectKey = ConfigurationManager.AppSettings["MfaSecrectKey"]?.ToSecureString();
  
        private TracingService _trace;
        protected TracingService trace => _trace ?? (_trace = new TracingService(GetType(), "BrowserAutomation"));

        protected XrmApp _xrmApp;
        protected WebClient _client;
      
        public TestContext TestContext { get; set; }

        private static TestContext _testContext;

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }

        public virtual void InitTest()
        {
            try
            {  
                trace.Log(TestContext.TestName);
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
            trace.Log(TestContext.TestName);
        }

        public XrmApp CreateApp(BrowserOptions options = null)
        {
            trace.Log($"Start ({_xrmUri})");
            options = options ?? TestSettings.Options;
            SetOptions(options);

            _client = new WebClient(options);
            _xrmApp = new XrmApp(_client);

            _xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);
            
            trace.Log("Success");
            return _xrmApp;
        }

        public void CloseApp()
        {
            _xrmApp?.Dispose();
            _xrmApp = null;
            _client = null;
            trace.Log("Success");
        }
        
        public virtual void SetOptions(BrowserOptions options) { }

        public virtual void NavigateToHomePage() {}
        
        public virtual void NavigateTo(string appName, string area = null, string subarea = null)
        {
            trace.Log($"(App: {appName.Format()}, Area: {area.Format()}, SubArea: {subarea.Format()})");
            _xrmApp.Navigation.OpenApp(appName);

            var hasArea = !string.IsNullOrWhiteSpace(area);
            var hasSubArea = !string.IsNullOrWhiteSpace(subarea);
            if(hasArea && hasSubArea)
                _xrmApp.Navigation.OpenSubArea(area, subarea);
            else if(hasArea)
                _xrmApp.Navigation.OpenArea(area);
            else if(hasSubArea)
                _xrmApp.Navigation.OpenSubArea(subarea);
        }
    }
}