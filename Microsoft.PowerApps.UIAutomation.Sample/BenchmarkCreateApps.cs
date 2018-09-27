using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;


namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class BenchmarkCreateApps
    {
        private static string _username = "";
        private static string _password = "";
        private static BrowserType _browserType;
        private static System.Uri _uri;
        //private static List<App> _apps;
        private static int _createAppIterations=1;
        private static string _resultsDirectory = "";

        public TestContext TestContext { get; set; }

        private static TestContext _testContext;


        [ClassInitialize]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;

            //_apps = new List<App>();
            _username = _testContext.Properties["OnlineUsername"].ToString();
            _password = _testContext.Properties["OnlinePassword"].ToString();
            _uri = new System.Uri(_testContext.Properties["OnlineURL"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
            _resultsDirectory = _testContext.Properties["ResultsDirectory"].ToString();
            if(! int.TryParse( _testContext.Properties["CreateNewAppsSaveAppsLoopCount"]?.ToString(), out _createAppIterations))
            {
                _createAppIterations = 1;
            }
        }

        private class ExceptionTracking
        {
            public string ExceptionMessage { get; set; }

            public ExceptionTracking(string exceptionMessage)
            {
                this.ExceptionMessage = exceptionMessage;
            }

        }
        [TestMethod]
        public void BenchmarkTestCreateAppFromTemplate()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            List<ExceptionTracking> ExceptionList = new List<ExceptionTracking>();
            for (int i = 0; i < _createAppIterations; i++)
            {
                Console.WriteLine($"Running Iteration {i + 1} of {_createAppIterations} for Create Blank App");
                using (var appBrowser = new PowerAppBrowser(options))
                {
                    try
                    {
                        //Login
                        Console.WriteLine("Attempting Login");
                        for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                        {
                            try
                            {
                                appBrowser.OnlineLogin.Login(_uri, _username.ToSecureString(), _password.ToSecureString());
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



                        //Launch the App
                        Console.WriteLine("Navigate via 'Apps' in Sidebar");
                        appBrowser.SideBar.NavigateTIPApps("Apps");

                        Console.WriteLine("Create Blank App");
                        appBrowser.Apps.CreateAppFromTemplate();

                        //Skip the Welcome Window
                        Console.WriteLine("Skip the Studio Welcome window");
                        appBrowser.Canvas.HideStudioWelcome(10000);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An error occurred during save/edit: {e}");
                        string location = $@"{_resultsDirectory}\BenchmarkTestSaveApps-Save.bmp";

                        ExceptionList.Add(new ExceptionTracking(e.Message));
                        appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                        _testContext.AddResultFile(location);

                    }
                }
            }



            Console.WriteLine($"Completed creating of blank app");

            if (ExceptionList.Count > 0)
            {
                throw new Exception($"Test failed with {ExceptionList.Count} exceptions. Please review test output for more details.");
            }
        }
        [TestMethod]
        public void BenchmarkTestCreateBlankApp()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            List<ExceptionTracking> ExceptionList = new List<ExceptionTracking>();
            for (int i = 0; i < _createAppIterations; i++)
            {
                Console.WriteLine($"Running Iteration {i + 1} of {_createAppIterations} for Create Blank App");
                using (var appBrowser = new PowerAppBrowser(options))
                {
                    try
                    {
                        //Login
                        Console.WriteLine("Attempting Login");
                        for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                        {
                            try
                            {
                                appBrowser.OnlineLogin.Login(_uri, _username.ToSecureString(), _password.ToSecureString());
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




                        Console.WriteLine("Navigate via 'Apps' in Sidebar");
                        appBrowser.SideBar.NavigateTIPApps("Apps");

                        Console.WriteLine("Create Blank App");
                        appBrowser.Apps.CreateBlankApp();

                        //Skip the Welcome Window
                        Console.WriteLine("Skip the Studio Welcome window");
                        appBrowser.Canvas.HideStudioWelcome(10000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An error occurred during save/edit: {e}");
                        string location = $@"{_resultsDirectory}\BenchmarkTestSaveApps-Save.bmp";

                        ExceptionList.Add(new ExceptionTracking(e.Message));
                        appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                        _testContext.AddResultFile(location);

                    }
                }
            }



            Console.WriteLine($"Completed creating of blank app");

            if (ExceptionList.Count > 0)
            {
                throw new Exception($"Test failed with {ExceptionList.Count} exceptions. Please review test output for more details.");
            }
        }

        [TestMethod]
        public void BenchmarkTestCreateAppFromData()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            List<ExceptionTracking> ExceptionList = new List<ExceptionTracking>();
            for (int i = 0; i < _createAppIterations; i++)
            {
                Console.WriteLine($"Running Iteration {i + 1} of {_createAppIterations} for Create Blank App");
                using (var appBrowser = new PowerAppBrowser(options))
                {
                    try
                    {
                        //Login
                        Console.WriteLine("Attempting Login");
                        for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                        {
                            try
                            {
                                appBrowser.OnlineLogin.Login(_uri, _username.ToSecureString(), _password.ToSecureString());
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




                        Console.WriteLine("Navigate via 'Apps' in Sidebar");
                        appBrowser.SideBar.NavigateTIPApps("Apps");

                        Console.WriteLine("Create App From SPO List");
                        appBrowser.Apps.CreateAppFromData(_testContext.Properties["SPOnlineURL"].ToString());

                        //Skip the Welcome Window
                        Console.WriteLine("Skip the Studio Welcome window");
                        appBrowser.Canvas.HideStudioWelcome(10000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An error occurred during save/edit: {e}");
                        string location = $@"{_resultsDirectory}\BenchmarkTestSaveApps-Save.bmp";

                        ExceptionList.Add(new ExceptionTracking(e.Message));
                        appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                        _testContext.AddResultFile(location);

                    }
                }
            }



            Console.WriteLine($"Completed creating of blank app");

            if (ExceptionList.Count > 0)
            {
                throw new Exception($"Test failed with {ExceptionList.Count} exceptions. Please review test output for more details.");
            }
        }
    }
}