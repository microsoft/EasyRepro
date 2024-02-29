// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using Microsoft.Dynamics365.UIAutomation.Api.DTO;
using Keys = Microsoft.Dynamics365.UIAutomation.Browser.Keys;
using System.Diagnostics;
using Microsoft.Dynamics365.UIAutomation.Api.Controls;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Represents an individual field on a Dynamics 365 Customer Experience web form.
    /// </summary>
    public class Field
    {
        public static class FieldReference
        {
            public static string ReadOnly = ".//*[@aria-readonly]";
            public static string Required = ".//*[@aria-required]";
            public static string RequiredIcon = ".//div[contains(@data-id, 'required-icon') or contains(@id, 'required-icon')]";

            public static string LookupItemLocator = "data-id";
            public static string LookupItemLocatorValue = "LookupResults";
            public static string MultiValueOptionSetLocator = "class";
            public static string MultiValueOptionSetLocatorValue = "msos-input";
            public static string RichTextEditorLocator = "class";
            public static string RichTextEditorLocatorValue = "fullPageContentEditorFrame";
        }
        //Constructors
        public Field(IElement containerIElement)
        {
            this.containerIElement = containerIElement;
        }

        public Field(WebClient client)
        {
            _client = client;
        }

        private WebClient _client { get; set; }
        internal IElement _inputIElement { get; set; }

        //IElement that contains the container for the field on the form
        internal IElement containerIElement { get; set; }

        public void Click(WebClient client)
        {
            _inputIElement.Click(client);
        }

        /// <summary>
        /// Gets or sets the identifier of the field.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Label of the field.
        /// </summary>
        /// <value>The field label</value>
        public string Label { get; set; }

        /// <summary>
        /// Returns if the field is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                if (_client.Browser.Browser.HasElement(containerIElement.Locator + FieldReference.ReadOnly))
                {
                    var readOnly = _client.Browser.Browser.FindElement(containerIElement.Locator + FieldReference.ReadOnly);

                    if (readOnly.HasAttribute(_client,"aria-readonly"))
                    {
                        // TwoOption / Text / Lookup Condition
                        bool isReadOnly = Convert.ToBoolean(readOnly.GetAttribute(_client,"aria-readonly"));
                        if (isReadOnly)
                            return true;
                    }
                    else if (readOnly.HasAttribute(_client, "readonly"))
                        return true;
                }
                else if (_client.Browser.Browser.HasElement(containerIElement.Locator + "//select"))
                {
                    // Option Set Condition
                    var readOnlySelect = _client.Browser.Browser.FindElement(containerIElement.Locator + "//select");

                    if (readOnlySelect.HasAttribute(_client, "disabled"))
                        return true;

                }
                else if (_client.Browser.Browser.HasElement(containerIElement.Locator + "//input"))
                {
                    // DateTime condition
                    var readOnlyInput = _client.Browser.Browser.FindElement(containerIElement.Locator + "//input");

                    if (readOnlyInput.HasAttribute(_client, "disabled") || readOnlyInput.HasAttribute(_client, "readonly"))
                        return true;
                }
                else if (_client.Browser.Browser.HasElement(containerIElement.Locator + "//textarea"))
                {
                    var readOnlyTextArea = _client.Browser.Browser.FindElement(containerIElement.Locator + "//textarea");
                    return readOnlyTextArea.HasAttribute(_client, "readonly");
                }
                else
                {
                    // Special Lookup Field condition (e.g. transactioncurrencyid)
                    var lookupRecordList = _client.Browser.Browser.FindElement(containerIElement.Locator + ".//div[contains(@id,'RecordList') and contains(@role,'presentation')]");
                    var lookupDescription = _client.Browser.Browser.FindElement(lookupRecordList.Locator + "//div");

                    if (lookupDescription != null)
                        return lookupDescription.GetAttribute(_client, "innerText").ToLowerInvariant().Contains("readonly", StringComparison.OrdinalIgnoreCase);                   
                    else
                        return false;                    
                }

                return false;
            }
        }

        /// <summary>
        /// Returns if the field is required.
        /// </summary>
        public bool IsRequired
        {
            get
            {
                if (_client.Browser.Browser.HasElement(containerIElement.Locator + FieldReference.RequiredIcon))
                {
                    var required = _client.Browser.Browser.FindElement(containerIElement.Locator + FieldReference.RequiredIcon);

                    if (required != null)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Returns if the field is visible.
        /// </summary>
        public bool IsVisible {
            get
            {
                return containerIElement.IsAvailable;
            }
        }

        /// <summary>
        /// Gets or sets the value of the field.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        internal void SetInputValue(IWebBrowser driver, IElement input, string value, TimeSpan? thinktime = null)
        {
            Trace.TraceInformation("Field.SetInputValue initiated.");
            input.Clear(_client, input.Locator);
            input.Click(_client);
            //input.SendKeys(_client, new string[] { Keys.Control, "a" });
            //input.SendKeys(_client, new string[] { Keys.Control + "a" });
            //input.SendKeys(_client, new string[] { Keys.Backspace });

            input.SetValue(_client, value);
            //driver.Wait();

            // Repeat set value if expected value is not set
            // Do this to ensure that the static placeholder '---' is removed 
            //driver.RepeatUntil(() =>
            //{
            //    input.Clear();
            //    input.Click();
            //    input.SendKeys(Keys.Control + "a");
            //    input.SendKeys(Keys.Control + "a");
            //    input.SendKeys(Keys.Backspace);
            //    input.SendKeys(value);
            //    driver.WaitForTransaction();
            //},
            //    d => input.GetAttribute("value").IsValueEqualsTo(value),
            //    TimeSpan.FromSeconds(9), 3,
            //    failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {value}. Actual: {input.GetAttribute("value")}")
            //);

            //driver.Wait();
        }

        internal static BrowserCommandResult<bool> ClearValue(WebClient client, string fieldName, FormContextType formContextType)
        {
            return client.Execute(client.GetOptions($"Clear Field {fieldName}"), driver =>
            {
                Field objField = new Field(client);
                objField.SetValue(client, fieldName, string.Empty, formContextType);

                return true;
            });
        }

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.Entity.SetValue("firstname", "Test");</example>
        internal BrowserCommandResult<bool> SetValue(WebClient client, string field, string value, FormContextType formContextType = FormContextType.Entity)
        {
            Trace.TraceInformation("Field.SetValue initiated with field " + field);
            return client.Execute(client.GetOptions("Set Value"), driver =>
            {
                IElement fieldContainer = null;
                fieldContainer = client.ValidateFormContext(driver, formContextType, field, fieldContainer);

                IElement input=null;
                bool found = driver.HasElement(fieldContainer.Locator + "//input");
                //input = client.Browser.Browser.FindElement(fieldContainer.Locator + "//input");
                if (!found)
                {
                    found = driver.HasElement(fieldContainer.Locator + "//textarea");
                    if (!found)
                    {
                        if (driver.HasElement(fieldContainer.Locator + "//iframe"))
                        {
                            input = driver.FindElement(fieldContainer.Locator + "//iframe");
                        }

                    }
                }
                else
                    input = driver.FindElement(fieldContainer.Locator + "//input");

                if (!found && input !=null && input.IsAvailable)
                {
                    //determine type of control
                    if (input.HasAttribute(_client, FieldReference.MultiValueOptionSetLocator) && input.GetAttribute(_client, FieldReference.MultiValueOptionSetLocator) == FieldReference.MultiValueOptionSetLocatorValue)
                    {
                        Trace.TraceInformation("Field.SetValue: Converting to MultiValueOptionSet.");
                        MultiValueOptionSet multiValueOptionSet = new MultiValueOptionSet() {Name = field };
                        multiValueOptionSet.Values = new string[] {value};
                        multiValueOptionSet.SetValue(client, multiValueOptionSet, formContextType);
                        return true;
                    }
                    if (input.HasAttribute(_client, FieldReference.LookupItemLocator) && input.GetAttribute(_client, FieldReference.LookupItemLocator).Contains(FieldReference.LookupItemLocatorValue))
                    {
                        Trace.TraceInformation("Field.SetValue: Converting to LookupItem.");
                        LookupItem lookup = new LookupItem();
                        lookup.Name = field;
                        lookup.Value = value;
                        Lookup lookup1 = new Lookup(client);
                        lookup1.SetValue(lookup, formContextType);
                        return true;
                    }
                    if (input.GetAttribute(_client, FieldReference.RichTextEditorLocator).Contains(FieldReference.RichTextEditorLocatorValue))
                    {
                        Trace.TraceInformation("Field.SetValue: Converting to RichTextEditorControl.");
                        RichTextEditor rteControl = new RichTextEditor(field);
                        rteControl.SetValue(client, value);
                        driver.SwitchToFrame("0");
                        return true;
                    }
                    found = true;
                }

                if (!found)
                    throw new KeyNotFoundException($"Field with name {field} does not exist.");

                SetInputValue(driver, input, value);

                return true;
            });
        }

        internal static void ClearFieldValue(WebClient client, IElement field)
        {
            if (field.GetAttribute(client, "value").Length > 0)
            {
                field.SendKeys(client, new string[] { Keys.Control + "a" });
                field.SendKeys(client, new string[] { Keys.Backspace });
            }

            client.ThinkTime(500);
        }
    }
}
