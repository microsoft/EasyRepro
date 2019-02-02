// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
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
        /// Clears a value from the field provided
        /// </summary>
        /// <param name="field"></param>
        public void ClearValue(string field)
        {
            _client.ClearValue(field);
        }

        /// <summary>
        /// Clears a value from the LookupItem provided
        /// </summary>
        /// <param name="control"></param>
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
        /// Gets the value of an OptionSet from the header
        /// </summary>
        /// <param name="option">The option you want to Get.</param>
        /// <example>xrmBrowser.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        public string GetHeaderValue(OptionSet control)
        {
            return _client.GetHeaderValue(control);
        }

        /// <summary>
        /// Gets the value of a field from the header
        /// </summary>
        /// <param name="control">The schema name of the field</param>
        /// <example>xrmBrowser.Entity.GetValue("emailaddress1");</example>
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
        /// Get the object id of the current entity
        /// </summary>
        public Guid GetObjectId()
        {
            return _client.GetObjectId();
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
        /// Gets the value of a field.
        /// </summary>
        /// <param name="control">The schema name of the field</param>
        public string GetValue(string field)
        {
            return _client.GetValue(field);
        }

        /// <summary>
        /// Gets the value of a OptionSet.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public string GetValue(OptionSet optionSet)
        {
            return _client.GetValue(optionSet);
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

        /// <summary>
        /// Saves the entity
        /// </summary>
        public void Save()
        {
            _client.Save();
            _client.HandleSaveDialog();
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
        /// Sets/Removes the value from the multselect type control in the header
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        public void SetHeaderValue(MultiValueOptionSet control)
        {
            _client.SetHeaderValue(control);
        }

        /// <summary>
        /// Sets the value of an OptionSet in the header
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetHeaderValue(OptionSet control)
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
        public void SetValue(LookupItem control, int index = 0)
        {
            _client.SetValue(control, index);
        }

        /// <summary>
        /// Sets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(OptionSet optionSet)
        {
            _client.SetValue(optionSet);
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(BooleanItem optionSet)
        {
            _client.SetValue(optionSet);
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="format">DateTime format</param>
        public void SetValue(string field, DateTime date, string format ="MM dd yyyy")
        {
            _client.SetValue(field, date, format);
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
    }   
}
