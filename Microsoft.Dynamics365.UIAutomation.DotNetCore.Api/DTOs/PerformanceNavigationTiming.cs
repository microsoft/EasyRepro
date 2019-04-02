// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class PerformanceNavigationTiming
    {
        /// <summary>
        /// Content Type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Navigation Type
        /// </summary>
        public string NavigationType { get; set; }

        /// <summary>
        /// Navigation Start
        /// </summary>
        public long NavigationStart { get; set; }
        
        /// <summary>
        /// Page Load Time
        /// </summary>
        public long PageLoadTime { get; set; }

        /// <summary>
        /// Dom Load Event End
        /// </summary>
        public long LoadEventEnd { get; set; }

        /// <summary>
        /// Fetch Start
        /// </summary>
        public long FetchStart { get; set; }

        /// <summary>
        /// Dom Content Load Time
        /// </summary>
        public long DomContentLoadTime { get; set; }

        /// <summary>
        /// Request ResponseTime
        /// </summary>
        public long RequestResponseTime { get; set; }

        /// <summary>
        /// Page Render Time
        /// </summary>
        public long PageRenderTime { get; set; }

        /// <summary>
        /// Network Latency: responseEnd-fetchStart
        /// </summary>
        public long NetworkLatency { get; set; }

        /// <summary>
        /// Redirect Time: redirectEnd - redirectStart
        /// </summary>
        public long RedirectTime { get; set; }

        /// <summary>
        /// Redirect Count
        /// </summary>
        public long RedirectCount { get; set; }

        /// <summary>
        /// Dom Parsing Time
        /// </summary>
        public long DomParsingTime { get; set; }
    }
}
