using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class CreateUser
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestCreateUser()
        {
            // string firstName = "Test";
            // string lastName = "User";
            // string displayName = "Test User";
            // string userName = "testuser";

            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.Navigation.OpenSubArea("Settings", "Security");

                xrmBrowser.Administration.OpenFeature("Users");

                xrmBrowser.CommandBar.ClickCommand("NEW");
                
                xrmBrowser.Dialogs.AddUser();

                // Legacy O365 Code Demo to show interaction outside of Dynamics 365
                // Example Only - currently not supported
                /* 
                xrmBrowser.Office365.CreateUser(firstName, lastName, displayName, userName);
                
                xrmBrowser.Grid.Search(displayName);

                var results = xrmBrowser.Grid.GetGridItems();

                if (results.Value.Count == 0)
                {
                    throw new InvalidOperationException("User not found or was not created.");
                }
                */

            }
        }
        
    }
}
