// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
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
                //Need to click the <div>, not the <a>.  Selenium FindElements By.XPath misbehaved when trying to break into rows and cells
                //Get a collection of cells and find the cell with the record name
                var cells = driver.FindElements(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.CellsContainer]));
                var cell = cells.FirstOrDefault(c => c.Text.Equals(name, StringComparison.OrdinalIgnoreCase));

                if(cell == null)
                    throw new InvalidOperationException($"No record with the name '{name}' exists in the grid.");

                cell.Click(true);


                /*bool found = false;

                foreach (var cell in cells)
                {
                    if (cell.Text.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        cell.Click(true);
                        break;
                    }
                }

                if(!found)
                    throw new InvalidOperationException($"No record with the name '{name}' exists in the grid.");
                    */
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
    }
}
