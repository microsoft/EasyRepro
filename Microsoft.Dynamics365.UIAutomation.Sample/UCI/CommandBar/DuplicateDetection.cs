// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class DuplicateDetection : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => NavigateTo(UCIAppName.Sales, "Sales", "Contacts");
        [TestMethod]
        public void UCITestDuplicateDetection()
        {
            _xrmApp.CommandBar.ClickCommand("New");

            _xrmApp.Entity.SetValue("firstname", "EasyRepro");
            _xrmApp.Entity.SetValue("lastname", "Duplicate");
            _xrmApp.Entity.SetValue("emailaddress1", "jz3@jztest.com");

            _xrmApp.Entity.Save();
        }
    }
}
