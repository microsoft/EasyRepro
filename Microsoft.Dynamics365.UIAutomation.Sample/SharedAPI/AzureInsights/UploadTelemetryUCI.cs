// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Dynamics365.UIAutomation.Sample.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.SharedAPI
{
    [TestClass]
    public class UploadTelemetryUCI : TestsBase
    {
        private readonly string _azureKey = System.Configuration.ConfigurationManager.AppSettings["AzureKey"];

        [TestMethod]
        public void SharedTestUploadTelemetryUCI()
        {
            //Setting the options here to demonstrate what needs to 
            //be set for tracking performance telemetry in UCI
            var options = TestSettings.Options;
            options.AppInsightsKey = _azureKey;
            options.UCIPerformanceMode = true;
            
            var client = new WebClient(options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                //Track Performance Center Telemetry
                //----------------------------------------------------------------
                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");
                xrmApp.Telemetry.TrackPerformanceEvents();
                //----------------------------------------------------------------
                            

                //Track Performance Center Telemetry with additional data
                //Add additional information to telemtry to specify which entity was opened
                //----------------------------------------------------------------
                xrmApp.Grid.OpenRecord(0);

                var props = new Dictionary<string, string>();
                props.Add("Entity", "Account"); 

                var metrics = new Dictionary<string, double>();
                var commandResult = xrmApp.CommandResults.FirstOrDefault(x => x.CommandName == "Open Grid Record");

                metrics.Add(commandResult.CommandName, commandResult.ExecutionTime);

                xrmApp.Telemetry.TrackPerformanceEvents(props, metrics);
                //----------------------------------------------------------------

                //Track Command Results Events 
                //----------------------------------------------------------------
                xrmApp.Telemetry.TrackCommandEvents();
                //----------------------------------------------------------------

                //Track Browser Performance Telemetry
                //----------------------------------------------------------------
                xrmApp.Telemetry.TrackBrowserEvents(Api.UCI.Telemetry.BrowserEventType.Resource, null, true);
                xrmApp.Telemetry.TrackBrowserEvents(Api.UCI.Telemetry.BrowserEventType.Navigation);
                xrmApp.Telemetry.TrackBrowserEvents(Api.UCI.Telemetry.BrowserEventType.Server);
                //----------------------------------------------------------------

            }
        }
    }
}