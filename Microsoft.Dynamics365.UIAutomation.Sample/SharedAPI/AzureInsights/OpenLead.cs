// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.SharedAPI
{
    [TestClass]
    public class OpenLeadforAzure
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());
        private readonly string _azureKey = System.Configuration.ConfigurationManager.AppSettings["AzureKey"].ToString();

        [TestMethod]
        public void SharedTestOpenActiveLead()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();
                
                xrmBrowser.Navigation.OpenSubArea("Sales", "Leads");
                
                xrmBrowser.Grid.SwitchView("All Leads");
                
                xrmBrowser.Grid.OpenRecord(0);


                var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
                telemetry.InstrumentationKey = _azureKey;


                foreach (ICommandResult result in xrmBrowser.CommandResults)
                {
                    
                    var properties = new Dictionary<string, string>();
                    var metrics = new Dictionary<string, double>();

                    properties.Add("StartTime", result.StartTime.Value.ToLongDateString());
                    properties.Add("EndTime", result.StopTime.Value.ToLongDateString());

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
}