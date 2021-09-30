// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class QuickCreate : Element
    {
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
            _client.CancelQuickCreate();
        }

        /// <summary>
        /// Clears a value from the text or date field provided
        /// </summary>
        /// <param name="field"></param>
        public void ClearValue(string field)
        {
            _client.ClearValue(field, FormContextType.QuickCreate);
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
            _client.ClearValue(control, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clears a value from the OptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(OptionSet control)
        {
            _client.ClearValue(control, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clears a value from the MultiValueOptionSet provided
        /// </summary>
        /// <param name="control"></param>
        public void ClearValue(MultiValueOptionSet control)
        {
            _client.ClearValue(control, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Gets the value of a field in the quick create form
        /// </summary>
        /// <param name="field">Schema name of the field</param>
        /// <param name="value">Value of the field</param>
        public string GetValue(string field)
        {
            return _client.GetValue(field);
        }

        /// <summary>
        /// Gets the value of a DateTime field in the quick create form.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The value.</returns>
        public DateTime? GetValue(DateTimeControl control)
        {
            return _client.GetValue(control);
        }

        /// <summary>
        /// Gets the value of a LookupItem field in the quick create form
        /// </summary>
        /// <param name="control">LookupItem of the field to set</param>
        public string GetValue(LookupItem field)
        {
            return _client.GetValue(field);
        }

        /// <summary>
        /// Gets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public string GetValue(OptionSet field)
        {
            return _client.GetValue(field);
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
        /// Gets the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        public MultiValueOptionSet GetValue(MultiValueOptionSet field)
        {
            return _client.GetValue(field);
        }

        /// <summary>
        /// Sets the value of a field in the quick create form
        /// </summary>
        /// <param name="field">Schema name of the field</param>
        /// <param name="value">Value of the field</param>
        public void SetValue(string field, string value)
        {
            _client.SetValue(field, value, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Sets the value of a LookupItem field in the quick create form
        /// </summary>
        /// <param name="control">LookupItem of the field to set</param>
        public void SetValue(LookupItem control)
        {
            _client.SetValue(control, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Sets the value of a picklist in the quick create form.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(OptionSet optionSet)
        {
            _client.SetValue(optionSet, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Sets the value of a Boolean Item in the quick create form.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public void SetValue(BooleanItem optionSet)
        {
            _client.SetValue(optionSet, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Sets the value of a Date Field in the quick create form.
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="format">Datetime format matching Short Date & Time formatting personal options.</param>
        public void SetValue(string field, DateTime date, string formatDate = null, string formatTime = null)
        {
            _client.SetValue(field, date, FormContextType.QuickCreate, formatDate, formatTime);
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            _client.SetValue(option, FormContextType.QuickCreate, removeExistingValues);
        }

        /// <summary>
        /// Click the Save button on the quick create form
        /// </summary>
        public void Save()
        {
            _client.SaveQuickCreate();
        }


    }
}