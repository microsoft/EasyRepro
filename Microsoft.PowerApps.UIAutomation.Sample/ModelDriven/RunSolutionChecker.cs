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
        public void TestRunSolutionChecker()
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

                    Console.WriteLine($"Switch to Model-driven design mode");
                    appBrowser.SideBar.ChangeDesignMode("Model-driven");

                    //Click Solutions
                    Console.WriteLine($"Click Solutions via Sidebar");
                    appBrowser.SideBar.Navigate("Solutions");

                    //Collapse the sidebar
                    Console.WriteLine("Collapse the sidebar");
                    appBrowser.SideBar.ExpandCollapse();

                    //Highlight Solution Name
                    Console.WriteLine($"Select solution with name: {_solutionName}");
                    appBrowser.ModelDrivenApps.SelectGridRecord(_solutionName);

                    //Click Solution Checker button
                    Console.WriteLine($"Click Solution Checker button in Command Bar and then click sub-command Run");
                    appBrowser.CommandBar.ClickCommand("Solution Checker", "Run");

                    //Wait 5 seconds
                    appBrowser.ThinkTime(5000);

                    /* Temporarily removing Verify step due to Command Bar overlay issue with 'Solution Checker Running...' button
                    //Click Solution Checker button and verify the run button and "download results" buttons are grayed out.  Verify Status is running
                    Console.WriteLine($"Verifying that Run and Download Last Results buttons are disabled");
                    appBrowser.CommandBar.VerifyButtonIsClickable("Solution Checker", "Run", true);
                    appBrowser.CommandBar.VerifyButtonIsClickable("Solution Checker", "Download last results", true);
                    */

                    //Wait for processing to complete
                    Console.WriteLine($"Waiting for Solution Checker run to finish");
                    appBrowser.ModelDrivenApps.WaitForProcessingToComplete(_solutionName);

                    //Once processing is complete, you must select a different row, and re-select your row in order to make the buttons enabled
                    Console.WriteLine($"Change to Default Solution grid item, then switch back to {_solutionName} solution");
                    appBrowser.ModelDrivenApps.SelectGridRecord("Default Solution");
                    appBrowser.ModelDrivenApps.SelectGridRecord(_solutionName);

                    //When status changes, verify if it succeeded or failed.  If successful, download results and verify notification is present
                    Console.WriteLine($"Downloading Results for Solution Checker run");
                    appBrowser.CommandBar.DownloadResults(_solutionName);

                    appBrowser.ThinkTime(5000);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred during Solution Checker test run for solution {_solutionName}: {e}");
                    string location = $@"{_resultsDirectory}\RunSolutionChecker-{_solutionName}-GenericError.bmp";

                    appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                    _testContext.AddResultFile(location);

                }

                Console.WriteLine("Solution Checker Test Run Complete");
            }
        }
    }
}
