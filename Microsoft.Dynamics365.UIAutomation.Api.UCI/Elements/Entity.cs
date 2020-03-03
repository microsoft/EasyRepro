// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Entity : Element
    {
        private readonly WebClient _client;

        public Entity(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Clicks the Assign button and completes the Assign dialog
        /// </summary>
        /// <param name="userOrTeam">Name of the user or team to assign the record to</param>
        public void Assign(string userOrTeam)
        {
            _client.Assign(userOrTeam);
        }

        /// <summary>
        /// Clears a value from the text or date field provided
        /// </summary>
        /// <param name="field"></param>
        public void ClearValue(string field)
        {
            _client.ClearValue(field);
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
            _client.ClearValue(control);
        }

        /// <summary>
        /// Clears a value from the OptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(OptionSet control)
        {
            _client.ClearValue(control);
        }

        /// <summary>
        /// Clears a value from the MultiValueOptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(MultiValueOptionSet control)
        {
            _client.ClearValue(control);
        }
        

        /// <summary>
        /// Clears a value from the DateTimeControl provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearHeaderValue(DateTimeControl control)
        {
            _client.ClearHeaderValue(control);
        }

        /// <summary>
        /// Clears a value from the DateTimeControl provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(DateTimeControl control)
        {
            _client.ClearValue(control);
        }

        /// <summary>
        /// Close Record Set Navigator
        /// </summary>
        /// <param name="thinkTime"></param>
        /// <example>xrmApp.Entity.CloseRecordSetNavigator();</example>
        public void CloseRecordSetNavigator()
        {
            _client.CloseRecordSetNavigator();
        }

        /// <summary>
        /// Clicks the Delete button on the command bar for an entity
        /// </summary>
        public void Delete()
        {
            _client.Delete();
        }

        public Field GetField(string field)
        {
            return _client.GetField(field);
        }

        /// <summary>
        /// Gets the value of the status from the footer
        /// </summary>
        /// <returns>Status of the entity record</returns>
        public string GetFooterStatusValue()
        {
            return _client.GetStatusFromFooter();
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
            return _client.GetHeaderValue(control);
        }

        /// <summary>
        /// Gets the value of an ActivityParty Lookup from the header
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.GetHeaderValue(new LookupItem[] { new LookupItem { Name = "to" } });</example>
        public string[] GetHeaderValue(LookupItem[] controls)
        {
            return _client.GetValue(controls);
        }

        /// <summary>
        /// Gets the value of a picklist or status field from the header
        /// </summary>
        /// <param name="option">The option you want to Get.</param>
        /// <example>xrmBrowser.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        public string GetHeaderValue(OptionSet control)
        {
            return _client.GetHeaderValue(control);
        }
        
        /// <summary>
        /// Gets the value of a Boolean Item from the header
        /// </summary>
        /// <param name="control">The boolean field you want to Get.</param>
        /// <example>xrmApp.Entity.GetHeaderValue(new BooleanItem { Name = "preferredcontactmethodcode"}); </example>
        public bool GetHeaderValue(BooleanItem control)
        {
            return _client.GetHeaderValue(control);
        }

        /// <summary>
        /// Gets the value of a text or date field from the header
        /// </summary>
        /// <param name="control">The schema name of the field</param>
        /// <example>xrmApp.Entity.GetHeaderValue("emailaddress1");</example>
        public string GetHeaderValue(string control)
        {
            return _client.GetHeaderValue(control);
        }

        /// <summary>
        /// Gets the value of a  MultiValueOptionSet from the header
        /// </summary>
        /// <param name="option">The option you want to Get.</param>
        public MultiValueOptionSet GetHeaderValue(MultiValueOptionSet control)
        {
            return _client.GetHeaderValue(control);
        }
        
        /// <summary>
        /// Gets the value of a DateTime Control from the header
        /// </summary>
        /// <param name="control">The date time field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new DateTimeControl { Name = "estimatedclosedate" });</example>
        public DateTime? GetHeaderValue(DateTimeControl control)
        {
            return _client.GetHeaderValue(control);
        }
        
        /// <summary>
        /// Get the object id of the current entity
        /// </summary>
        public Guid GetObjectId()
        {
            return _client.GetObjectId();
        }

        /// <summary>
        /// Get the Entity Name of the current entity
        /// </summary>
        public string GetEntityName()
        {
            return _client.GetEntityName();
        }

        /// <summary>
        /// Retrieve the items from a subgrid
        /// </summary>
        /// <param name="subgridName">Label of the subgrid to retrieve items from</param>
        public List<GridItem> GetSubGridItems(string subgridName)
        {
            return _client.GetSubGridItems(subgridName);
        }

        /// <summary>
        /// Retrieves the number of rows from a subgrid
        /// </summary>
        /// <param name="subgridName">Label of the subgrid to retrieve items from</param>
        /// <returns></returns>
        public int GetSubGridItemsCount(string subgridName)
        {
            return _client.GetSubGridItemsCount(subgridName);
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
        public void GetValue(MultiValueOptionSet option)
        {
            _client.GetValue(option);
        }

        /// <summary>
        /// Open Entity
        /// </summary>
        /// <param name="entityName">The entity name</param>
        /// <param name="id">The Id</param>
        public void OpenEntity(string entityname, Guid id)
        {
            _client.OpenEntity(entityname, id);
        }

        /// <summary>
        /// Open record set and navigate record index.
        /// This method supersedes Navigate Up and Navigate Down outside of UCI 
        /// </summary>
        /// <param name="index">The index.</param>
        public void OpenRecordSetNavigator(int index = 0)
        {
            _client.OpenRecordSetNavigator(index);
        }

        /// <summary>
        /// Open a record on a subgrid
        /// </summary>
        /// <param name="subgridName">Label of the subgrid</param>
        /// <param name="index">Index of the record to open</param>
        public void OpenSubGridRecord(string subgridName, int index = 0)
        {
            _client.OpenSubGridRecord(subgridName, index);
        }

        public void AddSubgridItem(string subgridName)
        {
            _client.ClickSubgridAddButton(subgridName);
        }

        /// <summary>
        /// Saves the entity
        /// </summary>
        public void Save()
        {
            _client.Save();
            _client.HandleSaveDialog();
            _client.Browser.Driver.WaitForTransaction();
        }

        /// <summary>
        /// Selects a Lookup Field
        /// </summary>
        /// <param name="control">LookupItem with the schema name of the field</param>
        public void SelectLookup(LookupItem control)
        {
            _client.SelectLookup(control);
        }

        /// <summary>
        /// Opens any tab on the web page.
        /// </summary>
        /// <param name="tabName">The name of the tab based on the References class</param>
        public void SelectTab(string tabName, string subTabName = "")
        {
            _client.SelectTab(tabName, subTabName);
        }

        public void SetHeaderValue(string field, string value)
        {
            _client.SetHeaderValue(field, value);
        }

        /// <summary>
        /// Sets the value of a Lookup in the header
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        public void SetHeaderValue(LookupItem control)
        {
            _client.SetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of am ActivityParty Lookup in the header
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetHeaderValue(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void SetHeaderValue(LookupItem[] controls)
        {
            _client.SetHeaderValue(controls);
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control in the header
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        public void SetHeaderValue(MultiValueOptionSet control)
        {
            _client.SetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of a picklist or status field in the header
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetHeaderValue(OptionSet control)
        {
            _client.SetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of a BooleanItem in the header
        /// </summary>
        /// <param name="control">The boolean field you want to set.</param>
        public void SetHeaderValue(BooleanItem control)
        {
            _client.SetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of a Date field in the header
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="formatDate">Datetime format matching Short Date formatting personal options.</param>
        /// <param name="formatTime">Datetime format matching Short Time formatting personal options.</param>
        /// <example>xrmApp.Entity.SetHeaderValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        public void SetHeaderValue(string field, DateTime date, string formatDate = null, string formatTime = null)
        {
            _client.SetHeaderValue(field, date, formatDate, formatTime);
        }

        /// <summary>
        /// Sets the value of a BooleanItem in the header
        /// </summary>
        /// <param name="control">The boolean field you want to set.</param>
        public void SetHeaderValue(DateTimeControl control)
        {
            _client.SetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of a field
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        public void SetValue(string field, string value)
        {
            _client.SetValue(field, value);
        }

        /// <summary>
        /// Sets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        public void SetValue(LookupItem control)
        {
            _client.SetValue(control);
        }

        /// <summary>
        /// Sets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The activityparty lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new LookupItem[] { new LookupItem { Name = "to", Value = "A. Datum Corporation (sample)" } });</example>
        public void SetValue(LookupItem[] controls)
        {
            _client.SetValue(controls);
        }

        /// <summary>
        /// Sets the value of a picklist or status field.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(OptionSet optionSet)
        {
            _client.SetValue(optionSet);
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        public void SetValue(BooleanItem option)
        {
            _client.SetValue(option);
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="formatDate">Datetime format matching Short Date formatting personal options.</param>
        /// <param name="formatTime">Datetime format matching Short Time formatting personal options.</param>
        /// <example>xrmApp.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        public void SetValue(string field, DateTime date, string formatDate = null, string formatTime = null)
        {
            _client.SetValue(field, date, formatDate, formatTime);
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="control">Date field control.</param>
        public void SetValue(DateTimeControl control)
        {
            _client.SetValue(control);
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            _client.SetValue(option, removeExistingValues);
        }
        
        /// <summary>
        /// Click Process>Switch Process
        /// </summary>
        /// <param name="processToSwitchTo">Name of the process to switch to</param>
        public void SwitchProcess(string processToSwitchTo)
        {
            _client.SwitchProcess(processToSwitchTo);
        }

        /// <summary>
        /// Switches forms using the form selector
        /// </summary>
        /// <param name="formName">Name of the form</param>
        /// <example>xrmApp.Entity.SelectForm("AI for Sales");</example>
        public void SelectForm(string formName)
        {
            _client.SelectForm(formName);
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
    }   
}
