using System;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Api.Extensions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using Guid = System.Guid;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web.FormJS
{
    [TestClass]
    public class CreateAccount
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        // Allow trigger to complete the set value action
        private string _enter(string value) => value + Keys.Enter;
        private string _timed(string value) => $"{value} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";
        private Uri _create(string entityType) => new Uri(_xrmUri, $"/main.aspx?etn={entityType}&pagetype=entityrecord");

        [TestMethod]
        public void CreateNewAccount_ReduceTheTime()
        {
            var options = TestSettings.Options;
            options.TimeFactor = 0.5f;
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
             
                xrmBrowser.ThinkTime(500);
                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.ThinkTime(2000);
                xrmBrowser.Grid.SwitchView("Active Accounts");

                xrmBrowser.ThinkTime(1000);
                xrmBrowser.CommandBar.ClickCommand("New");

                xrmBrowser.ThinkTime(5000);
                xrmBrowser.Entity.SetValue("name", "Test API Account");
                xrmBrowser.Entity.SetValue("telephone1", "555-555-5555");
                xrmBrowser.Entity.SetValue("creditonhold", true);

                xrmBrowser.CommandBar.ClickCommand("Save & Close");
                // Is not posible to check Save Sucess or Fail
                xrmBrowser.ThinkTime(2000);
            }
        }

        [TestMethod]
        public void CreateNewAccount_InconcluseSetValue()
        {
            var options = TestSettings.Options;
            options.TimeFactor = 0.5f;
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);
            
                xrmBrowser.ThinkTime(5000);
                xrmBrowser.Entity.SetValue("name", "Test API Account");
                var expectedPhone1 = "555-555-5555";
                xrmBrowser.Entity.SetValue("telephone1", expectedPhone1);

                xrmBrowser.ThinkTime(2000);
            
                string phone = xrmBrowser.FormJS.GetAttributeValue<string>("telephone1");
                Assert.AreEqual(expectedPhone1, phone);
             }   
        }

        [TestMethod]
        public void CreateNewAccount_ExecuteJavaScript_Generic()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.ThinkTime(2000);
                xrmBrowser.CommandBar.ClickCommand("New");

                xrmBrowser.ThinkTime(5000);
                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", name);

                xrmBrowser.ThinkTime(1000);

                string code = "var a = 1+1; return a";
                var result = xrmBrowser.Driver.ExecuteJavaScript<long>(code);
                
                Assert.AreEqual(2, result);
                xrmBrowser.ThinkTime(5000);
            }
        }
        
        [TestMethod]
        public void CreateNewAccount_ExecuteJavaScript_Xrm()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                xrmBrowser.ThinkTime(2000);
                var name = _timed($"Test Account");
                xrmBrowser.Entity.SetValue("name", _enter(name));

                xrmBrowser.ThinkTime(1000);
                
                object result = xrmBrowser.Entity.GetValue("name").Value;
                Assert.AreEqual(name, result);

                string code = @"
                    var result = Xrm.Page.getAttribute('name').getValue(); 
                    return result
                    ";
                
                xrmBrowser.Entity.SwitchToContentFrame();
                result = xrmBrowser.Driver.ExecuteJavaScript<string>(code);

                Assert.AreEqual(name, result);
                xrmBrowser.ThinkTime(5000);
            }
        }

        [TestMethod]
        public void CreateNewAccount_ExecuteJavaScript_Xrm_GetId()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                 xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);
               

                xrmBrowser.ThinkTime(2000);
                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", _enter(name));
                
                string code = @"return Xrm.Page.data.entity.getId()";
                
                xrmBrowser.Entity.SwitchToContentFrame();
                string result = xrmBrowser.Driver.ExecuteJavaScript<string>(code);

                Assert.IsNotNull(result);
                Assert.AreEqual(string.Empty, result);
                xrmBrowser.ThinkTime(5000);
            }
        }
        [TestMethod]
        public void CreateNewAccount_ExecuteJavaScript_Xrm_Using_HelperClass()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                xrmBrowser.ThinkTime(1000);
                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", _enter(name));

                xrmBrowser.ThinkTime(500);

                string result = xrmBrowser.Entity.GetValue("name");
                Assert.AreEqual(name, result);

                result = xrmBrowser.FormJS.GetAttributeValue<string>("name");
                Assert.AreEqual(name, result);
                xrmBrowser.ThinkTime(5000);
            }
        }

        [TestMethod]
        public void CreateNewAccount_CheckSave()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                xrmBrowser.ThinkTime(5000);
                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", name);

                Guid id = xrmBrowser.FormJS.GetEntityId();
                Assert.AreEqual(default(Guid), id);

                xrmBrowser.Entity.Save();
                xrmBrowser.ThinkTime(5000);

                id = xrmBrowser.FormJS.GetEntityId();
                Assert.AreNotEqual(default(Guid), id);
                Console.WriteLine(id);
            }
        }

        [TestMethod]
        public void CreateNewAccount_SetPhoneNummer()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", name);

                string expectedPhone1 = "+43 122466";
                xrmBrowser.Entity.SetValue("telephone1", _enter(expectedPhone1));
                xrmBrowser.ThinkTime(500);

                string expectedPhone2 = "+43 1223344";
                xrmBrowser.Entity.SetValue("fax", _enter(expectedPhone2));
                xrmBrowser.ThinkTime(500);

                string phone = xrmBrowser.Entity.GetValue("telephone1");
                Assert.AreEqual(expectedPhone1, phone);
                
                phone = xrmBrowser.Entity.GetValue("fax");
                Assert.AreEqual(expectedPhone2, phone);

                phone = xrmBrowser.FormJS.GetAttributeValue<string>("telephone1");
                Assert.AreEqual(expectedPhone1, phone);
                
                phone = xrmBrowser.FormJS.GetAttributeValue<string>("fax");
                Assert.AreEqual(expectedPhone2, phone);
            }
        }
        

        // Using SetValue to Override a Text field don't work
        [TestMethod]
        public void CreateNewAccount_OverridePhoneNummer_Fail()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", name);

                string expectedPhone1 = "+43 122466";
                xrmBrowser.Entity.SetValue("telephone1", _enter(expectedPhone1));
                xrmBrowser.ThinkTime(500);

                string expectedPhone2 = "+43 1223344";
                xrmBrowser.Entity.SetValue("fax", _enter(expectedPhone2));
                xrmBrowser.ThinkTime(500);

                string phone = xrmBrowser.Entity.GetValue("telephone1");
                Assert.AreEqual(expectedPhone1, phone);

                phone = xrmBrowser.Entity.GetValue("fax");
                Assert.AreEqual(expectedPhone2, phone);

                expectedPhone1 = "+43 122466 22";
                xrmBrowser.Entity.SetValue("telephone1", _enter(expectedPhone1));
                xrmBrowser.ThinkTime(500);

                expectedPhone2 = "+43 1223344 22";
                xrmBrowser.Entity.SetValue("fax", _enter(expectedPhone2));
                xrmBrowser.ThinkTime(500);

                phone = xrmBrowser.Entity.GetValue("telephone1");
                Assert.AreEqual(expectedPhone1, phone);

                phone = xrmBrowser.Entity.GetValue("fax");
                Assert.AreEqual(expectedPhone2, phone);

                phone = xrmBrowser.FormJS.GetAttributeValue<string>("telephone1");
                Assert.AreEqual(expectedPhone1, phone);

                phone = xrmBrowser.FormJS.GetAttributeValue<string>("fax");
                Assert.AreEqual(expectedPhone2, phone);
            }
        }

        // Using SetValue to Override a Text field don't work, clear the old value solve the problem
        // There is not clear function, clear the value is not easy to simulate without FormJS
        [TestMethod]
        public void CreateNewAccount_OverridePhoneNummer_Clearing()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", name);

                string expectedPhone1 = "+43 122466";
                xrmBrowser.Entity.SetValue("telephone1", _enter(expectedPhone1));
                xrmBrowser.ThinkTime(500);

                string expectedPhone2 = "+43 1223344";
                xrmBrowser.Entity.SetValue("fax", _enter(expectedPhone2));
                xrmBrowser.ThinkTime(500);

                string phone = xrmBrowser.Entity.GetValue("telephone1");
                Assert.AreEqual(expectedPhone1, phone);

                phone = xrmBrowser.Entity.GetValue("fax");
                Assert.AreEqual(expectedPhone2, phone);

                expectedPhone1 = "+43 122466 22";
                xrmBrowser.FormJS.Clear("telephone1");
                xrmBrowser.Entity.SetValue("telephone1", _enter(expectedPhone1));
                xrmBrowser.ThinkTime(500);

                expectedPhone2 = "+43 1223344 22";
                xrmBrowser.FormJS.Clear("fax");
                xrmBrowser.Entity.SetValue("fax", _enter(expectedPhone2));
                xrmBrowser.ThinkTime(500);

                phone = xrmBrowser.Entity.GetValue("telephone1");
                Assert.AreEqual(expectedPhone1, phone);

                phone = xrmBrowser.Entity.GetValue("fax");
                Assert.AreEqual(expectedPhone2, phone);

                phone = xrmBrowser.FormJS.GetAttributeValue<string>("telephone1");
                Assert.AreEqual(expectedPhone1, phone);

                phone = xrmBrowser.FormJS.GetAttributeValue<string>("fax");
                Assert.AreEqual(expectedPhone2, phone);
            }
        }

        // Please: Check that you has US Dollar & Euro Currencies in your system
        [TestMethod]
        public void CreateNewAccount_OverrideCurrency_Fail()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", name);

                var expectedCurrency = "US Dollar";
                xrmBrowser.Entity.SetLookupValue("transactioncurrencyid", expectedCurrency);

                string currency = xrmBrowser.Entity.GetLookupValue("transactioncurrencyid");
                Assert.AreEqual(expectedCurrency, currency);
            }
        }

        [TestMethod]
        public void CreateNewAccount_OverrideCurrency_Clearing()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", name);

                xrmBrowser.FormJS.Clear("transactioncurrencyid");
                var expectedCurrency = "US Dollar";
                xrmBrowser.Entity.SetLookupValue("transactioncurrencyid", expectedCurrency);
                
                string currency = xrmBrowser.Entity.GetLookupValue("transactioncurrencyid");
                Assert.AreEqual(expectedCurrency, currency);
            }
        }
        
        [TestMethod]
        public void OnChangeContry_UpdatePhonePrefix_OverrideCurrency_UsingDialog()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                xrmBrowser.ThinkTime(5000);
                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", name);
                
                xrmBrowser.FormJS.Clear("transactioncurrencyid");
                xrmBrowser.Entity.SetLookupValue("transactioncurrencyid", "Euro");

                xrmBrowser.ThinkTime(1000);

                var expectedCurrency = "US Dollar";
                xrmBrowser.OverrideLookupValue("transactioncurrencyid", expectedCurrency);

                xrmBrowser.ThinkTime(5000);

                string currency = xrmBrowser.Entity.GetValue("transactioncurrencyid");
                Assert.AreEqual(expectedCurrency, currency);
            }
        }

        // EntityExtention is a Combination of more Commands
        [TestMethod]
        public void CreateNewAccount_OverrideCurrency_JS_Clearing_Twice()
        {
            using (var xrmBrowser = new Microsoft.Dynamics365.UIAutomation.Api.Browser(TestSettings.Options))
            {
                var newAccountUrl = _create("account");
                xrmBrowser.LoginPage.Login(newAccountUrl, _username, _password);

                var name = _timed("Test Account");
                xrmBrowser.Entity.SetValue("name", name);
                var expectedCurrency = "Euro";
                xrmBrowser.FormJS.Clear("transactioncurrencyid");
                xrmBrowser.Entity.SetLookupValue("transactioncurrencyid", expectedCurrency);

                xrmBrowser.ThinkTime(1000);

                expectedCurrency = "US Dollar";
                xrmBrowser.FormJS.Clear("transactioncurrencyid");
                xrmBrowser.Entity.SetLookupValue("transactioncurrencyid", expectedCurrency);

                xrmBrowser.ThinkTime(1000);

                string currency = xrmBrowser.Entity.GetLookupValue("transactioncurrencyid");
                Assert.AreEqual(expectedCurrency, currency);
            }
        }
    }
}