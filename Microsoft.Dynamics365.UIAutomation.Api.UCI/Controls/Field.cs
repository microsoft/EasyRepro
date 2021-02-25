// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    /// Represents an individual field on a Dynamics 365 Customer Experience web form.
    /// </summary>
    public class Field
    {
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
                if (containerElement.HasElement(By.XPath(AppElements.Xpath[AppReference.Field.ReadOnly])))
                {
                    var readOnly = containerElement.FindElement(By.XPath(AppElements.Xpath[AppReference.Field.ReadOnly]));

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
                    var lookupRecordList = containerElement.FindElement(By.XPath(AppElements.Xpath[AppReference.Lookup.RecordList]));
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
                if (containerElement.HasElement(By.XPath(AppElements.Xpath[AppReference.Field.RequiredIcon])))
                {
                    var required = containerElement.FindElement(By.XPath(AppElements.Xpath[AppReference.Field.RequiredIcon]));

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
    }
}
