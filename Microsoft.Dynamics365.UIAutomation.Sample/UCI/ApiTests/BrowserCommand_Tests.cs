// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI.ApiTests
{
    [TestClass]
    public class BrowserCommand_Tests
    {
        // Current Logic
        // if is in exceptions list, retry N attemps & throws if desired
        // if not in exceptions list, throws if desired
        // if  
        [TestMethod]
        public void CommandRetry()
        {
            using (var client = new WebClient(TestSettings.Options))
            {
                var browserOpts = new BrowserCommandOptions(Constants.DefaultTraceSource,
                    "My Custom Set Lookup",
                    10, // RetryAttempts is ignored
                    300, // RetryDelay ignored
                    null,
                    true, //ThrowExceptions
                    typeof(NoSuchElementException) // exeptions to ignore
                    );

                int i = 0;
                try
                {
                    var result = client.Execute(browserOpts, driver =>
                    {
                        i++;
                        if (i <= 3)
                            throw new InvalidOperationException("Do not ignore & retry");

                        return true;
                    });
                    Assert.IsTrue(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine($"{i} attempts");
                Assert.IsTrue(i >= 10);
            }
        }
    }
}