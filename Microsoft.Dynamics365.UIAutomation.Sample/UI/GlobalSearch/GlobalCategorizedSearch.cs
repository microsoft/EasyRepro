// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class GlobalCategorizedSearchUci : TestsBase
    {
        [TestCategory("Global Search")]
        [TestMethod]
        public void TestGlobalCategorizedSearch()
        {
            using (var xrmApp = CreateApp())
            {
                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenGlobalSearch();

                xrmApp.GlobalSearch.Search("Fabrikam, Inc.");
                
                // This will fail unless the organization has Dataverse Search disabled
                xrmApp.GlobalSearch.FilterWith("Account");

                xrmApp.GlobalSearch.OpenRecord("Accounts", 0);
            }
        }
    }
}