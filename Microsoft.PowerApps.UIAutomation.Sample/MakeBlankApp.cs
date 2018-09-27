using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.PowerApps.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Configuration;
using System;

namespace Microsoft.PowerApps.UIAutomation.Sample
{
    [TestClass]
    public class MakeBlankApp
    {
        [TestMethod]
        public void TestMakeBlankApp()
        {
            var username = ConfigurationManager.AppSettings["OnlineUsername"];
            var password = ConfigurationManager.AppSettings["OnlinePassword"];
            var uri = new System.Uri("https://web.powerapps.com");
            var AZURE_KEY = "0c7a9c90-4cff-417f-8d97-28aac7aa76aa";

            //Create a button that uses CDS to read an account out of CRM

            using (var appCanvas = new PowerAppBrowser(TestSettings.Options))
            {
                //Login
                appCanvas.OnlineLogin.Login(uri, username.ToSecureString(), password.ToSecureString());


                //Collapse the SideBar
                appCanvas.SideBar.ExpandCollapse(8000);
                                
                //Click Home
                appCanvas.SideBar.Navigate("Home");

                //Hover over "Start from blank" and click the button
                appCanvas.Home.MakeApp("Start from blank");

                //Skip the Welcome Window
                appCanvas.Canvas.HideStudioWelcome();

                //Click the Insert tab
                appCanvas.Canvas.ClickTab("Insert");

               
                //Click the Button button in the ribbon
                var button = new CanvasControl{ Name = "Search",ControlType = "Button"};
                button.ControlProperties.Add(new CanvasControlProperty("X", "1035"));
                button.ControlProperties.Add(new CanvasControlProperty("Y", "123"));
                button.ControlProperties.Add(new CanvasControlProperty("Width", "160"));
                button.ControlProperties.Add(new CanvasControlProperty("Height", "40"));
                button.ControlProperties.Add(new CanvasControlProperty("Text", "Search"));
                appCanvas.Canvas.AddControl(button);
                
                //Click the Button button in the ribbon
                var label = new CanvasControl { Name = "Label", ControlType = "Label" };
                label.ControlProperties.Add(new CanvasControlProperty("X", "36"));
                label.ControlProperties.Add(new CanvasControlProperty("Y", "123"));
                label.ControlProperties.Add(new CanvasControlProperty("Width", "150"));
                label.ControlProperties.Add(new CanvasControlProperty("Height", "40"));
                label.ControlProperties.Add(new CanvasControlProperty("Text", "Find Contact:"));
                appCanvas.Canvas.AddControl(label);

                //Click the Button button in the ribbon
                var text = new CanvasControl { Name = "Text", ControlType = "Text:Text input" };
                text.ControlProperties.Add(new CanvasControlProperty("X", "236"));
                text.ControlProperties.Add(new CanvasControlProperty("Y", "123"));
                text.ControlProperties.Add(new CanvasControlProperty("Width", "754"));
                text.ControlProperties.Add(new CanvasControlProperty("Height", "40"));
                text.ControlProperties.Add(new CanvasControlProperty("Default", ""));
                appCanvas.Canvas.AddControl(text);

                //Upload Telemetry
                new Telemetry().AzureKey(AZURE_KEY)
                   .ExecutionId(Guid.NewGuid().ToString())
                   .TrackEvents(appCanvas.CommandResults);

                appCanvas.ThinkTime(4000);
            }
        }
    }
}

