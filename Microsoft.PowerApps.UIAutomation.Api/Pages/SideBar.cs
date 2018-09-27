// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  The Navigation page.
    ///  </summary>
    public class SideBar
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SideBar"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public SideBar(InteractiveBrowser browser)
            : base(browser)
        {
        }

        /// <summary>
        /// Opens the Menu
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> Navigate(string menuItem, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("SideBar Navigate"), driver =>
            {
                bool itemExists = false;

                driver.WaitUntilClickable(By.TagName("sidebar"));

                //start with the sidebar html item
                var sidebar = driver.FindElement(By.TagName("sidebar"));

                //Get menu items
                var items = driver.FindElements(By.TagName("sidebar-item"));


                foreach(var item in items)
                {
                    var name = item.GetAttribute("label");
                    
                    if (string.Equals(name, menuItem, StringComparison.OrdinalIgnoreCase))
                    {
                        item.Click();
                        itemExists = true;
                        break;
                    }
                }

                if (!itemExists) throw new Exception($"The menu item with name: {menuItem} does not exist.");

                return true;
            });
        }

        /// <summary>
        /// Opens the Menu
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> NavigateTIPApps(string menuItem, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("SideBar Navigate"), driver =>
            {
              

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.TIPApps]));

                /* THIS IS OLD CODE - RETAINING IN THE EVENT THE SIDEBAR IS RESTORED IN TIP
                 
                bool itemExists = false;
                 
                //start with the sidebar html item
                var sidebar = driver.FindElement(By.TagName("sidebar"));

                //Get menu items
                var items = driver.FindElements(By.TagName("sidebar-item"));


                foreach (var item in items)
                {
                    var name = item.GetAttribute("label");

                    if (string.Equals(name, menuItem, StringComparison.OrdinalIgnoreCase))
                    {
                        item.Click();
                        itemExists = true;
                        break;
                    }
                }

                if (!itemExists) throw new Exception($"The menu item with name: {menuItem} does not exist.");
                */


                return true;
            });
        }

        public BrowserCommandResult<bool> ExpandCollapse(int thinkTime = Constants.DefaultThinkTime)
        {
          
            return this.Navigate("", thinkTime);
        }
    }
}
