// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using static Microsoft.Dynamics365.UIAutomation.Api.Entity;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Represents an Option Set in Dynamics 365.
    /// </summary>
    public class OptionSet
    {
        public string Name { get; set; }
        public string Value { get; set; }
        private static EntityReference _entityReference;
        public OptionSet() {
            _entityReference = new EntityReference();
        }
        public void SelectOption(WebClient client, List<IElement> options, string value)
        {
            var selectedOption = options.FirstOrDefault(op => op.Text == value || op.GetAttribute(client, "value") == value);
            selectedOption.Click(client, true);
        }
        internal static BrowserCommandResult<bool> SetValue(WebClient client, OptionSet control, FormContextType formContextType)
        {
            var controlName = control.Name;
            return client.Execute(client.GetOptions($"Set OptionSet Value: {controlName}"), driver =>
            {
                IElement fieldContainer = null;
                fieldContainer = client.ValidateFormContext(driver, formContextType, controlName, fieldContainer);

                control.TrySetValue(client,fieldContainer, control);
                driver.Wait();
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

        internal void TrySetValue(WebClient client, IElement fieldContainer, OptionSet control)
        {
            var value = control.Value;
            bool success = client.Browser.Browser.HasElement(fieldContainer.Locator + "//select");
            if (success)
            {
                IElement select = client.Browser.Browser.FindElement(fieldContainer.Locator + "//select");
                client.Browser.Browser.WaitUntilAvailable(fieldContainer.Locator + "//select");
                var options = client.Browser.Browser.FindElements(select.Locator + "//option");
                SelectOption(client, options, value);
                return;
            }

            var name = control.Name;
            var hasStatusCombo = client.Browser.Browser.HasElement(fieldContainer.Locator + _entityReference.EntityOptionsetStatusCombo.Replace("[NAME]", name));
            if (hasStatusCombo)
            {
                // This is for statuscode (type = status) that should act like an optionset doesn't doesn't follow the same pattern when rendered
                client.Browser.Browser.ClickWhenAvailable(fieldContainer.Locator + _entityReference.EntityOptionsetStatusComboButton.Replace("[NAME]", name));

                var listBox = client.Browser.Browser.FindElement(fieldContainer.Locator + _entityReference.EntityOptionsetStatusComboList.Replace("[NAME]", name));

                var options = client.Browser.Browser.FindElements(listBox.Locator + "//li");
                SelectOption(client, options, value);
                return;
            }

            throw new InvalidOperationException($"OptionSet Field: '{name}' does not exist");
        }
    }
}
