// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class PowerApp : Element
    {
        private readonly WebClient _client;
        public enum AppType { EmbeddedPowerApp, CustomPage }
        private string _appId;
        public PowerApp(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Sends Power FX command to Power App
        /// </summary>
        /// <param name="appId">Id of the app to select</param>
        /// <param name="command">command to execute</param>
        public void SendCommand(string appId, string command)
        {
            _client.PowerAppSendCommand(appId, command);
        }

        public void Select(string appId, string control)
        {
            _client.PowerAppSelect(appId, control);
        }

        public void SetProperty(string appId, string control, string value)
        {
            _client.PowerAppSetProperty(appId, control, value);
        }

    }
}
