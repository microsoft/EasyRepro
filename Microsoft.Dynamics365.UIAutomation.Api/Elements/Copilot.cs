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
            private string _botContentContainer = "//div[@aria-roledescription=\"message\"]";
            private string _botContentMarkdown = "//div[@aria-roledescription=\"message\"]//div[contains(@class,\"markdown\")]";
            private string _botContentSources = "//div[@aria-roledescription=\"message\"]//div[contains(@id,\"check-sources\")]//button";
            private string _controlTabs = "//div[@data-id=\"MscrmControls.CSIntelligence.AICopilotControl_container\"]//div[@role=\"tablist\"]//button";
            private string _controlId = "//div[@id=\"AppSidePane_MscrmControls.CSIntelligence.AICopilotControl\"]";
            private string _controlButtonId = "//button[@aria-controls=\"AppSidePane_MscrmControls.CSIntelligence.AICopilotControl\"]";

            private string _emailAssistControl = "//div[@data-id=\"MscrmControls.CSIntelligence.AICopilotControl.MscrmControls.CSIntelligence.EmailAssistControl_container\"]";
            private string _emailAssistOptions = "//div[@data-id=\"MscrmControls.CSIntelligence.AICopilotControl.MscrmControls.CSIntelligence.EmailAssistControl_container\"]//button";
            private string _emailAssistStartOver = "//div[@data-id=\"MscrmControls.CSIntelligence.AICopilotControl.MscrmControls.CSIntelligence.EmailAssistControl_container\"]//button[@data-automationid=\"splitbuttonprimary\"]";
            private string _emailAssistText = "//div[@data-id=\"MscrmControls.CSIntelligence.AICopilotControl.MscrmControls.CSIntelligence.EmailAssistControl_container\"]//textarea[@data-testid=\"txtResponseDescription\"]";
            private string _userInput = "//textarea[@data-id=\"webchat-sendbox-input\"]";
            private string _userSubmit = "//div[@class=\"webchat__send-box__main\"]//button";


            public string BotContentContainer { get => _botContentContainer; set { _botContentContainer = value; } }
            public string BotContentMarkdown { get => _botContentMarkdown; set { _botContentMarkdown = value; } }
            public string BotContentSources { get => _botContentSources; set { _botContentSources = value; } }
            public string ControlId { get => _controlId; set { _controlId = value; } }
            public string ControlButtonId { get => _controlButtonId; set { _controlButtonId = value; } }
            public string ControlTabs { get => _controlTabs; set { _controlTabs = value; } }
            public string EmailAssistControl { get => _emailAssistControl; set { _emailAssistControl = value; } }
            public string EmailAssistOptions { get => _emailAssistOptions; set { _emailAssistOptions = value; } }
            public string EmailAssistStartOver { get => _emailAssistStartOver; set { _emailAssistStartOver = value; } }
            public string EmailAssistText{ get => _emailAssistText; set { _emailAssistText = value; } }
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

        public BrowserCommandResult<string> WriteAnEmail(string userInput, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.WriteAnEmail(userInput);
        }

        #region Copilot

        internal BrowserCommandResult<bool> EnableAskAQuestion()
        {
            return _client.Execute(_client.GetOptions($"Enable Ask A Question for Copilot"), driver =>
            {
                EnableCustomerServiceCopilot(driver);
                IElement relatedEntity = null;
                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<string> AskAQuestion(string userInput)
        {
            return _client.Execute(_client.GetOptions($"Ask A Question for Copilot"), driver =>
            {
                Trace.TraceInformation("CustomerServiceCopilot.AskAQuestion initated.");
                EnableCustomerServiceCopilot(driver);
                IElement relatedEntity = null;
                if (driver.HasElement(_client.ElementMapper.CustomerServiceCopilotReference.UserInput))
                {
                    driver.FindElement(_client.ElementMapper.CustomerServiceCopilotReference.UserInput).SetValue(_client, userInput);
                    driver.FindElement(_client.ElementMapper.CustomerServiceCopilotReference.UserSubmit).Click(_client);
                    driver.Wait();
                    

                    var response = ReadBotResponse(driver);
                    var sources = ReadBotResponseSources(driver);
                    Trace.TraceInformation("CustomerServiceCopilot.AskAQuestion finalized.");
                    return response;
                }
                else
                {
                    throw new KeyNotFoundException("Copilot Text Input not found. XPath: " + _client.ElementMapper.CustomerServiceCopilotReference.UserInput);
                }
                
            });
        }

        internal BrowserCommandResult<string> WriteAnEmail(string userInput)
        {
            return _client.Execute(_client.GetOptions($"Write an Email for Copilot"), driver =>
            {
                _client.CloseTeachingBubbles(driver);
                Trace.TraceInformation("CustomerServiceCopilot.WriteAnEmail initated.");
                EnableCustomerServiceCopilot(driver);
                IElement relatedEntity = null;

                SwitchCustomerServiceCopilotTab(driver, "Write an email");
                var emailOptions = driver.FindElements(_client.ElementMapper.CustomerServiceCopilotReference.EmailAssistOptions);
                foreach (var emailOption in emailOptions)
                {
                    Trace.TraceInformation("Email Option Name: " + emailOption.Text);
                    if (emailOption.Text == userInput)
                    {
                        driver.FindElement(emailOption.Locator + "[text()='" + userInput + "']").Click(_client);
                        driver.Wait();
                    }
                }
                
                


                var response = ReadBotResponse(driver);
                var sources = ReadBotResponseSources(driver);
                Trace.TraceInformation("CustomerServiceCopilot.WriteAnEmail finalized.");
                return response;

            });
        }

        internal bool EnableCustomerServiceCopilot(IWebBrowser driver)
        {
            IElement powerApp = null;
            Trace.WriteLine("Locating Copilot");
            if (driver.HasElement(_client.ElementMapper.CustomerServiceCopilotReference.ControlButtonId))
            {
                var customerServiceCopilotButton = driver.FindElement(_client.ElementMapper.CustomerServiceCopilotReference.ControlButtonId);
                if (customerServiceCopilotButton.GetAttribute(_client, "aria-selected") == "false")
                {
                    customerServiceCopilotButton.Click(_client);
                }
                return true;
            }
            else
            {
                throw new KeyNotFoundException("Copilot not found or not enabled.");
            }
        }

        internal bool SwitchCustomerServiceCopilotTab(IWebBrowser driver, string locator)
        {
            IElement powerApp = null;
            Trace.WriteLine("SwitchCustomerServiceCopilotTab");
            if (driver.HasElement(_client.ElementMapper.CustomerServiceCopilotReference.ControlTabs))
            {
                var customerServiceCopilotTab = driver.FindElements(_client.ElementMapper.CustomerServiceCopilotReference.ControlTabs);
                foreach(var tab in customerServiceCopilotTab)
                {
                    if (tab.Text == locator)
                    {
                        tab.Click(_client);
                        return true;
                    }
                }
                return false;
            }
            else
            {
                throw new KeyNotFoundException("Copilot not found or not enabled.");
            }
        }

        internal string ReadBotResponse(IWebBrowser driver)
        {
            Trace.TraceInformation("CustomerServiceCopilot.ReadBotResponse initated.");
            return driver.FindElement(_client.ElementMapper.CustomerServiceCopilotReference.BotContentMarkdown).Text;
        }
        internal List<string> ReadBotResponseSources(IWebBrowser driver)
        {
            Trace.TraceInformation("CustomerServiceCopilot.ReadBotResponseSources initated.");
            return driver.FindElements(_client.ElementMapper.CustomerServiceCopilotReference.BotContentSources).Select(x=>x.Text).ToList();
        }
        #endregion
    }
}