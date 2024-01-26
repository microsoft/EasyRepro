// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class QuickCreate 
    {

        #region DTO
        public class QuickCreateReference
        {
            public const string QuickCreate = "QuickCreate";
            private string _quickCreateFormContext = "//section[contains(@data-id,'quickCreateRoot')]";
            private string _saveButton = "//button[contains(@id,'quickCreateSaveBtn')]";
            private string _saveAndCloseButton = "//button[contains(@id,'quickCreateSaveAndCloseBtn')]";
            private string _cancelButton = "//button[contains(@id,'quickCreateCancelBtn')]";

            public string QuickCreateFormContext { get => _quickCreateFormContext; set { _quickCreateFormContext = value; } }
            public string SaveButton { get => _saveButton; set { _saveButton = value; } }
            public string SaveAndCloseButton { get => _saveAndCloseButton; set { _saveAndCloseButton = value; } }
            public string CancelButton { get => _cancelButton; set { _cancelButton = value; } }
        }
        #endregion
        private readonly WebClient _client;

        public QuickCreate(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Click the Cancel button on the quick create form
        /// </summary>
        public void Cancel()
        {
            Entity entity = new Entity(_client);
            //return entity.GetValue(field);
            entity.CancelQuickCreate();
        }

        /// <summary>
        /// Clears a value from the text or date field provided
        /// </summary>
        /// <param name="field"></param>
        public void ClearValue(string field)
        {
            Field.ClearValue(_client, field, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clears a value from the LookupItem provided
        /// Can be used on a lookup, customer, owner, or activityparty field
        /// </summary>
        /// <param name="control"></param>
        /// <example>xrmApp.QuickCreate.ClearValue(new LookupItem { Name = "parentcustomerid" });</example>
        /// <example>xrmApp.QuickCreate.ClearValue(new LookupItem { Name = "to" });</example>
        public void ClearValue(LookupItem control)
        {
            Lookup lookup = new Lookup(_client);
            lookup.ClearValue(control, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clears a value from the OptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(OptionSet control)
        {
            control.ClearValue(_client, control, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clears a value from the MultiValueOptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(MultiValueOptionSet control)
        {
            control.ClearValue(_client, control, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Gets the value of a field in the quick create form
        /// </summary>
        /// <param name="field">Schema name of the field</param>
        /// <param name="value">Value of the field</param>
        public string GetValue(string field)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(field);
        }

        /// <summary>
        /// Gets the value of a DateTime field in the quick create form.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The value.</returns>
        public DateTime? GetValue(DateTimeControl control)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(control);
        }

        /// <summary>
        /// Gets the value of a LookupItem field in the quick create form
        /// </summary>
        /// <param name="control">LookupItem of the field to set</param>
        public string GetValue(LookupItem field)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(field);
        }

        /// <summary>
        /// Gets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public string GetValue(OptionSet field)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(field);
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
        /// Gets the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        public MultiValueOptionSet GetValue(MultiValueOptionSet field)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(field);
        }

        /// <summary>
        /// Sets the value of a field in the quick create form
        /// </summary>
        /// <param name="field">Schema name of the field</param>
        /// <param name="value">Value of the field</param>
        public void SetValue(string field, string value)
        {
            Field objField = new Field(_client);
            objField.SetValue(_client, field, value, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Sets the value of a LookupItem field in the quick create form
        /// </summary>
        /// <param name="control">LookupItem of the field to set</param>
        public void SetValue(LookupItem control)
        {
            Lookup lookup = new Lookup(_client);
            lookup.SetValue(control, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Sets the value of a picklist in the quick create form.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(OptionSet optionSet)
        {
            OptionSet.SetValue(_client, optionSet, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Sets the value of a Boolean Item in the quick create form.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(BooleanItem optionSet)
        {
            optionSet.SetValue(_client, optionSet, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Sets the value of a Date Field in the quick create form.
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="format">Datetime format matching Short Date & Time formatting personal options.</param>
        public void SetValue(string field, DateTime date, string formatDate = null, string formatTime = null)
        {
            DateTimeControl.SetValue(_client, field, date, FormContextType.QuickCreate, formatDate, formatTime);
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            MultiValueOptionSet.SetValue(_client, option, FormContextType.QuickCreate, removeExistingValues);
        }

        /// <summary>
        /// Click the Save button on the quick create form
        /// </summary>
        public void Save()
        {
            Entity entity = new Entity(_client);
            entity.SaveQuickCreate();
        }


    }
}