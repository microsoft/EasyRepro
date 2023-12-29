// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class BusinessProcessFlow : Element
    {
        #region DTO
        public static class BusinessProcessFlowReference
        {
            public static string NextStage_UCI = "//li[contains(@id,'processHeaderStage')]";
            public static string Flyout_UCI = "//div[contains(@id,'businessProcessFlowFlyoutHeaderContainer')]";
            public static string NextStageButton = "//button[contains(@data-id,'nextButtonContainer')]";
            public static string SetActiveButton = "//button[contains(@data-id,'setActiveButton')]";
            public static string BusinessProcessFlowFieldName = "//input[contains(@id,'[NAME]')]";
            public static string BusinessProcessFlowFormContext = "//div[contains(@id, \'ProcessStageControl-processHeaderStageFlyoutInnerContainer\')]";
            public static string TextFieldContainer = ".//div[contains(@data-lp-id, \'header_process_[NAME]\')]";
            public static string FieldSectionItemContainer = ".//div[contains(@id, \'header_process_[NAME]-FieldSectionItemContainer\')]";
            public static string TextFieldLabel = "//label[contains(@id, \'header_process_[NAME]-field-label\')]";
            public static string BooleanFieldContainer = ".//div[contains(@data-id, \'header_process_[NAME].fieldControl-checkbox-container\')]";
            public static string BooleanFieldSelectedOption = "//div[contains(@data-id, 'header_process_[NAME].fieldControl-checkbox-container')]//option[@data-selected='true']";
            public static string DateTimeFieldContainer = ".//input[contains(@data-id, \'[NAME].fieldControl-date-time-input\')]";
            public static string FieldControlDateTimeInputUCI = ".//input[contains(@data-id,'[FIELD].fieldControl-date-time-input')]";
            public static string PinStageButton = "//button[contains(@id,'stageDockModeButton')]";
            public static string CloseStageButton = "//button[contains(@id,'stageContentClose')]";
        }
        #endregion
        private readonly WebClient _client;

        public BusinessProcessFlow(WebClient client)
        {
            _client = client;
        }

        #region public
        public Field GetField(string field)
        {
            return this.BPFGetField(field);
        }

        /// <summary>
        /// Retrieves the value of a text field
        /// </summary>
        /// <param name="field">Schema name of the field to retrieve</param>
        /// <returns></returns>
        public string GetValue(string field)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(field);
        }

        /// <summary>
        /// Retrieves the value of a Lookup field
        /// </summary>
        /// <param name="field">LookupItem with the schema name of the field to retrieve</param>
        public string GetValue(LookupItem field)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(field);
        }

        /// <summary>
        /// Retrieves the value of a OptionSet field
        /// </summary>
        /// <param name="field">OptionSet with the schema name of the field to retrieve</param
        public string GetValue(OptionSet field)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(field);
        }

        /// <summary>
        /// Retrieves the value of a BooleanItem field.
        /// </summary>
        /// <param name="field">BooleanItem with the schema name of the field to retrieve.</param>
        public bool GetValue(BooleanItem field)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(field);
        }

        /// <summary>
        /// Sets the stage provided to Active
        /// </summary>
        /// <param name="stageName">Name of the business process flow stage</param>
        public void SetActive(string stageName = "")
        {
            this.BPFSetActive(stageName);
        }

        /// <summary>
        /// Clicks "Next Stage" on the stage provided
        /// </summary>
        /// <param name="stageName">Name of the business process flow stage</param>
        /// <param name="businessProcessFlowField">Optional - field to set the value on for this business process flow stage</param>
        public void NextStage(string stageName, Field businessProcessFlowField = null)
        {
            this.BPFNextStage(stageName, businessProcessFlowField);
        }

        /// <summary>
        /// Selects the stage provided
        /// </summary>
        /// <param name="stageName">Name of the business process flow stage</param>
        public void SelectStage(string stageName)
        {
            this.BPFSelectStage(stageName);
        }

        /// <summary>
        /// Sets the value of a text field
        /// </summary>
        /// <param name="field">Schema name of the field to retrieve</param>
        public void SetValue(string field, string value)
        {
            this.BPFSetValue(field, value);
        }

        /// <summary>
        /// Sets the value of an OptionSet field
        /// </summary>
        /// <param name="field">OptionSet with the schema name of the field to retrieve</param>
        public void SetValue(OptionSet optionSet)
        {
            this.BPFSetValue(optionSet);
        }

        /// <summary>
        /// Sets the value of a BooleanItem field
        /// </summary>
        /// <param name="field">BooleanItem with the schema name of the field to retrieve</param>
        public void SetValue(BooleanItem optionSet)
        {
            this.BPFSetValue(optionSet);
        }

        /// <summary>
        /// Sets the value of a LookupItem field
        /// </summary>
        /// <param name="field">LookupItem with the schema name of the field to retrieve</param>
        public void SetValue(LookupItem control)
        {
            Lookup lookup = new Lookup(_client);
            lookup.SetValue(control, FormContextType.BusinessProcessFlow);
        }

        /// <summary>
        /// Sets the value of a Date field
        /// </summary>
        /// <param name="field">Schema name of the field to retrieve</param>
        public void SetValue(string field, DateTime date, string formatDate = null, string formatTime = null)
        {
            DateTimeControl.SetValue(_client, field, date, FormContextType.BusinessProcessFlow, formatDate, formatTime);
        }

        /// <summary>
        /// Sets the value of a MultiValueOptionSet field
        /// </summary>
        /// <param name="field">MultiValueOptionSet with the schema name of the field to retrieve</param>
        public void SetValue(WebClient client,MultiValueOptionSet option, bool removeExistingValues = false)
        {
            MultiValueOptionSet.SetValue(client, option, FormContextType.BusinessProcessFlow, removeExistingValues);
        }

        /// <summary>
        /// Pins the Business Process Flow Stage to the right side of the window
        /// </summary>
        /// <param name="stageName">The name of the Business Process Flow Stage</param>
        public void Pin(string stageName)
        {
            this.BPFPin(stageName);
        }

        /// <summary>
        /// Clicks the "X" button in the Business Process Flow flyout menu for the Stage provided
        /// </summary>
        /// <param name="stageName">Name of the business process flow stage</param>
        public void Close(string stageName)
        {
            this.BPFClose(stageName);
        }
        #endregion

        #region BusinessProcessFlow

        internal BrowserCommandResult<Field> BPFGetField(string field)
        {
            return _client.Execute(_client.GetOptions($"Get Field"), driver =>
            {

                // Initialize the Business Process Flow context
                var formContext = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFormContext));
                var fieldElement = formContext.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.FieldSectionItemContainer.Replace("[NAME]", field)));
                Field returnField = new Field(fieldElement);
                returnField.Name = field;

                IWebElement fieldLabel = null;
                try
                {
                    fieldLabel = fieldElement.FindElement(By.XPath(BusinessProcessFlowReference.TextFieldLabel.Replace("[NAME]", field)));
                }
                catch (NoSuchElementException)
                {
                    // Swallow
                }

                if (fieldLabel != null)
                {
                    returnField.Label = fieldLabel.Text;
                }

                return returnField;
            });
        }

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.BusinessProcessFlow.SetValue("firstname", "Test");</example>
        internal BrowserCommandResult<bool> BPFSetValue(string field, string value)
        {
            return _client.Execute(_client.GetOptions($"Set BPF Value"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.TextFieldContainer.Replace("[NAME]", field)));

                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    var input = fieldContainer.FindElement(By.TagName("input"));
                    if (input != null)
                    {
                        input.Click(true);
                        input.Clear();
                        input.SendKeys(value, true);
                        input.SendKeys(Keys.Tab);
                    }
                }
                else if (fieldContainer.FindElements(By.TagName("textarea")).Count > 0)
                {
                    var textarea = fieldContainer.FindElement(By.TagName("textarea"));
                    textarea.Click();
                    textarea.Clear();
                    textarea.SendKeys(value);
                }
                else
                {
                    throw new Exception($"Field with name {field} does not exist.");
                }

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        internal BrowserCommandResult<bool> BPFSetValue(OptionSet option)
        {
            return _client.Execute(_client.GetOptions($"Set BPF Value: {option.Name}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", option.Name)));

                if (fieldContainer.FindElements(By.TagName("select")).Count > 0)
                {
                    var select = fieldContainer.FindElement(By.TagName("select"));
                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.Text != option.Value && op.GetAttribute("value") != option.Value) continue;
                        op.Click(true);
                        break;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");
                }

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue(new BooleanItem { Name = "preferredcontactmethodcode"});</example>
        internal BrowserCommandResult<bool> BPFSetValue(BooleanItem option)
        {
            return _client.Execute(_client.GetOptions($"Set BPF Value: {option.Name}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BooleanFieldContainer.Replace("[NAME]", option.Name)));
                var existingValue = fieldContainer.GetAttribute("Title") == "Yes";

                if (option.Value != existingValue)
                {
                    fieldContainer.Click();
                    fieldContainer.ClickWhenAvailable(By.XPath("//option[not(@data-selected)]"));
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="format">DateTime format</param>
        /// <example> xrmBrowser.BusinessProcessFlow.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        internal BrowserCommandResult<bool> BPFSetValue(string field, DateTime date, string format = "MM dd yyyy")
        {
            return _client.Execute(_client.GetOptions($"Set BPF Value: {field}"), driver =>
            {
                var dateField = BusinessProcessFlowReference.DateTimeFieldContainer.Replace("[FIELD]", field);

                if (driver.HasElement(By.XPath(dateField)))
                {
                    var fieldElement = driver.ClickWhenAvailable(By.XPath(dateField));

                    if (fieldElement.GetAttribute("value").Length > 0)
                    {
                        //fieldElement.Click();
                        //fieldElement.SendKeys(date.ToString(format));
                        //fieldElement.SendKeys(Keys.Enter);

                        fieldElement.Click();
                        _client.ThinkTime(250);
                        fieldElement.Click();
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(date.ToString(format), true);
                        _client.ThinkTime(500);
                        fieldElement.SendKeys(Keys.Tab);
                        _client.ThinkTime(250);
                    }
                    else
                    {
                        fieldElement.Click();
                        _client.ThinkTime(250);
                        fieldElement.Click();
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(date.ToString(format));
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(Keys.Tab);
                        _client.ThinkTime(250);
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFNextStage(string stageName, Field businessProcessFlowField = null, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Next Stage"), driver =>
            {
                //Find the Business Process Stages
                var processStages = driver.FindElements(By.XPath(BusinessProcessFlowReference.NextStage_UCI));

                if (processStages.Count == 0)
                    return true;

                foreach (var processStage in processStages)
                {
                    var divs = processStage.FindElements(By.TagName("div"));

                    //Click the Label of the Process Stage if found
                    foreach (var div in divs)
                    {
                        if (div.Text.Equals(stageName, StringComparison.OrdinalIgnoreCase))
                        {
                            div.Click();
                        }
                    }
                }

                var flyoutFooterControls = driver.FindElements(By.XPath(BusinessProcessFlowReference.Flyout_UCI));

                foreach (var control in flyoutFooterControls)
                {
                    //If there's a field to enter, fill it out
                    if (businessProcessFlowField != null)
                    {
                        var bpfField = control.FindElement(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFieldName.Replace("[NAME]", businessProcessFlowField.Name)));

                        if (bpfField != null)
                        {
                            bpfField.Click();
                            for (int i = 0; i < businessProcessFlowField.Value.Length; i++)
                            {
                                bpfField.SendKeys(businessProcessFlowField.Value.Substring(i, 1));
                            }
                        }
                    }

                    //Click the Next Stage Button
                    var nextButton = control.FindElement(By.XPath(BusinessProcessFlowReference.NextStageButton));
                    nextButton.Click();
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFSelectStage(string stageName, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Select Stage: {stageName}"), driver =>
            {
                //Find the Business Process Stages
                var processStages = driver.FindElements(By.XPath(BusinessProcessFlowReference.NextStage_UCI));

                foreach (var processStage in processStages)
                {
                    var divs = processStage.FindElements(By.TagName("div"));

                    //Click the Label of the Process Stage if found
                    foreach (var div in divs)
                    {
                        if (div.Text.Equals(stageName, StringComparison.OrdinalIgnoreCase))
                        {
                            div.Click();
                        }
                    }
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFSetActive(string stageName = "", int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Set Active Stage: {stageName}"), driver =>
            {
                if (!String.IsNullOrEmpty(stageName))
                {
                    SelectStage(stageName);

                    if (!driver.HasElement(By.XPath("//button[contains(@data-id,'setActiveButton')]")))
                        throw new NotFoundException($"Unable to find the Set Active button. Please verify the stage name {stageName} is correct.");

                    driver.FindElement(By.XPath(BusinessProcessFlowReference.SetActiveButton)).Click(true);

                    driver.WaitForTransaction();
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFPin(string stageName, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Pin BPF: {stageName}"), driver =>
            {
                //Click the BPF Stage
                BPFSelectStage(stageName, 0);
                driver.WaitForTransaction();

                //Pin the Stage
                if (driver.HasElement(By.XPath(BusinessProcessFlowReference.PinStageButton)))
                    driver.FindElement(By.XPath(BusinessProcessFlowReference.PinStageButton)).Click();
                else
                    throw new NotFoundException($"Pin button for stage {stageName} not found.");

                driver.WaitForTransaction();
                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFClose(string stageName, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Close BPF: {stageName}"), driver =>
            {
                //Click the BPF Stage
                BPFSelectStage(stageName, 0);
                driver.WaitForTransaction();

                //Pin the Stage
                if (driver.HasElement(By.XPath(BusinessProcessFlowReference.CloseStageButton)))
                    driver.FindElement(By.XPath(BusinessProcessFlowReference.CloseStageButton)).Click(true);
                else
                    throw new NotFoundException($"Close button for stage {stageName} not found.");

                driver.WaitForTransaction();
                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFSwitchProcess(string processToSwitchTo, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Switch BusinessProcessFlow"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Entity.EntityReference.ProcessButton), TimeSpan.FromSeconds(5));

                driver.ClickWhenAvailable(
                    By.XPath(Entity.EntityReference.SwitchProcess),
                    TimeSpan.FromSeconds(5),
                    "The Switch Process Button is not available."
                );

                return true;
            });
        }

        #endregion
    }
}
