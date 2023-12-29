// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.Entity;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    /// Represents an Option Set in Dynamics 365.
    /// </summary>
    public class OptionSet
    {
        public string Name { get; set; }
        public string Value { get; set; }

        internal static BrowserCommandResult<bool> SetValue(WebClient client, OptionSet control, FormContextType formContextType)
        {
            var controlName = control.Name;
            return client.Execute(client.GetOptions($"Set OptionSet Value: {controlName}"), driver =>
            {
                IWebElement fieldContainer = null;
                fieldContainer = client.ValidateFormContext(driver, formContextType, controlName, fieldContainer);

                OptionSet.TrySetValue(client,fieldContainer, control);
                driver.WaitForTransaction();
                return true;
            });
        }
        internal BrowserCommandResult<bool> ClearValue(WebClient client,OptionSet control, FormContextType formContextType)
        {
            return client.Execute(client.GetOptions($"Clear Field {control.Name}"), driver =>
            {
                control.Value = "-1";
                SetValue(client, control, formContextType);

                return true;
            });
        }

        internal static void TrySetValue(WebClient client, IWebElement fieldContainer, OptionSet control)
        {
            var value = control.Value;
            bool success = fieldContainer.TryFindElement(By.TagName("select"), out IWebElement select);
            if (success)
            {
                fieldContainer.WaitUntilAvailable(By.TagName("select"));
                var options = select.FindElements(By.TagName("option"));
                SelectOption(options, value);
                return;
            }

            var name = control.Name;
            var hasStatusCombo = fieldContainer.HasElement(By.XPath(EntityReference.EntityOptionsetStatusCombo.Replace("[NAME]", name)));
            if (hasStatusCombo)
            {
                // This is for statuscode (type = status) that should act like an optionset doesn't doesn't follow the same pattern when rendered
                fieldContainer.ClickWhenAvailable(By.XPath(EntityReference.EntityOptionsetStatusComboButton.Replace("[NAME]", name)));

                var listBox = fieldContainer.FindElement(By.XPath(EntityReference.EntityOptionsetStatusComboList.Replace("[NAME]", name)));

                var options = listBox.FindElements(By.TagName("li"));
                SelectOption(options, value);
                return;
            }

            throw new InvalidOperationException($"OptionSet Field: '{name}' does not exist");
        }
    }
}
