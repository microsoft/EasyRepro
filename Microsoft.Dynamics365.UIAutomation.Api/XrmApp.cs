// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class XrmApp : IDisposable
    {
        internal WebClient _client;

        public List<ICommandResult> CommandResults => _client.CommandResults;

        public XrmApp(WebClient client)
        {
            _client = client;
            
        }

        public XrmApp(BrowserOptions options)
        {
            _client = new WebClient(options);
        }

        public OnlineLogin OnlineLogin => this.GetArea<OnlineLogin>(_client);
        public Navigation Navigation => this.GetArea<Navigation>(_client);
        public CommandBar CommandBar => this.GetArea<CommandBar>(_client);
        public Grid Grid => this.GetArea<Grid>(_client);
        public CustomerServiceCopilot CustomerServiceCopilot => this.GetArea<CustomerServiceCopilot>(_client);
        public PowerApp PowerApp => this.GetArea<PowerApp>(_client);
        public Entity Entity => this.GetArea<Entity>(_client);
        public Dialogs Dialogs => this.GetArea<Dialogs>(_client);
        public Timeline Timeline => this.GetArea<Timeline>(_client);
        public BusinessProcessFlow BusinessProcessFlow => this.GetArea<BusinessProcessFlow>(_client);
        public Dashboard Dashboard => this.GetArea<Dashboard>(_client);
        public RelatedGrid RelatedGrid => this.GetArea<RelatedGrid>(_client);

        public GlobalSearch GlobalSearch => this.GetArea<GlobalSearch>(_client);
		public QuickCreate QuickCreate => this.GetArea<QuickCreate>(_client);
        public Lookup Lookup => this.GetArea<Lookup>(_client);
        public Telemetry Telemetry => this.GetArea<Telemetry>(_client);

        public T GetArea<T>(WebClient client)
            //where T : IElement
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { client });
        }

        public void ThinkTime(int milliseconds)
        {
            Trace.TraceInformation("XrmApp.ThinkTime initiated.");
            _client.ThinkTime(milliseconds);
        }
        public void ThinkTime(TimeSpan timespan)
        {
            Trace.TraceInformation("XrmApp.ThinkTime initiated.");
            _client.ThinkTime((int)timespan.TotalMilliseconds);
        }

        public void Dispose()
        {
            Trace.TraceInformation("XrmApp.Dispose initiated.");
            _client.Dispose();
        }
    }
}
