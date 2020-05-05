// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

/*
 * This requires Relevance Search to be enabled.  To enable Relevance Search review the following article:
 * https://docs.microsoft.com/en-us/power-platform/admin/configure-relevance-search-organization#enable-relevance-search
 */

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class GlobalRelevanceSearchUci: TestsBase
    {
        [TestMethod]
        public void UCITestGlobalRelevanceSearch()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenGlobalSearch();

                xrmApp.GlobalSearch.ChangeSearchType("Relevance Search");

                xrmApp.GlobalSearch.Search("Adventure");

                xrmApp.GlobalSearch.Filter("Record Type", "Accounts");

                xrmApp.GlobalSearch.OpenRecord("account", 0);
            }
        }
    }
}