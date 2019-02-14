using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Security;
using System.Threading;

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

        [TestCategory("SolutionCheckerAutomation")]
        [Priority(1)]
        [TestMethod]
        public void TestRunSolutionCheckerFromCommandBar()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            using (var appBrowser = new PowerAppBrowser(options))
            {
                try
                {
                    //Login To PowerApps
                    Console.WriteLine($"Attempting Login to {_xrmUri}");

                    for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                    {
                        try
                        {
                            appBrowser.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                            break;
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine($"Exception on Attempt #{retryCount + 1}: {exc}");

                            if (retryCount+1 == Reference.Login.SignInAttempts)
                            {
                                // Login exception occurred, take screenshot
                                _resultsDirectory = TestContext.TestResultsDirectory;
                                string location = $@"{_resultsDirectory}\RunSolutionCheckerFromCommandBar-{_solutionName}-LoginErrorAttempt{retryCount + 1}.jpeg";

                                appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                _testContext.AddResultFile(location);

                                // Max Sign-In Attempts reached
                                Console.WriteLine($"Login failed after {retryCount + 1} attempts.");
                                throw new InvalidOperationException($"Login failed after {retryCount + 1} attempts. Exception Details: {exc}");
                            }
                            else
                            {
                                // Login exception occurred, take screenshot
                                _resultsDirectory = TestContext.TestResultsDirectory;
                                string location = $@"{_resultsDirectory}\RunSolutionCheckerFromCommandBar-{_solutionName}-LoginErrorAttempt{retryCount+1}.jpeg";

                                appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                _testContext.AddResultFile(location);

                                //Navigate away and retry
                                appBrowser.Navigate("about:blank");

                                Console.WriteLine($"Login failed after attempt #{retryCount + 1}.");
                                continue;
                            }
                        }
                    }

                    Console.WriteLine("Login Complete");

                    appBrowser.ThinkTime(1500);

                    Console.WriteLine($"Changing PowerApps Environment to {_environmentName}");
                    var environmentValidation = appBrowser.Navigation.ChangeEnvironment(_environmentName).Value;

                    Assert.AreEqual(_environmentName, environmentValidation);

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
                        throw new InvalidOperationException(messageBarText);
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
                        throw new InvalidOperationException($"Unexpected Solution Check Status. Value '{solutionCheckStatus}' received instead of 'Results as of ...' ");
                    }

                    // Validate that new status is not the same as original status pre-processing
                    if (solutionCheckStatus == originalSolutionStatus)
                    {
                        Console.WriteLine($"Starting Solution Status was: {originalSolutionStatus}");
                        Console.WriteLine($"Ending Solution Status was: {solutionCheckStatus}");
                        throw new InvalidOperationException($"Unexpected Solution Check Status. Value '{solutionCheckStatus}' has not changed from '{originalSolutionStatus}'. A failure occurred and was not reported to the message bar. ");
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

                    _resultsDirectory = TestContext.TestResultsDirectory;
                    Console.WriteLine($"Current results directory location: {_resultsDirectory}");
                    string location = $@"{_resultsDirectory}\RunSolutionCheckerFromCommandBar-{_solutionName}-GenericError.jpeg";

                    appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                    _testContext.AddResultFile(location);

                    throw;
                }

                Console.WriteLine("Solution Checker Test Run Complete");
            }
        }

        [TestCategory("SolutionCheckerAutomation")]
        [Priority(1)]
        [TestMethod]
        public void TestRunSolutionCheckerFromSolutionsGrid()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            using (var appBrowser = new PowerAppBrowser(options))
            {
                try
                {
                    //Login To PowerApps
                    Console.WriteLine($"Attempting Login to {_xrmUri}");

                    for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                    {
                        try
                        {
                            appBrowser.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                            break;
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine($"Exception on Attempt #{retryCount + 1}: {exc}");

                            if (retryCount + 1 == Reference.Login.SignInAttempts)
                            {
                                // Login exception occurred, take screenshot
                                _resultsDirectory = TestContext.TestResultsDirectory;
                                string location = $@"{_resultsDirectory}\RunSolutionCheckerFromSolutionsGrid-{_solutionName}-LoginErrorAttempt{retryCount + 1}.jpeg";

                                appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                _testContext.AddResultFile(location);

                                // Max Sign-In Attempts reached
                                Console.WriteLine($"Login failed after {retryCount + 1} attempts.");
                                throw new InvalidOperationException($"Login failed after {retryCount + 1} attempts. Exception Details: {exc}");
                            }
                            else
                            {
                                // Login exception occurred, take screenshot
                                _resultsDirectory = TestContext.TestResultsDirectory;
                                string location = $@"{_resultsDirectory}\RunSolutionCheckerFromSolutionsGrid-{_solutionName}-LoginErrorAttempt{retryCount + 1}.jpeg";

                                appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                _testContext.AddResultFile(location);

                                //Navigate away and retry
                                appBrowser.Navigate("about:blank");

                                Console.WriteLine($"Login failed after attempt #{retryCount + 1}.");
                                continue;
                            }
                        }
                    }

                    Console.WriteLine("Login Complete");

                    appBrowser.ThinkTime(1500);

                    Console.WriteLine($"Changing PowerApps Environment to {_environmentName}");
                    var environmentValidation = appBrowser.Navigation.ChangeEnvironment(_environmentName).Value;

                    Assert.AreEqual(_environmentName, environmentValidation);

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
                        throw new InvalidOperationException(messageBarText);
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
                        throw new InvalidOperationException($"Unexpected Solution Check Status. Value '{solutionCheckStatus}' received instead of 'Results as of ...' ");
                    }

                    // Validate that new status is not the same as original status pre-processing
                    if (solutionCheckStatus == originalSolutionStatus)
                    {
                        Console.WriteLine($"Starting Solution Status was: {originalSolutionStatus}");
                        Console.WriteLine($"Ending Solution Status was: {solutionCheckStatus}");
                        throw new InvalidOperationException($"Unexpected Solution Check Status. Value '{solutionCheckStatus}' has not changed from '{originalSolutionStatus}'. A failure occurred and was not reported to the message bar. ");
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

                    _resultsDirectory = TestContext.TestResultsDirectory;
                    Console.WriteLine($"Current results directory location: {_resultsDirectory}");
                    string location = $@"{_resultsDirectory}\RunSolutionCheckerFromGrid-{_solutionName}-GenericError.jpeg";

                    appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                    _testContext.AddResultFile(location);

                    throw;
                }

                Console.WriteLine("Solution Checker Test Run Complete");
            }
        }

        [TestCategory("SolutionCheckerAutomation")]
        [Priority(1)]
        [TestMethod]
        public void TestVerifySolutionChecker()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            using (var appBrowser = new PowerAppBrowser(options))
            {
                try
                {
                    //Login To PowerApps
                    Console.WriteLine($"Attempting Login to {_xrmUri}");

                    for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                    {
                        try
                        {
                            appBrowser.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                            break;
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine($"Exception on Attempt #{retryCount + 1}: {exc}");

                            if (retryCount + 1 == Reference.Login.SignInAttempts)
                            {
                                // Login exception occurred, take screenshot
                                _resultsDirectory = TestContext.TestResultsDirectory;
                                string location = $@"{_resultsDirectory}\VerifySolutionChecker-{_solutionName}-LoginErrorAttempt{retryCount + 1}.jpeg";

                                appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                _testContext.AddResultFile(location);

                                // Max Sign-In Attempts reached
                                Console.WriteLine($"Login failed after {retryCount + 1} attempts.");
                                throw new InvalidOperationException($"Login failed after {retryCount + 1} attempts. Exception Details: {exc}");
                            }
                            else
                            {
                                // Login exception occurred, take screenshot
                                _resultsDirectory = TestContext.TestResultsDirectory;
                                string location = $@"{_resultsDirectory}\VerifySolutionChecker-{_solutionName}-LoginErrorAttempt{retryCount + 1}.jpeg";

                                appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                _testContext.AddResultFile(location);

                                //Navigate away and retry
                                appBrowser.Navigate("about:blank");

                                Console.WriteLine($"Login failed after attempt #{retryCount + 1}.");
                                continue;
                            }
                        }
                    }

                    Console.WriteLine("Login Complete");

                    //Pick the Org
                    Console.WriteLine($"Changing PowerApps Environment to {_environmentName}");
                    var environmentValidation = appBrowser.Navigation.ChangeEnvironment(_environmentName).Value;

                    Assert.AreEqual(_environmentName, environmentValidation);
                    appBrowser.ThinkTime(1500);

                    //Click Solutions
                    Console.WriteLine($"Click {_sideBarButton} via Sidebar");
                    appBrowser.SideBar.Navigate(_sideBarButton);

                    Console.WriteLine("Make sure each managed solution does not have the solution checker button in the command bar");
                    appBrowser.ModelDrivenApps.VerifyManagedSolutionsUnavailable(_commandBarButton, 1000);

                    appBrowser.ThinkTime(5000);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred during Solution Checker test run for solution {_solutionName}: {e}");

                    _resultsDirectory = TestContext.TestResultsDirectory;
                    Console.WriteLine($"Current results directory location: {_resultsDirectory}");
                    string location = $@"{_resultsDirectory}\VerifySolutionChecker-{_solutionName}-GenericError.jpeg";

                    appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                    _testContext.AddResultFile(location);

                    throw;
                }

                Console.WriteLine("Solution Checker Test Run Complete");
            }
        }

        [TestCategory("SolutionCheckerAutomation")]
        [Priority(2)]
        [TestMethod]
        public void TestCancelSolutionCheckerRun()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            using (var appBrowser = new PowerAppBrowser(options))
            {
                try
                {
                    //Login To PowerApps
                    Console.WriteLine($"Attempting Login to {_xrmUri}");

                    for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                    {
                        try
                        {
                            appBrowser.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                            break;
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine($"Exception on Attempt #{retryCount + 1}: {exc}");

                            if (retryCount + 1 == Reference.Login.SignInAttempts)
                            {
                                // Login exception occurred, take screenshot
                                _resultsDirectory = TestContext.TestResultsDirectory;
                                string location = $@"{_resultsDirectory}\CancelSolutionCheckerRun-{_solutionName}-LoginErrorAttempt{retryCount + 1}.jpeg";

                                appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                _testContext.AddResultFile(location);

                                // Max Sign-In Attempts reached
                                Console.WriteLine($"Login failed after {retryCount + 1} attempts.");
                                throw new InvalidOperationException($"Login failed after {retryCount + 1} attempts. Exception Details: {exc}");
                            }
                            else
                            {
                                // Login exception occurred, take screenshot
                                _resultsDirectory = TestContext.TestResultsDirectory;
                                string location = $@"{_resultsDirectory}\CancelSolutionCheckerRun-{_solutionName}-LoginErrorAttempt{retryCount + 1}.jpeg";

                                appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                _testContext.AddResultFile(location);

                                //Navigate away and retry
                                appBrowser.Navigate("about:blank");

                                Console.WriteLine($"Login failed after attempt #{retryCount + 1}.");
                                continue;
                            }
                        }
                    }

                    Console.WriteLine("Login Complete");

                    appBrowser.ThinkTime(1500);

                    Console.WriteLine($"Changing PowerApps Environment to {_environmentName}");
                    var environmentValidation = appBrowser.Navigation.ChangeEnvironment(_environmentName).Value;

                    Assert.AreEqual(_environmentName, environmentValidation);

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

                    // Check to confirm the run has not thrown an initial error
                    string messageBarText = appBrowser.ModelDrivenApps.CheckForErrors();

                    if (!string.IsNullOrEmpty(messageBarText))
                    {
                        throw new InvalidOperationException(messageBarText);
                    }

                    // Click Solution Checker running button on the right of the command bar
                    Console.WriteLine($"Cancelling Solution Checker run...");
                    var cancelSuccess = appBrowser.CommandBar.CancelSolutionCheckerRun(_solutionName);

                    // Cancel should only take a few seconds at most... wait 10 seconds and check status
                    appBrowser.ThinkTime(10000);

                    // Get solution check status post-cancel
                    string solutionCheckStatus = appBrowser.ModelDrivenApps.GetCurrentStatus(_solutionName);

                    if (appBrowser.Options.BrowserType.ToString() != "IE")
                    {
                        // Validate that we did not receive an error status
                        if (!solutionCheckStatus.Contains("Results as of"))
                        {
                            throw new InvalidOperationException($"Unexpected Solution Check Status. Value '{solutionCheckStatus}' received instead of '{originalSolutionStatus}' ");
                        }

                        // Validate that "new" status is the same as original status pre-cancellation
                        if (solutionCheckStatus != originalSolutionStatus)
                        {
                            Console.WriteLine($"Starting Solution Status was: {originalSolutionStatus}");
                            Console.WriteLine($"Ending Solution Status was: {solutionCheckStatus}");
                            throw new InvalidOperationException($"Unexpected Solution Check Status. New Status Value '{solutionCheckStatus}' has changed from original value '{originalSolutionStatus}'.");
                        }
                    }

                    appBrowser.ThinkTime(10000);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred during Solution Checker test run for solution {_solutionName}: {e}");

                    _resultsDirectory = TestContext.TestResultsDirectory;
                    Console.WriteLine($"Current results directory location: {_resultsDirectory}");
                    string location = $@"{_resultsDirectory}\CancelSolutionChecker-{_solutionName}-GenericError.jpeg";

                    appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                    _testContext.AddResultFile(location);

                    throw;
                }

                Console.WriteLine("Solution Checker Test Run Complete");
            }
        }

    }
}
