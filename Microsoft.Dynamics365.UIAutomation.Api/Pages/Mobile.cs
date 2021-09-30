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
    public class Mobile
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mobile"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Mobile(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToDefaultContent();
        }

        [Obsolete("This method has been deprectead and is no longer supported.")]
        public BrowserCommandResult<bool> Open(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Mobile Page"), driver =>
            {
                Uri baseUri = new Uri(driver.Url);

                driver.Navigate().GoToUrl(baseUri.GetLeftPart(System.UriPartial.Authority) + "/m");

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Mobile.Page]), TimeSpan.FromSeconds(60),
                    e => driver.WaitForPageToLoad(),
                    "Mobile page failed to load."
                    ); 
                return true;
            });
        }
    }
}
