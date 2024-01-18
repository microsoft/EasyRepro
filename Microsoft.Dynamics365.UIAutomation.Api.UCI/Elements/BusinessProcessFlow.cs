// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class BusinessProcessFlow : Element
    {
        #region DTO
        public class BusinessProcessFlowReference
        {
            public const string BusinessProcessFlow = "BusinessProcessFlow";
            #region private prop
            private string _NextStage_UCI = "//li[contains(@id,'processHeaderStage')]";
            private string _Flyout_UCI = "//div[contains(@id,'businessProcessFlowFlyoutHeaderContainer')]";
            private string _NextStageButton = "//button[contains(@data-id,'nextButtonContainer')]";
            private string _SetActiveButton = "//button[contains(@data-id,'setActiveButton')]";
            private string _BusinessProcessFlowFieldName = "//input[contains(@id,'[NAME]')]";
            private string _BusinessProcessFlowFormContext = "//div[contains(@id, \'ProcessStageControl-processHeaderStageFlyoutInnerContainer\')]";
            private string _TextFieldContainer = ".//div[contains(@data-lp-id, \'header_process_[NAME]\')]";
            private string _FieldSectionItemContainer = ".//div[contains(@id, \'header_process_[NAME]-FieldSectionItemContainer\')]";
            private string _TextFieldLabel = "//label[contains(@id, \'header_process_[NAME]-field-label\')]";
            private string _BooleanFieldContainer = ".//div[contains(@data-id, \'header_process_[NAME].fieldControl-checkbox-container\')]";
            private string _BooleanFieldSelectedOption = "//div[contains(@data-id, 'header_process_[NAME].fieldControl-checkbox-container')]//option[@data-selected='true']";
            private string _DateTimeFieldContainer = ".//input[contains(@data-id, \'[NAME].fieldControl-date-time-input\')]";
            private string _FieldControlDateTimeInputUCI = ".//input[contains(@data-id,'[FIELD].fieldControl-date-time-input')]";
            private string _PinStageButton = "//button[contains(@id,'stageDockModeButton')]";
            private string _CloseStageButton = "//button[contains(@id,'stageContentClose')]";
            #endregion
            #region prop
            public string NextStage_UCI { get => _NextStage_UCI; set { _NextStage_UCI = value; } }
            public string Flyout_UCI { get => _Flyout_UCI; set { _Flyout_UCI = value; } }
            public string NextStageButton { get => _NextStageButton; set { _NextStageButton = value; } }
            public string SetActiveButton { get => _SetActiveButton; set { _SetActiveButton = value; } }
            public string BusinessProcessFlowFieldName { get => _BusinessProcessFlowFieldName; set { _BusinessProcessFlowFieldName = value; } }
            public string BusinessProcessFlowFormContext { get => _BusinessProcessFlowFormContext; set { _BusinessProcessFlowFormContext = value; } }
            public string TextFieldContainer { get => _TextFieldContainer; set { _TextFieldContainer = value; } }
            public string FieldSectionItemContainer { get => _FieldSectionItemContainer; set { _FieldSectionItemContainer = value; } }
            public string TextFieldLabel { get => _TextFieldLabel; set { _TextFieldLabel = value; } }
            public string BooleanFieldContainer { get => _BooleanFieldContainer; set { _BooleanFieldContainer = value; } }
            public string BooleanFieldSelectedOption { get => _BooleanFieldSelectedOption; set { _BooleanFieldSelectedOption = value; } }
            public string DateTimeFieldContainer { get => _DateTimeFieldContainer; set { _DateTimeFieldContainer = value; } }
            public string FieldControlDateTimeInputUCI { get => _FieldControlDateTimeInputUCI; set { _FieldControlDateTimeInputUCI = value; } }
            public string PinStageButton { get => _PinStageButton; set { _PinStageButton = value; } }
            public string CloseStageButton { get => _CloseStageButton; set { _CloseStageButton = value; } }
            #endregion
        }
        #endregion
        private readonly WebClient _client;
        public BusinessProcessFlow(WebClient client): base()
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
                var formContext = driver.WaitUntilAvailable(_client.ElementMapper.BusinessProcessFlowReference.BusinessProcessFlowFormContext);
                var fieldElement = driver.WaitUntilAvailable(_client.ElementMapper.BusinessProcessFlowReference.BusinessProcessFlowFormContext + _client.ElementMapper.BusinessProcessFlowReference.FieldSectionItemContainer.Replace("[NAME]", field));
                Field returnField = new Field(fieldElement);
                returnField.Name = field;

                Element fieldLabel = null;
                try
                {
                    fieldLabel = driver.FindElement(_client.ElementMapper.BusinessProcessFlowReference.BusinessProcessFlowFormContext + _client.ElementMapper.BusinessProcessFlowReference.FieldSectionItemContainer.Replace("[NAME]", field)+ _client.ElementMapper.BusinessProcessFlowReference.TextFieldLabel.Replace("[NAME]", field));
                }
                catch (KeyNotFoundException)
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
                var fieldContainer = driver.WaitUntilAvailable(_client.ElementMapper.BusinessProcessFlowReference.TextFieldContainer.Replace("[NAME]", field));

                if (driver.FindElements(_client.ElementMapper.BusinessProcessFlowReference.TextFieldContainer.Replace("[NAME]", field) + "//input").Count > 0)
                {
                    var input = driver.FindElement(_client.ElementMapper.BusinessProcessFlowReference.TextFieldContainer.Replace("[NAME]", field) + "//input//input");
                    if (input != null)
                    {
                        input.Click(_client);
                        input.Clear(_client, input.Locator);
                        input.SetValue(_client, value);
                        input.SendKeys(_client, new string[]{ Keys.Tab});
                    }
                }
                else if (driver.FindElements(_client.ElementMapper.BusinessProcessFlowReference.TextFieldContainer.Replace("[NAME]", field) + "//textarea").Count > 0)
                {
                    var textarea = driver.FindElement(_client.ElementMapper.BusinessProcessFlowReference.TextFieldContainer.Replace("[NAME]", field) + "//textarea");
                    textarea.Click(_client);
                    textarea.Clear(_client, textarea.Locator);
                    textarea.SendKeys(_client, new string[] { value });
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
                var fieldContainer = driver.WaitUntilAvailable(_client.ElementMapper.EntityReference.TextFieldContainer.Replace("[NAME]", option.Name));

                if (driver.FindElements(_client.ElementMapper.EntityReference.TextFieldContainer.Replace("[NAME]", option.Name) + "//select").Count > 0)
                {
                    var select = driver.FindElement(_client.ElementMapper.EntityReference.TextFieldContainer.Replace("[NAME]", option.Name) + "//select");
                    var options = driver.FindElements(_client.ElementMapper.EntityReference.TextFieldContainer.Replace("[NAME]", option.Name) + "//select//option");

                    foreach (var op in options)
                    {
                        if (op.Text != option.Value && op.GetAttribute(_client,"value") != option.Value) continue;
                        op.Click(_client);
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
                var fieldContainer = driver.WaitUntilAvailable(_client.ElementMapper.BusinessProcessFlowReference.BooleanFieldContainer.Replace("[NAME]", option.Name));
                var existingValue = fieldContainer.GetAttribute(_client, "Title") == "Yes";

                if (option.Value != existingValue)
                {
                    fieldContainer.Click(_client);
                    //fieldContainer.ClickWhenAvailable("//option[not(@data-selected)]");
                    driver.ClickWhenAvailable(_client.ElementMapper.BusinessProcessFlowReference.BooleanFieldContainer.Replace("[NAME]", option.Name) + "//option[not(@data-selected)]");
                }

                driver.Wait();

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
                var dateField = _client.ElementMapper.BusinessProcessFlowReference.DateTimeFieldContainer.Replace("[FIELD]", field);

                if (driver.HasElement(dateField))
                {
                    var fieldElement = driver.ClickWhenAvailable(dateField);

                    if (fieldElement.GetAttribute(_client, "value").Length > 0)
                    {
                        //fieldElement.Click();
                        //fieldElement.SendKeys(date.ToString(format));
                        //fieldElement.SendKeys(Keys.Enter);

                        fieldElement.Click(_client);
                        _client.ThinkTime(250);
                        fieldElement.Click(_client);
                        _client.ThinkTime(250);
                        //fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        //fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        //fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        fieldElement.SetValue(_client, date.ToString(format));
                        _client.ThinkTime(500);
                        fieldElement.SendKeys(_client, new string[] { Keys.Tab });
                        _client.ThinkTime(250);
                    }
                    else
                    {
                        fieldElement.Click(_client);
                        _client.ThinkTime(250);
                        fieldElement.Click(_client);
                        _client.ThinkTime(250);
                        //fieldElement.SendKeys(Keys.Backspace);
                        //_client.ThinkTime(250);
                        //fieldElement.SendKeys(Keys.Backspace);
                        //_client.ThinkTime(250);
                        //fieldElement.SendKeys(Keys.Backspace);
                        _client.ThinkTime(250);
                        fieldElement.SetValue(_client,date.ToString(format));
                        _client.ThinkTime(250);
                        fieldElement.SendKeys(_client, new string[] { Keys.Tab });
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
                var processStages = driver.FindElements(_client.ElementMapper.BusinessProcessFlowReference.NextStage_UCI);

                if (processStages.Count == 0)
                    return true;

                foreach (var processStage in processStages)
                {
                    var divs = driver.FindElements(_client.ElementMapper.BusinessProcessFlowReference.NextStage_UCI + "//div");

                    //Click the Label of the Process Stage if found
                    foreach (var div in divs)
                    {
                        if (div.Text.Equals(stageName, StringComparison.OrdinalIgnoreCase))
                        {
                            div.Click(_client);
                        }
                    }
                }

                var flyoutFooterControls = driver.FindElements(_client.ElementMapper.BusinessProcessFlowReference.Flyout_UCI);

                foreach (var control in flyoutFooterControls)
                {
                    //If there's a field to enter, fill it out
                    if (businessProcessFlowField != null)
                    {
                        var bpfField = driver.FindElement(_client.ElementMapper.BusinessProcessFlowReference.Flyout_UCI + _client.ElementMapper.BusinessProcessFlowReference.BusinessProcessFlowFieldName.Replace("[NAME]", businessProcessFlowField.Name));

                        if (bpfField != null)
                        {
                            bpfField.Click(_client);
                            for (int i = 0; i < businessProcessFlowField.Value.Length; i++)
                            {
                                bpfField.SetValue(_client,businessProcessFlowField.Value.Substring(i, 1));
                            }
                        }
                    }

                    //Click the Next Stage Button
                    var nextButton = driver.FindElement(_client.ElementMapper.BusinessProcessFlowReference.Flyout_UCI + _client.ElementMapper.BusinessProcessFlowReference.NextStageButton);
                    nextButton.Click(_client);
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFSelectStage(string stageName, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Select Stage: {stageName}"), driver =>
            {
                //Find the Business Process Stages
                var processStages = driver.FindElements(_client.ElementMapper.BusinessProcessFlowReference.NextStage_UCI);

                foreach (var processStage in processStages)
                {
                    var divs = driver.FindElements(_client.ElementMapper.BusinessProcessFlowReference.NextStage_UCI + "//div");

                    //Click the Label of the Process Stage if found
                    foreach (var div in divs)
                    {
                        if (div.Text.Equals(stageName, StringComparison.OrdinalIgnoreCase))
                        {
                            div.Click(_client);
                        }
                    }
                }

                driver.Wait();

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

                    if (!driver.HasElement("//button[contains(@data-id,'setActiveButton')]"))
                        throw new KeyNotFoundException($"Unable to find the Set Active button. Please verify the stage name {stageName} is correct.");

                    driver.FindElement(_client.ElementMapper.BusinessProcessFlowReference.SetActiveButton).Click(_client);

                    driver.Wait();
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
                driver.Wait();

                //Pin the Stage
                if (driver.HasElement(_client.ElementMapper.BusinessProcessFlowReference.PinStageButton))
                    driver.FindElement(_client.ElementMapper.BusinessProcessFlowReference.PinStageButton).Click(_client);
                else
                    throw new KeyNotFoundException($"Pin button for stage {stageName} not found.");

                driver.Wait();
                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFClose(string stageName, int thinkTime = Constants.DefaultThinkTime)
        {
            return _client.Execute(_client.GetOptions($"Close BPF: {stageName}"), driver =>
            {
                //Click the BPF Stage
                BPFSelectStage(stageName, 0);
                driver.Wait();

                //Pin the Stage
                if (driver.HasElement(_client.ElementMapper.BusinessProcessFlowReference.CloseStageButton))
                    driver.FindElement(_client.ElementMapper.BusinessProcessFlowReference.CloseStageButton).Click(_client);
                else
                    throw new KeyNotFoundException($"Close button for stage {stageName} not found.");

                driver.Wait();
                return true;
            });
        }

        internal BrowserCommandResult<bool> BPFSwitchProcess(string processToSwitchTo, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Switch BusinessProcessFlow"), driver =>
            {
                driver.ClickWhenAvailable(_client.ElementMapper.EntityReference.ProcessButton, TimeSpan.FromSeconds(5));

                driver.ClickWhenAvailable(
                    _client.ElementMapper.EntityReference.SwitchProcess,
                    TimeSpan.FromSeconds(5),
                    "The Switch Process Button is not available."
                );

                return true;
            });
        }

        #endregion
    }
}
