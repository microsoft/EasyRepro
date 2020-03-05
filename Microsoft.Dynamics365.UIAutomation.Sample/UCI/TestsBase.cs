// Enable System.Diagnostics Trace/Debug
#define DEBUG 
#define TRACE

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
        protected readonly SecureString _mfaSecretKey = ConfigurationManager.AppSettings["MfaSecretKey"]?.ToSecureString();
        protected readonly TracingService trace;
        protected XrmApp _xrmApp;
        protected WebClient _client;
      
        public TestContext TestContext { get; set; } // VSTest fulfill this property before run each test, remove for NUnit
        public  virtual string CurrentTestName => TestContext.TestName; // NUnit => replace or override this method with: TestContext.CurrentContext.Test.Name

        protected TestsBase()
        {
            trace = new TracingService(GetType(), Constants.DefaultTraceSource);
            trace.Log("Init Tracing Service - Success");
        }
        
        public virtual void InitTest()
        {
            try
            {  
                trace.Log(CurrentTestName);
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
            trace.Log(CurrentTestName);
        }

        public XrmApp CreateApp(BrowserOptions options = null)
        {
            trace.Log($"Start ({_xrmUri})");
            options = options ?? TestSettings.Options;
            SetOptions(options);

            _client = new WebClient(options);
            _xrmApp = new XrmApp(_client);

            _xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
            
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