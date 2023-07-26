// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.CustomerService
{
    public static class CustomerServiceCopilot
    {
        public static string controlId = "AppSidePane_MscrmControls.CSIntelligence.AICopilotControl";
        public static string controlButtonId = "sidepane-tab-button-AppSidePane_MscrmControls.CSIntelligence.AICopilotControl";
        public static string userInputId = "webchat-sendbox-input";
        public static string Name = "name";
        public static string StartTime = "starttime";
        public static string EndTime = "endtime";
        public static string Duration = "duration";
        public static string Capacity = "msdyn_effort";
        public static string Resource = "resource";
        public static string BookingStatus = "bookingstatus";
        public static string ResourceRequirement = "msdyn_resourcerequirement";
        public static string BookingType = "bookingtype";
    }

    [TestClass]
    public class EngageCopilot
    {

        private static SecureString _username;
        private static SecureString _password;
        private static SecureString _mfaSecretKey;
        private static BrowserType _browserType;
        private static Uri _xrmUri;
        private static string _browserVersion = "";
        private static string _driversPath = "";
        private static string _azureKey = "";
        private static string _sessionId = "";
        public TestContext TestContext { get; set; }

        private static TestContext _testContext;

        [ClassInitialize]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;

            _username = _testContext.Properties["OnlineUsername"].ToString().ToSecureString();
            _password = _testContext.Properties["OnlinePassword"].ToString().ToSecureString();
            _mfaSecretKey = _testContext.Properties["OnlinePassword"].ToString().ToSecureString();
            _xrmUri = new Uri(_testContext.Properties["OnlineCrmUrl"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
            _azureKey = _testContext.Properties["AzureKey"].ToString();
            _sessionId = _testContext.Properties["SessionId"].ToString() ?? Guid.NewGuid().ToString();
            _driversPath = _testContext.Properties["DriversPath"].ToString();
            if (!String.IsNullOrEmpty(_driversPath))
            {
                TestSettings.SharedOptions.DriversPath = _driversPath;
                TestSettings.Options.DriversPath = _driversPath;
            }
        }

        [TestCategory("Customer Service Workspace")]
        [TestCategory("Copilot")]
        [TestMethod]
        public void ProjectOpsTestCreateBookableResource()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                xrmApp.Navigation.OpenSubArea("Services", "Cases");

                xrmApp.Grid.SwitchView("Active Cases");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Copilot.AskAQuestion("Help summarize this case");

            }

        }
    }
}