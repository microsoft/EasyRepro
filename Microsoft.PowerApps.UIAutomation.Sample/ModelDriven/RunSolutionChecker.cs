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
        private readonly SecureString _username = ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(ConfigurationManager.AppSettings["OnlineUrl"].ToString());
        private readonly String _solutionName = "Plugins Demo";

        [TestMethod]
        public void TestRunSolutionChecker()
        {
            using (var appBrowser = new PowerAppBrowser(TestSettings.Options))
            {

                appBrowser.OnlineLogin.Login(_xrmUri, _username, _password);

                appBrowser.ThinkTime(5000);

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
            }
        }
    }
}
