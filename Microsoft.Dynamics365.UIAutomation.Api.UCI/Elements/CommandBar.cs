// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

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
        public void ClickCommand(string name, string subname = "", bool moreCommands = false)
        {
            _client.ClickCommand(name, subname, moreCommands);
        }


    }
}
