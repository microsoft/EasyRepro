// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class Office365Browser
        : InteractiveBrowser
    {
        #region Constructor(s)

        public Office365Browser(IWebDriver driver) : base(driver)
        {
        }

        public Office365Browser(BrowserType type) : base(type)
        {
        }

        public Office365Browser(BrowserOptions options) : base(options)
        {
        }

        #endregion Constructor(s)

        #region Login

        public LoginDialog LoginPage => this.GetPage<LoginDialog>();

        public void GoToPortalHome()
        {
            this.Driver.Navigate().GoToUrl(Constants.DefaultLoginUri);
            this.Driver.WaitForPageToLoad();
        }

        #endregion Login

        #region Navigation

        public Office365Navigation Navigation => this.GetPage<Office365Navigation>();

        #endregion Navigation

        #region Instance Picker

        public Office365XrmInstancePicker XrmInstancePicker => this.GetPage<Office365XrmInstancePicker>();

        #endregion Instance Picker
    }
}