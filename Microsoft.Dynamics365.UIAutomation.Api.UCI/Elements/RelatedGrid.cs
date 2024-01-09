// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.RelatedGrid;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    ///  Related Grid class.
    ///  </summary>
    public class RelatedGrid : Element
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
                var grid = driver.FindElement(By.XPath(_client.ElementMapper.GridReference.Container));
                var rows = grid
                    .FindElement(By.XPath(_client.ElementMapper.GridReference.CellContainer))
                    .FindElements(By.XPath(_client.ElementMapper.GridReference.Rows));

                if (rows.Count <= 0)
                {
                    return true;
                }
                else if (index + 1 > rows.Count)
                {
                    throw new IndexOutOfRangeException($"Grid record count: {rows.Count}. Expected: {index + 1}");
                }

                var row = rows.ElementAt(index + 1);
                var cell = row.FindElements(By.XPath(_client.ElementMapper.SubGridReference.SubGridCells)).ElementAt(1);

                new Actions(driver).DoubleClick(cell).Perform();
                driver.WaitForTransaction();

                return true;
            });
        }

        public BrowserCommandResult<bool> ClickRelatedCommand(string name, string subName = null, string subSecondName = null)
        {
            return _client.Execute(_client.GetOptions("Click Related Tab Command"), driver =>
            {
                // Locate Related Command Bar Button List
                var relatedCommandBarButtonList = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarButtonList));

                // Validate list has provided command bar button
                if (relatedCommandBarButtonList.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarButton.Replace("[NAME]", name))))
                {
                    relatedCommandBarButtonList.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarButton.Replace("[NAME]", name))).Click(true);

                    driver.WaitForTransaction();

                    if (subName != null)
                    {
                        //Look for Overflow flyout
                        if (driver.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer)))
                        {
                            var overFlowContainer = driver.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer));

                            if (!overFlowContainer.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName))))
                                throw new NotFoundException($"{subName} button not found");

                            overFlowContainer.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName))).Click(true);

                            driver.WaitForTransaction();
                        }

                        if (subSecondName != null)
                        {
                            //Look for Overflow flyout
                            if (driver.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer)))
                            {
                                var overFlowContainer = driver.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer));

                                if (!overFlowContainer.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName))))
                                    throw new NotFoundException($"{subName} button not found");

                                overFlowContainer.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName))).Click(true);

                                driver.WaitForTransaction();
                            }
                        }
                    }

                    return true;
                }
                else
                {
                    // Button was not found, check if we should be looking under More Commands (OverflowButton)
                    var moreCommands = relatedCommandBarButtonList.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowButton));

                    if (moreCommands)
                    {
                        var overFlowButton = relatedCommandBarButtonList.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowButton));
                        overFlowButton.Click(true);

                        if (driver.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer))) //Look for Overflow
                        {
                            var overFlowContainer = driver.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer));

                            if (overFlowContainer.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarButton.Replace("[NAME]", name))))
                            {
                                overFlowContainer.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarButton.Replace("[NAME]", name))).Click(true);

                                driver.WaitForTransaction();

                                if (subName != null)
                                {
                                    overFlowContainer = driver.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer));

                                    if (!overFlowContainer.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName))))
                                        throw new NotFoundException($"{subName} button not found");

                                    overFlowContainer.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName))).Click(true);

                                    driver.WaitForTransaction();

                                    if (subSecondName != null)
                                    {
                                        overFlowContainer = driver.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowContainer));

                                        if (!overFlowContainer.HasElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName))))
                                            throw new NotFoundException($"{subName} button not found");

                                        overFlowContainer.FindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarSubButton.Replace("[NAME]", subName))).Click(true);

                                        driver.WaitForTransaction();
                                    }
                                }

                                return true;
                            }
                        }
                        else
                        {
                            throw new NotFoundException($"{name} button not found in the More Commands container. Button names are case sensitive. Please check for proper casing of button name.");
                        }

                    }
                    else
                    {
                        throw new NotFoundException($"{name} button not found. Button names are case sensitive. Please check for proper casing of button name.");
                    }
                }

                return true;
            });
        }

        #endregion
    }
}
