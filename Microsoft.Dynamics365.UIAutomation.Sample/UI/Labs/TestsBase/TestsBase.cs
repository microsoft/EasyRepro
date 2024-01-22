// Enable System.Diagnostics Trace/Debug
#define DEBUG 
#define TRACE

using System;
using System.Configuration;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Browser.Logs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class TestsBase
    {
        protected static Uri _xrmUri;
        protected static SecureString _username;
        protected static SecureString _password;
        protected static SecureString _mfaSecretKey;
        protected TracingService trace;
        protected XrmApp _xrmApp;
        protected WebClient _client;
      
        public TestContext TestContext { get; set; } // VSTest fulfill this property before run each test, remove for NUnit
        public  virtual string CurrentTestName => TestContext.TestName; // NUnit => replace or override this method with: TestContext.CurrentContext.Test.Name

        protected TestsBase()
        {
            trace = new TracingService(GetType(), Constants.DefaultTraceSource);
            trace.Log("Init Tracing Service - Success");
        }

        private static BrowserFramework _framework;
        private static BrowserType _browserType;
        private static string _browserVersion = "";
        private static string _driversPath = "";
        private static string _azureKey = "";
        private static string _sessionId = "";
        //public TestContext TestContext { get; set; }

        private static TestContext _testContext;

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;

            _username = _testContext.Properties["OnlineUsername"].ToString().ToSecureString();
            _password = _testContext.Properties["OnlinePassword"].ToString().ToSecureString();
            _mfaSecretKey = _testContext.Properties["OnlinePassword"].ToString().ToSecureString();
            _xrmUri = new Uri(_testContext.Properties["OnlineCrmUrl"].ToString());
            _framework = (BrowserFramework)Enum.Parse(typeof(BrowserFramework), _testContext.Properties["Framework"].ToString());
            TestSettings.Options.BrowserFramework = _framework;
            TestSettings.SharedOptions.BrowserFramework = _framework;
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
            _azureKey = _testContext.Properties["AzureKey"].ToString();
            _sessionId = _testContext.Properties["SessionId"].ToString() ?? Guid.NewGuid().ToString();
            _driversPath = _testContext.Properties["DriversPath"].ToString();
            if (!String.IsNullOrEmpty(_driversPath))
            {
                TestSettings.SharedOptions.DriversPath = _driversPath;
                TestSettings.Options.DriversPath = _driversPath;
                
            }
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