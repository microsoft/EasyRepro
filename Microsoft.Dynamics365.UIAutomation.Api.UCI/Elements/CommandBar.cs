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

        public void ClickCommand(string name, string subname = "", bool moreCommands = false)
        {
            _client.ClickCommand(name, subname, moreCommands);
        }

        public void ClickRelatedGridCommand(string name, string subname = "", bool moreCommands = false)
        {
            _client.ClickRelatedGridCommand(name, subname, moreCommands);
        }

        public void ClickSubGridCommand(string name, string subname = "", bool moreCommands = false)
        {
            _client.ClickSubGridCommand(name, subname, moreCommands);
        }
    }
}
