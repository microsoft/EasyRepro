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
    public class XrmBrowser
        : InteractiveBrowser
    {
        #region Constructor(s)

        internal XrmBrowser(IWebDriver driver) : base(driver)
        {
        }

        public XrmBrowser(BrowserType type) : base(type)
        {
        }

        public XrmBrowser(BrowserOptions options) : base(options)
        {
            
        }

        #endregion Constructor(s)

        #region Login

        public LoginPage LoginPage => this.GetPage<LoginPage>();

        public void GoToXrmUri(Uri xrmUri)
        {
            this.Driver.Navigate().GoToUrl(xrmUri);
            this.Driver.WaitForPageToLoad();
        }

        #endregion Login

        #region Navigation

        public XrmNavigationPage Navigation => this.GetPage<XrmNavigationPage>();

        #endregion Navigation

        #region CommandBar

        public XrmCommandBarPage CommandBar => this.GetPage<XrmCommandBarPage>();

        #endregion CommandBar

        #region Performance Markers

        public XrmPerformanceCenterPage PerformanceCenter => this.GetPage<XrmPerformanceCenterPage>();

        #endregion Performance Markers

        #region Views

        public XrmGridPage Grid => this.GetPage<XrmGridPage>();

        #endregion Views

        #region DashBoards

        public XrmDashboardPage Dashboard => this.GetPage<XrmDashboardPage>();

        #endregion DashBoards

        #region Forms

        public XrmEntityPage Entity => this.GetPage<XrmEntityPage>();

        #endregion Forms

        #region Related

        public XrmRelatedGridPage Related => this.GetPage<XrmRelatedGridPage>();

        #endregion Related

        #region QuickCreate

        public XrmQuickCreatePage QuickCreate => this.GetPage<XrmQuickCreatePage>();

        #endregion QuickCreate

        #region LookupPage

        public XrmLookupPage Lookup => this.GetPage<XrmLookupPage>();

        #endregion LookupPage

        #region Guided Help

        public XrmGuidedHelpPage GuidedHelp => this.GetPage<XrmGuidedHelpPage>();

        #endregion Guided Help

        #region Notifications

        public XrmNotficationPage Notifications => this.GetPage<XrmNotficationPage>();

        #endregion Notifications

        #region Business Process Flow

        public XrmBusinessProcessFlow BusinessProcessFlow => this.GetPage<XrmBusinessProcessFlow>();

        #endregion Business Process Flow

        #region Dialog

        public XrmDialogPage Dialogs => this.GetPage<XrmDialogPage>();

        #endregion Dialog

        #region GlobalSearch

        public XrmGlobalSearchPage GlobalSearch => this.GetPage<XrmGlobalSearchPage>();

        #endregion GlobalSearch

        #region Document

        public XrmDocumentPage Document => this.GetPage<XrmDocumentPage>();

        #endregion Document

        #region ActivityFeed

        public XrmActivityFeedPage ActivityFeed => this.GetPage<XrmActivityFeedPage>();

        #endregion ActivityFeed

        #region Report

        public XrmReportPage Report => this.GetPage<XrmReportPage>();

        #endregion Report

        #region Mobile

        public XrmMobilePage Mobile => this.GetPage<XrmMobilePage>();

        #endregion Mobile

        #region Mobile

        public XrmProcessesPage Processes => this.GetPage<XrmProcessesPage>();

        #endregion Mobile

    }


}