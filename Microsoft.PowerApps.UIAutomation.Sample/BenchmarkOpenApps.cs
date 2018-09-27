using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class BenchmarkOpenApps
    {
        private static string _username = "";
        private static string _password = "";
        private static BrowserType _browserType;
        private static System.Uri _uri;
        private static List<App> _apps;
        private static string _resultsDirectory = "";

        public TestContext TestContext { get; set; }

        private static TestContext _testContext;


        [ClassInitialize]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;

            _apps = new List<App>();
            _username = _testContext.Properties["OnlineUsername"].ToString();
            _password = _testContext.Properties["OnlinePassword"].ToString();
            _uri = new System.Uri(_testContext.Properties["OnlineURL"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
            _resultsDirectory = _testContext.Properties["ResultsDirectory"].ToString();
            
            //appIds are stored as  Name|Id|Iterations;Name|Id|Iterations
            string[] apps = _testContext.Properties["AppIds"].ToString().Split(';');

            foreach (string app in apps)
            {
                var val = app.Split('|');
                               
                //default the iteration to 1 if the iteration count is not provided
                string iterTemp = val.Length < 3 ? "1" : val[2].ToString();

                int iterations = int.Parse(iterTemp);

                _apps.Add(new App(val[0].ToString(), val[1].ToString(), iterations));
            }
        }

        private class App
        {
            public App(string name, string id, int iterations)
            {
                Name = name;
                Id = id;
                Iterations = iterations;
            }
            public string Name = "";
            public string Id = "";
            public int Iterations = 1;
        }

        private class ExceptionTracking
        {
            public string ExceptionMessage { get; set; }
            public int interation { get; set; }

            public ExceptionTracking(string exceptionMessage, int iterationNumber)
            {
                this.ExceptionMessage = exceptionMessage;
                this.interation = iterationNumber;
            }

        }


        [TestMethod]
        public void BenchmarkTestOpenApps()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;
            List<ExceptionTracking> ExceptionList = new List<ExceptionTracking>();

            foreach (var app in _apps)
            {
                for (int i = 0; i < app.Iterations; i++)
                {

                    Console.WriteLine($"Running Iteration {i+1} of {app.Iterations}");

                    using (var appCanvas = new PowerAppBrowser(options))
                    {
                        try
                        {
                            var parameters = new Dictionary<string, string>();

                            //Login to PowerApps
                            Console.WriteLine("Performing Login");

                            for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                            {
                                try
                                {
                                    appCanvas.OnlineLogin.Login(_uri, _username.ToSecureString(), _password.ToSecureString());
                                    break;
                                }
                                catch (Exception)
                                {
                                    appCanvas.Navigate("about:blank");
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
                 
                            Console.WriteLine("Adding URL parameter for cold load");
                            parameters.Add("loadtype", "cold");
                            parameters.Add("origin", "performancebenchmark");

                            Console.WriteLine($"Attempting to open {app.Name} as cold load ");
                            
                            //Cold Load
                            appCanvas.Player.OpenApp(app.Id, parameters);

                            Console.WriteLine("App opened, waiting 20 seconds to write app metadata to local storage");
                            //MUST WAIT AT LEAST 20 SECONDS TO ENSURE APP METADATA IS WRITTEN TO LOCAL STORAGE
                            appCanvas.ThinkTime(20000);

                            Console.WriteLine("Add URL parameter for warm load");
                            parameters["loadtype"] = "warm";

                            Console.WriteLine($"Open app {app.Name} as warm load");
                            //Warm Load
                            appCanvas.Player.OpenApp(app.Id, parameters, true);

                            Console.WriteLine("Warm Load Complete");


                            Console.WriteLine($"App Benchmark for {app.Name} complete");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"An error occurred during iteration {i+1} of {app.Name}: {e}");
                            string location = $@"{_resultsDirectory}\BenchmarkTestOpenApps-{app.Name}-GenericError{i + 1}.bmp";

                            ExceptionList.Add(new ExceptionTracking(e.Message, i + 1));
                            appCanvas.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                            _testContext.AddResultFile(location);

                        }
                    }
                }
            }
            Console.WriteLine("Benchmark Test Complete");

            if (ExceptionList.Count > 0)
            {
                throw new Exception($"Test failed with {ExceptionList.Count} exceptions. Please review test output for more details.");
            }
        }
    }
}
