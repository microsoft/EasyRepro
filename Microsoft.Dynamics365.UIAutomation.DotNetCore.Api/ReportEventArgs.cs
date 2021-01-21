// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class ReportEventArgs : EventArgs
    {
        protected internal ReportEventArgs( IWebDriver driver)
        {
            this.Driver = driver;
        }
        public IWebDriver Driver { get; private set; }
    }
}