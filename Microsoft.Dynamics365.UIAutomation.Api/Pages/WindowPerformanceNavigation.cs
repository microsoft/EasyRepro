// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Window Performance Navigation
    /// </summary>
    public class WindowPerformanceNavigation : XrmPage
    {
        public WindowPerformanceNavigation(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToContent();
        }

        /// <summary>
        /// Get Window Performance Navigation Timings
        /// </summary>
        /// <example>xrmBrowser.WindowPerformance.GetWindowPerfNavigationTimings()</example>
        public BrowserCommandResult<List<PerformanceNavigationTiming>> GetWindowPerfNavigationTimings()
        {
            return this.Execute("Get Window Performance Resource Timings", driver =>
            {
                // content
                var perfNavTimings = new List<PerformanceNavigationTiming> {ProcessWindowPerfNavigationTiming()};

                // switch to default content
                Browser.Driver.SwitchTo().DefaultContent();
                perfNavTimings.Add(ProcessWindowPerfNavigationTiming(false));

                return perfNavTimings;
            });
        }


        /// <summary>
        /// Process Window Performance Navigation Timing
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public BrowserCommandResult<PerformanceNavigationTiming> ProcessWindowPerfNavigationTiming(bool content=true)
        {
            return this.Execute("Get Window Performance Navigation Timings", driver =>
            {
                var perfNavTiming = new PerformanceNavigationTiming
                {
                    ContentType = content ? "Content" : "DefaultContent",
                    FetchStart = (long)driver.ExecuteScript("return window.performance.timing.fetchStart"),
                    LoadEventEnd = (long)driver.ExecuteScript("return window.performance.timing.loadEventEnd"),
                    NavigationStart = (long)driver.ExecuteScript("return window.performance.timing.navigationStart"),
                    DomContentLoadTime = (long)driver.ExecuteScript("return window.performance.timing.domComplete - window.performance.timing.domInteractive"),
                    DomParsingTime = (long)driver.ExecuteScript("return window.performance.timing.domInteractive - window.performance.timing.domLoading"),
                    RequestResponseTime = (long)driver.ExecuteScript("return window.performance.timing.responseEnd - window.performance.timing.requestStart"),
                    PageRenderTime = (long)driver.ExecuteScript("return window.performance.timing.domComplete - window.performance.timing.domLoading"),
                    NetworkLatency = (long)driver.ExecuteScript("return window.performance.timing.responseEnd - window.performance.timing.fetchStart"),
                    RedirectTime = (long)driver.ExecuteScript("return window.performance.timing.redirectEnd - window.performance.timing.redirectStart"),
                    RedirectCount = (long)driver.ExecuteScript("return window.performance.navigation.redirectCount")
                };

                perfNavTiming.PageLoadTime = perfNavTiming.LoadEventEnd -
                                                    perfNavTiming.NavigationStart;

                var navigationType = (long)driver.ExecuteScript("return window.performance.navigation.type");
                switch (navigationType)
                {
                    case 0:
                        perfNavTiming.NavigationType = "Navigation";
                        break;
                    case 1:
                        perfNavTiming.NavigationType = "Reload";
                        break;
                    case 2:
                        perfNavTiming.NavigationType = "History";
                        break;
                    case 3:
                        perfNavTiming.NavigationType = "Unknown";
                        break;
                }

                return perfNavTiming;
            });
        }
    }
}

