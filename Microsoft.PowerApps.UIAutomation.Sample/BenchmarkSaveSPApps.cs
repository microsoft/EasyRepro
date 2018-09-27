using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;


namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class BenchmarkSaveSPApps
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

                _apps.Add(new App(val[0].ToString(), val[1].ToString()));
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
            public App(string name, string id)
            {
                Name = name;
                Id = id;

            }
            public string Name = "";
            public string Id = "";
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

            public ExceptionTracking(string exceptionMessage)
            {
                this.ExceptionMessage = exceptionMessage;
            }

        }

        [TestMethod]
        public void BenchmarkTestSaveSPApps()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            List<ExceptionTracking> ExceptionList = new List<ExceptionTracking>();

            foreach (var list in _lists)
            {
                using (var appBrowser = new PowerAppBrowser(options))
                {

                    try
                    {

                        //Login to SharePoint
                        Console.WriteLine("Performing Login to SharePoint");

                        for (int retryCount = 0; retryCount < Reference.Login.SignInAttempts; retryCount++)
                        {
                            try
                            {
                                appBrowser.OnlineLogin.SharePointLogin(_uri, _username.ToSecureString(), _password.ToSecureString());
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

                        //Open the SharePoint List associated with the SPO Form
                        Console.WriteLine($"Opening SharePoint List {list.listName}");
                        appBrowser.Sharepoint.OpenSharePointList(list.listName, _featureFlag);

                        //Customize SPO Form
                        appBrowser.Sharepoint.ClickButton("PowerApps", "Customize forms", 5000);

                        //Skip the Welcome Window
                        Console.WriteLine("Skip the Studio Welcome window");
                        appBrowser.Canvas.HideStudioWelcome(10000);

                        //Edit the App
                        //appBrowser.Canvas.ClickRibbon("Fill");
                        appBrowser.Canvas.ClickButton("Fill", 5000);

                        appBrowser.ThinkTime(2000);

                        //Choose Color Theme
                        appBrowser.Canvas.ChangeTheme(5000);

                        //Wait 1s before clicking File tab/menu
                        appBrowser.ThinkTime(1000);
                                               
                        //Open File Menu
                        appBrowser.Canvas.ClickTab("File");

                        //Save the App
                        Console.WriteLine($"Attempting to Save App {list.listName}");
                        appBrowser.Backstage.Sidebar.Navigate("Save", 5000);

                        //Interact with save prompt
                        Console.WriteLine("Interact with save prompt");
                        appBrowser.Backstage.AppSettings.Save(5000);

                        //Publish the App
                        Console.WriteLine($"Attempting to Publish App to SharePoint: {list.listName}");
                        appBrowser.Backstage.AppSettings.PublishToSharePoint(5000);

                        appBrowser.ThinkTime(5000);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An error occurred during publish of {list.listName}: {e}");
                        string location = $@"{_resultsDirectory}\BenchmarkTestSaveSPApps-{list.listName}-GenericError.bmp";

                        ExceptionList.Add(new ExceptionTracking(e.Message));
                        appBrowser.TakeWindowScreenShot(location, OpenQA.Selenium.ScreenshotImageFormat.Bmp);
                        _testContext.AddResultFile(location);

                    }

                }
            }

            Console.WriteLine($"Completed publishing of {_lists.Count} apps");

            if (ExceptionList.Count > 0)
            {
                throw new Exception($"Test failed with {ExceptionList.Count} exceptions. Please review test output for more details.");
            }
        }
    }
}