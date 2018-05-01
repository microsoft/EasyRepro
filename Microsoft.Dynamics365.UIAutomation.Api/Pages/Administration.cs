// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class Administration : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mobile"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Administration(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToContentFrame();
        }

        public BrowserCommandResult<bool> OpenFeature(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Feature"), driver =>
            {
                var container = driver.FindElement(By.ClassName("content"));
                var items = container.FindElements(By.TagName("a"));

                var itemExists = false;
                foreach (var item in items)
                {
                    if (String.Equals(item.Text, name, StringComparison.OrdinalIgnoreCase))
                    {
                        itemExists = true;
                        item.Click();
                    }
                }

                if (!itemExists) { throw new InvalidOperationException($"The Feature {name} does not exist.");}

                return true;
            });
        }
    }
}
