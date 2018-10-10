// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    /// Xrm Page
    /// </summary>
    public class AppPage : BrowserPage
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AppPage"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public AppPage(InteractiveBrowser browser) : base(browser)
        {
        }

        internal BrowserCommandOptions GetOptions(string commandName)
        {
            return new BrowserCommandOptions(Constants.DefaultTraceSource,
                commandName,
                0,
                0,
                null,
                true,
                typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }
        
    }
     
}
