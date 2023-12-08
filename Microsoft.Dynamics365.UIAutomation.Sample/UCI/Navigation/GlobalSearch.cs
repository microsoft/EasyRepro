// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenGlobalSearchUci : TestsBase
    {

        [TestCategory("Navigation")]
        [TestMethod]
        public void UCITestOpenGlobalSearch()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);
                xrmApp.Navigation.OpenGlobalSearch();
            }
        }
    }
}