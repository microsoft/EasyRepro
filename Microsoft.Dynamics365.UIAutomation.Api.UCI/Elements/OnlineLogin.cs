﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System;
using System.Diagnostics;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OtpNet;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class OnlineLogin : Element
    {
        #region prop
        public enum LoginResult
        {
            Success,
            Failure,
            Redirect
        }
        #endregion
        #region DTO
        public static class LoginReference
        {
            public static string UserId = "//input[@type='email']";
            public static string LoginPassword = "//input[@type='password']";
            public static string SignIn = "id(\"cred_sign_in_button\")";
            public static string CrmMainPage = "//*[contains(@id,'crmTopBar') or contains(@data-id,'topBar')]";
            public static string CrmUCIMainPage = "//*[contains(@data-id,'topBar')]";
            public static string StaySignedIn = "//div[@data-viewid and contains(@data-bind, 'kmsi-view')]//input[@id='idSIButton9']";
            public static string OneTimeCode = "//input[@name='otc']";
            public static string UseAnotherAccount = "otherTile";
        }
        #endregion
        private readonly WebClient _client;

        public OnlineLogin(WebClient client) 
        {
            _client = client;
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

            _client.InitializeModes();

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

            LoginResult result = this.WebLogin(_client.Browser.Driver, orgUri, username, password, mfaSecretKey);
            if (result == LoginResult.Failure)
                throw new InvalidOperationException("Login Failure, please check your configuration");

            _client.InitializeModes();

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
            LoginResult result = this.WebLogin(_client.Browser.Driver,orgUrl, username, password, mfaSecretKey, redirectAction);
            if(result == LoginResult.Failure) 
                throw new InvalidOperationException("Login Failure, please check your configuration");
            
            _client.InitializeModes();

            return result;
        }

        #endregion
        #region LoginHelpers
        private LoginResult WebLogin(IWebDriver driver, Uri uri, SecureString username, SecureString password, SecureString mfaSecretKey = null, Action<LoginRedirectEventArgs> redirectAction = null)
        {
            bool online = !(_client.OnlineDomains != null && !_client.OnlineDomains.Any(d => uri.Host.EndsWith(d)));
            driver.Navigate().GoToUrl(uri);

            if (!online)
                return LoginResult.Success;

            driver.ClickIfVisible(By.Id(LoginReference.UseAnotherAccount));

            bool waitingForOtc = false;
            bool success = EnterUserName(driver, username);
            if (!success)
            {
                var isUserAlreadyLogged = IsUserAlreadyLogged();
                if (isUserAlreadyLogged)
                {
                    SwitchToDefaultContent(driver);
                    return LoginResult.Success;
                }

                _client.ThinkTime(1000);
                waitingForOtc = GetOtcInput(driver) != null;

                if (!waitingForOtc)
                    throw new Exception($"Login page failed. {LoginReference.UserId} not found.");
            }

            if (!waitingForOtc)
            {
                driver.ClickIfVisible(By.Id("aadTile"));
                _client.ThinkTime(1000);

                //If expecting redirect then wait for redirect to trigger
                if (redirectAction != null)
                {
                    //Wait for redirect to occur.
                    _client.ThinkTime(3000);

                    redirectAction.Invoke(new LoginRedirectEventArgs(username, password, driver));
                    return LoginResult.Redirect;
                }

                EnterPassword(driver, password);
                _client.ThinkTime(1000);
            }

            int attempts = 0;
            bool entered;
            do
            {
                entered = EnterOneTimeCode(driver, mfaSecretKey);
                success = ClickStaySignedIn(driver) || IsUserAlreadyLogged();
                attempts++;
            }
            while (!success && attempts <= Constants.DefaultRetryAttempts); // retry to enter the otc-code, if its fail & it is requested again 

            if (entered && !success)
                throw new InvalidOperationException("Something went wrong entering the OTC. Please check the MFA-SecretKey in configuration.");

            return success ? LoginResult.Success : LoginResult.Failure;
        }

        private bool IsUserAlreadyLogged() => _client.WaitForMainPage(2.Seconds());

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

        private bool EnterUserName(IWebDriver driver, SecureString username)
        {
            var input = driver.WaitUntilAvailable(By.XPath(LoginReference.UserId), new TimeSpan(0, 0, 30));
            if (input == null)
                return false;

            input.SendKeys(username.ToUnsecureString());
            input.SendKeys(Keys.Enter);
            return true;
        }

        private static void EnterPassword(IWebDriver driver, SecureString password)
        {
            var input = driver.FindElement(By.XPath(LoginReference.LoginPassword));
            input.SendKeys(password.ToUnsecureString());
            input.Submit();
        }

        private bool EnterOneTimeCode(IWebDriver driver, SecureString mfaSecretKey)
        {
            try
            {
                IWebElement input = GetOtcInput(driver); // wait for the dialog, even if key is null, to print the right error
                if (input == null)
                    return true;

                string key = mfaSecretKey?.ToUnsecureString(); // <- this 2FA secret key.
                if (string.IsNullOrWhiteSpace(key))
                    throw new InvalidOperationException("The application is wait for the OTC but your MFA-SecretKey is not set. Please check your configuration.");

                var oneTimeCode = GenerateOneTimeCode(key);
                _client.SetInputValue(driver, input, oneTimeCode, 1.Seconds());
                input.Submit();
                return true; // input found & code was entered
            }
            catch (Exception e)
            {
                var message = $"An Error occur entering OTC. Exception: {e.Message}";
                Trace.TraceInformation(message);
                throw new InvalidOperationException(message, e);
            }
        }


        private static IWebElement GetOtcInput(IWebDriver driver)
            => driver.WaitUntilAvailable(By.XPath(LoginReference.OneTimeCode), TimeSpan.FromSeconds(2));

        private static bool ClickStaySignedIn(IWebDriver driver)
        {
            var xpath = By.XPath(LoginReference.StaySignedIn);
            var element = driver.ClickIfVisible(xpath, 2.Seconds());
            return element != null;
        }

        private static void SwitchToDefaultContent(IWebDriver driver)
        {
            SwitchToMainFrame(driver);

            //Switch Back to Default Content for Navigation Steps
            driver.SwitchTo().DefaultContent();
        }

        private static void SwitchToMainFrame(IWebDriver driver)
        {
            driver.WaitForPageToLoad();
            driver.SwitchTo().Frame(0);
            driver.WaitForPageToLoad();
        }

        internal BrowserCommandResult<LoginResult> PassThroughLogin(Uri uri)
        {
            return _client.Execute(_client.GetOptions("Pass Through Login"), driver =>
            {
                driver.Navigate().GoToUrl(uri);

                _client.WaitForMainPage(60.Seconds(),
                    _ =>
                    {
                        //determine if we landed on the Unified Client Main page
                        var isUCI = driver.HasElement(By.XPath(LoginReference.CrmUCIMainPage));
                        if (isUCI)
                        {
                            driver.WaitForPageToLoad();
                            driver.WaitForTransaction();
                        }
                        else
                            //else we landed on the Web Client main page or app picker page
                            SwitchToDefaultContent(driver);
                    },
                    () => throw new InvalidOperationException("Load Main Page Fail.")
                );

                return LoginResult.Success;
            });
        }


        public void ADFSLoginAction(LoginRedirectEventArgs args)

        {
            //Login Page details go here.  You will need to find out the id of the password field on the form as well as the submit button. 
            //You will also need to add a reference to the Selenium Webdriver to use the base driver. 
            //Example

            var driver = args.Driver;

            driver.FindElement(By.Id("passwordInput")).SendKeys(args.Password.ToUnsecureString());
            driver.ClickWhenAvailable(By.Id("submitButton"), TimeSpan.FromSeconds(2));

            //Insert any additional code as required for the SSO scenario

            //Wait for CRM Page to load
            _client.WaitForMainPage(TimeSpan.FromSeconds(60), "Login page failed.");
            SwitchToMainFrame(driver);
        }

        public void MSFTLoginAction(LoginRedirectEventArgs args)

        {
            //Login Page details go here.  You will need to find out the id of the password field on the form as well as the submit button. 
            //You will also need to add a reference to the Selenium Webdriver to use the base driver. 
            //Example

            var driver = args.Driver;

            //d.FindElement(By.Id("passwordInput")).SendKeys(args.Password.ToUnsecureString());
            //d.ClickWhenAvailable(By.Id("submitButton"), TimeSpan.FromSeconds(2));

            //This method expects single sign-on

            _client.ThinkTime(5000);

            driver.WaitUntilVisible(By.XPath("//div[@id=\"mfaGreetingDescription\"]"));

            var azureMFA = driver.FindElement(By.XPath("//a[@id=\"WindowsAzureMultiFactorAuthentication\"]"));
            azureMFA.Click(true);

            Thread.Sleep(20000);

            //Insert any additional code as required for the SSO scenario

            //Wait for CRM Page to load
            _client.WaitForMainPage(TimeSpan.FromSeconds(60), "Login page failed.");
            SwitchToMainFrame(driver);
        }

        #endregion
    }
}
