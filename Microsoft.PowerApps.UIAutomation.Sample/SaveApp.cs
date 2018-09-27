using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class SaveApp
    {
        [TestMethod]
        public void TestSaveApp()
        {
            var username = ConfigurationManager.AppSettings["OnlineUsername"];
            var password = ConfigurationManager.AppSettings["OnlinePassword"];
            var uri = new System.Uri("https://web.powerapps.com");

            //Create a button that uses CDS to read an account out of CRM

            using (var appBrowser = new PowerAppBrowser(TestSettings.Options))
            {
                //Login
                appBrowser.OnlineLogin.Login(uri, username.ToSecureString(), password.ToSecureString());

                //Launch the App
                appBrowser.SideBar.Navigate("Apps");

                appBrowser.Apps.OpenApp("Hello World");

                //Click the Edit button
                appBrowser.Apps.Edit();

                //Skip the Welcome Window
                appBrowser.Canvas.HideStudioWelcome();

                //Click "File" Menu
                appBrowser.Canvas.ClickTab("File");

                //Click App settings
                appBrowser.Backstage.Sidebar.Navigate("App settings");

                //Update the description
                appBrowser.Backstage.AppSettings.UpdateDescription("test");

                //Save the App
                appBrowser.Backstage.Sidebar.Navigate("Save");

                appBrowser.Backstage.AppSettings.Save();
                appBrowser.Backstage.AppSettings.Publish();

            }

        }
    }
}