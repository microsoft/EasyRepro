// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class CustomerServiceCopilot : Element
    {
        private readonly WebClient _client;

        public CustomerServiceCopilot(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Clicks command on the command bar
        /// </summary>
        /// <param name="name">Name of button to click</param>
        /// <param name="subname">Name of button on submenu to click</param>
        /// <param name="subSecondName">Name of button on submenu (3rd level) to click</param>
        public void ClickButton(string name, string subname = null, string subSecondName = null)
        {
            _client.ClickCommand(name, subname, subSecondName);
        }



                    
        public BrowserCommandResult<bool> EnableAskAQuestion(int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.EnableAskAQuestion();
        }

        public BrowserCommandResult<string> AskAQuestion(string userInput, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.AskAQuestion(userInput);
        }
    }
}
