// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class OnlineLogin : Element
    {
        private readonly WebClient _client;

        public OnlineLogin(WebClient client) 
        {
            _client = client;
        }

        /// <summary>
        /// Logs into the organization without providing a username and password.  This login action will use pass through authentication and automatically log you in. 
        /// </summary>
        /// <param name="orgUrl">URL of the organization</param>
        public void Login(Uri orgUrl)
        {
            LoginResult result = _client.Login(orgUrl);
            if(result == LoginResult.Failure) 
                throw new InvalidOperationException("Login Failure, please check your configuration");
            
            _client.InitializeModes();
        }

        /// <summary>
        /// Logs into the organization with the user and password provided
        /// </summary>
        /// <param name="orgUrl">URL of the organization</param>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <param name="mfaSecretKey">SecretKey for multi-factor authentication</param>
        public void Login(Uri orgUrl, SecureString username, SecureString password, SecureString mfaSecretKey = null)
        {
            LoginResult result = _client.Login(orgUrl, username, password, mfaSecretKey);
            if(result == LoginResult.Failure) 
                throw new InvalidOperationException("Login Failure, please check your configuration");
            
            _client.InitializeModes();
        }

        /// <summary>
        /// Logs into the organization with the user and password provided
        /// </summary>
        /// <param name="orgUrl">URL of the organization</param>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <param name="mfaSecretKey">SecretKey for multi-factor authentication</param>
        /// <param name="redirectAction">Actions required during redirect</param>
        public void Login(Uri orgUrl, SecureString username, SecureString password, SecureString mfaSecretKey, Action<LoginRedirectEventArgs> redirectAction)
        {
            LoginResult result = _client.Login(orgUrl, username, password, mfaSecretKey, redirectAction);
            if(result == LoginResult.Failure) 
                throw new InvalidOperationException("Login Failure, please check your configuration");
            
            _client.InitializeModes();           
        }
    }
}
