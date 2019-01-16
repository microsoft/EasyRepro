// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Grid : Element
    {
        private readonly WebClient _client;

        public Grid(WebClient client) : base()
        {
            _client = client;
        }

        public void SwitchView(string viewName)
        {
            _client.SwitchView(viewName, _client.Browser.Options.DefaultThinkTime);
        }
        public void OpenRecord(int index)
        {
            _client.OpenRecord(index, _client.Browser.Options.DefaultThinkTime);
        }
        public void Search(string searchCriteria, bool clearByDefault = true)
        {
            _client.Search(searchCriteria, clearByDefault, _client.Browser.Options.DefaultThinkTime);
        }

        public void ClearSearch()
        {
            _client.ClearSearch(_client.Browser.Options.DefaultThinkTime);
        }

        public void GetGridItems()
        {
            _client.GetGridItems(_client.Browser.Options.DefaultThinkTime);
        }

        public void Sort(string columnName)
        {
            _client.Sort(columnName, _client.Browser.Options.DefaultThinkTime);
        }
    }
}
