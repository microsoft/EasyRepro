// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.ProjectOps
{
    public static class ProjectContract
    {
        public static string Customer = "customerid";
        public static string Name = "name";
        public static string Opportunity = "opportunityid";
        public static string ProductPriceList = "pricelevelid";
    }

    public static class ProjectContractLine
    {
        public static string BillingMethod = "msdyn_billingmethod";
        public static string Name = "productdescription";
        public static string Project = "msdyn_project";
        public static string ContractedAmount = "priceperunit";
    }

    public static class ProductContractLine
    {
        public static string ProductType = "producttypecode";
        public static string DeliveryDate = "requesteddeliveryby";
        public static string Product = "productid";
        public static string Quantity = "quantity";
        public static string Unit = "uomid";
        public static string SalesPrice = "priceperunit";
    }

    [TestClass]
    public class CreateProjectContract
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
        public void ProjectOpsTestCreateProjectContract()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.ProjectOps);

                xrmApp.Navigation.OpenSubArea("Sales", "Project Contracts");

                xrmApp.CommandBar.ClickCommand("New");

                xrmApp.Entity.SetValue(ProjectContract.Name, "UI Automated Order");

                xrmApp.Entity.SetValue(new LookupItem() { Name = ProjectContract.Customer, Value = "Adventure Works" });

                xrmApp.Entity.SetValue(new LookupItem() { Name = ProjectContract.ProductPriceList, Value = "Products and Packaged Services" });

                xrmApp.Entity.Save();

            }

        }

        [TestCategory("Project Operations")]
        [TestMethod]
        public void ProjectOpsTestCreateProjectContractProjectBasedLine()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.ProjectOps);

                xrmApp.Navigation.OpenSubArea("Sales", "Project Contracts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Entity.SelectTab("Contract Lines");

                xrmApp.Entity.SubGrid.ClickCommand("ProjectContractLines", "Add New Contract Line");

                xrmApp.Entity.SetValue(ProjectContractLine.Name, "UI Automated Order");

                xrmApp.Entity.SetValue(new OptionSet() { Name = ProjectContractLine.BillingMethod, Value = "Time and Material" });

                xrmApp.Entity.SetValue(new LookupItem() { Name = ProjectContractLine.Project, Value = "Contoso Project" });

                xrmApp.Entity.SetValue(ProjectContractLine.ContractedAmount, "100");

                xrmApp.Entity.Save();

            }

        }

        [TestCategory("Project Operations")]
        [TestMethod]
        public void ProjectOpsTestCreateProjectContractProductBasedLine()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.ProjectOps);

                xrmApp.Navigation.OpenSubArea("Sales", "Project Contracts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Entity.SelectTab("Contract Lines");

                xrmApp.Entity.SubGrid.ClickCommand("salesorderdetailsGrid", "Add New Contract Line");

                xrmApp.Entity.SetValue(new OptionSet() { Name = ProductContractLine.ProductType, Value = "Product" });

                xrmApp.Entity.SetValue(ProductContractLine.Quantity, "100");

                xrmApp.Entity.SetValue(new LookupItem() { Name = ProductContractLine.Product, Value = "ArmBand 100" });



                xrmApp.Entity.Save();

            }

        }
    }
}