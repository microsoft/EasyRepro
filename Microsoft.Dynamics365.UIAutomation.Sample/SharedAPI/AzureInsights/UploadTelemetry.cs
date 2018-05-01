using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Api;
using System.Security;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Sample.SharedAPI
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
        public void SharedTestUploadTelemetry()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
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
                    Dictionary<string, PerformanceMarker> perfResults = perf.GetMarkers().Value;

                    new Telemetry(System.Configuration.ConfigurationManager.AppSettings["AzureKey"], executionId, perf.GetRequestId())
                        .TrackEvents(perfResults.Select((KeyValuePair<string, PerformanceMarker> x) => x.Value).ToList());
                }
                new Telemetry(System.Configuration.ConfigurationManager.AppSettings["AzureKey"], executionId)
                    .TrackEvents(xrmBrowser.CommandResults);
            }
        }

        [TestMethod]
        public void UploadWindowsPerfResourceTimings()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.ThinkTime(3000);

                // uncomment this line if target env has initial warning dialog
                // xrmBrowser.Dialogs.CloseWarningDialog();

                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                var windowsPerf = xrmBrowser.WindowPerfResource;

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                var output = windowsPerf.GetWindowPerfResourceTimings();

                new Telemetry(System.Configuration.ConfigurationManager.AppSettings["AzureKey"], Guid.NewGuid().ToString())
                   .TrackEvents(output, "Navigation.OpenSubArea");

                // clear resource times
                windowsPerf.ClearResourceTimings();
                xrmBrowser.Navigation.OpenSubArea("Sales", "Contacts");
                var output2 = windowsPerf.GetWindowPerfResourceTimings();

                new Telemetry(System.Configuration.ConfigurationManager.AppSettings["AzureKey"], Guid.NewGuid().ToString())
                   .TrackEvents(output2, "Navigation.OpenSubArea");
            }
        }

        [TestMethod]
        public void UploadWindowsPerNavigationTimings()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

                xrmBrowser.ThinkTime(3000);
                
                xrmBrowser.GuidedHelp.CloseGuidedHelp();

                var windowsPerfNav = xrmBrowser.WindowPerfNavigation;

                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");
                var output = windowsPerfNav.GetWindowPerfNavigationTimings();

                new Telemetry(System.Configuration.ConfigurationManager.AppSettings["AzureKey"], Guid.NewGuid().ToString())
                   .TrackEvents(output, "Navigation.OpenSubArea");
            }
        }
    }
}
