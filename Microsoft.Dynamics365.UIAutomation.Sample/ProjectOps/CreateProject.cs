﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.ProjectOps
{
    public static class Project
    {
        public static string Name = "msdyn_subject";
        public static string Customer = "msdyn_customer";
    }

    [TestClass]
    public class CreateProject
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

        [TestCategory("Project Operations")]
        [TestMethod]
        public void ProjectOpsTestCreateProject()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.ProjectOps);

                xrmApp.Navigation.OpenSubArea("Projects", "Projects");

                xrmApp.CommandBar.ClickCommand("New Project");

                xrmApp.Entity.SetValue(Project.Name, "Contoso Project");

                xrmApp.Entity.SetValue(new LookupItem() { Name = Project.Customer, Value = "Adventure Works"});

                xrmApp.Entity.SetValue(new DateTimeControl("msdyn_finish") { Value = DateTime.Now.AddYears(1)});

                xrmApp.Entity.Save();
                
            }
            
        }
    }
}