// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.ComponentModel.DataAnnotations;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.Dialogs;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Dialogs : Element
    {
        public class AddConnectionReference
        {
            public const string AddConnection = "AddConnection";
            private string _DescriptionId = "description";
            private string _Save = "id(\"connection|NoRelationship|Form|Mscrm.Form.connection.SaveAndClose-Large\")";
            private string _RoleLookupButton = "id(\"record2roleid\")";
            private string _RoleLookupTable = "id(\"record2roleid_IMenu\")";
            public string DescriptionId { get => _DescriptionId; set { _DescriptionId = value; } }
            public string Save { get => _Save; set { _Save = value; } }
            public string RoleLookupButton { get => _RoleLookupButton; set { _RoleLookupButton = value; } }
            public string RoleLookupTable { get => _RoleLookupTable; set { _RoleLookupTable = value; } }
        }
        public class AddUserReference
        {
            public const string AddUser = "AddUser";
            private string _header = "id(\"addUserDescription\")";
            private string _add = "id(\"buttonNext\")";
            public string Header { get => _header; set { _header = value; } }
            public string Add { get => _add; set { _add = value; } }
        }
        public class AssignReference
        {
            public const string Assign = "Assign";
            private string _ok = "id(\"ok_id\")";
            private string _userOrTeamLookupId = "systemuserview_id";
            private string _assignToId = "rdoMe_id";
            public string Ok { get => _ok; set { _ok = value; } }
            public string UserOrTeamLookupId { get => _userOrTeamLookupId; set { _userOrTeamLookupId = value; } }
            public string AssignToId { get => _assignToId; set { _assignToId = value; } }

        }
        public class CloseActivityReference
        {
            public const string CloseActivity = "CloseActivity";
            private string _close = ".//button[contains(@data-id, 'ok_id')]";
            private string _cancel = ".//button[contains(@data-id, 'cancel_id')]";

            public string Close { get => _close; set { _close = value; } }
            public string Cancel { get => _cancel; set { _cancel = value; } }
        }
        public class CloseOpportunityReference
        {
            public const string CloseOpportunity = "CloseOpportunity";
            private string _ok = "//button[contains(@data-id, 'ok_id')]";
            private string _cancel = "//button[contains(@data-id, 'cancel_id')]";
            private string _actualRevenueId = "actualrevenue_id";
            private string _closeDateId = "closedate_id";
            private string _descriptionId = "description_id";

            public string Ok { get => _ok; set { _ok = value; } }
            public string Cancel { get => _cancel; set { _cancel = value; } }
            public string ActualRevenueId { get => _actualRevenueId; set { _actualRevenueId = value; } }
            public string CloseDateId { get => _closeDateId; set { _closeDateId = value; } }
            public string DescriptionId { get => _descriptionId; set { _descriptionId = value; } }
        }
        public class DeleteReference
        {
            public const string Delete = "Delete";
            private string _ok = "id(\"butBegin\")";
            public string Ok { get => _ok; set { _ok = value; } }
        }
        public class DuplicateDetectionReference
        {
            public const string DuplicateDetection = "DuplicateDetection";
            private string _save = "id(\"butBegin\")";
            private string _cancel = "id(\"cmdDialogCancel\")";
            public string Save { get => _save; set { _save = value; } }
            public string Cancel { get => _cancel; set { _cancel = value; } }
        }
        public class FramesReference
        {
            #region private
            private string _ContentPanel = "Frame_ContentPanel";
            private string _ContentFrameId = "Frame_ContentFrameId";
            private string _DialogFrame = "Frame_DialogFrame";
            private string _DialogFrameId = "Frame_DialogFrameId";
            private string _QuickCreateFrame = "Frame_QuickCreateFrame";
            private string _QuickCreateFrameId = "Frame_QuickCreateFrameId";
            private string _WizardFrame = "Frame_WizardFrame";
            private string _WizardFrameId = "Frame_WizardFrameId";
            private string _ViewFrameId = "Frame_ViewFrameId";
            #endregion
            #region prop
            public string ContentPanel { get => _ContentPanel; set { _ContentPanel = value; } }
            public string ContentFrameId { get => _ContentFrameId; set { _ContentFrameId = value; } }
            public string DialogFrame { get => _DialogFrame; set { _DialogFrame = value; } }
            public string DialogFrameId { get => _DialogFrameId; set { _DialogFrameId = value; } }
            public string QuickCreateFrame { get => _QuickCreateFrame; set { _QuickCreateFrame = value; } }
            public string QuickCreateFrameId { get => _QuickCreateFrameId; set { _QuickCreateFrameId = value; } }
            public string WizardFrame { get => _WizardFrame; set { _WizardFrame = value; } }
            public string WizardFrameId { get => _WizardFrameId; set { _WizardFrameId = value; } }
            public string ViewFrameId { get => _ViewFrameId; set { _ViewFrameId = value; } }
            #endregion

        }
        public class RunReportReference
        {
            public const string RunReport = "RunReport";
            private string _header = "crmDialog";
            private string _confirm = "id(\"butBegin\")";
            private string _default = "id(\"reportDefault\")";
            private string _selected = "id(\"reportSelected\")";
            private string _view = "id(\"reportView\")";
            public string Header { get => _header; set { _header = value; } }
            public string Confirm { get => _confirm; set { _confirm = value; } }
            public string Default { get => _default; set { _default = value; } }
            public string Selected { get => _selected; set { _selected = value; } }
            public string View { get => _view; set { _view = value; } }
        }
        public class RunWorkflowReference
        {
            public const string RunWorkflow = "RunWorkflow";
            private string _confirm = "id(\"butBegin\")";
            public string Confirm { get => _confirm; set { _confirm = value; } }
        }
        public class SwitchProcessReference
        {
            public const string SwitchProcess = "SwitchProcess";
            private string _process = "ms-crm-ProcessSwitcher-ProcessTitle";
            private string _selectedRadioButton = "ms-crm-ProcessSwitcher-Process-Selected";
            public string Process { get => _process; set { _process = value; } }
            public string SelectedRadioButton { get => _selectedRadioButton; set { _selectedRadioButton = value; } }
        }
        #region DTO
        public class DialogsReference
        {
            public const string Dialogs = "Dialogs";
            public const string AddConnectionReference = "AddConnection";
            public const string AddUserReference = "AddUser";
            public const string AssignReference = "Assign";
            public const string CloseActivityReference = "CloseActivity";
            public const string CloseOpportunityReference = "CloseOpportunity";
            public const string DeleteReference = "Delete";
            public const string DuplicateDetectionReference = "DuplicateDetection";
            public const string FramesReference = "Frames";
            public const string RunReportReference = "RunReport";
            public const string RunWorkflowReference = "RunWorkflow";
            public const string SwitchProcessReference = "SwitchProcess";
            #region private
            private AddConnectionReference _addConnection = new AddConnectionReference();
            private AddUserReference _addUser = new AddUserReference();
            private AssignReference _assign = new AssignReference();
            private CloseActivityReference _closeActivity = new CloseActivityReference();
            private CloseOpportunityReference _closeOpportunityReference = new CloseOpportunityReference();
            private DeleteReference _deleteReference = new DeleteReference();
            private DuplicateDetectionReference _duplicateDetectionReference = new DuplicateDetectionReference();
            private FramesReference _frames = new FramesReference();
            private RunReportReference _runReport = new RunReportReference();
            private RunWorkflowReference _runWorkflow = new RunWorkflowReference();
            private SwitchProcessReference _switchProcess = new SwitchProcessReference();
            #endregion
            #region prop
            public AddConnectionReference AddConnection { get => _addConnection; set { _addConnection = value; } }
            public AddUserReference AddUser { get => _addUser; set { _addUser = value; } }
            public AssignReference Assign { get => _assign; set { _assign = value; } }
            public CloseActivityReference CloseActivity { get => _closeActivity; set { _closeActivity = value; } }
            public CloseOpportunityReference CloseOpportunity { get => _closeOpportunityReference; set { _closeOpportunityReference = value; } }
            public DeleteReference Delete { get => _deleteReference; set { _deleteReference = value; } }
            public DuplicateDetectionReference DuplicateDetection{ get => _duplicateDetectionReference; set { _duplicateDetectionReference = value; } }
            public FramesReference Frames { get => _frames; set { _frames = value; } }
            public RunReportReference RunReport { get => _runReport; set { _runReport = value; } }
            public RunWorkflowReference RunWorkflow { get => _runWorkflow; set { _runWorkflow = value; } }
            public SwitchProcessReference SwitchProcess { get => _switchProcess; set { _switchProcess = value; } }
            #endregion
 
            public string AssignDialogUserTeamLookupResults = "//ul[contains(@data-id,'systemuserview_id.fieldControl-LookupResultsDropdown_systemuserview_id_tab')]";
            public string AssignDialogOKButton = "//button[contains(@data-id, 'ok_id')]";
            public string AssignDialogToggle = "//label[contains(@data-id,'rdoMe_id.fieldControl-checkbox-inner-first')]";
            public string ConfirmButton = "//*[@data-id=\"confirmButton\"]";
            public string CancelButton = "//*[@data-id=\"cancelButton\"]";
            public string OkButton = "//*[@id=\"okButton\"]";
            public string DuplicateDetectionIgnoreSaveButton = "//button[contains(@data-id, 'ignore_save')]";
            public string DuplicateDetectionCancelButton = "//button[contains(@data-id, 'close_dialog')]";
            public string PublishConfirmButton = "//*[@data-id=\"ok_id\"]";
            public string PublishCancelButton = "//*[@data-id=\"cancel_id\"]";
            public string SetStateDialog = "//div[@data-id=\"SetStateDialog\"]";
            public string SetStateActionButton = ".//button[@data-id=\"ok_id\"]";
            public string SetStateCancelButton = ".//button[@data-id=\"cancel_id\"]";
            public string SwitchProcessDialog = "Entity_SwitchProcessDialog";
            public string SwitchProcessDialogOK = "//button[contains(@data-id,'ok_id')]";
            public string ActiveProcessGridControlContainer = "//div[contains(@data-lp-id,'activeProcessGridControlContainer')]";
            //public string DialogContext = "//div[contains(@role, 'dialog') and @aria-hidden != 'true']";
            public string DialogContext = "//div[contains(@role, 'dialog') and @aria-modal = 'true']";
            public string SwitchProcessContainer = "//div[contains(@id,'switchProcess_id-FieldSectionItemContainer')]";

            public string Header = "id(\"dialogHeaderTitle\")";
            public string DeleteHeader = "id(\"tdDialogHeader\")";
            public string WorkflowHeader = "id(\"DlgHdContainer\")";
            public string ProcessFlowHeader = "id(\"processSwitcherFlyout\")";
            public string AddConnectionHeader = "id(\"EntityTemplateTab.connection.NoRelationship.Form.Mscrm.Form.connection.MainTab-title\")";
            public string WarningFooter = "//*[@id=\"crmDialogFooter\"]";
            public string WarningCloseButton = "//*[@id=\"butBegin\"]";
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
            Entity entity = new Entity(_client);
            return entity.GetValue(control);
        }


        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        public DateTime? GetValue(DateTimeControl control)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(control);
        }

        /// <summary>
        /// Gets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new LookupItem[] { new LookupItem { Name = "to" } });</example>
        public string[] GetValue(LookupItem[] controls)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(controls);
        }

        /// <summary>
        /// Gets the value of a text or date field.
        /// </summary>
        /// <param name="control">The schema name of the field</param>
        /// <example>xrmApp.Entity.GetValue("emailaddress1");</example>
        public string GetValue(string field)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(field);
        }

        /// <summary>
        /// Gets the value of a picklist or status field.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public string GetValue(OptionSet optionSet)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(optionSet);
        }

        /// <summary>
        /// Gets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        public bool GetValue(BooleanItem option)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(option);
        }

        /// <summary>
        /// Gets the value of a MultiValueOptionSet.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public MultiValueOptionSet GetValue(MultiValueOptionSet option)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(option);
        }


        /// <summary>
        /// Sets the value of a field
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        public void SetValue(string field, string value)
        {
            Field.SetValue(_client, field, value, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        public void SetValue(LookupItem control)
        {
            Lookup lookup = new Lookup(_client);
            lookup.SetValue(control, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void SetValue(LookupItem[] controls)
        {
            Lookup lookup = new Lookup(_client);
            lookup.SetValue(controls, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets the value of a picklist or status field.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(OptionSet optionSet)
        {
            OptionSet.SetValue(_client, optionSet, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        public void SetValue(BooleanItem option)
        {
            BooleanItem.SetValue(_client, option, FormContextType.Dialog);
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
            DateTimeControl.SetValue(_client, field, date, FormContextType.Dialog, formatDate, formatTime);
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="control">Date field control.</param>
        public void SetValue(DateTimeControl control)
        {
            control.SetValue(_client, control, FormContextType.Dialog);
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            MultiValueOptionSet.SetValue(_client, option, FormContextType.Dialog, removeExistingValues);
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
                if (driver.HasElement(By.XPath(string.Format(_client.ElementMapper.DialogsReference.DialogContext))))
                {
                    var dialogContainer = driver.FindElement(By.XPath(string.Format(_client.ElementMapper.DialogsReference.DialogContext)));
                    ribbon = dialogContainer.WaitUntilAvailable(By.XPath(string.Format(_client.ElementMapper.CommandBarReference.Container)));
                }
                else
                {
                    ribbon = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.CommandBarReference.Container));
                }


                if (ribbon == null)
                {
                    ribbon = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.CommandBarReference.ContainerGrid),
                        TimeSpan.FromSeconds(5),
                        "Unable to find the ribbon.");
                }

                //Is the button in the ribbon?
                if (ribbon.TryFindElement(By.XPath(_client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name)), out var command))
                {
                    command.Click(true);
                    driver.WaitForTransaction();
                }
                else
                {
                    //Is the button in More Commands?
                    if (ribbon.TryFindElement(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarOverflowButton), out var moreCommands))
                    {
                        // Click More Commands
                        moreCommands.Click(true);
                        driver.WaitForTransaction();

                        //Click the button
                        var flyOutMenu = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.RelatedGridReference.CommandBarFlyoutButtonList)); ;
                        if (flyOutMenu.TryFindElement(By.XPath(_client.ElementMapper.SubGridReference.SubGridCommandLabel.Replace("[NAME]", name)), out var overflowCommand))
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
                    var submenu = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.CommandBarReference.MoreCommandsMenu));

                    submenu.TryFindElement(By.XPath(_client.ElementMapper.SubGridReference.SubGridOverflowButton.Replace("[NAME]", subname)), out var subbutton);

                    if (subbutton != null)
                    {
                        subbutton.Click(true);
                    }
                    else
                        throw new InvalidOperationException($"No sub command with the name '{subname}' exists inside of Commandbar.");

                    if (!string.IsNullOrEmpty(subSecondName))
                    {
                        var subSecondmenu = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.CommandBarReference.MoreCommandsMenu));

                        subSecondmenu.TryFindElement(
                            By.XPath(_client.ElementMapper.SubGridReference.SubGridOverflowButton
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
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.DialogsReference.OkButton));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(_client.ElementMapper.DialogsReference.OkButton)).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    buttonToClick = dialogFooter.FindElement(By.XPath(_client.ElementMapper.DialogsReference.OkButton));
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
            var inlineDialog = _client.Browser.Driver.HasElement(By.XPath(_client.ElementMapper.DialogsReference.Frames.DialogFrame.Replace("[INDEX]", index)));
            if (inlineDialog)
            {
                //wait for the content panel to render
                _client.Browser.Driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.DialogsReference.Frames.DialogFrame.Replace("[INDEX]", index)),
                    TimeSpan.FromSeconds(2),
                    d => { _client.Browser.Driver.SwitchTo().Frame(_client.ElementMapper.DialogsReference.Frames.DialogFrameId.Replace("[INDEX]", index)); });
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
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.DialogsReference.WarningFooter));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(_client.ElementMapper.DialogsReference.WarningCloseButton)).Count >
                          0)) return true;
                    var closeBtn = dialogFooter.FindElement(By.XPath(_client.ElementMapper.DialogsReference.WarningCloseButton));
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
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.DialogsReference.ConfirmButton));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(_client.ElementMapper.DialogsReference.ConfirmButton)).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    if (ClickConfirmButton)
                        buttonToClick = dialogFooter.FindElement(By.XPath(_client.ElementMapper.DialogsReference.ConfirmButton));
                    else
                        buttonToClick = dialogFooter.FindElement(By.XPath(_client.ElementMapper.DialogsReference.CancelButton));

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
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.DialogsReference.DuplicateDetectionIgnoreSaveButton));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(_client.ElementMapper.DialogsReference.DuplicateDetectionIgnoreSaveButton)).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    if (clickSaveOrCancel)
                        buttonToClick = dialogFooter.FindElement(By.XPath(_client.ElementMapper.DialogsReference.DuplicateDetectionIgnoreSaveButton));
                    else
                        buttonToClick = dialogFooter.FindElement(By.XPath(_client.ElementMapper.DialogsReference.DuplicateDetectionCancelButton));

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
                    var dialog = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.DialogsReference.SetStateDialog));

                    if (
                        !(dialog?.FindElements(By.TagName("button")).Count >
                          0)) return true;

                    //Click the Activate/Deactivate or Cancel button
                    IWebElement buttonToClick;
                    if (clickOkButton)
                        buttonToClick = dialog.FindElement(By.XPath(_client.ElementMapper.DialogsReference.SetStateActionButton));
                    else
                        buttonToClick = dialog.FindElement(By.XPath(_client.ElementMapper.DialogsReference.SetStateCancelButton));

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
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(_client.ElementMapper.DialogsReference.PublishConfirmButton));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(_client.ElementMapper.DialogsReference.PublishConfirmButton)).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    if (ClickConfirmButton)
                        buttonToClick = dialogFooter.FindElement(By.XPath(_client.ElementMapper.DialogsReference.PublishConfirmButton));
                    else
                        buttonToClick = dialogFooter.FindElement(By.XPath(_client.ElementMapper.DialogsReference.PublishCancelButton));

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
                Entity.EntityReference entityReference = new Entity.EntityReference();
                var inlineDialog = this.SwitchToDialog();
                if (!inlineDialog)
                    return false;

                if (to == Dialogs.AssignTo.Me)
                {
                    //SetValue(new OptionSet { Name = Elements.ElementId[Reference.Dialogs.Assign.AssignToId], Value = "Me" }, FormContextType.Dialog);
                    OptionSet.SetValue(_client, new OptionSet { Name = _client.ElementMapper.DialogsReference.Assign.AssignToId, Value = "Me" }, FormContextType.Dialog);
                }
                else
                {
                    OptionSet.SetValue(_client, new OptionSet { Name = _client.ElementMapper.DialogsReference.Assign.AssignToId, Value = "User or team" }, FormContextType.Dialog);

                    //Set the User Or Team
                    var userOrTeamField = driver.WaitUntilAvailable(By.XPath(entityReference.TextFieldLookup), "User field unavailable");
                    var input = userOrTeamField.ClickWhenAvailable(By.TagName("input"), "User field unavailable");
                    input.SendKeys(userOrTeamName, true);

                    _client.ThinkTime(2000);

                    //Pick the User from the list
                    var container = driver.WaitUntilVisible(By.XPath(_client.ElementMapper.DialogsReference.AssignDialogUserTeamLookupResults));
                    container.WaitUntil(
                        c => c.FindElements(By.TagName("li")).FirstOrDefault(r => r.Text.StartsWith(userOrTeamName, StringComparison.OrdinalIgnoreCase)),
                        successCallback: e => e.Click(true),
                        failureCallback: () => throw new InvalidOperationException($"None {to} found which match with '{userOrTeamName}'"));
                }

                //Click Assign
                driver.ClickWhenAvailable(By.XPath(_client.ElementMapper.DialogsReference.AssignDialogOKButton), TimeSpan.FromSeconds(5),
                    "Unable to click the OK button in the assign dialog");

                return true;
            });
        }

        public BrowserCommandResult<bool> SwitchProcessDialog(string processToSwitchTo)
        {
            return _client.Execute(_client.GetOptions($"Switch Process Dialog"), driver =>
            {
                //Wait for the Grid to load
                driver.WaitUntilVisible(By.XPath(_client.ElementMapper.DialogsReference.ActiveProcessGridControlContainer));

                //Select the Process
                var popup = driver.FindElement(By.XPath(_client.ElementMapper.DialogsReference.SwitchProcessContainer));
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
                var okBtn = driver.FindElement(By.XPath(_client.ElementMapper.DialogsReference.SwitchProcessDialogOK));
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
                    var xPath = _client.ElementMapper.DialogsReference.CloseOpportunity.Ok;

                    //Cancel
                    if (!clickOK)
                        xPath = _client.ElementMapper.DialogsReference.CloseOpportunity.Ok;

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
                if (driver.HasElement(By.XPath(_client.ElementMapper.EntityReference.DuplicateDetectionWindowMarker)))
                {
                    if (driver.HasElement(By.XPath(_client.ElementMapper.EntityReference.DuplicateDetectionGridRows)))
                    {
                        //Select the first record in the grid
                        driver.FindElements(By.XPath(_client.ElementMapper.EntityReference.DuplicateDetectionGridRows))[0].Click(true);

                        //Click Ignore and Save
                        driver.FindElement(By.XPath(_client.ElementMapper.EntityReference.DuplicateDetectionIgnoreAndSaveButton)).Click(true);
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
