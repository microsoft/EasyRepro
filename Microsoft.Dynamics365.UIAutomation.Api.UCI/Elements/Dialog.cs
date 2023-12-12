// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Dialogs : Element
    {
        #region DTO
        public static class DialogsReference
        {
            public static class CloseOpportunity
            {
                public static string Ok = "//button[contains(@data-id, 'ok_id')]";
                public static string Cancel = "//button[contains(@data-id, 'cancel_id')]";
                public static string ActualRevenueId = "actualrevenue_id";
                public static string CloseDateId = "closedate_id";
                public static string DescriptionId = "description_id";
            }
            public static class CloseActivity
            {
                public static string Close = ".//button[contains(@data-id, 'ok_id')]";
                public static string Cancel = ".//button[contains(@data-id, 'cancel_id')]";
            }
            public static class AddConnection
            {
                public static string DescriptionId = "description";
                public static string Save = "id(\"connection|NoRelationship|Form|Mscrm.Form.connection.SaveAndClose-Large\")";
                public static string RoleLookupButton = "id(\"record2roleid\")";
                public static string RoleLookupTable = "id(\"record2roleid_IMenu\")";
            }
            public static class Assign
            {
                public static string Ok = "id(\"ok_id\")";
                public static string UserOrTeamLookupId = "systemuserview_id";
                public static string AssignToId = "rdoMe_id";
            }
            public static class Delete
            {
                public static string Ok = "id(\"butBegin\")";
            }
            public static class SwitchProcess
            {
                public static string Process = "ms-crm-ProcessSwitcher-ProcessTitle";
                public static string SelectedRadioButton = "ms-crm-ProcessSwitcher-Process-Selected";
            }
            public static class DuplicateDetection
            {
                public static string Save = "id(\"butBegin\")";
                public static string Cancel = "id(\"cmdDialogCancel\")";

            }
            public static class RunWorkflow
            {
                public static string Confirm = "id(\"butBegin\")";
            }
            public static class RunReport
            {
                public static string Header = "crmDialog";
                public static string Confirm = "id(\"butBegin\")";
                public static string Default = "id(\"reportDefault\")";
                public static string Selected = "id(\"reportSelected\")";
                public static string View = "id(\"reportView\")";
            }
            public static class AddUser
            {
                public static string Header = "id(\"addUserDescription\")";
                public static string Add = "id(\"buttonNext\")";
            }
            public static string AssignDialogUserTeamLookupResults = "//ul[contains(@data-id,'systemuserview_id.fieldControl-LookupResultsDropdown_systemuserview_id_tab')]";
            public static string AssignDialogOKButton = "//button[contains(@data-id, 'ok_id')]";
            public static string AssignDialogToggle = "//label[contains(@data-id,'rdoMe_id.fieldControl-checkbox-inner-first')]";
            public static string ConfirmButton = "//*[@data-id=\"confirmButton\"]";
            public static string CancelButton = "//*[@data-id=\"cancelButton\"]";
            public static string OkButton = "//*[@id=\"okButton\"]";
            public static string DuplicateDetectionIgnoreSaveButton = "//button[contains(@data-id, 'ignore_save')]";
            public static string DuplicateDetectionCancelButton = "//button[contains(@data-id, 'close_dialog')]";
            public static string PublishConfirmButton = "//*[@data-id=\"ok_id\"]";
            public static string PublishCancelButton = "//*[@data-id=\"cancel_id\"]";
            public static string SetStateDialog = "//div[@data-id=\"SetStateDialog\"]";
            public static string SetStateActionButton = ".//button[@data-id=\"ok_id\"]";
            public static string SetStateCancelButton = ".//button[@data-id=\"cancel_id\"]";
            public static string SwitchProcessDialog = "Entity_SwitchProcessDialog";
            public static string SwitchProcessDialogOK = "//button[contains(@data-id,'ok_id')]";
            public static string ActiveProcessGridControlContainer = "//div[contains(@data-lp-id,'activeProcessGridControlContainer')]";
            //public static string DialogContext = "//div[contains(@role, 'dialog') and @aria-hidden != 'true']";
            public static string DialogContext = "//div[contains(@role, 'dialog') and @aria-modal = 'true']";
            public static string SwitchProcessContainer = "//div[contains(@id,'switchProcess_id-FieldSectionItemContainer')]";

            public static string Header = "id(\"dialogHeaderTitle\")";
            public static string DeleteHeader = "id(\"tdDialogHeader\")";
            public static string WorkflowHeader = "id(\"DlgHdContainer\")";
            public static string ProcessFlowHeader = "id(\"processSwitcherFlyout\")";
            public static string AddConnectionHeader = "id(\"EntityTemplateTab.connection.NoRelationship.Form.Mscrm.Form.connection.MainTab-title\")";
            public static string WarningFooter = "//*[@id=\"crmDialogFooter\"]";
            public static string WarningCloseButton = "//*[@id=\"butBegin\"]";
        }
        #endregion
        #region prop
        private readonly WebClient _client;
        #endregion
        #region ctor
        public Dialogs(WebClient client) : base()
        {
            _client = client;
        }
        #endregion
        #region public
        public enum AssignTo
        {
            Me,
            User,
            Team
        }

        /// <summary>
        /// Assigns a record to a user or team
        /// </summary>
        /// <param name="to">Enum used to assign record to user or team</param>
        /// <param name="userOrTeamName">Name of the user or team to assign to</param>
        public void Assign(AssignTo to, string userOrTeamName = null)
        {
            this.AssignDialog(to, userOrTeamName);
        }
        /// <summary>
        /// Clicks the Close button if true, or clicks Cancel if false
        /// </summary>
        /// <param name="closeOrCancel"></param>
        public void CloseActivity(bool closeOrCancel)
        {
            Timeline timeline = new Timeline(_client);
            timeline.CloseActivity(closeOrCancel);
        }

        /// <summary>
        /// Clicks Close As Won or Close As Loss on Opportunity Close dialog
        /// </summary>
        /// <param name="closeAsWon"></param>
        public void CloseOpportunity(bool closeAsWon)
        {
            CommandBar cmd = new CommandBar(_client);
            cmd.CloseOpportunity(closeAsWon);
        }

        /// <summary>
        /// Enters the values provided and closes an opportunity
        /// </summary>
        /// <param name="revenue">Value for Revenue field</param>
        /// <param name="closeDate">Value for Close Date field</param>
        /// <param name="description">Value for Description field</param>
        public void CloseOpportunity(double revenue, DateTime closeDate, string description)
        {
            CommandBar cmd = new CommandBar(_client);
            cmd.CloseOpportunity(revenue, closeDate, description);
        }



        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        public string GetValue(LookupItem control)
        {
            return _client.GetValue(control);
        }


        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        public DateTime? GetValue(DateTimeControl control)
        {
            return _client.GetValue(control);
        }

        /// <summary>
        /// Gets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new LookupItem[] { new LookupItem { Name = "to" } });</example>
        public string[] GetValue(LookupItem[] controls)
        {
            return _client.GetValue(controls);
        }

        /// <summary>
        /// Gets the value of a text or date field.
        /// </summary>
        /// <param name="control">The schema name of the field</param>
        /// <example>xrmApp.Entity.GetValue("emailaddress1");</example>
        public string GetValue(string field)
        {
            return _client.GetValue(field);
        }

        /// <summary>
        /// Gets the value of a picklist or status field.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public string GetValue(OptionSet optionSet)
        {
            return _client.GetValue(optionSet);
        }

        /// <summary>
        /// Gets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        public bool GetValue(BooleanItem option)
        {
            return _client.GetValue(option);
        }

        /// <summary>
        /// Gets the value of a MultiValueOptionSet.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public MultiValueOptionSet GetValue(MultiValueOptionSet option)
        {
            return _client.GetValue(option);
        }


        /// <summary>
        /// Sets the value of a field
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        public void SetValue(string field, string value)
        {
            _client.SetValue(field, value, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        public void SetValue(LookupItem control)
        {
            _client.SetValue(control, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void SetValue(LookupItem[] controls)
        {
            _client.SetValue(controls, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets the value of a picklist or status field.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(OptionSet optionSet)
        {
            _client.SetValue(optionSet, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        public void SetValue(BooleanItem option)
        {
            _client.SetValue(option, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="formatDate">Datetime format matching Short Date formatting personal options.</param>
        /// <param name="formatTime">Datetime format matching Short Time formatting personal options.</param>
        /// <example>xrmApp.Dialog.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        /// <example>xrmApp.Dialog.SetValue("new_actualclosedatetime", DateTime.Now, "MM/dd/yyyy", "hh:mm tt");</example>
        /// <example>xrmApp.Dialog.SetValue("estimatedclosedate", DateTime.Now);</example>
        public void SetValue(string field, DateTime date, string formatDate = null, string formatTime = null)
        {
            _client.SetValue(field, date, FormContextType.Dialog, formatDate, formatTime);
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="control">Date field control.</param>
        public void SetValue(DateTimeControl control)
        {
            _client.SetValue(control, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            _client.SetValue(option, FormContextType.Dialog, removeExistingValues);
        }


        /// <summary>
        /// Clicks on entity dialog ribbon button
        /// </summary>
        /// <param name="secondSubButtonName"></param>
        /// <param name="buttonName">Name of button to click</param>
        /// <param name="subButtonName">Name of button on submenu to click</param>
        /// <param name="secondSubButtonName">Name of button on submenu (3rd level) to click</param>
        //public bool ClickCommand(string buttonName, string subButtonName = null, string secondSubButtonName = null)
        //{
        //    return this.ClickCommand(buttonName, subButtonName, secondSubButtonName);
        //}

        /// <summary>
        /// Clicks on entity dialog ribbon button
        /// </summary>
        /// <param name="tabName">The name of the tab based on the References class</param>
        /// <param name="subtabName">The name of the subtab based on the References class</param>
        public bool SelectTab(string tabName, string subtabName = null)
        {
            Entity entity = new Entity(_client);
            return entity.EntitySelectTab(tabName, subtabName);
        }
        #endregion

        #region WebClient public

        public BrowserCommandResult<bool> ClickCommand(string name, string subname = null, string subSecondName = null, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Click Command"), driver =>
            {
                // Find the button in the CommandBar
                IWebElement ribbon;
                // Checking if any dialog is active
                if (driver.HasElement(By.XPath(string.Format(DialogsReference.DialogContext))))
                {
                    var dialogContainer = driver.FindElement(By.XPath(string.Format(DialogsReference.DialogContext)));
                    ribbon = dialogContainer.WaitUntilAvailable(By.XPath(string.Format(CommandBar.CommandBarReference.Container)));
                }
                else
                {
                    ribbon = driver.WaitUntilAvailable(By.XPath(CommandBar.CommandBarReference.Container));
                }


                if (ribbon == null)
                {
                    ribbon = driver.WaitUntilAvailable(By.XPath(CommandBar.CommandBarReference.ContainerGrid),
                        TimeSpan.FromSeconds(5),
                        "Unable to find the ribbon.");
                }

                //Is the button in the ribbon?
                if (ribbon.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridCommandLabel].Replace("[NAME]", name)), out var command))
                {
                    command.Click(true);
                    driver.WaitForTransaction();
                }
                else
                {
                    //Is the button in More Commands?
                    if (ribbon.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarOverflowButton]), out var moreCommands))
                    {
                        // Click More Commands
                        moreCommands.Click(true);
                        driver.WaitForTransaction();

                        //Click the button
                        var flyOutMenu = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Related.CommandBarFlyoutButtonList])); ;
                        if (flyOutMenu.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridCommandLabel].Replace("[NAME]", name)), out var overflowCommand))
                        {
                            overflowCommand.Click(true);
                            driver.WaitForTransaction();
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar or the flyout menu.");
                    }
                    else
                        throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");
                }

                if (!string.IsNullOrEmpty(subname))
                {
                    var submenu = driver.WaitUntilAvailable(By.XPath(CommandBar.CommandBarReference.MoreCommandsMenu));

                    submenu.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Entity.SubGridOverflowButton].Replace("[NAME]", subname)), out var subbutton);

                    if (subbutton != null)
                    {
                        subbutton.Click(true);
                    }
                    else
                        throw new InvalidOperationException($"No sub command with the name '{subname}' exists inside of Commandbar.");

                    if (!string.IsNullOrEmpty(subSecondName))
                    {
                        var subSecondmenu = driver.WaitUntilAvailable(By.XPath(CommandBar.CommandBarReference.MoreCommandsMenu));

                        subSecondmenu.TryFindElement(
                            By.XPath(AppElements.Xpath[AppReference.Entity.SubGridOverflowButton]
                                .Replace("[NAME]", subSecondName)), out var subSecondbutton);

                        if (subSecondbutton != null)
                        {
                            subSecondbutton.Click(true);
                        }
                        else
                            throw new InvalidOperationException($"No sub command with the name '{subSecondName}' exists inside of Commandbar.");
                    }
                }

                driver.WaitForTransaction();

                return true;
            });
        }
        ///// <summary>
        ///// Clicks OK or Cancel on the confirmation dialog.  true = OK, false = Cancel
        ///// </summary>
        ///// <param name="clickConfirmButton"></param>
        ///// <returns></returns>
        public BrowserCommandResult<bool> ClickOk()
        {
            //Passing true clicks the confirm button.  Passing false clicks the Cancel button.
            return _client.Execute(_client.GetOptions($"Dialog Click OK"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    //Wait until the buttons are available to click
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(DialogsReference.OkButton));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(DialogsReference.OkButton)).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    buttonToClick = dialogFooter.FindElement(By.XPath(DialogsReference.OkButton));
                    buttonToClick.Click();
                }

                return true;
            });
        }
        internal bool SwitchToDialog(int frameIndex = 0)
        {
            var index = "";
            if (frameIndex > 0)
                index = frameIndex.ToString();

            _client.Browser.Driver.SwitchTo().DefaultContent();

            // Check to see if dialog is InlineDialog or popup
            var inlineDialog = _client.Browser.Driver.HasElement(By.XPath(Elements.Xpath[Reference.Frames.DialogFrame].Replace("[INDEX]", index)));
            if (inlineDialog)
            {
                //wait for the content panel to render
                _client.Browser.Driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.DialogFrame].Replace("[INDEX]", index)),
                    TimeSpan.FromSeconds(2),
                    d => { _client.Browser.Driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.DialogFrameId].Replace("[INDEX]", index)); });
                return true;
            }
            else
            {
                // need to add this functionality
                //SwitchToPopup();
            }

            return true;
        }
        /// <summary>
        /// Closes the warning dialog during login
        /// </summary>
        /// <returns></returns>
        public BrowserCommandResult<bool> CloseWarningDialog()
        {
            return _client.Execute(_client.GetOptions($"Close Warning Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(DialogsReference.WarningFooter));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(DialogsReference.WarningCloseButton)).Count >
                          0)) return true;
                    var closeBtn = dialogFooter.FindElement(By.XPath(DialogsReference.WarningCloseButton));
                    closeBtn.Click();
                }

                return true;
            });
        }
        /// <summary>
        /// Clicks OK or Cancel on the confirmation dialog.  true = OK, false = Cancel
        /// </summary>
        /// <param name="clickConfirmButton"></param>
        /// <returns></returns>
        public BrowserCommandResult<bool> ConfirmationDialog(bool ClickConfirmButton)
        {
            //Passing true clicks the confirm button.  Passing false clicks the Cancel button.
            return _client.Execute(_client.GetOptions($"Confirm or Cancel Confirmation Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    //Wait until the buttons are available to click
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(DialogsReference.ConfirmButton));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(DialogsReference.ConfirmButton)).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    if (ClickConfirmButton)
                        buttonToClick = dialogFooter.FindElement(By.XPath(DialogsReference.ConfirmButton));
                    else
                        buttonToClick = dialogFooter.FindElement(By.XPath(DialogsReference.CancelButton));

                    buttonToClick.Click();
                }

                return true;
            });
        }
        /// <summary>
        /// Clicks 'Ignore And Save' or 'Cancel' on the Duplicate Detection dialog.  true = Ignore And Save, false = Cancel
        /// </summary>
        /// <param name="clickConfirmButton"></param>
        /// <returns></returns>
        public BrowserCommandResult<bool> DuplicateDetection(bool clickSaveOrCancel)
        {
            string operationType;

            if (clickSaveOrCancel)
            {
                operationType = "Ignore and Save";
            }
            else
                operationType = "Cancel";

            //Passing true clicks the Ignore and Save button.  Passing false clicks the Cancel button.
            return _client.Execute(_client.GetOptions($"{operationType} Duplicate Detection Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    //Wait until the buttons are available to click
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(DialogsReference.DuplicateDetectionIgnoreSaveButton));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(DialogsReference.DuplicateDetectionIgnoreSaveButton)).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    if (clickSaveOrCancel)
                        buttonToClick = dialogFooter.FindElement(By.XPath(DialogsReference.DuplicateDetectionIgnoreSaveButton));
                    else
                        buttonToClick = dialogFooter.FindElement(By.XPath(DialogsReference.DuplicateDetectionCancelButton));

                    buttonToClick.Click();
                }

                if (clickSaveOrCancel)
                {
                    // Wait for Save before proceeding
                    driver.WaitForTransaction();
                }

                return true;
            });
        }
        /// <summary>
        /// Clicks OK or Cancel on the Set State (Activate / Deactivate) dialog.  true = OK, false = Cancel
        /// </summary>
        /// <param name="clickOkButton"></param>
        /// <returns></returns>
        public BrowserCommandResult<bool> SetStateDialog(bool clickOkButton)
        {
            //Passing true clicks the Activate/Deactivate button.  Passing false clicks the Cancel button.
            return _client.Execute(_client.GetOptions($"Interact with Set State Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    //Wait until the buttons are available to click
                    var dialog = driver.WaitUntilAvailable(By.XPath(DialogsReference.SetStateDialog));

                    if (
                        !(dialog?.FindElements(By.TagName("button")).Count >
                          0)) return true;

                    //Click the Activate/Deactivate or Cancel button
                    IWebElement buttonToClick;
                    if (clickOkButton)
                        buttonToClick = dialog.FindElement(By.XPath(DialogsReference.SetStateActionButton));
                    else
                        buttonToClick = dialog.FindElement(By.XPath(DialogsReference.SetStateCancelButton));

                    buttonToClick.Click();
                }

                return true;
            });
        }
        /// <summary>
        /// Clicks Confirm or Cancel on the Publish dialog.  true = Confirm, false = Cancel
        /// </summary>
        /// <param name="clickOkButton"></param>
        /// <returns></returns>
        public BrowserCommandResult<bool> PublishDialog(bool ClickConfirmButton)
        {
            //Passing true clicks the confirm button.  Passing false clicks the Cancel button.
            return _client.Execute(_client.GetOptions($"Confirm or Cancel Publish Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    //Wait until the buttons are available to click
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(DialogsReference.PublishConfirmButton));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(DialogsReference.PublishConfirmButton)).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    if (ClickConfirmButton)
                        buttonToClick = dialogFooter.FindElement(By.XPath(DialogsReference.PublishConfirmButton));
                    else
                        buttonToClick = dialogFooter.FindElement(By.XPath(DialogsReference.PublishCancelButton));

                    buttonToClick.Click();
                }

                return true;
            });
        }

        /// <summary>
        /// Assigns a record to a user or team
        /// </summary>
        /// <param name="to">Enum used to assign record to user or team</param>
        /// <param name="userOrTeamName">Name of the user or team to assign to</param>
        public BrowserCommandResult<bool> AssignDialog(Dialogs.AssignTo to, string userOrTeamName = null)
        {
            return _client.Execute(_client.GetOptions($"Assign to User or Team Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (!inlineDialog)
                    return false;

                if (to == Dialogs.AssignTo.Me)
                {
                    //SetValue(new OptionSet { Name = Elements.ElementId[Reference.Dialogs.Assign.AssignToId], Value = "Me" }, FormContextType.Dialog);
                    _client.SetValue(new OptionSet { Name = Dialogs.DialogsReference.Assign.AssignToId, Value = "Me" }, FormContextType.Dialog);
                }
                else
                {
                    _client.SetValue(new OptionSet { Name = DialogsReference.Assign.AssignToId, Value = "User or team" }, FormContextType.Dialog);

                    //Set the User Or Team
                    var userOrTeamField = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookup]), "User field unavailable");
                    var input = userOrTeamField.ClickWhenAvailable(By.TagName("input"), "User field unavailable");
                    input.SendKeys(userOrTeamName, true);

                    _client.ThinkTime(2000);

                    //Pick the User from the list
                    var container = driver.WaitUntilVisible(By.XPath(DialogsReference.AssignDialogUserTeamLookupResults));
                    container.WaitUntil(
                        c => c.FindElements(By.TagName("li")).FirstOrDefault(r => r.Text.StartsWith(userOrTeamName, StringComparison.OrdinalIgnoreCase)),
                        successCallback: e => e.Click(true),
                        failureCallback: () => throw new InvalidOperationException($"None {to} found which match with '{userOrTeamName}'"));
                }

                //Click Assign
                driver.ClickWhenAvailable(By.XPath(DialogsReference.AssignDialogOKButton), TimeSpan.FromSeconds(5),
                    "Unable to click the OK button in the assign dialog");

                return true;
            });
        }

        public BrowserCommandResult<bool> SwitchProcessDialog(string processToSwitchTo)
        {
            return _client.Execute(_client.GetOptions($"Switch Process Dialog"), driver =>
            {
                //Wait for the Grid to load
                driver.WaitUntilVisible(By.XPath(DialogsReference.ActiveProcessGridControlContainer));

                //Select the Process
                var popup = driver.FindElement(By.XPath(DialogsReference.SwitchProcessContainer));
                var labels = popup.FindElements(By.TagName("label"));
                foreach (var label in labels)
                {
                    if (label.Text.Equals(processToSwitchTo, StringComparison.OrdinalIgnoreCase))
                    {
                        label.Click();
                        break;
                    }
                }

                //Click the OK button
                var okBtn = driver.FindElement(By.XPath(DialogsReference.SwitchProcessDialogOK));
                okBtn.Click();

                return true;
            });
        }

        public BrowserCommandResult<bool> CloseOpportunityDialog(bool clickOK)
        {
            return _client.Execute(_client.GetOptions($"Close Opportunity Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();

                if (inlineDialog)
                {
                    //Close Opportunity
                    var xPath = DialogsReference.CloseOpportunity.Ok;

                    //Cancel
                    if (!clickOK)
                        xPath = DialogsReference.CloseOpportunity.Ok;

                    driver.ClickWhenAvailable(By.XPath(xPath), TimeSpan.FromSeconds(5), "The Close Opportunity dialog is not available.");
                }

                return true;
            });
        }

        public BrowserCommandResult<bool> HandleSaveDialog()
        {
            //If you click save and something happens, handle it.  Duplicate Detection/Errors/etc...
            //Check for Dialog and figure out which type it is and return the dialog type.

            //Introduce think time to avoid timing issues on save dialog
            _client.ThinkTime(1000);

            return _client.Execute(_client.GetOptions($"Validate Save"), driver =>
            {
                //Is it Duplicate Detection?
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionWindowMarker])))
                {
                    if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows])))
                    {
                        //Select the first record in the grid
                        driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows]))[0].Click(true);

                        //Click Ignore and Save
                        driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton])).Click(true);
                        driver.WaitForTransaction();
                    }
                }

                //Is it an Error?
                if (driver.HasElement(By.XPath("//div[contains(@data-id,'errorDialogdialog')]")))
                {
                    var errorDialog = driver.FindElement(By.XPath("//div[contains(@data-id,'errorDialogdialog')]"));

                    var errorDetails = errorDialog.FindElement(By.XPath(".//*[contains(@data-id,'errorDialog_subtitle')]"));

                    if (!String.IsNullOrEmpty(errorDetails.Text))
                        throw new InvalidOperationException(errorDetails.Text);
                }


                return true;
            });
        }

        public BrowserCommandResult<string> GetBusinessProcessErrorText(int waitTimeInSeconds)
        {

            return _client.Execute(_client.GetOptions($"Get Business Process Error Text"), driver =>
            {
                string errorDetails = string.Empty;
                var errorDialog = driver.WaitUntilAvailable(By.XPath("//div[contains(@data-id,'errorDialogdialog')]"), new TimeSpan(0, 0, waitTimeInSeconds));

                // Is error dialog present?
                if (errorDialog != null)
                {
                    var errorDetailsElement = errorDialog.FindElement(By.XPath(".//*[contains(@data-id,'errorDialog_subtitle')]"));

                    if (errorDetailsElement != null)
                    {
                        if (!String.IsNullOrEmpty(errorDetailsElement.Text))
                            errorDetails = errorDetailsElement.Text;
                    }
                }

                return errorDetails;
            });
        }


        #endregion

    }
}
