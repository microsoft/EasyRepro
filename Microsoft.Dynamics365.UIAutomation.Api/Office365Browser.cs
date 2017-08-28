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

        public LoginPage LoginPage => this.GetPage<LoginPage>();

        public void GoToPortalHome()
        {
            this.Driver.Navigate().GoToUrl(Constants.DefaultLoginUri);
            this.Driver.WaitForPageToLoad();
        }

        #endregion Login

        #region Navigation

        public Office365NavigationPage Navigation => this.GetPage<Office365NavigationPage>();

        #endregion Navigation

        #region Instance Picker

        public Office365XrmInstancePickerPage XrmInstancePicker => this.GetPage<Office365XrmInstancePickerPage>();

        #endregion Instance Picker
    }
}