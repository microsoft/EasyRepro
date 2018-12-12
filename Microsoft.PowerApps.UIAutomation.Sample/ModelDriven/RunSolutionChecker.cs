using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Security;

namespace Microsoft.PowerApps.UIAutomation.Sample.ModelDriven
{
    [TestClass]
    public class RunSolutionChecker
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

        [TestMethod]
        public void TestRunSolutionCheckerFromCommandBar()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            using (var appBrowser = new PowerAppBrowser(TestSettings.Options))
            {
                try
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

                    Console.WriteLine($"Changing PowerApps Environment to {_environmentName}");
                    appBrowser.Navigation.ChangeEnvironment(_environmentName);

                    appBrowser.ThinkTime(1500);

                    // Click Solutions
                    Console.WriteLine($"Click {_sideBarButton} via Sidebar");
                    appBrowser.SideBar.Navigate(_sideBarButton);

                    // Collapse the sidebar
                    Console.WriteLine("Collapse the sidebar");
                    appBrowser.SideBar.ExpandCollapse();

                    // Get Solution check status value
                    string originalSolutionStatus = appBrowser.ModelDrivenApps.GetCurrentStatus(_solutionName);

                    // Highlight Solution Name
                    Console.WriteLine($"Select solution with name: {_solutionName}");
                    appBrowser.ModelDrivenApps.SelectGridRecord(_solutionName);

                    // Click Solution Checker button
                    Console.WriteLine($"Click Solution Checker button in Command Bar and then click sub-command Run");
                    appBrowser.CommandBar.ClickCommand(_commandBarButton, "Run");

                    // Wait 5 seconds
                    appBrowser.ThinkTime(15000);

                    // Check to confirm the run was successful
                    string messageBarText = appBrowser.ModelDrivenApps.CheckForErrors();

                    if (!string.IsNullOrEmpty(messageBarText))
                    {
                        throw new ApplicationException(messageBarText);
                    }

                    //Click Solution Checker button and verify the run button and "download results" buttons are grayed out.  Verify Status is running
                    Console.WriteLine($"Verifying that Run and Download Last Results buttons are disabled");
                    appBrowser.CommandBar.VerifyButtonIsClickable(_commandBarButton, "Run", true);
                    appBrowser.CommandBar.VerifyButtonIsClickable(_commandBarButton, "Download last results", true);                   

                    // Wait for processing to complete
                    Console.WriteLine($"Waiting for Solution Checker run to finish");
                    appBrowser.ModelDrivenApps.WaitForProcessingToComplete(_solutionName);

                    // Get solution check status post-processing
                    string solutionCheckStatus = appBrowser.ModelDrivenApps.GetCurrentStatus(_solutionName);

                    // Validate that we did not receive an error status
                    if (!solutionCheckStatus.Contains("Results as of"))
                    {
                        throw new ApplicationException($"Unexpected Solution Check Status. Value {solutionCheckStatus} received instead of 'Results as of ...' ");
                    }

                    // Validate that new status is not the same as original status pre-processing
                    if (solutionCheckStatus == originalSolutionStatus)
                    {
                        throw new ApplicationException($"Unexpected Solution Check Status. Value {solutionCheckStatus} has not changed from {originalSolutionStatus}. A failure occurred and was not reported to the message bar. ");
                    }

                    // Once processing is complete, you must select a different row, and re-select your row in order to make the buttons enabled
                    Console.WriteLine($"Change to Default Solution grid item, then switch back to {_solutionName} solution");
                    appBrowser.ModelDrivenApps.SelectGridRecord("Default Solution");
                    appBrowser.ModelDrivenApps.SelectGridRecord(_solutionName);

                    // When status changes, verify if it succeeded or failed.  If successful, download results and verify notification is present
                    // Due to Modal Dialog on Download in IE, skip this step
                    if (_browserType.ToString() != "IE")
                    {
                        Console.WriteLine($"Downloading Results for Solution Checker run");
                        appBrowser.CommandBar.DownloadResults(_solutionName, _commandBarButton);
                    }

                    appBrowser.ThinkTime(10000);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred during Solution Checker test run for solution {_solutionName}: {e}");
                    string location = $@"{_resultsDirectory}\RunSolutionChecker-{_solutionName}-GenericError.bmp";

                    appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                    _testContext.AddResultFile(location);

                    Assert.Fail($"An error occurred during Solution Checker test run for solution {_solutionName}: {e}");
                }

                Console.WriteLine("Solution Checker Test Run Complete");
            }
        }

        [TestMethod]
        public void TestRunSolutionCheckerFromSolutionsGrid()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            using (var appBrowser = new PowerAppBrowser(TestSettings.Options))
            {
                try
                {

                    // Login To PowerApps
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

                    Console.WriteLine($"Changing PowerApps Environment to {_environmentName}");
                    appBrowser.Navigation.ChangeEnvironment(_environmentName);

                    appBrowser.ThinkTime(1500);

                    // Click Projects
                    Console.WriteLine($"Click {_sideBarButton} via Sidebar");
                    appBrowser.SideBar.Navigate(_sideBarButton);

                    // Collapse the sidebar
                    Console.WriteLine("Collapse the sidebar");
                    appBrowser.SideBar.ExpandCollapse();

                    // Get Solution check status value
                    string originalSolutionStatus = appBrowser.ModelDrivenApps.GetCurrentStatus(_solutionName);

                    // Click desired grid row, then click Projects Checker button via ... commands in the grid
                    Console.WriteLine($"Click the ... button in Projects grid, click on 'Solution Checker', and then click sub-command 'Run'");
                    appBrowser.ModelDrivenApps.MoreCommands(_solutionName, _commandBarButton, "Run");

                    // Wait 5 seconds
                    appBrowser.ThinkTime(15000);

                    // Check to confirm the run was successful
                    string messageBarText = appBrowser.ModelDrivenApps.CheckForErrors();

                    if (!string.IsNullOrEmpty(messageBarText))
                    {
                        throw new ApplicationException(messageBarText);
                    }

                    // Click Solution Checker button and verify the run button and "download results" buttons are grayed out.  Verify Status is running
                    Console.WriteLine($"Verifying that Run and Download Last Results buttons are disabled in the grid");
                    appBrowser.ModelDrivenApps.VerifyButtonIsClickable(_solutionName , _commandBarButton, "Run", true);
                    appBrowser.ModelDrivenApps.VerifyButtonIsClickable(_solutionName, _commandBarButton, "Download last results", true);

                    // Wait for processing to complete
                    Console.WriteLine($"Waiting for Solution Checker run to finish");
                    appBrowser.ModelDrivenApps.WaitForProcessingToComplete(_solutionName);

                    // Get solution check status post-processing
                    string solutionCheckStatus = appBrowser.ModelDrivenApps.GetCurrentStatus(_solutionName);

                    // Validate that we did not receive an error status
                    if (!solutionCheckStatus.Contains("Results as of"))
                    {
                        throw new ApplicationException($"Unexpected Solution Check Status. Value {solutionCheckStatus} received instead of 'Results as of ...' ");
                    }

                    // Validate that new status is not the same as original status pre-processing
                    if (solutionCheckStatus == originalSolutionStatus)
                    {
                        throw new ApplicationException($"Unexpected Solution Check Status. Value {solutionCheckStatus} has not changed from {originalSolutionStatus}. A failure occurred and was not reported to the message bar. ");
                    }

                    // Once processing is complete, you must select a different row, and re-select your row in order to make the buttons enabled
                    Console.WriteLine($"Change to Default Solution grid item, then switch back to {_solutionName} solution");
                    appBrowser.ModelDrivenApps.SelectGridRecord("Default Solution");
                    appBrowser.ModelDrivenApps.SelectGridRecord(_solutionName);

                    // hen status changes, verify if it succeeded or failed.  If successful, download results and verify notification is present
                    // Due to Modal Dialog on Download in IE, skip this step
                    if (_browserType.ToString() != "IE")
                    {
                        Console.WriteLine($"Downloading Results for Solution Checker run");
                        appBrowser.CommandBar.DownloadResults(_solutionName, _commandBarButton);
                    }

                    appBrowser.ThinkTime(10000);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred during Solution Checker test run for solution {_solutionName}: {e}");
                    string location = $@"{_resultsDirectory}\RunSolutionChecker-{_solutionName}-GenericError.bmp";

                    appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                    _testContext.AddResultFile(location);
                 
                    Assert.Fail($"An error occurred during Solution Checker test run for solution {_solutionName}: {e}");
                }

                Console.WriteLine("Solution Checker Test Run Complete");
            }
        }

    }
}
