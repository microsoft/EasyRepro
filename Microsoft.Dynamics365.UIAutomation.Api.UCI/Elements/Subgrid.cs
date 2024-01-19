// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
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
        public class SubGridReference
        {
            public const string SubGrid = "SubGrid";
            #region private
            private string _SubGridTitle = "//div[contains(text(), '[NAME]')]";
            private string _SubGridRow = "//div[@data-id='[NAME]-pcf_grid_control_container']//div[@data-id='grid-container']//div[@data-list-index=\'[INDEX]\']";
            private string _SubGridLastRow = "//div[@ref='centerContainer']//div[@role='rowgroup']//div[contains(@class, 'ag-row-last')]";
            private string _SubGridContents = "//div[@id=\"dataSetRoot_[NAME]\"]";
            private string _SubGridList = "//div[@data-id='[NAME]-pcf_grid_control_container']//div[@data-id='grid-container']//div[@data-automationid='ListCell']";
            private string _SubGridListCells = ".//div[@class='ag-center-cols-viewport']//div[@role='rowgroup']//div[@row-index]";
            private string _SubGridViewPickerButton = ".//span[contains(@id, 'ViewSelector') and contains(@id, 'button')]";
            private string _SubGridViewPickerFlyout = "//div[contains(@id, 'ViewSelector') and contains(@flyoutroot, 'flyoutRootNode')]";
            private string _SubGridCommandBar = ".//ul[contains(@data-id, 'CommandBar')]";
            private string _SubGridCommandLabel = ".//button//span[text()=\"[NAME]\"]";
            private string _SubGridOverflowContainer = ".//div[contains(@data-id, 'flyoutRootNode')]";
            private string _SubGridOverflowButton = ".//button[contains(@aria-label, '[NAME]')]";
            private string _SubGridHighDensityList = ".//div[contains(@data-lp-id, \"ReadOnlyGrid|[NAME]\") and contains(@class, 'editableGrid')]";
            private string _EditableSubGridList = ".//div[contains(@data-lp-id, \"[NAME]\") and contains(@class, 'editableGrid') and not(contains(@class, 'readonly'))]";
            private string _EditableSubGridListCells = ".//div[contains(@wj-part, 'cells') and contains(@class, 'wj-cells') and contains(@role, 'grid')]";
            private string _EditableSubGridListCellRows = ".//div[contains(@class, 'wj-row') and contains(@role, 'row')]";
            private string _EditableSubGridCells = ".//div[@role='gridcell']";
            private string _SubGridControl = "Entity_SubGridControl";
            private string _SubGridCells = "Entity_SubGridCells";
            private string _SubGridRows = ".//div[@role='row' and ./div[@role='gridcell']]";
            private string _SubGridRowsHighDensity = ".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Data')]";
            private string _SubGridDataRowsEditable = ".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Data')]";
            private string _SubGridHeaders = ".//div[contains(@class,'grid-header-text')]";
            private string _SubGridHeadersHighDensity = ".//div[contains(@class, 'wj-colheaders') and contains(@wj-part, 'chcells')]/div/div";
            private string _SubGridHeadersEditable = ".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Header')]/div";
            private string _SubGridRecordCheckbox = "//div[contains(@data-id,'cell-[INDEX]-1') and contains(@data-lp-id,'[NAME]')]";
            private string _SubGridSearchBox = ".//div[contains(@data-id, 'data-set-quickFind-container')]";
            private string _SubGridAddButton = "//button[contains(@data-id,'[NAME].AddNewStandard')]/parent::li/parent::ul[contains(@data-lp-id, 'commandbar-SubGridStandard:[NAME]')]";
            #endregion
            public string SubGridTitle { get => _SubGridTitle; set { _SubGridTitle = value; } }
            public string SubGridRow { get => _SubGridRow; set { _SubGridRow = value; } }
            public string SubGridLastRow { get => _SubGridLastRow; set { _SubGridLastRow = value; } }
            public string SubGridContents { get => _SubGridContents; set { _SubGridContents = value; } }
            public string SubGridList { get => _SubGridList; set { _SubGridList = value; } }
            public string SubGridListCells { get => _SubGridListCells; set { _SubGridListCells = value; } }
            public string SubGridViewPickerButton { get => _SubGridViewPickerButton; set { _SubGridViewPickerButton = value; } }
            public string SubGridViewPickerFlyout { get => _SubGridViewPickerFlyout; set { _SubGridViewPickerFlyout = value; } }
            public string SubGridCommandBar { get => _SubGridCommandBar; set { _SubGridCommandBar = value; } }
            public string SubGridCommandLabel { get => _SubGridCommandLabel; set { _SubGridCommandLabel = value; } }
            public string SubGridOverflowContainer { get => _SubGridOverflowContainer; set { _SubGridOverflowContainer = value; } }
            public string SubGridOverflowButton { get => _SubGridOverflowButton; set { _SubGridOverflowButton = value; } }
            public string SubGridHighDensityList { get => _SubGridHighDensityList; set { _SubGridHighDensityList = value; } }
            public string EditableSubGridList { get => _EditableSubGridList; set { _EditableSubGridList = value; } }
            public string EditableSubGridListCells { get => _EditableSubGridListCells; set { _EditableSubGridListCells = value; } }
            public string EditableSubGridListCellRows { get => _EditableSubGridListCellRows; set { _EditableSubGridListCellRows = value; } }
            public string EditableSubGridCells { get => _EditableSubGridCells; set { _EditableSubGridCells = value; } }
            public string SubGridControl { get => _SubGridControl; set { _SubGridControl = value; } }
            public string SubGridCells { get => _SubGridCells; set { _SubGridCells = value; } }
            public string SubGridRows { get => _SubGridRows; set { _SubGridRows = value; } }
            public string SubGridRowsHighDensity { get => _SubGridRowsHighDensity; set { _SubGridRowsHighDensity = value; } }
            public string SubGridDataRowsEditable { get => _SubGridDataRowsEditable; set { _SubGridDataRowsEditable = value; } }
            public string SubGridHeaders { get => _SubGridHeaders; set { _SubGridHeaders = value; } }
            public string SubGridHeadersHighDensity { get => _SubGridHeadersHighDensity; set { _SubGridHeadersHighDensity = value; } }
            public string SubGridHeadersEditable { get => _SubGridHeadersEditable; set { _SubGridHeadersEditable = value; } }
            public string SubGridRecordCheckbox { get => _SubGridRecordCheckbox; set { _SubGridRecordCheckbox = value; } }
            public string SubGridSearchBox { get => _SubGridSearchBox; set { _SubGridSearchBox = value; } }
            public string SubGridAddButton { get => _SubGridAddButton; set { _SubGridAddButton = value; } }
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
                var subGrid = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridContents.Replace("[NAME]", subGridName));

                return subGrid.GetAttribute(_client,"innerHTML");
            });
        }
        internal BrowserCommandResult<bool> SwitchSubGridView(string subGridName, string viewName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Switch SubGrid View"), driver =>
            {
                // Initialize required variables
                Element viewPicker = null;

                // Find the SubGrid
                var subGrid = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridContents.Replace("[NAME]", subGridName));

                var foundPicker = driver.HasElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridViewPickerButton);

                if (foundPicker)
                {
                    viewPicker = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridViewPickerButton);
                    viewPicker.Click(_client);

                    // Locate the ViewSelector flyout
                    var viewPickerFlyout = driver.WaitUntilAvailable(_client.ElementMapper.SubGridReference.SubGridViewPickerFlyout, new TimeSpan(0, 0, 2), "Cannot click SubGrid View Picker Flyout. XPath: '" + _client.ElementMapper.SubGridReference.SubGridViewPickerFlyout + "'");

                    var viewItems = driver.FindElements(viewPickerFlyout.Locator + "//li");


                    //Is the button in the ribbon?
                    if (viewItems.Any(x => x.GetAttribute(_client,"aria-label").Equals(viewName, StringComparison.OrdinalIgnoreCase)))
                    {
                        viewItems.FirstOrDefault(x => x.GetAttribute(_client,"aria-label").Equals(viewName, StringComparison.OrdinalIgnoreCase)).Click(_client);
                    }

                }
                else
                    throw new KeyNotFoundException($"Unable to locate the viewPicker for SubGrid {subGridName}");

                driver.Wait();

                return true;
            });
        }
        /// This method is obsolete. Do not use.
        public BrowserCommandResult<bool> ClickSubgridAddButton(string subgridName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Click add button of subgrid: {subgridName}"), driver =>
            {
                driver.FindElement(_client.ElementMapper.SubGridReference.SubGridAddButton.Replace("[NAME]", subgridName))?.Click(_client);

                return true;
            });
        }

        public BrowserCommandResult<bool> ClickSubGridCommand(string subGridName, string name, string subName = null, string subSecondName = null)
        {
            return _client.Execute(_client.GetOptions("Click SubGrid Command"), driver =>
            {
                // Initialize required local variables
                Element subGridCommandBar = null;

                // Find the SubGrid
                var subGrid = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridContents.Replace("[NAME]", subGridName));

                if (subGrid == null)
                    throw new KeyNotFoundException($"Unable to locate subgrid contents for {subGridName} subgrid.");

                // Check if grid commandBar was found
                if (driver.HasElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridCommandBar.Replace("[NAME]", subGridName)))
                {
                    subGridCommandBar = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridCommandBar.Replace("[NAME]", subGridName));
                    //Is the button in the ribbon?
                    if (driver.HasElement(subGridCommandBar.Locator + _client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name)))
                    {
                        var command = driver.FindElement(subGridCommandBar.Locator + _client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name));
                        command.Click(_client);
                        driver.Wait();
                    }
                    else
                    {
                        // Is the button in More Commands overflow?
                        if (driver.HasElement(subGridCommandBar.Locator + _client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", "More commands")))
                        {
                            var moreCommands = driver.FindElement(subGridCommandBar.Locator + _client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", "More commands"));
                            // Click More Commandss
                            moreCommands.Click(_client);
                            driver.Wait();

                            // Locate the overflow button (More Commands flyout)
                            var overflowContainer = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridOverflowContainer);

                            //Click the primary button, if found
                            if (driver.HasElement(overflowContainer.Locator + _client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", name)))
                            {
                                var overflowCommand = driver.FindElement(overflowContainer.Locator + _client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", name));
                                overflowCommand.Click(_client);
                                driver.Wait();
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
                        var overflowContainer = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridOverflowContainer);

                        //Click the primary button, if found
                        if (driver.HasElement(overflowContainer.Locator + _client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", subName)))
                        {
                            var overflowButton = driver.FindElement(overflowContainer.Locator + _client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", subName));
                            overflowButton.Click(_client);
                            driver.Wait();
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{subName}' exists under the {name} command inside of {subGridName} Commandbar.");

                        // Check if we need to go to a 3rd level
                        if (subSecondName != null)
                        {
                            // Locate the sub-button flyout if subSecondName present
                            overflowContainer = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridOverflowContainer);

                            //Click the primary button, if found
                            if (driver.HasElement(overflowContainer.Locator + _client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", subSecondName)))
                            {
                                var secondOverflowCommand = driver.FindElement(overflowContainer.Locator + _client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", subSecondName));
                                secondOverflowCommand.Click(_client);
                                driver.Wait();
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
                    _client.ElementMapper.SubGridReference.SubGridContents.Replace("[NAME]", subGridName),
                    5.Seconds(),
                    $"Unable to find subgrid named {subGridName}.");

                driver.ClickWhenAvailable(subGrid.Locator + "//div[@role='columnheader']//span[@role='checkbox']");
                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SearchSubGrid(string subGridName, string searchCriteria, bool clearByDefault = false)
        {
            return _client.Execute(_client.GetOptions($"Search SubGrid {subGridName}"), driver =>
            {
                Element subGridSearchField = null;
                // Find the SubGrid
                var subGrid = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridContents.Replace("[NAME]", subGridName));
                if (subGrid != null)
                {
                    var foundSearchField = driver.HasElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridSearchBox);
                    if (foundSearchField)
                    {
                        var inputElement = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridSearchBox);

                        if (clearByDefault)
                        {
                            inputElement.Clear(_client, inputElement.Locator);
                        }

                        inputElement.SetValue(_client, searchCriteria );

                        var startSearch = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridSearchBox + "//button");
                        startSearch.Click(_client);

                        driver.Wait();
                    }
                    else
                        throw new KeyNotFoundException($"Unable to locate the search box for subgrid {subGridName}. Please validate that view search is enabled for this subgrid");
                }
                else
                    throw new KeyNotFoundException($"Unable to locate subgrid with name {subGridName}");

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
                Element subGridRecordList = null;
                List<string> columns = new List<string>();
                List<string> cellValues = new List<string>();
                GridItem item = new GridItem();
                Dictionary<string, object> WindowStateData = (Dictionary<string, object>)driver.ExecuteScript($"return JSON.parse(JSON.stringify(window[Object.keys(window).find(i => !i.indexOf(\"__store$\"))].getState().data))");
                // Find the SubGrid
                var subGrid = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridContents.Replace("[NAME]", subgridName));

                if (subGrid == null)
                    throw new KeyNotFoundException($"Unable to locate subgrid contents for {subgridName} subgrid.");
                // Check if ReadOnlyGrid was found
                if (driver.HasElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridListCells.Replace("[NAME]", subgridName)))
                {
                    subGridRecordList = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridListCells.Replace("[NAME]", subgridName));
                    // Locate record list
                    var foundRecords = driver.HasElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridListCells.Replace("[NAME]", subgridName));
                    subGridRecordList = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridListCells.Replace("[NAME]", subgridName));
                    if (foundRecords)
                    {
                        var subGridRecordRows = driver.FindElements(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridRows.Replace("[NAME]", subgridName));
                        var SubGridContainer = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridContents.Replace("[NAME]", subgridName));
                        string[] gridDataId = driver.FindElement(SubGridContainer.Locator + $"//div[contains(@data-lp-id,'{subgridName}')]").GetAttribute(_client, "data-lp-id").Split('|');
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
                        throw new KeyNotFoundException($"Unable to locate record list for subgrid {subgridName}");

                }
                // Attempt to locate the editable grid list
                else if (driver.HasElement(subGrid.Locator + _client.ElementMapper.SubGridReference.EditableSubGridList.Replace("[NAME]", subgridName)))
                {
                    subGridRecordList = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.EditableSubGridList.Replace("[NAME]", subgridName));
                    //Find the columns
                    var headerCells = driver.FindElements(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridHeadersEditable);

                    foreach (Element headerCell in headerCells)
                    {
                        var headerTitle = headerCell.GetAttribute(_client,"title");
                        columns.Add(headerTitle);
                    }

                    //Find the rows
                    var rows = driver.FindElements(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridDataRowsEditable);

                    //Process each row
                    foreach (Element row in rows)
                    {
                        var cells = driver.FindElements(row.Locator + _client.ElementMapper.SubGridReference.SubGridCells);

                        if (cells.Count > 0)
                        {
                            foreach (Element thisCell in cells)
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
                else if (driver.HasElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridHighDensityList.Replace("[NAME]", subgridName)))
                {
                    subGridRecordList = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridHighDensityList.Replace("[NAME]", subgridName));
                    //Find the columns
                    var headerCells = driver.FindElements(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridHeadersHighDensity);

                    foreach (Element headerCell in headerCells)
                    {
                        var headerTitle = headerCell.GetAttribute(_client, "data-id");
                        columns.Add(headerTitle);
                    }

                    //Find the rows
                    var rows = driver.FindElements(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridRowsHighDensity);

                    //Process each row
                    foreach (Element row in rows)
                    {
                        //Get the entityId and entity Type
                        if (row.GetAttribute(_client, "data-lp-id") != null)
                        {
                            var rowAttributes = row.GetAttribute(_client, "data-lp-id").Split('|');
                            item.EntityName = rowAttributes[4];
                            //The row record IDs are not in the DOM. Must be retrieved via JavaScript
                            var getId = $"return Xrm.Page.getControl(\"{subgridName}\").getGrid().getRows().get({rows.IndexOf(row)}).getData().entity.getId()";
                            item.Id = new Guid((string)driver.ExecuteScript(getId));
                        }

                        var cells = driver.FindElements(row.Locator + _client.ElementMapper.SubGridReference.SubGridCells);

                        if (cells.Count > 0)
                        {
                            foreach (Element thisCell in cells)
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

        private void ClickSubGridAndPageDown(IWebBrowser driver, Element grid)
        {
            //Actions actions = new Actions(driver);
            //var topRow = driver.FindElement(By.XPath("//div[@data-id='entity_control-pcf_grid_control_container']//div[@ref='centerContainer']//div[@role='rowgroup']//div[@role='row']"));
            var topRow = driver.FindElement(_client.ElementMapper.GridReference.Rows);
            //topRow.Click();
            //actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Perform();

            driver.FindElement(topRow.Locator + "//div[@role='listitem']//button").Focus(_client, topRow.Locator + "//div[@role='listitem']//button");
            driver.SendKeys(grid.Locator, new string[] { Keys.Alt, Keys.ArrowDown });
            //actions.KeyDown(OpenQA.Selenium.Keys.Alt).SendKeys(OpenQA.Selenium.Keys.ArrowDown).Build().Perform();
            //return actions;

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
                var subGrid = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridContents.Replace("[NAME]", subgridName));

                // Find list of SubGrid records
                Element subGridRecordList = null;
                var foundGrid = driver.HasElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridListCells.Replace("[NAME]", subgridName));
                subGridRecordList = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridListCells.Replace("[NAME]", subgridName));
                // Read Only Grid Found
                if (subGridRecordList != null && foundGrid)
                {
                    var subGridRecordRows = driver.FindElements(subGrid.Locator + _client.ElementMapper.SubGridReference.SubGridListCells.Replace("[NAME]", subgridName));
                    if (subGridRecordRows == null)
                        throw new Exception($"No records were found for subgrid {subgridName}");
                    //Actions actions = new Actions(driver);
                    //actions.MoveToElement(subGrid).Perform();
                    subGrid.Focus(_client, subGrid.Locator);
                    Element gridRow = null;
                    if (index + 1 < subGridRecordRows.Count)
                    {
                        gridRow = subGridRecordRows[index];
                    }
                    else
                    {
                        var grid = driver.FindElement("//div[@ref='eViewport']");
                        //actions.DoubleClick(gridRow).Perform();
                        //driver.WaitForTransaction();
                        gridRow.DoubleClick(_client, gridRow.Locator);
                        driver.Wait();
                    }
                    if (gridRow == null)
                        throw new IndexOutOfRangeException($"Subgrid {subgridName} record count: {subGridRecordRows.Count}. Expected: {index + 1}");

                    gridRow.DoubleClick(_client, gridRow.Locator);
                    //actions.DoubleClick(gridRow).Perform();
                    driver.Wait();
                    return true;
                }
                else if (!foundGrid)
                {
                    // Read Only Grid Not Found
                    var foundEditableGrid = driver.HasElement(subGrid.Locator + _client.ElementMapper.SubGridReference.EditableSubGridList.Replace("[NAME]", subgridName));
                    subGridRecordList = driver.FindElement(subGrid.Locator + _client.ElementMapper.SubGridReference.EditableSubGridList.Replace("[NAME]", subgridName));
                    if (foundEditableGrid)
                    {
                        var editableGridListCells = driver.FindElement(subGridRecordList.Locator + _client.ElementMapper.SubGridReference.EditableSubGridListCells);

                        var editableGridCellRows = driver.FindElements(editableGridListCells.Locator + _client.ElementMapper.SubGridReference.EditableSubGridListCellRows);

                        var editableGridCellRow = driver.FindElements(editableGridCellRows[index + 1] + "./div");

                        editableGridCellRow[0].DoubleClick(_client, editableGridCellRow[0].Locator);

                        //Actions actions = new Actions(driver);
                        //actions.DoubleClick(editableGridCellRow[0]).Perform();

                        driver.Wait();

                        return true;
                    }
                    else
                    {
                        // Editable Grid Not Found
                        // Check for special 'Related' grid form control
                        // This opens a limited form view in-line on the grid

                        //Get the GridName
                        string subGridName = subGrid.GetAttribute(_client, "data-id").Replace("dataSetRoot_", String.Empty);

                        //cell-0 is the checkbox for each record
                        var checkBox = driver.FindElement(_client.ElementMapper.SubGridReference.SubGridRecordCheckbox.Replace("[INDEX]", index.ToString()).Replace("[NAME]", subGridName));

                        checkBox.DoubleClick(_client, checkBox.Locator);
                        //driver.DoubleClick(checkBox);

                        driver.Wait();
                    }
                }

                return true;

            });
        }

        #endregion
    }
}
