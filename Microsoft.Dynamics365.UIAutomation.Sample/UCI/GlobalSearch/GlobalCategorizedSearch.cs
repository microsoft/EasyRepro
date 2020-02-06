// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class GlobalCategorizedSearchUci : TestsBase
    {
        [TestMethod]
        public void UCITestGlobalCategorizedSearch()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecrectKey);

                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenGlobalSearch();

                xrmApp.GlobalSearch.ChangeSearchType("Categorized Search");

                xrmApp.GlobalSearch.Search("Test");

                xrmApp.GlobalSearch.FilterWith("Account");

                 xrmApp.GlobalSearch.OpenRecord("account", 0);
            }
        }
    }
}