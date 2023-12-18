// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using Newtonsoft.Json;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Grid : Element
    {
        #region DTO
        public static class GridReference
        {
            public static string Container = "//div[@data-id='data-set-body-container']";
            public static string PcfContainer = "//div[@ref='eViewport']";
            public static string QuickFind = "//*[contains(@id, \'quickFind_text\')]";
            public static string FirstPage = "//button[contains(@data-id,'loadFirstPage')]";
            public static string NextPage = "//button[contains(@data-id,'moveToNextPage')]";
            public static string PreviousPage = "//button[contains(@data-id,'moveToPreviousPage')]";
            public static string SelectAll = "//button[contains(@title,'Select All')]";
            public static string ShowChart = "//button[contains(@aria-label,'Show Chart')]";
            public static string HideChart = "Grid_HideChart";
            public static string JumpBar = "//*[@id=\"JumpBarItemsList\"]";
            public static string FilterByAll = "//*[@id=\"All_link\"]";
            public static string RowsContainerCheckbox = "//div[@role='checkbox']";
            public static string RowsContainer = "//div[contains(@role,'grid')]";
            public static string LegacyReadOnlyRows = "//div[@data-id='grid-container']//div[@class='ag-center-cols-container']//div[@role='row']";
            public static string Rows = "//div[@data-id='entity_control-powerapps_onegrid_control_container']//div[@class='ag-center-cols-container']//div[@role='row']";
            public static string Row = "//div[@class='ag-center-cols-container']//div[@row-index=\'[INDEX]\']";
            public static string LastRow = "//div[@data-id='entity_control-pcf_grid_control_container']//div[@ref='centerContainer']//div[@role='rowgroup']//div[contains(@class, 'ag last-row')]";
            public static string Columns = "//div[contains(@ref,'gridHeader')]";
            public static string Control = "//div[contains(@data-lp-id, 'MscrmControls.Grid.PCFGridControl')]";
            public static string ChartSelector = "//span[contains(@id,'ChartSelector')]";
            public static string ChartViewList = "//ul[contains(@role,'listbox')]";
            public static string GridSortColumn = "//div[contains(@role,'columnheader')]//label[@title='[COLNAME]']";
            public static string Cells = ".//div[@role='gridcell']";
            public static string CellContainer = "//div[@role='grid'][@data-id='grid-cell-container']";
            public static string ViewSelector = "//button[contains(@id,'ViewSelector')]";
            public static string ViewContainer = "//div[contains(@aria-label,'Views')]";
            public static string ViewSelectorMenuItem = ".//li[contains(@class, 'ms-ContextualMenu-item')]";
            public static string SubArea = "//*[contains(@data-id,'[NAME]')]";
        }
        #endregion
        private readonly WebClient _client;
        public enum GridType { PowerAppsGridControl, LegacyReadOnlyGrid, ReadOnlyGrid }
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
                var gridContainer = driver.FindElement(By.XPath("//div[contains(@data-lp-id,'MscrmControls.Grid')]"));

                return gridContainer.GetAttribute("innerHTML");
            });
        }

        public BrowserCommandResult<Dictionary<string, IWebElement>> OpenViewPicker(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Open View Picker"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(GridReference.ViewSelector),
                    TimeSpan.FromSeconds(20),
                    "Unable to click the View Picker"
                );

                var viewContainer = driver.FindElement(By.XPath(GridReference.ViewContainer));
                var viewItems = viewContainer.FindElements(By.TagName("li"));

                var result = new Dictionary<string, IWebElement>();
                foreach (var viewItem in viewItems)
                {
                    var role = viewItem.GetAttribute("role");

                    if (role != "presentation")
                        continue;

                    //var key = viewItem.FindElement(By.XPath(GridReference.ViewSelectorMenuItem])).Text.ToLowerString();
                    var key = viewItem.Text.ToLowerString();
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

                view.Click(true);

                if (subViewName != null)
                {
                    // TBD
                }

                driver.WaitForTransaction();

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
                var grid = driver.FindElement(By.XPath(GridReference.PcfContainer));
                bool lastRow = false;
                IWebElement gridRow = null;
                Grid.GridType gridType = Grid.GridType.PowerAppsGridControl;
                int lastRowInCurrentView = 0;
                while (!lastRow)
                {
                    //determine which grid
                    if (driver.HasElement(By.XPath(GridReference.Rows)))
                    {
                        gridType = Grid.GridType.PowerAppsGridControl;
                        Trace.WriteLine("Found Power Apps Grid.");
                    }
                    else if (driver.HasElement(By.XPath(GridReference.LegacyReadOnlyRows)))
                    {
                        gridType = Grid.GridType.LegacyReadOnlyGrid;
                        Trace.WriteLine("Found Legacy Read Only Grid.");
                    }


                    if (!driver.HasElement(By.XPath(GridReference.Row.Replace("[INDEX]", (index).ToString()))))
                    {
                        lastRowInCurrentView = ClickGridAndPageDown(driver, grid, lastRowInCurrentView, gridType);
                    }
                    else
                    {
                        gridRow = driver.FindElement(By.XPath(GridReference.Row.Replace("[INDEX]", index.ToString())));
                        lastRow = true;
                    }
                    if (driver.HasElement(By.XPath(GridReference.LastRow)))
                    {
                        lastRow = true;
                    }
                }
                if (gridRow == null) throw new NotFoundException($"Grid Row {index} not found.");
                var xpathToGrid = By.XPath("//div[contains(@data-id,'DataSetHostContainer')]");
                IWebElement control = driver.WaitUntilAvailable(xpathToGrid);

                Func<Actions, Actions> action;
                if (checkRecord)
                    action = e => e.Click();
                else
                    action = e => e.DoubleClick();
                var xpathToCell = By.XPath(GridReference.Row.Replace("[INDEX]", index.ToString()));
                control.WaitUntilClickable(xpathToCell,
                    cell =>
                    {
                        var emptyDiv = cell.FindElement(By.TagName("div"));
                        switch (gridType)
                        {
                            case Grid.GridType.LegacyReadOnlyGrid:
                                driver.Perform(action, emptyDiv, null);
                                break;
                            case Grid.GridType.ReadOnlyGrid:
                                driver.Perform(action, emptyDiv, null);
                                break;
                            case Grid.GridType.PowerAppsGridControl:
                                cell.FindElement(By.XPath("//a[contains(@aria-label,'Read only')]")).Click();
                                break;
                            default: throw new InvalidSelectorException("Did not find Read Only or Power Apps Grid.");
                        }
                        Trace.WriteLine("Clicked record.");
                    },
                    $"An error occur trying to open the record at position {index}"
                );

                driver.WaitForTransaction();
                Trace.WriteLine("Click Record transaction complete.");
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

            return _client.Execute(_client.GetOptions($"Search"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(GridReference.QuickFind));

                if (clearByDefault)
                {
                    driver.FindElement(By.XPath(GridReference.QuickFind)).Clear();
                }

                driver.FindElement(By.XPath(GridReference.QuickFind)).SendKeys(searchCriteria);
                driver.FindElement(By.XPath(GridReference.QuickFind)).SendKeys(Keys.Enter);

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
                driver.WaitUntilClickable(By.XPath(GridReference.QuickFind));

                driver.FindElement(By.XPath(GridReference.QuickFind)).Clear();

                return true;
            });
        }

        public BrowserCommandResult<bool> ShowChart(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Show Chart"), driver =>
            {
                if (driver.HasElement(By.XPath(GridReference.ShowChart)))
                {
                    driver.ClickWhenAvailable(By.XPath(GridReference.ShowChart));

                    driver.WaitForTransaction();
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
                if (driver.HasElement(By.XPath(GridReference.HideChart)))
                {
                    driver.ClickWhenAvailable(By.XPath(GridReference.HideChart));

                    driver.WaitForTransaction();
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
                var jumpBar = driver.FindElement(By.XPath(GridReference.JumpBar));
                var link = jumpBar.FindElement(By.Id(filter + "_link"));

                if (link != null)
                {
                    link.Click();

                    driver.WaitForTransaction();
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
                var jumpBar = driver.FindElement(By.XPath(GridReference.JumpBar));
                var link = jumpBar.FindElement(By.XPath(GridReference.FilterByAll));

                if (link != null)
                {
                    link.Click();

                    driver.WaitForTransaction();
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
                var container = driver.WaitUntilAvailable(By.XPath(GridReference.RowsContainer), "Grid Container does not exist.");

                var row = container.FindElement(By.Id("id-cell-" + index + "-1"));
                if (row == null)
                    throw new Exception($"Row with index: {index} does not exist.");

                row.Click();
                return true;
            });
        }

        public BrowserCommandResult<bool> SwitchChart(string chartName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            if (!_client.Browser.Driver.IsVisible(By.XPath(GridReference.ChartSelector)))
                ShowChart();

            _client.ThinkTime(1000);

            return _client.Execute(_client.GetOptions("Switch Chart"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(GridReference.ChartSelector));

                var list = driver.FindElement(By.XPath(GridReference.ChartViewList));

                driver.ClickWhenAvailable(By.XPath("//li[contains(@title,'" + chartName + "')]"));

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
                var sortCol = driver.FindElement(By.XPath(GridReference.GridSortColumn.Replace("[COLNAME]", columnName)));

                if (sortCol == null)
                    throw new InvalidOperationException($"Column: {columnName} Does not exist");
                else
                {
                    sortCol.Click(true);
                    driver.WaitUntilClickable(By.XPath($@"//button[@name='{sortOptionButtonText}']")).Click(true);
                }

                driver.WaitForTransaction();
                return true;
            });
        }

        #endregion
        #region Private
        private static int ClickGridAndPageDown(IWebDriver driver, IWebElement grid, int lastKnownFloor, Grid.GridType gridType)
        {
            Actions actions = new Actions(driver);
            By rowGroupLocator = null;
            By topRowLocator = null;
            switch (gridType)
            {
                case Grid.GridType.LegacyReadOnlyGrid:
                    rowGroupLocator = By.XPath(GridReference.LegacyReadOnlyRows);
                    topRowLocator = By.XPath(GridReference.Rows);
                    break;
                case Grid.GridType.ReadOnlyGrid:
                    rowGroupLocator = By.XPath(GridReference.Rows);
                    topRowLocator = By.XPath(GridReference.Rows);
                    break;
                case Grid.GridType.PowerAppsGridControl:
                    rowGroupLocator = By.XPath(GridReference.Rows);
                    topRowLocator = By.XPath(GridReference.Rows);
                    break;
                default:
                    break;
            }
            var CurrentRows = driver.FindElements(rowGroupLocator);
            var lastFloor = CurrentRows.Where(x => Convert.ToInt32(x.GetAttribute("row-index")) == lastKnownFloor).First();
            //var topRow = driver.FindElement(topRowLocator);
            var topRow = CurrentRows.First();
            var firstCell = lastFloor.FindElement(By.XPath("//div[@aria-colindex='1']"));
            lastFloor.Click();
            actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Perform();
            return Convert.ToInt32(driver.FindElements(rowGroupLocator).Last().GetAttribute("row-index"));
        }
        internal static string GetGridQueryKey(IWebDriver driver, string dataSetName = null)
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
                var gridContainer = driver.FindElement(By.XPath("//div[contains(@data-id,'data-set-body-container')]/div"));
                string[] gridDataId = gridContainer.GetAttribute("data-lp-id").Split('|');
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
                driver.ClickWhenAvailable(By.XPath(GridReference.NextPage));

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> PreviousPage(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Previous Page"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(GridReference.PreviousPage));

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> FirstPage(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"First Page"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(GridReference.FirstPage));

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectAll(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Select All"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(GridReference.SelectAll));

                driver.WaitForTransaction();

                return true;
            });
        }

        #endregion
    }
}