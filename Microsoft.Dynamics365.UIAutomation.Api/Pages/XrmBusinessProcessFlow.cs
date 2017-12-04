// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api
{

    /// <summary>
    /// Business process flow.
    /// </summary>
    public class XrmBusinessProcessFlow
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XrmBusinessProcessFlow"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public XrmBusinessProcessFlow(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToContent();
        }

        private readonly string _previousStageCssSelector = ".layer0.selectedStage";
        /// <summary>
        /// Moves to the Next stage in the Business Process Flow.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.NextStage();</example>
        public BrowserCommandResult<bool> NextStage(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Next Stage", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.NextStage])))
                    throw new Exception("Business Process Flow Next Stage Element does not exist");

                if(driver.FindElement(By.Id("stageAdvanceActionContainer")).GetAttribute("tabindex") == "-1")
                    throw new Exception("Business Process Flow Next Stage Element does not exist");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.NextStage]));

                return true;
            });
        }

        /// <summary>
        /// Moves to the Previous stage in the Business Process Flow.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.PreviousStage();</example>
        public BrowserCommandResult<bool> PreviousStage(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Previous Stage", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.PreviousStage])))
                    throw new Exception("Business Process Flow Previous Stage Element does not exist");
                if (driver.HasElement(By.CssSelector(_previousStageCssSelector)))
                    throw new Exception("Business Process Flow Previous Stage Element does not exist");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.PreviousStage]));

                return true;
            });
        }

        /// <summary>
        /// Hides the Business Process flow UI.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>

        public BrowserCommandResult<bool> Hide(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Hide", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Hide])))
                    throw new Exception("Business Process Flow Next Stage Element does not exist");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Hide]));

                return true;
            });
        }
        /// <summary>
        /// Selects the Business Process Flow stage.
        /// </summary>
        /// <param name="stagenumber">The stage number that you would like to select. The stages start with 0.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SelectStage(0);</example>
        public BrowserCommandResult<bool> SelectStage(int stagenumber, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Select Stage", driver =>
            {
                var xpath = Elements.Xpath[Reference.BusinessProcessFlow.SelectStage].Replace("[STAGENUM]", stagenumber.ToString());

                if (!driver.HasElement(By.XPath(xpath)))
                    throw new Exception("Business Process Flow Select Stage Element does not exist");

                driver.ClickWhenAvailable(By.XPath(xpath));

                return true;
            });
        }

        /// <summary>
        /// Sets the current selected Stage as Active.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetActive();</example>
        public BrowserCommandResult<bool> SetActive(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Set Active", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.SetActive])))
                    throw new Exception("Business Process Flow Set Active Element does not exist");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.SetActive]));

                return true;
            });
        }

        /// <summary>
        /// Selects the Business Process Flow from the Dialog.
        /// </summary>
        /// <param name="name">The name of the business process flow you want to select.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SelectBusinessProcessFlow("Opportunity Sales Process");</example>
        public BrowserCommandResult<bool> SelectBusinessProcessFlow(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Delete"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.ProcessFlowHeader]),
                                          new TimeSpan(0, 0, 10),
                                          "The Select Business Process Flow dialog is not available.");

                if (driver.FindElement(By.ClassName(Elements.CssClass[Reference.Dialogs.SwitchProcess.SelectedRadioButton])).Text.Split('\r')[0] != name)
                {
                var processes = driver.FindElements(By.ClassName(Elements.CssClass[Reference.Dialogs.SwitchProcess.Process]));
                IWebElement element = null;

                foreach (var process in processes)
                {
                    if (process.Text == name)
                        {
                        element = process;
                            break;
                        }
                }

                if (element != null)
                    element.Click();
                else
                    throw new InvalidOperationException($"The Business Process with name: '{name}' does not exist");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Ok]));
                }
                else
                {
                    throw new InvalidOperationException($"The Business Process with name: '{name}' is already selected");
                }
                return true;
            });
        }
    }
}