// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.Generic;

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
            _client.SwitchView(viewName);
        }
        public void OpenRecord(int index)
        {
            _client.OpenRecord(index);
        }
        public void Search(string searchCriteria)
        {
            _client.Search(searchCriteria);
        }

        public void GetGridItems()
        {
            _client.GetGridItems();
        }

        public List<GridItem> GetGridItems(bool returnItem= false)
        {
            return(_client.GetGridItems());
        }

        public void Sort(string columnName)
        {
            _client.Sort(columnName);
        }
    }
}
