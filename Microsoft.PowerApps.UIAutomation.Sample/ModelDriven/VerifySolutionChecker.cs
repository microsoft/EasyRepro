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
            _xrmUri = new Uri(_testContext.Properties["OnlineUrl"].ToString());
            _resultsDirectory = _testContext.Properties["ResultsDirectory"].ToString();
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
        }

        [TestMethod]
        public void TestVerifySolutionChecker()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            using (var appBrowser = new PowerAppBrowser(TestSettings.Options))
            {
                try
                {
                    //Login
                    PerformLogin(appBrowser);

                    //Pick the Org
                    Console.WriteLine($"Changing PowerApps Environment to {_environmentName}");
                    appBrowser.Navigation.ChangeEnvironment(_environmentName);
                    appBrowser.ThinkTime(1500);

                    //Click Solutions
                    Console.WriteLine($"Click Solutions via Sidebar");
                    appBrowser.SideBar.Navigate("Projects");

                    Console.WriteLine("Make sure each managed solution does not have the solution checker button in the command bar");
                    appBrowser.ModelDrivenApps.VerifyManagedSolutionsUnavailable(1000);

                    appBrowser.ThinkTime(2000);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred during Project Checker test run for solution {_solutionName}: {e}");
                    string location = $@"{_resultsDirectory}\RunSolutionChecker-{_solutionName}-GenericError.bmp";

                    appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                    _testContext.AddResultFile(location);

                    Assert.Fail($"An error occurred during Project Checker test run for solution {_solutionName}: {e}");
                }

                Console.WriteLine("Project Checker Test Run Complete");
            }
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
