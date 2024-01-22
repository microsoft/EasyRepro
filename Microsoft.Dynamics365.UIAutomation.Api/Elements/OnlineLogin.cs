// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security;
using System.Web;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OtpNet;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class OnlineLogin : Element
    {
        #region prop

        public string[] OnlineDomains { get; set; }
        public enum LoginResult
        {
            Success,
            Failure,
            Redirect
        }
        #endregion
        #region DTO
        public class LoginReference
        {
            public const string Login = "Login";
            private string _userId = "//input[@type='email']";
            private string _loginPassword = "//input[@type='password']";
            private string _signIn = "id(\"cred_sign_in_button\")";
            private string _crmMainPage = "//*[contains(@id,'crmTopBar') or contains(@data-id,'topBar')]";
            //private string _crmMainPage = "//*[contains(@data-id,'topBar')]";
            private string _staySignedIn = "//div[@data-viewid and contains(@data-bind, 'kmsi-view')]//input[@id='idSIButton9']";
            private string _oneTimeCode = "//input[@name='otc']";
            private string _useAnotherAccount = "//*[@id='otherTile']";

            public string UserId { get => _userId; set { _userId = value; } }
            public string LoginPassword { get => _loginPassword; set { _loginPassword = value; } }
            public string SignIn { get => _signIn; set { _signIn = value; } }
            //public string CrmMainPage { get => _crmMainPage; set { _crmMainPage = value; } }
            public string CrmMainPage { get => _crmMainPage; set { _crmMainPage = value; } }
            public string StaySignedIn { get => _staySignedIn; set { _staySignedIn = value; } }
            public string OneTimeCode { get => _oneTimeCode; set { _oneTimeCode = value; } }
            public string UseAnotherAccount { get => _useAnotherAccount; set { _useAnotherAccount = value; } }
        }
        #endregion
        private readonly WebClient _client;

        public OnlineLogin(WebClient client) 
        {
            _client = client;
            OnlineDomains = Constants.Xrm.XrmDomains;
        }

        #region public
        /// <summary>
        /// Logs into the organization without providing a username and password.  This login action will use pass through authentication and automatically log you in. 
        /// </summary>
        /// <param name="orgUrl">URL of the organization</param>
        public BrowserCommandResult<LoginResult> Login(Uri uri)
        {
            var username = _client.Browser.Options.Credentials.Username;
            if (username == null)
                return PassThroughLogin(uri);

            var password = _client.Browser.Options.Credentials.Password;
            //return Login(uri, username, password);

            LoginResult result = this.Login(uri, username, password);
            if (result == LoginResult.Failure)
                throw new InvalidOperationException("Login Failure, please check your configuration");

            this.InitializeModes();

            return result;
        }

        /// <summary>
        /// Logs into the organization with the user and password provided
        /// </summary>
        /// <param name="orgUrl">URL of the organization</param>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <param name="mfaSecretKey">SecretKey for multi-factor authentication</param>
        public BrowserCommandResult<LoginResult> Login(Uri orgUri, SecureString username, SecureString password, SecureString mfaSecretKey = null)
        {
            //return _client.Execute(_client.GetOptions("Login"), Login, orgUri, username, password, mfaSecretKey, redirectAction);

            LoginResult result = this.WebLogin(orgUri, username, password, mfaSecretKey);
            if (result == LoginResult.Failure)
                throw new InvalidOperationException("Login Failure, please check your configuration");

            this.InitializeModes();

            return result;
        }
        /// <summary>
        /// Logs into the organization with the user and password provided
        /// </summary>
        /// <param name="orgUrl">URL of the organization</param>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <param name="mfaSecretKey">SecretKey for multi-factor authentication</param>
        /// <param name="redirectAction">Actions required during redirect</param>
        public BrowserCommandResult<LoginResult> Login(Uri orgUrl, SecureString username, SecureString password, SecureString mfaSecretKey, Action<LoginRedirectEventArgs> redirectAction)
        {
            LoginResult result = this.WebLogin(orgUrl, username, password, mfaSecretKey, redirectAction);
            if(result == LoginResult.Failure) 
                throw new InvalidOperationException("Login Failure, please check your configuration");
            
            this.InitializeModes();

            return result;
        }

        #endregion
        #region LoginHelpers
        private LoginResult WebLogin(Uri uri, SecureString username, SecureString password, SecureString mfaSecretKey = null, Action<LoginRedirectEventArgs> redirectAction = null)
        {
            bool online = !(this.OnlineDomains != null && !this.OnlineDomains.Any(d => uri.Host.EndsWith(d)));
            //driver.Navigate().GoToUrl(uri);
            _client.Browser.Navigate(uri);
            if (!online)
                return LoginResult.Success;

            //driver.ClickIfVisible(By.Id(LoginReference.UseAnotherAccount));
            if (_client.Browser.Browser.HasElement(_client.ElementMapper.LoginReference.UseAnotherAccount))
            {
                _client.Browser.Browser.FindElement(_client.ElementMapper.LoginReference.UseAnotherAccount).Click(_client);
            }

            bool waitingForOtc = false;
            bool success = EnterUserName(username);
            if (!success)
            {
                var isUserAlreadyLogged = IsUserAlreadyLogged();
                if (isUserAlreadyLogged)
                {
                    SwitchToDefaultContent();
                    return LoginResult.Success;
                }

                _client.ThinkTime(1000);
                waitingForOtc = GetOtcInput() != null;

                if (!waitingForOtc)
                    throw new Exception($"Login page failed. {_client.ElementMapper.LoginReference.UserId} not found.");
            }

            if (!waitingForOtc)
            {
                //driver.ClickIfVisible(By.Id("aadTile"));
                _client.Browser.Browser.ClickWhenAvailable("//*[@id='aadTile']");
                _client.ThinkTime(1000);

                //If expecting redirect then wait for redirect to trigger
                if (redirectAction != null)
                {
                    //Wait for redirect to occur.
                    _client.ThinkTime(3000);

                    redirectAction.Invoke(new LoginRedirectEventArgs(username, password));
                    return LoginResult.Redirect;
                }

                EnterPassword(password);
                _client.ThinkTime(1000);
            }

            int attempts = 0;
            bool entered;
            do
            {
                entered = EnterOneTimeCode(mfaSecretKey);
                success = ClickStaySignedIn() || IsUserAlreadyLogged();
                attempts++;
            }
            while (!success && attempts <= Constants.DefaultRetryAttempts); // retry to enter the otc-code, if its fail & it is requested again 

            if (entered && !success)
                throw new InvalidOperationException("Something went wrong entering the OTC. Please check the MFA-SecretKey in configuration.");

            return success ? LoginResult.Success : LoginResult.Failure;
        }
        internal bool WaitForMainPage(TimeSpan timeout, string errorMessage)
            => WaitForMainPage(timeout, null, () => throw new InvalidOperationException(errorMessage));

        internal bool WaitForMainPage(TimeSpan? timeout = null, Action<Element> successCallback = null, Action failureCallback = null)
        {
            //IWebDriver driver = Browser;
            timeout = timeout ?? Constants.DefaultTimeout;
            bool isApp = _client.Browser.Browser.HasElement(_client.ElementMapper.LoginReference.CrmMainPage);
            if (isApp)
                _client.Browser.Browser.Wait();
            //successCallback = successCallback ?? (
            //                      _ =>
            //                      {
            //                          bool isUCI = this.Browser.Browser.HasElement(this.ElementMapper.LoginReference.CrmUCIMainPage);
            //                          if (isUCI)
            //                              driver.Wait();
            //                      });

            var xpathToMainPage = _client.ElementMapper.LoginReference.CrmMainPage;
            var element = _client.Browser.Browser.WaitUntilAvailable(xpathToMainPage, timeout.Value, "Cannot navigate to main page");
            return element != null;
        }

        private bool IsUserAlreadyLogged() => WaitForMainPage(2.Seconds());

        private static string GenerateOneTimeCode(string key)
        {
            // credits:
            // https://dev.to/j_sakamoto/selenium-testing---how-to-sign-in-to-two-factor-authentication-2joi
            // https://www.nuget.org/packages/Otp.NET/
            byte[] base32Bytes = Base32Encoding.ToBytes(key);

            var totp = new Totp(base32Bytes);
            var result = totp.ComputeTotp(); // <- got 2FA code at this time!
            return result;
        }

        private bool EnterUserName(SecureString username)
        {
            //var input = driver.WaitUntilAvailable(By.XPath(LoginReference.UserId), new TimeSpan(0, 0, 30));
            var input = _client.Browser.Browser.WaitUntilAvailable(_client.ElementMapper.LoginReference.UserId, new TimeSpan(0, 0, 30), "Cannot find User Id");
            if (input == null)
                return false;

            input.SetValue(_client, username.ToUnsecureString());
            _client.Browser.Browser.SendKey(input.Locator, Keys.Enter);
            return true;
        }

        private void EnterPassword(SecureString password)
        {
            var input = _client.Browser.Browser.FindElement(_client.ElementMapper.LoginReference.LoginPassword);
            input.SetValue(_client, password.ToUnsecureString());
            _client.Browser.Browser.SendKey(input.Locator, Keys.Enter);
        }

        private bool EnterOneTimeCode(SecureString mfaSecretKey)
        {
            try
            {
                Element input = GetOtcInput(); // wait for the dialog, even if key is null, to print the right error
                if (input == null)
                    return true;

                string key = mfaSecretKey?.ToUnsecureString(); // <- this 2FA secret key.
                if (string.IsNullOrWhiteSpace(key))
                    throw new InvalidOperationException("The application is wait for the OTC but your MFA-SecretKey is not set. Please check your configuration.");

                var oneTimeCode = GenerateOneTimeCode(key);
                Field objField = new Field(_client);
                objField.SetInputValue(_client.Browser.Browser, input, oneTimeCode, 1.Seconds());
                _client.Browser.Browser.SendKey(input.Locator, Keys.Enter);
                return true; // input found & code was entered
            }
            catch (Exception e)
            {
                var message = $"An Error occur entering OTC. Exception: {e.Message}";
                Trace.TraceInformation(message);
                throw new InvalidOperationException(message, e);
            }
        }


        private Element GetOtcInput()
            => _client.Browser.Browser.WaitUntilAvailable(_client.ElementMapper.LoginReference.OneTimeCode);

        private bool ClickStaySignedIn()
        {
            var xpath = _client.ElementMapper.LoginReference.StaySignedIn;
            var element = _client.Browser.Browser.ClickWhenAvailable(_client.ElementMapper.LoginReference.StaySignedIn, 2.Seconds(), "");
            return element != null;
        }

        private void SwitchToDefaultContent()
        {
            SwitchToMainFrame();

            //Switch Back to Default Content for Navigation Steps
            //driver.SwitchTo().DefaultContent();
            _client.Browser.SwitchToDefaultContent();
        }

        private void SwitchToMainFrame()
        {
            //driver.WaitForPageToLoad();
            //driver.SwitchTo().Frame(0);
            //driver.WaitForPageToLoad();
            _client.Browser.WaitForPageToLoad();
            _client.Browser.SwitchToFrame(0);
            _client.Browser.WaitForPageToLoad();

        }

        internal BrowserCommandResult<LoginResult> PassThroughLogin(Uri uri)
        {
            return _client.Execute(_client.GetOptions("Pass Through Login"), driver =>
            {
                driver.Navigate(uri.ToString());
                driver.Wait(PageEvent.Load);
                var isApp = driver.HasElement(_client.ElementMapper.LoginReference.CrmMainPage);
                if (isApp)
                {
                    driver.Wait(PageEvent.Load);
                    driver.Wait();
                }
                else
                    //else we landed on the Web Client main page or app picker page
                    SwitchToDefaultContent();

                //_client.WaitForMainPage(60.Seconds(),
                //    _ =>
                //    {
                //        //determine if we landed on the Unified Client Main page
                //        var isUCI = driver.HasElement(By.XPath(_client.ElementMapper.LoginReference.CrmUCIMainPage));
                //        if (isUCI)
                //        {
                //            driver.WaitForPageToLoad();
                //            driver.WaitForTransaction();
                //        }
                //        else
                //            //else we landed on the Web Client main page or app picker page
                //            SwitchToDefaultContent();
                //    },
                //    () => throw new InvalidOperationException("Load Main Page Fail.")
                //);

                return LoginResult.Success;
            });
        }


        public void ADFSLoginAction(LoginRedirectEventArgs args)

        {
            //Login Page details go here.  You will need to find out the id of the password field on the form as well as the submit button. 
            //You will also need to add a reference to the Selenium Webdriver to use the base driver. 
            //Example

            //var driver = args.Driver;

            //driver.FindElement(By.Id("passwordInput")).SendKeys(args.Password.ToUnsecureString());
            //driver.ClickWhenAvailable(By.Id("submitButton"), TimeSpan.FromSeconds(2));
            _client.Browser.Browser.FindElement("passwordInput").SendKeys(_client, new string[] { args.Password.ToUnsecureString() });
            _client.Browser.Browser.ClickWhenAvailable("submitButton", TimeSpan.FromSeconds(2));
            //Insert any additional code as required for the SSO scenario

            //Wait for CRM Page to load
            _client.Browser.Browser.Wait(TimeSpan.FromSeconds(60));
            _client.Browser.SwitchToMainFrame();
        }

        public void MSFTLoginAction(LoginRedirectEventArgs args)

        {
            //Login Page details go here.  You will need to find out the id of the password field on the form as well as the submit button. 
            //You will also need to add a reference to the Selenium Webdriver to use the base driver. 
            //Example

            //var driver = args.Driver;

            //d.FindElement(By.Id("passwordInput")).SendKeys(args.Password.ToUnsecureString());
            //d.ClickWhenAvailable(By.Id("submitButton"), TimeSpan.FromSeconds(2));

            //This method expects single sign-on

            _client.ThinkTime(5000);

            _client.Browser.Browser.WaitUntilAvailable("//div[@id=\"mfaGreetingDescription\"]");

            var azureMFA = _client.Browser.Browser.FindElement("//a[@id=\"WindowsAzureMultiFactorAuthentication\"]");
            azureMFA.Click(_client);

            Thread.Sleep(20000);

            //Insert any additional code as required for the SSO scenario

            //Wait for CRM Page to load
            _client.Browser.Browser.Wait(TimeSpan.FromSeconds(60));
            SwitchToMainFrame();
        }

        internal BrowserCommandResult<bool> InitializeModes()
        {
            return _client.Execute(_client.GetOptions("Initialize Unified Interface Modes"), driver =>
            {
                driver.SwitchToFrame("0");

                // Wait for main page to load before attempting this. If you don't do this it might still be authenticating and the URL will be wrong
                _client.Browser.Browser.Wait(PageEvent.Load);

                string uri = driver.Url;
                if (string.IsNullOrEmpty(uri))
                    return false;

                var prevQuery = GetUrlQueryParams(uri);
                bool requireRedirect = false;
                string queryParams = "";
                if (prevQuery.Get("flags") == null)
                {
                    queryParams += "&flags=easyreproautomation=true";
                    if (_client.Browser.Options.TestMode)
                        queryParams += ",testmode=true";
                    requireRedirect = true;
                }

                if (_client.Browser.Options.PerformanceMode && prevQuery.Get("perf") == null)
                {
                    queryParams += "&perf=true";
                    requireRedirect = true;
                }

                if (!requireRedirect)
                    return true;

                var testModeUri = uri + queryParams;
                driver.Navigate(testModeUri);

                // Again wait for loading
                driver.Wait(PageEvent.Load);
                return true;
            });
        }

        private NameValueCollection GetUrlQueryParams(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            Uri uri = new Uri(url);
            var query = uri.Query.ToLower();
            NameValueCollection result = HttpUtility.ParseQueryString(query);
            return result;
        }



 

        #endregion
    }
}
