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
            _xrmUri = new System.Uri(_testContext.Properties["OnlineUrl"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());

        }

        [TestMethod]
        public void TestRunSolutionChecker()
        {
            BrowserOptions options = TestSettings.Options;
            options.BrowserType = _browserType;

            using (var appBrowser = new PowerAppBrowser(TestSettings.Options))
            {

                appBrowser.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());

                appBrowser.ThinkTime(1500);

                appBrowser.Navigation.ChangeEnvironment(_environmentName);

                appBrowser.ThinkTime(1500);

                appBrowser.SideBar.ChangeDesignMode("Model-driven");
                
                //Click Solutions
                appBrowser.SideBar.Navigate("Solutions");
                
                //Highlight Solution Name
                appBrowser.ModelDrivenApps.SelectGridRecord(_solutionName);
                
                //Click Solution Checker button
                appBrowser.CommandBar.ClickCommand("Solution Checker","Run");

                //Wait 5 seconds
                appBrowser.ThinkTime(5000);

                //Click Solution Checker button and verify the run button and "download results" buttons are grayed out.  Verify Status is running
                appBrowser.CommandBar.VerifyButtonIsClickable("Solution Checker","Run", true);
                appBrowser.CommandBar.VerifyButtonIsClickable("Solution Checker", "Download last results", true);

                //Wait for processing to complete
                appBrowser.ModelDrivenApps.WaitForProcessingToComplete(_solutionName);

                //Once processing is complete, you must select a different row, and re-select your row in order to make the buttons enabled
                appBrowser.ModelDrivenApps.SelectGridRecord("Default Solution");
                appBrowser.ModelDrivenApps.SelectGridRecord(_solutionName);

                //When status changes, verify if it succeeded or failed.  If successful, download results and verify notification is present
                appBrowser.CommandBar.DownloadResults(_solutionName);

                appBrowser.ThinkTime(5000);
            }
        }
    }
}
