using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    ///  Telemetry class
    /// </summary>
    public class Telemetry
    {
        /// <summary>
        ///  Azure Key
        /// </summary>
        public string AzureKey { get; }

        /// <summary>
        /// Execution Id
        /// </summary>
        public string ExecutionId { get; }

        /// <summary>
        /// Request Id
        /// </summary>
        public string RequestId { get; }

        public Telemetry(string key, string executionId, string requestId = "")
        {
            AzureKey = key;
            ExecutionId = executionId;
            RequestId = requestId;
        }

        /// <summary>
        /// Track Events
        /// </summary>
        /// <param name="metrics">Dictionary<string,double> of telemetry metrics</param>
        public void TrackEvents(Dictionary<string,double> metrics)
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient {InstrumentationKey = AzureKey};
            
            var properties = new Dictionary<string, string>();

            properties.Add("ExecutionId", ExecutionId);
            properties.Add("RequestId", RequestId);

            telemetry.TrackEvent("Performance Markers", properties, metrics);
            telemetry.Flush();
        }

        /// <summary>
        /// Track Events
        /// </summary>
        /// <param name="results">List of ICommandResult</param>
        public void TrackEvents( List<ICommandResult> results)
        {
            var telemetry = new ApplicationInsights.TelemetryClient {InstrumentationKey = AzureKey};

            foreach (ICommandResult result in results)
            {
                var properties = new Dictionary<string, string>();
                var metrics = new Dictionary<string, double>();

                properties.Add("StartTime", result.StartTime.Value.ToLongDateString());
                properties.Add("EndTime", result.StopTime.Value.ToLongDateString());
                properties.Add("ExecutionId", ExecutionId);

                metrics.Add("ThinkTime", result.ThinkTime);
                metrics.Add("TransitionTime", result.TransitionTime);
                metrics.Add("ExecutionTime", result.ExecutionTime);
                metrics.Add("ExecutionAttempts", result.ExecutionAttempts);

                telemetry.TrackEvent(result.CommandName, properties, metrics);
            }

            telemetry.Flush();
        }

        /// <summary>
        /// Track Events
        /// </summary>
        /// <param name="perfResourceList">List of PerformanceResource</param>
        /// <param name="apiName">Api Name</param>
        public void TrackEvents(List<PerformanceResource> perfResourceList, string apiName )
        {
            var telemetry = new ApplicationInsights.TelemetryClient { InstrumentationKey = AzureKey };
            
            foreach (var entry in perfResourceList)
            {
                var properties = new Dictionary<string, string>();
                var metrics = new Dictionary<string, double>();

                properties.Add("ExecutionId", ExecutionId);
                properties.Add("Name", entry.Name);
                properties.Add("EntryType", entry.EntryType);
                properties.Add("ContentType", entry.ContentType);

                metrics.Add("Duration", entry.Duration);
                metrics.Add("StartTime", entry.StartTime);
                metrics.Add("RedirectTime", entry.RedirectTime);
                metrics.Add("DomainLookupStart", entry.DomainLookupStart);
                metrics.Add("DomainLookupEnd", entry.DomainLookupEnd);
                metrics.Add("DnsTime", entry.DnsTime);
                metrics.Add("TcpHandshake", entry.TcpHandshake);
                metrics.Add("ResponseStart", entry.ResponseStart);
                metrics.Add("ResponseEnd", entry.ResponseEnd);
                metrics.Add("ResponseTime", entry.ResponseTime);
                metrics.Add("ResourceTimingDuration", entry.ResourceTimingDuration);

                telemetry.TrackEvent(apiName, properties, metrics);
            }
            telemetry.Flush();
        }

        /// <summary>
        /// Track Event
        /// </summary>
        /// <param name="perfNavTimingList">List of PerformanceNavigationTiming</param>
        /// <param name="apiName">Api Name</param>
        public void TrackEvents(List<PerformanceNavigationTiming> perfNavTimingList, string apiName)
        {
            var telemetry = new ApplicationInsights.TelemetryClient { InstrumentationKey = AzureKey };

            foreach (var entry in perfNavTimingList)
            {
                var properties = new Dictionary<string, string>();
                var metrics = new Dictionary<string, double>();

                properties.Add("ExecutionId", ExecutionId);
                properties.Add("NavigationType", entry.NavigationType);
                properties.Add("ContentType", entry.ContentType);

                metrics.Add("FetchStart", entry.FetchStart);
                metrics.Add("LoadEventEnd", entry.LoadEventEnd);
                metrics.Add("NavigationStart", entry.NavigationStart);
                metrics.Add("DomContentLoadTime", entry.DomContentLoadTime);
                metrics.Add("DomParsingTime", entry.DomParsingTime);
                metrics.Add("RequestResponseTime", entry.RequestResponseTime);
                metrics.Add("PageRenderTime", entry.PageRenderTime);
                metrics.Add("NetworkLatency", entry.NetworkLatency);
                metrics.Add("RedirectTime", entry.RedirectTime);
                metrics.Add("RedirectCount", entry.RedirectCount);
                metrics.Add("PageLoadTime", entry.PageLoadTime);

                telemetry.TrackEvent(apiName, properties, metrics);
            }
            telemetry.Flush();
        }
    }
}
