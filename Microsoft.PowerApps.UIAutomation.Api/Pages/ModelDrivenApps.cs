// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  The Home page.
    ///  </summary>
    public class ModelDrivenApps
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public ModelDrivenApps(InteractiveBrowser browser)
            : base(browser)
        {
        }

        public BrowserCommandResult<bool> SelectGridRecord(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Select Grid Record"), driver =>
            {
                return true;
            });
        }
        public BrowserCommandResult<bool> SelectGridRecord(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Select Grid Record"), driver =>
            {
                SelectGridRecord(name, true, thinkTime);
                return true;
            });
        }
        public BrowserCommandResult<bool> SelectGridRecord(string name, bool clickRightSide, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Select Grid Record"), driver =>
            {
                //Need to click the <div>, not the <a>.  Selenium FindElements By.XPath misbehaved when trying to break into rows and cells
                //Get a collection of cells and find the cell with the record name
                var cells = driver.FindElements(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.CellsContainer]));
                var cell = cells.FirstOrDefault(c => c.Text.Equals(name, StringComparison.OrdinalIgnoreCase));

                if(cell == null)
                    throw new InvalidOperationException($"No record with the name '{name}' exists in the grid.");

                //In the Solution grid, clicking the element opens the solution in the CRM org.  Clicking the far right side of the div highlights the row
                if (clickRightSide)
                {
                    //Ignore StaleElementReferenceExceptions?????
                    Actions act = new Actions(driver);
                    act.MoveToElement(cell).MoveByOffset((cell.Size.Width / 2) - 2, 0).Click().Perform();
                }
                else
                    cell.Click(true);

                return true;
            });
        }
        public BrowserCommandResult<bool> WaitForProcessingToComplete(string solutionName)
        {
            return this.Execute(GetOptions("Wait For Processing To Complete"), driver =>
            {
                string currentStatus = GetCurrentStatus(solutionName);

                if (currentStatus.Contains("running", StringComparison.OrdinalIgnoreCase))
                    WaitUntilStatusChanges(solutionName, currentStatus, 3600);

                return true;
            });
        }
        public BrowserCommandResult<bool> VerifyManagedSolutionsUnavailable(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Verify Managed Soltuions Unavailable"), driver =>
            {
                List<SolutionGridRow> solutions = GetSolutionTableRows();

                var solutionRows = solutions.Where(s => s.ManagedExternally.Equals("Yes", StringComparison.OrdinalIgnoreCase));
                foreach(var row in solutionRows)
                {
                    SelectGridRecord(row.Name, true);

                    CommandBar bar = new CommandBar(Browser);
                    var commands = bar.GetVisibleCommands(250);
                    
                    //Converting the collection ToList() is not great for performance
                    if(commands.Value.ToList().Exists(cmd => cmd.Text.Contains("Solution checker", StringComparison.OrdinalIgnoreCase)))
                        throw new InvalidOperationException("Solution checker button should not be present");
                }

                return true;
            });
        }

        internal bool WaitUntilStatusChanges(string solutionName, string currentStatus, int maxWaitTimeInSeconds)
        {
            var driver = Browser.Driver;
            bool state = false;
            try
            {
                //Poll every half second to see if UCI is idle
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(500));
                wait.Until(d =>
                {
                    try
                    {
                        string thisStatus = GetCurrentStatus(solutionName);
                        //This is specific to DownloadResults - looking specifically for a status that begins with Results...
                        if (!thisStatus.Equals(currentStatus) && thisStatus.Contains("Results", StringComparison.OrdinalIgnoreCase))
                            state = true;
                    }
                    catch (TimeoutException)
                    {

                    }
                    catch (NullReferenceException)
                    {

                    }

                    return state;
                });
            }
            catch (Exception)
            {

            }

            return state;
        }
        internal string GetCurrentStatus(string solutionName)
        {
            var driver = Browser.Driver;
            //Retrieve current status.  XPath was misbehaving trying to traverse rows and columns so we have to cheat.
            var solutionNames = driver.FindElements(By.XPath(Elements.Xpath[Reference.CommandBar.GridSolutionNameColumn]));
            var solutionStatuses = driver.FindElements(By.XPath(Elements.Xpath[Reference.CommandBar.GridSolutionStatusColumn]));

            int rowNumber = -1;
            int cnt = 0;

            foreach (var row in solutionNames)
            {
                if (row.Text.Contains(solutionName, StringComparison.OrdinalIgnoreCase))
                {
                    rowNumber = cnt;
                    break;
                }

                cnt++;
            }

            if (rowNumber == -1)
                Console.WriteLine("Could not find status for this solution");
            return solutionStatuses[rowNumber].Text;
        }
        internal List<SolutionGridRow> GetSolutionTableRows()
        {
            var driver = Browser.Driver;

            List<String> columnHeaders = new List<string>();
            List<SolutionGridRow> tableRows = new List<SolutionGridRow>();

            var rows = driver.FindElements(By.XPath("//div[@role='row']"));
            foreach (var row in rows)
            {
                List<String> cellValues = new List<string>();

                var cells = row.FindElements(By.TagName("div"));
                foreach (var cell in cells)
                {
                    if (cell.GetAttribute("data-automationid") != null)
                    {
                        //If it's the header row, figure out the column order.  If it's a record, collect all of the values
                        switch (cell.GetAttribute("data-automationid").ToLower())
                        {
                            case "columnsheadercolumn":
                                if (!columnHeaders.Contains(cell.Text))
                                    columnHeaders.Add(cell.Text);
                                break;
                            case "detailsrowcell":
                                cellValues.Add(cell.Text);
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (cellValues.Count > 0)
                    tableRows.Add(new SolutionGridRow(cellValues[columnHeaders.FindIndex(x => x.Equals("name", StringComparison.OrdinalIgnoreCase))], cellValues[columnHeaders.FindIndex(x => x.Contains("Managed externally", StringComparison.OrdinalIgnoreCase))]));
            }

            return tableRows;
        }
    }
    internal class SolutionGridRow
    {
        public String Name { get; set; }
        public String ManagedExternally { get; set; }

        public SolutionGridRow(String name, String managedExternally)
        {
            this.Name = name;
            this.ManagedExternally = managedExternally;
        }
    }
}
