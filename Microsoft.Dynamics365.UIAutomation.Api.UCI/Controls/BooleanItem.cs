// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    /// Represents a Boolean Item in Dynamics 365.
    /// </summary>
    public class BooleanItem
    {
        public string Name { get; set; }
        public bool Value { get; set; }

        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        /// <example>xrmApp.Entity.SetValue(new BooleanItem { Name = "donotemail", Value = true });</example>
        internal static BrowserCommandResult<bool> SetValue(WebClient client, BooleanItem option, FormContextType formContextType)
        {
            return client.Execute(client.GetOptions($"Set BooleanItem Value: {option.Name}"), driver =>
            {
                // ensure that the option.Name value is lowercase -- will cause XPath lookup issues
                option.Name = option.Name.ToLowerInvariant();

                IWebElement fieldContainer = null;
                fieldContainer = client.ValidateFormContext(driver, formContextType, option.Name, fieldContainer);

                var hasRadio = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name)));
                var hasCheckbox = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name)));
                var hasList = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name)));
                var hasToggle = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name)));
                var hasFlipSwitch = fieldContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldFlipSwitchLink.Replace("[NAME]", option.Name)));

                // Need to validate whether control is FlipSwitch or Button
                IWebElement flipSwitchContainer = null;
                var flipSwitch = hasFlipSwitch ? fieldContainer.TryFindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldFlipSwitchContainer.Replace("[NAME]", option.Name)), out flipSwitchContainer) : false;
                var hasButton = flipSwitchContainer != null ? flipSwitchContainer.HasElement(By.XPath(Entity.EntityReference.EntityBooleanFieldButtonTrue)) : false;
                hasFlipSwitch = hasButton ? false : hasFlipSwitch; //flipSwitch and button have the same container reference, so if it has a button it is not a flipSwitch
                hasFlipSwitch = hasToggle ? false : hasFlipSwitch; //flipSwitch and Toggle have the same container reference, so if it has a Toggle it is not a flipSwitch

                if (hasRadio)
                {
                    var trueRadio = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldRadioTrue.Replace("[NAME]", option.Name)));
                    var falseRadio = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldRadioFalse.Replace("[NAME]", option.Name)));

                    if (option.Value && bool.Parse(falseRadio.GetAttribute("aria-checked")) || !option.Value && bool.Parse(trueRadio.GetAttribute("aria-checked")))
                    {
                        driver.ClickWhenAvailable(By.XPath(Entity.EntityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name)));
                    }
                }
                else if (hasCheckbox)
                {
                    var checkbox = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name)));

                    if (option.Value && !checkbox.Selected || !option.Value && checkbox.Selected)
                    {
                        driver.ClickWhenAvailable(By.XPath(Entity.EntityReference.EntityBooleanFieldCheckboxContainer.Replace("[NAME]", option.Name)));
                    }
                }
                else if (hasList)
                {
                    var list = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name)));
                    var options = list.FindElements(By.TagName("option"));
                    var selectedOption = options.FirstOrDefault(a => a.HasAttribute("data-selected") && bool.Parse(a.GetAttribute("data-selected")));
                    var unselectedOption = options.FirstOrDefault(a => !a.HasAttribute("data-selected"));

                    var trueOptionSelected = false;
                    if (selectedOption != null)
                    {
                        trueOptionSelected = selectedOption.GetAttribute("value") == "1";
                    }

                    if (option.Value && !trueOptionSelected || !option.Value && trueOptionSelected)
                    {
                        if (unselectedOption != null)
                        {
                            driver.ClickWhenAvailable(By.Id(unselectedOption.GetAttribute("id")));
                        }
                    }
                }
                else if (hasToggle)
                {
                    var toggle = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name)));
                    var link = toggle.FindElement(By.TagName("button"));
                    var value = bool.Parse(link.GetAttribute("aria-checked"));

                    if (value != option.Value)
                    {
                        link.Click();
                    }
                }
                else if (hasFlipSwitch)
                {
                    // flipSwitchContainer should exist based on earlier TryFindElement logic
                    var link = flipSwitchContainer.FindElement(By.TagName("a"));
                    var value = bool.Parse(link.GetAttribute("aria-checked"));

                    if (value != option.Value)
                    {
                        link.Click();
                    }
                }
                else if (hasButton)
                {
                    var container = fieldContainer.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldButtonContainer.Replace("[NAME]", option.Name)));
                    var trueButton = container.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldButtonTrue));
                    var falseButton = container.FindElement(By.XPath(Entity.EntityReference.EntityBooleanFieldButtonFalse));

                    if (option.Value)
                    {
                        trueButton.Click();
                    }
                    else
                    {
                        falseButton.Click();
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");


                return true;
            });
        }
    }
}
