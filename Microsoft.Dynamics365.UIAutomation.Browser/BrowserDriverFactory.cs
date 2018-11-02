// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Edge;
using System;
using OpenQA.Selenium.Remote;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public static class BrowserDriverFactory
    {
        public static IWebDriver CreateWebDriver(BrowserOptions options)
        {
            var driver = options.RemoteServerUrl != null ? CreateRemoteDriver(options) : CreateLocalDriver(options);

            driver.Manage().Timeouts().PageLoad = options.PageLoadTimeout;

            if (options.StartMaximized && options.BrowserType != BrowserType.Chrome) //Handle Chrome in the Browser Options
                driver.Manage().Window.Maximize();

            if (options.FireEvents || options.EnableRecording)
            {
                // Wrap the newly created driver.
                driver = new EventFiringWebDriver(driver);
            }

            return driver;
        }

        private static IWebDriver CreateLocalDriver(BrowserOptions options)
        {
            IWebDriver driver;

            switch (options.BrowserType)
            {
                case BrowserType.Chrome:
                    var chromeService = ChromeDriverService.CreateDefaultService();
                    chromeService.HideCommandPromptWindow = options.HideDiagnosticWindow;
                    driver = new ChromeDriver(chromeService, options.ToChrome());
                    break;
                case BrowserType.IE:
                    var ieService = InternetExplorerDriverService.CreateDefaultService();
                    ieService.SuppressInitialDiagnosticInformation = options.HideDiagnosticWindow;
                    driver = new InternetExplorerDriver(ieService, options.ToInternetExplorer(), TimeSpan.FromMinutes(20));
                    break;
                case BrowserType.Firefox:
                    var ffService = FirefoxDriverService.CreateDefaultService();
                    ffService.HideCommandPromptWindow = options.HideDiagnosticWindow;
                    driver = new FirefoxDriver(ffService, options.ToFireFox(), TimeSpan.FromMinutes(20));
                    driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 5);
                    break;
                case BrowserType.Edge:
                    var edgeService = EdgeDriverService.CreateDefaultService();
                    edgeService.HideCommandPromptWindow = options.HideDiagnosticWindow;
                    driver = new EdgeDriver(edgeService, options.ToEdge(), TimeSpan.FromMinutes(20));
                    break;
                default:
                    throw new InvalidOperationException(
                        $"The browser type '{options.BrowserType}' is not recognized.");
            }

            return driver;
        }

        private static IWebDriver CreateRemoteDriver(BrowserOptions browserOptions)
        {
            return new RemoteWebDriver(browserOptions.RemoteServerUrl, GetDriverOptions(browserOptions));
        }

        private static DriverOptions GetDriverOptions(BrowserOptions options)
        {
            switch (options.BrowserType)
            {
                case BrowserType.Chrome:
                    return options.ToChrome();
                case BrowserType.IE:
                    return options.ToInternetExplorer();
                case BrowserType.Firefox:
                    return options.ToFireFox();
                case BrowserType.Edge:
                    return options.ToEdge();
                default:
                    throw new InvalidOperationException(
                        $"The browser type '{options.BrowserType}' is not recognized.");
            }
        }
    }
}