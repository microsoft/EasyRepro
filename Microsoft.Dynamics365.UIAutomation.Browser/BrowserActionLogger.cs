// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public interface IBrowserActionLogger
    {
        void LogEvent(BrowserRecordingEvent @event);
        void LogEvents(IList<BrowserRecordingEvent> events);
    }

    public abstract class BrowserActionLogger : IBrowserActionLogger
    {
        public abstract void LogEvent(BrowserRecordingEvent @event);
        public abstract void LogEvents(IList<BrowserRecordingEvent> events);
    }

    public class InMemoryBrowserActionLogger : BrowserActionLogger
    {
        public InMemoryBrowserActionLogger()
        {
            this.Events = new List<BrowserRecordingEvent>();
        }

        public List<BrowserRecordingEvent> Events { get; private set; }

        public override void LogEvent(BrowserRecordingEvent @event)
        {
            this.Events.Add(@event);
        }

        public override void LogEvents(IList<BrowserRecordingEvent> events)
        {
            this.Events.AddRange(events);
        }
    }
}
