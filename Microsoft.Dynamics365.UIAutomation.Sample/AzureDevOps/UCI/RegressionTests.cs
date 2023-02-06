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
    }
}