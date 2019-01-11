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

        public void SwitchView(string viewName)
        {
            _client.SwitchLookupView(viewName);
        }
        public void OpenRecord(int index)
        {
            _client.OpenLookupRecord(index);
        }

        public void SelectRelatedEntity(string entityLabel)
        {
            _client.SelectLookupRelatedEntity(entityLabel);
        }

        public void New()
        {
            _client.SelectLookupNewButton();
        }

    }
}
