// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class CommandBarItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// Command bar page.
    /// </summary>
    public class CommandBar : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBar"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public CommandBar(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToDefault();
        }

        /// <summary>
        /// Returns the values of CommandBar objects
        /// </summary>
        /// <param name="moreCommands">The moreCommands</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Related.ClickCommand("ADD NEW CASE");</example>
        public BrowserCommandResult<List<string>> GetCommandValues(bool includeMoreCommandsValues = false, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Get CommandBar Command Count"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.CommandBar.RibbonManager]), new TimeSpan(0, 0, 5));

                List<string> commandValues = new List<string>();

                var retrieveCommandValues = GetCommands(false).Value;

                foreach (var value in retrieveCommandValues)
                {
                    if (value.Text != "")
                    {
                        string commandText = value.Text.ToString();
                        string trimmedCommandText = commandText.Substring(0, commandText.LastIndexOf("\r\n"));

                        commandValues.Add(trimmedCommandText);
                    }
                }

                if (includeMoreCommandsValues)
                {
                    driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.CommandBar.MoreCommands]));

                    var retrieveMoreCommandsValues = GetCommands(true).Value;

                    foreach (var value in retrieveMoreCommandsValues)
                    {
                        if (value.Text != "")
                        {
                            string commandText = value.Text.ToString();
                            string trimmedCommandText = commandText.Substring(0, commandText.LastIndexOf("\r\n")).ToUpper();

                            commandValues.Add(trimmedCommandText);
                        }
                    }
                }

                return commandValues;
            });
        }
    }
}
