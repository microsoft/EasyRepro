// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.ProjectOps
{
    public static class TeamMember
    {
        public static string PositionName = "msdyn_name";
        public static string BookableResource = "msdyn_bookableresourceid";
        public static string Role = "msdyn_resourcecategory";
        public static string Start = "msdyn_start";
        public static string Finish = "msdyn_finish";
        public static string AllocationMethod = "msdyn_allocationmethod";
    }

    [TestClass]
    public class CreateProjectTeamMember
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
        public void ProjectOpsTestAddProjectTeamMember()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                //xrmApp.Navigation.OpenApp(UCIAppName.ProjectOps);

                xrmApp.Navigation.OpenSubArea("Projects", "Projects");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Entity.SelectTab("Team");

                xrmApp.Entity.SubGrid.ClickCommand("SubGrid_TeamMember","New");

                xrmApp.QuickCreate.SetValue(TeamMember.PositionName, "UI Tester");

                xrmApp.QuickCreate.SetValue(new LookupItem() { Name = TeamMember.BookableResource, Value = "Abraham McCormick" });

                xrmApp.QuickCreate.SetValue(new LookupItem() { Name = TeamMember.Role, Value = "Optimization Specialist" });

                xrmApp.QuickCreate.SetValue(TeamMember.Finish,DateTime.Now.AddYears(1));

                xrmApp.QuickCreate.SetValue(TeamMember.Start, DateTime.Now);

                xrmApp.QuickCreate.Save();
                
            }
            
        }
    }
}