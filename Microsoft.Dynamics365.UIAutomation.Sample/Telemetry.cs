using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Api;

namespace Microsoft.Dynamics365.UIAutomation
{
    public class Telemetry
    {
        private string _key = "";
        private string _executionId = "";
        private string _requestId = "";
        
        public Telemetry AzureKey(string key) { _key = key;  return this;}
        public Telemetry ExecutionId(string id) { _executionId = id; return this; }
        public Telemetry RequestId(string id) { _requestId = id; return this; }

        public void TrackEvents(List<XrmPerformanceMarker> results)
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient {InstrumentationKey = _key};
            
            var properties = new Dictionary<string, string>();
            var metrics = new Dictionary<string, double>();

            properties.Add("ExecutionId", _executionId);
            properties.Add("RequestId", _requestId);

            foreach (XrmPerformanceMarker result in results)
            {
                if (metrics.ContainsKey(result.Name))
                {
                    var existingValue = metrics[result.Name];
                    if (existingValue < (double)result.ExecutionTime)
                        metrics[result.Name] = (double)result.ExecutionTime;
                }
                else
                    metrics.Add(result.Name, (double)result.ExecutionTime);
            }

            telemetry.TrackEvent("Performance Markers", properties, metrics);

            telemetry.Flush();
        }
        public void TrackEvents( List<ICommandResult> results)
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient {InstrumentationKey = _key};

            foreach (ICommandResult result in results)
            {

                var properties = new Dictionary<string, string>();
                var metrics = new Dictionary<string, double>();

                properties.Add("StartTime", result.StartTime.Value.ToLongDateString());
                properties.Add("EndTime", result.StopTime.Value.ToLongDateString());
                properties.Add("ExecutionId", _executionId);

                metrics.Add("ThinkTime", result.ThinkTime);
                metrics.Add("TransitionTime", result.TransitionTime);
                metrics.Add("ExecutionTime", result.ExecutionTime);
                metrics.Add("ExecutionAttempts", result.ExecutionAttempts);

                telemetry.TrackEvent(result.CommandName, properties, metrics);

            }

            telemetry.Flush();
        }
    }
}
