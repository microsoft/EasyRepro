using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class BenchmarkOpenSPOForms
    {
        private static string _username = "";
        private static string _password = "";
        private static BrowserType _browserType;
        private static System.Uri _uri;
        private static List<App> _apps;
        private static List<SPList> _lists;
        private static string _resultsDirectory = "";
        private static string _featureFlag = "";

        public TestContext TestContext { get; set; }

        private static TestContext _testContext;


        [ClassInitialize]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;

            _apps = new List<App>();
            _lists = new List<SPList>();
            _username = _testContext.Properties["OnlineUsername"].ToString();
            _password = _testContext.Properties["OnlinePassword"].ToString();
            _uri = new System.Uri(_testContext.Properties["OnlineURL"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
            _resultsDirectory = _testContext.Properties["ResultsDirectory"].ToString();
            _featureFlag = _testContext.Properties["FeatureFlag"].ToString();

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

            //lists are stored as  Name|Iterations;Name|Iterations
            string[] lists = _testContext.Properties["ListNames"].ToString().Split(';');

            foreach (string list in lists)
            {
                var val = list.Split('|');

                //default the iteration to 1 if the iteration count is not provided
                string iterTemp = val.Length < 2 ? "1" : val[1].ToString();

                int iterations = int.Parse(iterTemp);

                _lists.Add(new SPList(val[0].ToString(), iterations));
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

        private class SPList
        {
            public SPList(string listname, int listiterations)
            {
                listName = listname;
                listIterations = listiterations;
            }
            public string listName = "";
            public int listIterations = 1;
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
        public void BenchmarkTestOpenSPOForms()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            List<ExceptionTracking> ExceptionList = new List<ExceptionTracking>();

            foreach (var list in _lists)
            {
                for (int i = 0; i < list.listIterations; i++)
                {

                    Console.WriteLine($"Running Iteration {i+1} of {list.listIterations}");

                    using (var appCanvas = new PowerAppBrowser(options))
                    {
                        try
                        {
                            var parameters = new Dictionary<string, string>();

                            //Login to SharePoint
                            Console.WriteLine("Performing Login");

                            for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                            {
                                try
                                {
                                    appCanvas.OnlineLogin.SharePointLogin(_uri, _username.ToSecureString(), _password.ToSecureString());
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

                            Console.WriteLine($"Attempting Cold Load of SharePoint App for {list.listName}, List Item 1");
                            appCanvas.Sharepoint.OpenSharePointApp(list.listName, 1, _featureFlag);

                            //Wait 20 seconds for metadata write to local storage
                            appCanvas.ThinkTime(20000);

                            Console.WriteLine($"Attempting Warm Load of SharePoint App for {list.listName}, List Item 2");
                            appCanvas.Sharepoint.OpenSharePointApp(list.listName, 2, _featureFlag, true);

                            Console.WriteLine("Warm Load Complete, waiting 1 second");
                            appCanvas.ThinkTime(1000);

                            Console.WriteLine($"SPO Benchmark for {list.listName} complete");

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"An error occurred during iteration {i+1} of {list.listName}: {e}");
                            string location = $@"{_resultsDirectory}\BenchmarkTestOpenSPOFormBenchmark-{list.listName}-GenericError{i + 1}.bmp";

                            ExceptionList.Add(new ExceptionTracking(e.Message, i + 1));
                            appCanvas.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                            _testContext.AddResultFile(location);

                        }
                        
                    }
                }
            }
            Console.WriteLine("SPO Benchmark Test Complete");

            if (ExceptionList.Count > 0)
            {
                throw new Exception($"Test failed with {ExceptionList.Count} exceptions. Please review test output for more details.");
            }
        }
    }
}
