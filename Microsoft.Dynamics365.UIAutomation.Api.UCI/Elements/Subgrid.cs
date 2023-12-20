// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.Collections.Generic;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.Grid;
using System.Collections.ObjectModel;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.SubGrid;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    ///  SubGrid Grid class.
    ///  </summary>
    public class SubGrid : Element
    {
        #region DTO
        public static class SubGridReference
        {
            public static string SubGridTitle = "//div[contains(text(), '[NAME]')]";
            public static string SubGridRow = "//div[@data-id='[NAME]-pcf_grid_control_container']//div[@data-id='grid-container']//div[@data-list-index=\'[INDEX]\']";
            public static string SubGridLastRow = "//div[@ref='centerContainer']//div[@role='rowgroup']//div[contains(@class, 'ag-row-last')]";
            public static string SubGridContents = "//div[@id=\"dataSetRoot_[NAME]\"]";
            public static string SubGridList = "//div[@data-id='[NAME]-pcf_grid_control_container']//div[@data-id='grid-container']//div[@data-automationid='ListCell']";
            public static string SubGridListCells = ".//div[@class='ag-center-cols-viewport']//div[@role='rowgroup']//div[@row-index]";
            public static string SubGridViewPickerButton = ".//span[contains(@id, 'ViewSelector') and contains(@id, 'button')]";
            public static string SubGridViewPickerFlyout = "//div[contains(@id, 'ViewSelector') and contains(@flyoutroot, 'flyoutRootNode')]";
            public static string SubGridCommandBar = ".//ul[contains(@data-id, 'CommandBar')]";
            public static string SubGridCommandLabel = ".//button//span[text()=\"[NAME]\"]";
            public static string SubGridOverflowContainer = ".//div[contains(@data-id, 'flyoutRootNode')]";
            public static string SubGridOverflowButton = ".//button[contains(@aria-label, '[NAME]')]";
            public static string SubGridHighDensityList = ".//div[contains(@data-lp-id, \"ReadOnlyGrid|[NAME]\") and contains(@class, 'editableGrid')]";
            public static string EditableSubGridList = ".//div[contains(@data-lp-id, \"[NAME]\") and contains(@class, 'editableGrid') and not(contains(@class, 'readonly'))]";
            public static string EditableSubGridListCells = ".//div[contains(@wj-part, 'cells') and contains(@class, 'wj-cells') and contains(@role, 'grid')]";
            public static string EditableSubGridListCellRows = ".//div[contains(@class, 'wj-row') and contains(@role, 'row')]";
            public static string EditableSubGridCells = ".//div[@role='gridcell']";
            public static string SubGridControl = "Entity_SubGridControl";
            public static string SubGridCells = "Entity_SubGridCells";
            public static string SubGridRows = ".//div[@role='row' and ./div[@role='gridcell']]";
            public static string SubGridRowsHighDensity = ".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Data')]";
            public static string SubGridDataRowsEditable = ".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Data')]";
            public static string SubGridHeaders = ".//div[contains(@class,'grid-header-text')]";
            public static string SubGridHeadersHighDensity = ".//div[contains(@class, 'wj-colheaders') and contains(@wj-part, 'chcells')]/div/div";
            public static string SubGridHeadersEditable = ".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Header')]/div";
            public static string SubGridRecordCheckbox = "//div[contains(@data-id,'cell-[INDEX]-1') and contains(@data-lp-id,'[NAME]')]";
            public static string SubGridSearchBox = ".//div[contains(@data-id, 'data-set-quickFind-container')]";
            public static string SubGridAddButton = "//button[contains(@data-id,'[NAME].AddNewStandard')]/parent::li/parent::ul[contains(@data-lp-id, 'commandbar-SubGridStandard:[NAME]')]";
        }
        #endregion

        private readonly WebClient _client;
        public SubGrid(WebClient client) : base()
        {
            _client = client;
        }

        #region public

        /// <summary>
        /// Clicks a button in the subgrid menu
        /// </summary>
        /// <param name="name">Name of the button to click</param>
        /// <param name="subName">Name of the submenu button to click</param>
        public void ClickCommand(string subGridName, string name, string subName = null, string subSecondName = null)
        {
            this.ClickSubGridCommand(subGridName, name, subName, subSecondName);
        }


 

        /// <summary>
        /// Retrieve the items from a subgrid
        /// </summary>
        /// <param name="subgridName">schemaName of the SubGrid control to retrieve items from</param>
        //public List<GridItem> GetSubGridItems(string subgridName)
        //{
        //    return this.GetSubGridItems(subgridName);
        //}

        /// <summary>
        /// Retrieves the number of rows from a subgrid
        /// </summary>
        /// <param name="subgridName">schemaName of the SubGrid control</param>
        /// <returns></returns>
        public int GetSubGridItemsCount(string subgridName)
        {
            Entity entity = new Entity(_client);
            return entity.GetSubGridItemsCount(subgridName);
        }




        /// <summary>
        /// Performs a Search on the subgrid
        /// </summary>
        /// <param name="searchCriteria">Search term</param>
        /// <param name="clearByDefault">Determines whether to clear the search field before supplying the search term</param>
        public void Search(string subGridName, string searchCriteria, bool clearField = false)
        {
            this.SearchSubGrid(subGridName, searchCriteria, clearField);
        }

        /// <summary>
        /// Switches the View on a SubGrid
        /// </summary>
        /// <param name="subgridName">schemaName of the SubGrid control</param>
        /// <param name="viewName">Name of the view to select</param>
        public void SwitchView(string subgridName, string viewName)
        {
            this.SwitchSubGridView(subgridName, viewName);
        }
        #endregion
        #region Subgrid
        /// <summary>
        /// Returns HTML of Grid
        /// </summary>
        public BrowserCommandResult<string> GetSubGridControl(string subGridName)
        {

            return _client.Execute(_client.GetOptions($"Get Sub Grid Control"), driver =>
            {
                var subGrid = driver.FindElement(By.XPath(SubGridReference.SubGridContents.Replace("[NAME]", subGridName)));

                return subGrid.GetAttribute("innerHTML");
            });
        }
        internal BrowserCommandResult<bool> SwitchSubGridView(string subGridName, string viewName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Switch SubGrid View"), driver =>
            {
                // Initialize required variables
                IWebElement viewPicker = null;

                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(SubGridReference.SubGridContents.Replace("[NAME]", subGridName)));

                var foundPicker = subGrid.TryFindElement(By.XPath(SubGridReference.SubGridViewPickerButton), out viewPicker);

                if (foundPicker)
                {
                    viewPicker.Click(true);

                    // Locate the ViewSelector flyout
                    var viewPickerFlyout = driver.WaitUntilAvailable(By.XPath(SubGridReference.SubGridViewPickerFlyout), new TimeSpan(0, 0, 2));

                    var viewItems = viewPickerFlyout.FindElements(By.TagName("li"));


                    //Is the button in the ribbon?
                    if (viewItems.Any(x => x.GetAttribute("aria-label").Equals(viewName, StringComparison.OrdinalIgnoreCase)))
                    {
                        viewItems.FirstOrDefault(x => x.GetAttribute("aria-label").Equals(viewName, StringComparison.OrdinalIgnoreCase)).Click(true);
                    }

                }
                else
                    throw new NotFoundException($"Unable to locate the viewPicker for SubGrid {subGridName}");

                driver.WaitForTransaction();

                return true;
            });
        }
        /// This method is obsolete. Do not use.
        public BrowserCommandResult<bool> ClickSubgridAddButton(string subgridName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Click add button of subgrid: {subgridName}"), driver =>
            {
                driver.FindElement(By.XPath(SubGridReference.SubGridAddButton.Replace("[NAME]", subgridName)))?.Click();

                return true;
            });
        }

        public BrowserCommandResult<bool> ClickSubGridCommand(string subGridName, string name, string subName = null, string subSecondName = null)
        {
            return _client.Execute(_client.GetOptions("Click SubGrid Command"), driver =>
            {
                // Initialize required local variables
                IWebElement subGridCommandBar = null;

                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(SubGridReference.SubGridContents.Replace("[NAME]", subGridName)));

                if (subGrid == null)
                    throw new NotFoundException($"Unable to locate subgrid contents for {subGridName} subgrid.");

                // Check if grid commandBar was found
                if (subGrid.TryFindElement(By.XPath(SubGridReference.SubGridCommandBar.Replace("[NAME]", subGridName)), out subGridCommandBar))
                {
                    //Is the button in the ribbon?
                    if (subGridCommandBar.TryFindElement(By.XPath(SubGridReference.SubGridCommandLabel.Replace("[NAME]", name)), out var command))
                    {
                        command.Click(true);
                        driver.WaitForTransaction();
                    }
                    else
                    {
                        // Is the button in More Commands overflow?
                        if (subGridCommandBar.TryFindElement(By.XPath(SubGridReference.SubGridOverflowButton.Replace("[NAME]", "More commands")), out var moreCommands))
                        {
                            // Click More Commandss
                            moreCommands.Click(true);
                            driver.WaitForTransaction();

                            // Locate the overflow button (More Commands flyout)
                            var overflowContainer = driver.FindElement(By.XPath(SubGridReference.SubGridOverflowContainer));

                            //Click the primary button, if found
                            if (overflowContainer.TryFindElement(By.XPath(SubGridReference.SubGridOverflowButton.Replace("[NAME]", name)), out var overflowCommand))
                            {
                                overflowCommand.Click(true);
                                driver.WaitForTransaction();
                            }
                            else
                                throw new InvalidOperationException($"No command with the name '{name}' exists inside of {subGridName} Commandbar.");
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{name}' exists inside of {subGridName} CommandBar.");
                    }

                    if (subName != null)
                    {
                        // Locate the sub-button flyout if subName present
                        var overflowContainer = driver.FindElement(By.XPath(SubGridReference.SubGridOverflowContainer));

                        //Click the primary button, if found
                        if (overflowContainer.TryFindElement(By.XPath(SubGridReference.SubGridOverflowButton.Replace("[NAME]", subName)), out var overflowButton))
                        {
                            overflowButton.Click(true);
                            driver.WaitForTransaction();
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{subName}' exists under the {name} command inside of {subGridName} Commandbar.");

                        // Check if we need to go to a 3rd level
                        if (subSecondName != null)
                        {
                            // Locate the sub-button flyout if subSecondName present
                            overflowContainer = driver.FindElement(By.XPath(SubGridReference.SubGridOverflowContainer));

                            //Click the primary button, if found
                            if (overflowContainer.TryFindElement(By.XPath(SubGridReference.SubGridOverflowButton.Replace("[NAME]", subSecondName)), out var secondOverflowCommand))
                            {
                                secondOverflowCommand.Click(true);
                                driver.WaitForTransaction();
                            }
                            else
                                throw new InvalidOperationException($"No command with the name '{subSecondName}' exists under the {subName} command inside of {name} on the {subGridName} SubGrid Commandbar.");
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate the Commandbar for the {subGrid} SubGrid.");

                return true;
            });
        }

        /// <summary>
        /// Select all records in a subgrid
        /// </summary>
        /// <param name="subGridName">Schema name of the subgrid to select all</param>
        /// <param name="thinkTime">Additional time to wait, if required</param>
        public BrowserCommandResult<bool> ClickSubgridSelectAll(string subGridName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Click Select All Button on subgrid: {subGridName}"), driver =>
            {
                // Find the SubGrid
                var subGrid = driver.WaitUntilAvailable(
                    By.XPath(SubGridReference.SubGridContents.Replace("[NAME]", subGridName)),
                    5.Seconds(),
                    $"Unable to find subgrid named {subGridName}.");

                subGrid.ClickWhenAvailable(By.XPath("//div[@role='columnheader']//span[@role='checkbox']"));
                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SearchSubGrid(string subGridName, string searchCriteria, bool clearByDefault = false)
        {
            return _client.Execute(_client.GetOptions($"Search SubGrid {subGridName}"), driver =>
            {
                IWebElement subGridSearchField = null;
                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(SubGridReference.SubGridContents.Replace("[NAME]", subGridName)));
                if (subGrid != null)
                {
                    var foundSearchField = subGrid.TryFindElement(By.XPath(SubGridReference.SubGridSearchBox), out subGridSearchField);
                    if (foundSearchField)
                    {
                        var inputElement = subGridSearchField.FindElement(By.TagName("input"));

                        if (clearByDefault)
                        {
                            inputElement.Clear();
                        }

                        inputElement.SendKeys(searchCriteria);

                        var startSearch = subGridSearchField.FindElement(By.TagName("button"));
                        startSearch.Click(true);

                        driver.WaitForTransaction();
                    }
                    else
                        throw new NotFoundException($"Unable to locate the search box for subgrid {subGridName}. Please validate that view search is enabled for this subgrid");
                }
                else
                    throw new NotFoundException($"Unable to locate subgrid with name {subGridName}");

                return true;
            });
        }
        public List<GridItem> GetSubGridItems(string subgridName)
        {
            return this.SubGridGetSubGridItems(subgridName);
        }
        public BrowserCommandResult<List<GridItem>> SubGridGetSubGridItems(string subgridName)
        {
            return _client.Execute(_client.GetOptions($"Get Subgrid Items for Subgrid {subgridName}"), driver =>
            {
                // Initialize return object
                List<GridItem> subGridRows = new List<GridItem>();

                // Initialize required local variables
                IWebElement subGridRecordList = null;
                List<string> columns = new List<string>();
                List<string> cellValues = new List<string>();
                GridItem item = new GridItem();
                Dictionary<string, object> WindowStateData = (Dictionary<string, object>)driver.ExecuteScript($"return JSON.parse(JSON.stringify(window[Object.keys(window).find(i => !i.indexOf(\"__store$\"))].getState().data))");
                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(SubGridReference.SubGridContents.Replace("[NAME]", subgridName)));

                if (subGrid == null)
                    throw new NotFoundException($"Unable to locate subgrid contents for {subgridName} subgrid.");
                // Check if ReadOnlyGrid was found
                if (subGrid.TryFindElement(By.XPath(SubGridReference.SubGridListCells.Replace("[NAME]", subgridName)), out subGridRecordList))
                {

                    // Locate record list
                    var foundRecords = subGrid.TryFindElement(By.XPath(SubGridReference.SubGridListCells.Replace("[NAME]", subgridName)), out subGridRecordList);

                    if (foundRecords)
                    {
                        var subGridRecordRows = subGrid.FindElements(By.XPath(SubGridReference.SubGridRows.Replace("[NAME]", subgridName)));
                        var SubGridContainer = driver.FindElement(By.XPath(SubGridReference.SubGridContents.Replace("[NAME]", subgridName)));
                        string[] gridDataId = SubGridContainer.FindElement(By.XPath($"//div[contains(@data-lp-id,'{subgridName}')]")).GetAttribute("data-lp-id").Split('|');
                        //Need to add entity name
                        string keyForData = Grid.GetGridQueryKey(driver, gridDataId[2] + ":" + subgridName);

                        Dictionary<string, object> WindowStateDataLists = (Dictionary<string, object>)WindowStateData["lists"];

                        //Find Data by Key
                        Dictionary<string, object> WindowStateDataKeyedForData = (Dictionary<string, object>)WindowStateDataLists[keyForData];
                        //Find Record Ids for Key Data Set
                        ReadOnlyCollection<object> WindowStateDataKeyedForDataRecordsIds = (ReadOnlyCollection<object>)WindowStateDataKeyedForData["records"];

                        //Get Data
                        Dictionary<string, object> WindowStateEntityData = (Dictionary<string, object>)WindowStateData["entities"];
                        Dictionary<string, object> WindowStateEntityDataEntity = (Dictionary<string, object>)WindowStateEntityData[gridDataId[2]];
                        foreach (Dictionary<string, object> record in WindowStateDataKeyedForDataRecordsIds)
                        {
                            Dictionary<string, object> recordId = (Dictionary<string, object>)record["id"];
                            //Dictionary<string, object> definedRecord = (Dictionary<string, object>)WindowStateEntityDataEntity[(string)recordId["guid"]];
                            //Dictionary<string, object> attributes = (Dictionary<string, object>)definedRecord["fields"];
                            GridItem gridItem = new GridItem()
                            {
                                EntityName = gridDataId[2],
                                Id = new Guid((string)recordId["guid"])
                            };
                            //ProcessGridRowAttributes(attributes, gridItem);
                            subGridRows.Add(gridItem);
                        }


                    }
                    else
                        throw new NotFoundException($"Unable to locate record list for subgrid {subgridName}");

                }
                // Attempt to locate the editable grid list
                else if (subGrid.TryFindElement(By.XPath(SubGridReference.EditableSubGridList.Replace("[NAME]", subgridName)), out subGridRecordList))
                {
                    //Find the columns
                    var headerCells = subGrid.FindElements(By.XPath(SubGridReference.SubGridHeadersEditable));

                    foreach (IWebElement headerCell in headerCells)
                    {
                        var headerTitle = headerCell.GetAttribute("title");
                        columns.Add(headerTitle);
                    }

                    //Find the rows
                    var rows = subGrid.FindElements(By.XPath(SubGridReference.SubGridDataRowsEditable));

                    //Process each row
                    foreach (IWebElement row in rows)
                    {
                        var cells = row.FindElements(By.XPath(SubGridReference.SubGridCells));

                        if (cells.Count > 0)
                        {
                            foreach (IWebElement thisCell in cells)
                                cellValues.Add(thisCell.Text);

                            for (int i = 0; i < columns.Count; i++)
                            {
                                //The first cell is always a checkbox for the record.  Ignore the checkbox.
                                if (i == 0)
                                {
                                    // Do Nothing
                                }
                                else
                                {
                                    item[columns[i]] = cellValues[i];
                                }
                            }

                            subGridRows.Add(item);

                            // Flush Item and Cell Values To Get New Rows
                            cellValues = new List<string>();
                            item = new GridItem();
                        }
                    }

                    return subGridRows;

                }
                // Special 'Related' high density grid control for entity forms
                else if (subGrid.TryFindElement(By.XPath(SubGridReference.SubGridHighDensityList.Replace("[NAME]", subgridName)), out subGridRecordList))
                {
                    //Find the columns
                    var headerCells = subGrid.FindElements(By.XPath(SubGridReference.SubGridHeadersHighDensity));

                    foreach (IWebElement headerCell in headerCells)
                    {
                        var headerTitle = headerCell.GetAttribute("data-id");
                        columns.Add(headerTitle);
                    }

                    //Find the rows
                    var rows = subGrid.FindElements(By.XPath(SubGridReference.SubGridRowsHighDensity));

                    //Process each row
                    foreach (IWebElement row in rows)
                    {
                        //Get the entityId and entity Type
                        if (row.GetAttribute("data-lp-id") != null)
                        {
                            var rowAttributes = row.GetAttribute("data-lp-id").Split('|');
                            item.EntityName = rowAttributes[4];
                            //The row record IDs are not in the DOM. Must be retrieved via JavaScript
                            var getId = $"return Xrm.Page.getControl(\"{subgridName}\").getGrid().getRows().get({rows.IndexOf(row)}).getData().entity.getId()";
                            item.Id = new Guid((string)driver.ExecuteScript(getId));
                        }

                        var cells = row.FindElements(By.XPath(SubGridReference.SubGridCells));

                        if (cells.Count > 0)
                        {
                            foreach (IWebElement thisCell in cells)
                                cellValues.Add(thisCell.Text);

                            for (int i = 0; i < columns.Count; i++)
                            {
                                //The first cell is always a checkbox for the record.  Ignore the checkbox.
                                if (i == 0)
                                {
                                    // Do Nothing
                                }
                                else
                                {
                                    item[columns[i]] = cellValues[i];
                                }

                            }

                            subGridRows.Add(item);

                            // Flush Item and Cell Values To Get New Rows
                            cellValues = new List<string>();
                            item = new GridItem();
                        }
                    }

                    return subGridRows;
                }

                // Return rows object
                return subGridRows;
            });
        }

        private static Actions ClickSubGridAndPageDown(IWebDriver driver, IWebElement grid)
        {
            Actions actions = new Actions(driver);
            //var topRow = driver.FindElement(By.XPath("//div[@data-id='entity_control-pcf_grid_control_container']//div[@ref='centerContainer']//div[@role='rowgroup']//div[@role='row']"));
            var topRow = driver.FindElement(By.XPath(GridReference.Rows));
            //topRow.Click();
            //actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Perform();

            actions.MoveToElement(topRow.FindElement(By.XPath("//div[@role='listitem']//button"))).Perform();
            actions.KeyDown(OpenQA.Selenium.Keys.Alt).SendKeys(OpenQA.Selenium.Keys.ArrowDown).Build().Perform();
            return actions;

        }
        /// <summary>
        /// Open a record on a subgrid
        /// </summary>
        /// <param name="subgridName">schemaName of the SubGrid control</param>
        /// <param name="index">Index of the record to open</param>
        public BrowserCommandResult<bool> OpenSubGridRecord(string subgridName, int index = 0)
        {
            return _client.Execute(_client.GetOptions($"Open Subgrid record for subgrid {subgridName}"), driver =>
            {
                // Find the SubGrid
                var subGrid = driver.FindElement(By.XPath(SubGridReference.SubGridContents.Replace("[NAME]", subgridName)));

                // Find list of SubGrid records
                IWebElement subGridRecordList = null;
                var foundGrid = subGrid.TryFindElement(By.XPath(SubGridReference.SubGridListCells.Replace("[NAME]", subgridName)), out subGridRecordList);

                // Read Only Grid Found
                if (subGridRecordList != null && foundGrid)
                {
                    var subGridRecordRows = subGrid.FindElements(By.XPath(SubGridReference.SubGridListCells.Replace("[NAME]", subgridName)));
                    if (subGridRecordRows == null)
                        throw new NoSuchElementException($"No records were found for subgrid {subgridName}");
                    Actions actions = new Actions(driver);
                    actions.MoveToElement(subGrid).Perform();

                    IWebElement gridRow = null;
                    if (index + 1 < subGridRecordRows.Count)
                    {
                        gridRow = subGridRecordRows[index];
                    }
                    else
                    {
                        var grid = driver.FindElement(By.XPath("//div[@ref='eViewport']"));
                        actions.DoubleClick(gridRow).Perform();
                        driver.WaitForTransaction();
                    }
                    if (gridRow == null)
                        throw new IndexOutOfRangeException($"Subgrid {subgridName} record count: {subGridRecordRows.Count}. Expected: {index + 1}");


                    actions.DoubleClick(gridRow).Perform();
                    driver.WaitForTransaction();
                    return true;
                }
                else if (!foundGrid)
                {
                    // Read Only Grid Not Found
                    var foundEditableGrid = subGrid.TryFindElement(By.XPath(SubGridReference.EditableSubGridList.Replace("[NAME]", subgridName)), out subGridRecordList);

                    if (foundEditableGrid)
                    {
                        var editableGridListCells = subGridRecordList.FindElement(By.XPath(SubGridReference.EditableSubGridListCells));

                        var editableGridCellRows = editableGridListCells.FindElements(By.XPath(SubGridReference.EditableSubGridListCellRows));

                        var editableGridCellRow = editableGridCellRows[index + 1].FindElements(By.XPath("./div"));

                        Actions actions = new Actions(driver);
                        actions.DoubleClick(editableGridCellRow[0]).Perform();

                        driver.WaitForTransaction();

                        return true;
                    }
                    else
                    {
                        // Editable Grid Not Found
                        // Check for special 'Related' grid form control
                        // This opens a limited form view in-line on the grid

                        //Get the GridName
                        string subGridName = subGrid.GetAttribute("data-id").Replace("dataSetRoot_", String.Empty);

                        //cell-0 is the checkbox for each record
                        var checkBox = driver.FindElement(By.XPath(SubGridReference.SubGridRecordCheckbox.Replace("[INDEX]", index.ToString()).Replace("[NAME]", subGridName)));

                        driver.DoubleClick(checkBox);

                        driver.WaitForTransaction();
                    }
                }

                return true;

            });
        }

        #endregion
    }
}
