// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    ///  Dialog page.
    ///  </summary>
    public class Dialog
       : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dialog"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Dialog(InteractiveBrowser browser)
            : base(browser)
        {
            this.SwitchToDialog();
        }
        /// <summary>
        /// Enum for the Assign Dialog to determine which type of record you will be assigning to. 
        /// </summary>
        public enum AssignTo
        {
            Me,
            User,
            Team
        }

        public enum ReportRecords
        {
            AllRecords,
            SelectedRecords,
            AllRecordsOnPage
        }
        /// <summary>
        /// Closes the opportunity you are currently working on.
        /// </summary>
        /// <param name="revenue">The revenue you want to assign to the opportunity.</param>
        /// <param name="closeDate">The close date for the opportunity.</param>
        /// <param name="description">The description of the closing.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Dialogs.CloseOpportunity(10000, DateTime.Now, "Testing the Close Opportunity API");</example>
        public BrowserCommandResult<bool> CloseOpportunity(double revenue, DateTime closeDate, string description, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);
            return this.Execute(GetOptions("Close Opportunity"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.Header]),
                                          new TimeSpan(0, 0, 10), 
                                          "The Close Opportunity dialog is not available.");

                SetValue(Elements.ElementId[Reference.Dialogs.CloseOpportunity.ActualRevenueId], revenue.ToString());
                SetValue(new DateTimeControl { Name = Elements.ElementId[Reference.Dialogs.CloseOpportunity.CloseDateId], Value = closeDate });
                SetValue(Elements.ElementId[Reference.Dialogs.CloseOpportunity.DescriptionId], description);

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.CloseOpportunity.Ok]));

                return true;
            });
        }

        /// <summary>
        /// Closes the opportunity you are currently working on.
        /// </summary>
        /// <param name="index">The address to choose, starting from 1</param>
        /// <example>xrmBrowser.Dialogs.ChooseFoundPlace(1);</example>
        public BrowserCommandResult<bool> ChooseFoundPlace(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);
            return this.Execute(GetOptions("Choose Address From Found Places Dialog"), driver =>
            {
                SwitchToDialog();
                var addressSuggestor = driver.FindElement(By.Id("ctrAddressSuggestor"));
                var suggestedAddresses = addressSuggestor.FindElements(By.TagName("li"));

                if (suggestedAddresses.Count > (index - 1))
                {
                    var targetAddress = suggestedAddresses[index - 1].FindElement(By.TagName("a"));

                    targetAddress.Click(true);
                }
                else
                    throw new InvalidOperationException($"Suggested Address List does not have {index} items.");

                SwitchToContentFrame();

                return true;
            });
        }

        public BrowserCommandResult<bool> AddConnection(string description, string roleName, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            var bcr = this.Execute(GetOptions("Add Connection"), driver =>
            {

                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.AddConnectionHeader]),
                                          new TimeSpan(0, 0, 10),
                                          "The Add Connection dialog is not available.");

                SwitchToContent();

                this.SetValue(Elements.ElementId[Reference.Dialogs.AddConnection.DescriptionId], description);

                if (roleName != null || roleName != "")
                {
                    // Wait till role lookup button is available 
                    driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.AddConnection.RoleLookupButton]));

                    // wait till role table is available
                    driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.AddConnection.RoleLookupTable]));

                    try
                    {
                       driver.FindElement(By.XPath("//span[contains(text(),'" + roleName + "')]")).Click();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception Message: " + ex.Message + "Role name not found in lookup table.");
                    }
                }

                SwitchToDefault();

                //Save and Close
                var savebtn = driver.HasElement(By.XPath(Elements.Xpath[Reference.Dialogs.AddConnection.Save]));
                try
                {
                    driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.AddConnection.Save]));
                }
                catch { }
                

                //Go back to main window
                driver.LastWindow();

                return true;
            });

            return bcr;
        }

        /// <summary>
        /// Assigns the record to a User or Team
        /// </summary>
        /// <param name="to">The User or Team you want to assign the record to</param>
        /// <param name="value">The value of the User or Team you want to find and select</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Dialogs.Assign(XrmDialogPage.AssignTo.Me, "");</example>
        public BrowserCommandResult<bool> Assign(AssignTo to, string value, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Assign"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.Header]),
                                          new TimeSpan(0, 0, 10),
                                          "The Assign dialog is not available.");

                switch (to)
                {
                    case AssignTo.Me:
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.Assign.Ok]));
                       
                        break;

                    case AssignTo.User:
                        this.SetValue(new LookupItem() { Name = Elements.ElementId[Reference.Dialogs.Assign.UserOrTeamLookupId], Value = value });
                        break;

                    case AssignTo.Team:
                        this.SetValue(new LookupItem() { Name = Elements.ElementId[Reference.Dialogs.Assign.UserOrTeamLookupId]});
                        break;
                }

                return true;
            });
        }

        /// <summary>
        /// Deletes the selected record.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Dialogs.Delete();</example>
        public BrowserCommandResult<bool> Delete(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Delete"), driver =>
            {
                var iframe = driver.FindElements(By.XPath(Elements.Xpath[Reference.Dialogs.DeleteIframe]));
                if(iframe.Any())
                {
                    driver.SwitchTo().Frame(iframe.First());
                }

                var deleteHeader = driver.FindElements(By.XPath(Elements.Xpath[Reference.Dialogs.DeleteHeader]));
                if(!deleteHeader.Any())
                {
                    throw new Exception("The Delete dialog is not available.");
                }

                var deleteButton = driver.FindElements(By.XPath(Elements.Xpath[Reference.Dialogs.Delete.Ok]));
                if(deleteButton.Any())
                {
                    deleteButton.First().Click(true);
                }
                else
                {
                    return false;
                }
                driver.SwitchTo().DefaultContent();
                return true;
            });
        }

        /// <summary>
        /// Checks for Duplicate Detection Dialog. If duplicate detection is enable then you can confirm the save or cancel.
        /// </summary>
        /// <param name="save">If set to <c>true</c> Save the record otherwise it will cancel.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Dialogs.DuplicateDetection(true);</example>
        public BrowserCommandResult<bool> DuplicateDetection(bool save, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Duplicate Detection"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.Header]), 
                                            new TimeSpan(0, 0, 5), 
                                            d => //If duplicate detection dialog shows up
                 {

                     if (save)
                         driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.DuplicateDetection.Save]));
                     else
                         driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.DuplicateDetection.Cancel]));
                 });

                return true;
            });
        }

        /// <summary>
        /// Checks for Duplicate Warning dialog on Lead Qualification. If the pop-up appears, then you can confirm the save or cancel.
        /// </summary>
        /// <param name="save">If set to <c>true</c> Save the record otherwise it will cancel.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Dialogs.QualifyLead(true);</example>
        public BrowserCommandResult<bool> QualifyLead(bool save, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Qualify"), driver =>
            {

                var iframe = driver.FindElements(By.XPath(Elements.Xpath[Reference.Dialogs.DuplicateWarningIframe]));
                if (iframe.Any())
                {
                    driver.SwitchTo().Frame(iframe.First());
                }

                var continueButton = driver.FindElements(By.XPath(Elements.Xpath[Reference.Dialogs.QualifyLead.QualifyContinue]));
                var cancelButton = driver.FindElements(By.XPath(Elements.Xpath[Reference.Dialogs.QualifyLead.QualifyCancel]));

                if (continueButton.Any() && save)
                {
                    continueButton.First().Click(true);
                }
                else if (cancelButton.Any() && !save)
                {
                    cancelButton.First().Click(true);
                }
                else
                {
                    return false;
                }

                driver.SwitchTo().DefaultContent();
                return true;

            });
        }

        /// <summary>
        /// Run Work flow
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Dialogs.RunWorkflow("Account Set Phone Number");</example>
        public BrowserCommandResult<bool> RunWorkflow(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Run Workflow"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.WorkflowHeader]),
                                          new TimeSpan(0, 0, 10),
                                          "The RunWorkflow dialog is not available.");

                var lookup = this.Browser.GetPage<Lookup>();
                Browser.Depth++;
                lookup.Search(name);
                lookup.SelectItem(name);
                lookup.Add();

                SwitchToDialog(1);
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.RunWorkflow.Confirm]));
                return true;
            });
        }


        /// <summary>
        /// Run Work flow
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Dialogs.RunWorkflow("Account Set Phone Number");</example>
        public BrowserCommandResult<bool> RunReport(ReportRecords records, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Run Report"), driver =>
            {
                driver.WaitUntilAvailable(By.Name(Elements.Name[Reference.Dialogs.RunReport.Header]),
                                          new TimeSpan(0, 0, 10),
                                          "The Run Report dialog is not available.");
                switch (records)
                {
                    case ReportRecords.AllRecords:
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.RunReport.Default]));

                        break;

                    case ReportRecords.SelectedRecords:
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.RunReport.Selected]));
                        break;

                    case ReportRecords.AllRecordsOnPage:
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.RunReport.View]));
                        break;
                }
                
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.RunReport.Confirm]));
                return true;
            });
        }

        public BrowserCommandResult<bool> AddUser(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Add User Dialog"), driver =>
            {
                SwitchToWizard();

                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.AddUser.Header]),
                    new TimeSpan(0, 0, 10),
                    "The Add User dialog is not available.");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.AddUser.Add]));
                
                return true;
            });
        }

        public BrowserCommandResult<bool> CloseWarningDialog()
        {
            return this.Execute(GetOptions($"Close Warning Dialog"), driver =>
            {
                var dialogFooter = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.WarningFooter]));

                if (
                    !(dialogFooter?.FindElements(By.XPath(Elements.Xpath[Reference.Dialogs.WarningCloseButton])).Count >
                      0)) return true;
                var closeBtn = dialogFooter.FindElement(By.XPath(Elements.Xpath[Reference.Dialogs.WarningCloseButton]));
                closeBtn.Click();
                return true;
            });
        }
    }
}
