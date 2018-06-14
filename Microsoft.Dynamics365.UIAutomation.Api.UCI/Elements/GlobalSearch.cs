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

        public bool Search(string criteria)
        {
            return _client.GlobalSearch(criteria);
        }
        public bool FilterWith(string entity)
        {
            return _client.FilterWith(entity);
        }
        public bool OpenRecord(string entity, int index)
        {
            return _client.OpenGlobalSearchRecord(entity, index);
        }
    }
}
