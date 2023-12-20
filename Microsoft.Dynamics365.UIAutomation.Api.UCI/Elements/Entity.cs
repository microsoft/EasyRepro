// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.BusinessProcessFlow;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using System.Web;
using System.Text.RegularExpressions;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Entity : Element
    {

        #region DTO

        public static class EntityReference
        {
            public static string FormContext = "//*[@data-id='editFormRoot']";
            public static string FormSelector = "//*[@data-id='form-selector']";
            public static string HeaderTitle = "//*[@data-id='header_title']";
            public static string HeaderContext = ".//div[@data-id='headerFieldsFlyout']";
            public static string Save = "//button[@role='menuitem' and .//*[text()='Save']]";
            public static string TextFieldContainer = ".//*[contains(@id, \'[NAME]-FieldSectionItemContainer\')]";
            public static string TextFieldLabel = ".//label[contains(@id, \'[NAME]-field-label\')]";
            public static string TextFieldValue = ".//input[contains(@data-id, \'[NAME].fieldControl\')]";
            public static string TextFieldLookup = ".//*[contains(@id, \'systemuserview_id.fieldControl-LookupResultsDropdown')]";
            public static string TextFieldLookupSearchButton = ".//button[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_search')]";
            public static string TextFieldLookupMenu = "//div[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]') and contains(@data-id,'tabContainer')]";
            public static string LookupFieldExistingValue = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag') and @role='link']";
            public static string LookupFieldDeleteExistingValue = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag_delete')]";
            public static string LookupFieldExpandCollapseButton = ".//button[contains(@data-id,'[NAME].fieldControl-LookupResultsDropdown_[NAME]_expandCollapse')]/descendant::label[not(text()='+0')]";
            public static string LookupFieldNoRecordsText = ".//*[@data-id=\'[NAME].fieldControl-LookupResultsDropdown_[NAME]_No_Records_Text']";
            public static string LookupFieldResultList = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_tab')]";
            public static string LookupFieldResultListItem = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_resultsContainer')]";
            public static string LookupFieldHoverExistingValue = ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_SelectedRecordList')]";
            public static string LookupResultsDropdown = "//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_tab')]";
            public static string OptionSetFieldContainer = ".//div[@data-id='[NAME].fieldControl-option-set-container']";
            public static string TextFieldLookupFieldContainer = ".//div[@data-id='[NAME].fieldControl-Lookup_[NAME]']";
            public static string RecordSetNavigator = "//button[contains(@data-lp-id, 'recordset-navigator')]";
            public static string RecordSetNavigatorOpen = "//button[contains(@data-lp-id, 'recordset-navigator')]";
            public static string RecordSetNavList = "//ul[contains(@data-id, 'recordSetNavList')]";
            public static string RecordSetNavCollapseIcon = "//*[contains(@data-id, 'recordSetNavCollapseIcon')]";
            public static string RecordSetNavCollapseIconParent = "//*[contains(@data-id, 'recordSetNavCollapseIcon')]";
            public static string FieldControlDateTimeContainer = "//div[@data-id='[NAME]-FieldSectionItemContainer']";
            public static string FieldControlDateTimeInputUCI = ".//*[contains(@data-id, '[FIELD].fieldControl-date-time-input')]";
            public static string FieldControlDateTimeTimeInputUCI = ".//div[contains(@data-id,'[FIELD].fieldControl._timecontrol-datetime-container')]/div/div/input";
            public static string Delete = "//button[contains(@data-id,'Delete')]";
            public static string Assign = "//button[contains(@data-id,'Assign')]";
            public static string SwitchProcess = "//button[contains(@data-id,'SwitchProcess')]";
            public static string CloseOpportunityWin = "//button[contains(@data-id,'MarkAsWon')]";
            public static string CloseOpportunityLoss = "//button[contains(@data-id,'MarkAsLost')]";
            public static string ProcessButton = "//button[contains(@data-id,'MBPF.ConvertTo')]";
            public static string SwitchProcessDialog = "Entity_SwitchProcessDialog";
            public static string TabList = ".//ul[contains(@id, \"tablist\")]";
            public static string Tab = ".//li[@title='{0}']";
            public static string MoreTabs = ".//div[@data-id='more_button']";
            public static string MoreTabsMenu = "//div[@id='__flyoutRootNode']";
            public static string SubTab = "//div[@id=\"__flyoutRootNode\"]//span[text()=\"{0}\"]";
            
            public static string FieldLookupButton = "//button[contains(@data-id,'[NAME]_search')]";
            public static string SearchButtonIcon = "//span[contains(@data-id,'microsoftIcon_searchButton')]";
            public static string DuplicateDetectionWindowMarker = "//div[contains(@data-id,'ManageDuplicates')]";
            public static string DuplicateDetectionGridRows = "//div[contains(@class,'data-selectable')]";
            public static string DuplicateDetectionIgnoreAndSaveButton = "//button[contains(@data-id,'ignore_save')]";
            public static string EntityBooleanFieldRadioContainer = "//div[contains(@data-id, '[NAME].fieldControl-checkbox-container') and contains(@role,'radiogroup')]";
            public static string EntityBooleanFieldRadioTrue = "//div[contains(@data-id, '[NAME].fieldControl-checkbox-containercheckbox-inner-second')]";
            public static string EntityBooleanFieldRadioFalse = "//div[contains(@data-id, '[NAME].fieldControl-checkbox-containercheckbox-inner-first')]";
            public static string EntityBooleanFieldButtonContainer = "//div[contains(@data-id, '[NAME].fieldControl_container')]";
            public static string EntityBooleanFieldButtonTrue = ".//label[contains(@class, 'first-child')]";
            public static string EntityBooleanFieldButtonFalse = ".//label[contains(@class, 'last-child')]";
            public static string EntityBooleanFieldCheckboxContainer = "//div[contains(@data-id, '[NAME].fieldControl-checkbox-container')]";
            public static string EntityBooleanFieldCheckbox = "//input[contains(@data-id, '[NAME].fieldControl-checkbox-toggle')]";
            public static string EntityBooleanFieldList = "//select[contains(@data-id, '[NAME].fieldControl-checkbox-select')]";
            public static string EntityBooleanFieldFlipSwitchLink = "//div[contains(@data-id, '[NAME]-FieldSectionItemContainer')]";
            public static string EntityBooleanFieldFlipSwitchContainer = "//div[@data-id= '[NAME].fieldControl_container']";
            public static string EntityBooleanFieldToggle = "//div[contains(@data-id, '[NAME].fieldControl-toggle-container')]";
            public static string EntityOptionsetStatusCombo = "//div[contains(@data-id, '[NAME].fieldControl-pickliststatus-comboBox')]";
            public static string EntityOptionsetStatusComboButton = "//div[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_button')]";
            public static string EntityOptionsetStatusComboList = "//ul[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_list')]";
            public static string EntityOptionsetStatusTextValue = "//span[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_text-value')]";
            public static string FormMessageBar = "//*[@id=\"notificationMessageAndButtons\"]/div/div/span";
            public static string FormMessageBarTypeIcon = ".//span[contains(@data-id,'formReadOnlyIcon')]";
            public static string FormNotifcationBar = "//div[contains(@data-id, 'notificationWrapper')]";
            public static string FormNotifcationExpandButton = ".//span[@id='notificationExpandIcon']";
            public static string FormNotifcationFlyoutRoot = "//div[@id='__flyoutRootNode']";
            public static string FormNotifcationList = ".//ul[@data-id='notificationList']";
            public static string FormNotifcationTypeIcon = ".//span[contains(@id,'notification_icon_')]";

            public static class Header
            {
                public static string Container = "//div[contains(@data-id,'form-header')]";
                public static string Flyout = "//div[@data-id='headerFieldsFlyout']";
                public static string FlyoutButton = "//button[contains(@id,'headerFieldsExpandButton')]";
                public static string LookupFieldContainer = "//div[@data-id='header_[NAME].fieldControl-Lookup_[NAME]']";
                public static string TextFieldContainer = "//div[@data-id='header_[NAME].fieldControl-text-box-container']";
                public static string OptionSetFieldContainer = "//div[@data-id='header_[NAME]']";
                public static string DateTimeFieldContainer = "//div[@data-id='header_[NAME]-FieldSectionItemContainer']";
            }
        }
        #endregion
        private readonly WebClient _client;

        public SubGrid SubGrid => this.GetElement<SubGrid>(_client);
        public RelatedGrid RelatedGrid => this.GetElement<RelatedGrid>(_client);

        public Entity(WebClient client) : base()
        {
            _client = client;
        }

        #region public
        public T GetElement<T>(WebClient client)
    where T : Element
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
            _client.ClearValue(field, FormContextType.Entity);
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
            _client.ClearValue(control, FormContextType.Entity);
        }

        /// <summary>
        /// Clears a value from the OptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(OptionSet control)
        {
            _client.ClearValue(control, FormContextType.Entity);
        }

        /// <summary>
        /// Clears a value from the MultiValueOptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(MultiValueOptionSet control)
        {
            _client.ClearValue(control, FormContextType.Entity);
        }


        /// <summary>
        /// Clears a value from the DateTimeControl provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearHeaderValue(DateTimeControl control)
        {
            this.EntityClearHeaderValue(control);
        }

        /// <summary>
        /// Clears a value from the DateTimeControl provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(DateTimeControl control)
        {
            _client.ClearValue(control, FormContextType.Entity);
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
            _client.Browser.Driver.WaitForTransaction();
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
            return _client.GetFormNotifications().Value;
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
        /// This method supersedes Navigate Up and Navigate Down outside of UCI 
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
        /// Saves the entity
        /// </summary>
        public void Save()
        {
            this.Save();
            Dialogs dialogs = new Dialogs(_client);
            dialogs.HandleSaveDialog();
            _client.Browser.Driver.WaitForTransaction();
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
            this.EntitySetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of a field
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        public void SetValue(string field, string value)
        {
            _client.SetValue(field, value, FormContextType.Entity);
        }

        /// <summary>
        /// Sets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        public void SetValue(LookupItem control)
        {
            _client.SetValue(control, FormContextType.Entity);
        }

        /// <summary>
        /// Sets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void SetValue(LookupItem[] controls)
        {
            _client.SetValue(controls, FormContextType.Entity);
        }

        /// <summary>
        /// Sets the value of a picklist or status field.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(OptionSet optionSet)
        {
            _client.SetValue(optionSet, FormContextType.Entity);
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        public void SetValue(BooleanItem option)
        {
            _client.SetValue(option, FormContextType.Entity);
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
            _client.SetValue(field, date, FormContextType.Entity, formatDate, formatTime);
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="control">Date field control.</param>
        public void SetValue(DateTimeControl control)
        {
            _client.SetValue(control, FormContextType.Entity);
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            _client.SetValue(option, FormContextType.Entity, removeExistingValues);
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
            _client.AddValues(controls);
        }

        /// <summary>
        /// Removes values from an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.RemoveValues(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void RemoveValues(LookupItem[] controls)
        {
            _client.RemoveValues(controls);
        }
        #endregion
        #region internals
        internal BrowserCommandResult<bool> Assign(string userOrTeamToAssign, int thinkTime = Constants.DefaultThinkTime)
        {
            //Click the Assign Button on the Entity Record
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Assign Entity"), driver =>
            {
                var assignBtn = driver.WaitUntilAvailable(By.XPath(EntityReference.Assign),
                    "Assign Button is not available");

                assignBtn?.Click();
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
                IWebElement tabList;
                if (driver.HasElement(By.XPath(Dialogs.DialogsReference.DialogContext)))
                {
                    var dialogContainer = driver.FindElement(By.XPath(Dialogs.DialogsReference.DialogContext));
                    tabList = dialogContainer.WaitUntilAvailable(By.XPath(EntityReference.TabList));
                }
                else
                {
                    tabList = driver.WaitUntilAvailable(By.XPath(EntityReference.TabList));
                }

                ClickTab(tabList, EntityReference.Tab, tabName);

                //Click Sub Tab if provided
                if (!String.IsNullOrEmpty(subTabName))
                {
                    this.ClickTab(tabList, EntityReference.SubTab, subTabName);
                }

                driver.WaitForTransaction();
                return true;
            });
        }

        internal void ClickTab(IWebElement tabList, string xpath, string name)
        {
            IWebElement moreTabsButton;
            IWebElement listItem;
            // Look for the tab in the tab list, else in the more tabs menu
            IWebElement searchScope = null;
            if (tabList.HasElement(By.XPath(string.Format(xpath, name))))
            {
                searchScope = tabList;
            }
            else if (tabList.TryFindElement(By.XPath(EntityReference.MoreTabs), out moreTabsButton))
            {
                moreTabsButton.Click();

                // No tab to click - subtabs under 'Related' are automatically expanded in overflow menu
                if (name == "Related")
                {
                    return;
                }
                else
                {
                    searchScope = _client.Browser.Driver.FindElement(By.XPath(EntityReference.MoreTabsMenu));
                }
            }

            if (searchScope.TryFindElement(By.XPath(string.Format(xpath, name)), out listItem))
            {
                listItem.Click(true);
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
                var save = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.CancelButton]),
                    "Quick Create Cancel Button is not available");
                save?.Click(true);

                driver.WaitForTransaction();

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
                var uri = new Uri(_client.Browser.Driver.Url);
                var qs = HttpUtility.ParseQueryString(uri.Query.ToLower());
                var appId = qs.Get("appid");
                var link = $"{uri.Scheme}://{uri.Authority}/main.aspx?appid={appId}&etn={entityName}&pagetype=entityrecord&id={id}";

                if (_client.Browser.Options.UCITestMode)
                {
                    link += "&flags=testmode=true";
                }
                if (_client.Browser.Options.UCIPerformanceMode)
                {
                    link += "&perf=true";
                }

                driver.Navigate().GoToUrl(link);

                //SwitchToContent();
                driver.WaitForPageToLoad();
                driver.WaitForTransaction();
                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Entity.Form]),
                    TimeSpan.FromSeconds(30),
                    "CRM Record is Unavailable or not finished loading. Timeout Exceeded"
                );

                return true;
            });
        }

        internal BrowserCommandResult<bool> SaveQuickCreate(int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"SaveQuickCreate"), driver =>
            {
                var save = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.SaveAndCloseButton]),
                    "Quick Create Save Button is not available");
                save?.Click(true);

                driver.WaitForTransaction();

                return true;
            });
        }

        /// <summary>
        /// Open record set and navigate record index.
        /// This method supersedes Navigate Up and Navigate Down outside of UCI 
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
                driver.WaitForTransaction();

                if (!driver.TryFindElement(By.XPath(EntityReference.RecordSetNavList), out var navList))
                {
                    driver.FindElement(By.XPath(EntityReference.RecordSetNavigator)).Click();
                    driver.WaitForTransaction();
                    navList = driver.FindElement(By.XPath(EntityReference.RecordSetNavList));
                }

                var links = navList.FindElements(By.TagName("li"));

                try
                {
                    links[index].Click();
                }
                catch
                {
                    throw new InvalidOperationException($"No record with the index '{index}' exists.");
                }

                driver.WaitForTransaction();

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
                var closeSpan = driver.HasElement(By.XPath(EntityReference.RecordSetNavCollapseIcon));
                if (closeSpan)
                {
                    driver.FindElement(By.XPath(EntityReference.RecordSetNavCollapseIconParent)).Click();
                }

                return true;
            });
        }




        internal static ICollection<IWebElement> GetListItems(IWebElement container, LookupItem control)
        {
            var name = control.Name;
            var xpathToItems = By.XPath(EntityReference.LookupFieldResultListItem.Replace("[NAME]", name));

            //wait for complete the search
            container.WaitUntil(d => d.FindVisible(xpathToItems)?.Text?.Contains(control.Value, StringComparison.OrdinalIgnoreCase) == true);

            ICollection<IWebElement> result = container.WaitUntil(
                d => d.FindElements(xpathToItems),
                failureCallback: () => throw new InvalidOperationException($"No Results Matching {control.Value} Were Found.")
                );
            return result;
        }



        public static void SelectOption(ReadOnlyCollection<IWebElement> options, string value)
        {
            var selectedOption = options.FirstOrDefault(op => op.Text == value || op.GetAttribute("value") == value);
            selectedOption.Click(true);
        }




        internal BrowserCommandResult<Field> EntityGetField(string field)
        {
            return _client.Execute(_client.GetOptions($"Get Field"), driver =>
            {
                var fieldElement = driver.WaitUntilAvailable(By.XPath(EntityReference.TextFieldContainer.Replace("[NAME]", field)));
                Field returnField = new Field(fieldElement);
                returnField.Name = field;

                IWebElement fieldLabel = null;
                try
                {
                    fieldLabel = fieldElement.FindElement(By.XPath(EntityReference.TextFieldLabel.Replace("[NAME]", field)));
                }
                catch (NoSuchElementException)
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
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(EntityReference.TextFieldContainer.Replace("[NAME]", field)));

                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    var input = fieldContainer.FindElement(By.TagName("input"));
                    if (input != null)
                    {
                        //IWebElement fieldValue = input.FindElement(By.XPath(EntityReference.TextFieldValue].Replace("[NAME]", field)));
                        text = input.GetAttribute("value").ToString();

                        // Needed if getting a date field which also displays time as there isn't a date specifc GetValue method
                        var timefields = driver.FindElements(By.XPath(EntityReference.FieldControlDateTimeTimeInputUCI.Replace("[FIELD]", field)));
                        if (timefields.Any())
                        {
                            text += $" {timefields.First().GetAttribute("value")}";
                        }
                    }
                }
                else if (fieldContainer.FindElements(By.TagName("textarea")).Count > 0)
                {
                    text = fieldContainer.FindElement(By.TagName("textarea")).GetAttribute("value");
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
                var xpathToContainer = EntityReference.TextFieldLookupFieldContainer.Replace("[NAME]", controlName);
                IWebElement fieldContainer = driver.WaitUntilAvailable(By.XPath(xpathToContainer));
                string lookupValue = TryGetValue(fieldContainer, control);

                return lookupValue;
            });
        }
        private string TryGetValue(IWebElement fieldContainer, LookupItem control)
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
                var xpathToContainer = By.XPath(EntityReference.TextFieldLookupFieldContainer.Replace("[NAME]", controlName));
                var fieldContainer = driver.WaitUntilAvailable(xpathToContainer);
                string[] result = TryGetValue(fieldContainer, controls);

                return result;
            });
        }

        private string[] TryGetValue(IWebElement fieldContainer, LookupItem[] controls)
        {
            var controlName = controls.First().Name;
            var xpathToExistingValues = By.XPath(EntityReference.LookupFieldExistingValue.Replace("[NAME]", controlName));
            var existingValues = fieldContainer.FindElements(xpathToExistingValues);

            var xpathToExpandButton = By.XPath(EntityReference.LookupFieldExpandCollapseButton.Replace("[NAME]", controlName));
            bool expandButtonFound = fieldContainer.TryFindElement(xpathToExpandButton, out var expandButton);
            if (expandButtonFound)
            {
                expandButton.Click(true);

                int count = existingValues.Count;
                fieldContainer.WaitUntil(fc => fc.FindElements(xpathToExistingValues).Count > count);

                existingValues = fieldContainer.FindElements(xpathToExistingValues);
            }

            Exception ex = null;
            try
            {
                if (existingValues.Count > 0)
                {
                    string[] lookupValues = existingValues.Select(v => v.GetAttribute("innerText").TrimSpecialCharacters()).ToArray(); //IE can return line breaks
                    return lookupValues;
                }

                if (fieldContainer.FindElements(By.TagName("input")).Any())
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
                var xpathToFieldContainer = EntityReference.OptionSetFieldContainer.Replace("[NAME]", controlName);
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(xpathToFieldContainer));
                string result = TryGetValue(fieldContainer, control);

                return result;
            });
        }

        private static string TryGetValue(IWebElement fieldContainer, OptionSet control)
        {
            bool success = fieldContainer.TryFindElement(By.TagName("select"), out IWebElement select);
            if (success)
            {
                var options = select.FindElements(By.TagName("option"));
                string result = GetSelectedOption(options);
                return result;
            }

            var name = control.Name;
            var hasStatusCombo = fieldContainer.HasElement(By.XPath(EntityReference.EntityOptionsetStatusCombo.Replace("[NAME]", name)));
            if (hasStatusCombo)
            {
                // This is for statuscode (type = status) that should act like an optionset doesn't doesn't follow the same pattern when rendered
                var valueSpan = fieldContainer.FindElement(By.XPath(EntityReference.EntityOptionsetStatusTextValue.Replace("[NAME]", name)));
                return valueSpan.Text;
            }

            throw new InvalidOperationException($"OptionSet Field: '{name}' does not exist");
        }

        private static string GetSelectedOption(ReadOnlyCollection<IWebElement> options)
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

                var fieldContainer = driver.WaitUntilAvailable(By.XPath(EntityReference.TextFieldContainer.Replace("[NAME]", option.Name)));

                var hasRadio = fieldContainer.HasElement(By.XPath(EntityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name)));
                var hasCheckbox = fieldContainer.HasElement(By.XPath(EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name)));
                var hasList = fieldContainer.HasElement(By.XPath(EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name)));
                var hasToggle = fieldContainer.HasElement(By.XPath(EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name)));

                if (hasRadio)
                {
                    var trueRadio = fieldContainer.FindElement(By.XPath(EntityReference.EntityBooleanFieldRadioTrue.Replace("[NAME]", option.Name)));

                    check = bool.Parse(trueRadio.GetAttribute("aria-checked"));
                }
                else if (hasCheckbox)
                {
                    var checkbox = fieldContainer.FindElement(By.XPath(EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name)));

                    check = bool.Parse(checkbox.GetAttribute("aria-checked"));
                }
                else if (hasList)
                {
                    var list = fieldContainer.FindElement(By.XPath(EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name)));
                    var options = list.FindElements(By.TagName("option"));
                    var selectedOption = options.FirstOrDefault(a => a.HasAttribute("data-selected") && bool.Parse(a.GetAttribute("data-selected")));

                    if (selectedOption != null)
                    {
                        check = int.Parse(selectedOption.GetAttribute("value")) == 1;
                    }
                }
                else if (hasToggle)
                {
                    var toggle = fieldContainer.FindElement(By.XPath(EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name)));
                    var link = toggle.FindElement(By.TagName("button"));

                    check = bool.Parse(link.GetAttribute("aria-checked"));
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
                var containerXPath = By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", option.Name));
                var container = driver.WaitUntilAvailable(containerXPath, $"Multi-select option set {option.Name} not found.");

                container.Hover(driver, true);
                var expandButtonXPath = By.XPath(AppElements.Xpath[AppReference.MultiSelect.ExpandCollapseButton]);
                if (container.TryFindElement(expandButtonXPath, out var expandButton) && expandButton.IsClickable())
                {
                    expandButton.Click();
                }

                var selectedOptionsXPath = By.XPath(AppElements.Xpath[AppReference.MultiSelect.SelectedRecordLabel]);
                var selectedOptions = container.FindElements(selectedOptionsXPath);

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
            => _client.Execute($"Get DateTime Value: {control.Name}", driver => WebClient.TryGetValue(driver, container: driver, control: control));

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
                    throw new NotFoundException("Unable to retrieve object Id for this entity");

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
                    throw new NotFoundException("Unable to retrieve Entity Name for this entity");
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
                driver.WaitUntilVisible(By.XPath(EntityReference.FormSelector));

                string formName = driver.ExecuteScript("return Xrm.Page.ui.formContext.ui.formSelector.getCurrentItem().getLabel();").ToString();

                if (string.IsNullOrEmpty(formName))
                {
                    throw new NotFoundException("Unable to retrieve Form Name for this entity");
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
                var headerTitle = driver.WaitUntilVisible(By.XPath(EntityReference.HeaderTitle), new TimeSpan(0, 0, 5));

                var headerTitleName = headerTitle?.GetAttribute("title");

                if (string.IsNullOrEmpty(headerTitleName))
                {
                    throw new NotFoundException("Unable to retrieve Header Title for this entity");
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
        //            .FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Control]))
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
                if (driver.HasElement(By.XPath(EntityReference.FieldLookupButton.Replace("[NAME]", control.Name))))
                {
                    var lookupButton = driver.FindElement(By.XPath(EntityReference.FieldLookupButton.Replace("[NAME]", control.Name)));

                    lookupButton.Hover(driver);

                    driver.WaitForTransaction();

                    driver.FindElement(By.XPath(EntityReference.SearchButtonIcon)).Click(true);
                }
                else
                    throw new NotFoundException($"Lookup field {control.Name} not found");

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<string> EntityGetHeaderValue(LookupItem control)
        {
            var controlName = control.Name;
            return _client.Execute(_client.GetOptions($"Get Header LookupItem Value {controlName}"), driver =>
            {
                var xpathToContainer = EntityReference.Header.LookupFieldContainer.Replace("[NAME]", controlName);
                string lookupValue = ExecuteInHeaderContainer(driver, xpathToContainer, container => TryGetValue(container, control));

                return lookupValue;
            });
        }

        internal BrowserCommandResult<string[]> EntityGetHeaderValue(LookupItem[] controls)
        {
            var controlName = controls.First().Name;
            var xpathToContainer = EntityReference.Header.LookupFieldContainer.Replace("[NAME]", controlName);
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
            var xpathToContainer = EntityReference.Header.OptionSetFieldContainer.Replace("[NAME]", controlName);
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
            var xpathToContainer = EntityReference.Header.DateTimeFieldContainer.Replace("[NAME]", control.Name);
            return _client.Execute(_client.GetOptions($"Get Header DateTime Value {control.Name}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    container => WebClient.TryGetValue(driver, container, control)));
        }

        internal BrowserCommandResult<string> GetStateFromForm()
        {
            return _client.Execute(_client.GetOptions($"Get Status value from form"), driver =>
            {
                driver.WaitForTransaction();
                if (!driver.TryFindElement(By.Id("message-formReadOnlyNotification"), out var readOnlyNotification))
                {
                    return "Active";
                }

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
                    throw new NotFoundException("Unable to determine the status from the form. This can happen if you do not have access to edit the record and the state is not in the header.", ex);
                }
            });
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(string field, string value)
        {
            return _client.Execute(_client.GetOptions($"Set Header Value {field}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                _client.SetValue(field, value, FormContextType.Header);

                TryCloseHeaderFlyout(driver);
                return true;
            });
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(LookupItem control)
        {
            var controlName = control.Name;
            bool isHeader = true;
            bool removeAll = true;
            var xpathToContainer = EntityReference.Header.LookupFieldContainer.Replace("[NAME]", controlName);
            return _client.Execute(_client.GetOptions($"Set Header LookupItem Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    fieldContainer =>
                    {
                        WebClient.TryRemoveLookupValue(driver, fieldContainer, control, removeAll, isHeader);
                        _client.TrySetValue(driver, fieldContainer, control);

                        TryCloseHeaderFlyout(driver);
                        return true;
                    }));
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(LookupItem[] controls, bool clearFirst = true)
        {
            var control = controls.First();
            var controlName = control.Name;
            var xpathToContainer = EntityReference.Header.LookupFieldContainer.Replace("[NAME]", controlName);
            return _client.Execute(_client.GetOptions($"Set Header Activityparty LookupItem Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    container =>
                    {
                        if (clearFirst)
                            WebClient.TryRemoveLookupValue(driver, container, control);

                        _client.TryToSetValue(driver, container, controls);

                        TryCloseHeaderFlyout(driver);
                        return true;
                    }));
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(OptionSet control)
        {
            var controlName = control.Name;
            var xpathToContainer = EntityReference.Header.OptionSetFieldContainer.Replace("[NAME]", controlName);
            return _client.Execute(_client.GetOptions($"Set Header OptionSet Value {controlName}"),
                driver => ExecuteInHeaderContainer(driver, xpathToContainer,
                    container =>
                    {
                        WebClient.TrySetValue(container, control);

                        TryCloseHeaderFlyout(driver);
                        return true;
                    }));
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(MultiValueOptionSet control)
        {
            return _client.Execute(_client.GetOptions($"Set Header MultiValueOptionSet Value {control.Name}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                _client.SetValue(control, FormContextType.Header);

                TryCloseHeaderFlyout(driver);
                return true;
            });
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(BooleanItem control)
        {
            return _client.Execute(_client.GetOptions($"Set Header BooleanItem Value {control.Name}"), driver =>
            {
                TryExpandHeaderFlyout(driver);

                _client.SetValue(control, FormContextType.Header);

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
            return EntitySetHeaderValue(control);
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(DateTimeControl control)
            => _client.Execute(_client.GetOptions($"Set Header Date/Time Value: {control.Name}"), driver => TrySetHeaderValue(driver, control));

        internal BrowserCommandResult<bool> EntityClearHeaderValue(DateTimeControl control)
        {
            var controlName = control.Name;
            return _client.Execute(_client.GetOptions($"Clear Header Date/Time Value: {controlName}"),
                driver => TrySetHeaderValue(driver, new DateTimeControl(controlName)));
        }

        private bool TrySetHeaderValue(IWebDriver driver, DateTimeControl control)
        {
            var xpathToContainer = EntityReference.Header.DateTimeFieldContainer.Replace("[NAME]", control.Name);
            return ExecuteInHeaderContainer(driver, xpathToContainer,
                container => _client.TrySetValue(driver, container, control, FormContextType.Header));
        }


        internal BrowserCommandResult<bool> EntitySelectForm(string formName)
        {
            return _client.Execute(_client.GetOptions($"Select Form {formName}"), driver =>
            {
                driver.WaitForTransaction();

                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.FormSelector])))
                    throw new NotFoundException("Unable to find form selector on the form");

                var formSelector = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.FormSelector]));
                // Click didn't work with IE
                formSelector.SendKeys(Keys.Enter);

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Entity.FormSelectorFlyout]));

                var flyout = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.FormSelectorFlyout]));
                var forms = flyout.FindElements(By.XPath(Elements.Xpath[Reference.Entity.FormSelectorItem]));

                var form = forms.FirstOrDefault(a => a.GetAttribute("data-text").EndsWith(formName, StringComparison.OrdinalIgnoreCase));
                if (form == null)
                    throw new NotFoundException($"Form {formName} is not in the form selector");

                driver.ClickWhenAvailable(By.Id(form.GetAttribute("id")));

                driver.WaitForPageToLoad();
                driver.WaitForTransaction();

                return true;
            });
        }

        internal TResult ExecuteInHeaderContainer<TResult>(IWebDriver driver, string xpathToContainer, Func<IWebElement, TResult> function)
        {
            TResult lookupValue = default(TResult);

            TryExpandHeaderFlyout(driver);

            var xpathToFlyout = EntityReference.Header.Flyout;
            driver.WaitUntilVisible(By.XPath(xpathToFlyout), TimeSpan.FromSeconds(5),
                flyout =>
                {
                    IWebElement container = flyout.FindElement(By.XPath(xpathToContainer));
                    lookupValue = function(container);
                });

            return lookupValue;
        }

        internal void TryExpandHeaderFlyout(IWebDriver driver)
        {
            driver.WaitUntilAvailable(
                By.XPath(EntityReference.Header.Container),
                "Unable to find header on the form");

            var xPath = By.XPath(EntityReference.Header.FlyoutButton);
            var headerFlyoutButton = driver.FindElement(xPath);
            bool expanded = bool.Parse(headerFlyoutButton.GetAttribute("aria-expanded"));

            if (!expanded)
                headerFlyoutButton.Click(true);
        }

        internal void TryCloseHeaderFlyout(IWebDriver driver)
        {
            bool hasHeader = driver.HasElement(By.XPath(EntityReference.Header.Container));
            if (!hasHeader)
                throw new NotFoundException("Unable to find header on the form");

            var xPath = By.XPath(EntityReference.Header.FlyoutButton);
            var headerFlyoutButton = driver.FindElement(xPath);
            bool expanded = bool.Parse(headerFlyoutButton.GetAttribute("aria-expanded"));

            if (expanded)
                headerFlyoutButton.Click(true);
        }



        #endregion

    }
}
