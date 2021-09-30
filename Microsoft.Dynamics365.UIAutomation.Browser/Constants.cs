// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public static partial class Constants
    {
        /// <summary>
        /// The default amount of time to wait for an operation to complete by the Selenium driver.
        /// </summary>
        public static readonly TimeSpan DefaultTimeout = new TimeSpan(0, 0, 30);

        /// <summary>
        /// The default amount of time to wait between retrying command executions if they fail.
        /// Value is expressed in miliseconds.
        /// </summary>
        public const int DefaultRetryDelay = 5000;

        /// <summary>
        /// The default number of retry attempts for a command execution if it fails.
        /// </summary>
        public const int DefaultRetryAttempts = 2;

        /// <summary>
        /// The default page to direct a user to if none other is specified.
        /// </summary>
        public const string DefaultLoginUri = "https://portal.office.com";

        /// <summary>
        /// The default tracing source for browser automation.
        /// </summary>
        public const string DefaultTraceSource = "BrowserAutomation";

        /// <summary>
        /// The default tracing source for browser automation.
        /// </summary>
        public const int DefaultThinkTime = 2000;

        /// <summary>
        /// Constants and defaults related to the InteractiveBrowser.
        /// </summary>
        public static class Browser
        {
            internal const string IESettingsRegistryHive = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main";
            internal const string IESettingsTabProcGrowthKey = "TabProcGrowth";

            public static class Recording
            {
                internal const string GetRecordedEventsCommand = "return Recorder.getEvents();";
                internal const string GlobalEventCollectionScript = "document.eventCollection = new Array();";
                internal const string CheckIfScriptExistsScript = @"return typeof Recorder === 'undefined'? 'false' : 'true';";
                internal const string CheckIfEventCollectionExistsScript = @"return typeof document.eventCollection  === 'undefined'? 'false' : 'true';";
                internal const string RemoveEventsScript = @"if (typeof Recorder !== undefined) { Recorder.removeEvents({0}); }";

                /// <summary>
                /// The default scan interval.
                /// </summary>
                public const short DefaultScanInterval = 250;
            }
        }

        public static class Tracing
        {
            /* InteractiveBrowser */
            public const int BrowserEventFiringEventId = 9000;
            public const int BrowserEventFiredEventId = 9001;

            public const int BrowserElementClickedEventId = 9011;
            public const int BrowserElementClickingEventId = 9010;
            public const int BrowserElementValueChangedEventId = 9013;
            public const int BrowserElementValueChangingEventId = 9012;
            public const int BrowserExceptionThrownEventId = 9050;
            public const int BrowserFindElementCompletedEventId = 9015;
            public const int BrowserFindingElementEventId = 9014;
            public const int BrowserNavigatedEventId = 9021;
            public const int BrowserNavigatedBackEventId = 9023;
            public const int BrowserNavigatedForwardEventId = 9025;
            public const int BrowserNavigatingEventId = 9020;
            public const int BrowserNavigatingBackEventId = 9022;
            public const int BrowserNavigatingForwardEventId = 9024;
            public const int BrowserScriptExecutedEventId = 9031;
            public const int BrowserScriptExecutingEventId = 9030;

            /* Commands */
            public const int CommandStartEventId = 10001;
            public const int CommandStopEventId = 10002;
            public const int CommandErrorEventId = 10003;
            public const int CommandRetryEventId = 10004;
        }

        public static class Xrm
        {
            public static readonly string[] XrmDomains =
            {
                ".crm.dynamics.com", ".crm2.dynamics.com", ".crm3.dynamics.com",
                ".crm4.dynamics.com", "crm5.dynamics.com", "crm6.dynamics.com", "crm7.dynamics.com",
                ".crm8.dynamics.com", ".crm9.dynamics.com", ".crm10.dynamics.com", ".crm11.dynamics.com",
                ".crm12.dynamics.com", ".crm15.dynamics.com", "portal.office.com", "crm2.crmlivetie.com", ".crm.microsoftdynamics.us", "portal.office365.us"
            };
        }

    }
}