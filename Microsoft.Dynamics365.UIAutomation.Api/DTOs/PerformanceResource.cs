// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Performance Resource DTO
    /// </summary>
    public class PerformanceResource
    {
        /// <summary>
        /// Entry Type
        /// </summary>
        public string EntryType { get; set; }

        /// <summary>
        /// Name of the resource
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Start Time
        /// </summary>
        public double StartTime { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Redirect Time
        /// </summary>
        public double RedirectTime { get; set; }

        /// <summary>
        /// Domain Lookup Start
        /// </summary>
        public double DomainLookupStart { get; set; }

        /// <summary>
        /// Domain Lookup End
        /// </summary>
        public double DomainLookupEnd { get; set; }
        /// <summary>
        /// DNS Time
        /// </summary>
        public double DnsTime { get; set; }

        /// <summary>
        /// TCP Handshake
        /// </summary>
        public double TcpHandshake { get; set; }

        /// <summary>
        /// Response Start
        /// </summary>
        public double ResponseStart { get; set; }

        /// <summary>
        /// Response End
        /// </summary>
        public double ResponseEnd { get; set; }
        
        /// <summary>
        /// ResponseTime
        /// </summary>
        public double ResponseTime { get; set; }

        /// <summary>
        /// Request Start
        /// </summary>
        public double RequestStart { get; set; }

        /// <summary>
        /// Resource Timing Duration
        /// </summary>
        public double ResourceTimingDuration { get; set; }

        /// <summary>
        /// Content Type
        /// </summary>
        public string ContentType { get; set; }
        
    }
}
