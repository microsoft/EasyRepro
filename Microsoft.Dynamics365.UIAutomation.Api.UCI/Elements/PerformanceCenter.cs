// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class PerformanceCenter : Element
    {
        private readonly WebClient _client;

        public PerformanceCenter(WebClient client)
        {
            _client = client;
        }
        /// <summary>
        /// Enables Performance Center for UCI 
        /// </summary>
        public void Enable()
        {
            _client.EnablePerformanceCenter();
        }

        public void TrackEvents()
        {
            if (string.IsNullOrEmpty(_client.Browser.Options.AppInsightsKey)) throw new InvalidOperationException("The Application Insights key was not specified.  Please specify an Instrumentation key in the Browser Options.");

            _client.TrackPerformanceEvents();
        }
    }
}
