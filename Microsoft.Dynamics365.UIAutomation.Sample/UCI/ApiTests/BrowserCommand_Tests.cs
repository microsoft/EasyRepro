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

        private const int RetryAttempts = 5;
        readonly BrowserCommandOptions Options = new BrowserCommandOptions(Constants.DefaultTraceSource,
            "TestRetry",
            RetryAttempts, // RetryAttempts is ignored
            300, // RetryDelay ignored
            null,
            true, //ThrowExceptions
            typeof(NoSuchElementException) // exceptions to ignore
        );

        [TestMethod]
        public void CommandRetry_NotIgnoredExceptions_RetryHalfTimes()
        {
            using (var client = new WebClient(TestSettings.Options))
            {
                int i = 0;
                try
                {
                    var result = client.Execute<bool>(Options, driver =>
                    {
                        i++;
                        throw new InvalidOperationException("Do not ignore & retry");
                    });
                    Assert.IsTrue(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine($"{i} attempts");
                Assert.AreEqual(Options.RetryAttemptsNotIgnoredExceptions, i);
            }
        }
      
        [TestMethod]
        public void CommandRetry_IgnoredExceptions_RetryNTimes()
        {
            using (var client = new WebClient(TestSettings.Options))
            {
                int i = 0;
                try
                {
                    var result = client.Execute<bool>(Options, driver =>
                    {
                        i++;
                        throw new NoSuchElementException("Ignore & retry");
                    });
                    Assert.IsTrue(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine($"{i} attempts");
                Assert.AreEqual(RetryAttempts, i);
            }
        }
        
        [TestMethod]
        public void CommandRetry_NotIgnoredExceptions_DoNotRetry()
        {
            using (var client = new WebClient(TestSettings.Options))
            {
                int i = 0;
                try
                {
                    Options.RetryAttemptsNotIgnoredExceptions = 0;
                    var result = client.Execute<bool>(Options, driver =>
                    {
                        i++;
                        throw new InvalidOperationException("Do not ignore, but retry");
                    });
                    Assert.IsTrue(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine($"{i} attempts");
                Assert.AreEqual(1, i);
            }
        }


        [TestMethod]
        public void CommandRetry_DoNotRetry()
        {
            using (var client = new WebClient(TestSettings.Options))
            {
                int i = 0;
                try
                {
                    var result = client.Execute<bool>(BrowserCommandOptions.NoRetry, driver =>
                    {
                        i++;
                        throw new NoSuchElementException("Explode");
                    });
                    Assert.IsTrue(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine($"{i} attempts");
                Assert.AreEqual(1, i);
            }
        }

        [TestMethod]
        public void CommandRetry_SuccessAfter3Retry()
        {
            using (var client = new WebClient(TestSettings.Options))
            {
                int i = 0;
                try
                {
                    var result = client.Execute(Options, driver =>
                    {
                        i++;
                        if (i <= 3)
                            throw new InvalidOperationException("Do not ignore, but retry");

                        return true;
                    });
                    Assert.IsTrue(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine($"{i} attempts");
                Assert.AreEqual(3, i);
            }
        }
    }
}