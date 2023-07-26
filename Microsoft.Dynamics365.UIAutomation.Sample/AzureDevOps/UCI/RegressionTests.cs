// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Linq;
using System.Security;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class RegressionTests
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
            _mfaSecretKey = _testContext.Properties["MfaSecretKey"].ToString().ToSecureString();
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

        #region Helper Methods
        private string TakeScreenshot(WebClient client, ICommandResult command)
        {
            ScreenshotImageFormat fileFormat = ScreenshotImageFormat.Bmp;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
            string strFileName = String.Format("{2}_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat, command.CommandName.Replace(":",""));
            client.Browser.TakeWindowScreenShot(strFileName, fileFormat);
            TestContext.AddResultFile(strFileName);
            return strFileName;
        }
        private string WriteSource(string fileNamePrefix, string source)
        {
            string fileName = fileNamePrefix + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".html";
            Trace.WriteLine("Creating Page Source file with name " + fileName);
            System.IO.File.AppendAllText(fileName, source);
            TestContext.AddResultFile(fileName);
            return fileName;
        }
        #endregion
        [TestCategory("Regression")]
        [TestCategory("2021ReleaseWave2")]
        [TestCategory("Grid")]
        [TestMethod]
        public void Regression_Grid_SwitchView_GetItems_Sort_SearchRecord_HighlightRecord_OpenRecord()
        {
            Trace.TraceInformation("Starting test " + TestContext.TestName);
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient { InstrumentationKey = _azureKey };
            telemetry.Context.Operation.Id = Guid.NewGuid().ToString();
            telemetry.Context.Operation.ParentId = _sessionId;
            telemetry.Context.GlobalProperties.Add("Test", TestContext.TestName);
            
            var client = new WebClient(TestSettings.Options);
            var xrmApp = new XrmApp(client);
            try
            {
                telemetry.TrackTrace("Login Started");
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                telemetry.TrackTrace("Login Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                //xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                telemetry.TrackTrace("OpenAbout Started");
                xrmApp.Navigation.OpenAbout();
                telemetry.TrackTrace("OpenAbout Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                xrmApp.Dialogs.ClickOk();

                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");
                telemetry.TrackTrace("OpenSubArea Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("SwitchView Started");
                xrmApp.Grid.SwitchView("Active Accounts");
                telemetry.TrackTrace("SwitchView Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("GetGridControl Started");
                var gridHtml = xrmApp.Grid.GetGridControl();
                WriteSource("GRID_", gridHtml);
                telemetry.TrackTrace("GetGridControl Completed");

                telemetry.TrackTrace("GetGridItems Started");
                var getGridItems = xrmApp.Grid.GetGridItems();
                telemetry.TrackTrace("GetGridItems Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                Assert.IsTrue(getGridItems.Count > 0, "GetGridItems returned 0 records.");
                Assert.IsTrue(getGridItems[0].Id != Guid.Empty, "GetGridItems returned a record but the ID is empty.");

                telemetry.TrackTrace("Sort Started");
                xrmApp.Grid.Sort("Main Phone", "Sort A to Z");
                telemetry.TrackTrace("Sort Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("Search Started");
                xrmApp.Grid.Search("Contoso");
                telemetry.TrackTrace("Search Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("HighLightRecord Started");
                xrmApp.Grid.HighLightRecord(0);
                telemetry.TrackTrace("HighLightRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(0);
                telemetry.TrackTrace("OpenRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackEvent(String.Format("{0} is successful.", TestContext.TestName));
            }
            catch (System.Exception ex)
            {
                telemetry.TrackException(ex);
                WriteSource("EXCEPTION_Source_", client.Browser.Driver.PageSource);
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                throw ex;
            }
            finally
            {
                xrmApp.Dispose();
                telemetry.Flush();
            }
        }

        [TestCategory("Regression")]
        [TestCategory("2021ReleaseWave2")]
        [TestCategory("Grid")]
        [TestMethod]
        public void Regression_Grid_SelectFirstCell()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient { InstrumentationKey = _azureKey };
            telemetry.Context.Operation.Id = Guid.NewGuid().ToString();
            telemetry.Context.Operation.ParentId = _sessionId;
            telemetry.Context.GlobalProperties.Add("Test", TestContext.TestName);
            var client = new WebClient(TestSettings.Options);
            var xrmApp = new XrmApp(client);
            try
            {
                telemetry.TrackTrace("Login Started");
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                telemetry.TrackTrace("Login Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                //xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                telemetry.TrackTrace("OpenAbout Started");
                xrmApp.Navigation.OpenAbout();
                telemetry.TrackTrace("OpenAbout Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                xrmApp.Dialogs.ClickOk();

                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");
                telemetry.TrackTrace("OpenSubArea Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("SwitchView Started");
                xrmApp.Grid.SwitchView("My Active Accounts");
                telemetry.TrackTrace("SwitchView Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("GetGridControl Started");
                var gridHtml = xrmApp.Grid.GetGridControl();
                WriteSource("GRID_", gridHtml);
                telemetry.TrackTrace("GetGridControl Completed");

                telemetry.TrackTrace("Search Started");
                xrmApp.Grid.Search("Contoso");
                telemetry.TrackTrace("Search Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(0);
                telemetry.TrackTrace("OpenRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                #region Boolean
                telemetry.TrackTrace("GetValue-Text Started");
                xrmApp.Entity.GetValue("telephone1");
                telemetry.TrackTrace("GetValue-Text Completed");

                telemetry.TrackTrace("SetValue-Text Started");
                xrmApp.Entity.SetValue("telephone1", "867-5309");
                telemetry.TrackTrace("SetValue-Text Completed");
                #endregion
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                //#region Lookup
                //telemetry.TrackTrace("SetValue-LookupItem Started");
                //xrmApp.Entity.SetValue(new LookupItem { Name = "customerid", Value = "maria campbell", Index = 0 });

                //telemetry.TrackTrace("SetValue-LookupItem Completed");
                //#endregion


                telemetry.TrackEvent(String.Format("{0} is successful.", TestContext.TestName));
            }
            catch (System.Exception ex)
            {
                telemetry.TrackException(ex);
                WriteSource("EXCEPTION_Source_", client.Browser.Driver.PageSource);
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                throw ex;
            }
            finally
            {
                xrmApp.Dispose();
                telemetry.Flush();
            }
        }

        [TestCategory("Regression")]
        [TestCategory("2021ReleaseWave2")]
        [TestCategory("Controls")]
        [TestMethod]
        public void Regression_EntityControls_GetValue_SetValue()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient { InstrumentationKey = _azureKey };
            telemetry.Context.Operation.Id = Guid.NewGuid().ToString();
            telemetry.Context.Operation.ParentId = _sessionId;
            telemetry.Context.GlobalProperties.Add("Test", TestContext.TestName);
            var client = new WebClient(TestSettings.Options);
            var xrmApp = new XrmApp(client);
            try
            {
                telemetry.TrackTrace("Login Started");
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                telemetry.TrackTrace("Login Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                //xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                telemetry.TrackTrace("OpenAbout Started");
                xrmApp.Navigation.OpenAbout();
                telemetry.TrackTrace("OpenAbout Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                xrmApp.Dialogs.ClickOk();

                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");
                telemetry.TrackTrace("OpenSubArea Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("SwitchView Started");
                xrmApp.Grid.SwitchView("All Accounts");
                telemetry.TrackTrace("SwitchView Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("GetGridControl Started");
                var gridHtml = xrmApp.Grid.GetGridControl();
                WriteSource("GRID_", gridHtml);
                telemetry.TrackTrace("GetGridControl Completed");

                telemetry.TrackTrace("Search Started");
                xrmApp.Grid.Search("Contoso");
                telemetry.TrackTrace("Search Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(0);
                telemetry.TrackTrace("OpenRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                #region Boolean
                telemetry.TrackTrace("GetValue-Text Started");
                xrmApp.Entity.GetValue("telephone1");
                telemetry.TrackTrace("GetValue-Text Completed");

                telemetry.TrackTrace("SetValue-Text Started");
                xrmApp.Entity.SetValue("telephone1","867-5309");
                telemetry.TrackTrace("SetValue-Text Completed");
                #endregion
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                //#region Lookup
                //telemetry.TrackTrace("SetValue-LookupItem Started");
                //xrmApp.Entity.SetValue(new LookupItem { Name = "customerid", Value = "maria campbell", Index = 0 });

                //telemetry.TrackTrace("SetValue-LookupItem Completed");
                //#endregion


                telemetry.TrackEvent(String.Format("{0} is successful.", TestContext.TestName));
            }
            catch (System.Exception ex)
            {
                telemetry.TrackException(ex);
                WriteSource("EXCEPTION_Source_", client.Browser.Driver.PageSource);
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                throw ex;
            }
            finally
            {
                xrmApp.Dispose();
                telemetry.Flush();
            }
        }

        [TestCategory("Regression")]
        [TestCategory("2021ReleaseWave2")]
        [TestCategory("SubGrid")]
        [TestMethod]
        public void Regression_SubGrid_GetSubGridItems_OpenRecord()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient { InstrumentationKey = _azureKey };
            telemetry.Context.Operation.Id = Guid.NewGuid().ToString();
            telemetry.Context.Operation.ParentId = _sessionId;
            telemetry.Context.GlobalProperties.Add("Test", TestContext.TestName);
            var client = new WebClient(TestSettings.Options);
            var xrmApp = new XrmApp(client);
            try
            {
                telemetry.TrackTrace("Login Started");
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                telemetry.TrackTrace("Login Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                //telemetry.TrackTrace("OpenAbout Started");
                //xrmApp.Navigation.OpenAbout();
                //telemetry.TrackTrace("OpenAbout Completed");
                //TakeScreenshot(client, xrmApp.CommandResults.Last());
                //xrmApp.Dialogs.ClickOk();

                telemetry.TrackTrace("Begin Testing Modern Read Only Grid");
                Trace.WriteLine("Begin Testing Modern Read Only Grid");
                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");
                telemetry.TrackTrace("OpenSubArea Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("SwitchView Started");
                xrmApp.Grid.SwitchView("Active Contacts");
                telemetry.TrackTrace("SwitchView Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(0);
                telemetry.TrackTrace("OpenRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                telemetry.TrackTrace("End Testing Modern Read Only Grid");
                Trace.WriteLine("End Testing Modern Read Only Grid");


                telemetry.TrackTrace("GetGridControl Started");
                var subGridHtml = xrmApp.Entity.SubGrid.GetSubGridControl("contactopportunitiesgrid");
                WriteSource("SUBGRID_", subGridHtml);
                telemetry.TrackTrace("GetGridControl Completed");

                telemetry.TrackTrace("GetSubGridItems Started");
                var getSubGridItems = xrmApp.Entity.SubGrid.GetSubGridItems("contactopportunitiesgrid");
                telemetry.TrackTrace("GetSubGridItems Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                Assert.IsTrue(getSubGridItems.Count > 0, "GetSubGridItems returned 0 records.");

                xrmApp.Entity.SubGrid.OpenSubGridRecord("contactopportunitiesgrid", 0);


                telemetry.TrackTrace("Begin Testing Modern Read Only Grid Scrolling");
                Trace.WriteLine("Begin Testing Modern Read Only Grid Scrolling");
                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");
                telemetry.TrackTrace("OpenSubArea Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("SwitchView Started");
                xrmApp.Grid.SwitchView("All Accounts");
                telemetry.TrackTrace("SwitchView Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(100);
                telemetry.TrackTrace("OpenRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                telemetry.TrackTrace("End Testing Modern Read Only Grid Scrolling");
                Trace.WriteLine("End Testing Modern Read Only Grid Scrolling");

                telemetry.TrackTrace("Begin Testing Power Apps Grid Scrolling");
                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");
                telemetry.TrackTrace("OpenSubArea Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("SwitchView Started");
                xrmApp.Grid.SwitchView("Active Contacts");
                telemetry.TrackTrace("SwitchView Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(100);
                telemetry.TrackTrace("OpenRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                telemetry.TrackTrace("End Testing Power Apps Grid Scrolling");

                telemetry.TrackTrace("Begin Testing Power Apps Grid");
                Trace.WriteLine("Begin Testing Power Apps Grid");
                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");
                telemetry.TrackTrace("OpenSubArea Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("SwitchView Started");
                xrmApp.Grid.SwitchView("Active Contacts");
                telemetry.TrackTrace("SwitchView Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(0);
                telemetry.TrackTrace("OpenRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                telemetry.TrackTrace("End Testing Power Apps Grid");
                Trace.WriteLine("End Testing Power Apps Grid");

                telemetry.TrackTrace("Testing Paging on Power Apps Grid");
                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("Sales", "Contacts");
                telemetry.TrackTrace("OpenSubArea Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("SwitchView Started");
                xrmApp.Grid.SwitchView("Active Contacts");
                telemetry.TrackTrace("SwitchView Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(100);
                telemetry.TrackTrace("OpenRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());



                telemetry.TrackTrace("OpenSubGridRecord Started");
                xrmApp.Entity.SubGrid.OpenSubGridRecord("Contacts", 30);
                telemetry.TrackTrace("OpenSubGridRecord Started");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackEvent(String.Format("{0} is successful.", TestContext.TestName));
            }
            catch (System.Exception ex)
            {
                telemetry.TrackException(ex);
                WriteSource("EXCEPTION_Source_",client.Browser.Driver.PageSource);
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                throw ex;
            }
            finally
            {
                xrmApp.Dispose();
                telemetry.Flush();
            }
        }
        [TestCategory("Regression")]
        [TestCategory("Controls")]
        [TestMethod]
        public void Regression_EntityControls_GetValue_SetValue_All()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient { InstrumentationKey = _azureKey };
            telemetry.Context.Operation.Id = Guid.NewGuid().ToString();
            telemetry.Context.Operation.ParentId = _sessionId;
            telemetry.Context.GlobalProperties.Add("Test", TestContext.TestName);
 
            var client = new WebClient(TestSettings.Options);
            var xrmApp = new XrmApp(client);
            try
            {
                telemetry.TrackTrace("Login Started");
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                telemetry.TrackTrace("Login Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                //TODO
                //telemetry.TrackTrace("OpenAbout Started");
                //xrmApp.Navigation.OpenAbout();
                //telemetry.TrackTrace("OpenAbout Completed");
                //TakeScreenshot(client, xrmApp.CommandResults.Last());
                //xrmApp.Dialogs.ClickOk();

                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("App Insights", "Test Harnesses");
                telemetry.TrackTrace("OpenSubArea Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(0);
                telemetry.TrackTrace("OpenRecord Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                #region Rich Text Editor
                try
                {
                    //Work with Text Area
                    Trace.WriteLine("SetValue pfe_multilinetext1");
                    xrmApp.Entity.SetValue("pfe_multilinetext1", "test text area");
                    string textAreaReturn = xrmApp.Entity.GetValue("pfe_multilinetext1");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_multilinetext1";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Attributes
                #region Auto Number
                //Work with AutoNumber
                Trace.WriteLine("SetValue pfe_autonumber");
                xrmApp.Entity.SetValue("pfe_autonumber", TestSettings.GetRandomString(5, 10));
                #endregion
                #region Currency
                //Work with Currency
                Trace.WriteLine("SetValue pfe_currency");
                xrmApp.Entity.SetValue("pfe_currency", "1.00");
                #endregion
                #region Customer
                //Work with Lookup
                //Trace.WriteLine("SetValue pfe_customer");
                //LookupItem pfe_customer = new LookupItem() { Name = "pfe_customer", Value = "Adventure Works (Sample)", Index = 0 };
                //xrmApp.Entity.SetValue(pfe_customer);
                #endregion
                #region DateTime
                try
                {
                    //Work with DateTime
                    Trace.WriteLine("SetValue pfe_dateandtime");
                    DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                    xrmApp.Entity.SetValue("pfe_dateandtime", expectedDate);
                    DateTime? date = xrmApp.Entity.GetValue(new DateTimeControl("pfe_dateandtime"));
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_dateandtime";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw;
                }
                try
                {
                    //Work with DateOnly
                    Trace.WriteLine("SetValue pfe_dateonly");
                    DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                    xrmApp.Entity.SetValue("pfe_dateonly", expectedDate);
                    DateTime? date = xrmApp.Entity.GetValue(new DateTimeControl("pfe_dateonly"));
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_dateonly";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Decimal
                try
                {
                    //Work with Decimal
                    Trace.WriteLine("SetValue pfe_decimalnumber");
                    xrmApp.Entity.SetValue("pfe_decimalnumber", ".01");
                    string decimalReturn = xrmApp.Entity.GetValue("pfe_decimalnumber");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_decimalnumber";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Duration
                try
                {
                    //Work with Duration
                    Trace.WriteLine("SetValue pfe_duration");
                    xrmApp.Entity.SetValue("pfe_duration", "1 hour");
                    string decimalReturn = xrmApp.Entity.GetValue("pfe_duration");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_duration";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Email
                try
                {
                    //Work with Email
                    Trace.WriteLine("SetValue pfe_email");
                    xrmApp.Entity.SetValue("pfe_email", "easyreprosupport@contoso.com");
                    string emailReturn = xrmApp.Entity.GetValue("pfe_email");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_email";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region File - TODO
                try
                {
                    //Work with File
                    Trace.WriteLine("SetValue pfe_file");
                    xrmApp.Entity.SetValue("pfe_file", "easyreprosupport@contoso.com");
                    string fileReturn = xrmApp.Entity.GetValue("pfe_file");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_file";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Floating Point Number
                try
                {
                    //Work with Floating Point Number
                    Trace.WriteLine("SetValue pfe_floatingpointnumber");
                    xrmApp.Entity.SetValue("pfe_floatingpointnumber", "100.001");
                    string pfe_floatingpointnumberReturn = xrmApp.Entity.GetValue("pfe_floatingpointnumber");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_floatingpointnumber";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Image - TODO
                try
                {
                    //Work with Image
                    Trace.WriteLine("SetValue pfe_image");
                    xrmApp.Entity.SetValue("pfe_image", "test.jpg");
                    string imageReturn = xrmApp.Entity.GetValue("pfe_image");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_image";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Language - TODO
                try
                {
                    //Work with Language
                    Trace.WriteLine("SetValue pfe_language");
                    xrmApp.Entity.SetValue("pfe_language", "English (United States)");
                    string languageReturn = xrmApp.Entity.GetValue("pfe_language");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_language";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Lookup
                try
                {
                    //Work with Lookup
                    Trace.WriteLine("SetValue pfe_lookup");
                    LookupItem pfe_customer = new LookupItem() { Name = "pfe_lookup", Value = "Adventure Works (Sample)", Index = 0 };
                    xrmApp.Entity.SetValue(pfe_customer);
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_lookup";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Multi Line Textbox
                try
                {
                    //Work with Multi Line Textbox
                    Trace.WriteLine("SetValue pfe_multilinetext");
                    xrmApp.Entity.SetValue("pfe_multilinetext", "Spanish");
                    string multiLineTextReturn = xrmApp.Entity.GetValue("pfe_multilinetext");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_multilinetext";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Multi Select Option Set
                try
                {
                    //Work with Multi Select Option Set
                    Trace.WriteLine("SetValue pfe_multiselectoptionset");
                    xrmApp.Entity.SetValue(new MultiValueOptionSet { Name = "pfe_multiselectoptionset", Values = new string[] { "Allowed", "Not Allowed" }  });
                    //xrmApp.Entity.SetValue("pfe_multiselectoptionset", "Allowed");
                    //xrmApp.Entity.SetValue("pfe_multiselectoptionset", "Not Allowed");
                    string multiselectOptionSetReturn = xrmApp.Entity.GetValue("pfe_multiselectoptionset");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_multiselectoptionset";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Text
                try
                {
                    //Work with Text
                    Trace.WriteLine("SetValue pfe_name");
                    xrmApp.Entity.SetValue("pfe_name", TestSettings.GetRandomString(5, 10));
                    string textReturn = xrmApp.Entity.GetValue("pfe_name");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_name";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Option Set - TODO
                try
                {
                    //Work with Option Set
                    Trace.WriteLine("SetValue pfe_optionset");
                    xrmApp.Entity.SetValue(new OptionSet { Name = "pfe_optionset", Value = "Allowed" });
                    string textReturn = xrmApp.Entity.GetValue(new OptionSet() { Name = "pfe_optionset" });
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_optionset";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Text Area
                try
                {
                    //Work with Text Area
                    Trace.WriteLine("SetValue pfe_textarea");
                    xrmApp.Entity.SetValue("pfe_textarea", "test text area");
                    string textAreaReturn = xrmApp.Entity.GetValue("pfe_textarea");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_textarea";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion

                #region Ticker Symbol
                try
                {
                    //Work with tickersymbol
                    Trace.WriteLine("SetValue pfe_tickersymbol");
                    xrmApp.Entity.SetValue("pfe_tickersymbol", "MSFT");
                    string tickersymbolReturn = xrmApp.Entity.GetValue("pfe_tickersymbol");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_tickersymbol";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Time Zone - TODO
                try
                {
                    //Work with pfe_timezone
                    Trace.WriteLine("SetValue pfe_timezone");
                    xrmApp.Entity.SetValue("pfe_timezone", "(GMT-06:00) Central Time (US & Canada)");
                    string pfe_timezoneReturn = xrmApp.Entity.GetValue("pfe_timezone");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_timezone";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Two Options
                try
                {
                    //Work with pfe_twooptions
                    Trace.WriteLine("SetValue pfe_twooptions");
                    xrmApp.Entity.SetValue(new BooleanItem() { Name = "pfe_twooptions", Value = true });
                    string pfe_timezoneReturn = xrmApp.Entity.GetValue("pfe_twooptions");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_twooptions";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Url
                try
                {
                    //Work with pfe_url
                    Trace.WriteLine("SetValue pfe_url");
                    xrmApp.Entity.SetValue("pfe_url", "https://microsoft.com");
                    string pfe_urlReturn = xrmApp.Entity.GetValue("pfe_url");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_url";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Whole Number
                try
                {
                    //Work with pfe_wholenumber
                    Trace.WriteLine("SetValue pfe_wholenumber");
                    xrmApp.Entity.SetValue("pfe_wholenumber", "10000");
                    string pfe_wholenumberReturn = xrmApp.Entity.GetValue("pfe_wholenumber");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_wholenumber";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #endregion
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                telemetry.TrackEvent(String.Format("{0} is successful.", TestContext.TestName));
            }
            catch (System.Exception ex)
            {
                telemetry.TrackException(ex);
                WriteSource("EXCEPTION_Source_", client.Browser.Driver.PageSource);
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                throw ex;
            }
            finally
            {
                xrmApp.Dispose();
                telemetry.Flush();
            }
        }

        [TestCategory("Regression")]
        [TestCategory("Controls")]
        [TestCategory("QuickCreate")]
        [TestMethod]
        public void Regression_QuickCreateControls_GetValue_SetValue_All()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient { InstrumentationKey = _azureKey };
            telemetry.Context.Operation.Id = Guid.NewGuid().ToString();
            telemetry.Context.Operation.ParentId = _sessionId;
            telemetry.Context.GlobalProperties.Add("Test", TestContext.TestName);
            Dictionary<string, string> localProperties = new Dictionary<string, string>();
            var client = new WebClient(TestSettings.Options);
            var xrmApp = new XrmApp(client);
            try
            {
                telemetry.TrackTrace("Login Started");
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                telemetry.TrackTrace("Login Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                //TODO
                //telemetry.TrackTrace("OpenAbout Started");
                //xrmApp.Navigation.OpenAbout();
                //telemetry.TrackTrace("OpenAbout Completed");
                //TakeScreenshot(client, xrmApp.CommandResults.Last());
                //xrmApp.Dialogs.ClickOk();

                telemetry.TrackTrace("Navigation.QuickCreate Started");
                xrmApp.Navigation.QuickCreate("Test Harness");
                telemetry.TrackTrace("Navigation.QuickCreate Completed");
                TakeScreenshot(client, xrmApp.CommandResults.Last());

                #region Attributes
                #region Auto Number
                //Work with AutoNumber
                Trace.WriteLine("QuickCreate.SetValue pfe_autonumber");
                telemetry.TrackTrace("QuickCreate.SetValue pfe_autonumber");
                xrmApp.QuickCreate.SetValue("pfe_autonumber", TestSettings.GetRandomString(5, 10));
                #endregion
                #region Currency
                //Work with Currency
                Trace.WriteLine("QuickCreate.SetValue pfe_currency");
                telemetry.TrackTrace("QuickCreate.SetValue pfe_currency");
                xrmApp.QuickCreate.SetValue("pfe_currency", "1.00");
                #endregion
                #region Customer
                //Work with Lookup
                //Trace.WriteLine("SetValue pfe_customer");
                //LookupItem pfe_customer = new LookupItem() { Name = "pfe_customer", Value = "Adventure Works (Sample)", Index = 0 };
                //xrmApp.Entity.SetValue(pfe_customer);
                #endregion
                #region DateTime
                try
                {
                    //Work with DateTime
                    Trace.WriteLine("QuickCreate.SetValue pfe_dateandtime");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_dateandtime");
                    DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                    xrmApp.QuickCreate.SetValue("pfe_dateandtime", expectedDate);
                    DateTime? date = xrmApp.Entity.GetValue(new DateTimeControl("pfe_dateandtime"));
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_dateandtime";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    telemetry.TrackException(ex);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw;
                }
                try
                {
                    //Work with DateOnly
                    Trace.WriteLine("QuickCreate.SetValue pfe_dateonly");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_dateonly");
                    DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                    xrmApp.QuickCreate.SetValue("pfe_dateonly", expectedDate);
                    DateTime? date = xrmApp.Entity.GetValue(new DateTimeControl("pfe_dateonly"));
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_dateonly";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Decimal
                try
                {
                    //Work with Decimal
                    Trace.WriteLine("QuickCreate.SetValue pfe_decimalnumber");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_decimalnumber");
                    xrmApp.QuickCreate.SetValue("pfe_decimalnumber", ".01");
                    string decimalReturn = xrmApp.Entity.GetValue("pfe_decimalnumber");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_decimalnumber";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Duration
                try
                {
                    //Work with Duration
                    Trace.WriteLine("QuickCreate.SetValue pfe_duration");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_duration");
                    xrmApp.QuickCreate.SetValue("pfe_duration", "1 hour");
                    string decimalReturn = xrmApp.Entity.GetValue("pfe_duration");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_duration";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Email
                try
                {
                    //Work with Email
                    Trace.WriteLine("QuickCreate.SetValue pfe_email");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_email");
                    xrmApp.QuickCreate.SetValue("pfe_email", "easyreprosupport@contoso.com");
                    string emailReturn = xrmApp.Entity.GetValue("pfe_email");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_email";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region File - TODO
                try
                {
                    //Work with File
                    Trace.WriteLine("QuickCreate.SetValue pfe_file");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_file");
                    xrmApp.QuickCreate.SetValue("pfe_file", "easyreprosupport@contoso.com");
                    string fileReturn = xrmApp.QuickCreate.GetValue("pfe_file");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_file";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Floating Point Number
                try
                {
                    //Work with Floating Point Number
                    Trace.WriteLine("QuickCreate.SetValue pfe_floatingpointnumber");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_floatingpointnumber");
                    xrmApp.QuickCreate.SetValue("pfe_floatingpointnumber", "100.001");
                    string pfe_floatingpointnumberReturn = xrmApp.Entity.GetValue("pfe_floatingpointnumber");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_floatingpointnumber";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Image - TODO
                try
                {
                    //Work with Image
                    Trace.WriteLine("QuickCreate.SetValue pfe_image");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_image");
                    xrmApp.QuickCreate.SetValue("pfe_image", "test.jpg");
                    string imageReturn = xrmApp.Entity.GetValue("pfe_image");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_image";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Language - TODO
                try
                {
                    //Work with Language
                    Trace.WriteLine("QuickCreate.SetValue pfe_language");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_language");
                    xrmApp.QuickCreate.SetValue("pfe_language", "English (United States)");
                    string languageReturn = xrmApp.Entity.GetValue("pfe_language");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_language";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Lookup
                try
                {
                    //Work with Lookup
                    Trace.WriteLine("QuickCreate.SetValue pfe_lookup");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_lookup");
                    LookupItem pfe_customer = new LookupItem() { Name = "pfe_lookup", Value = "Adventure Works (Sample)", Index = 0 };
                    xrmApp.QuickCreate.SetValue(pfe_customer);
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_lookup";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Multi Line Textbox
                try
                {
                    //Work with Multi Line Textbox
                    Trace.WriteLine("QuickCreate.SetValue pfe_multilinetext");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_multilinetext");
                    xrmApp.QuickCreate.SetValue("pfe_multilinetext", "Spanish");
                    string multiLineTextReturn = xrmApp.Entity.GetValue("pfe_multilinetext");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_multilinetext";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Multi Select Option Set
                try
                {
                    //Work with Multi Select Option Set
                    localProperties.Clear();
                    Trace.WriteLine("QuickCreate.SetValue pfe_multiselectoptionset");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_multiselectoptionset Started");
                    xrmApp.QuickCreate.SetValue(new MultiValueOptionSet { Name = "pfe_multiselectoptionset", Values = new string[] { "Allowed", "Not Allowed" } });
                    localProperties = TrackCommandExecutionMetrics(localProperties, client, xrmApp, TakeScreenshot(client, xrmApp.CommandResults.Last()));
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_multiselectoptionset Completed", ApplicationInsights.DataContracts.SeverityLevel.Information, localProperties);

                    localProperties.Clear();
                    Trace.WriteLine("QuickCreate.GetValue pfe_multiselectoptionset Started");
                    telemetry.TrackTrace("QuickCreate.GetValue pfe_multiselectoptionset Started");
                    string multiselectOptionSetReturn = xrmApp.QuickCreate.GetValue("pfe_multiselectoptionset");
                    localProperties = TrackCommandExecutionMetrics(localProperties, client, xrmApp, TakeScreenshot(client, xrmApp.CommandResults.Last()));
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_multiselectoptionset Completed");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_multiselectoptionset";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Text
                try
                {
                    //Work with Text
                    Trace.WriteLine("QuickCreate.SetValue pfe_name");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_name");
                    xrmApp.QuickCreate.SetValue("pfe_name", TestSettings.GetRandomString(5, 10));
                    string textReturn = xrmApp.QuickCreate.GetValue("pfe_name");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_name";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Option Set - TODO
                try
                {
                    //Work with Option Set
                    Trace.WriteLine("QuickCreate.SetValue pfe_optionset");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_optionset");
                    xrmApp.QuickCreate.SetValue(new OptionSet { Name = "pfe_optionset", Value = "Allowed" });
                    string textReturn = xrmApp.Entity.GetValue(new OptionSet() { Name = "pfe_optionset" });
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_optionset";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Text Area
                try
                {
                    //Work with Text Area
                    Trace.WriteLine("QuickCreate.SetValue pfe_textarea");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_textarea");
                    xrmApp.QuickCreate.SetValue("pfe_textarea", "test text area");
                    string textAreaReturn = xrmApp.QuickCreate.GetValue("pfe_textarea");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_textarea";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Ticker Symbol
                try
                {
                    //Work with tickersymbol
                    Trace.WriteLine("QuickCreate.SetValue pfe_tickersymbol");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_tickersymbol");
                    xrmApp.QuickCreate.SetValue("pfe_tickersymbol", "MSFT");
                    string tickersymbolReturn = xrmApp.QuickCreate.GetValue("pfe_tickersymbol");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_tickersymbol";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Time Zone - TODO
                try
                {
                    //Work with pfe_timezone
                    Trace.WriteLine("QuickCreate.SetValue pfe_timezone");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_timezone");
                    xrmApp.QuickCreate.SetValue("pfe_timezone", "(GMT-06:00) Central Time (US & Canada)");
                    string pfe_timezoneReturn = xrmApp.QuickCreate.GetValue("pfe_timezone");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_timezone";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Two Options
                try
                {
                    //Work with pfe_twooptions
                    Trace.WriteLine("QuickCreate.SetValue pfe_twooptions");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_twooptions");
                    xrmApp.QuickCreate.SetValue(new BooleanItem() { Name = "pfe_twooptions", Value = true });
                    string pfe_timezoneReturn = xrmApp.QuickCreate.GetValue("pfe_twooptions");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_twooptions";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Url
                try
                {
                    //Work with pfe_url
                    Trace.WriteLine("QuickCreate.SetValue pfe_url");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_url");
                    xrmApp.QuickCreate.SetValue("pfe_url", "https://microsoft.com");
                    string pfe_urlReturn = xrmApp.QuickCreate.GetValue("pfe_url");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_url";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #region Whole Number
                try
                {
                    //Work with pfe_wholenumber
                    Trace.WriteLine("QuickCreate.SetValue pfe_wholenumber");
                    telemetry.TrackTrace("QuickCreate.SetValue pfe_wholenumber");
                    xrmApp.QuickCreate.SetValue("pfe_wholenumber", "10000");
                    string pfe_wholenumberReturn = xrmApp.QuickCreate.GetValue("pfe_wholenumber");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_wholenumber";
                    telemetry.TrackException(ex);
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, xrmApp.CommandResults.Last());
                    //throw; 
                }
                #endregion
                #endregion
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                Trace.WriteLine("QuickCreate.Save");
                telemetry.TrackTrace("QuickCreate.Save");
                xrmApp.QuickCreate.Save();
                telemetry.TrackEvent(String.Format("{0} is successful.", TestContext.TestName));
            }
            catch (System.Exception ex)
            {
                telemetry.TrackException(ex);
                WriteSource("EXCEPTION_Source_", client.Browser.Driver.PageSource);
                TakeScreenshot(client, xrmApp.CommandResults.Last());
                throw ex;
            }
            finally
            {
                xrmApp.Dispose();
                telemetry.Flush();
            }
        }

        private Dictionary<string, string> TrackCommandExecutionMetrics(Dictionary<string, string> localProperties, WebClient client, XrmApp xrmApp, string artifactLocation)
        {
            localProperties.Add("Screenshot", artifactLocation);
            localProperties.Add("Success", xrmApp.CommandResults.Last().Success.ToString());
            localProperties.Add("StartTime", xrmApp.CommandResults.Last().StartTime.ToString());
            localProperties.Add("StopTime", xrmApp.CommandResults.Last().StopTime.ToString());
            localProperties.Add("ExecutionTime", xrmApp.CommandResults.Last().ExecutionTime.ToString());
            return localProperties;
        }
    }
}