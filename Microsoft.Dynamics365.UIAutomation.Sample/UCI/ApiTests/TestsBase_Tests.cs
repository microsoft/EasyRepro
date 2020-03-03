// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI.ApiTests
{
    [TestClass]
    public class TestsBase_TestInitializeFail : TestsBase
    {
        public override void NavigateToHomePage()
        {
            // Assert that CreateApp was success 
            Assert.IsNotNull(_client);
            Assert.IsNotNull(_xrmApp);

            throw new Exception("Simulating Navigation Fail => Browser should be closed & the exception rethrow");
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void NavigationFail_ExceptionIsRethrow()
        {
            // Simulate => [TestInitialize]
            base.InitTest();

            Assert.Fail("If Exception is rethrow. This code should never get executed");

            // Simulate => [TestCleanup]
            // base.FinishTest();
        }

        [TestMethod]
        public void NavigationFail_BrowserIsClosed()
        {
            try
            {
                // Simulate => [TestInitialize]
                base.InitTest();

                Assert.Fail("If Exception is rethrow. This code should never get executed");
            }
            catch 
            {
                // Assert that CloseApp was called
                Assert.IsNull(_xrmApp, "Browser still open");
                Assert.IsNull(_client, "Browser still open");
            }
        }
    }
}