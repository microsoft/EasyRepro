// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Xrm Report Page
    /// </summary>
    public class Report
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickCreate"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Report(InteractiveBrowser browser)
            : base(browser)
        {
            this.Browser.Driver.LastWindow();
        }

        /// <summary>
        /// Cancel the Report
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Report.Cancel();</example>
        public BrowserCommandResult<bool> Cancel(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Cancel"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Report.Close]));

                return true;
            });
        }

        /// <summary>
        /// Run the Report
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Report.RunReport(customaction);</example>
        public BrowserCommandResult<bool> RunReport(Action<ReportEventArgs> setFilterCriteriaAction, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Run Report"), driver =>
            {
                if (setFilterCriteriaAction != null)
                {
                    setFilterCriteriaAction?.Invoke(new ReportEventArgs( driver));
                }
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Report.RunReport]));

                return true;
            });
        }


    }
}
