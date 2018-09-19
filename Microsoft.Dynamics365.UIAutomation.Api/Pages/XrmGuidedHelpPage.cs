// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

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
                    this.Browser.Driver.ExecuteScript("return Xrm.Internal.isFeatureEnabled('FCB.GuidedHelp') && Xrm.Internal.isGuidedHelpEnabledForUser();").ToString(),
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
                    driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.GuidedHelp.MarsOverlay]), new TimeSpan(0, 0, 15), d =>
                    {
                        var allMarsElements = driver
                            .FindElement(By.XPath(Elements.Xpath[Reference.GuidedHelp.MarsOverlay]))
                            .FindElements(By.XPath(".//*"));

                        foreach (var element in allMarsElements)
                        {
                            var buttonId = driver.ExecuteScript("return arguments[0].id;", element).ToString();

                            if (buttonId.Equals(Elements.ElementId[Reference.GuidedHelp.Close], StringComparison.InvariantCultureIgnoreCase))
                            {
                                driver.WaitUntilClickable(By.Id(buttonId), new TimeSpan(0, 0, 5));

                                element.Click();
                            }
                        }

                        returnValue = true;
                    });
                }

                return returnValue;
            });
        }

        /// <summary>
        /// Closes the Welcome Tour
        /// </summary>
        public BrowserCommandResult<bool> CloseWelcomeTour()
        {
            return this.Execute(GetOptions("Close Welcome Tour"), driver =>
            {
                // Close the email and nav tour approval dialog if it's there - go top to bottom (reverse)
                foreach (var frame in driver.FindElements(By.TagName("iframe")).Reverse())
                {
                    // navigate down into the frame
                    driver.SwitchTo().Frame(frame);

                    // look for nav tour
                    if (driver.HasElement(By.XPath(Elements.Xpath[Reference.GuidedHelp.ButBegin])))
                    {
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.GuidedHelp.ButBegin]));

                        Thread.Sleep(1000);

                        driver.SwitchTo().ParentFrame();

                        continue;
                    }

                    if (driver.HasElement(By.XPath(Elements.Xpath[Reference.GuidedHelp.ButtonClose])))
                    {
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.GuidedHelp.ButtonClose]));

                        Thread.Sleep(1000);

                        driver.SwitchTo().ParentFrame();

                        continue;
                    }

                    driver.SwitchTo().ParentFrame();
                }

                return true;
            });
        }
    }
}