// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class BusinessProcessFlow : Element
    {
        private readonly WebClient _client;

        public BusinessProcessFlow(WebClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Retrieves the value of a text field
        /// </summary>
        /// <param name="field">Schema name of the field to retrieve</param>
        /// <returns></returns>
        public string GetValue(string field)
        {
            return _client.GetValue(field);
        }

        /// <summary>
        /// Retrieves the value of a Lookup field
        /// </summary>
        /// <param name="field">LookupItem with the schema name of the field to retrieve
        public string GetValue(LookupItem field)
        {
            return _client.GetValue(field);
        }

        /// <summary>
        /// Retrieves the value of a OptionSet field
        /// </summary>
        /// <param name="field">OptionSet with the schema name of the field to retrieve
        public string GetValue(OptionSet field)
        {
            return _client.GetValue(field);
        }

        /// <summary>
        /// Sets the stage provided to Active
        /// </summary>
        /// <param name="stageName">Name of the business process flow stage</param>
        public void SetActive(string stageName = "")
        {
            _client.SetActive(stageName);
        }

        /// <summary>
        /// Clicks "Next Stage" on the stage provided
        /// </summary>
        /// <param name="stageName">Name of the business process flow stage</param>
        /// <param name="businessProcessFlowField">Optional - field to set the value on for this business process flow stage</param>
        public void NextStage(string stageName, Field businessProcessFlowField = null)
        {
            _client.NextStage(stageName, businessProcessFlowField);
        }

        /// <summary>
        /// Selects the stage provided
        /// </summary>
        /// <param name="stageName">Name of the business process flow stage</param>
        public void SelectStage(string stageName)
        {
            _client.SelectStage(stageName);
        }

        /// <summary>
        /// Sets the value of a text field
        /// </summary>
        /// <param name="field">Schema name of the field to retrieve</param>
        public void SetValue(string field, string value)
        {
            _client.BPFSetValue(field, value);
        }

        /// <summary>
        /// Sets the value of an OptionSet field
        /// </summary>
        /// <param name="field">OptionSet with the schema name of the field to retrieve</param>
        public void SetValue(OptionSet optionSet)
        {
            _client.BPFSetValue(optionSet);
        }

        /// <summary>
        /// Sets the value of a BooleanItem field
        /// </summary>
        /// <param name="field">BooleanItem with the schema name of the field to retrieve</param>
        public void SetValue(BooleanItem optionSet)
        {
            _client.BPFSetValue(optionSet);
        }

        /// <summary>
        /// Sets the value of a LookupItem field
        /// </summary>
        /// <param name="field">LookupItem with the schema name of the field to retrieve</param>
        public void SetValue(LookupItem control, int index = 0)
        {
            _client.SetValue(control, index);
        }

        /// <summary>
        /// Sets the value of a Date field
        /// </summary>
        /// <param name="field">Schema name of the field to retrieve</param>
        public void SetValue(string field, DateTime date, string format = "MM dd yyyy")
        {
            _client.SetValue(field, date, format);
        }

        /// <summary>
        /// Sets the value of a MultiValueOptionSet field
        /// </summary>
        /// <param name="field">MultiValueOptionSet with the schema name of the field to retrieve</param>
        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            _client.SetValue(option, removeExistingValues);
        }

        /// <summary>
        /// Pins the Business Process Flow Stage to the right side of the window
        /// </summary>
        /// <param name="stageName">The name of the Business Process Flow Stage</param>
        public void Pin(string stageName)
        {
            _client.BPFPin(stageName);
        }

        /// <summary>
        /// Clicks the "X" button in the Business Process Flow flyout menu for the Stage provided
        /// </summary>
        /// <param name="stageName">Name of the business process flow stage</param>
        public void Close(string stageName)
        {
            _client.BPFClose(stageName);
        }
    }
}
