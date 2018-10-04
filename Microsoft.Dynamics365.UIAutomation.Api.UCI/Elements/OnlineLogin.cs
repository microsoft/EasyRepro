// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class OnlineLogin : Element
    {
        private WebClient _client;

        public OnlineLogin(WebClient client) 
        {
            _client = client;
        }

        public void Login(Uri orgUrl, SecureString username, SecureString password)
        {
            _client.Login(orgUrl, username, password);
        }

        public void Login(Uri orgUrl, SecureString username, SecureString password, Action<LoginRedirectEventArgs> redirectAction)
        {
            _client.Login(orgUrl, username, password, redirectAction);
        }

    }
}
