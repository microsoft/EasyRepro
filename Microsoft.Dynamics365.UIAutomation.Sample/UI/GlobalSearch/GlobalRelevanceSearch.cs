// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;

/*
 * This requires Relevance Search to be enabled.  To enable Relevance Search review the following article:
 * https://docs.microsoft.com/en-us/power-platform/admin/configure-relevance-search-organization#enable-relevance-search
 */

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class GlobalRelevanceSearchUci : TestsBase
    {
        [TestCategory("Global Search")]
        [TestMethod]
        public void TestGlobalRelevanceSearch()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(AppName.Sales);

                xrmApp.Navigation.OpenGlobalSearch();

                xrmApp.GlobalSearch.Search("Northwind Traders");

                xrmApp.GlobalSearch.Filter("Record Type", "Accounts");

                xrmApp.ThinkTime(4000);

                xrmApp.GlobalSearch.OpenRecord("Accounts", 0);
            }
        }
    }
}