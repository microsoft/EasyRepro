// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
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
        }
        //Constructors
        public Field(IWebElement containerElement)
        {
            this.containerElement = containerElement;
        }

        public Field()
        {

        }


        internal IWebElement _inputElement { get; set; }

        //Element that contains the container for the field on the form
        internal IWebElement containerElement { get; set; }

        public void Click()
        {
            _inputElement.Click(true);
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
                if (containerElement.HasElement(By.XPath(FieldReference.ReadOnly)))
                {
                    var readOnly = containerElement.FindElement(By.XPath(FieldReference.ReadOnly));

                    if (readOnly.HasAttribute("aria-readonly"))
                    {
                        // TwoOption / Text / Lookup Condition
                        bool isReadOnly = Convert.ToBoolean(readOnly.GetAttribute("aria-readonly"));
                        if (isReadOnly)
                            return true;
                    }
                    else if (readOnly.HasAttribute("readonly"))
                        return true;
                }
                else if (containerElement.HasElement(By.TagName("select")))
                {
                    // Option Set Condition
                    var readOnlySelect = containerElement.FindElement(By.TagName("select"));

                    if (readOnlySelect.HasAttribute("disabled"))
                        return true;

                }
                else if (containerElement.HasElement(By.TagName("input")))
                {
                    // DateTime condition
                    var readOnlyInput = containerElement.FindElement(By.TagName("input"));

                    if (readOnlyInput.HasAttribute("disabled") || readOnlyInput.HasAttribute("readonly"))
                        return true;
                }
                else if (containerElement.HasElement(By.TagName("textarea")))
                {
                    var readOnlyTextArea = containerElement.FindElement(By.TagName("textarea"));
                    return readOnlyTextArea.HasAttribute("readonly");
                }
                else
                {
                    // Special Lookup Field condition (e.g. transactioncurrencyid)
                    var lookupRecordList = containerElement.FindElement(By.XPath(Lookup.LookupReference.RecordList));
                    var lookupDescription = lookupRecordList.FindElement(By.TagName("div"));

                    if (lookupDescription != null)
                        return lookupDescription.GetAttribute("innerText").ToLowerInvariant().Contains("readonly", StringComparison.OrdinalIgnoreCase);                   
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
                if (containerElement.HasElement(By.XPath(FieldReference.RequiredIcon)))
                {
                    var required = containerElement.FindElement(By.XPath(FieldReference.RequiredIcon));

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
                return containerElement.Displayed;
            }
        }

        /// <summary>
        /// Gets or sets the value of the field.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        internal static void SetInputValue(IWebDriver driver, IWebElement input, string value, TimeSpan? thinktime = null)
        {
            // Repeat set value if expected value is not set
            // Do this to ensure that the static placeholder '---' is removed 
            driver.RepeatUntil(() =>
            {
                input.Clear();
                input.Click();
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Backspace);
                input.SendKeys(value);
                driver.WaitForTransaction();
            },
                d => input.GetAttribute("value").IsValueEqualsTo(value),
                TimeSpan.FromSeconds(9), 3,
                failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {value}. Actual: {input.GetAttribute("value")}")
            );

            driver.WaitForTransaction();
        }

        internal static BrowserCommandResult<bool> ClearValue(WebClient client, string fieldName, FormContextType formContextType)
        {
            return client.Execute(client.GetOptions($"Clear Field {fieldName}"), driver =>
            {
                Field.SetValue(client, fieldName, string.Empty, formContextType);

                return true;
            });
        }

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.Entity.SetValue("firstname", "Test");</example>
        internal static BrowserCommandResult<bool> SetValue(WebClient client, string field, string value, FormContextType formContextType = FormContextType.Entity)
        {
            return client.Execute(client.GetOptions("Set Value"), driver =>
            {
                IWebElement fieldContainer = null;
                fieldContainer = client.ValidateFormContext(driver, formContextType, field, fieldContainer);

                IWebElement input;
                bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);

                if (!found)
                    found = fieldContainer.TryFindElement(By.TagName("textarea"), out input);

                if (!found)
                    throw new NoSuchElementException($"Field with name {field} does not exist.");

                Field.SetInputValue(driver, input, value);

                return true;
            });
        }

        internal static void ClearFieldValue(WebClient client, IWebElement field)
        {
            if (field.GetAttribute("value").Length > 0)
            {
                field.SendKeys(Keys.Control + "a");
                field.SendKeys(Keys.Backspace);
            }

            client.ThinkTime(500);
        }
    }
}
