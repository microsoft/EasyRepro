// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class PowerApp : Element
    {

        #region DTO
        public class PowerAppReference
        {
            public const string PowerApp = "PowerApp";
            private string _modelFormContainer = "//iframe[contains(@src,\'[NAME]\')]";
            private string _control = "//div[@data-control-name=\'[NAME]\']";
            private string _publishedAppIFrame = "//iframe[@class='publishedAppIframe']";
            public string ModelFormContainer { get => _modelFormContainer; set { _modelFormContainer = value; } }
            public string Control { get => _control; set { _control = value; } }
            public string PublishedAppIFrame { get => _publishedAppIFrame; set { _publishedAppIFrame = value; } }

        }
        #endregion
        private readonly WebClient _client;
        public enum AppType { EmbeddedPowerApp, CustomPage }
        private string _appId;
        public PowerApp(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Sends Power FX command to Power App
        /// </summary>
        /// <param name="appId">Id of the app to select</param>
        /// <param name="command">command to execute</param>
        public void SendCommand(string appId, string command)
        {
            this.PowerAppSendCommand(appId, command);
        }

        public void Select(string appId, string control)
        {
            this.PowerAppSelect(appId, control);
        }

        public void SetProperty(string appId, string control, string value)
        {
            this.PowerAppSetProperty(appId, control, value);
        }

        #region PowerApp
        private bool _inPowerApps = false;
        internal IWebElement LocatePowerApp(IWebDriver driver, string appId)
        {
            IWebElement powerApp = null;
            Trace.WriteLine(String.Format("Locating {0} App", appId));
            if (driver.HasElement(By.XPath(this._client.ElementMapper.PowerAppReference.ModelFormContainer.Replace("[NAME]", appId))))
            {
                powerApp = driver.FindElement(By.XPath(this._client.ElementMapper.PowerAppReference.ModelFormContainer.Replace("[NAME]", appId)));
                driver.SwitchTo().Frame(powerApp);
                powerApp = driver.FindElement(By.XPath(this._client.ElementMapper.PowerAppReference.PublishedAppIFrame));
                driver.SwitchTo().Frame(powerApp);
                _inPowerApps = true;
            }
            else
            {
                throw new NotFoundException(String.Format("PowerApp with Id {0} not found.", appId));
            }
            return powerApp;
        }
        private BrowserCommandResult<bool> PowerAppSendCommand(string appId, string command)
        {
            return _client.Execute(_client.GetOptions("PowerApp Send Command"), driver =>
            {
                LocatePowerApp(driver, appId);
                return true;
            });
        }
        public BrowserCommandResult<bool> PowerAppSelect(string appId, string control)
        {

            return _client.Execute(_client.GetOptions("PowerApp Select"), driver =>
            {
                if (!_inPowerApps) LocatePowerApp(driver, appId);
                if (driver.HasElement(By.XPath(this._client.ElementMapper.PowerAppReference.Control.Replace("[NAME]", control))))
                {
                    driver.FindElement(By.XPath(this._client.ElementMapper.PowerAppReference.Control.Replace("[NAME]", control))).Click();
                }
                else
                {
                    throw new NotFoundException(String.Format("Control {0} not found in Power App {1}", control, appId));
                }
                return true;
            });
        }
        public BrowserCommandResult<bool> PowerAppSetProperty(string appId, string control, string value)
        {

            return _client.Execute(_client.GetOptions("PowerApp Set Property"), driver =>
            {
                LocatePowerApp(driver, appId);
                return true;
            });
        }
        #endregion PowerApp

    }
}
