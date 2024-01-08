// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.BusinessProcessFlow;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.Entity;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.QuickCreate;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    /// Represents a DateTime field in Dynamics 365.
    /// </summary>
    public class DateTimeControl
    {
        public DateTimeControl(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public DateTime? Value { get; set; }

        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }

        private string _dateAsString;
        public string DateAsString
        {
            get => _dateAsString ?? (_dateAsString = string.IsNullOrWhiteSpace(DateFormat) ? Value?.ToShortDateString() : Value?.ToString(DateFormat));
            set => _dateAsString = value;
        }

        private string _timeAsString;
        public string TimeAsString
        {
            get => _timeAsString ?? (_timeAsString = string.IsNullOrWhiteSpace(TimeFormat) ? Value?.ToShortTimeString()?.ToUpper() : Value?.ToString(TimeFormat))?.ToUpper();
            set => _timeAsString = value;
        }

        public static DateTime? TryGetValue(WebClient client, ISearchContext container, DateTimeControl control)
        {
            string field = control.Name;
            client.Browser.Driver.WaitForTransaction();
            var xpathToDateField = By.XPath(client.ElementMapper.EntityReference.FieldControlDateTimeInputUCI.Replace("[FIELD]", field));

            var dateField = container.WaitUntilAvailable(xpathToDateField, $"Field: {field} Does not exist");
            string strDate = dateField.GetAttribute("value");
            if (strDate.IsEmptyValue())
                return null;

            var date = DateTime.Parse(strDate);

            // Try get Time
            var timeFieldXPath = By.XPath(client.ElementMapper.EntityReference.FieldControlDateTimeTimeInputUCI.Replace("[FIELD]", field));
            bool success = container.TryFindElement(timeFieldXPath, out var timeField);
            if (!success || timeField == null)
                return date;

            string strTime = timeField.GetAttribute("value");
            if (strTime.IsEmptyValue())
                return date;

            var time = DateTime.Parse(strTime);

            var result = date.AddHours(time.Hour).AddMinutes(time.Minute).AddSeconds(time.Second);

            return result;
        }
        internal static void TrySetTime(WebClient client, ISearchContext container, DateTimeControl control, FormContextType formContextType)
        {
            By timeFieldXPath = By.XPath(client.ElementMapper.EntityReference.FieldControlDateTimeTimeInputUCI.Replace("[FIELD]", control.Name));

            IWebElement formContext = null;

            if (formContextType == FormContextType.QuickCreate)
            {
                //IWebDriver formContext;
                // Initialize the quick create form context
                // If this is not done -- element input will go to the main form due to new flyout design
                formContext = container.WaitUntilAvailable(By.XPath(QuickCreateReference.QuickCreateFormContext), new TimeSpan(0, 0, 1));
            }
            else if (formContextType == FormContextType.Entity)
            {
                // Initialize the entity form context
                formContext = container.WaitUntilAvailable(By.XPath(client.ElementMapper.EntityReference.FormContext), new TimeSpan(0, 0, 1));
            }
            else if (formContextType == FormContextType.BusinessProcessFlow)
            {
                // Initialize the Business Process Flow context
                formContext = container.WaitUntilAvailable(By.XPath(client.ElementMapper.BusinessProcessFlowReference.BusinessProcessFlowFormContext), new TimeSpan(0, 0, 1));
            }
            else if (formContextType == FormContextType.Header)
            {
                // Initialize the Header context
                formContext = container as IWebElement;
            }
            else if (formContextType == FormContextType.Dialog)
            {
                // Initialize the Header context
                formContext = container.WaitUntilAvailable(By.XPath(client.ElementMapper.DialogsReference.DialogContext), new TimeSpan(0, 0, 1));
            }

            client.Browser.Driver.WaitForTransaction();

            if (formContext.TryFindElement(timeFieldXPath, out var timeField))
            {
                TrySetTime(client.Browser.Driver,container, timeField, control.TimeAsString);
            }
        }
        internal static void TrySetTime(IWebDriver driver,ISearchContext container, IWebElement timeField, string time)
        {
            // click & wait until the time get updated after change/clear the date
            timeField.Click();
            driver.WaitForTransaction();

            driver.RepeatUntil(() =>
            {
                timeField.Clear();
                timeField.Click();
                timeField.SendKeys(time);
                timeField.SendKeys(Keys.Tab);
                driver.WaitForTransaction();
            },
            d => timeField.GetAttribute("value").IsValueEqualsTo(time),
            TimeSpan.FromSeconds(9), 3,
            failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {time}. Actual: {timeField.GetAttribute("value")}")
            );
        }
        internal void TrySetDateValue(IWebDriver driver, WebClient client, IWebElement dateField, string date, FormContextType formContextType)
        {
            var strExpanded = dateField.GetAttribute("aria-expanded");

            if (strExpanded != null)
            {
                bool success = bool.TryParse(strExpanded, out var isCalendarExpanded);
                if (success && isCalendarExpanded)
                    dateField.Click(); // close calendar

                driver.RepeatUntil(() =>
                {
                    Field.ClearFieldValue(client, dateField);
                    if (date != null)
                    {
                        dateField.SendKeys(date);
                        dateField.SendKeys(Keys.Tab);
                    }
                },
                d => dateField.GetAttribute("value").IsValueEqualsTo(date),
                TimeSpan.FromSeconds(9), 3,
                failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {date}. Actual: {dateField.GetAttribute("value")}")
                );
            }
            else
            {
                driver.RepeatUntil(() =>
                {
                    dateField.Click(true);
                    if (date != null)
                    {
                        dateField = dateField.FindElement(By.TagName("input"));

                        // Only send Keys.Escape to avoid element not interactable exceptions with calendar flyout on forms.
                        // This can cause the Header or BPF flyouts to close unexpectedly
                        if (formContextType == FormContextType.Entity || formContextType == FormContextType.QuickCreate)
                        {
                            dateField.SendKeys(Keys.Escape);
                        }

                        Field.ClearFieldValue(client, dateField);
                        dateField.SendKeys(date);
                    }
                },
                    d => dateField.GetAttribute("value").IsValueEqualsTo(date),
                    TimeSpan.FromSeconds(9), 3,
                    failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {date}. Actual: {dateField.GetAttribute("value")}")
                );
            }
        }

        private void TrySetDateValue(IWebDriver driver, WebClient client, ISearchContext container, DateTimeControl control, FormContextType formContextType)
        {
            string controlName = control.Name;
            IWebElement fieldContainer = null;
            var xpathToInput = By.XPath(client.ElementMapper.EntityReference.FieldControlDateTimeInputUCI.Replace("[FIELD]", controlName));

            if (formContextType == FormContextType.QuickCreate)
            {
                // Initialize the quick create form context
                // If this is not done -- element input will go to the main form due to new flyout design
                var formContext = container.WaitUntilAvailable(By.XPath(QuickCreateReference.QuickCreateFormContext));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(client.ElementMapper.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }
            }
            else if (formContextType == FormContextType.Entity)
            {
                // Initialize the entity form context
                var formContext = container.WaitUntilAvailable(By.XPath(client.ElementMapper.EntityReference.FormContext));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(client.ElementMapper.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }
            }
            else if (formContextType == FormContextType.BusinessProcessFlow)
            {
                // Initialize the Business Process Flow context
                var formContext = driver.WaitUntilAvailable(By.XPath(client.ElementMapper.BusinessProcessFlowReference.BusinessProcessFlowFormContext));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(client.ElementMapper.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }
            }
            else if (formContextType == FormContextType.Header)
            {
                // Initialize the Header context
                var formContext = driver.WaitUntilAvailable(By.XPath(client.ElementMapper.EntityReference.HeaderContext));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(client.ElementMapper.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }
            }
            else if (formContextType == FormContextType.Dialog)
            {
                // Initialize the Dialog context
                var formContext = driver.WaitUntilAvailable(By.XPath(client.ElementMapper.DialogsReference.DialogContext));
                fieldContainer = formContext.WaitUntilAvailable(xpathToInput, $"DateTime Field: '{controlName}' does not exist");

                var strExpanded = fieldContainer.GetAttribute("aria-expanded");

                if (strExpanded == null)
                {
                    fieldContainer = formContext.FindElement(By.XPath(client.ElementMapper.EntityReference.TextFieldContainer.Replace("[NAME]", controlName)));
                }

            }

            TrySetDateValue(driver,client, fieldContainer, control.DateAsString, formContextType);
        }
        internal BrowserCommandResult<bool> ClearValue(WebClient client, DateTimeControl control, FormContextType formContextType)
            => client.Execute(client.GetOptions($"Clear Field: {control.Name}"),
             driver => TrySetValue(client, container: driver, control: new DateTimeControl(control.Name), formContextType)); // Pass an empty control

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="value">DateTime value.</param>
        /// <param name="formatDate">Datetime format matching Short Date formatting personal options.</param>
        /// <param name="formatTime">Datetime format matching Short Time formatting personal options.</param>
        /// <example>xrmApp.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        /// <example>xrmApp.Entity.SetValue("new_actualclosedatetime", DateTime.Now, "MM/dd/yyyy", "hh:mm tt");</example>
        /// <example>xrmApp.Entity.SetValue("estimatedclosedate", DateTime.Now);</example>
        public static BrowserCommandResult<bool> SetValue(WebClient client, string field, DateTime value, FormContextType formContext, string formatDate = null, string formatTime = null)
        {
            var control = new DateTimeControl(field)
            {
                Value = value,
                DateFormat = formatDate,
                TimeFormat = formatTime
            };
            return control.SetValue(client, control, formContext);
        }

        public BrowserCommandResult<bool> SetValue(WebClient client, DateTimeControl control, FormContextType formContext)
            => client.Execute(client.GetOptions($"Set Date/Time Value: {control.Name}"),
                driver => TrySetValue(client, container: driver, control: control, formContext));
        internal bool TrySetValue(WebClient client, ISearchContext container, DateTimeControl control, FormContextType formContext)
        {
            this.TrySetDateValue(client.Browser.Driver,client, container, control, formContext);
            DateTimeControl.TrySetTime(client, container, control, formContext);

            if (formContext == FormContextType.Header)
            {
                Entity entity = new Entity(client);
                entity.TryCloseHeaderFlyout(client.Browser.Driver);
            }

            return true;
        }

        internal BrowserCommandResult<bool> EntitySetHeaderValue(WebClient client,Entity entity, DateTimeControl control)
        {
            return client.Execute(client.GetOptions($"Set Header Date/Time Value: {control.Name}"), driver => TrySetHeaderValue(client,entity, control));
        }

        internal BrowserCommandResult<bool> EntityClearHeaderValue(WebClient client, Entity entity, DateTimeControl control)
        {
            var controlName = control.Name;
            return client.Execute(client.GetOptions($"Clear Header Date/Time Value: {controlName}"),
                driver => TrySetHeaderValue(client,entity, new DateTimeControl(controlName)));
        }

        private bool TrySetHeaderValue(WebClient client,Entity entity, DateTimeControl control)
        {
            var xpathToContainer = client.ElementMapper.EntityReference.HeaderDateTimeFieldContainer.Replace("[NAME]", control.Name);
            return entity.ExecuteInHeaderContainer(client.Browser.Driver, xpathToContainer,
                container => control.TrySetValue(client, container, control, FormContextType.Header));
        }
    }
}
