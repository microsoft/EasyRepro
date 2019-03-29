// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Security;
using System.Threading;

namespace Microsoft.PowerApps.UIAutomation.Api
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
    public class OnlineLogin
        : AppPage
    {
        public string[] OnlineDomains { get; set; }

        public OnlineLogin(InteractiveBrowser browser)
            : base(browser)
        {
            this.OnlineDomains = Constants.Xrm.XrmDomains;
        }

        public OnlineLogin(InteractiveBrowser browser, params string[] onlineDomains)
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
            if (this.Browser.Options.Credentials.IsDefault)
                throw new InvalidOperationException("The default login method cannot be invoked without first setting credentials on the Browser object.");

            return this.Execute(GetOptions("Login"), this.Login, uri, this.Browser.Options.Credentials.Username, this.Browser.Options.Credentials.Password, default(Action<LoginRedirectEventArgs>));
        }
        /// <summary>
        /// Login Page
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <param name="username">The Username to login to CRM application</param>
        /// <param name="password">The Password to login to CRM application</param>
        /// <example>xrmBrowser.OnlineLogin.Login(_xrmUri, _username, _password);</example>
        public BrowserCommandResult<LoginResult> Login(Uri uri, SecureString username, SecureString password)
        {
            return this.Execute(GetOptions("Login"), this.Login, uri, username, password, default(Action<LoginRedirectEventArgs>));
        }

        /// <summary>
        /// Login Page
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <param name="username">The Username to login to SharePoint application</param>
        /// <param name="password">The Password to login to SharePoint application</param>
        /// <example>xrmBrowser.OnlineLogin.Login(_xrmUri, _username, _password);</example>
        public BrowserCommandResult<LoginResult> SharePointLogin(Uri uri, SecureString username, SecureString password)
        {
            return this.Execute(GetOptions("SharePoint Login"), this.SharePointLogin, uri, username, password, default(Action<LoginRedirectEventArgs>));
        }

        /// <summary>
        /// Login Page
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <param name="username">The Username to login to CRM application</param>
        /// <param name="password">The Password to login to CRM application</param>
        /// <param name="redirectAction">The RedirectAction</param>
        /// <example>xrmBrowser.OnlineLogin.Login(_xrmUri, _username, _password, ADFSLogin);</example>
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

                // Attempt to locate the UserId field
                var userIdField = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]));

                // Known Issue: Hosted Agent unable to identify UserId field. Check NULL and try to refresh the page.
                if (userIdField is null)
                {
                    driver.Manage().Window.Minimize();
                    driver.Manage().Window.Maximize();
                    driver.Navigate().Refresh();
                    userIdField = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]));
                }

                var userIdFieldVisible = driver.IsVisible(By.XPath(Elements.Xpath[Reference.Login.UserId]));
                Console.WriteLine($"Value of userIdFieldVisible: {userIdFieldVisible}");

                if (userIdFieldVisible)
                {
                    Console.WriteLine("UserID field is visible. Proceeding with login.");
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(username.ToUnsecureString());
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(Keys.Tab);
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(Keys.Enter);

                    Thread.Sleep(2000);

                    //If expecting redirect then wait for redirect to trigger
                    if (redirectAction != null)
                    {
                        //Wait for redirect to occur.
                        Thread.Sleep(3000);

                        redirectAction?.Invoke(new LoginRedirectEventArgs(username, password, driver));

                        redirect = true;
                    }
                    else
                    {
                        Thread.Sleep(1000);

                        driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(password.ToUnsecureString());
                        driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(Keys.Tab);
                        driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).Submit();

                        Thread.Sleep(1000);

                        if (driver.IsVisible(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])))
                        {
                            driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]));
                        }

                        driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.MainPage])
                            , new TimeSpan(0, 2, 0),
                            e =>
                            {
                                try
                                {
                                    e.WaitUntilClickable(By.ClassName("d365shell-c-groups-menu-toggle"), new TimeSpan(0, 0, 30));
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine("The Environment Picker did not return clickable");
                                    throw new InvalidOperationException($"The Environment Picker did not return clickable: {exc}");
                                }

                                e.WaitForPageToLoad();
                            },
                            f =>
                            {
                                Console.WriteLine("Login.MainPage failed to load in 2 minutes.");
                                throw new Exception("Login page failed.");
                            });
                    }
                }
                else
                {
                    Console.WriteLine("UserID field is not visible. This should indicate a previous main page load failure.");
                    // This scenario should only be hit in the event of a login.microsoftonline.com failure, or a login retry authentication where an authentication token was already retrieved
                    driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.MainPage])
                        , new TimeSpan(0, 2, 0),
                        e =>
                        {
                            try
                            {
                                e.WaitUntilClickable(By.ClassName("d365shell-c-groups-menu-toggle"), new TimeSpan(0, 0, 30));

                            }
                            catch (Exception exc)
                            {
                                Console.WriteLine("The Environment Picker did not return clickable");
                                throw new InvalidOperationException($"The Environment Picker did not return clickable: {exc}");
                            }

                            e.WaitForPageToLoad();
                        },
                        f =>
                        {
                            Console.WriteLine("Login.MainPage failed to load in 2 minutes.");
                            throw new Exception("Login page failed.");
                        });
                }
            }

            return redirect ? LoginResult.Redirect : LoginResult.Success;
        }

        private LoginResult SharePointLogin(IWebDriver driver, Uri uri, SecureString username, SecureString password, Action<LoginRedirectEventArgs> redirectAction)
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
                Thread.Sleep(2000);

                //If expecting redirect then wait for redirect to trigger
                if (redirectAction != null)
                {
                    //Wait for redirect to occur.
                    Thread.Sleep(3000);

                    redirectAction?.Invoke(new LoginRedirectEventArgs(username, password, driver));

                    redirect = true;
                }
                else
                {
                    Thread.Sleep(1000);

                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(password.ToUnsecureString());
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(Keys.Tab);
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).Submit();

                    Thread.Sleep(1000);

                    if (driver.IsVisible(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])))
                    {
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]));
                    }

                    driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.SharePointPage])
                        , new TimeSpan(0, 0, 60),
                        e =>
                        {
                            e.WaitUntilVisible(By.Id("sideNavBox"), new TimeSpan(0, 0, 30));

                            //e.WaitForPageToLoad();
                        },
                        f => { throw new Exception("Login page failed."); });
                }
            }

            return redirect ? LoginResult.Redirect : LoginResult.Success;
        }

    }
}