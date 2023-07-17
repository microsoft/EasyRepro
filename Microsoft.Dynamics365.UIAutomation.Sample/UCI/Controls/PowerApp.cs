using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI.Controls
{
    [TestClass]
    public class PowerApp
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
        [TestMethod]
        public void UCIPowerAppSelectButton()
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
                //xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                telemetry.TrackTrace("OpenSubArea Started");
                xrmApp.Navigation.OpenSubArea("App Insights", "Application Insights Tests");
                telemetry.TrackTrace("OpenSubArea Completed");
                

                telemetry.TrackTrace("OpenRecord Started");
                xrmApp.Grid.OpenRecord(0);
                telemetry.TrackTrace("OpenRecord Completed");

                xrmApp.PowerApp.Select("8d0c1125-27da-4449-bef6-470f1ca69f97", "Button2");
                xrmApp.PowerApp.Select("8d0c1125-27da-4449-bef6-470f1ca69f97", "Button1");

            }
            catch (System.Exception ex)
            {
                telemetry.TrackException(ex);
                throw ex;
            }
        }
    }
}
