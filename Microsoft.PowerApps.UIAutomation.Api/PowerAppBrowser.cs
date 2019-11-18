// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Microsoft.PowerApps.UIAutomation.Api
{
    /// <summary>
    /// Provides API methods to simulate user interaction with the Dynamics 365 application. 
    /// </summary>
    /// <seealso cref="Microsoft.Dynamics365.UIAutomation.Browser.InteractiveBrowser" />
    public class PowerAppBrowser : InteractiveBrowser
    {
        #region Constructor(s)

        internal PowerAppBrowser(IWebDriver driver) : base(driver)
        {
        }

        public PowerAppBrowser(BrowserType type) : base(type)
        {
        }

        public PowerAppBrowser(BrowserOptions options) : base(options)
        {

        }

        #endregion Constructor(s)

        #region Login

        public OnlineLogin OnlineLogin => this.GetPage<OnlineLogin>();

        public void GoToXrmUri(Uri xrmUri)
        {
            this.Driver.Navigate().GoToUrl(xrmUri);
            this.Driver.WaitForPageToLoad();
        }

        #endregion Login

        #region Navigation
        public Navigation Navigation => this.GetPage<Navigation>();
        #endregion

        #region TestFramework
        public TestFramework TestFramework => this.GetPage<TestFramework>();
        #endregion

        #region SideBar
        public SideBar SideBar => this.GetPage<SideBar>();
        #endregion

        #region HomePage
        public Home Home => this.GetPage<Home>();
        #endregion

        #region Apps
        public Apps Apps => this.GetPage<Apps>();
        #endregion

        #region Canvas
        public Canvas Canvas => this.GetPage<Canvas>();
        #endregion

        #region Player
        public Player Player => this.GetPage<Player>();
        #endregion

        #region Backstage
        public Backstage Backstage => this.GetPage<Backstage>();
        #endregion
        #region Sharepoint
        public Sharepoint Sharepoint => this.GetPage<Sharepoint>();
        #endregion
        #region ModelDrivenApps
        public ModelDrivenApps ModelDrivenApps => this.GetPage<ModelDrivenApps>();
        #endregion
        #region CommandBar
        public CommandBar CommandBar => this.GetPage<CommandBar>();
        #endregion
    }
}
