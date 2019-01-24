// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

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
        /// Click the Save button on the quick create form
        /// </summary>
        public void Save()
        {
            _client.SaveQuickCreate();
        }


    }
}