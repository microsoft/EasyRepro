// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class BusinessProcessFlow : Element
    {
        private readonly WebClient _client;

        public BusinessProcessFlow(WebClient client)
        {
            _client = client;
        }

        public void SetActive(string stageName = "")
        {
            // This makes the assumption that SelectStage() has already been called
            _client.SetActive(stageName);
        }

        public void NextStage(string stageName, Field businessProcessFlowField = null)
        {
            //Keeping code here for now so I don't check-in and overwrite changes to WebClient.cs
            _client.NextStage(stageName, businessProcessFlowField);
        }

        public void SelectStage(string stageName, Field businessProcessFlowField = null)
        {
            _client.SelectStage(stageName, businessProcessFlowField);
        }
    }
}
