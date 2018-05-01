// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class InvalidOpenEntity : TestBase
    {
        [TestInitialize]
        public override void TestSetup()
        {
            XrmTestBrowser.ThinkTime(500);
            // Invalid entity name: "Account"
            try
            {
                OpenEntity("Sales", "Account");
                Assert.Fail("Account is an invalid Entity name and hence should have failed");
            }
            catch (Exception e)
            {
                Assert.IsTrue(true, String.Format("Exception expected: {0}", e.Message));
            }
        }

        [TestMethod]
        public void WEBTestInvalidOpenEntity()
        {
            XrmTestBrowser.ThinkTime(5000);
        }
    }
}