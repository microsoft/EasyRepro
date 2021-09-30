// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Win32;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class BrowserOptions
    {
        public BrowserOptions()
        {
            this.DriversPath = Path.Combine(Directory.GetCurrentDirectory()); //, @"Drivers\");
            this.DownloadsPath = null;
            this.BrowserType = BrowserType.IE;
            this.PageLoadTimeout = new TimeSpan(0, 3, 0);
            this.CommandTimeout = TimeSpan.FromMinutes(20);
            this.StartMaximized = true;
            this.FireEvents = false;
            this.TraceSource = Constants.DefaultTraceSource;
            this.EnableRecording = false;
            this.RecordingScanInterval = TimeSpan.FromMilliseconds(Constants.Browser.Recording.DefaultScanInterval);
            this.Credentials = BrowserCredentials.Default;
            this.HideDiagnosticWindow = true;
            this.Height = null;
            this.Width = null;
            this.UCITestMode = true;
            this.UCIPerformanceMode = false;
            this.AppInsightsKey = string.Empty;
            this.DisableExtensions = false;
            this.DisableFeatures = false;
            this.DisablePopupBlocking = false;
            this.DisableSettingsWindow = false;
            this.EnableJavascript = false;
            this.NoSandbox = false;
            this.DisableGpu = false;
            this.DumpDom = false;
            this.EnableAutomation = false;
            this.DisableImplSidePainting = false;
            this.DisableDevShmUsage = false;
            this.DisableInfoBars = false;
            this.Headless = false;
            this.TestTypeBrowser = false;
        }

        public BrowserType RemoteBrowserType { get; set; }
        public Uri RemoteHubServer { get; set; }
        public BrowserType BrowserType { get; set; }
        public BrowserCredentials Credentials { get; set; }
        public string DriversPath { get; set; }
        public string DownloadsPath { get; set; }
        public bool PrivateMode { get; set; }
        public bool CleanSession { get; set; }
        public TimeSpan PageLoadTimeout { get; set; }
        public TimeSpan CommandTimeout { get; set; } = TimeSpan.FromMinutes(20);
        /// <summary>
        /// When <see langword="true" /> the browser will open maximized at the highest supported resolution.
        /// </summary>
        public bool StartMaximized { get; set; }
        public bool FireEvents { get; set; }
        public bool EnableRecording { get; set; }
        public TimeSpan RecordingScanInterval { get; set; }
        public string TraceSource { get; set; }
        public bool HideDiagnosticWindow { get; set; }
        public bool Headless { get; set; }
        public bool DisableExtensions { get; set; }
        public bool DisableFeatures { get; set; }
        public bool DisablePopupBlocking { get; set; }
        public bool DisableSettingsWindow { get; set; }
        public bool EnableJavascript { get; set; }
        public bool NoSandbox { get; set; }
        public bool DisableGpu { get; set; }
        public bool DumpDom { get; set; }
        public bool EnableAutomation { get; set; }
        public bool DisableImplSidePainting { get; set; }
        public bool DisableDevShmUsage { get; set; }
        public bool DisableInfoBars { get; set; }
        public bool TestTypeBrowser { get; set; }
        public bool UserAgent { get; set; }
        public string UserAgentValue { get; set; }
        public int DefaultThinkTime { get; set; }

        /// <summary>
        /// Gets or sets the browser height when <see cref="StartMaximized"/> is <see langword="false" />. Both <see cref="Height"/> and <see cref="Width"/> must be set.
        /// </summary>
        public int? Height { get; set; }
        /// <summary>
        /// Gets or sets the browser width when Both <see cref="StartMaximized"/> is <see langword="false" />. Both <see cref="Height"/> and <see cref="Width"/> must be set.
        /// </summary>
        public int? Width { get; set; }
        /// <summary>
        /// Gets or sets the TestMode flag for the UnifiedInterface. This flag should not be used when capturing performance measurements
        /// This flag introduces full loading patterns that are not typical of a normal user experience, but are required for full DOM interaction.
        /// Please raise any issues with this TestMode being enabled to the Microsoft/EasyRepro community on GitHub for review.
        /// </summary>
        public bool UCITestMode { get; set; }

        /// <summary>
        /// Gets or sets the Performance Mode to enable performance center telemetry. 
        /// </summary>
        public bool UCIPerformanceMode { get; set; }

        /// <summary>
        /// Gets or sets the Application Insights Instrumentation Key to write telemetry to. 
        /// </summary>
        public string AppInsightsKey { get; set; }

        public virtual ChromeOptions ToChrome()
        {
            var options = new ChromeOptions();

            if (this.StartMaximized)
            {
                options.AddArgument("--start-maximized");
            }

            if (this.PrivateMode)
            {
                options.AddArgument("--incognito");
            }

            if (this.Headless)
            {
                options.AddArgument("--headless");
            }

            if (UserAgent && !string.IsNullOrEmpty(UserAgentValue))
            {
                options.AddArgument("--user-agent=" + UserAgentValue);
            }

            if (this.DisableExtensions)
            {
                options.AddArgument("--disable-extensions");
            }

            if (this.DisableFeatures)
            {
                options.AddArgument("--disable-features");
            }

            if (this.DisablePopupBlocking)
            {
                options.AddArgument("--disable-popup-blocking");
            }

            if (this.DisableSettingsWindow)
            {
                options.AddArgument("--disable-settings-window");
            }

            if (this.DisableImplSidePainting)
            {
                options.AddArgument("--disable-impl-side-painting");
            }

            if (this.EnableJavascript)
            {
                options.AddArgument("--enable-javascript");
            }

            if (this.NoSandbox)
            {
                options.AddArgument("--no-sandbox");
            }

            if (this.DisableGpu)
            {
                options.AddArgument("--disable-gpu");
            }

            if (this.DumpDom)
            {
                options.AddArgument("--dump-dom");
            }

            if (this.EnableAutomation)
            {
                options.AddArgument("--enable-automation");
            }

            if (this.DisableDevShmUsage)
            {
                options.AddArgument("--disable-dev-shm-usage");
            }

            if (this.DisableInfoBars)
            {
                options.AddArgument("disable-infobars");
            }

            if (this.TestTypeBrowser)
            {
                options.AddArgument("test-type");
                options.AddArgument("test-type=browser");
            }

            if (!string.IsNullOrEmpty(DownloadsPath))
            {
                options.AddUserProfilePreference("download.default_directory", DownloadsPath);
            }

            return options;
        }

        public virtual InternetExplorerOptions ToInternetExplorer()
        {

            // For IE, TabProcGrowth must be set to 0 if we want to initiate through
            // the CreateProcess API, which is required for InPrivate mode.
            if (this.PrivateMode)
            {
                var value = Registry.GetValue(Constants.Browser.IESettingsRegistryHive, Constants.Browser.IESettingsTabProcGrowthKey, null);

                if (value == null || value.ToString() != "0")
                {
                    Registry.SetValue(Constants.Browser.IESettingsRegistryHive, Constants.Browser.IESettingsTabProcGrowthKey, 0);
                }
            }

            var options = new InternetExplorerOptions()
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                EnsureCleanSession = this.CleanSession,
                ForceCreateProcessApi = this.PrivateMode,
                //page = InternetExplorerPageLoadStrategy.Eager,
                IgnoreZoomLevel = true,
                EnablePersistentHover = true,
                BrowserCommandLineArguments = this.PrivateMode ? "-private" : ""

            };

            return options;
        }

        public virtual FirefoxOptions ToFireFox()
        {
            var options = new FirefoxOptions()
            {
                UseLegacyImplementation = false
            };

            if (!string.IsNullOrEmpty(DownloadsPath))
            {
                options.SetPreference("browser.download.folderList", 2);
                options.SetPreference("browser.download.dir", DownloadsPath);
                options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "text/csv,application/java-archive, application/x-msexcel,application/excel,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/x-excel,application/vnd.ms-excel,image/png,image/jpeg,text/html,text/plain,application/msword,application/xml,application/vnd.microsoft.portable-executable");
            }

            return options;
        }

        public virtual EdgeOptions ToEdge()
        {
            var options = new EdgeOptions()
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                UseInPrivateBrowsing = PrivateMode
            };

            return options;
        }
    }
}