// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.Pages;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Provides API methods to simulate user interaction with the Dynamics 365 application. 
    /// </summary>
    /// <seealso cref="Microsoft.Dynamics365.UIAutomation.Browser.InteractiveBrowser" />
    public class Browser
        : InteractiveBrowser
    {
        #region Constructor(s)

        internal Browser(IWebDriver driver) : base(driver)
        {
        }

        public Browser(BrowserType type) : base(type)
        {
        }

        public Browser(BrowserOptions options) : base(options)
        {
            
        }

        #endregion Constructor(s)

        #region Login

        public LoginDialog LoginPage => this.GetPage<LoginDialog>();

        public void GoToXrmUri(Uri xrmUri)
        {
            this.Driver.Navigate().GoToUrl(xrmUri);
            this.Driver.WaitForPageToLoad();
        }

        #endregion Login

        #region Navigation

        public Navigation Navigation => this.GetPage<Navigation>();

        #endregion Navigation

        #region CommandBar

        public CommandBar CommandBar => this.GetPage<CommandBar>();

        #endregion CommandBar

        #region Performance Markers

        public PerformanceCenter PerformanceCenter => this.GetPage<PerformanceCenter>();

        public WindowPerformanceResource WindowPerfResource => this.GetPage<WindowPerformanceResource>();

        public WindowPerformanceNavigation WindowPerfNavigation => this.GetPage<WindowPerformanceNavigation>();

        #endregion Performance Markers

        #region Views

        public Grid Grid => this.GetPage<Grid>();

        #endregion Views

        #region DashBoards

        public Dashboard Dashboard => this.GetPage<Dashboard>();

        #endregion DashBoards

        #region Forms

        public Entity Entity => this.GetPage<Entity>();

        #endregion Forms

        #region Related

        public RelatedGrid Related => this.GetPage<RelatedGrid>();

        #endregion Related

        #region QuickCreate

        public QuickCreate QuickCreate => this.GetPage<QuickCreate>();

        #endregion QuickCreate

        #region LookupPage

        public Lookup Lookup => this.GetPage<Lookup>();

        #endregion LookupPage

        #region Guided Help

        public GuidedHelp GuidedHelp => this.GetPage<GuidedHelp>();

        #endregion Guided Help

        #region Notifications

        public Notfication Notifications => this.GetPage<Notfication>();

        #endregion Notifications

        #region Business Process Flow

        public BusinessProcessFlow BusinessProcessFlow => this.GetPage<BusinessProcessFlow>();

        #endregion Business Process Flow

        #region Dialog

        public Dialog Dialogs => this.GetPage<Dialog>();

        #endregion Dialog

        #region GlobalSearch

        public GlobalSearch GlobalSearch => this.GetPage<GlobalSearch>();

        #endregion GlobalSearch

        #region Document

        public Document Document => this.GetPage<Document>();

        #endregion Document

        #region ActivityFeed

        public ActivityFeed ActivityFeed => this.GetPage<ActivityFeed>();

        #endregion ActivityFeed

        #region Report

        public Report Report => this.GetPage<Report>();

        #endregion Report

        #region Mobile

        public Mobile Mobile => this.GetPage<Mobile>();

        #endregion Mobile

        #region Processes

        public Processes Processes => this.GetPage<Processes>();

        #endregion Processes

        #region Administration

        public Administration Administration => this.GetPage<Administration>();

        #endregion Administration

        #region Office365

        public Office365 Office365 => this.GetPage<Office365>();

        public Office365Navigation Office365Navigation => this.GetPage<Office365Navigation>();

        #endregion Office365
    }


}