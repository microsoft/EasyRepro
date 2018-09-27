// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  The Home page.
    ///  </summary>
    public class BackstageSidebar
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Backstage"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public BackstageSidebar(InteractiveBrowser browser)
            : base(browser)
        {
        }

        public BrowserCommandResult<bool> Navigate(String buttonName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Click BackStage Navigate"), driver =>
            {
                //Find the container with the button
                var menuContainer = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BackStage.MenuContainer]),
                                                              $"The Backstage sidebar is not available.  Unable to Navigate to {buttonName}");

                var buttons = menuContainer.FindElements(By.TagName("li"));
                bool found = false;

                foreach (var button in buttons)
                {
                    if (button.Text.Equals(buttonName, StringComparison.OrdinalIgnoreCase))
                    {
                        button.Click();
                        found = true;
                    }

                }

                if (!found)
                    throw new Exception("Backstage Menu Item Not Found");

                if (buttonName == "Connections")
                {
                    Browser.ThinkTime(thinkTime);
                    driver.LastWindow();
                }

                return true;
            });
        }
    }
}
