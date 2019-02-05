using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Security;

namespace Microsoft.PowerApps.UIAutomation.Sample.ModelDriven
{
    [TestClass]
    public class VerifySolutionChecker
    {
        private static string _username = "";
        private static string _password = "";
        private static BrowserType _browserType;
        private static Uri _xrmUri;
        private static string _solutionName = "";
        private static string _environmentName = "";
        private static string _sideBarButton = "";
        private static string _commandBarButton = "";
        private static string _resultsDirectory = "";

        public TestContext TestContext { get; set; }

        private static TestContext _testContext;

        [ClassInitialize]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;

            _username = _testContext.Properties["OnlineUsername"].ToString();
            _password = _testContext.Properties["OnlinePassword"].ToString();
            _solutionName = _testContext.Properties["SolutionName"].ToString();
            _environmentName = _testContext.Properties["EnvironmentName"].ToString();
            _sideBarButton = _testContext.Properties["SideBarButton"].ToString();
            _commandBarButton = _testContext.Properties["CommandBarButton"].ToString();
            _xrmUri = new Uri(_testContext.Properties["OnlineUrl"].ToString());
            _resultsDirectory = _testContext.Properties["ResultsDirectory"].ToString();
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
        }

        private void PerformLogin(PowerAppBrowser appBrowser)
        {
            //Login To PowerApps
            Console.WriteLine("Performing Login");

            for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
            {
                try
                {
                    appBrowser.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                    break;
                }
                catch (Exception)
                {
                    appBrowser.Navigate("about:blank");
                    if (retryCount == Reference.Login.SignInAttempts - 1)
                    {
                        Console.WriteLine("Login failed after {0} attempts.", retryCount);
                        throw;
                    }
                    Console.WriteLine("Login failed in #{0} attempt.", retryCount);
                    continue;
                }
            }
            Console.WriteLine("Login Complete");

            appBrowser.ThinkTime(1500);
        }
    }
}
