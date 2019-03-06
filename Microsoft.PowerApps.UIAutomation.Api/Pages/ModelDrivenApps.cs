// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public BrowserCommandResult<string> CheckForErrors(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Check for Message Bar Errors"), driver =>
            {
                string messageBarText = "";                
                bool isMessageBarVisible = driver.IsVisible(By.ClassName("ms-MessageBar"));

                if (isMessageBarVisible)
                {
                    char[] trimCharacters = { '', '\r', '\n', '', '\r','\n','' };
                    messageBarText = driver.FindElement(By.ClassName("ms-MessageBar")).Text.Trim(trimCharacters);
                }


                return messageBarText;
            });
        }

        /// <summary>
        /// Closes a Notification container if present/> class.
        /// </summary>
        public BrowserCommandResult<bool> CloseNotification(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Close Notification"), driver =>
            {
                bool isNotificationVisible = driver.IsVisible(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.NotificationContainer]));

                if (isNotificationVisible)
                {
                    var notificationContainer = driver.FindElement(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.NotificationContainer]));

                    var innerDiv = notificationContainer.FindElements(By.TagName("div"));

                    innerDiv[0].Click(true);
                }

                return isNotificationVisible;
            });
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

                if (cell == null)
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

        public BrowserCommandResult<bool> MoreCommands(string solutionName, string commandName, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions("Click More Commands Button"), driver =>
            {
                MoreCommands(solutionName, commandName, "", thinkTime);
                return true;
            });
        }
        public BrowserCommandResult<bool> MoreCommands(string solutionName, string commandName, string subButton = "", int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Click More Commands Button"), driver =>
            {
                ClickMoreCommandsButton(solutionName, commandName, subButton, false);
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
        public BrowserCommandResult<bool> VerifyManagedSolutionsUnavailable(string commandBarButton, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Verify Managed Solutions Unavailable"), driver =>
            {

                // Due to grid column changes, change filter to Managed solutions only
                string buttonName = "All";
                string subButton = "Managed";
                CommandBar cmdBar = new CommandBar(Browser);
                cmdBar.ClickCommand(buttonName, subButton);

                Browser.ThinkTime(5000);

                List<SolutionGridRow> solutions = GetSolutionTableRows();

                foreach (var row in solutions)
                {
                    SelectGridRecord(row.Name, true);

                    Browser.ThinkTime(500);

                    var commands = cmdBar.GetVisibleCommands(1000);

                    //Converting the collection ToList() is not great for performance
                    if (commands.Value.ToList().Exists(cmd => cmd.Text.Contains(commandBarButton, StringComparison.OrdinalIgnoreCase)))
                        throw new InvalidOperationException($"{commandBarButton} button should not be present");
                }


                return true;
            });
        }

        public BrowserCommandResult<bool> VerifyButtonIsClickable(string solutionName, string commandName, string subButton, bool throwExceptionIfVisible)
        {
            return this.Execute(GetOptions("Verify Button is Clickable"), driver =>
            {
                bool isDisabled = IsButtonDisabled(solutionName, commandName, subButton);

                if (throwExceptionIfVisible && !isDisabled)
                    throw new InvalidOperationException($"SubButton '{subButton}' should not be visible.");

                return true;
            });
        }

        internal bool ClickMoreCommandsButton(string solutionName, string commandName, string subButton = "", bool throwExceptionIfVisible = false)
        {
            var driver = Browser.Driver;

            //Need to click the <div>, not the <a>.  Selenium FindElements By.XPath misbehaved when trying to break into rows and cells
            //Get a collection of cells and find the cell with the record name
            driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.CellsContainer]),new TimeSpan(0,0,5));
            var cells = driver.FindElements(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.CellsContainer]));
            var cell = cells.FirstOrDefault(c => c.Text.Equals(solutionName, StringComparison.OrdinalIgnoreCase));

            if (cell == null)
                throw new InvalidOperationException($"No record with the name '{solutionName}' exists in the grid.");

            //Click on the More Commands menu
            var moreCommandsButton = cell.FindElement(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.MoreCommandsButton]));
            moreCommandsButton.Click(true);

            //First Command button
            driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.MoreCommandsContainer]));
            var moreCommandsContainer = driver.FindElement(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.MoreCommandsContainer]));
            var buttons = moreCommandsContainer.FindElements(By.TagName("button"));
            var button = buttons.FirstOrDefault(b => b.Text.Contains(commandName, StringComparison.OrdinalIgnoreCase));

            if (button == null)
                throw new InvalidOperationException($"No command with the name '{commandName}' exists inside of Commandbar.");

            button.Click(true);

            //Sub Command Button
            if (!string.IsNullOrEmpty(subButton))
            {
                //found = false;
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.SubButtonContainer]),new TimeSpan(0,0,5));
                var subButtonContainer = driver.FindElements(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.SubButtonContainer]));
                var subButtons = subButtonContainer[1].FindElements(By.TagName("button"));

                var sButton = subButtons.FirstOrDefault(b => b.Text.Contains(subButton, StringComparison.OrdinalIgnoreCase));

                if (sButton == null)
                    throw new InvalidOperationException($"No subButton with the name '{subButton}' exists inside of the More Commands menu.");

                //Is the button visible?
                bool isDisabled;
                var currentVisibleStatus = sButton.GetAttribute("aria-disabled");
                bool.TryParse(currentVisibleStatus, out isDisabled);

                if (!isDisabled)
                    sButton.Click(true);
            }

            return true;
        }

        internal bool IsButtonDisabled(string solutionName, string commandName, string subButton = "")
        {
            var driver = Browser.Driver;
            bool isDisabled = true;

            ClickMoreCommandsButton(solutionName, commandName, "");

            driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.SubButtonContainer]),
                new TimeSpan(0, 0, 5),
                e =>
                    {
                        Console.WriteLine("Located SubButton Container");
                    },
                f =>
                    {
                        throw new InvalidOperationException("Unable to locate SubButton Container within 5 seconds.");
                    });

            var subButtonContainer = driver.FindElements(By.XPath(Elements.Xpath[Reference.ModelDrivenApps.SubButtonContainer]));

            // This condition should never be hit. Legacy code.
            if (subButtonContainer.Count == 0 || subButtonContainer is null)
                throw new InvalidOperationException("SubButton container is empty");

            var subButtons = subButtonContainer[1].FindElements(By.TagName("button"));
            var sButton = subButtons.FirstOrDefault(b => b.Text.Equals(subButton, StringComparison.OrdinalIgnoreCase));

            try
            {
                bool.TryParse(sButton.GetAttribute("aria-disabled"), out isDisabled);
                Console.WriteLine($"isDisabled value for {subButton} in the solution grid is: {isDisabled} ");
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"An error occurred trying to validate that the button '{subButton}' is disabled: {exc}");
            }

            ClickMoreCommandsButton(solutionName, commandName, "");

            return isDisabled;
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
                    catch (TimeoutException timeout)
                    {
                        throw new InvalidOperationException($"Error: Timeout Exception occurred waiting for analysis to complete. {timeout}");
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
        public string GetCurrentStatus(string solutionName)
        {
            var driver = Browser.Driver;
            driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.CommandBar.GridSolutionNameColumn]));
            
            //Retrieve current status.  XPath was misbehaving trying to traverse rows and columns so we have to cheat.
            var solutionNames = driver.FindElements(By.XPath(Elements.Xpath[Reference.CommandBar.GridSolutionNameColumn]));
            var solutionStatuses = driver.FindElements(By.XPath(Elements.Xpath[Reference.CommandBar.GridSolutionStatusColumn]));

            int rowNumber = -1;
            int cnt = 0;

            foreach (var row in solutionNames)
            {
                if (row.Text.Equals(solutionName, StringComparison.OrdinalIgnoreCase))
                {
                    rowNumber = cnt;
                    break;
                }

                cnt++;
            }

            if (rowNumber == -1)
                throw new InvalidOperationException($"Could not find status for solution: {solutionName}");

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
                    tableRows.Add(new SolutionGridRow(cellValues[columnHeaders.FindIndex(x => x.Equals("display name", StringComparison.OrdinalIgnoreCase))]));
            }

            return tableRows;
        }
        public BrowserCommandResult<bool> DownloadResults(string solutionName, string commandBarButton, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Download Results"), driver =>
            {


                string currentStatus = GetCurrentStatus(solutionName);

                //Download results if/when complete
                if (currentStatus.Contains("Results", StringComparison.OrdinalIgnoreCase))
                {
                    //Click off the current record and back onto this one before downloading results
                    ClickMoreCommandsButton(solutionName, commandBarButton, "Download last results");
                }

                return true;
            });

        }
        internal class SolutionGridRow
        {
            public String Name { get; set; }

            public SolutionGridRow(String name)
            {
                this.Name = name;

            }
        }
    }
}



