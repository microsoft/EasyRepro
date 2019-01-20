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

        /// <summary>
        /// Logs into the organization with the user and password provided
        /// </summary>
        /// <param name="orgUrl">URL of the organization</param>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        public void Login(Uri orgUrl, SecureString username, SecureString password)
        {
            _client.Login(orgUrl, username, password);
        }

        /// <summary>
        /// Logs into the organization with the user and password provided
        /// </summary>
        /// <param name="orgUrl">URL of the organization</param>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <param name="redirectAction">Actions required during redirect</param>
        public void Login(Uri orgUrl, SecureString username, SecureString password, Action<LoginRedirectEventArgs> redirectAction)
        {
            _client.Login(orgUrl, username, password, redirectAction);
        }

    }
}
