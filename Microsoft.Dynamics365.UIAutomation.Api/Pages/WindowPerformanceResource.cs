// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    ///  Window Performance Resource
    /// </summary>
    public class WindowPerformanceResource : XrmPage
    {
        internal readonly string[] ResourceValues = {"name", "startTime", "duration", "redirectEnd", "redirectStart", "domainLookupEnd", "domainLookupStart", "connectEnd", "connectStart", "requestStart", "responseStart", "responseEnd"};

        public WindowPerformanceResource(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToContent();
        }

        /// <summary>
        /// Get Window Performance Resource Timings
        /// </summary>
        /// <example>xrmBrowser.WindowPerformance.GetWindowPerfResourceTimings()</example>
        public BrowserCommandResult<List<PerformanceResource>> GetWindowPerfResourceTimings()
        {
            return this.Execute("Get Window Performance Resource Timings", driver =>
            {
                // content
                var performanceResourceList = ProcessWindowPerfResourceTimings();

                // switch to default content
                Browser.Driver.SwitchTo().DefaultContent();
                var contentperformanceResourceList = ProcessWindowPerfResourceTimings(false);

                if (!contentperformanceResourceList.Any()) return performanceResourceList;
                var joinedPerfLists =
                    performanceResourceList.Concat(contentperformanceResourceList).OrderBy(x => x.StartTime).ToList();
                return joinedPerfLists;
            });
        }

        /// <summary>
        /// Clear Resource Timings
        /// </summary>
        /// <example>xrmBrowser.WindowPerformance.ClearResourceTimings()</example>
        public BrowserCommandResult<bool> ClearResourceTimings()
        {
            return this.Execute("Clear Resource Timings", driver =>
            {
                driver.ExecuteScript("return window.performance.clearResourceTimings();");
                return true;
            });
        }

        /// <summary>
        /// Process WindowPerfResourceTimings
        /// </summary>
        /// <param name="content">Content Type bool</param>
        /// <returns>List of PerformanceResource</returns>
        /// <example>var performanceResourceList = ProcessWindowPerfResourceTimings();</example>
        public List<PerformanceResource> ProcessWindowPerfResourceTimings(bool content = true)
        {
            return this.Execute("Get Window Performance Resource Timings", driver =>
            {
                var entries = (IEnumerable<object>)driver.ExecuteScript("return window.performance.getEntriesByType('resource')");
                var performanceResourceList = new List<PerformanceResource>();
                var resultsList = ProcessResourceDictionary(entries);
                foreach (var entry in resultsList)
                {
                    string value;
                    var perfResource = new PerformanceResource()
                    {
                        EntryType = "Resource",
                        Name = entry.TryGetValue("name", out value) ? value : "",
                        Duration = entry.TryGetValue("duration", out value) ? double.Parse(value) : 0,
                        StartTime = entry.TryGetValue("startTime", out value) ? double.Parse(value) : 0,
                        RequestStart = entry.TryGetValue("requestStart", out value) ? double.Parse(value) : 0,
                        ContentType = content ? "Content" : "DefaultContent"
                    };

                    //redirect time, dns time, tcp handshake, response time, resource time duration
                    string end, start;
                    if (entry.TryGetValue("redirectEnd", out end) &&
                        entry.TryGetValue("redirectStart", out start))
                        perfResource.RedirectTime = double.Parse(end) - double.Parse(start);

                    if (entry.TryGetValue("domainLookupEnd", out end) &&
                        entry.TryGetValue("domainLookupStart", out start))
                    {
                        perfResource.DomainLookupStart = double.Parse(start);
                        perfResource.DomainLookupEnd = double.Parse(end);
                        perfResource.DnsTime = double.Parse(end) - double.Parse(start);
                    }

                    if (entry.TryGetValue("connectEnd", out end) &&
                        entry.TryGetValue("connectStart", out start))
                        perfResource.TcpHandshake = double.Parse(end) - double.Parse(start);

                    if (entry.TryGetValue("responseEnd", out end) &&
                        entry.TryGetValue("responseStart", out start))
                    {
                        perfResource.ResponseStart = double.Parse(start);
                        perfResource.ResponseEnd = double.Parse(end);
                        perfResource.ResponseTime = double.Parse(end) - double.Parse(start);
                    }

                    perfResource.ResourceTimingDuration = perfResource.ResponseEnd - perfResource.StartTime;

                    performanceResourceList.Add(perfResource);
                }

                return performanceResourceList;
            });
        }
        private IEnumerable<Dictionary<string, string>> ProcessResourceDictionary(IEnumerable<object> entries)
        {
            var results = new List<Dictionary<string, string>>();
            foreach (var entry in entries)
            {
                var row = new Dictionary<string, string>();
                var dic = (Dictionary<string, object>)entry;

                foreach (var key in dic.Keys)
                {
                    if (!((IList<string>)ResourceValues).Contains(key)) continue;

                    object val;
                    if (!dic.TryGetValue(key, out val))
                    {
                        val = key == "name" ? "" : "0";
                    }
                    row.Add(key, val.ToString());
                }
                results.Add(row);
            }
            return results;
        }
    }
}
