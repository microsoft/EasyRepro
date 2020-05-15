// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class CommandBar : Element
    {
        private readonly WebClient _client;

        public CommandBar(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Clicks command on the command bar
        /// </summary>
        /// <param name="name">Name of button to click</param>
        /// <param name="subname">Name of button on submenu to click</param>
        /// <param name="subSecondName">Name of button on submenu (3rd level) to click</param>
        public void ClickCommand(string name, string subname = null, string subSecondName = null)
        {
            _client.ClickCommand(name, subname, subSecondName);
        }



        /// <summary>
        /// Returns the values of CommandBar objects
        /// </summary>
        /// <param name="includeMoreCommandsValues">Flag to determine whether values should be returned from the more commands menu</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmApp.CommandBar.GetCommandValues(true);</example>
        public BrowserCommandResult<List<string>> GetCommandValues(bool includeMoreCommandsValues = false, int thinkTime = Constants.DefaultThinkTime)
        {
            List<string> commandValues = new List<string>();
            commandValues = _client.GetCommandValues(includeMoreCommandsValues, thinkTime);

            return commandValues;
        }
    }
}
