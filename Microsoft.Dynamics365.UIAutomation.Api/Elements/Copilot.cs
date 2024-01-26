// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using static Microsoft.Dynamics365.UIAutomation.Api.Entity;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class ModelAppCopilot
    {
        private readonly WebClient _client;

        public ModelAppCopilot(WebClient client) : base()
        {
            _client = client;
        }


    }
    public class SalesCopilot : ModelAppCopilot
    {
        private readonly WebClient _client;
        private bool _coPilotEnabled = false;
        private bool _inPowerApps = false;
        public SalesCopilot(WebClient client) : base(client)
        {
            _client = client;
        }

        /// <summary>
        /// Clicks command on the command bar
        /// </summary>
        /// <param name="name">Name of button to click</param>
        /// <param name="subname">Name of button on submenu to click</param>
        /// <param name="subSecondName">Name of button on submenu (3rd level) to click</param>
        public BrowserCommandResult<bool> Enable()
        {
            return this.EnableSalesCopilot(_client.Browser.Browser);
        }

        internal bool EnableSalesCopilot(IWebBrowser driver)
        {
            IElement powerApp = null;
            Trace.WriteLine("Locating Copilot");
            if (driver.HasElement(_client.ElementMapper.PowerAppReference.ModelFormContainer))
            {
                powerApp = driver.FindElement(_client.ElementMapper.PowerAppReference.ModelFormContainer);
                driver.SwitchToFrame(_client.ElementMapper.PowerAppReference.ModelFormContainer);
                powerApp = driver.FindElement("//iframe[@class='publishedAppIframe']");
                driver.SwitchToFrame("//iframe[@class='publishedAppIframe']");
                _inPowerApps = true;
            }
            else
            {
                throw new KeyNotFoundException("Copilot not found or not enabled.");
            }
            return true;
        }
    }
    public class CustomerServiceCopilot :ModelAppCopilot
    {
        private CustomerServiceCopilotReference _customerServiceCopilotReference { get; set; }
        public class CustomerServiceCopilotReference
        {
            public const string CustomerServiceCopilot = "CustomerServiceCopilot";
            private string _controlId = "//div[@id=\"AppSidePane_MscrmControls.CSIntelligence.AICopilotControl\"]";
            private string _controlButtonId = "//button[@id=\"sidepane-tab-button-AppSidePane_MscrmControls.CSIntelligence.AICopilotControl\"]";
            private string _userInput = "//textarea[@data-id=\"webchat-sendbox-input\"]";
            private string _userSubmit = "//div[@class=\"webchat__send-box__main\"]//button";

            public string ControlId { get => _controlId; set { _controlId = value; } }
            public string ControlButtonId { get => _controlButtonId; set { _controlButtonId = value; } }
            public string UserInput { get => _userInput; set { _userInput = value; } }
            public string UserSubmit { get => _userSubmit; set { _userSubmit = value; } }
        }

        private readonly WebClient _client;

        public CustomerServiceCopilot(WebClient client) : base(client)
        {
            _client = client;
        }

        /// <summary>
        /// Clicks command on the command bar
        /// </summary>
        /// <param name="name">Name of button to click</param>
        /// <param name="subname">Name of button on submenu to click</param>
        /// <param name="subSecondName">Name of button on submenu (3rd level) to click</param>
        public BrowserCommandResult<bool> Enable()
        {
            return this.EnableCustomerServiceCopilot(_client.Browser.Browser);
        }

        public BrowserCommandResult<bool> EnableAskAQuestion(int thinkTime = Constants.DefaultThinkTime)
        {
            return this.EnableAskAQuestion();
        }

        public BrowserCommandResult<string> AskAQuestion(string userInput, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.AskAQuestion(userInput);
        }

        #region Copilot
        private bool _coPilotEnabled = false;
        private bool _inPowerApps = false;
        internal bool EnableCustomerServiceCopilot(IWebBrowser driver)
        {
            IElement powerApp = null;
            Trace.WriteLine("Locating Copilot");
            if (driver.HasElement(_client.ElementMapper.PowerAppReference.ModelFormContainer))
            {
                powerApp = driver.FindElement(_client.ElementMapper.PowerAppReference.ModelFormContainer);
                driver.SwitchToFrame(_client.ElementMapper.PowerAppReference.ModelFormContainer);
                powerApp = driver.FindElement("//iframe[@class='publishedAppIframe']");
                driver.SwitchToFrame("//iframe[@class='publishedAppIframe']");
                _inPowerApps = true;
            }
            else
            {
                throw new KeyNotFoundException("Copilot not found or not enabled.");
            }
            return true;
        }
        internal BrowserCommandResult<bool> EnableAskAQuestion()
        {
            return _client.Execute(_client.GetOptions($"Enable Ask A Question for Copilot"), driver =>
            {
                if (!_coPilotEnabled) EnableCustomerServiceCopilot(driver);
                IElement relatedEntity = null;
                //if (driver.TryFindElement(By.XPath(""), out var copilot))
                //{
                //    // Advanced lookup
                //    //relatedEntity = advancedLookup.WaitUntilAvailable(
                //    //    By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.FilterTable].Replace("[NAME]", entityName)),
                //    //    2.Seconds());
                //}
                //else
                //{
                //    // Lookup 
                //    //relatedEntity = driver.WaitUntilAvailable(
                //    //    By.XPath(AppElements.Xpath[AppReference.Lookup.RelatedEntityLabel].Replace("[NAME]", entityName)),
                //    //    2.Seconds());
                //}

                //if (relatedEntity == null)
                //{
                //    throw new NotFoundException($"Lookup Entity {entityName} not found.");
                //}

                //relatedEntity.Click();
                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<string> AskAQuestion(string userInput)
        {
            return _client.Execute(_client.GetOptions($"Ask A Question for Copilot"), driver =>
            {
                if (!_coPilotEnabled) EnableCustomerServiceCopilot(driver);
                IElement relatedEntity = null;
                //if (driver.TryFindElement(By.XPath(""), out var copilot))
                //{
                //    // Advanced lookup
                //    //relatedEntity = advancedLookup.WaitUntilAvailable(
                //    //    By.XPath(AppElements.Xpath[AppReference.AdvancedLookup.FilterTable].Replace("[NAME]", entityName)),
                //    //    2.Seconds());
                //}
                //else
                //{
                //    // Lookup 
                //    //relatedEntity = driver.WaitUntilAvailable(
                //    //    By.XPath(AppElements.Xpath[AppReference.Lookup.RelatedEntityLabel].Replace("[NAME]", entityName)),
                //    //    2.Seconds());
                //}

                //if (relatedEntity == null)
                //{
                //    throw new NotFoundException($"Lookup Entity {entityName} not found.");
                //}

                //relatedEntity.Click();
                driver.Wait();

                return string.Empty;
            });
        }
        #endregion
    }
}