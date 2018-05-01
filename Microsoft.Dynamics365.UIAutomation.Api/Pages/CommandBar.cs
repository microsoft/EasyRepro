// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class CommandBarItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// Command bar page.
    /// </summary>
    public class CommandBar : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBar"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public CommandBar(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToDefault();
        }
    }
}
