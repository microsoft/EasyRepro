// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using static Microsoft.Dynamics365.UIAutomation.Api.BusinessProcessFlow;
using System.Collections.ObjectModel;
using System.Web;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class Entity
    {
        private EntityReference _entityReference { get; set; }
        #region DTO
        //private EntityReference _entityReference;
        public class EntityReference
        {
            public const string Entity = "Entity";
            #region private prop
            private string _form = "//*[contains(@id, \"tablist\")]";

            private string _FormContext = "//*[@data-id='editFormRoot']";
            private string _FormSelector = "//*[@data-id='form-selector']";
            private string _FormSelectorFlyout = "//*[@data-id=\"form-selector-flyout\"]";
            private string _FormSelectorItem = "//li[contains(@data-id, 'form-selector-item')]";
            private string _HeaderTitle = "//*[@data-id='header_title']";
            private string _HeaderContext = ".//div[@data-id='headerFieldsFlyout']";
            private string _Save = "//button[@role='menuitem' and .//*[text()='Save']]";
            private string _TextFieldContainer = ".//*[contains(@id, \'[NAME]-FieldSectionItemContainer\')]";
            private string _TextFieldLabel = ".//label[contains(@id, \'[NAME]-field-label\')]";
            private string _TextFieldValue = ".//input[contains(@data-id, \'[NAME].fieldControl\')]";
            private string _TextFieldLookup = ".//*[contains(@id, \'systemuserview_id.fieldControl-LookupResultsDropdown')]";
            private string _TextFieldLookupSearchButton = ".//button[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_search')]";
            private string _TextFieldLookupMenu = "//div[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]') and contains(@data-id,'tabContainer')]";
            private string _LookupFieldExistingValue = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag') and @role='link']";
            private string _LookupFieldDeleteExistingValue = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag_delete')]";
            private string _LookupFieldExpandCollapseButton = ".//button[contains(@data-id,'[NAME].fieldControl-LookupResultsDropdown_[NAME]_expandCollapse')]/descendant::label[not(text()='+0')]";
            private string _LookupFieldNoRecordsText = ".//*[@data-id=\'[NAME].fieldControl-LookupResultsDropdown_[NAME]_No_Records_Text']";
            private string _LookupFieldResultList = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_tab')]";
            private string _LookupFieldResultListItem = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_resultsContainer')]";
            private string _LookupFieldHoverExistingValue = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_SelectedRecordList')]";
            private string _LookupResultsDropdown = "//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_tab')]";
            private string _OptionSetFieldContainer = ".//div[@data-id='[NAME].fieldControl-option-set-container']";
            private string _TextFieldLookupFieldContainer = ".//div[@data-id='[NAME].fieldControl-Lookup_[NAME]']";
            private string _RecordSetNavigator = "//button[contains(@data-lp-id, 'recordset-navigator')]";
            private string _RecordSetNavigatorOpen = "//button[contains(@data-lp-id, 'recordset-navigator')]";
            private string _RecordSetNavList = "//ul[contains(@data-id, 'recordSetNavList')]";
            private string _RecordSetNavCollapseIcon = "//*[contains(@data-id, 'recordSetNavCollapseIcon')]";
            private string _RecordSetNavCollapseIconParent = "//*[contains(@data-id, 'recordSetNavCollapseIcon')]";
            private string _FieldControlDateTimeContainer = "//div[@data-id='[NAME]-FieldSectionItemContainer']";
            private string _FieldControlDateTimeInput = ".//*[contains(@data-id, '[FIELD].fieldControl-date-time-input')]";
            private string _FieldControlDateTimeTimeInput = ".//div[contains(@data-id,'[FIELD].fieldControl._timecontrol-datetime-container')]/div/div/input";
            private string _Delete = "//button[contains(@data-id,'Delete')]";
            private string _Assign = "//button[contains(@data-id,'Assign')]";
            private string _MoreCommands = ".//button[contains(@data-id, 'OverflowButton') and contains(@data-lp-id, 'Form')]";
            private string _SwitchProcess = "//button[contains(@data-id,'SwitchProcess')]";
            private string _CloseOpportunityWin = "//button[contains(@data-id,'MarkAsWon')]";
            private string _CloseOpportunityLoss = "//button[contains(@data-id,'MarkAsLost')]";
            private string _ProcessButton = "//button[contains(@data-id,'MBPF.ConvertTo')]";
            private string _SwitchProcessDialog = "Entity_SwitchProcessDialog";
            private string _TabList = ".//ul[contains(@id, \"tablist\")]";
            private string _Tab = ".//li[@title='{0}']";
            private string _MoreTabs = ".//div[@data-id='more_button']";
            private string _MoreTabsMenu = "//div[@id='__flyoutRootNode']";
            private string _SubTab = "//div[@id=\"__flyoutRootNode\"]//span[text()=\"{0}\"]";
            private string _FieldLookupButton = "//button[contains(@data-id,'[NAME]_search')]";
            private string _SearchButtonIcon = "//span[contains(@data-id,'microsoftIcon_searchButton')]";
            private string _DuplicateDetectionWindowMarker { get; set; }
            private string _DuplicateDetectionGridRows = "//div[contains(@class,'data-selectable')]";
            private string _DuplicateDetectionIgnoreAndSaveButton = "//button[contains(@data-id,'ignore_save')]";
            private string _EntityBooleanFieldRadioContainer = "//div[contains(@data-id, '[NAME].fieldControl-checkbox-container') and contains(@role,'radiogroup')]";
            private string _EntityBooleanFieldRadioTrue = "//div[contains(@data-id, '[NAME].fieldControl-checkbox-containercheckbox-inner-second')]";
            private string _EntityBooleanFieldRadioFalse = "//div[contains(@data-id, '[NAME].fieldControl-checkbox-containercheckbox-inner-first')]";
            private string _EntityBooleanFieldButtonContainer = "//div[contains(@data-id, '[NAME].fieldControl_container')]";
            private string _EntityBooleanFieldButtonTrue = ".//label[contains(@class, 'first-child')]";
            private string _EntityBooleanFieldButtonFalse = ".//label[contains(@class, 'last-child')]";
            private string _EntityBooleanFieldCheckboxContainer = "//div[contains(@data-id, '[NAME].fieldControl-checkbox-container')]";
            private string _EntityBooleanFieldCheckbox = "//input[contains(@data-id, '[NAME].fieldControl-checkbox-toggle')]";
            private string _EntityBooleanFieldList = "//select[contains(@data-id, '[NAME].fieldControl-checkbox-select')]";
            private string _EntityBooleanFieldFlipSwitchLink = "//div[contains(@data-id, '[NAME]-FieldSectionItemContainer')]";
            private string _EntityBooleanFieldFlipSwitchContainer = "//div[@data-id= '[NAME].fieldControl_container']";
            private string _EntityBooleanFieldToggle = "//div[contains(@data-id, '[NAME].fieldControl-toggle-container')]";
            private string _EntityOptionsetStatusCombo = "//div[contains(@data-id, '[NAME].fieldControl-pickliststatus-comboBox')]";
            private string _EntityOptionsetStatusComboButton = "//div[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_button')]";
            private string _EntityOptionsetStatusComboList = "//ul[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_list')]";
            private string _EntityOptionsetStatusTextValue = "//span[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_text-value')]";
            private string _FormMessageBar = "//*[@id=\"notificationMessageAndButtons\"]/div/div/span";
            private string _FormMessageBarTypeIcon = ".//span[contains(@data-id,'formReadOnlyIcon')]";
            private string _FormNotifcationBar = "//div[contains(@data-id, 'notificationWrapper')]";
            private string _FormNotifcationExpandButton = ".//span[@id='notificationExpandIcon']";
            private string _FormNotifcationFlyoutRoot = "//div[@id='__flyoutRootNode']";
            private string _FormNotifcationList = ".//ul[@data-id='notificationList']";
            private string _FormNotifcationTypeIcon = ".//span[contains(@id,'notification_icon_')]";
            private string _HeaderContainer = "//div[contains(@data-id,'form-header')]";
            private string _HeaderFlyout = "//div[@data-id='headerFieldsFlyout']";
            private string _HeaderFlyoutButton = "//button[contains(@id,'headerFieldsExpandButton')]";
            private string _HeaderLookupFieldContainer = "//div[@data-id='header_[NAME].fieldControl-Lookup_[NAME]']";
            private string _HeaderTextFieldContainer = "//div[@data-id='header_[NAME].fieldControl-text-box-container']";
            private string _HeaderOptionSetFieldContainer = "//div[@data-id='header_[NAME]']";
            private string _HeaderDateTimeFieldContainer = "//div[@data-id='header_[NAME]-FieldSectionItemContainer']";
            #endregion
            #region prop
            public string Form { get => _form; set { _form = value; } }
            public string FormContext { get => _FormContext; set { _FormContext = value; } }
            public string FormSelector { get => _FormSelector; set { _FormSelector = value; } }
            public string FormSelectorFlyout { get => _FormSelectorFlyout; set { _FormSelectorFlyout = value; } }
            public string FormSelectorItem { get => _FormSelectorItem; set { _FormSelectorItem = value; } }
            public string HeaderTitle { get => _HeaderTitle; set { _HeaderTitle = value; } }
            public string HeaderContext { get => _HeaderContext; set { _HeaderContext = value; } }
            public string Save { get => _Save; set { _Save = value; } }
            public string TextFieldContainer { get => _TextFieldContainer; set { _TextFieldContainer = value; } }
            public string TextFieldLabel { get => _TextFieldLabel; set { _TextFieldLabel = value; } }
            public string TextFieldValue { get => _TextFieldValue; set { _TextFieldValue = value; } }
            public string TextFieldLookup { get => _TextFieldLookup; set { _TextFieldLookup = value; } }
            public string TextFieldLookupSearchButton { get => _TextFieldLookupSearchButton; set { _TextFieldLookupSearchButton = value; } }
            public string TextFieldLookupMenu { get => _TextFieldLookupMenu; set { _TextFieldLookupMenu = value; } }
            public string LookupFieldExistingValue { get => _LookupFieldExistingValue; set { _LookupFieldExistingValue = value; } }
            public string LookupFieldDeleteExistingValue { get => _LookupFieldDeleteExistingValue; set { _LookupFieldDeleteExistingValue = value; } }
            public string LookupFieldExpandCollapseButton { get => _LookupFieldExpandCollapseButton; set { _LookupFieldExpandCollapseButton = value; } }
            public string LookupFieldNoRecordsText { get => _LookupFieldNoRecordsText; set { _LookupFieldNoRecordsText = value; } }
            public string LookupFieldResultList { get => _LookupFieldResultList; set { _LookupFieldResultList = value; } }
            public string LookupFieldResultListItem { get => _LookupFieldResultListItem; set { _LookupFieldResultListItem = value; } }
            public string LookupFieldHoverExistingValue { get => _LookupFieldHoverExistingValue; set { _LookupFieldHoverExistingValue = value; } }
            public string LookupResultsDropdown { get => _LookupResultsDropdown; set { _LookupResultsDropdown = value; } }
            public string OptionSetFieldContainer { get => _OptionSetFieldContainer; set { _OptionSetFieldContainer = value; } }
            public string TextFieldLookupFieldContainer { get => _TextFieldLookupFieldContainer; set { _TextFieldLookupFieldContainer = value; } }
            public string RecordSetNavigator { get => _RecordSetNavigator; set { _RecordSetNavigator = value; } }
            public string RecordSetNavigatorOpen { get => _RecordSetNavigatorOpen; set { _RecordSetNavigatorOpen = value; } }
            public string RecordSetNavList { get => _RecordSetNavList; set { _RecordSetNavList = value; } }
            public string RecordSetNavCollapseIcon { get => _RecordSetNavCollapseIcon; set { _RecordSetNavCollapseIcon = value; } }
            public string RecordSetNavCollapseIconParent { get => _RecordSetNavCollapseIconParent; set { _RecordSetNavCollapseIconParent = value; } }
            public string FieldControlDateTimeContainer { get => _FieldControlDateTimeContainer; set { _FieldControlDateTimeContainer = value; } }
            public string FieldControlDateTimeInput { get => _FieldControlDateTimeInput; set { _FieldControlDateTimeInput = value; } }
            public string FieldControlDateTimeTimeInput { get => _FieldControlDateTimeTimeInput; set { _FieldControlDateTimeTimeInput = value; } }
            public string Delete { get => _Delete; set { _Delete = value; } }
            public string Assign { get => _Assign; set { _Assign = value; } }
            public string MoreCommands { get => _MoreCommands; set { _MoreCommands = value; } }
            public string SwitchProcess { get => _SwitchProcess; set { _SwitchProcess = value; } }
            public string CloseOpportunityWin { get => _CloseOpportunityWin; set { _CloseOpportunityWin = value; } }
            public string CloseOpportunityLoss { get => _CloseOpportunityLoss; set { _CloseOpportunityLoss = value; } }
            public string ProcessButton { get => _ProcessButton; set { _ProcessButton = value; } }
            public string SwitchProcessDialog { get => _SwitchProcessDialog; set { _SwitchProcessDialog = value; } }
            public string TabList { get => _TabList; set { _TabList = value; } }
            public string Tab { get => _Tab; set { _Tab = value; } }
            public string MoreTabs { get => _MoreTabs; set { _MoreTabs = value; } }
            public string MoreTabsMenu { get => _MoreTabsMenu; set { _MoreTabsMenu = value; } }
            public string SubTab { get => _SubTab; set { _SubTab = value; } }

            public string FieldLookupButton { get => _FieldLookupButton; set { _FieldLookupButton = value; } }
            public string SearchButtonIcon { get => _SearchButtonIcon; set { _SearchButtonIcon = value; } }
            public string DuplicateDetectionWindowMarker { get => _DuplicateDetectionWindowMarker; set { _DuplicateDetectionWindowMarker = value; } }
            public string DuplicateDetectionGridRows { get => _DuplicateDetectionGridRows; set { _DuplicateDetectionGridRows = value; } }
            public string DuplicateDetectionIgnoreAndSaveButton { get => _DuplicateDetectionIgnoreAndSaveButton; set { _DuplicateDetectionIgnoreAndSaveButton = value; } }
            public string EntityBooleanFieldRadioContainer { get => _EntityBooleanFieldRadioContainer; set { _EntityBooleanFieldRadioContainer = value; } }
            public string EntityBooleanFieldRadioTrue { get => _EntityBooleanFieldRadioTrue; set { _EntityBooleanFieldRadioTrue = value; } }
            public string EntityBooleanFieldRadioFalse { get => _EntityBooleanFieldRadioFalse; set { _EntityBooleanFieldRadioFalse = value; } }
            public string EntityBooleanFieldButtonContainer { get => _EntityBooleanFieldButtonContainer; set { _EntityBooleanFieldButtonContainer = value; } }
            public string EntityBooleanFieldButtonTrue { get => _EntityBooleanFieldButtonTrue; set { _EntityBooleanFieldButtonTrue = value; } }
            public string EntityBooleanFieldButtonFalse { get => _EntityBooleanFieldButtonFalse; set { _EntityBooleanFieldButtonFalse = value; } }
            public string EntityBooleanFieldCheckboxContainer { get => _EntityBooleanFieldCheckboxContainer; set { _EntityBooleanFieldCheckboxContainer = value; } }
            public string EntityBooleanFieldCheckbox { get => _EntityBooleanFieldCheckbox; set { _EntityBooleanFieldCheckbox = value; } }
            public string EntityBooleanFieldList { get => _EntityBooleanFieldList; set { _EntityBooleanFieldList = value; } }
            public string EntityBooleanFieldFlipSwitchLink { get => _EntityBooleanFieldFlipSwitchLink; set { _EntityBooleanFieldFlipSwitchLink = value; } }
            public string EntityBooleanFieldFlipSwitchContainer { get => _EntityBooleanFieldFlipSwitchContainer; set { _EntityBooleanFieldFlipSwitchContainer = value; } }
            public string EntityBooleanFieldToggle { get => _EntityBooleanFieldToggle; set { _EntityBooleanFieldToggle = value; } }
            public string EntityOptionsetStatusCombo { get => _EntityOptionsetStatusCombo; set { _EntityOptionsetStatusCombo = value; } }
            public string EntityOptionsetStatusComboButton { get => _EntityOptionsetStatusComboButton; set { _EntityOptionsetStatusComboButton = value; } }
            public string EntityOptionsetStatusComboList { get => _EntityOptionsetStatusComboList; set { _EntityOptionsetStatusComboList = value; } }
            public string EntityOptionsetStatusTextValue { get => _EntityOptionsetStatusTextValue; set { _EntityOptionsetStatusTextValue = value; } }
            public string FormMessageBar { get => _FormMessageBar; set { _FormMessageBar = value; } }
            public string FormMessageBarTypeIcon { get => _FormMessageBarTypeIcon; set { _FormMessageBarTypeIcon = value; } }
            public string FormNotifcationBar { get => _FormNotifcationBar; set { _FormNotifcationBar = value; } }
            public string FormNotifcationExpandButton { get => _FormNotifcationExpandButton; set { _FormNotifcationExpandButton = value; } }
            public string FormNotifcationFlyoutRoot { get => _FormNotifcationFlyoutRoot; set { _FormNotifcationFlyoutRoot = value; } }
            public string FormNotifcationList { get => _FormNotifcationList; set { _FormNotifcationList = value; } }
            public string FormNotifcationTypeIcon { get => _FormNotifcationTypeIcon; set { _FormNotifcationTypeIcon = value; } }
            public string HeaderContainer { get => _HeaderContainer; set { _HeaderContainer = value; } }
            public string HeaderFlyout { get => _HeaderFlyout; set { _HeaderFlyout = value; } }
            public string HeaderFlyoutButton { get => _HeaderFlyoutButton; set { _HeaderFlyoutButton = value; } }
            public string HeaderLookupFieldContainer { get => _HeaderLookupFieldContainer; set { _HeaderLookupFieldContainer = value; } }
            public string HeaderTextFieldContainer { get => _HeaderTextFieldContainer; set { _HeaderTextFieldContainer = value; } }
            public string HeaderOptionSetFieldContainer { get => _HeaderOptionSetFieldContainer; set { _HeaderOptionSetFieldContainer = value; } }
            public string HeaderDateTimeFieldContainer { get => _HeaderDateTimeFieldContainer; set { _HeaderDateTimeFieldContainer = value; } }
            #endregion


        }
        #endregion
        private readonly WebClient _client;

        public SubGrid SubGrid => this.GetElement<SubGrid>(_client);
        public RelatedGrid RelatedGrid => this.GetElement<RelatedGrid>(_client);

        public Entity(WebClient client) : base()
        {
            _client = client;
            _entityReference = new EntityReference();
        }

        #region public
        public T GetElement<T>(WebClient client)
    //where T : IElement
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { client });
        }

        /// <summary>
        /// Clicks the Assign button and completes the Assign dialog
        /// </summary>
        /// <param name="userOrTeam">Name of the user or team to assign the record to</param>

        /// <summary>
        /// Clears a value from the text or date field provided
        /// </summary>
        /// <param name="field"></param>
        public void ClearValue(string field)
        {
            Field.ClearValue(_client, field, FormContextType.Entity);
            _entityReference.ToString();
        }

        /// <summary>
        /// Clears a value from the LookupItem provided
        /// Can be used on a lookup, customer, owner, or activityparty field
        /// </summary>
        /// <param name="control"></param>
        /// <example>xrmApp.Entity.ClearValue(new LookupItem { Name = "parentcustomerid" });</example>
        /// <example>xrmApp.Entity.ClearValue(new LookupItem { Name = "to" });</example>
        public void ClearValue(LookupItem control)
        {
            Lookup lookup = new Lookup(_client);
            lookup.ClearValue(control, FormContextType.Entity);
        }

        /// <summary>
        /// Clears a value from the OptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(OptionSet control)
        {
            control.ClearValue(_client, control, FormContextType.Entity);
        }

        /// <summary>
        /// Clears a value from the MultiValueOptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(MultiValueOptionSet control)
        {
            control.ClearValue(_client, control, FormContextType.Entity);
        }


        /// <summary>
        /// Clears a value from the DateTimeControl provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearHeaderValue(DateTimeControl control)
        {
            control.EntityClearHeaderValue(_client,this, control);
        }

        /// <summary>
        /// Clears a value from the DateTimeControl provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(DateTimeControl control)
        {
            control.ClearValue(_client, control, FormContextType.Entity);
        }

        /// <summary>
        /// Close Record Set Navigator
        /// </summary>
        /// <param name="thinkTime"></param>
        /// <example>xrmApp.Entity.CloseRecordSetNavigator();</example>
        public void CloseRecordSetNavigator()
        {
            this.CloseRecordSetNavigator();
        }

        /// <summary>
        /// Clicks the Delete button on the command bar for an entity
        /// </summary>
        public void Delete()
        {
            CommandBar cmd = new CommandBar(_client);
            cmd.Delete();
        }

        public Field GetField(string field)
        {
            return this.EntityGetField(field);
        }

        /// <summary>
        /// Gets test from a Business Process Error, if present
        /// <paramref name="waitTimeInSeconds"/>Number of seconds to wait for the error dialog. Default value is 120 seconds</param>
        /// </summary>
        /// <example>var errorText = xrmApp.Entity.GetBusinessProcessError(int waitTimeInSeconds);</example>
        public string GetBusinessProcessError(int waitTimeInSeconds = 120)
        {
            _client.Browser.Browser.Wait();
            Dialogs dialogs = new Dialogs(_client);
            return dialogs.GetBusinessProcessErrorText(waitTimeInSeconds);
        }

        /// <summary>
        /// Gets the state from the form.
        /// </summary>
        /// <returns>The state of the record.</returns>
        public string GetFormState()
        {
            return this.GetStateFromForm();
        }

        [Obsolete("Forms no longer have footers. Use Entity.GetFormState() instead.")]
        public string GetFooterStatusValue()
        {
            return this.GetFormState();
        }

        public IReadOnlyList<FormNotification> GetFormNotifications()
        {
            Lookup lookup = new Lookup(_client);
            return lookup.GetFormNotifications().Value;
        }

        /// <summary>
        /// Gets the value of a LookupItem from the header
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new Lookup { Name = "primarycontactid" });</example>
        public string GetHeaderValue(LookupItem control)
        {
            return this.EntityGetHeaderValue(control);
        }

        /// <summary>
        /// Gets the value of an ActivityParty Lookup from the header
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.GetHeaderValue(new LookupItem[] { new LookupItem { Name = "to" } });</example>
        public string[] GetHeaderValue(LookupItem[] controls)
        {
            return this.GetValue(controls);
        }

        /// <summary>
        /// Gets the value of a picklist or status field from the header
        /// </summary>
        /// <param name="option">The option you want to Get.</param>
        /// <example>xrmBrowser.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        public string GetHeaderValue(OptionSet control)
        {
            return this.EntityGetHeaderValue(control);
        }

        /// <summary>
        /// Gets the value of a Boolean Item from the header
        /// </summary>
        /// <param name="control">The boolean field you want to Get.</param>
        /// <example>xrmApp.Entity.GetHeaderValue(new BooleanItem { Name = "preferredcontactmethodcode"}); </example>
        public bool GetHeaderValue(BooleanItem control)
        {
            return this.EntityGetHeaderValue(control);
        }

        /// <summary>
        /// Gets the value of a text or date field from the header
        /// </summary>
        /// <param name="control">The schema name of the field</param>
        /// <example>xrmApp.Entity.GetHeaderValue("emailaddress1");</example>
        public string GetHeaderValue(string control)
        {
            return this.EntityGetHeaderValue(control);
        }

        /// <summary>
        /// Gets the value of a  MultiValueOptionSet from the header
        /// </summary>
        /// <param name="option">The option you want to Get.</param>
        public MultiValueOptionSet GetHeaderValue(MultiValueOptionSet control)
        {
            return this.EntityGetHeaderValue(control);
        }

        /// <summary>
        /// Gets the value of a DateTime Control from the header
        /// </summary>
        /// <param name="control">The date time field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new DateTimeControl { Name = "estimatedclosedate" });</example>
        public DateTime? GetHeaderValue(DateTimeControl control)
        {
            return this.EntityGetHeaderValue(control);
        }

        /// <summary>
        /// Get the object id of the current entity
        /// </summary>
        public Guid GetObjectId()
        {
            return this.GetObjectId();
        }

        /// <summary>
        /// Get the Entity Name of the current entity
        /// </summary>
        public string GetEntityName()
        {
            return this.EntityGetEntityName();
        }

        /// <summary>
        /// Get the Form Name of the current entity
        /// </summary>
        public string GetFormName()
        {
            return this.EntityGetFormName();
        }

        /// <summary>
        /// Get the Header Title of the current entity
        /// </summary>
        public string GetHeaderTitle()
        {
            return this.EntityGetHeaderTitle();
        }


        public List<GridItem> GetSubGridItems(string subgridName)
        {
            SubGrid subgrid = new SubGrid(_client);
            return subgrid.GetSubGridItems(subgridName);
        }

        /// <summary>
        /// Retrieves the number of rows from a subgrid
        /// </summary>
        /// <param name="subgridName">Label of the subgrid to retrieve items from</param>
        /// <returns></returns>
        [Obsolete("GetSubGridItemsCount(string subgridName) is deprecated, please use the equivalent Entity.SubGrid.<Method> instead.")]
        public int GetSubGridItemsCount(string subgridName)
        {
            
            return this.EntityGetSubGridItemsCount(subgridName);
        }

        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        public string GetValue(LookupItem control)
        {
            return this.EntityGetValue(control);
        }


        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        public DateTime? GetValue(DateTimeControl control)
        {
            return this.EntityGetValue(control);
        }

        /// <summary>
        /// Gets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new LookupItem[] { new LookupItem { Name = "to" } });</example>
        public string[] GetValue(LookupItem[] controls)
        {
            return this.EntityGetValue(controls);
        }

        /// <summary>
        /// Gets the value of a text or date field.
        /// </summary>
        /// <param name="control">The schema name of the field</param>
        /// <example>xrmApp.Entity.GetValue("emailaddress1");</example>
        public string GetValue(string field)
        {
            return this.EntityGetValue(field);
        }

        /// <summary>
        /// Gets the value of a picklist or status field.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public string GetValue(OptionSet optionSet)
        {
            return this.EntityGetValue(optionSet);
        }

        /// <summary>
        /// Gets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        public bool GetValue(BooleanItem option)
        {
            return this.EntityGetValue(option);
        }

        /// <summary>
        /// Gets the value of a MultiValueOptionSet.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public MultiValueOptionSet GetValue(MultiValueOptionSet option)
        {
            return this.EntityGetValue(option);
        }

        /// <summary>
        /// Open Entity
        /// </summary>
        /// <param name="entityName">The entity name</param>
        /// <param name="id">The Id</param>
        public void OpenEntity(string entityname, Guid id)
        {
            this.EntityOpenEntity(entityname, id);
        }

        /// <summary>
        /// Open record set and navigate record index.
        /// This method supersedes Navigate Up and Navigate Down outside of UI 
        /// </summary>
        /// <param name="index">The index.</param>
        public void OpenRecordSetNavigator(int index = 0)
        {
            this.EntityOpenRecordSetNavigator(index);
        }

        /// <summary>
        /// Open a record on a subgrid
        /// </summary>
        /// <param name="subgridName">Label of the subgrid</param>
        /// <param name="index">Index of the record to open</param>
        [Obsolete("OpenSubGridRecord(string subgridName, int index = 0) is deprecated, please use the equivalent Entity.SubGrid.<Method> instead.")]
        public void OpenSubGridRecord(string subgridName, int index = 0)
        {
            SubGrid subgrid = new SubGrid(_client);
            subgrid.OpenSubGridRecord(subgridName, index);
        }

        [Obsolete("AddSubgridItem(string subgridName) is deprecated, please use the Entity.SubGrid.ClickCommand(string buttonName) instead.")]
        public void AddSubgridItem(string subgridName)
        {
            SubGrid subgrid = new SubGrid(_client);
            subgrid.ClickSubgridAddButton(subgridName);
        }

        /// <summary>
        /// Selects a Lookup Field
        /// </summary>
        /// <param name="control">LookupItem with the schema name of the field</param>
        public void SelectLookup(LookupItem control)
        {
            this.EntitySelectLookup(control);
        }

        /// <summary>
        /// Opens any tab on the web page.
        /// </summary>
        /// <param name="tabName">The name of the tab based on the References class</param>
        /// <param name="subtabName">The name of the subtab based on the References class</param>
        public void SelectTab(string tabName, string subTabName = "")
        {
            this.EntitySelectTab(tabName, subTabName);
        }

        public void SetHeaderValue(string field, string value)
        {
            this.EntitySetHeaderValue(field, value);
        }

        /// <summary>
        /// Sets the value of a Lookup in the header
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        public void SetHeaderValue(LookupItem control)
        {
            this.EntitySetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of am ActivityParty Lookup in the header
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetHeaderValue(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void SetHeaderValue(LookupItem[] controls)
        {
            this.EntitySetHeaderValue(controls);
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control in the header
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        public void SetHeaderValue(MultiValueOptionSet control)
        {
            this.EntitySetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of a picklist or status field in the header
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetHeaderValue(OptionSet control)
        {
            this.EntitySetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of a BooleanItem in the header
        /// </summary>
        /// <param name="control">The boolean field you want to set.</param>
        public void SetHeaderValue(BooleanItem control)
        {
            this.EntitySetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of a Date field in the header
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="formatDate">Datetime format matching Short Date formatting personal options.</param>
        /// <param name="formatTime">Datetime format matching Short Time formatting personal options.</param>
        /// <example>xrmApp.Entity.SetHeaderValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        /// <example>xrmApp.Entity.SetHeaderValue("new_actualclosedatetime", DateTime.Now, "MM/dd/yyyy", "hh:mm tt");</example>
        /// <example>xrmApp.Entity.SetHeaderValue("estimatedclosedate", DateTime.Now);</example>
        public void SetHeaderValue(string field, DateTime date, string formatDate = null, string formatTime = null)
        {
            this.EntitySetHeaderValue(field, date, formatDate, formatTime);
        }

        /// <summary>
        /// Sets the value of a BooleanItem in the header
        /// </summary>
        /// <param name="control">The boolean field you want to set.</param>
        public void SetHeaderValue(DateTimeControl control)
        {
            control.EntitySetHeaderValue(_client,this, control);
        }

        /// <summary>
        /// Sets the value of a field
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        public void SetValue(string field, string value)
        {
            Field objField = new Field(_client);
            objField.SetValue(_client, field, value, FormContextType.Entity);
        }

        /// <summary>
        /// Sets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        public void SetValue(LookupItem control)
        {
            Lookup lookup = new Lookup(_client);
            lookup.SetValue(control, FormContextType.Entity);
        }

        /// <summary>
        /// Sets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void SetValue(LookupItem[] controls)
        {
            Lookup lookup = new Lookup(_client);
            lookup.SetValue(controls, FormContextType.Entity);
        }

        /// <summary>
        /// Sets the value of a picklist or status field.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(OptionSet optionSet)
        {
            OptionSet.SetValue(_client, optionSet, FormContextType.Entity);
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        public void SetValue(BooleanItem option)
        {
            option.SetValue(_client, option, FormContextType.Entity);
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="formatDate">Datetime format matching Short Date formatting personal options.</param>
        /// <param name="formatTime">Datetime format matching Short Time formatting personal options.</param>
        /// <example>xrmApp.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        /// <example>xrmApp.Entity.SetValue("new_actualclosedatetime", DateTime.Now, "MM/dd/yyyy", "hh:mm tt");</example>
        /// <example>xrmApp.Entity.SetValue("estimatedclosedate", DateTime.Now);</example>
        public void SetValue(string field, DateTime date, string formatDate = null, string formatTime = null)
        {
            DateTimeControl.SetValue(_client, field, date, FormContextType.Entity, formatDate, formatTime);
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="control">Date field control.</param>
        public void SetValue(DateTimeControl control)
        {
            control.SetValue(_client, control, FormContextType.Entity);
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            MultiValueOptionSet.SetValue(_client, option, FormContextType.Entity, removeExistingValues);
        }

        /// <summary>
        /// Click Process>Switch Process
        /// </summary>
        /// <param name="processToSwitchTo">Name of the process to switch to</param>
        public void SwitchProcess(string processToSwitchTo)
        {
            BusinessProcessFlow bpf = new BusinessProcessFlow(_client);
            bpf.BPFSwitchProcess(processToSwitchTo);
        }

        /// <summary>
        /// Switches forms using the form selector
        /// </summary>
        /// <param name="formName">Name of the form</param>
        /// <example>xrmApp.Entity.SelectForm("AI for Sales");</example>
        public void SelectForm(string formName)
        {
            this.EntitySelectForm(formName);
        }

        /// <summary>
        /// Adds values to an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.AddValues(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void AddValues(LookupItem[] controls)
        {
            Lookup lookup = new Lookup(_client);
            lookup.AddValues(controls);
        }

        /// <summary>
        /// Removes values from an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.RemoveValues(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void RemoveValues(LookupItem[] controls)
        {
            Lookup lookup = new Lookup(_client);
            lookup.RemoveValues(controls);
        }
        #endregion
        #region internals
        internal BrowserCommandResult<bool> Assign(string userOrTeamToAssign, int thinkTime = Constants.DefaultThinkTime)
        {
            //Click the Assign Button on the Entity Record
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Assign Entity"), driver =>
            {
                var assignBtn = driver.WaitUntilAvailable(this._entityReference.Assign,
                    "Assign Button is not available");

                assignBtn?.Click(_client);
                Dialogs dialogs = new Dialogs(_client);
                dialogs.AssignDialog(Dialogs.AssignTo.User, userOrTeamToAssign);

                return true;
            });
        }

        /// <summary>
        /// Generic method to help click on any item which is clickable or uniquely discoverable with a By object.
        /// </summary>
        /// <param name="by">The xpath of the HTML item as a By object</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> EntitySelectTab(string tabName, string subTabName = "", int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute($"Select Tab", driver =>
            {
                IElement tabList;
                if (driver.HasElement(_client.ElementMapper.DialogsReference.DialogContext))
                {
                    var dialogContainer = driver.FindElement(_client.ElementMapper.DialogsReference.DialogContext);
                    tabList = driver.WaitUntilAvailable(_client.ElementMapper.DialogsReference.DialogContext + this._entityReference.TabList);
                }
                else
                {
                    tabList = driver.WaitUntilAvailable(this._entityReference.TabList);
                }

                ClickTab(tabList, this._entityReference.Tab, tabName);

                //Click Sub Tab if provided
                if (!String.IsNullOrEmpty(subTabName))
                {
                    this.ClickTab(tabList, this._entityReference.SubTab, subTabName);
                }

                driver.Wait();
                return true;
            });
        }

        internal void ClickTab(IElement tabList, string xpath, string name)
        {
            IElement moreTabsButton;
            IElement listItem;
            // Look for the tab in the tab list, else in the more tabs menu
            IElement searchScope = null;
            if (_client.Browser.Browser.HasElement(tabList.Locator + string.Format(xpath, name)))
            {
                searchScope = tabList;
            }
            else if (_client.Browser.Browser.HasElement(tabList.Locator + this._entityReference.MoreTabs))
            {
                moreTabsButton = _client.Browser.Browser.FindElement(tabList.Locator + this._entityReference.MoreTabs);
                moreTabsButton.Click(_client);

                // No tab to click - subtabs under 'Related' are automatically expanded in overflow menu
                if (name == "Related")
                {
                    return;
                }
                else
                {
                    searchScope = _client.Browser.Browser.FindElement(this._entityReference.MoreTabsMenu);
                }
            }

            if (_client.Browser.Browser.HasElement(string.Format(xpath, name)))
            {
                listItem = _client.Browser.Browser.FindElement(string.Format(xpath, name));
                listItem.Click(_client, true);
            }
            else
            {
                throw new Exception($"The tab with name: {name} does not exist");
            }
        }
        #endregion

        #region Entity

        internal BrowserCommandResult<bool> CancelQuickCreate(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Cancel Quick Create"), driver =>
            {
                var save = driver.WaitUntilAvailable(this._client.ElementMapper.QuickCreateReference.CancelButton,
                    "Quick Create Cancel Button is not available");
                save?.Click(_client, true);

                driver.Wait();

                return true;
            });
        }

        /// <summary>
        /// Open Entity
        /// </summary>
        /// <param name="entityName">The entity name</param>
        /// <param name="id">The Id</param>
        /// <param name="thinkTime">The think time</param>
        internal BrowserCommandResult<bool> EntityOpenEntity(string entityName, Guid id, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Open: {entityName} {id}"), driver =>
            {
                //https:///main.aspx?appid=98d1cf55-fc47-e911-a97c-000d3ae05a70&pagetype=entityrecord&etn=lead&id=ed975ea3-531c-e511-80d8-3863bb3ce2c8
                var uri = new Uri(_client.Browser.Browser.Url);
                var qs = HttpUtility.ParseQueryString(uri.Query.ToLower());
                var appId = qs.Get("appid");
                var link = $"{uri.Scheme}://{uri.Authority}/main.aspx?appid={appId}&etn={entityName}&pagetype=entityrecord&id={id}";

                if (_client.Browser.Options.TestMode)
                {
                    link += "&flags=testmode=true";
                }
                if (_client.Browser.Options.PerformanceMode)
                {
                    link += "&perf=true";
                }
                driver.Navigate(link);
                //SwitchToContent();
                driver.Wait();
                driver.Wait();
                driver.WaitUntilAvailable(this._entityReference.Form,
                    TimeSpan.FromSeconds(30),
                    "Dynamics 365 Record is Unavailable or not finished loading. Timeout Exceeded"
                );

                return true;
            });
        }

        /// <summary>
        /// Saves the entity
        /// </summary>
        /// <param name="thinkTime"></param>
        public BrowserCommandResult<bool> Save(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Save"), driver =>
            {
                //Actions action = new Actions(driver);
                //action.KeyDown(Keys.Control).SendKeys("S").Perform();

                driver.SendKeys(_client.ElementMapper.EntityReference.Form, new string[] { Keys.Control, "S" });

                Dialogs dialogs = new Dialogs(_client);
                dialogs.HandleSaveDialog();
                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SaveQuickCreate(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"SaveQuickCreate"), driver =>
            {
                var save = driver.WaitUntilAvailable(_client.ElementMapper.QuickCreateReference.SaveAndCloseButton,
                    "Quick Create Save Button is not available");
                save?.Click(_client, true);

                driver.Wait();

                return true;
            });
        }

        /// <summary>
        /// Open record set and navigate record index.
        /// This method supersedes Navigate Up and Navigate Down outside of UI 
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.OpenRecordSetNavigator();</example>
        public BrowserCommandResult<bool> EntityOpenRecordSetNavigator(int index = 0, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Open Record Set Navigator"), driver =>
            {
                // check if record set navigator parent div is set to open
                driver.Wait();

                if (!driver.HasElement(this._entityReference.RecordSetNavList))
                {
                    var navList = driver.FindElement(this._entityReference.RecordSetNavList);
                    driver.FindElement(this._entityReference.RecordSetNavigator).Click(_client);
                    driver.Wait();
                    navList = driver.FindElement(this._entityReference.RecordSetNavList);
                }

                var links = driver.FindElements(this._entityReference.RecordSetNavList + "//li");

                try
                {
                    links[index].Click(_client);
                }
                catch
                {
                    throw new InvalidOperationException($"No record with the index '{index}' exists.");
                }

                driver.Wait();

                return true;
            });
        }

        /// <summary>
        /// Close Record Set Navigator
        /// </summary>
        /// <param name="thinkTime"></param>
        /// <example>xrmApp.Entity.CloseRecordSetNavigator();</example>
        public BrowserCommandResult<bool> CloseRecordSetNavigator(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions("Close Record Set Navigator"), driver =>
            {
                var closeSpan = driver.HasElement(this._entityReference.RecordSetNavCollapseIcon);
                if (closeSpan)
                {
                    driver.FindElement(this._entityReference.RecordSetNavCollapseIconParent).Click(_client);
                }

                return true;
            });
        }













        internal BrowserCommandResult<Field> EntityGetField(string field)
        {
            return _client.Execute(_client.GetOptions($"Get Field"), driver =>
            {
                var fieldIElement = driver.WaitUntilAvailable(this._entityReference.TextFieldContainer.Replace("[NAME]", field));
                Field returnField = new Field(fieldIElement);
                returnField.Name = field;

                IElement fieldLabel = null;
                try
                {
                    fieldLabel = driver.FindElement(this._entityReference.TextFieldContainer.Replace("[NAME]", field) + this._entityReference.TextFieldLabel.Replace("[NAME]", field));
                }
                catch (KeyNotFoundException)
                {
                    // Swallow
                }

                if (fieldLabel != null)
                {
                    returnField.Label = fieldLabel.Text;
                }

                return returnField;
            });
        }

        internal BrowserCommandResult<string> EntityGetValue(string field)
        {
            return _client.Execute(_client.GetOptions($"Get Value"), driver =>
            {
                string text = string.Empty;
                var fieldContainer = driver.WaitUntilAvailable(this._entityReference.TextFieldContainer.Replace("[NAME]", field));

                if (driver.FindElements(this._entityReference.TextFieldContainer.Replace("[NAME]", field) + "//input").Count > 0)
                {
                    var input = driver.FindElement(this._entityReference.TextFieldContainer.Replace("[NAME]", field) + "//input");
                    if (input != null)
                    {
                        //IWebIElement fieldValue = input.FindElement(By.XPath(EntityReference.TextFieldValue].Replace("[NAME]", field)));
                        text = input.GetAttribute(_client, "value").ToString();

                        // Needed if getting a date field which also displays time as there isn't a date specifc GetValue method
                        var timefields = driver.FindElements(this._entityReference.FieldControlDateTimeTimeInput.Replace("[FIELD]", field));
                        if (timefields.Any())
                        {
                            text += $" {timefields.First().GetAttribute(_client,"value")}";
                        }
                    }
                }
                else if (driver.FindElements(this._entityReference.TextFieldContainer.Replace("[NAME]", field) + "//textarea").Count > 0)
                {
                    text = driver.FindElement(this._entityReference.TextFieldContainer.Replace("[NAME]", field) + "//textarea").GetAttribute(_client, "value");
                }
                else
                {
                    throw new Exception($"Field with name {field} does not exist.");
                }

                return text;
            });
        }

        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new Lookup { Name = "primarycontactid" });</example>
        public BrowserCommandResult<string> EntityGetValue(LookupItem control)
        {
            var controlName = control.Name;
            return _client.Execute($"Get Lookup Value: {controlName}", driver =>
            {
                var xpathToContainer = _client.ElementMapper.EntityReference.TextFieldLookupFieldContainer.Replace("[NAME]", controlName);
                IElement fieldContainer = driver.WaitUntilAvailable(xpathToContainer);
                string lookupValue = TryGetValue(fieldContainer, control);

                return lookupValue;
            });
        }
        private string TryGetValue(IElement fieldContainer, LookupItem control)
        {
            string[] lookupValues = TryGetValue(fieldContainer, new[] { control });
            string result = string.Join("; ", lookupValues);
            return result;
        }

        /// <summary>
        /// Gets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The lookup field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new LookupItem[] { new LookupItem { Name = "to" } });</example>
        public BrowserCommandResult<string[]> EntityGetValue(LookupItem[] controls)
        {
            var controlName = controls.First().Name;
            return _client.Execute($"Get ActivityParty Lookup Value: {controlName}", driver =>
            {
                var xpathToContainer = this._entityReference.TextFieldLookupFieldContainer.Replace("[NAME]", controlName);
                var fieldContainer = driver.WaitUntilAvailable(xpathToContainer);
                string[] result = TryGetValue(fieldContainer, controls);

                return result;
            });
        }

        private string[] TryGetValue(IElement fieldContainer, LookupItem[] controls)
        {
            var controlName = controls.First().Name;
            var xpathToExistingValues = _client.ElementMapper.EntityReference.LookupFieldExistingValue.Replace("[NAME]", controlName);
            var existingValues = _client.Browser.Browser.FindElements(fieldContainer.Locator + xpathToExistingValues);

            var xpathToExpandButton = this._entityReference.LookupFieldExpandCollapseButton.Replace("[NAME]", controlName);
            bool expandButtonFound = _client.Browser.Browser.HasElement(fieldContainer.Locator + xpathToExpandButton);
            if (expandButtonFound)
            {
                var expandButton = _client.Browser.Browser.FindElement(fieldContainer.Locator + xpathToExpandButton);
                expandButton.Click(_client, true);
                
                int count = existingValues.Count;
                
                //fieldContainer.WaitUntil(fc => fc.FindElements(xpathToExistingValues).Count > count);

                existingValues = _client.Browser.Browser.FindElements(fieldContainer.Locator + xpathToExistingValues);
            }
            
            Exception ex = null;
            try
            {
                if (existingValues.Count > 0)
                {
                    string[] lookupValues = existingValues.Select(v => v.GetAttribute(_client,"innerText").TrimSpecialCharacters()).ToArray(); //IE can return line breaks
                    return lookupValues;
                }

                if (_client.Browser.Browser.FindElements(fieldContainer.Locator + "//input").Any())
                    return new string[0];
            }
            catch (Exception e)
            {
                ex = e;
            }

            throw new InvalidOperationException($"Field: {controlName} Does not exist", ex);
        }

        /// <summary>
        /// Gets the value of a picklist or status field.
        /// </summary>
        /// <param name="control">The option you want to set.</param>
        /// <example>xrmApp.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        internal BrowserCommandResult<string> EntityGetValue(OptionSet control)
        {
            var controlName = control.Name;
            return _client.Execute($"Get OptionSet Value: {controlName}", driver =>
            {
                var xpathToFieldContainer = this._entityReference.OptionSetFieldContainer.Replace("[NAME]", controlName);
                var fieldContainer = driver.WaitUntilAvailable(xpathToFieldContainer);
                string result = TryGetValue(fieldContainer, control);

                return result;
            });
        }

        private string TryGetValue(IElement fieldContainer, OptionSet control)
        {
            bool success = _client.Browser.Browser.HasElement(fieldContainer.Locator + "//select");
            if (success)
            {
                var select = _client.Browser.Browser.FindElement(fieldContainer.Locator + "//select");
                var options = _client.Browser.Browser.FindElements(fieldContainer.Locator + "//select//option");
                string result = GetSelectedOption(options);
                return result;
            }

            var name = control.Name;
            var hasStatusCombo = _client.Browser.Browser.HasElement(fieldContainer.Locator + this._entityReference.EntityOptionsetStatusCombo.Replace("[NAME]", name));
            if (hasStatusCombo)
            {
                // This is for statuscode (type = status) that should act like an optionset doesn't doesn't follow the same pattern when rendered
                var valueSpan = _client.Browser.Browser.FindElement(fieldContainer.Locator + this._entityReference.EntityOptionsetStatusTextValue.Replace("[NAME]", name));
                return valueSpan.Text;
            }

            throw new InvalidOperationException($"OptionSet Field: '{name}' does not exist");
        }

        private static string GetSelectedOption(List<IElement> options)
        {
            var selectedOption = options.FirstOrDefault(op => op.Selected);
            return selectedOption?.Text ?? string.Empty;
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        /// <example>xrmApp.Entity.GetValue(new BooleanItem { Name = "creditonhold" });</example>
        internal BrowserCommandResult<bool> EntityGetValue(BooleanItem option)
        {
            return _client.Execute($"Get BooleanItem Value: {option.Name}", driver =>
            {
                var check = false;

                var fieldContainer = this._entityReference.TextFieldContainer.Replace("[NAME]", option.Name);

                var hasRadio = driver.HasElement(fieldContainer + this._entityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name));
                var hasCheckbox = driver.HasElement(fieldContainer + this._entityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name));
                var hasList = driver.HasElement(fieldContainer + this._entityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name));
                var hasToggle = driver.HasElement(fieldContainer + this._entityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name));

                if (hasRadio)
                {
                    var trueRadio = driver.FindElement(fieldContainer + this._entityReference.EntityBooleanFieldRadioTrue.Replace("[NAME]", option.Name));

                    check = bool.Parse(trueRadio.GetAttribute(_client,"aria-checked"));
                }
                else if (hasCheckbox)
                {
                    var checkbox = driver.FindElement(fieldContainer + this._entityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name));

                    check = bool.Parse(checkbox.GetAttribute(_client, "aria-checked"));
                }
                else if (hasList)
                {
                    var list = driver.FindElement(fieldContainer + this._entityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name));
                    var options = driver.FindElements(fieldContainer + this._entityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name) + "//option");
                    var selectedOption = options.FirstOrDefault(a => a.HasAttribute(_client,"data-selected") && bool.Parse(a.GetAttribute(_client,"data-selected")));

                    if (selectedOption != null)
                    {
                        check = int.Parse(selectedOption.GetAttribute(_client,"value")) == 1;
                    }
                }
                else if (hasToggle)
                {
                    var toggle = driver.FindElement(fieldContainer + this._entityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name));
                    var link = driver.FindElement(fieldContainer + this._entityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name) + "//button");

                    check = bool.Parse(link.GetAttribute(_client, "aria-checked"));
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                return check;
            });
        }

        /// <summary>
        /// Gets the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field</param>
        /// <returns>MultiValueOptionSet object where the values field contains all the contact names</returns>
        internal BrowserCommandResult<MultiValueOptionSet> EntityGetValue(MultiValueOptionSet option)
        {
            return _client.Execute(_client.GetOptions($"Get Multi Select Value: {option.Name}"), driver =>
            {
                MultiSelect multiSelect = new MultiSelect(_client.Configuration);
                var containerXPath = multiSelect.DivContainer.Replace("[NAME]", option.Name);
                var container = driver.WaitUntilAvailable(containerXPath, $"Multi-select option set {option.Name} not found.");

                container.Hover(_client,container.Locator);
                var expandButtonXPath = driver.FindElement(container.Locator + multiSelect.ExpandCollapseButton);
                var expandButton = driver.FindElement(container.Locator + expandButtonXPath);
                if (driver.HasElement(container.Locator + expandButtonXPath) && expandButton.IsClickable)
                {
                    
                    expandButton.Click(_client);
                }

                var selectedOptionsXPath = multiSelect.SelectedRecordLabel;
                var selectedOptions = driver.FindElements(containerXPath + selectedOptionsXPath);

                return new MultiValueOptionSet
                {
                    Name = option.Name,
                    Values = selectedOptions.Select(o => o.Text).ToArray()
                };
            });
        }


        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new DateTimeControl { Name = "scheduledstart" });</example>
        public BrowserCommandResult<DateTime?> EntityGetValue(DateTimeControl control)
            => _client.Execute($"Get DateTime Value: {control.Name}", driver => DateTimeControl.TryGetValue(_client, control: control));

        /// <summary>
        /// Returns the ObjectId of the entity
        /// </summary>
        /// <returns>Guid of the Entity</returns>
        internal BrowserCommandResult<Guid> EntityGetObjectId(int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Get Object Id"), driver =>
            {
                var objectId = driver.ExecuteScript("return Xrm.Page.data.entity.getId();");

                Guid oId;
                if (!Guid.TryParse(objectId.ToString(), out oId))
                    throw new KeyNotFoundException("Unable to retrieve object Id for this entity");

                return oId;
            });
        }

        /// <summary>
        /// Returns the Entity Name of the entity
        /// </summary>
        /// <returns>Entity Name of the Entity</returns>
        internal BrowserCommandResult<string> EntityGetEntityName(int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Get Entity Name"), driver =>
            {
                var entityName = driver.ExecuteScript("return Xrm.Page.data.entity.getEntityName();").ToString();

                if (string.IsNullOrEmpty(entityName))
                {
                    throw new KeyNotFoundException("Unable to retrieve Entity Name for this entity");
                }

                return entityName;
            });
        }

        /// <summary>
        /// Returns the Form Name of the entity
        /// </summary>
        /// <returns>Form Name of the Entity</returns>
        internal BrowserCommandResult<string> EntityGetFormName(int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Get Form Name"), driver =>
            {
                // Wait for form selector visible
                driver.WaitUntilAvailable(this._entityReference.FormSelector);

                string formName = driver.ExecuteScript("return Xrm.Page.ui.formContext.ui.formSelector.getCurrentItem().getLabel();").ToString();

                if (string.IsNullOrEmpty(formName))
                {
                    throw new KeyNotFoundException("Unable to retrieve Form Name for this entity");
                }

                return formName;
            });
        }

        /// <summary>
        /// Returns the Header Title of the entity
        /// </summary>
        /// <returns>Header Title of the Entity</returns>
        internal BrowserCommandResult<string> EntityGetHeaderTitle(int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Get Header Title"), driver =>
            {
                // Wait for form selector visible
                var headerTitle = driver.WaitUntilAvailable(this._entityReference.HeaderTitle, new TimeSpan(0, 0, 5), "Header title not found with XPath: '" + this._entityReference.HeaderTitle + "'");

                var headerTitleName = headerTitle?.GetAttribute(_client, "title");

                if (string.IsNullOrEmpty(headerTitleName))
                {
                    throw new KeyNotFoundException("Unable to retrieve Header Title for this entity");
                }

                return headerTitleName;
            });
        }


        //internal BrowserCommandResult<List<GridItem>> GetSubGridItems(string subgridName)
        //{
        //    return _client.Execute(_client.GetOptions($"Get Subgrid Items for Subgrid {subgridName}"), driver =>
        //    {
        //        var subGridRows = new List<GridItem>();

        //        // Find the SubGrid
        //        if (!driver.TryFindElement(By.XPath(EntityReference.SubGridContents].Replace("[NAME]", subgridName)), out var subGrid))
        //        {
        //            throw new NotFoundException($"Unable to locate {subgridName} subgrid.");
        //        }

        //        var entityName = subGrid
        //            .FindElement(By.XPath(AppIElements.Xpath[AppReference.Grid.Control]))
        //            .GetAttribute("data-lp-id")
        //            .Split('|')
        //            .ElementAt(4);
        //        var appUrl = driver.ExecuteScript($"return Xrm.Utility.getGlobalContext().getCurrentAppUrl();");

        //        GridItem CreateItem(int index)
        //        {
        //            var id = new Guid(
        //                (string)driver.ExecuteScript(
        //                    $"return Xrm.Page.getControl(\"{subgridName}\").getGrid().getRows().get({index}).getData().entity.getId()"));

        //            return new GridItem
        //            {
        //                EntityName = entityName,
        //                Id = id,
        //                Url = new Uri($"{appUrl}&pagetype=entityrecord&etn={entityName}&id={id}"),
        //            };
        //        }

        //        if (subGrid.HasElement(By.CssSelector(@"div.pcf-grid")))
        //        {
        //            // Read-only subgrid
        //            subGridRows = subGrid
        //                .FindElement(By.XPath(EntityReference.SubGridListCells]))
        //                .FindElements(By.XPath(EntityReference.SubGridRows]))
        //                .Select((row, index) =>
        //                {
        //                    var item = CreateItem(index);

        //                    var cells = row.FindElements(By.XPath(EntityReference.SubGridCells]));
        //                    foreach (var cell in cells)
        //                    {
        //                        var column = cell.GetAttribute<string>("col-id");
        //                        item[column] = cell.Text;
        //                    }

        //                    return item;
        //                })
        //                .ToList();
        //        }
        //        else if (subGrid.TryFindElement(By.XPath(EntityReference.EditableSubGridList].Replace("[NAME]", subgridName)), out var editableGridList))
        //        {
        //            // Editable subgrid
        //            var headers = subGrid
        //                .FindElements(By.XPath(EntityReference.SubGridHeadersEditable]))
        //                .Select(h => h.GetAttribute("title"))
        //                // 'Select' column
        //                .Skip(1);

        //            subGridRows = subGrid
        //                .FindElement(By.XPath(EntityReference.EditableSubGridListCells]))
        //                .FindElements(By.XPath(EntityReference.EditableSubGridListCellRows]))
        //                .Select((row, index) =>
        //                {
        //                    var item = CreateItem(index);

        //                    var cells = row
        //                        .FindElements(By.XPath(EntityReference.EditableSubGridCells]))
        //                        // 'Select' column
        //                        .Skip(1);

        //                    for (int i = 0; i < headers.Count(); i++)
        //                    {
        //                        item[headers.ElementAt(i)] = cells.ElementAt(i).Text;
        //                    }

        //                    return item;
        //                })
        //                .ToList();
        //        }
        //        else if (subGrid.TryFindElement(By.XPath(EntityReference.SubGridHighDensityList].Replace("[NAME]", subgridName)), out var highDensityGridList))
        //        {
        //            // Special 'Related' high density grid control for entity forms
        //            var columns = subGrid
        //                .FindElements(By.XPath(EntityReference.SubGridHeadersHighDensity]))
        //                .Select(h => h.GetAttribute("data-id"));

        //            subGridRows = subGrid
        //                .FindElements(By.XPath(EntityReference.SubGridRowsHighDensity]))
        //                .Select((row, index) =>
        //                {
        //                    var item = CreateItem(index);

        //                    var cells = row
        //                        .FindElements(By.XPath(EntityReference.SubGridCells]))
        //                        // 'Select' column
        //                        .Skip(1);

        //                    for (int i = 0; i < columns.Count(); i++)
        //                    {
        //                        item[columns.ElementAt(i)] = cells.ElementAt(i).Text;
        //                    }

        //                    return item;
        //                })
        //                .ToList();
        //        }

        //        return subGridRows;
        //    });
        //}

        //internal BrowserCommandResult<bool> OpenSubGridRecord(string subgridName, int index = 0)
        //{
        //    return _client.Execute(_client.GetOptions($"Open Subgrid record for subgrid {subgridName}"), driver =>
        //    {
        //        // Find the SubGrid
        //        var subGrid = driver.FindElement(By.XPath(EntityReference.SubGridContents].Replace("[NAME]", subgridName)));

        //        if (subGrid.HasElement(By.CssSelector(@"div.pcf-grid")))
        //        {
        //            // Read-only subgrid
        //            var subGridTable = subGrid.FindElement(By.XPath(EntityReference.SubGridListCells]));
        //            var rows = subGridTable.FindElements(By.XPath(EntityReference.SubGridRows]));

        //            if (rows.Count == 0)
        //            {
        //                throw new NoSuchElementException($"No records were found for subgrid {subgridName}");
        //            }
        //            else if (index + 1 > rows.Count)
        //            {
        //                throw new IndexOutOfRangeException($"Subgrid {subgridName} record count: {rows.Count}. Expected: {index + 1}");
        //            }

        //            var row = rows.ElementAt(index);
        //            var cell = row.FindElements(By.XPath(EntityReference.SubGridCells])).ElementAt(1);

        //            new Actions(driver).DoubleClick(cell).Perform();
        //            driver.WaitForTransaction();
        //        }
        //        else if (subGrid.TryFindElement(By.XPath(EntityReference.EditableSubGridList].Replace("[NAME]", subgridName)), out var subGridRecordList))
        //        {
        //            // Editable subgrid
        //            var editableGridListCells = subGridRecordList.FindElement(By.XPath(EntityReference.EditableSubGridListCells]));
        //            var editableGridCellRows = editableGridListCells.FindElements(By.XPath(EntityReference.EditableSubGridListCellRows]));
        //            var editableGridCellRow = editableGridCellRows[index + 1].FindElements(By.XPath("./div"));

        //            new Actions(driver).DoubleClick(editableGridCellRow[0]).Perform();
        //            driver.WaitForTransaction();
        //        }
        //        else
        //        {
        //            // Check for special 'Related' grid form control
        //            // This opens a limited form view in-line on the grid

        //            //Get the GridName
        //            string subGridName = subGrid.GetAttribute("data-id").Replace("dataSetRoot_", string.Empty);

        //            //cell-0 is the checkbox for each record
        //            var checkBox = driver.FindElement(
        //                By.XPath(
        //                    EntityReference.SubGridRecordCheckbox]
        //                    .Replace("[INDEX]", index.ToString())
        //                    .Replace("[NAME]", subGridName)));

        //            driver.DoubleClick(checkBox);
        //            driver.WaitForTransaction();
        //        }

        //        return true;
        //    });
        //}

        internal BrowserCommandResult<int> EntityGetSubGridItemsCount(string subgridName)
        {
            return _client.Execute(_client.GetOptions($"Get Subgrid Items Count for subgrid {subgridName}"), driver =>
            {
                List<GridItem> rows = GetSubGridItems(subgridName);
                return rows.Count;
            });
        }

        /// <summary>
        /// Click the magnifying glass icon for the lookup control supplied
        /// </summary>
        /// <param name="control">The LookupItem field on the form</param>
        /// <returns></returns>
        internal BrowserCommandResult<bool> EntitySelectLookup(LookupItem control)
        {
            return _client.Execute(_client.GetOptions($"Select Lookup Field {control.Name}"), driver =>
            {
                if (driver.HasElement(this._entityReference.FieldLookupButton.Replace("[NAME]", control.Name)))
                {
                    var lookupButton = driver.FindElement(this._entityReference.FieldLookupButton.Replace("[NAME]", control.Name));

                    lookupButton.Hover(_client, lookupButton.Locator);

                    driver.Wait();

                    driver.FindElement(this._entityReference.SearchButtonIcon).Click(_client, true);
                }
                else
                    throw new KeyNotFoundException($"Lookup field {control.Name} not found");

                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<string> EntityGetHeaderValue(LookupItem control)
        {
            var controlName = control.Name;
            return _client.Execute(_client.GetOptions($"Get Header LookupItem Value {controlName}"), driver =>
            {
                var xpathToContainer = this._entityReference.HeaderLookupFieldContainer.Replace("[NAME]", controlName);
                string lookupValue = ExecuteInHeaderContainer(driver, xpathToContainer, container => TryGetValue(container, control));

                return lookupValue;
            });
        }

        internal BrowserCommandResult<string[]> EntityGetHeaderValue(LookupItem[] controls)
        {
            var controlName = controls.First().Name;
            var xpathToContainer = this._entityReference.HeaderLookupFieldContainer.Replace("[NAME]", controlName);
            return _client.Execute(_client.GetOptions($"Get Header Activityparty LookupItem Value {controlName}"), driver =>
            {
                string[] lookupValues = ExecuteInHeaderContainer(driver, xpathToContainer, container => TryGetValue(container, controls));

                return lookupValues;
            });
        }

        internal BrowserCommandResult<string> EntityGetHeaderValue(string control)
        {
            return _client.Execute(_client.GetOptions($"Get Header Value {control}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                return GetValue(control);
            });
        }

        internal BrowserCommandResult<MultiValueOptionSet> EntityGetHeaderValue(MultiValueOptionSet control)
        {
            return _client.Execute(_client.GetOptions($"Get Header MultiValueOptionSet Value {control.Name}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                return GetValue(control);
            });
        }

        internal BrowserCommandResult<string> EntityGetHeaderValue(OptionSet control)
        {
            var controlName = control.Name;
            var xpathToContainer = this._entityReference.HeaderOptionSetFieldContainer.Replace("[NAME]", controlName);
            return _client.Execute(_client.GetOptions($"Get Header OptionSet Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer, container => TryGetValue(container, control))
            );
        }

        internal BrowserCommandResult<bool> EntityGetHeaderValue(BooleanItem control)
        {
            return _client.Execute(_client.GetOptions($"Get Header BooleanItem Value {control}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                return GetValue(control);
            });
        }

        internal BrowserCommandResult<DateTime?> EntityGetHeaderValue(DateTimeControl control)
        {
            var xpathToContainer = this._entityReference.HeaderDateTimeFieldContainer.Replace("[NAME]", control.Name);
            return _client.Execute(_client.GetOptions($"Get Header DateTime Value {control.Name}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    container => DateTimeControl.TryGetValue(_client, control)));
        }

        internal BrowserCommandResult<string> GetStateFromForm()
        {
            return _client.Execute(_client.GetOptions($"Get Status value from form"), driver =>
            {
                driver.Wait();
                if (!driver.HasElement("//*[id='message-formReadOnlyNotification']"))
                {
                    return "Active";
                }
                var readOnlyNotification = driver.FindElement("//*[id='message-formReadOnlyNotification']");
                var match = Regex.Match(readOnlyNotification.Text, "This record’s status: (.*)");
                if (match.Success)
                {
                    return match.Captures[1].Value;
                }

                try
                {
                    return EntityGetHeaderValue(new OptionSet { Name = "statecode" }).Value;
                }
                catch (Exception ex)
                {
                    throw new KeyNotFoundException("Unable to determine the status from the form. This can happen if you do not have access to edit the record and the state is not in the header.", ex);
                }
            });
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(string field, string value)
        {
            return _client.Execute(_client.GetOptions($"Set Header Value {field}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                Field objField = new Field(_client);
                objField.SetValue(_client, field, value, FormContextType.Header);

                TryCloseHeaderFlyout(driver);
                return true;
            });
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(LookupItem control)
        {
            var controlName = control.Name;
            bool isHeader = true;
            bool removeAll = true;
            var xpathToContainer = this._entityReference.HeaderLookupFieldContainer.Replace("[NAME]", controlName);
            return _client.Execute(_client.GetOptions($"Set Header LookupItem Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    fieldContainer =>
                    {
                        Lookup lookup = new Lookup(_client);
                        lookup.TryRemoveLookupValue(driver, fieldContainer, control, removeAll, isHeader);
                        lookup.TrySetValue(driver, fieldContainer, control);

                        TryCloseHeaderFlyout(driver);
                        return true;
                    }));
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(LookupItem[] controls, bool clearFirst = true)
        {
            var control = controls.First();
            var controlName = control.Name;
            var xpathToContainer = this._entityReference.HeaderLookupFieldContainer.Replace("[NAME]", controlName);
            return _client.Execute(_client.GetOptions($"Set Header Activityparty LookupItem Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    container =>
                    {
                        Lookup lookup = new Lookup(_client);
                        if (clearFirst)
                            lookup.TryRemoveLookupValue(driver, container, control);

                        lookup.TryToSetValue(driver, container, controls);

                        TryCloseHeaderFlyout(driver);
                        return true;
                    }));
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(OptionSet control)
        {
            var controlName = control.Name;
            var xpathToContainer = this._entityReference.HeaderOptionSetFieldContainer.Replace("[NAME]", controlName);
            return _client.Execute(_client.GetOptions($"Set Header OptionSet Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    container =>
                    {
                        control.TrySetValue(_client, container, control);

                        TryCloseHeaderFlyout(driver);
                        return true;
                    }));
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(MultiValueOptionSet control)
        {
            return _client.Execute(_client.GetOptions($"Set Header MultiValueOptionSet Value {control.Name}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                MultiValueOptionSet.SetValue(_client, control, FormContextType.Header);

                TryCloseHeaderFlyout(driver);
                return true;
            });
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(BooleanItem control)
        {
            return _client.Execute(_client.GetOptions($"Set Header BooleanItem Value {control.Name}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                control.SetValue(_client, control, FormContextType.Header);

                TryCloseHeaderFlyout(driver);
                return true;
            });
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(string field, DateTime value, string formatDate = null, string formatTime = null)
        {
            var control = new DateTimeControl(field)
            {
                Value = value,
                DateFormat = formatDate,
                TimeFormat = formatTime
            };
            return control.EntitySetHeaderValue(_client,this,control);
        }




        internal BrowserCommandResult<bool> EntitySelectForm(string formName)
        {
            return _client.Execute(_client.GetOptions($"Select Form {formName}"), driver =>
            {
                driver.Wait();

                if (!driver.HasElement(this._entityReference.FormSelector))
                    throw new KeyNotFoundException("Unable to find form selector on the form");

                var formSelector = driver.WaitUntilAvailable(this._entityReference.FormSelector);
                // Click didn't work with IE
                formSelector.SendKeys(_client,new string[] { Keys.Enter });

                driver.WaitUntilAvailable(this._entityReference.FormSelectorFlyout);

                var flyout = driver.FindElement(this._entityReference.FormSelectorFlyout);
                var forms = driver.FindElements(flyout.Locator + this._entityReference.FormSelectorItem);

                var form = forms.FirstOrDefault(a => a.GetAttribute(_client,"data-text").EndsWith(formName, StringComparison.OrdinalIgnoreCase));
                if (form == null)
                    throw new KeyNotFoundException($"Form {formName} is not in the form selector");

                driver.ClickWhenAvailable(form.GetAttribute(_client, "id"));

                driver.Wait(PageEvent.Load);
                driver.Wait();

                return true;
            });
        }

        internal TResult ExecuteInHeaderContainer<TResult>(IWebBrowser driver, string xpathToContainer, Func<IElement, TResult> function)
        {
            TResult lookupValue = default(TResult);

            TryExpandHeaderFlyout(driver);

            var xpathToFlyout = this._entityReference.HeaderFlyout;
            var flyout = driver.WaitUntilAvailable(xpathToFlyout, TimeSpan.FromSeconds(5), "Flyout not available in header container.");
            var container = driver.FindElement(xpathToContainer);
            lookupValue = function(container);
                //flyout =>
                //{
                //    IWebIElement container = flyout.FindElement(By.XPath(xpathToContainer));
                //    lookupValue = function(container);
                //}); ;

            return lookupValue;
        }

        internal void TryExpandHeaderFlyout(IWebBrowser driver)
        {
            driver.WaitUntilAvailable(
                this._entityReference.HeaderContainer,
                "Unable to find header on the form");

            var xPath = this._entityReference.HeaderFlyoutButton;
            var headerFlyoutButton = driver.FindElement(xPath);
            bool expanded = bool.Parse(headerFlyoutButton.GetAttribute(_client,"aria-expanded"));

            if (!expanded)
                headerFlyoutButton.Click(_client,true);
        }

        //internal void TryCloseHeaderFlyout(IWebBrowser driver)
        //{
        //    bool hasHeader = driver.HasElement(this._entityReference.HeaderContainer);
        //    if (!hasHeader)
        //        throw new NotFoundException("Unable to find header on the form");

        //    var xPath = By.XPath(this._entityReference.HeaderFlyoutButton);
        //    var headerFlyoutButton = driver.FindElement(xPath);
        //    bool expanded = bool.Parse(headerFlyoutButton.GetAttribute("aria-expanded"));

        //    if (expanded)
        //        headerFlyoutButton.Click(true);
        //}

        internal void TryCloseHeaderFlyout(IWebBrowser driver)
        {
            bool hasHeader = driver.HasElement(this._entityReference.HeaderContainer);
            if (!hasHeader)
                throw new KeyNotFoundException("Unable to find header on the form");

            var xPath = this._entityReference.HeaderFlyoutButton;
            var headerFlyoutButton = driver.FindElement(xPath);
            bool expanded = bool.Parse(headerFlyoutButton.GetAttribute(_client, "aria-expanded"));

            if (expanded)
                headerFlyoutButton.Click(_client, true);
        }



        #endregion

    }
}
