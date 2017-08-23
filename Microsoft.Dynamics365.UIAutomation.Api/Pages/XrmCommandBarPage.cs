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
    public class XrmCommandBarItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// Command bar page.
    /// </summary>
    public class XrmCommandBarPage : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XrmCommandBarPage"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public XrmCommandBarPage(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToDefault();
        }


        /// <summary>
        /// Gets the Commands
        /// </summary>
        /// <param name="moreCommands">The MoreCommands</param>
        /// <example></example>
        private BrowserCommandResult<ReadOnlyCollection<IWebElement>> GetCommands(bool moreCommands = false)
        {
            return this.Execute("Get Command Bar Buttons", driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.CommandBar.RibbonManager]),new TimeSpan(0,0,5));

                IWebElement ribbon = null;
                if (moreCommands)
                    ribbon = driver.FindElement(By.XPath(Elements.Xpath[Reference.CommandBar.List]));
                else
                    ribbon = driver.FindElement(By.XPath(Elements.Xpath[Reference.CommandBar.RibbonManager]));

                var items = ribbon.FindElements(By.TagName("li"));

                return items;//.Where(item => item.Text.Length > 0).ToDictionary(item => item.Text, item => item.GetAttribute("id"));
            });
        }

        /// <summary>
        /// Clicks the  Command
        /// </summary>
        /// <param name="name">The Name of the command</param>
        /// <param name="subName">The SubName</param>
        /// <param name="moreCommands">The MoreCommands</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.CommandBar.ClickCommand("New");</example>
        public BrowserCommandResult<bool> ClickCommand(string name, string subName = "", bool moreCommands = false, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Click Command"), driver =>
            {
                if (this.Browser.Options.BrowserType == BrowserType.Firefox)
                    Thread.Sleep(5000);

                if (moreCommands)
                    driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.CommandBar.MoreCommands]));
               
                var buttons = GetCommands(moreCommands).Value;
                IWebElement button;
                if (this.Browser.Options.BrowserType == BrowserType.Firefox)
                {
                    button = buttons.FirstOrDefault(x => x.Text.Split('\r')[0].ToLowerString() == name.ToLowerString());
                }
                else
                {
                    button = buttons.FirstOrDefault(x => x.Text.ToLowerString() == name.ToLowerString());
                }

                if (string.IsNullOrEmpty(subName))
                {
                    if (button != null)
                    {
                        button.Click();
                    }
                    else
                    {
                        throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");
                    }
                }

                else
                {
                    button?.FindElement(By.ClassName(Elements.CssClass[Reference.CommandBar.FlyoutAnchorArrow])).Click();

                    var flyoutId = button?.GetAttribute("id").Replace("|", "_").Replace(".", "_") + "Menu";
                    var subButtons = driver.FindElement(By.Id(flyoutId)).FindElements(By.TagName("li"));
                    subButtons.FirstOrDefault(x => x.Text.ToLowerString() == subName.ToLowerString())?.Click();
                }

                driver.WaitForPageToLoad();
                return true;
            });
        }
    }
}
