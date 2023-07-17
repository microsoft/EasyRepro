// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class GlobalCategorizedSearchUci : TestsBase
    {
        [TestMethod]
        public void UCITestGlobalCategorizedSearch()
        {
            using (var xrmApp = CreateApp())
            {
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenGlobalSearch();

                xrmApp.GlobalSearch.Search("Fabrikam, Inc.");
                
                // This will fail unless the organization has Dataverse Search disabled
                xrmApp.GlobalSearch.FilterWith("Account");

                xrmApp.GlobalSearch.OpenRecord("Accounts", 0);
            }
        }
    }
}