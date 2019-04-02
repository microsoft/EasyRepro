// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class GlobalSearch : Element
    {
        private readonly WebClient _client;

        public GlobalSearch(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Search using Relevance Search
        /// </summary>
        /// <param name="criteria">Criteria to search for</param>
        /// <returns></returns>
        public bool Search(string criteria)
        {
            return _client.GlobalSearch(criteria);
        }

        /// <summary>
        /// Filter by entity in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to filter with.</param>
        /// <example>xrmBrowser.GlobalSearch.FilterWith("Account");</example>
        public bool FilterWith(string entity)
        {
            return _client.FilterWith(entity);
        }

        /// <summary>
        /// Opens the specified record in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to open a record.</param>
        /// <param name="index">The index of the record you want to open.</param>
        /// <example>xrmBrowser.GlobalSearch.OpenRecord("Accounts",0);</example>
        public bool OpenRecord(string entity, int index)
        {
            return _client.OpenGlobalSearchRecord(entity, index);
        }
    }
}
