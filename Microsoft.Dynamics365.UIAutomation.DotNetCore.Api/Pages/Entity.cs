// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    ///  Xrm Entity page.
    ///  </summary>
    public class Entity
        : XrmPage 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>              
        public Entity(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToContent();
        }

        private readonly string _navigateDownCssSelector = "img.recnav-down.ms-crm-ImageStrip-Down_Enabled_proxy";
        private readonly string _navigateUpCssSelector = "img.recnav-up.ms-crm-ImageStrip-Up_Enabled_proxy";

        /// <summary>
        /// Clears the value of a Text field on an Entity header
        /// </summary>
        /// <param name="field">The field</param>
        /// <example>xrmBrowser.Entity.ClearHeaderValue("firstname", "Test");</example>
        public BrowserCommandResult<bool> ClearHeaderValue(string field)
        {
            return this.Execute(GetOptions($"Clear Text Field Header Value: {field}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Header].Replace("[NAME]", field.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Header].Replace("[NAME]", field.ToLower())));

                    fieldElement.Click(true);

                    if (fieldElement.FindElements(By.TagName("input")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("input")).Clear();
                        fieldElement.SendKeys(Keys.Tab);
                    }
                    else if (fieldElement.TagName == "textarea")
                    {
                        fieldElement.Clear();
                        fieldElement.SendKeys(Keys.Tab);
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate field '{field}' in the header. Please verify the field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Checkbox/TwoOption field on an Entity header
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.Entity.ClearHeaderValue(new TwoOption{ Name = "creditonhold"});</example>
        public BrowserCommandResult<bool> ClearHeaderValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Clear Checkbox/TwoOption Header Value: {option.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer_Header].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer_Header].Replace("[NAME]", option.Name.ToLower())));

                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var select = fieldElement;
                        var label = fieldElement.FindElement(By.TagName("label")).Text;

                        if (fieldElement.TagName != "select")
                            select = fieldElement.FindElement(By.TagName("select"));

                        var options = select.FindElements(By.TagName("option"));

                        foreach (var op in options)
                        {
                            if (((op.Text.ToLower() != label.ToLower() || op.GetAttribute("title").ToLower() != label.ToLower()) && op.GetAttribute("value") == "0"))
                            {
                                fieldElement.Click(true);
                            }
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' in the header. Please verify the TwoOption field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of an option set / picklist on an Entity header
        /// </summary>
        /// <param name="option">The option you want to clear.</param>
        /// <example>xrmBrowser.Entity.ClearHeaderValue(new OptionSet { Name = "preferredcontactmethodcode"});</example>
        public BrowserCommandResult<bool> ClearHeaderValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Clear OptionSet Header Value: {option.Name}"), driver =>
            {

                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Header].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Header].Replace("[NAME]", option.Name.ToLower())));
                    var select = fieldElement;

                    if (fieldElement.TagName != "select")
                        select = fieldElement.FindElement(By.TagName("select"));

                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.GetAttribute("value") == "")
                        {
                            fieldElement.Click(true);
                            op.Click(true);
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate OptionSet '{option.Name}' in the header. Please verify the OptionSet exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Lookup on an Entity header
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.Entity.ClearHeaderValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        public BrowserCommandResult<bool> ClearHeaderValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Clear Lookup Header Value: {control.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", control.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", control.Name.ToLower())));

                    if (fieldElement.Text != "")
                    {
                        fieldElement.Hover(driver, true);

                        if (fieldElement.FindElement(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon_Header].Replace("[NAME]", control.Name.ToLower()))) == null)
                            throw new InvalidOperationException($"Field: {control.Name} is not Lookup control");

                        driver.Manage().Window.Maximize();
                        var lookupSearch = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon_Header].Replace("[NAME]", control.Name.ToLower())));

                        if (!lookupSearch.Displayed)
                        {
                            driver.Manage().Window.Minimize();
                            driver.Manage().Window.Maximize();
                            fieldElement.Hover(driver, true);
                            lookupSearch = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon_Header].Replace("[NAME]", control.Name.ToLower())));
                        }

                        lookupSearch.Click(true);

                        var dialogName = $"Dialog_header_{control.Name}_IMenu";
                        var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

                        var dialogItems = OpenDialog(dialog).Value;

                        if (dialogItems.Any())
                        {
                            var dialogItem = dialogItems.Last();
                            dialogItem.Element.Click();
                        }

                        SwitchToDialog();

                        driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.LookUp.Remove])).Click(true);

                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate Lookup '{control.Name}' in the header. Please verify the Lookup exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a DateTime field on an Entity header.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.Entity.ClearHeaderValue(new DateTime {Name = "birthdate"}));</example>
        public BrowserCommandResult<bool> ClearHeaderValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Clear DateTime Header Field: {date.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer_Header].Replace("[NAME]", date.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer_Header].Replace("[NAME]", date.Name.ToLower())));

                    // Check whether the DateTime field has an existing value
                    if (fieldElement.GetAttribute("title") != "Select to enter data")
                    {
                        fieldElement.Click(true);
                        var fieldInput = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.DateFieldInput_Header].Replace("[NAME]", date.Name.ToLower())));
                        // Clear any existing values
                        fieldInput.Clear();
                        fieldElement.Click(true);
                        fieldElement.SendKeys(Keys.Enter);
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' in the header. Please verify the DateTime field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Placeholder.
        /// </summary>
        /// <param name="option">The option you want to clear.</param>
        /// <example>xrmBrowser.Entity.ClearHeaderValue(new OptionSet { Name = "preferredcontactmethodcode"});</example>
        public BrowserCommandResult<bool> ClearHeaderValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Clear MultiValueOptionSet Header Value: {option.Name}"), driver =>
            {

                /*
                driver.WaitUntilVisible(By.Id(option.Name));

                if (driver.HasElement(By.Id(option.Name)))
                {
                    var container = driver.ClickWhenAvailable(By.Id(option.Name));

                    if (removeExistingValues)
                    {
                        //Remove Existing Values
                        var values = container.FindElements(By.ClassName(Elements.CssClass[Reference.SetValue.MultiSelectPicklistDeleteClass]));
                        foreach (var value in values)
                            value.Click(true);
                    }

                    var input = container.FindElement(By.TagName("input"));
                    input.Click();
                    input.SendKeys(" ");

                    var options = container.FindElements(By.TagName("li"));

                    foreach (var op in options)
                    {
                        var label = op.FindElement(By.TagName("label"));

                        if (option.Values.Contains(op.Text) || option.Values.Contains(op.GetAttribute("value")) || option.Values.Contains(label.GetAttribute("title")))
                            op.Click(true);
                    }

                    container.Click();
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                */

                return true;
            });
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="control">The Composite control values you want to clear.</param>
        /// <example>xrmBrowser.Entity.ClearHeaderValue(new CompositeControl() {Id = "fullname"});</example>
        public BrowserCommandResult<bool> ClearHeaderValue(CompositeControl control)
        {
            return this.Execute(GetOptions($"Clear ConpositeControl Header Value: {control.Id}"), driver =>
            {
                driver.WaitUntilVisible(By.Id(control.Id));

                if (!driver.HasElement(By.Id(control.Id)))
                    return false;

                driver.ClickWhenAvailable(By.Id(control.Id));

                if (driver.HasElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut])))
                {
                    var compcntrl =
                        driver.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut]));

                    foreach (var field in control.Fields)
                    {
                        compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id)).Click(true);

                        var result = compcntrl.FindElements(By.TagName("input"))
                            .ToList()
                            .FirstOrDefault(i => i.GetAttribute("id").Contains(field.Id));

                        result?.Clear();
                        result?.SendKeys(Keys.Tab);

                        if (compcntrl.IsVisible(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id + "_warnSpan")))
                        {
                            throw new InvalidOperationException($"The field {field.Id} has displayed a warning and cannot be cleared.");
                        }
                    }

                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click();

                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Text field on an Entity form
        /// </summary>
        /// <param name="field">The field</param>
        /// <example>xrmBrowser.Entity.ClearValue("firstname", "Test");</example>
        public BrowserCommandResult<bool> ClearValue(string field)
        {
            return this.Execute(GetOptions($"Clear Text Field Value: {field}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer].Replace("[NAME]", field.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer].Replace("[NAME]", field.ToLower())));

                    fieldElement.Click(true);

                    if (fieldElement.FindElements(By.TagName("input")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("input")).Clear();
                    }
                    else if (fieldElement.TagName == "textarea")
                    {
                        fieldElement.Clear();
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate field '{field}' on the form. Please verify the field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Checkbox/TwoOption field on an Entity form
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.Entity.ClearValue(new TwoOption{ Name = "creditonhold"});</example>
        public BrowserCommandResult<bool> ClearValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Clear Checkbox/TwoOption Value: {option.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));

                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var select = fieldElement;
                        var label = fieldElement.FindElement(By.TagName("label")).Text;

                        if (fieldElement.TagName != "select")
                            select = fieldElement.FindElement(By.TagName("select"));

                        var options = select.FindElements(By.TagName("option"));

                        foreach (var op in options)
                        {
                            if (((op.Text.ToLower() != label.ToLower() || op.GetAttribute("title").ToLower() != label.ToLower()) && op.GetAttribute("value") == "0"))
                            {
                                fieldElement.Click(true);
                            }
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' on the form. Please verify the TwoOption field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of an option set / picklist on an Entity form
        /// </summary>
        /// <param name="option">The option you want to clear.</param>
        /// <example>xrmBrowser.Entity.ClearValue(new OptionSet { Name = "preferredcontactmethodcode"});</example>
        public BrowserCommandResult<bool> ClearValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Clear OptionSet Value: {option.Name}"), driver =>
            {

                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));
                    var select = fieldElement;

                    if (fieldElement.TagName != "select")
                        select = fieldElement.FindElement(By.TagName("select"));

                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.GetAttribute("value") == "")
                        {
                            fieldElement.Click(true);
                            op.Click(true);
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate OptionSet '{option.Name}' on the form. Please verify the OptionSet exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Lookup on an Entity form
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.Entity.ClearValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        public BrowserCommandResult<bool> ClearValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Clear Lookup Value: {control.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower())));

                    if (fieldElement.Text != "")
                    {
                        fieldElement.Hover(driver, true);

                        if (fieldElement.FindElement(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", control.Name.ToLower()))) == null)
                            throw new InvalidOperationException($"Field: {control.Name} is not Lookup control");

                        driver.Manage().Window.Maximize();
                        var lookupSearch = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", control.Name.ToLower())));

                        if (!lookupSearch.Displayed)
                        {
                            driver.Manage().Window.Minimize();
                            driver.Manage().Window.Maximize();
                            fieldElement.Hover(driver, true);
                            lookupSearch = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", control.Name.ToLower())));
                        }

                        lookupSearch.Click(true);

                        var dialogName = $"Dialog_{control.Name}_IMenu";
                        var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

                        var dialogItems = OpenDialog(dialog).Value;

                        if (dialogItems.Any())
                        {
                            var dialogItem = dialogItems.Last();
                            dialogItem.Element.Click();
                        }

                        SwitchToDialog();

                        driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.LookUp.Remove])).Click(true);

                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate Lookup '{control.Name}' on the form. Please verify the Lookup exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a DateTime field on an Entity form.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.Entity.ClearValue(new DateTime {Name = "birthdate"}));</example>
        public BrowserCommandResult<bool> ClearValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Clear DateTime Field: {date.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer].Replace("[NAME]", date.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer].Replace("[NAME]", date.Name.ToLower())));

                    // Check whether the DateTime field has an existing value
                    if (fieldElement.GetAttribute("title") != "Select to enter data")
                    {
                        fieldElement.Click(true);
                        var fieldInput = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.DateFieldInput].Replace("[NAME]", date.Name.ToLower())));
                        // Clear any existing values
                        fieldInput.Clear();
                        fieldElement.Click(true);
                        fieldElement.SendKeys(Keys.Enter);
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' on the form. Please verify the DateTime field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Placeholder: MultiValueOptionSets are not currently supported in BPFs.
        /// </summary>
        /// <param name="option">The option you want to clear.</param>
        /// <example>xrmBrowser.Entity.ClearValue(new OptionSet { Name = "preferredcontactmethodcode"});</example>
        public BrowserCommandResult<bool> ClearValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Clear MultiValueOptionSet Value: {option.Name}"), driver =>
            {

                /*
                driver.WaitUntilVisible(By.Id(option.Name));

                if (driver.HasElement(By.Id(option.Name)))
                {
                    var container = driver.ClickWhenAvailable(By.Id(option.Name));

                    if (removeExistingValues)
                    {
                        //Remove Existing Values
                        var values = container.FindElements(By.ClassName(Elements.CssClass[Reference.SetValue.MultiSelectPicklistDeleteClass]));
                        foreach (var value in values)
                            value.Click(true);
                    }

                    var input = container.FindElement(By.TagName("input"));
                    input.Click();
                    input.SendKeys(" ");

                    var options = container.FindElements(By.TagName("li"));

                    foreach (var op in options)
                    {
                        var label = op.FindElement(By.TagName("label"));

                        if (option.Values.Contains(op.Text) || option.Values.Contains(op.GetAttribute("value")) || option.Values.Contains(label.GetAttribute("title")))
                            op.Click(true);
                    }

                    container.Click();
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                */

                return true;
            });
        }

        /// <summary>
        /// Placeholder: CompositeControls are not currently supported in BPFs.
        /// </summary>
        /// <param name="control">The Composite control values you want to clear.</param>
        /// <example>xrmBrowser.Entity.ClearValue(new CompositeControl {Id = "fullname"});</example>
        public BrowserCommandResult<bool> ClearValue(CompositeControl control)
        {
            return this.Execute(GetOptions($"Clear ConpositeControl Value: {control.Id}"), driver =>
            {
                driver.WaitUntilVisible(By.Id(control.Id));

                if (!driver.HasElement(By.Id(control.Id)))
                    return false;

                driver.ClickWhenAvailable(By.Id(control.Id));

                if (driver.HasElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut])))
                {
                    var compcntrl =
                        driver.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut]));

                    foreach (var field in control.Fields)
                    {
                        compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id)).Click(true);

                        var result = compcntrl.FindElements(By.TagName("input"))
                            .ToList()
                            .FirstOrDefault(i => i.GetAttribute("id").Contains(field.Id));

                        result?.Clear();
                        result?.SendKeys(Keys.Tab);

                        if (compcntrl.IsVisible(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id + "_warnSpan")))
                        {
                            throw new InvalidOperationException($"The field {field.Id} has displayed a warning and cannot be cleared.");
                        }
                    }

                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click();

                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Click add button of subgridName
        /// </summary>
        /// <param name="subgridName">The SubgridName</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.ClickSubgridAddButton("Stakeholders");</example>
        public BrowserCommandResult<bool> ClickSubgridAddButton(string subgridName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Click add button of subgrid: {subgridName}"), driver =>
            {
                driver.FindElement(By.Id($"{subgridName}_addImageButton"))?.Click();

                return true;
            });
        }

        /// <summary>
        /// Click GridView button of subgridName
        /// </summary>
        /// <param name="subgridName">The subgridName</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> ClickSubgridGridViewButton(string subgridName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Click GridView button of subgrid: {subgridName}"), driver =>
            {
                driver.FindElement(By.Id($"{subgridName}_openAssociatedGridViewImageButton"))?.Click();

                return true;
            });
        }

        /// <summary>
        /// Closes the current entity record you are on.
        /// </summary>
        /// <example>xrmBrowser.Entity.CloseEntity();</example>
        public BrowserCommandResult<bool> CloseEntity(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Close Entity"), driver =>
            {
                SwitchToDefault();

                var filter = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.Close]),
                    "Close Buttton is not available");

                filter?.Click();

                return true;
            });
        }

        /// <summary>
        /// Collapses the Tab on a CRM Entity form.
        /// </summary>
        /// <param name="name">The name of the Tab.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.CollapseTab("Summary");</example>
        public BrowserCommandResult<bool> CollapseTab(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Collapse Tab: {name}"), driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name))))
                {
                    throw new InvalidOperationException($"Tab with name '{name}' does not exist.");
                }
                var tab = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name)));

                if (tab.GetAttribute("title").Contains("Collapse"))
                    tab?.Click();

                return true;
            });
        }

        /// <summary>
        /// Dismiss the Alert If Present
        /// </summary>
        /// <param name="stay"></param>
        public BrowserCommandResult<bool> DismissAlertIfPresent(bool stay = false)
        {

            return this.Execute(GetOptions("Dismiss Confirm Save Alert"), driver =>
            {
                if (driver.AlertIsPresent())
                {
                    if (stay)
                        driver.SwitchTo().Alert().Dismiss();
                    else
                        driver.SwitchTo().Alert().Accept();
                }

                return true;
            });
        }

        /// <summary>
        /// Expands the Tab on a CRM Entity form.
        /// </summary>
        /// <param name="name">The name of the Tab.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.ExpandTab("Summary");</example>
        public BrowserCommandResult<bool> ExpandTab(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Expand Tab: {name}"), driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name))))
                {
                    throw new InvalidOperationException($"Tab with name '{name}' does not exist.");
                }
                var tab = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name)));

                if (tab.GetAttribute("title").Contains("Expand"))
                    tab?.Click();

                return true;
            });
        }

        /// <summary>
        /// Gets the value of a Text/Description field on an Entity footer.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <returns>The value</returns>
        /// <example>xrmBrowser.Entity.GetFooterValue("mobilephone");</example>
        public BrowserCommandResult<string> GetFooterValue(string field)
        {
            return this.Execute($"Get Text Field Footer Value: {field}", driver =>
            {
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Footer].Replace("[NAME]", field.ToLower())));

                string text = string.Empty;
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Footer].Replace("[NAME]", field.ToLower()))))
                {
                    var fieldElement = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Footer].Replace("[NAME]", field.ToLower())));

                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        text = fieldElement.FindElement(By.TagName("textarea")).GetAttribute("value");
                    }
                    else
                    {
                        text = fieldElement.FindElement(By.TagName("input")).GetAttribute("value");
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate field '{field}' in the footer. Please verify the field exists and try again.");

                return text;
            });
        }

        /// <summary>
        /// PLACEHOLDER: Gets the value of a Composite control on an Entity footer.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <example>xrmBrowser.Entity.GetFooterValue(new CompositeControl { Id = "fullname", Fields = fields });</example>
        public BrowserCommandResult<string> GetFooterValue(CompositeControl control)
        {
            return this.Execute($"Get ConpositeControl Footer Value: {control.Id}", driver =>
            {
                string text = string.Empty;

                /*
                driver.WaitUntilVisible(By.Id(control.Id));

                driver.ClickWhenAvailable(By.Id(control.Id));

                if (driver.HasElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut])))
                {
                    var compcntrl =
                        driver.WaitUntilAvailable(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut]));

                    foreach (var field in control.Fields)
                    {
                        compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id)).Click(true);

                        var result = compcntrl.FindElements(By.TagName("input"))
                            .ToList()
                            .FirstOrDefault(i => i.GetAttribute("id").Contains(field.Id));
                        text += result.GetAttribute("value") + " ";
                    }

                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click();
                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");

                */
                return text.TrimEnd(' ');
            });
        }

        /// <summary>
        /// Gets the value of a picklist on an Entity footer
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.GetFooterValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        public BrowserCommandResult<string> GetFooterValue(OptionSet option)
        {
            return this.Execute($"Get OptionSet Footer Value: {option.Name}", driver =>
            {
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Footer].Replace("[NAME]", option.Name.ToLower())));

                string text = string.Empty;
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Footer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var input = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Footer].Replace("[NAME]", option.Name.ToLower())));
                    text = input.Text;
                }
                else
                    throw new InvalidOperationException($"Unable to locate OptionSet '{option.Name}' in the footer. Please verify the OptionSet exists and try again.");

                return text;
            });
        }

        /// <summary>
        /// Gets the value of a Lookup on an Entity footer.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.Entity.GetFooterValue(new Lookup { Name = "primarycontactid" });</example>
        public BrowserCommandResult<string> GetFooterValue(LookupItem control)
        {
            return this.Execute($"Get Lookup Footer Value: {control.Name}", driver =>
            {
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Footer].Replace("[NAME]", control.Name.ToLower())));

                string lookupValue = string.Empty;
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Footer].Replace("[NAME]", control.Name.ToLower()))))
                {
                    var input = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Footer].Replace("[NAME]", control.Name.ToLower())));
                    lookupValue = input.Text;
                }
                else
                    throw new InvalidOperationException($"Unable to locate Lookup '{control.Name}' in the footer. Please verify the Lookup exists and try again.");

                return lookupValue;
            });
        }

        /// <summary>
        /// Gets the value of a DateTime field on an Entity footer.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.Entity.GetFooterValue(new DateTime {Name = "birthdate"));</example>
        public BrowserCommandResult<string> GetFooterValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Get DateTime Header Value: {date.Name}"), driver =>
            {
                string dateValue = "";
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer_Footer].Replace("[NAME]", date.Name.ToLower()))))
                {

                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer_Footer].Replace("[NAME]", date.Name.ToLower())));

                    // Check whether the DateTime field has an existing value
                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var label = fieldElement.FindElement(By.TagName("label"));
                        dateValue = label.Text;
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' in the footer. Please verify the DateTime field exists and try again.");

                return dateValue;
            });
        }

        /// <summary>
        /// Gets the value of a Checkbox/TwoOption field on an Entity footer.
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.Entity.GetFooterValue(new TwoOption {Name="creditonhold"});</example>
        public BrowserCommandResult<bool> GetFooterValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Get Checkbox/TwoOption Footer Value: {option.Name}"), driver =>
            {
                bool check = false;

                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer_Footer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer_Footer].Replace("[NAME]", option.Name.ToLower())));
                    var select = fieldElement;
                    var text = "";


                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var label = fieldElement.FindElement(By.TagName("label"));
                        text = label.Text;
                    }

                    if (fieldElement.TagName != "select")
                        select = fieldElement.FindElement(By.TagName("select"));

                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.Text.ToLower() == text.ToLower() || op.GetAttribute("title").ToLower() == text.ToLower())
                        {
                            var value = Convert.ToInt32(op.GetAttribute("value"));

                            check = Convert.ToBoolean(value);
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' in the footer. Please verify the TwoOption field exists and try again.");

                return check;
            });
        }

        /// <summary>
        /// Gets the value of a Text/Description field on an Entity header.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <returns>The value</returns>
        /// <example>xrmBrowser.Entity.GetHeaderValue("mobilephone");</example>
        public BrowserCommandResult<string> GetHeaderValue(string field)
        {
            return this.Execute($"Get Text Field Header Value: {field}", driver =>
            {
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Header].Replace("[NAME]", field.ToLower())));

                string text = string.Empty;
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Header].Replace("[NAME]", field.ToLower()))))
                {
                    var fieldElement = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Header].Replace("[NAME]", field.ToLower())));

                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        text = fieldElement.FindElement(By.TagName("textarea")).GetAttribute("value");
                    }
                    else
                    {
                        text = fieldElement.FindElement(By.TagName("input")).GetAttribute("value");
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate field '{field}' in the header. Please verify the field exists and try again.");

                return text;
            });
        }

        /// <summary>
        /// PLACEHOLDER: Gets the value of a Composite control on an Entity header.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <example>xrmBrowser.Entity.GetHeaderValue(new CompositeControl() { Id = "fullname", Fields = fields });</example>
        public BrowserCommandResult<string> GetHeaderValue(CompositeControl control)
        {
            return this.Execute($"Get ConpositeControl Header Value: {control.Id}", driver =>
            {
                string text = string.Empty;

                driver.WaitUntilVisible(By.Id(control.Id));

                driver.ClickWhenAvailable(By.Id(control.Id));

                if (driver.HasElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut])))
                {
                    var compcntrl =
                        driver.WaitUntilAvailable(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut]));

                    foreach (var field in control.Fields)
                    {
                        compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id)).Click(true);

                        var result = compcntrl.FindElements(By.TagName("input"))
                            .ToList()
                            .FirstOrDefault(i => i.GetAttribute("id").Contains(field.Id));
                        text += result.GetAttribute("value") + " ";
                    }

                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click();
                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");

                return text.TrimEnd(' ');
            });
        }

        /// <summary>
        /// Gets the value of a picklist on an Entity header
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.GetHeaderValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        public BrowserCommandResult<string> GetHeaderValue(OptionSet option)
        {
            return this.Execute($"Get OptionSet Header Value: {option.Name}", driver =>
            {
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Header].Replace("[NAME]", option.Name.ToLower())));

                string text = string.Empty;
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Header].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var input = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Header].Replace("[NAME]", option.Name.ToLower())));
                    text = input.Text;
                }
                else
                    throw new InvalidOperationException($"Unable to locate OptionSet '{option.Name}' in the header. Please verify the OptionSet exists and try again.");

                return text;
            });
        }

        /// <summary>
        /// Gets the value of a Lookup on an Entity header.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.Entity.GetHeaderValue(new Lookup { Name = "primarycontactid" });</example>
        public BrowserCommandResult<string> GetHeaderValue(LookupItem control)
        {
            return this.Execute($"Get Lookup Header Value: {control.Name}", driver =>
            {
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", control.Name.ToLower())));

                string lookupValue = string.Empty;
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", control.Name.ToLower()))))
                {
                    var input = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", control.Name.ToLower())));
                    lookupValue = input.Text;
                }
                else
                    throw new InvalidOperationException($"Unable to locate Lookup '{control.Name}' in the header. Please verify the Lookup exists and try again.");

                return lookupValue;
            });
        }

        /// <summary>
        /// Gets the value of a DateTime field on an Entity header.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.Entity.GetHeaderValue(new DateTime {Name = "birthdate"));</example>
        public BrowserCommandResult<string> GetHeaderValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Get DateTime Header Value: {date.Name}"), driver =>
            {
                string dateValue = "";
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer_Header].Replace("[NAME]", date.Name.ToLower()))))
                {

                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer_Header].Replace("[NAME]", date.Name.ToLower())));

                    // Check whether the DateTime field has an existing value
                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var label = fieldElement.FindElement(By.TagName("label"));
                        dateValue = label.Text;
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' in the header. Please verify the DateTime field exists and try again.");

                return dateValue;
            });
        }

        /// <summary>
        /// Gets the value of a Checkbox/TwoOption field on an Entity header.
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.Entity.GetHeaderValue(new TwoOption {Name="creditonhold"});</example>
        public BrowserCommandResult<bool> GetHeaderValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Get Checkbox/TwoOption Header Value: {option.Name}"), driver =>
            {
                bool check = false;

                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer_Header].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer_Header].Replace("[NAME]", option.Name.ToLower())));
                    var select = fieldElement;
                    var text = "";


                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var label = fieldElement.FindElement(By.TagName("label"));
                        text = label.Text;
                    }

                    if (fieldElement.TagName != "select")
                        select = fieldElement.FindElement(By.TagName("select"));

                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.Text.ToLower() == text.ToLower() || op.GetAttribute("title").ToLower() == text.ToLower())
                        {
                            var value = Convert.ToInt32(op.GetAttribute("value"));

                            check = Convert.ToBoolean(value);
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' in the header. Please verify the TwoOption field exists and try again.");

                return check;
            });
        }

        /// <summary>
        /// Returns the GUID for the current entity record. 
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.GetRecordGuid();</example>
        public BrowserCommandResult<Guid> GetRecordGuid(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Get GUID for the Current Record"), driver =>
            {
                SwitchToContentFrame();

                var recordGuid = driver.ExecuteScript("return Xrm.Page.data.entity.getId();").ToString(); ;

                return Guid.Parse(recordGuid);
            });
        }


        /// <summary>
        /// Returns the state of the tab, either Expanded or Collapsed. 
        /// </summary>
        /// <param name="name">The name of the tab.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.GetTabState("Details");</example>
        public BrowserCommandResult<string> GetTabState(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Get State for Tab: {name}"), driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name))))
                {
                    throw new InvalidOperationException($"Tab with name '{name}' does not exist.");
                }
                var tab = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name)));
                var tabState = "";

                if (!tab.GetAttribute("title").Contains("Collapse"))
                    tabState = "Collapsed";
                else if (!tab.GetAttribute("title").Contains("Expand"))
                    tabState = "Expanded";
                else
                    throw new InvalidOperationException("Unexpected tab state. Tab state should be either Collapsed or Expanded");

                return tabState;
            });
        }

        /// <summary>
        /// Gets the value of a Text/Description field on an Entity form.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <returns>The value</returns>
        /// <example>xrmBrowser.Entity.GetValue("mobilephone");</example>
        public BrowserCommandResult<string> GetValue(string field)
        {
            return this.Execute($"Get Text Field Value: {field}", driver =>
            {
                driver.WaitUntilVisible(By.Id(field));

                string text = string.Empty;
                if (driver.HasElement(By.Id(field)))
                {
                    driver.WaitUntilVisible(By.Id(field));
                    var fieldElement = driver.FindElement(By.Id(field));

                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        text = fieldElement.FindElement(By.TagName("textarea")).GetAttribute("value");
                    }
                    else
                    {
                        text = fieldElement.FindElement(By.TagName("input")).GetAttribute("value");

                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return text;
            });
        }

        /// <summary>
        /// Gets the value of a Composite control.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <example>xrmBrowser.Entity.GetValue(new CompositeControl { Id = "fullname", Fields = fields });</example>
        public BrowserCommandResult<string> GetValue(CompositeControl control)
        {
            return this.Execute($"Get ConpositeControl Value: {control.Id}", driver =>
            {
                string text = string.Empty;

                driver.WaitUntilVisible(By.Id(control.Id));

                driver.ClickWhenAvailable(By.Id(control.Id));

                if (driver.HasElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut])))
                {
                    var compcntrl =
                        driver.WaitUntilAvailable(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut]));

                    foreach (var field in control.Fields)
                    {
                        compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id)).Click(true);

                        var result = compcntrl.FindElements(By.TagName("input"))
                            .ToList()
                            .FirstOrDefault(i => i.GetAttribute("id").Contains(field.Id));
                        text += result.GetAttribute("value") + " ";
                    }

                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click();
                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");

                return text.TrimEnd(' ');
            });
        }

        /// <summary>
        /// Gets the value of a picklist on an Entity form
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        public BrowserCommandResult<string> GetValue(OptionSet option)
        {
            return this.Execute($"Get OptionSet Value: {option.Name}", driver =>
            {
                driver.WaitUntilVisible(By.Id(option.Name));
                string text = string.Empty;
                if (driver.HasElement(By.Id(option.Name)))
                {
                    var input = driver.FindElement(By.Id(option.Name));
                    text = input.Text;
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                return text;
            });
        }

        /// <summary>
        /// Gets the value of a Lookup on an Entity form.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.Entity.GetValue(new Lookup { Name = "primarycontactid" });</example>
        public BrowserCommandResult<string> GetValue(LookupItem control)
        {
            return this.Execute($"Get Lookup Value: {control.Name}", driver =>
            {
                driver.WaitUntilVisible(By.Id(control.Name));

                string lookupValue = string.Empty;
                if (driver.HasElement(By.Id(control.Name)))
                {
                    var input = driver.FindElement(By.Id(control.Name));
                    lookupValue = input.Text;
                }
                else
                    throw new InvalidOperationException($"Field: {control.Name} Does not exist");

                return lookupValue;
            });
        }

        /// <summary>
        /// Gets the value of a DateTime field on an entity form.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.Entity.GetValue(new DateTime {Name = "birthdate"));</example>
        public BrowserCommandResult<string> GetValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Get DateTime Value: {date.Name}"), driver =>
            {
                string dateValue = "";
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer].Replace("[NAME]", date.Name.ToLower()))))
                {

                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer].Replace("[NAME]", date.Name.ToLower())));

                    // Check whether the DateTime field has an existing value
                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var label = fieldElement.FindElement(By.TagName("label"));
                        dateValue = label.Text;
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' on the form. Please verify the DateTime field exists and try again.");

                return dateValue;
            });
        }

        /// <summary>
        /// Gets the value of a Checkbox/TwoOption field in an Entity form.
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.Entity.GetValue(new TwoOption {Name="creditonhold"});</example>
        public BrowserCommandResult<bool> GetValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Get Checkbox/TwoOption Value: {option.Name}"), driver =>
            {
                bool check = false;

                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));
                    var select = fieldElement;
                    var text = "";


                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var label = fieldElement.FindElement(By.TagName("label"));
                        text = label.Text;
                    }

                    if (fieldElement.TagName != "select")
                        select = fieldElement.FindElement(By.TagName("select"));

                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.Text.ToLower() == text.ToLower() || op.GetAttribute("title").ToLower() == text.ToLower())
                        {
                            var value = Convert.ToInt32(op.GetAttribute("value"));

                            check = Convert.ToBoolean(value);
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' on the form. Please verify the TwoOption field exists and try again.");

                return check;
            });
        }

        /// <summary>
        /// Opens the Entity
        /// </summary>
        /// <param name="entityName">The Entity Name you want to open</param>
        /// <param name="id">The Guid</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.OpenEntity(TestSettings.AccountLogicalName, Guid.Parse(TestSettings.AccountId));</example>
        public BrowserCommandResult<bool> OpenEntity(string entityName, Guid id, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open: {entityName} {id}"), driver =>
            {
                var uri = new Uri(this.Browser.Driver.Url);
                var link = $"{uri.Scheme}://{uri.Authority}/main.aspx?etn={entityName}&pagetype=entityrecord&id=%7B{id:D}%7D";
                return OpenEntity(new Uri(link)).Value;
            });
        }

        /// <summary>
        /// Navigate Down the record
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.NavigateDown();</example>
        public BrowserCommandResult<bool> NavigateDown(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Navigate Down"), driver =>
            {
                SwitchToDefault();
                if (!driver.HasElement(By.CssSelector(_navigateDownCssSelector)))
                    return false;

                var buttons = driver.FindElements(By.CssSelector(_navigateDownCssSelector));

                buttons[0].Click();

                //driver.WaitFor(d => d.ExecuteScript(XrmPerformanceCenterPage.GetAllMarkersJavascriptCommand).ToString().Contains("AllSubgridsLoaded"));
                driver.WaitForPageToLoad();

                return true;
            });
        }

        /// <summary>
        /// Navigate Up the record
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.NavigateUp();</example>
        public BrowserCommandResult<bool> NavigateUp(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Navigate Up"), driver =>
            {
                SwitchToDefault();
                if (!driver.HasElement(By.CssSelector(_navigateUpCssSelector)))
                    return false;

                var buttons = driver.FindElements(By.CssSelector(_navigateUpCssSelector));

                buttons[0].Click();

                //driver.WaitFor(d => d.ExecuteScript(XrmPerformanceCenterPage.GetAllMarkersJavascriptCommand).ToString().Contains("AllSubgridsLoaded"));
                driver.WaitForPageToLoad();
                
                return true;
            });
        }

        /// <summary>
        /// Opens the Entity
        /// </summary>
        /// <param name="uri">The uri</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> OpenEntity(Uri uri, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open: {uri}"), driver =>
            {

                driver.Navigate().GoToUrl(uri);

                DismissAlertIfPresent();

                // driver.WaitFor(d => d.ExecuteScript(XrmPerformanceCenterPage.GetAllMarkersJavascriptCommand).ToString().Contains("AllSubgridsLoaded"));
                SwitchToContent();
                driver.WaitForPageToLoad();
                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Entity.Form]),
                                            new TimeSpan(0, 0, 30),
                                            null,
                                            d => { throw new Exception("CRM Record is Unavailable or not finished loading. Timeout Exceeded"); }
                                        );

                return true;
            });
        }

        /// <summary>
        /// Popout the Entity form
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.Popout();</example>
        public BrowserCommandResult<bool> Popout(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Popout Entity Form"), driver =>
            {
                SwitchToDefault();
                driver.ClickWhenAvailable(By.ClassName(Elements.CssClass[Reference.Entity.Popout]));

                return true;
            });
        }

        /// <summary>
        /// Saves the specified entity record.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.Save();</example>
        public BrowserCommandResult<bool> Save(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Save Entity"), driver =>
            {
                var save = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.Save]),
                    "Save Buttton is not available");

                save?.Click();

                driver.WaitUntilVisible(By.Id("titlefooter_statuscontrol"));

                // Wait until the footer is not equal to 'saving', indicating save is complete or failed
                driver.WaitFor(x => x.FindElement(By.Id("titlefooter_statuscontrol")).Text != "saving", new TimeSpan(0, 2, 0));

                var footerText = driver.FindElement(By.Id("titlefooter_statuscontrol")).Text;

                // Initilize inlineDialogError for general error dialog scenarios
                // Errors could include Business Process Error, Generic SQL Error, etc...
                bool inlineDialogError = false;

                if (String.IsNullOrEmpty(footerText))
                {
                    driver.SwitchTo().DefaultContent();
                    inlineDialogError = driver.FindElement(By.Id("InlineDialog_Iframe")).Displayed;
                }

                if (inlineDialogError)
                {
                    driver.SwitchTo().Frame("InlineDialog_Iframe");
                    footerText = driver.FindElement(By.Id("ErrorMessage")).Text;
                }

                if (!string.IsNullOrEmpty(footerText))
                {
                    throw new InvalidOperationException(footerText);
                }

                return true;
            });
        }

        /// <summary>
        /// Selects the Desired Entity Form
        /// </summary>
        /// <param name="name">The name of the form</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.SelectForm("Details");</example>
        public BrowserCommandResult<bool> SelectForm(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Change Entity Form: {name}"), driver =>
            { 
                var form = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.SelectForm]) ,"The Select Form option is not available.");
                form.Click();

                var items = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.SelectFormContentTable])).FindElements(By.TagName("a"));
                items.Where(x => x.Text == name).FirstOrDefault()?.Click();

                return true;

            });
        }

        /// <summary>
        /// Selects the Form Section
        /// </summary>
        /// <param name="name">The name of the form section</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.SelectFormSection("Details");</example>
        public BrowserCommandResult<bool> SelectFormSection(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Select Form Section: {name}"), driver =>
            {
                var form = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.SelectFormSection]), "The Select Form Section option is not available.");
                form.Click();

                var items = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.FormSectionContentTable])).FindElements(By.TagName("a"));
                items.Where(x => x.Text == name).FirstOrDefault()?.Click();

                return true;

            });
        }

        /// <summary>
        /// Set Lookup Value for Subgrid subgridName
        /// </summary>
        /// <param name="subgridName">The SubgridName</param>
        /// <param name="value"></param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", "Alex Wu");</example>
        public BrowserCommandResult<bool> SelectSubgridLookup(string subgridName, string value, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Set Lookup Value for Subgrid {subgridName}"), driver =>
            {
                if (driver.HasElement(By.Id($"inlineLookupControlForSubgrid_{subgridName}")))
                {
                    var input = driver.ClickWhenAvailable(By.Id($"inlineLookupControlForSubgrid_{subgridName}"));
                    
                    var lookupIcon = input.FindElement(By.ClassName(Elements.CssClass[Reference.Entity.LookupRender]));
                    lookupIcon.Click();

                    var dialogName = $"Dialog_lookup_{subgridName}_i_IMenu";
                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if (!dialogItems.Exists(x => x.Title == value))
                        throw new InvalidOperationException($"List does not have {value}.");

                    var dialogItem = dialogItems.Where(x => x.Title == value).First();
                    dialogItem.Element.Click();
                }

                return true;
            });
        }

        /// <summary>
        /// Set Lookup Value for Subgrid
        /// </summary>
        /// <param name="subgridName">The subgridName</param>
        /// <param name="index">The Index</param>
        /// <example>xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", 3);</example>
        public BrowserCommandResult<bool> SelectSubgridLookup(string subgridName, [Range(0, 9)]int index)
        {
            return this.Execute(GetOptions($"Set Lookup Value for Subgrid {subgridName}"), driver =>
            {
                if (driver.HasElement(By.Id($"inlineLookupControlForSubgrid_{subgridName}")))
                {
                    var input = driver.ClickWhenAvailable(By.Id($"inlineLookupControlForSubgrid_{subgridName}"));

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.Entity.LookupRender])).Click();

                    var dialogName = $"Dialog_lookup_{subgridName}_i_IMenu";
                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if (dialogItems.Count < index)
                        throw new InvalidOperationException($"List does not have {index + 1} items.");

                    var dialogItem = dialogItems[index];
                    dialogItem.Element.Click();
                }

                return true;
            });
        }

        /// <summary>
        /// Set Lookup Value for Subgrid
        /// </summary>
        /// <param name="subgridName">The SubgridName</param>
        /// <param name="openLookupPage"></param>
        /// <example>xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", true);</example>
        public BrowserCommandResult<bool> SelectSubgridLookup(string subgridName, bool openLookupPage = true)
        {
            return this.Execute(GetOptions($"Set Lookup Value for Subgrid {subgridName}"), driver =>
            {
                if (driver.HasElement(By.Id($"inlineLookupControlForSubgrid_{subgridName}")))
                {
                    var input = driver.ClickWhenAvailable(By.Id($"inlineLookupControlForSubgrid_{subgridName}"));
                    
                    input.FindElement(By.ClassName(Elements.CssClass[Reference.Entity.LookupRender])).Click();

                    var dialogName = $"Dialog_lookup_{subgridName}_i_IMenu";

                    driver.WaitUntilVisible(By.Id(dialogName), new TimeSpan(0, 0, 2));

                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    var dialogItem = dialogItems.Last();
                    
                    if (this.Browser.Options.BrowserType == BrowserType.Firefox)
                    {
                        var id = dialog.FindElements(By.TagName("li")).Last().GetAttribute("id");
                       
                        driver.ExecuteScript($"document.getElementById('{id}').childNodes[1].click();");
                    }
                    else
                        dialogItem.Element.Click();
                }

                return true;
            });
        }

        /// <summary>
        /// Selects the tab and clicks. If the tab is expanded it will collapse it. If the tab is collapsed it will expand it. 
        /// </summary>
        /// <param name="name">The name of the tab.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.SelectTab("Details");</example>
        public BrowserCommandResult<bool> SelectTab(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"SelectTab: {name}"), driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name))))
                {
                    throw new InvalidOperationException($"Tab with name '{name}' does not exist.");
                }
                var tab = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name)));

                tab?.Click();

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a TwoOption / Checkbox field on an Entity header.
        /// </summary>
        /// <param name="option">Field name or ID.</param>
        /// <example>xrmBrowser.Entity.SetHeaderValue(new TwoOption{ Name = "creditonhold"});</example>
        public BrowserCommandResult<bool> SetHeaderValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Set TwoOption Header Value: {option.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer_Header].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer_Header].Replace("[NAME]", option.Name.ToLower())));

                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var label = fieldElement.FindElement(By.TagName("label"));

                        if (label.Text != option.Value)
                        {
                            fieldElement.Click(true);
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' in the header. Please verify the TwoOption field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date Field on an Entity header.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.Entity.SetHeaderValue(new DateTimeControl { Name = "birthdate", Value =  DateTime.Parse("11/1/1980")});</example>
        public BrowserCommandResult<bool> SetHeaderValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Set DateTime Header Value: {date.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer_Header].Replace("[NAME]", date.Name.ToLower()))))
                {
                    var fieldElement = driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Entity.DateFieldContainer_Header].Replace("[NAME]", date.Name.ToLower())));

                    //Check to see if focus is on field already
                    if (fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])) != null)
                        fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();
                    else
                        fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.ValueClass])).Click();

                    var input = fieldElement.FindElement(By.TagName("input"));

                    if (input.GetAttribute("value").Length > 0)
                    {
                        input.Clear();
                        fieldElement.Click();
                        input.SendKeys(date.Value.ToShortDateString());
                        input.SendKeys(Keys.Enter);
                    }
                    else
                    {
                        input.SendKeys(date.Value.ToShortDateString());
                        input.SendKeys(Keys.Enter);
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' in the header. Please verify the DateTime field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Text/Description field on an Entity header.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <param name="value">The value.</param>
        /// <example>xrmBrowser.Entity.SetHeaderValue("name", "Test API Account");</example>
        public BrowserCommandResult<bool> SetHeaderValue(string field, string value)
        {
            var returnval = this.Execute(GetOptions($"Set Text Field Header Value: {field}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Header].Replace("[NAME]", field.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.TextFieldContainer_Header].Replace("[NAME]", field.ToLower())));

                    fieldElement.Click();

                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("textarea")).Clear();
                        fieldElement.FindElement(By.TagName("textarea")).SendKeys(value);
                    }
                    else if (fieldElement.TagName == "textarea")
                    {
                        fieldElement.Clear();
                        fieldElement.SendKeys(value);
                    }
                    else
                    {
                        fieldElement.FindElement(By.TagName("input")).Clear();
                        fieldElement.FindElement(By.TagName("input")).SendKeys(value, true);
                        fieldElement.SendKeys(Keys.Tab);
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate field '{field}' in the header. Please verify the field exists and try again.");

                return true;
            });
            return returnval;
        }

        /// <summary>
        /// Sets the value of a picklist on an Entity header.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.SetHeaderValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public BrowserCommandResult<bool> SetHeaderValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Set OptionSet Header Value: {option.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Header].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.OptionSetFieldContainer_Header].Replace("[NAME]", option.Name.ToLower())));
                    var select = fieldElement;

                    if (fieldElement.TagName != "select")
                        select = fieldElement.FindElement(By.TagName("select"));

                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.Text.ToLower() == option.Value.ToLower() || op.GetAttribute("title").ToLower() == option.Value.ToLower())
                        {
                            fieldElement.Click(true);
                            op.Click(true);
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate OptionSet '{option.Name}' in the header. Please verify the OptionSet exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// PLACEHOLDER: Sets the value of a multi-value picklist on an Entity header.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.SetHeaderValue(new MultiValueOptionSet { });</example>
        public BrowserCommandResult<bool> SetHeaderValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Set MultiOptionSet Header Value: {option.Name}"), driver =>
            {
                driver.WaitUntilVisible(By.Id(option.Name));

                if (driver.HasElement(By.Id(option.Name)))
                {
                    var container = driver.ClickWhenAvailable(By.Id(option.Name));

                    if (removeExistingValues)
                    {
                        //Remove Existing Values
                        var values = container.FindElements(By.ClassName(Elements.CssClass[Reference.SetValue.MultiSelectPicklistDeleteClass]));
                        foreach (var value in values)
                            value.Click(true);
                    }

                    var input = container.FindElement(By.TagName("input"));
                    input.Click();
                    input.SendKeys(" ");

                    var options = container.FindElements(By.TagName("li"));

                    foreach (var op in options)
                    {
                        var label = op.FindElement(By.TagName("label"));

                        if (option.Values.Contains(op.Text) || option.Values.Contains(op.GetAttribute("value")) || option.Values.Contains(label.GetAttribute("title")))
                            op.Click(true);
                    }

                    container.Click();
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// PLACEHOLDER: Sets the value of a Composite control on an Entity header.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <example>xrmBrowser.Entity.SetHeaderValue(new CompositeControl {Id = "fullname", Fields = fields});</example>
        public BrowserCommandResult<bool> SetHeaderValue(CompositeControl control)
        {
            return this.Execute(GetOptions($"Set ConpositeControl Header Value: {control.Id}"), driver =>
            {
                driver.WaitUntilVisible(By.Id(control.Id));

                if (!driver.HasElement(By.Id(control.Id)))
                    return false;

                driver.ClickWhenAvailable(By.Id(control.Id));

                if (driver.HasElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut])))
                {
                    var compcntrl =
                        driver.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut]));

                    foreach (var field in control.Fields)
                    {
                        compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id)).Click();

                        var result = compcntrl.FindElements(By.TagName("input"))
                            .ToList()
                            .FirstOrDefault(i => i.GetAttribute("id").Contains(field.Id));

                        //BugFix - Setvalue -The value is getting erased even after setting the value ,might be due to recent CSS changes.
                        driver.ExecuteScript("document.getElementById('" + result?.GetAttribute("id") + "').value = ''");
                        result?.SendKeys(field.Value);
                    }

                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click();
                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Lookup on an Entity header.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.Entity.SetHeaderValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        public BrowserCommandResult<bool> SetHeaderValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Set Lookup Header Value: {control.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", control.Name.ToLower()))))
                {
                    driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", control.Name.ToLower())));

                    var lookupInput = driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", control.Name.ToLower())));

                    if (lookupInput.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {control.Name} is not Lookup control");

                    lookupInput.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click(true);

                    var dialogName = $"Dialog_header_{control.Name}_IMenu";

                    var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;


                    if (control.Value != null)
                    {

                        if (!dialogItems.Exists(x => x.Title.ToLower() == control.Value.ToLower()))
                            throw new InvalidOperationException($"List does not contain value {control.Value}.");

                        var dialogItem = dialogItems.Where(x => x.Title.ToLower() == control.Value.ToLower()).First();

                        dialogItem.Element.Hover(driver, true);
                        dialogItem.Element.Click(true);


                    }
                    else
                    {
                        if (dialogItems.Count < control.Index)
                            throw new InvalidOperationException($"List does not have {control.Index + 1} items.");

                        var dialogItem = dialogItems[control.Index];

                        dialogItem.Element.Hover(driver, true);
                        dialogItem.Element.Click(true);

                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate Lookup '{control.Name}' in the header. Please verify the Lookup exists and try again.");

                return true;
            });
        }
    }
}