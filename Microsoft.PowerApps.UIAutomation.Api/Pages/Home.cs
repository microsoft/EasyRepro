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
    public class Home
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Home(InteractiveBrowser browser)
            : base(browser)
        {
        }

        /// <summary>
        /// Opens Items from the Home page
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> MakeApp(string AppName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("MakeApp: " + AppName), driver =>
            {
                //Find the container with the list of apps
                var container = driver.FindElements(By.ClassName("try-apps-section"))[0];

                //Find the apps on the home page
                var tiles = container.FindElements(By.ClassName("sample-name"));

                //Navigate to the requested button
                foreach (var tile in tiles)
                {
                    if(tile.Text == AppName)
                    {
                        //We found our App.  Move up one level to find the right button
                        var parent = tile.FindElement(By.XPath("../.."));

                        var button = parent.FindElement(By.TagName("button"));

                        //Hover over the tile
                        tile.Hover(driver, true);
                        //Click the button
                        button.Click();
                    }
                }

                //Wait for the main page to render
                driver.LastWindow();
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Canvas.CanvasMainPage]));

                return true;
            });
        }
    }
}
