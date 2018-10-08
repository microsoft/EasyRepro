// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    public static class TestSettings
    {
        private static readonly string Type = System.Configuration.ConfigurationManager.AppSettings["BrowserType"].ToString();

        public static BrowserOptions Options = new BrowserOptions
        {
            BrowserType = (BrowserType)Enum.Parse(typeof(BrowserType), Type.ToString()),
            PrivateMode = true,
            FireEvents = false,
            Headless = false,
            UserAgent = false
        };
    }
}
