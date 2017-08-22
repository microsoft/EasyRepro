// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public static class StringExtensions
    {
        public static SecureString ToSecureString(this string @string)
        {
            var secureString = new SecureString();

            if (@string.Length > 0)
            {
                foreach (var c in @string.ToCharArray())
                    secureString.AppendChar(c);
            }

            return secureString;
        }

        public static string ToUnsecureString(this SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);

                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        public static string ToLowerString(this string value)
        {
            return value.Trim()
                        .Replace("\r", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace(Environment.NewLine, string.Empty)
                        .ToLower();
        }
        public static bool Contains(this string source, string value, StringComparison compare)
        {
            return source.IndexOf(value, compare) >= 0;
        }

    }

    public static class BoolExtensions
    {
        public static string AsString(this bool @bool, string trueCondition, string falseCondition)
        {
            return @bool ? trueCondition : falseCondition;
        }

        public static string IfFalse(this bool @bool, string falseCondition)
        {
            return AsString(@bool, string.Empty, falseCondition);
        }

        public static string IfTrue(this bool @bool, string trueCondition)
        {
            return AsString(@bool, trueCondition, string.Empty);
        }
    }
}