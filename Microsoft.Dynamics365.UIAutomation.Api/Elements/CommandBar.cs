// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class CommandBar
    {
        #region DTO
        public class CommandBarReference
        {
            public const string CommandBar = "CommandBar";
            #region private
            private string _Container = ".//ul[contains(@data-lp-id,\"commandbar-Form\")]";
            private string _ContainerGrid = "//ul[contains(@data-lp-id,\"commandbar-HomePageGrid\")]";
            private string _MoreCommandsMenu = "//*[@id=\"__flyoutRootNode\"]";
            private string _Button = "//*[contains(text(),'[NAME]')]";
            #endregion
            #region prop
            public string Container { get => _Container; set { _Container = value; } }
            public string ContainerGrid { get => _ContainerGrid; set { _ContainerGrid = value; } }
            public string MoreCommandsMenu { get => _MoreCommandsMenu; set { _MoreCommandsMenu = value; } }
            public string Button { get => _Button; set { _Button = value; } }
            #endregion
        }
        #endregion
        private readonly WebClient _client;
        private Entity.EntityReference _entityReference;
        #region ctor
        public CommandBar(WebClient client) : base()
        {
            _client = client;
            _entityReference = new Entity.EntityReference();
        }
        #endregion

        //public void ClickCommand(string name, string subname = null, string subSecondName = null)
        //{
        //    _client.ClickCommand(name, subname, subSecondName);
        //}



        /// <summary>
        /// Returns the values of CommandBar objects
        /// </summary>
        /// <param name="includeMoreCommandsValues">Flag to determine whether values should be returned from the more commands menu</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmApp.CommandBar.GetCommandValues(true);</example>
        //public BrowserCommandResult<List<string>> GetCommandValues(bool includeMoreCommandsValues = false, int thinkTime = Constants.DefaultThinkTime)
        //{
        //    List<string> commandValues = new List<string>();
        //    commandValues = _client.GetCommandValues(includeMoreCommandsValues, thinkTime);

        //    return commandValues;
        //}

        #region public Browser Commands
        #region CommandBar
        /// <summary>
        /// Clicks command on the command bar
        /// </summary>
        /// <param name="name">Name of button to click</param>
        /// <param name="subname">Name of button on submenu to click</param>
        /// <param name="subSecondName">Name of button on submenu (3rd level) to click</param>
        public BrowserCommandResult<bool> ClickCommand(string name, string subname = null, string subSecondName = null, int thinkTime = Constants.DefaultThinkTime)
        {
            Trace.TraceInformation("CommandBar.ClickCommand for command " + name);
            return _client.Execute(_client.GetOptions($"Click Command"), driver =>
            {
                // Find the button in the CommandBar
                IElement ribbon;
                // Checking if any dialog is active
                if (driver.HasElement(string.Format(_client.ElementMapper.DialogsReference.DialogContext)))
                {
                    Trace.TraceInformation("CommandBar.ClickCommand: Found in Dialog.");
                    var dialogContainer = driver.FindElement(string.Format(_client.ElementMapper.DialogsReference.DialogContext));
                    ribbon = driver.WaitUntilAvailable(_client.ElementMapper.DialogsReference.DialogContext + _client.ElementMapper.CommandBarReference.Container);
                }
                else
                {
                    ribbon = driver.WaitUntilAvailable(_client.ElementMapper.CommandBarReference.Container);
                }


                if (ribbon == null)
                {
                    ribbon = driver.WaitUntilAvailable(_client.ElementMapper.CommandBarReference.ContainerGrid,
                        TimeSpan.FromSeconds(5),
                        "Unable to find the ribbon.");
                    ribbon.Click(_client);
                }

                //Is the button in the ribbon?
                
                if (driver.HasElement(_client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name)))
                {
                    Trace.TraceInformation("CommandBar.ClickCommand: Found in SubGrid.");
                    var command = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name));
                    command.Click(_client);
                    driver.Wait();
                }
                else
                {
                    //Is the button in More Commands?
                    if (driver.HasElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowButton))
                    {
                        Trace.TraceInformation("CommandBar.ClickCommand: Searching in RelatedGrid.");
                        // Click More Commands
                        var moreCommands = driver.FindElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowButton);
                        moreCommands.Click(_client);
                        driver.Wait();

                        //Click the button
                        var flyOutMenu = driver.WaitUntilAvailable(_client.ElementMapper.RelatedGridReference.CommandBarFlyoutButtonList);
                        if (driver.HasElement(_client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name)))
                        {
                            Trace.TraceInformation("CommandBar.ClickCommand: Found in RelatedGrid.");
                            var overflowCommand = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name));
                            overflowCommand.Click(_client);
                            driver.Wait();
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar or the flyout menu.");
                    }
                    else if (driver.HasElement(_client.ElementMapper.EntityReference.MoreCommands))
                    {
                        Trace.TraceInformation("CommandBar.ClickCommand: Searching in MoreCommands.");
                        var moreCommands = driver.FindElement(_client.ElementMapper.EntityReference.MoreCommands);
                        moreCommands.Click(_client);
                        driver.Wait();

                        //Click the button
                        var flyOutMenu = driver.WaitUntilAvailable(_client.ElementMapper.RelatedGridReference.CommandBarFlyoutButtonList); ;
                        if (driver.HasElement(_client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name)))
                        {
                            Trace.TraceInformation("CommandBar.ClickCommand: Found in MoreCommands.");
                            var overflowCommand = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name));
                            overflowCommand.Click(_client);
                            driver.Wait();
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar or the flyout menu.");
                    }
                    else
                        throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");
                }

                if (!string.IsNullOrEmpty(subname))
                {
                    Trace.TraceInformation("CommandBar.ClickCommand: Searching SubCommands in MoreCommands.");
                    var submenu = driver.WaitUntilAvailable(_client.ElementMapper.CommandBarReference.MoreCommandsMenu);


                    if (driver.FindElement(_client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", subname)) != null)
                    {
                        driver.FindElement(_client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", subname)).Click(_client);
                    }
                    else
                        throw new InvalidOperationException($"No sub command with the name '{subname}' exists inside of Commandbar.");

                    if (!string.IsNullOrEmpty(subSecondName))
                    {
                        var subSecondmenu = driver.WaitUntilAvailable(_client.ElementMapper.CommandBarReference.MoreCommandsMenu);

                        var subSecondbutton = driver.FindElement(_client.ElementMapper.CommandBarReference.MoreCommandsMenu + 
                            _client.ElementMapper.SubGridReference.SubGridOverflowButton
                                .Replace("[NAME]", subSecondName));

                        if (subSecondbutton != null)
                        {
                            subSecondbutton.Click(_client);
                        }
                        else
                            throw new InvalidOperationException($"No sub command with the name '{subSecondName}' exists inside of Commandbar.");
                    }
                }

                driver.Wait();

                return true;
            });
        }


        /// <summary>
        /// Returns the values of CommandBar objects
        /// </summary>
        /// <param name="includeMoreCommandsValues">Whether or not to check the more commands overflow list</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmApp.CommandBar.GetCommandValues();</example>
        public BrowserCommandResult<List<string>> GetCommandValues(bool includeMoreCommandsValues = false, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return this._client.Execute(_client.GetOptions("Get CommandBar Command Count"), driver => TryGetCommandValues(includeMoreCommandsValues, driver));
        }

        private List<string> TryGetCommandValues(bool includeMoreCommandsValues, IWebBrowser driver)
        {
            const string moreCommandsLabel = "more commands";

            //Find the button in the CommandBar
            IElement ribbon = GetRibbon(driver);

            //Get the CommandBar buttons
            Dictionary<string, IElement> commandBarItems = GetMenuItems(driver, ribbon);
            bool hasMoreCommands = commandBarItems.TryGetValue(moreCommandsLabel, out var moreCommandsButton);
            if (includeMoreCommandsValues && hasMoreCommands)
            {
                moreCommandsButton.Click(_client);

                var menu = driver.WaitUntilAvailable(_client.ElementMapper.CommandBarReference.MoreCommandsMenu);
                if (menu == null) { throw new KeyNotFoundException("More Commands Not Found. XPath: " + _client.ElementMapper.CommandBarReference.MoreCommandsMenu); }
                AddMenuItems(driver, menu, commandBarItems);

            }

            var result = GetCommandNames(commandBarItems.Values);
            return result;
        }
        private static void AddMenuItems(IWebBrowser browser, IElement menu, Dictionary<string, IElement> dictionary)
        {
            var menuItems = browser.FindElements(menu.Locator + "//li");
            foreach (var item in menuItems)
            {
                string key = item.Text.ToLowerString();
                if (dictionary.ContainsKey(key))
                    continue;
                dictionary.Add(key, item);
            }
        }
        private static Dictionary<string, IElement> GetMenuItems(IWebBrowser browser, IElement menu)
        {
            var result = new Dictionary<string, IElement>();
            AddMenuItems(browser, menu, result);
            return result;
        }

        private static List<string> GetCommandNames(IEnumerable<IElement> commandBarItems)
        {
            var result = new List<string>();
            foreach (var value in commandBarItems)
            {
                string commandText = value.Text.Trim();
                if (string.IsNullOrWhiteSpace(commandText))
                    continue;

                if (commandText.Contains("\r\n"))
                {
                    commandText = commandText.Substring(0, commandText.IndexOf("\r\n", StringComparison.Ordinal));
                }
                result.Add(commandText);
            }
            return result;
        }

        private IElement GetRibbon(IWebBrowser driver)
        {
            var xpathCommandBarContainer = _client.ElementMapper.CommandBarReference.Container;
            var xpathCommandBarGrid = _client.ElementMapper.CommandBarReference.ContainerGrid;

            IElement ribbon =
                driver.WaitUntilAvailable(xpathCommandBarContainer, new TimeSpan(0,0,5), null) ??
                driver.WaitUntilAvailable(xpathCommandBarGrid, new TimeSpan(0, 0, 5), null) ??
                throw new InvalidOperationException("Unable to find the ribbon.");

            return ribbon;
        }

        internal BrowserCommandResult<bool> CloseOpportunity(bool closeAsWon, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            var xPathQuery = closeAsWon
                ? _entityReference.CloseOpportunityWin
                : _entityReference.CloseOpportunityLoss;

            return _client.Execute(_client.GetOptions($"Close Opportunity"), driver =>
            {
                var closeBtn = driver.WaitUntilAvailable(xPathQuery, "Opportunity Close Button is not available");

                closeBtn?.Click(_client);
                driver.WaitUntilAvailable(_client.ElementMapper.DialogsReference.CloseOpportunity.Ok);
                Dialogs dialogs = new Dialogs(_client);
                dialogs.CloseOpportunityDialog(true);

                return true;
            });
        }

        internal BrowserCommandResult<bool> CloseOpportunity(double revenue, DateTime closeDate, string description, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Close Opportunity"), driver =>
            {
                //SetValue(IElements.IElementId[AppReference.Dialogs.CloseOpportunity.ActualRevenueId], revenue.ToString(CultureInfo.CurrentCulture));
                //SetValue(IElements.IElementId[AppReference.Dialogs.CloseOpportunity.CloseDateId], closeDate);
                //SetValue(IElements.IElementId[AppReference.Dialogs.CloseOpportunity.DescriptionId], description);

                driver.ClickWhenAvailable(_client.ElementMapper.DialogsReference.CloseOpportunity.Ok,
    TimeSpan.FromSeconds(5),
    "The Close Opportunity dialog is not available."
    );

                return true;
            });
        }

        internal BrowserCommandResult<bool> Delete(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Delete Entity"), driver =>
            {
                var deleteBtn = driver.WaitUntilAvailable(_entityReference.Delete,
                    "Delete Button is not available");

                deleteBtn?.Click(_client);
                Dialogs dialogs = new Dialogs(_client);
                dialogs.ConfirmationDialog(true);

                driver.Wait();

                return true;
            });
        }

        /// <summary>
        /// Saves the entity
        /// </summary>
        /// <param name="thinkTime"></param>
        internal BrowserCommandResult<bool> Save(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Save"), driver =>
            {
                driver.SendKeys(_client.ElementMapper.CommandBarReference.Button, new string[] { Keys.Control, "S" });
                //driver.SendKeys("S");

                //Actions action = new Actions(driver);
                //action.KeyDown(Keys.Control).SendKeys("S").Perform();

                return true;
            });
        }
        #endregion
        #endregion
    }
}
