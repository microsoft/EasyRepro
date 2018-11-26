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

        public void SetValue(string field, string value)
        {
            _client.SetValue(field, value);
        }

        public void Save()
        {
            _client.SaveQuickCreate();
        }


    }
}