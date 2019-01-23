// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    ///  Related Grid class.
    ///  </summary>
    public class RelatedGrid : Element
    {
        private readonly WebClient _client;
        public RelatedGrid(WebClient client) : base()
        {
            _client = client;
        }
        public void OpenGridRow(int index)
        {
            _client.OpenGridRow(index);
        }

        public void OpenEditableGridRow(string entity,int index)
        {
            _client.OpenEditableGridRow(entity,index);
        }
    }
}
