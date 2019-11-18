// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  Test Framework methods.
    ///  </summary>
    public class TestFramework
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFramework"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public TestFramework(InteractiveBrowser browser)
            : base(browser)
        {
        }

        public BrowserCommandResult<bool> ExecuteTestSuite(Uri uri)
        {
            return this.Execute(GetOptions("Home Page"), driver =>
            {
                
                driver.Navigate().GoToUrl(uri);

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.TestFramework.ToastMessage])
                    , new TimeSpan(0, 5, 0),
                    e => { e.WaitForPlayerToLoad(); },
                    f => { throw new Exception("Toast Message Not Found - Unexpected Test Failure."); });

                return true;
            });
        }
    }
}
