// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Net;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class BrowserCredentials
    {
        private BrowserCredentials()
        {

        }

        public BrowserCredentials(string username, string password)
        {
            this.Username = username.ToSecureString();
            this.Password = password.ToSecureString();
        }

        public BrowserCredentials(SecureString username, SecureString password)
        {
            this.Username = username;
            this.Password = password;
        }

        public SecureString Username { get; private set; }
        public SecureString Password { get; private set; }

        public bool IsDefault => this.Username == null && this.Password == null;

        public static BrowserCredentials Default => new BrowserCredentials();

        public static bool operator == (BrowserCredentials credentials1, BrowserCredentials credentials2)
        {
            return credentials1.Equals(credentials2);
        }

        public static bool operator != (BrowserCredentials credentials1, BrowserCredentials credentials2)
        {
            return !credentials1.Equals(credentials2);
        }

        protected bool Equals(BrowserCredentials other)
        {
            return Equals(Username, other.Username) && Equals(Password, other.Password);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == this.GetType() && Equals((BrowserCredentials)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Username?.GetHashCode() ?? 0) * 397) ^ (Password?.GetHashCode() ?? 0);
            }
        }

    }
}