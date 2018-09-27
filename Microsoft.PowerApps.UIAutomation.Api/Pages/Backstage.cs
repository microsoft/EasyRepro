// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  The Home page.
    ///  </summary>
    public class Backstage
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Backstage"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Backstage(InteractiveBrowser browser)
            : base(browser)
        {
        }

        #region Sidebar
        public BackstageSidebar Sidebar => this.Browser.GetPage<BackstageSidebar>();
        #endregion

        #region AppSettings
        public BackStageAppSettings AppSettings => this.Browser.GetPage<BackStageAppSettings>();
        #endregion
    }
}
