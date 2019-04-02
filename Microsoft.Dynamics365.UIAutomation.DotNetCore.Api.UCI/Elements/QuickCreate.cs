// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
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
        /// Gets the value of a field in the quick create form
        /// </summary>
        /// <param name="field">Schema name of the field</param>
        /// <param name="value">Value of the field</param>
        public string GetValue(string field)
        {
            return _client.GetValue(field);
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
            _client.SetValue(field, value);
        }

        /// <summary>
        /// Sets the value of a LookupItem field in the quick create form
        /// </summary>
        /// <param name="control">LookupItem of the field to set</param>
        public void SetValue(LookupItem control)
        {
            _client.SetValue(control);
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
        public void SetValue(string field, DateTime date, string format = "MM dd yyyy")
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
        /// Click the Save button on the quick create form
        /// </summary>
        public void Save()
        {
            _client.SaveQuickCreate();
        }


    }
}