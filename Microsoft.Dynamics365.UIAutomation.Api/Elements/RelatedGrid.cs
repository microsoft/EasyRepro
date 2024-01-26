// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using static Microsoft.Dynamics365.UIAutomation.Api.RelatedGrid;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    ///  Related Grid class.
    ///  </summary>
    public class RelatedGrid 
    {
        #region DTO
        public class RelatedReference
        {
            public const string RelatedGrid = "RelatedGrid";
            public string _commandBarButton = ".//button[contains(@aria-label, '[NAME]') and contains(@id,'SubGrid')]";
            public string _commandBarSubButton = ".//button[contains(., '[NAME]')]";
            public string _commandBarOverflowContainer = "//div[contains(@data-id, 'flyoutRootNode')]";
            public string _commandBarOverflowButton = ".//button[contains(@data-id, 'OverflowButton') and contains(@data-lp-id, 'Grid')]";
            public string _commandBarButtonList = "//ul[contains(@data-lp-id, 'commandbar-SubGridAssociated')]";
            public string _commandBarFlyoutButtonList = "//ul[contains(@data-id, 'OverflowFlyout')]";
            #region prop
            public string CommandBarButton { get => _commandBarButton; set { _commandBarButton = value; } }
            public string CommandBarSubButton { get => _commandBarSubButton; set { _commandBarSubButton = value; } }
            public string CommandBarOverflowContainer { get => _commandBarOverflowContainer; set { _commandBarOverflowContainer = value; } }
            public string CommandBarOverflowButton { get => _commandBarOverflowButton; set { _commandBarOverflowButton = value; } }
            public string CommandBarButtonList { get => _commandBarButtonList; set { _commandBarButtonList = value; } }
            public string CommandBarFlyoutButtonList { get => _commandBarFlyoutButtonList; set { _commandBarFlyoutButtonList = value; } }
            #endregion
        }
        #endregion
        private readonly WebClient _client;
        public RelatedGrid(WebClient client) : base()
        {
            _client = client;
        }
        #region public
        /// <summary>
        /// Opens a record from a related grid
        /// </summary>
        /// <param name="index">Index of the record to open</param>
        public void OpenGridRow(int index)
        {
            this.OpenRelatedGridRow(index);
        }

        /// <summary>
        /// Clicks a button in the Related grid menu
        /// </summary>
        /// <param name="name">Name of the button to click</param>
        /// <param name="subName">Name of the submenu button to click</param>
        public void ClickCommand(string name, string subName = null, string subSecondName = null)
        {
            this.ClickRelatedCommand(name, subName, subSecondName);
        }
        #endregion

        #region RelatedGrid

        /// <summary>
        /// Opens the grid record.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> OpenRelatedGridRow(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Open Grid Item"), driver =>
            {
                var grid = driver.FindElement(_client.ElementMapper.GridReference.Container);
                var rows = driver.FindElements(grid.Locator + _client.ElementMapper.GridReference.CellContainer + _client.ElementMapper.GridReference.Rows);

                if (rows.Count <= 0)
                {
                    return true;
                }
                else if (index + 1 > rows.Count)
                {
                    throw new IndexOutOfRangeException($"Grid record count: {rows.Count}. Expected: {index + 1}");
                }

                var row = rows.ElementAt(index + 1);
                var cell = driver.FindElements(row.Locator + _client.ElementMapper.SubGridReference.SubGridCells).ElementAt(1);

                cell.DoubleClick(_client, cell.Locator);

                //new Actions(driver).DoubleClick(cell).Perform();
                driver.Wait();

                return true;
            });
        }

        public BrowserCommandResult<bool> ClickRelatedCommand(string name, string subName = null, string subSecondName = null)
        {
            return _client.Execute(_client.GetOptions("Click Related Tab Command"), driver =>
            {
                // Locate Related Command Bar Button List
                var relatedCommandBarButtonList = driver.WaitUntilAvailable(_client.ElementMapper.RelatedGridReference.CommandBarButtonList);

                // Validate list has provided command bar button
                if (driver.HasElement(_client.ElementMapper.RelatedGridReference.CommandBarButtonList + _client.ElementMapper.RelatedGridReference.CommandBarButton.Replace("[NAME]", name)))
                {
                    driver.FindElement(_client.ElementMapper.RelatedGridReference.CommandBarButtonList + _client.ElementMapper.RelatedGridReference.CommandBarButton.Replace("[NAME]", name)).Click(_client);

                    driver.Wait();

                    if (subName != null)
                    {
                        //Look for Overflow flyout
                        if (driver.HasElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer))
                        {
                            var overFlowContainer = driver.FindElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer);

                            if (!driver.HasElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName)))
                                throw new KeyNotFoundException($"{subName} button not found");

                            driver.FindElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName)).Click(_client);

                            driver.Wait();
                        }

                        if (subSecondName != null)
                        {
                            //Look for Overflow flyout
                            if (driver.HasElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer))
                            {
                                var overFlowContainer = driver.FindElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer);

                                if (!driver.HasElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName)))
                                    throw new KeyNotFoundException($"{subName} button not found");

                                driver.FindElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName)).Click(_client);

                                driver.Wait();
                            }
                        }
                    }

                    return true;
                }
                else
                {
                    // Button was not found, check if we should be looking under More Commands (OverflowButton)
                    var moreCommands = driver.HasElement(relatedCommandBarButtonList.Locator + _client.ElementMapper.RelatedGridReference.CommandBarOverflowButton);

                    if (moreCommands)
                    {
                        var overFlowButton = driver.FindElement(relatedCommandBarButtonList.Locator + _client.ElementMapper.RelatedGridReference.CommandBarOverflowButton);
                        overFlowButton.Click(_client);

                        if (driver.HasElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer)) //Look for Overflow
                        {
                            var overFlowContainer = driver.FindElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer);

                            if (driver.HasElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarButton.Replace("[NAME]", name)))
                            {
                                driver.FindElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarButton.Replace("[NAME]", name)).Click(_client);

                                driver.Wait();

                                if (subName != null)
                                {
                                    overFlowContainer = driver.FindElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer);

                                    if (!driver.HasElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName)))
                                        throw new KeyNotFoundException($"{subName} button not found");

                                    driver.FindElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName)).Click(_client);

                                    driver.Wait();

                                    if (subSecondName != null)
                                    {
                                        overFlowContainer = driver.FindElement(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer);

                                        if (!driver.HasElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName)))
                                            throw new KeyNotFoundException($"{subName} button not found");

                                        driver.FindElement(overFlowContainer.Locator + _client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName)).Click(_client);

                                        driver.Wait();
                                    }
                                }

                                return true;
                            }
                        }
                        else
                        {
                            throw new KeyNotFoundException($"{name} button not found in the More Commands container. Button names are case sensitive. Please check for proper casing of button name.");
                        }

                    }
                    else
                    {
                        throw new KeyNotFoundException($"{name} button not found. Button names are case sensitive. Please check for proper casing of button name.");
                    }
                }

                return true;
            });
        }

        #endregion
    }
}
