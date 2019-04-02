// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Lookup : Element
    {
        private readonly WebClient _client;

        public Lookup(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Opens a record from a lookup control
        /// </summary>
        /// <param name="index">Index of the row to open</param>
        public void OpenRecord(int index)
        {
            _client.OpenLookupRecord(index);
        }

        /// <summary>
        /// Clicks the New button in a lookup control
        /// </summary>
        public void New()
        {
            _client.SelectLookupNewButton();
        }

        /// <summary>
        /// Searches records in a lookup control
        /// </summary>
        /// <param name="control">LookupItem with the name of the lookup field</param>
        /// <param name="searchCriteria">Criteria used for search</param>
        public void Search(LookupItem control, string searchCriteria)
        {
            _client.SearchLookupField(control, searchCriteria);
        }

        /// <summary>
        /// Selects a related entity on a lookup control
        /// </summary>
        /// <param name="entityLabel">Name of the entity to select</param>
        public void SelectRelatedEntity(string entityLabel)
        {
            _client.SelectLookupRelatedEntity(entityLabel);
        }

        /// <summary>
        /// Clicks the "Change View" button on a lookup control and selects the view provided
        /// </summary>
        /// <param name="viewName">Name of the view to select</param>
        public void SwitchView(string viewName)
        {
            _client.SwitchLookupView(viewName);
        }

    }
}
