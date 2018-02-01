// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Xrm Guided Help Page
    /// </summary>
    public class XrmGuidedHelpPage
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XrmGuidedHelpPage"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public XrmGuidedHelpPage(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToDefault();
        }

        public bool IsEnabled
        {
            get
            {
                bool isGuidedHelpEnabled = false;
                bool.TryParse(
                    this.Browser.Driver.ExecuteScript("return Xrm.Internal.isGuidedHelpEnabledForUser();").ToString(),
                    out isGuidedHelpEnabled);

                return isGuidedHelpEnabled;
            }
        }

        /// <summary>
        /// Closes the Guided Help
        /// </summary>
        /// <example>xrmBrowser.GuidedHelp.CloseGuidedHelp();</example>
        public BrowserCommandResult<bool> CloseGuidedHelp()
        {
            return this.Execute(GetOptions("Close Guided Help"), driver =>
            {
                bool returnValue = false;

                if (IsEnabled)
                {
                    driver.WaitUntilVisible(By.Id(Reference.GuidedHelp.MarsOverlay), new TimeSpan(0, 0, 15), d =>
                    {
                        driver.SwitchTo().Frame("InlineDialog_Iframe");
                        driver.WaitUntilClickable(By.Id(Reference.GuidedHelp.ButtonClose), new TimeSpan(0, 0, 5));
                        var e = driver.FindElement(By.Id(Reference.GuidedHelp.ButtonClose));
                        e.Click(true);
                        driver.SwitchTo().DefaultContent();

                        returnValue = true;
                    });
                }

                return returnValue;
            });
        }
    }
}