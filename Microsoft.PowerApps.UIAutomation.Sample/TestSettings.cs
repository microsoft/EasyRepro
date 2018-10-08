// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Configuration;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    public class TestSettings
    {

        private static string Type = ConfigurationManager.AppSettings["BrowserType"].ToString();

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
