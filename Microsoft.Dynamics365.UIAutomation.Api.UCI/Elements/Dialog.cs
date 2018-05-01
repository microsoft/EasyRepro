// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Dialog : Element
    {
        private readonly WebClient _client;

        public Dialog(WebClient client) : base()
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
    }
}
