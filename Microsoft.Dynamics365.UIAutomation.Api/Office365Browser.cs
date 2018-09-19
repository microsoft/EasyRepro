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

<<<<<<< HEAD
        public LoginDialog LoginPage => this.GetPage<LoginDialog>();
=======
        public LoginPage LoginPage => this.GetPage<LoginPage>();
>>>>>>> origin/releases/v8.1

        public void GoToPortalHome()
        {
            this.Driver.Navigate().GoToUrl(Constants.DefaultLoginUri);
            this.Driver.WaitForPageToLoad();
        }

        #endregion Login

        #region Navigation

<<<<<<< HEAD
        public Office365Navigation Navigation => this.GetPage<Office365Navigation>();
=======
        public Office365NavigationPage Navigation => this.GetPage<Office365NavigationPage>();
>>>>>>> origin/releases/v8.1

        #endregion Navigation

        #region Instance Picker

<<<<<<< HEAD
        public Office365XrmInstancePicker XrmInstancePicker => this.GetPage<Office365XrmInstancePicker>();
=======
        public Office365XrmInstancePickerPage XrmInstancePicker => this.GetPage<Office365XrmInstancePickerPage>();
>>>>>>> origin/releases/v8.1

        #endregion Instance Picker
    }
}