using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;


namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class BenchmarkSaveApps
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

                _apps.Add(new App(val[0].ToString(), val[1].ToString()));
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

        private class ExceptionTracking
        {
            public string ExceptionMessage { get; set; }

            public ExceptionTracking(string exceptionMessage)
            {
                this.ExceptionMessage = exceptionMessage;
            }

        }

        [TestMethod]
        public void BenchmarkTestSaveApps()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            List<ExceptionTracking> ExceptionList = new List<ExceptionTracking>();

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


                    foreach (var app in _apps)
                    {
                        //Launch the App
                        Console.WriteLine("Navigate via 'Apps' in Sidebar");
                        appBrowser.SideBar.NavigateTIPApps("Apps");

                        Console.WriteLine($"Open app {app.Name}");
                        appBrowser.Apps.OpenApp(app.Name);

                        //Click the Edit button
                        Console.WriteLine($"Click the Edit button for {app.Name}");
                        appBrowser.Apps.Edit();

                        //Skip the Welcome Window
                        Console.WriteLine("Skip the Studio Welcome window");
                        appBrowser.Canvas.HideStudioWelcome(10000);

                        //Click "File" Menu
                        Console.WriteLine("Click the File tab in the canvas");
                        appBrowser.Canvas.ClickTab("File");

                        //Click App settings
                        Console.WriteLine("Click App Settings via Menu");
                        appBrowser.Backstage.Sidebar.Navigate("App settings");

                        //Update the description
                        Console.WriteLine($"Updating the description for app {app.Name}");
                        appBrowser.Backstage.AppSettings.UpdateDescription("test");

                        //Save the App
                        Console.WriteLine($"Attempting to Save App {app.Name}");
                        appBrowser.Backstage.Sidebar.Navigate("Save");

                        //Interact with save prompt
                        Console.WriteLine("Interact with save prompt");
                        appBrowser.Backstage.AppSettings.Save();

                        //Publish the App
                        Console.WriteLine($"Attempting to Publish App {app.Name}");
                        appBrowser.Backstage.AppSettings.Publish();

                        appBrowser.ThinkTime(5000);
                        //TODO
                        //Navigate back to the starting point for Apps
                        //appBrowser.Backstage.Sidebar.Navigate("Connections");

                        //Close the Studio Editor
                        appBrowser.Backstage.Sidebar.Navigate("Close");

                        appBrowser.Driver.Close();
                        appBrowser.Driver.LastWindow();

                        appBrowser.ThinkTime(2000);

                        //Go Back to Home
                        appBrowser.Navigation.Homepage();
                        appBrowser.ThinkTime(2000);

                    }

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

            Console.WriteLine($"Completed publishing of {_apps.Count} apps");

            if (ExceptionList.Count > 0)
            {
                throw new Exception($"Test failed with {ExceptionList.Count} exceptions. Please review test output for more details.");
            }
        }
    }
}