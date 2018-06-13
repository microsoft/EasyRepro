// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Dialogs : Element
    {
        private readonly WebClient _client;

        public Dialogs(WebClient client) : base()
        {
            _client = client;
        }

        public bool CloseWarningDialog()
        {
            return _client.CloseWarningDialog();
        }
        public bool ConfirmationDialog(bool clickConfirmButton)
        {
            return _client.ConfirmationDialog(clickConfirmButton);
        }
        public void CloseOpportunity(bool closeAsWon)
        {
            _client.CloseOpportunity(closeAsWon);
        }
    }
}
