// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Grid : Element
    {
        #region DTO
        public  class GridReference
        {
            public const string Grid = "Grid";
            #region private
            private string _Container = "//div[@data-id='data-set-body-container']";
            private string _GridContainer = "//div[translate(@data-type, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='grid']";
            private string _PowerAppsGridControlClickableCell = "//div[@aria-readonly = 'true']//a";
            private string _EditableGrid = "//div[@aria-label='Editable Grid']";
            private string _EditableGridRows = "//div[@wj-part='cells' and @role='grid']";
            private string _QuickFind = "//*[contains(@id, \'quickFind_text\')]";
            private string _FirstPage = "//button[contains(@data-id,'loadFirstPage')]";
            private string _NextPage = "//button[contains(@data-id,'moveToNextPage')]";
            private string _PreviousPage = "//button[contains(@data-id,'moveToPreviousPage')]";
            private string _SelectAll = "//button[contains(@title,'Select All')]";
            private string _ShowChart = "//button[contains(@aria-label,'Show Chart')]";
            private string _HideChart = "Grid_HideChart";
            private string _JumpBar = "//*[@id=\"JumpBarItemsList\"]";
            private string _FilterByAll = "//*[@id=\"All_link\"]";
            private string _RowsContainerCheckbox = "//div[@role='checkbox']";
            private string _RowsContainer = "//div[contains(@role,'grid')]";
            private string _LegacyReadOnlyRows = "//div[@data-id='grid-container']//div[@class='ag-center-cols-container']//div[@role='row']";
            private string _Rows = "//div[@data-id='entity_control-powerapps_onegrid_control_container']//div[@class='ag-center-cols-container']//div[@role='row']";
            private string _Row = "//div[@class='ag-center-cols-container']//div[@row-index=\'[INDEX]\']";
            private string _LastRow = "//div[@data-id='entity_control-pcf_grid_control_container']//div[@ref='centerContainer']//div[@role='rowgroup']//div[contains(@class, 'ag last-row')]";
            private string _Columns = "//div[contains(@ref,'gridHeader')]";
            private string _Control = "//div[contains(@data-lp-id, 'MscrmControls.Grid.PCFGridControl')]";
            private string _ChartSelector = "//span[contains(@id,'ChartSelector')]";
            private string _ChartViewList = "//ul[contains(@role,'listbox')]";
            private string _GridSortColumn = "//div[contains(@role,'columnheader')]//label[@title='[COLNAME]']";
            private string _Cells = ".//div[@role='gridcell']";
            private string _CellContainer = "//div[@role='grid'][@data-id='grid-cell-container']";
            private string _ViewSelector = "//button[contains(@id,'ViewSelector')]";
            private string _ViewContainer = "//div[contains(@aria-label,'Views')]";
            private string _ViewSelectorMenuItem = ".//li[contains(@class, 'ms-ContextualMenu-item')]";
            private string _SubArea = "//*[contains(@data-id,'[NAME]')]";
            #endregion
            #region prop
            public string Container { get => _Container; set { _Container = value; } }
            public string GridContainer { get => _GridContainer; set { _GridContainer = value; } }
            public string PowerAppsGridControlClickableCell { get => _PowerAppsGridControlClickableCell; set { _PowerAppsGridControlClickableCell = value; } }
            public string EditableGrid { get => _EditableGrid; set { _EditableGrid = value; } }
            public string EditableGridRows { get => _EditableGridRows; set { _EditableGridRows = value; } }
            public string QuickFind { get => _QuickFind; set { _QuickFind = value; } }
            public string FirstPage { get => _FirstPage; set { _FirstPage = value; } }
            public string NextPage { get => _NextPage; set { _NextPage = value; } }
            public string PreviousPage { get => _PreviousPage; set { _PreviousPage = value; } }
            public string SelectAll { get => _SelectAll; set { _SelectAll = value; } }
            public string ShowChart { get => _ShowChart; set { _ShowChart = value; } }
            public string HideChart { get => _HideChart; set { _HideChart = value; } }
            public string JumpBar { get => _JumpBar; set { _JumpBar = value; } }
            public string FilterByAll { get => _FilterByAll; set { _FilterByAll = value; } }
            public string RowsContainerCheckbox { get => _RowsContainerCheckbox; set { _RowsContainerCheckbox = value; } }
            public string RowsContainer { get => _RowsContainer; set { _RowsContainer = value; } }
            public string LegacyReadOnlyRows { get => _LegacyReadOnlyRows; set { _LegacyReadOnlyRows = value; } }
            public string Rows { get => _Rows; set { _Rows = value; } }
            public string Row { get => _Row; set { _Row = value; } }
            public string LastRow { get => _LastRow; set { _LastRow = value; } }
            public string Columns { get => _Columns; set { _Columns = value; } }
            public string Control { get => _Control; set { _Control = value; } }
            public string ChartSelector { get => _ChartSelector; set { _ChartSelector = value; } }
            public string ChartViewList { get => _ChartViewList; set { _ChartViewList = value; } }
            public string GridSortColumn { get => _GridSortColumn; set { _GridSortColumn = value; } }
            public string Cells { get => _Cells; set { _Cells = value; } }
            public string CellContainer { get => _CellContainer; set { _CellContainer = value; } }
            public string ViewSelector { get => _ViewSelector; set { _ViewSelector = value; } }
            public string ViewContainer { get => _ViewContainer; set { _ViewContainer = value; } }
            public string ViewSelectorMenuItem { get => _ViewSelectorMenuItem; set { _ViewSelectorMenuItem = value; } }
            public string SubArea { get => _SubArea; set { _SubArea = value; } }
            #endregion
        }
        #endregion
        private readonly WebClient _client;
        public enum GridType { PowerAppsGridControl, LegacyReadOnlyGrid, ReadOnlyGrid, EditableGrid }
        public Grid(WebClient client) : base()
        {
            _client = client;
        }

        #region public

        /// <summary>
        /// Checks / Ticks a record in the grid
        /// </summary>
        /// <param name="index">The index of the row to open</param>
        public void HighLightRecord(int index)
        {
            this.SelectRecord(index);
        }
        #endregion

        #region Grid
        /// <summary>
        /// Returns HTML of Grid
        /// </summary>
        public BrowserCommandResult<string> GetGridControl()
        {

            return _client.Execute(_client.GetOptions($"Get Grid Control"), driver =>
            {
                var gridContainer = driver.FindElement("//div[contains(@data-lp-id,'MscrmControls.Grid')]");

                

                return gridContainer.GetAttribute(_client,"innerHTML");
            });
        }

        public BrowserCommandResult<Dictionary<string, Element>> OpenViewPicker(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Open View Picker"), driver =>
            {
                driver.ClickWhenAvailable(_client.ElementMapper.GridReference.ViewSelector,
                    TimeSpan.FromSeconds(20),
                    "Unable to click the View Picker"
                );

                var viewContainer = driver.FindElement(_client.ElementMapper.GridReference.ViewContainer);
                var viewItems = driver.FindElements(viewContainer.Locator + "//li");

                var result = new Dictionary<string, Element>();
                foreach (var viewItem in viewItems)
                {
                    var role = viewItem.GetAttribute(_client,"role");

                    if (role != "presentation")
                        continue;
                    if (!driver.HasElement(viewItem.Locator + "//label")) continue;
                    var key = driver.FindElement(viewItem.Locator + "//label").Text.ToLowerString();
                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    if (!result.ContainsKey(key))
                        result.Add(key, viewItem);
                }
                return result;
            });
        }
        /// <summary>
        /// Switches the view to the view supplied
        /// </summary>
        /// <param name="viewName">Name of the view to select</param>
        public BrowserCommandResult<bool> SwitchView(string viewName, string subViewName = null, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Switch View"), driver =>
            {
                var views = OpenViewPicker().Value;
                Thread.Sleep(500);
                var key = viewName.ToLowerString();
                bool success = views.TryGetValue(key, out var view);
                if (!success)
                    throw new InvalidOperationException($"No view with the name '{key}' exists.");

                view.Click(_client);

                if (subViewName != null)
                {
                    // TBD
                }

                driver.Wait();

                return true;
            });
        }

        /// <summary>
        /// Opens a record in the grid
        /// </summary>
        /// <param name="index">The index of the row to open</param>
        public BrowserCommandResult<bool> OpenRecord(int index, int thinkTime = Constants.DefaultThinkTime, bool checkRecord = false)
        {
            _client.ThinkTime(thinkTime);
            return _client.Execute(_client.GetOptions("Open Grid Record"), driver =>
            {
                //Generic Grid = //div[@data-type='Grid']
                //Editable Grid = //div[@aria-label='Editable Grid']

                var grid = driver.FindElement(_client.ElementMapper.GridReference.GridContainer);
                bool lastRow = false;
                Element gridRow = null;
                Grid.GridType gridType = Grid.GridType.PowerAppsGridControl;
                int lastRowInCurrentView = 0;
                string lastRowXPathLocator = _client.ElementMapper.GridReference.Row.Replace("[INDEX]", (index).ToString());
                while (!lastRow)
                {
                    //determine which grid
                    if (driver.HasElement(_client.ElementMapper.GridReference.Rows)) //Contacts
                    {
                        gridType = Grid.GridType.PowerAppsGridControl;
                        Trace.WriteLine("Found Power Apps Grid.");
                    }
                    else if (driver.HasElement(_client.ElementMapper.GridReference.LegacyReadOnlyRows))//Lead
                    {
                        gridType = Grid.GridType.LegacyReadOnlyGrid;
                        Trace.WriteLine("Found Legacy Read Only Grid.");
                    }
                    else if (driver.HasElement(_client.ElementMapper.GridReference.EditableGrid)) //Account
                    {
                        gridType = Grid.GridType.EditableGrid;
                        Trace.WriteLine("Found Editable Grid.");
                        lastRowXPathLocator = _client.ElementMapper.GridReference.EditableGridRows + "//div[@row-index='[INDEX]']".Replace("[INDEX]", (index).ToString());
                        //if (!grid.HasElement(By.XPath("//div[@role='row' and @aria-rowindex='[INDEX]']".Replace("[INDEX]", (index).ToString()))))
                        //{
                        //    lastRowInCurrentView = ClickGridAndPageDown(driver, grid, lastRowInCurrentView, gridType);
                        //}
                    }


                    if (!driver.HasElement(lastRowXPathLocator))
                    {
                        lastRowInCurrentView = ClickGridAndPageDown(driver, grid, lastRowInCurrentView, gridType);
                    }
                    else
                    {
                        gridRow = driver.FindElement(lastRowXPathLocator);
                        lastRow = true;
                    }
                    if (driver.HasElement(_client.ElementMapper.GridReference.LastRow))
                    {
                        lastRow = true;
                    }
                }
                if (gridRow == null) throw new KeyNotFoundException($"Grid Row {index} not found.");
                var xpathToGrid = "//div[contains(@data-id,'DataSetHostContainer')]";//works for: PowerAppsGridControl, LegacyReadOnlyControl, 
                Element control = driver.WaitUntilAvailable(xpathToGrid);

                //Func<Actions, Actions> action;
                //if (checkRecord)
                //    action = e => e.Click();
                //else
                //    action = e => e.DoubleClick();
                ////div[@class='ag-center-cols-container']//div[@row-index='[INDEX]']
                var xpathToCell = _client.ElementMapper.GridReference.Row.Replace("[INDEX]", index.ToString());

                var gridControlCell = driver.WaitUntilAvailable(control.Locator + xpathToCell);
                var emptyDiv = driver.WaitUntilAvailable(xpathToCell + "//div");
                switch (gridType)
                {
                    case Grid.GridType.LegacyReadOnlyGrid: //Lead
                        //driver.Perform(action, emptyDiv, null);
                        if (checkRecord) emptyDiv.Click(_client); else emptyDiv.DoubleClick(_client, emptyDiv.Locator);
                        break;
                    case Grid.GridType.ReadOnlyGrid:
                        //driver.Perform(action, emptyDiv, null);
                        if (checkRecord) emptyDiv.Click(_client); else emptyDiv.DoubleClick(_client, emptyDiv.Locator);
                        break;
                    case Grid.GridType.PowerAppsGridControl:
                        if (checkRecord) driver.FindElement(gridControlCell.Locator + _client.ElementMapper.GridReference.PowerAppsGridControlClickableCell).Click(_client); else driver.FindElement(gridControlCell.Locator + _client.ElementMapper.GridReference.PowerAppsGridControlClickableCell).DoubleClick(_client, gridControlCell.Locator + _client.ElementMapper.GridReference.PowerAppsGridControlClickableCell);//Contacts
                        break;
                    case Grid.GridType.EditableGrid:
                        if (checkRecord)
                            driver.FindElement(gridControlCell.Locator + "//a[contains(@aria-label,'Read only')]").Click(_client);
                            else driver.FindElement(gridControlCell.Locator + "//a[contains(@aria-label,'Read only')]").DoubleClick(_client, gridControlCell.Locator + "//a[contains(@aria-label,'Read only')]");
                        break;
                    default: throw new Exception("Did not find Read Only or Power Apps Grid.");
                }
                Trace.WriteLine("Clicked record.");


                //control.WaitUntilAvailable(xpathToCell,
                //    cell =>
                //    {
                //        var emptyDiv = cell.FindElement(cell.LocBy.TagName("div"));
                //        switch (gridType)
                //        {
                //            case Grid.GridType.LegacyReadOnlyGrid: //Lead
                //                driver.Perform(action, emptyDiv, null);
                //                emptyDiv.Click();
                //                break;
                //            case Grid.GridType.ReadOnlyGrid:
                //                driver.Perform(action, emptyDiv, null);
                //                break;
                //            case Grid.GridType.PowerAppsGridControl:
                //                cell.FindElement(_client.ElementMapper.GridReference.PowerAppsGridControlClickableCell).Click();//Contacts
                //                break;
                //            case Grid.GridType.EditableGrid:
                //                cell.FindElement("//a[contains(@aria-label,'Read only')]").Click();
                //                break;
                //            default: throw new Exception("Did not find Read Only or Power Apps Grid.");
                //        }
                //        Trace.WriteLine("Clicked record.");
                //    },
                //    $"An error occur trying to open the record at position {index}"
                //);

                driver.Wait();
                Trace.WriteLine("Click Record transaction complete.");
                if (driver.HasElement("//button[contains(@class,'ms-TeachingBubble-closebutton') and @data-is-focusable= 'true' and @aria-label='Dismiss']")){
                    Trace.WriteLine(String.Format("Found {0} Clickable Teaching Bubbles.", driver.FindElements("//button[contains(@class,'ms-TeachingBubble-closebutton') and @data-is-focusable= 'true' and @aria-label='Dismiss']").Count));
                    foreach (var item in driver.FindElements("//button[contains(@class,'ms-TeachingBubble-closebutton') and @data-is-focusable= 'true' and @aria-label='Dismiss']"))
                    {
                        item.Click(_client);
                    }
                }
                return true;
            });
        }
        /// <summary>
        /// Performs a Quick Find on the grid
        /// </summary>
        /// <param name="searchCriteria">Search term</param>
        /// <param name="clearByDefault">Determines whether to clear the quick find field before supplying the search term</param>
        public BrowserCommandResult<bool> Search(string searchCriteria, bool clearByDefault = true, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            //find search bar
            //input value
            //pass results back

            return _client.Execute(_client.GetOptions($"Search"), driver =>
            {
                driver.WaitUntilAvailable(_client.ElementMapper.GridReference.QuickFind);

                if (clearByDefault)
                {
                    driver.FindElement(_client.ElementMapper.GridReference.QuickFind).Clear(_client, _client.ElementMapper.GridReference.QuickFind);
                }

                driver.FindElement(_client.ElementMapper.GridReference.QuickFind).SendKeys(_client,new string[] { searchCriteria });
                driver.FindElement(_client.ElementMapper.GridReference.QuickFind).SendKeys(_client,new string[] { Keys.Enter });

                //driver.WaitForTransaction();

                return true;
            });
        }
        /// <summary>
        /// Clears the quick find field
        /// </summary>
        public BrowserCommandResult<bool> ClearSearch(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Clear Search"), driver =>
            {
                driver.WaitUntilAvailable(_client.ElementMapper.GridReference.QuickFind);

                driver.FindElement(_client.ElementMapper.GridReference.QuickFind).Clear(_client, _client.ElementMapper.GridReference.QuickFind);

                return true;
            });
        }

        public BrowserCommandResult<bool> ShowChart(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Show Chart"), driver =>
            {
                if (driver.HasElement(_client.ElementMapper.GridReference.ShowChart))
                {
                    driver.ClickWhenAvailable(_client.ElementMapper.GridReference.ShowChart);

                    driver.Wait();
                }
                else
                {
                    throw new Exception("The Show Chart button does not exist.");
                }

                return true;
            });
        }

        public BrowserCommandResult<bool> HideChart(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Hide Chart"), driver =>
            {
                if (driver.HasElement(_client.ElementMapper.GridReference.HideChart))
                {
                    driver.ClickWhenAvailable(_client.ElementMapper.GridReference.HideChart);

                    driver.Wait();
                }
                else
                {
                    throw new Exception("The Hide Chart button does not exist.");
                }

                return true;
            });
        }
        /// <summary>
        /// Filter the grid by the Character provided
        /// </summary>
        /// <param name="filter">Label of footer filter - View records that starts with this letter</param>
        public BrowserCommandResult<bool> FilterByLetter(char filter, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            if (!Char.IsLetter(filter) && filter != '#')
                throw new InvalidOperationException("Filter criteria is not valid.");

            return _client.Execute(_client.GetOptions("Filter by Letter"), driver =>
            {
                var jumpBar = driver.FindElement(_client.ElementMapper.GridReference.JumpBar);
                var link = driver.FindElement(jumpBar.Locator + filter + "_link");
                //var link = driver.FindElement(By.Id(jumpBar.Locator + filter + "_link"));
                if (link != null)
                {
                    link.Click(_client);

                    driver.Wait();
                }
                else
                {
                    throw new Exception($"Filter with letter: {filter} link does not exist");
                }

                return true;
            });
        }

        public BrowserCommandResult<bool> FilterByAll(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Filter by All Records"), driver =>
            {
                var jumpBar = driver.FindElement(_client.ElementMapper.GridReference.JumpBar);
                var link = driver.FindElement(jumpBar.Locator + _client.ElementMapper.GridReference.FilterByAll);

                if (link != null)
                {
                    link.Click(_client);

                    driver.Wait();
                }
                else
                {
                    throw new Exception($"Filter by All link does not exist");
                }

                return true;
            });
        }

        public BrowserCommandResult<bool> SelectRecord(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Select Grid Record"), driver =>
            {
                var container = driver.WaitUntilAvailable(_client.ElementMapper.GridReference.RowsContainer, "Grid Container does not exist.");

                var row = driver.FindElement(container.Locator + "id-cell-" + index + "-1");
                //var row = container.FindElement(By.Id("id-cell-" + index + "-1"));
                if (row == null)
                    throw new Exception($"Row with index: {index} does not exist.");

                row.Click(_client);
                return true;
            });
        }

        public BrowserCommandResult<bool> SwitchChart(string chartName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            if (!_client.Browser.Browser.HasElement(_client.ElementMapper.GridReference.ChartSelector))
                ShowChart();

            _client.ThinkTime(1000);

            return _client.Execute(_client.GetOptions("Switch Chart"), driver =>
            {
                driver.ClickWhenAvailable(_client.ElementMapper.GridReference.ChartSelector);

                var list = driver.FindElement(_client.ElementMapper.GridReference.ChartViewList);

                driver.ClickWhenAvailable("//li[contains(@title,'" + chartName + "')]");

                return true;
            });
        }
        /// <summary>
        /// Sorts the grid by the column provided
        /// </summary>
        /// <param name="columnName">Label of the column name</param>
        /// <param name="sortOptionButtonText">Sort option button text</param>
        public BrowserCommandResult<bool> Sort(string columnName, string sortOptionButtonText, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Sort by {columnName}"), driver =>
            {
                var sortCol = driver.FindElement(_client.ElementMapper.GridReference.GridSortColumn.Replace("[COLNAME]", columnName));

                if (sortCol == null)
                    throw new InvalidOperationException($"Column: {columnName} Does not exist");
                else
                {
                    sortCol.Click(_client);
                    driver.WaitUntilAvailable($@"//button[@name='{sortOptionButtonText}']").Click(_client);
                }

                driver.Wait();
                return true;
            });
        }

        #endregion
        #region Private
        private int ClickGridAndPageDown(IWebBrowser driver, Element grid, int lastKnownFloor, Grid.GridType gridType)
        {
            //Actions actions = new Actions(driver);
            string rowGroupLocator = null;
            string topRowLocator = null;
            switch (gridType)
            {
                case Grid.GridType.LegacyReadOnlyGrid:
                    rowGroupLocator = _client.ElementMapper.GridReference.LegacyReadOnlyRows;
                    topRowLocator = _client.ElementMapper.GridReference.Rows;
                    break;
                case Grid.GridType.ReadOnlyGrid:
                    rowGroupLocator = _client.ElementMapper.GridReference.Rows;
                    topRowLocator = _client.ElementMapper.GridReference.Rows;
                    break;
                case Grid.GridType.PowerAppsGridControl:
                    rowGroupLocator = _client.ElementMapper.GridReference.Rows;
                    topRowLocator = _client.ElementMapper.GridReference.Rows;
                    break;
                case Grid.GridType.EditableGrid:
                    rowGroupLocator = _client.ElementMapper.GridReference.EditableGridRows;
                    topRowLocator = _client.ElementMapper.GridReference.EditableGridRows + "//div[@role='row']";
                    break;
                default:
                    break;
            }
            var CurrentRows = driver.FindElements(rowGroupLocator);
            var lastFloor = CurrentRows.Where(x => Convert.ToInt32(x.GetAttribute(_client,"row-index")) == lastKnownFloor).First();
            //var topRow = driver.FindElement(topRowLocator);
            var topRow = CurrentRows.First();
            var firstCell = driver.FindElement(lastFloor.Locator + "//div[@aria-colindex='1']");
            lastFloor.Click(_client);
            driver.SendKeys(rowGroupLocator, new string[] { Keys.PageDown });
            return Convert.ToInt32(driver.FindElements(rowGroupLocator).Last().GetAttribute(_client,"row-index"));
        }
        internal static string GetGridQueryKey(IWebBrowser driver, string dataSetName = null)
        {
            Dictionary<string, object> pages = (Dictionary<string, object>)driver.ExecuteScript($"return window[Object.keys(window).find(i => !i.indexOf(\"__store$\"))].getState().pages");
            //This is the current view
            Dictionary<string, object> pageData = (Dictionary<string, object>)pages.Last().Value;
            IList<KeyValuePair<string, object>> datasets = pageData.Where(i => i.Key == "datasets").ToList();
            //Get Entity From Page List
            Dictionary<string, object> entityName = null;
            if (dataSetName != null)
            {
                foreach (KeyValuePair<string, object> dataset in datasets)
                {
                    foreach (KeyValuePair<string, object> datasetList in (Dictionary<string, object>)dataset.Value)
                    {
                        if (datasetList.Key == dataSetName)
                        {
                            entityName = (Dictionary<string, object>)datasetList.Value;
                            return (string)entityName["queryKey"];
                        }
                    }

                }
                throw new Exception("Invalid DataSet Name");
            }
            else
            {
                entityName = (Dictionary<string, object>)datasets[0].Value;
                Dictionary<string, object> entityQueryListList = (Dictionary<string, object>)entityName.First().Value;
                return (string)entityQueryListList["queryKey"];
            }
            if (entityName == null) throw new Exception("Invalid DataSet Name");
        }
        private static void ProcessGridRowAttributes(Dictionary<string, object> attributes, GridItem gridItem)
        {
            foreach (string attributeKey in attributes.Keys)
            {

                var serializedString = JsonConvert.SerializeObject(attributes[attributeKey]);
                var deserializedRecord = JsonConvert.DeserializeObject<SerializedGridItem>(serializedString);
                if (deserializedRecord.value != null)
                {
                    gridItem[attributeKey] = deserializedRecord.value;
                }
                else if (deserializedRecord.label != null)
                {
                    gridItem[attributeKey] = deserializedRecord.label;
                }
                else if (deserializedRecord.id != null)
                {
                    gridItem[attributeKey] = deserializedRecord.id.guid;
                }
                else if (deserializedRecord.reference != null)
                {
                    gridItem[attributeKey] = deserializedRecord.reference.id.guid;
                }
            }
        }
        /// <summary>
        /// Returns a list of items from the current grid
        /// </summary>
        /// <returns></returns>
        public BrowserCommandResult<List<GridItem>> GetGridItems(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Get Grid Items"), driver =>
            {
                var returnList = new List<GridItem>();
                //#1294
                var gridContainer = driver.FindElement("//div[contains(@data-id,'data-set-body-container')]/div");
                string[] gridDataId = gridContainer.GetAttribute(_client, "data-lp-id").Split('|');
                Dictionary<string, object> WindowStateData = (Dictionary<string, object>)driver.ExecuteScript($"return window[Object.keys(window).find(i => !i.indexOf(\"__store$\"))].getState().data");
                string keyForData = GetGridQueryKey(driver, null);
                //Get Data Store

                Dictionary<string, object> WindowStateDataLists = (Dictionary<string, object>)WindowStateData["lists"];

                //Find Data by Key
                Dictionary<string, object> WindowStateDataKeyedForData = (Dictionary<string, object>)WindowStateDataLists[keyForData];
                //Find Record Ids for Key Data Set
                ReadOnlyCollection<object> WindowStateDataKeyedForDataRecordsIds = (ReadOnlyCollection<object>)WindowStateDataKeyedForData["records"];

                //Get Data
                Dictionary<string, object> WindowStateEntityData = (Dictionary<string, object>)WindowStateData["entities"];

                if (!WindowStateEntityData.ContainsKey(gridDataId[2]))
                {
                    return returnList;

                }
                Dictionary<string, object> WindowStateEntityDataEntity = (Dictionary<string, object>)WindowStateEntityData[gridDataId[2]];
                foreach (Dictionary<string, object> record in WindowStateDataKeyedForDataRecordsIds)
                {
                    Dictionary<string, object> recordId = (Dictionary<string, object>)record["id"];
                    Dictionary<string, object> definedRecord = (Dictionary<string, object>)WindowStateEntityDataEntity[(string)recordId["guid"]];
                    Dictionary<string, object> attributes = (Dictionary<string, object>)definedRecord["fields"];
                    GridItem gridItem = new GridItem()
                    {
                        EntityName = gridDataId[2],
                        Id = new Guid((string)recordId["guid"])
                    };
                    ProcessGridRowAttributes(attributes, gridItem);
                    returnList.Add(gridItem);
                }
                return returnList;
            });
        }

        internal BrowserCommandResult<bool> NextPage(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Next Page"), driver =>
            {
                driver.ClickWhenAvailable(_client.ElementMapper.GridReference.NextPage);

                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> PreviousPage(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Previous Page"), driver =>
            {
                driver.ClickWhenAvailable(_client.ElementMapper.GridReference.PreviousPage);

                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> FirstPage(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"First Page"), driver =>
            {
                driver.ClickWhenAvailable(_client.ElementMapper.GridReference.FirstPage);

                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectAll(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Select All"), driver =>
            {
                driver.ClickWhenAvailable(_client.ElementMapper.GridReference.SelectAll);

                driver.Wait();

                return true;
            });
        }

        #endregion
    }
}