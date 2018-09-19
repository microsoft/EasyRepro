using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Api;
using System.Security;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class UploadTelemetry
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager
            .AppSettings["OnlineUsername"]
            .ToSecureString();

        private readonly SecureString _password = System.Configuration.ConfigurationManager
            .AppSettings["OnlinePassword"]
            .ToSecureString();

        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"]
            .ToString());

        [TestMethod]
        public void TestUploadTelemetry()
        {
            using (var xrmBrowser = new XrmBrowser(TestSettings.Options))
            {
                string executionId = Guid.NewGuid().ToString();

                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                var perf = xrmBrowser.PerformanceCenter;

                if (!perf.IsEnabled)
                    perf.IsEnabled = true;

                xrmBrowser.Navigation.OpenSubArea("Sales", "Contacts");

                xrmBrowser.Grid.SwitchView("Active Contacts");

                var items = xrmBrowser.Grid.GetGridItems().Value;

                foreach (var item in items)
                {
                    xrmBrowser.Entity.OpenEntity(item.Url);
                    xrmBrowser.ThinkTime(2000);
                    Dictionary<string, XrmPerformanceMarker> perfResults = perf.GetMarkers().Value;

                    new Telemetry().AzureKey("[AZURE KEY GOES HERE]")
                        .ExecutionId(executionId)
                        .RequestId(perf.GetRequestId())
                        .TrackEvents(perfResults.Select(x => x.Value).ToList());

                }
                new Telemetry().AzureKey("[AZURE KEY GOES HERE]")
                    .ExecutionId(executionId)
                    .TrackEvents(xrmBrowser.CommandResults);

            }

        }
    }
}
