// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Runtime.CompilerServices;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    /// Represents a Boolean Item in Dynamics 365.
    /// </summary>
    public class BooleanItem : Element
    {
        public string Name { get; set; }
        public bool Value { get; set; }
        public BooleanItem() : base()
        {
        }
        public override void SetValue(BrowserPage client, string value)
        {
            return client.Execute(client.GetOptions($"Set BooleanItem Value: {option.Name}"), driver =>
            {

                // ensure that the option.Name value is lowercase -- will cause XPath lookup issues
                option.Name = option.Name.ToLowerInvariant();

                Element fieldContainer = null;
                fieldContainer = client.ValidateFormContext(driver, formContextType, option.Name, fieldContainer);

                var hasRadio = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name));
                var hasCheckbox = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name));
                var hasList = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name));
                var hasToggle = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name));
                var hasFlipSwitch = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldFlipSwitchLink.Replace("[NAME]", option.Name));

                // Need to validate whether control is FlipSwitch or Button
                Element flipSwitchContainer = null;
                var flipSwitch = hasFlipSwitch ? driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldFlipSwitchContainer.Replace("[NAME]", option.Name)) : false;
                var hasButton = flipSwitchContainer != null ? driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldButtonTrue) : false;
                hasFlipSwitch = hasButton ? false : hasFlipSwitch; //flipSwitch and button have the same container reference, so if it has a button it is not a flipSwitch
                hasFlipSwitch = hasToggle ? false : hasFlipSwitch; //flipSwitch and Toggle have the same container reference, so if it has a Toggle it is not a flipSwitch

                if (hasRadio)
                {
                    var trueRadio = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldRadioTrue.Replace("[NAME]", option.Name));
                    var falseRadio = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldRadioFalse.Replace("[NAME]", option.Name));

                    if (option.Value && bool.Parse(falseRadio.GetAttribute("aria-checked")) || !option.Value && bool.Parse(trueRadio.GetAttribute("aria-checked")))
                    {
                        driver.ClickWhenAvailable(client.ElementMapper.EntityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name));
                    }
                }
                else if (hasCheckbox)
                {
                    var checkbox = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name));

                    if (option.Value && !checkbox.Selected || !option.Value && checkbox.Selected)
                    {
                        driver.ClickWhenAvailable(client.ElementMapper.EntityReference.EntityBooleanFieldCheckboxContainer.Replace("[NAME]", option.Name));
                    }
                }
                else if (hasList)
                {
                    var list = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name));
                    var options = list.FindElements(client.ElementMapper.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name) + "//option");
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
                            driver.ClickWhenAvailable(unselectedOption.GetAttribute("id"));
                        }
                    }
                }
                else if (hasToggle)
                {
                    var toggle = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name));
                    var link = toggle.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name) + "//button");
                    var value = bool.Parse(link.GetAttribute("aria-checked"));

                    if (value != option.Value)
                    {
                        link.Click();
                    }
                }
                else if (hasFlipSwitch)
                {
                    // flipSwitchContainer should exist based on earlier TryFindElement logic
                    var link = flipSwitchContainer.FindElement("//a");
                    var value = bool.Parse(link.GetAttribute("aria-checked"));

                    if (value != option.Value)
                    {
                        link.Click();
                    }
                }
                else if (hasButton)
                {
                    var container = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldButtonContainer.Replace("[NAME]", option.Name));
                    var trueButton = container.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldButtonTrue);
                    var falseButton = container.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldButtonFalse);

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
        /// <summary>
        /// Sets the value of a Boolean Item.
        /// </summary>
        /// <param name="option">The boolean field name.</param>
        /// <example>xrmApp.Entity.SetValue(new BooleanItem { Name = "donotemail", Value = true });</example>
        internal BrowserCommandResult<bool> SetValue(WebClient client, BooleanItem option, FormContextType formContextType)
        {
            return client.Execute(client.GetOptions($"Set BooleanItem Value: {option.Name}"), driver =>
            {
                
                // ensure that the option.Name value is lowercase -- will cause XPath lookup issues
                option.Name = option.Name.ToLowerInvariant();

                Element fieldContainer = null;
                fieldContainer = client.ValidateFormContext(driver, formContextType, option.Name, fieldContainer);

                var hasRadio = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name));
                var hasCheckbox = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name));
                var hasList = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name));
                var hasToggle = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name));
                var hasFlipSwitch = driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldFlipSwitchLink.Replace("[NAME]", option.Name));

                // Need to validate whether control is FlipSwitch or Button
                Element flipSwitchContainer = null;
                var flipSwitch = hasFlipSwitch ? driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldFlipSwitchContainer.Replace("[NAME]", option.Name)) : false;
                var hasButton = flipSwitchContainer != null ? driver.HasElement(client.ElementMapper.EntityReference.EntityBooleanFieldButtonTrue) : false;
                hasFlipSwitch = hasButton ? false : hasFlipSwitch; //flipSwitch and button have the same container reference, so if it has a button it is not a flipSwitch
                hasFlipSwitch = hasToggle ? false : hasFlipSwitch; //flipSwitch and Toggle have the same container reference, so if it has a Toggle it is not a flipSwitch

                if (hasRadio)
                {
                    var trueRadio = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldRadioTrue.Replace("[NAME]", option.Name));
                    var falseRadio = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldRadioFalse.Replace("[NAME]", option.Name));

                    if (option.Value && bool.Parse(falseRadio.GetAttribute("aria-checked")) || !option.Value && bool.Parse(trueRadio.GetAttribute("aria-checked")))
                    {
                        driver.ClickWhenAvailable(client.ElementMapper.EntityReference.EntityBooleanFieldRadioContainer.Replace("[NAME]", option.Name));
                    }
                }
                else if (hasCheckbox)
                {
                    var checkbox = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldCheckbox.Replace("[NAME]", option.Name));

                    if (option.Value && !checkbox.Selected || !option.Value && checkbox.Selected)
                    {
                        driver.ClickWhenAvailable(client.ElementMapper.EntityReference.EntityBooleanFieldCheckboxContainer.Replace("[NAME]", option.Name));
                    }
                }
                else if (hasList)
                {
                    var list = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name));
                    var options = list.FindElements(client.ElementMapper.EntityReference.EntityBooleanFieldList.Replace("[NAME]", option.Name)+"//option");
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
                            driver.ClickWhenAvailable(unselectedOption.GetAttribute("id"));
                        }
                    }
                }
                else if (hasToggle)
                {
                    var toggle = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name));
                    var link = toggle.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldToggle.Replace("[NAME]", option.Name) + "//button");
                    var value = bool.Parse(link.GetAttribute("aria-checked"));

                    if (value != option.Value)
                    {
                        link.Click();
                    }
                }
                else if (hasFlipSwitch)
                {
                    // flipSwitchContainer should exist based on earlier TryFindElement logic
                    var link = flipSwitchContainer.FindElement("//a");
                    var value = bool.Parse(link.GetAttribute("aria-checked"));

                    if (value != option.Value)
                    {
                        link.Click();
                    }
                }
                else if (hasButton)
                {
                    var container = fieldContainer.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldButtonContainer.Replace("[NAME]", option.Name));
                    var trueButton = container.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldButtonTrue);
                    var falseButton = container.FindElement(client.ElementMapper.EntityReference.EntityBooleanFieldButtonFalse);

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
