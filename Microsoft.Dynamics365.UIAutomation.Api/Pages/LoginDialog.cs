// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Security;
using System.Threading;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public enum LoginResult
    {
        Success,
        Failure,
        Redirect
    }

    /// <summary>
    /// Login Page
    /// </summary>
    public class LoginDialog
        : XrmPage
    {
        public string[] OnlineDomains { get; set; }

        public LoginDialog(InteractiveBrowser browser)
            : base(browser)
        {
            this.OnlineDomains = Constants.Xrm.XrmDomains;
        }

        public LoginDialog(InteractiveBrowser browser, params string[] onlineDomains)
            : this(browser)
        {
            this.OnlineDomains = onlineDomains;
        }

        public BrowserCommandResult<LoginResult> Login()
        {
            return this.Login(new Uri(Constants.DefaultLoginUri));
        }

        public BrowserCommandResult<LoginResult> Login(SecureString username, SecureString password)
        {
            return this.Execute(GetOptions("Login"), this.Login, new Uri(Constants.DefaultLoginUri), username, password, default(Action<LoginRedirectEventArgs>));
        }

        public BrowserCommandResult<LoginResult> Login(SecureString username, SecureString password, Action<LoginRedirectEventArgs> redirectAction)
        {
            return this.Execute(GetOptions("Login"), this.Login, new Uri(Constants.DefaultLoginUri), username, password, redirectAction);
        }

        public BrowserCommandResult<LoginResult> Login(Uri uri)
        {
            if (this.Browser.Options.Credentials.Username == null)
                return PassThroughLogin(uri);
            else
                return this.Execute(GetOptions("Login"), this.Login, uri, this.Browser.Options.Credentials.Username, this.Browser.Options.Credentials.Password, default(Action<LoginRedirectEventArgs>));
        }

        /// <summary>
        /// Login Page
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <param name="username">The Username to login to CRM application</param>
        /// <param name="password">The Password to login to CRM application</param>
        /// <example>xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);</example>
        public BrowserCommandResult<LoginResult> Login(Uri uri, SecureString username, SecureString password)
        {
            return this.Execute(GetOptions("Login"), this.Login, uri, username, password, default(Action<LoginRedirectEventArgs>));
        }

        /// <summary>
        /// Login Page
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <param name="username">The Username to login to CRM application</param>
        /// <param name="password">The Password to login to CRM application</param>
        /// <param name="redirectAction">The RedirectAction</param>
        /// <example>xrmBrowser.LoginPage.Login(_xrmUri, _username, _password, ADFSLogin);</example>
        public BrowserCommandResult<LoginResult> Login(Uri uri, SecureString username, SecureString password, Action<LoginRedirectEventArgs> redirectAction)
        {
            return this.Execute(GetOptions("Login"), this.Login, uri, username, password, redirectAction);
        }

        private LoginResult Login(IWebDriver driver, Uri uri, SecureString username, SecureString password, Action<LoginRedirectEventArgs> redirectAction)
        {
            var redirect = false;
            bool online = !(this.OnlineDomains != null && !this.OnlineDomains.Any(d => uri.Host.EndsWith(d)));
            driver.Navigate().GoToUrl(uri);

            if (online)
            {
                if (driver.IsVisible(By.Id("use_another_account_link")))
                    driver.ClickWhenAvailable(By.Id("use_another_account_link"));

                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]),
                    $"The Office 365 sign in page did not return the expected result and the user '{username}' could not be signed in.");

                driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(username.ToUnsecureString());
                driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(Keys.Tab);
                driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(Keys.Enter);

                Thread.Sleep(1000);

                if (driver.IsVisible(By.Id("aadTile")))
                {
                    driver.FindElement(By.Id("aadTile")).Click(true);
                }

                Thread.Sleep(1000);

                //If expecting redirect then wait for redirect to trigger
                if (redirectAction != null)
                {
                    //Wait for redirect to occur.
                    Thread.Sleep(3000);

                    redirectAction?.Invoke(new LoginRedirectEventArgs(username, password, driver));

                    redirect = true;

                    MarkOperation(driver);
                }
                else
                {
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(password.ToUnsecureString());
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(Keys.Tab);
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).Submit();

                    Thread.Sleep(2000);

                    if (driver.IsVisible(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])))
                    {
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]));

                        //Click didn't work so use submit
                        if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])))
                            driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])).Submit();
                    }

                    driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.CrmMainPage])
                        , new TimeSpan(0, 0, 60),
                        e =>
                        {
                            driver.WaitForPageToLoad();
                            MarkOperation(driver);
                            driver.SwitchTo().Frame(0);
                            driver.WaitForPageToLoad();
                        },
                        "Login page failed."
                    );
                }
            }

            return redirect ? LoginResult.Redirect : LoginResult.Success;
        }

        internal BrowserCommandResult<LoginResult> PassThroughLogin(Uri uri)
        {
            return this.Execute(GetOptions("Pass Through Login"), driver =>
            {
                driver.Navigate().GoToUrl(uri);

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.CrmMainPage]),
                    new TimeSpan(0, 0, 60),
                    e =>
                    {
                        driver.WaitForPageToLoad();
                        MarkOperation(driver);
                        driver.SwitchTo().Frame(0);
                        driver.WaitForPageToLoad();
                    },
                    "Login page failed."
                );
                return LoginResult.Success;
            });
        }

        private void MarkOperation(IWebDriver driver)
        {
            if (driver.HasElement(By.Id(Elements.ElementId[Reference.Login.TaggingId])))
                driver.ExecuteScript($"document.getElementById('{Elements.ElementId[Reference.Login.TaggingId]}').src = '_imgs/NavBar/Invisible.gif?operation=easyrepro|web|{Guid.NewGuid().ToString()}';");
        }

        public void ADFSLoginAction(LoginRedirectEventArgs args)
        {
            //Login Page details go here.  You will need to find out the id of the password field on the form as well as the submit button. 
            //You will also need to add a reference to the Selenium Webdriver to use the base driver. 
            //Example

            var driver = args.Driver;

            driver.FindElement(By.Id("passwordInput")).SendKeys(args.Password.ToUnsecureString());
            driver.ClickWhenAvailable(By.Id("submitButton"), new TimeSpan(0, 0, 2));

            //Insert any additional code as required for the SSO scenario

            //Wait for CRM Page to load
            driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.CrmMainPage]),
                TimeSpan.FromSeconds(60),
                e =>
                {
                    driver.WaitForPageToLoad();
                    driver.SwitchTo().Frame(0);
                    driver.WaitForPageToLoad();
                },
                "Login page failed."
            );
        }
    }
}