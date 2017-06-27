using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
//using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Security;


namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class ADFSLogin
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void TestADFSLogin()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password, ADFSLoginAction);
            }
        }

        public void ADFSLoginAction(LoginRedirectEventArgs args)
        {
            //Login Page details go here.  You will need to find out the id of the password field on the form as well as the submit button. 
            //You will also need to add a reference to the Selenium Webdriver to use the base driver. 

            //Example
            //--------------------------------------------------------------------------------------
            //   var d = args.Driver;
            //   d.FindElement(By.Id("passwordInput")).SendKeys(args.Password.ToUnsecureString());
            //   d.ClickWhenAvailable(By.Id("submitButton"), new TimeSpan(0, 0, 2));
            //   d.WaitForPageToLoad();
            //--------------------------------------------------------------------------------------
        }
    }
}
