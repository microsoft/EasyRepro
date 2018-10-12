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

                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Navigation.Sidebar]));

                //start with the sidebar html item
                var sidebar = driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.Sidebar]));

                //Get menu items
                var items = sidebar.FindElements(By.TagName("button"));

                foreach(var item in items)
                {
                    if(item.Text.Contains(menuItem,StringComparison.OrdinalIgnoreCase))
                    {
                        item.Click(true);
                        itemExists = true;
                        break;
                    }
                }

                if (!itemExists) throw new Exception($"The menu item with name: {menuItem} does not exist.");

                return true;
            });
        }

        /// <summary>
        /// Change between Canvas and Model Driven design modes
        /// </summary>
        /// <param name="designMode">Switch between Canvas or Model-driven design modes. If model-driven design mode isn't availalbe. You may need to create an environment.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> ChangeDesignMode(string designMode, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Change Design Mode"), driver =>
            {

                //For future implementation - if model driven design mode is not available, catch and throw error if it is the desired design mode
                //bool isModelDrivenModeAvailable = false;

                var buttonSwitchDesignMode = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Navigation.SwitchDesignMode]));

                var designModeText = buttonSwitchDesignMode.Text;
                
                if (!designModeText.Contains(designMode))
                {
                    buttonSwitchDesignMode.Click(true);

                    var button = buttonSwitchDesignMode.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.DesignModeButton].Replace("[NAME]", designMode)));

                    button.Click(true);

                }
                else
                {
                    return true;
                }

                return true;
            });
        }

        /// <summary>
        /// Opens the Menu
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> ExpandCollapse(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Expand/Collapse the Sidebar"), driver =>
            {

                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Navigation.Sidebar]));

                //start with the sidebar html item
                var sidebar = driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.Sidebar]));

                //Get menu items
                var items = sidebar.FindElements(By.TagName("button"));

                items[0].Click(true);

                return true;
            });
        }
    }
}
