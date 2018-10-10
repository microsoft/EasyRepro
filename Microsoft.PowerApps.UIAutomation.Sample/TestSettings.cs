﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    public class TestSettings
    {

        public TestContext TestContext { get; set; }

        private static TestContext _testContext;

        private static BrowserType Type;

        [ClassInitialize]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;
            Type = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
        }

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
