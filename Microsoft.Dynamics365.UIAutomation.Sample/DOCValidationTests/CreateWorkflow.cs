using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.DOCValidationTests
{
    [TestClass]
    public class CreateWorkflow
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestCreateWorkflow()
        {

            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.Navigation.OpenSubArea("Settings", "Processes");

                xrmBrowser.Processes.CreateProcess("Test Process", XrmProcessesPage.ProcessType.Workflow,"Account");

                xrmBrowser.Driver.LastWindow().Close();

                xrmBrowser.Driver.LastWindow();

                var rows = xrmBrowser.Grid.GetGridItems().Value;
                xrmBrowser.Grid.Sort("Created On");

                xrmBrowser.Grid.SelectRecord(rows.Count-1);   //Select the newly created record
                xrmBrowser.Processes.Activate();

                xrmBrowser.Grid.SelectRecord(rows.Count - 1);
                xrmBrowser.Processes.Deactivate();

                xrmBrowser.Grid.SelectRecord(rows.Count - 1);
                xrmBrowser.Processes.Delete();

            }
        }
    }
}
