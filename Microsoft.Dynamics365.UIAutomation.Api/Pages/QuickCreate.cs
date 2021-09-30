// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Xrm Quick Create Page
    /// </summary>
    public class QuickCreate
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickCreate"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public QuickCreate(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToQuickCreate();
        }

        /// <summary>
        /// Cancel the Quick Create Page
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.QuickCreate.Cancel();</example>
        public BrowserCommandResult<bool> Cancel(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Cancel"), driver =>
            {
                SwitchToDefault();

                driver.FindElement(By.XPath(Elements.Xpath[Reference.QuickCreate.Cancel]))?.Click();
                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Text field on a QuickCreate form
        /// </summary>
        /// <param name="field">The field</param>
        /// <example>xrmBrowser.QuickCreate.ClearValue("firstname", "Test");</example>
        public BrowserCommandResult<bool> ClearValue(string field)
        {
            return this.Execute(GetOptions($"Clear QuickCreate Text Field Value: {field}"), driver =>
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
                    throw new InvalidOperationException($"Unable to locate field '{field}' on the QuickCreate form. Please verify the field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Checkbox/TwoOption field on a QuickCreate form
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.QuickCreate.ClearValue(new TwoOption{ Name = "creditonhold"});</example>
        public BrowserCommandResult<bool> ClearValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Clear QuickCreate Checkbox/TwoOption Value: {option.Name}"), driver =>
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
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' on the QuickCreate form. Please verify the TwoOption field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of an option set / picklist on a QuickCreate form
        /// </summary>
        /// <param name="option">The option you want to clear.</param>
        /// <example>xrmBrowser.QuickCreate.ClearValue(new OptionSet { Name = "preferredcontactmethodcode"});</example>
        public BrowserCommandResult<bool> ClearValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Clear QuickCreate OptionSet Value: {option.Name}"), driver =>
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
                    throw new InvalidOperationException($"Unable to locate OptionSet '{option.Name}' on the QuickCreate form. Please verify the OptionSet exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Lookup on a QuickCreate form
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.QuickCreate.ClearValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        public BrowserCommandResult<bool> ClearValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Clear QuickCreate Lookup Value: {control.Name}"), driver =>
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
                    throw new InvalidOperationException($"Unable to locate Lookup '{control.Name}' on the QuickCreate form. Please verify the Lookup exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a DateTime field on a QuickCreate form.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.QuickCreate.ClearValue(new DateTime {Name = "birthdate"}));</example>
        public BrowserCommandResult<bool> ClearValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Clear QuickCreate DateTime Field: {date.Name}"), driver =>
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
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' on the QuickCreate form. Please verify the DateTime field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="option">The option you want to clear.</param>
        /// <example>xrmBrowser.QuickCreate.ClearValue(new OptionSet { Name = "preferredcontactmethodcode"});</example>
        public BrowserCommandResult<bool> ClearValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Clear QuickCreate MultiValueOptionSet Value: {option.Name}"), driver =>
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
        /// <example>xrmBrowser.QuickCreate.ClearValue(new CompositeControl {Id = "fullname"});</example>
        public BrowserCommandResult<bool> ClearValue(CompositeControl control)
        {
            return this.Execute(GetOptions($"Clear QuickCreate ConpositeControl Value: {control.Id}"), driver =>
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
        /// Gets the value of a Text/Description field on a QuickCreate form.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <returns>The value</returns>
        /// <example>xrmBrowser.Entity.GetValue("mobilephone");</example>
        public BrowserCommandResult<string> GetValue(string field)
        {
            return this.Execute($"Get QuickCreate Text Field Value: {field}", driver =>
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
        /// Gets the value of a Composite control on a QuickCreate form.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <example>xrmBrowser.QuickCreate.GetValue(new CompositeControl { Id = "fullname", Fields = fields });</example>
        public BrowserCommandResult<string> GetValue(CompositeControl control)
        {
            return this.Execute($"Get QuickCreate ConpositeControl Value: {control.Id}", driver =>
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
        /// Gets the value of a picklist on a QuickCreate form
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.QuickCreate.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        public BrowserCommandResult<string> GetValue(OptionSet option)
        {
            return this.Execute($"Get QuickCreate OptionSet Value: {option.Name}", driver =>
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
        /// Gets the value of a Lookup on a QuickCreate form.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.QuickCreate.GetValue(new Lookup { Name = "primarycontactid" });</example>
        public BrowserCommandResult<string> GetValue(LookupItem control)
        {
            return this.Execute($"Get QuickCreate Lookup Value: {control.Name}", driver =>
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
        /// Gets the value of a DateTime field on a QuickCreate form.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.QuickCreate.GetValue(new DateTime {Name = "birthdate"));</example>
        public BrowserCommandResult<string> GetValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Get QuickCreate DateTime Value: {date.Name}"), driver =>
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
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' on the QuickCreate form. Please verify the DateTime field exists and try again.");

                return dateValue;
            });
        }

        /// <summary>
        /// Gets the value of a Checkbox/TwoOption field on a QuickCreate form.
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.QuickCreate.GetValue(new TwoOption {Name="creditonhold"});</example>
        public BrowserCommandResult<bool> GetValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Get QuickCreate Checkbox/TwoOption Value: {option.Name}"), driver =>
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
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' on the QuickCreate form. Please verify the TwoOption field exists and try again.");

                return check;
            });
        }

        /// <summary>
        /// Save the Quick create page
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.QuickCreate.Save();</example>
        public BrowserCommandResult<bool> Save(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Save"), driver =>
            {
                SwitchToDefault();
                driver.FindElement(By.XPath(Elements.Xpath[Reference.QuickCreate.Save]))?.Click();
                SwitchToQuickCreateFrame();
                var errorMessageElements = driver.FindElements(By.ClassName("ms-crm-Inline-Validation"));
                if (errorMessageElements.Any(p => p.Displayed))
                    throw new InvalidOperationException(errorMessageElements.First(p => p.Displayed).Text);
                return true;
            });
        }

        /// <summary>
        /// Set Lookup Value for the field
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="openLookupPage">The Open Lookup Page</param>
        /// <param name="clearFieldValue">Remove Existing Field Value, if present. False = Click the existing value</param>
        public new BrowserCommandResult<bool> SelectLookup(LookupItem field, bool clearFieldValue = true, bool openLookupPage = true)
        {
            return this.Execute(GetOptions($"Select QuickCreate Lookup for: {field.Name}"), driver =>
            {
                if (driver.HasElement(By.Id(field.Name)))
                {
                    var fieldContainer = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.FieldContainer].Replace("[NAME]", field.Name)));

                    if (fieldContainer.Text != "" && clearFieldValue)
                    {
                        fieldContainer.SendKeys(Keys.Clear);
                    }
                    else if (fieldContainer.Text != "" && !clearFieldValue)
                    {
                        fieldContainer.Click();
                        return true;
                    }

                    var input = driver.ClickWhenAvailable(By.Id(field.Name));

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {field.Name} is not lookup");

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click();

                    Browser.ThinkTime(1000);
                    var dialogName = $"Dialog_{field.Name}_IMenu";
                    var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if (dialogItems.Any())
                    {
                        var dialogItem = dialogItems.Last();
                        dialogItem.Element.Click();
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field.Name} Does not exist");

                return true;

            });
        }

        /// <summary>
        /// Sets the value of a Checkbox / TwoOption.
        /// </summary>
        /// <param name="field">Field name or ID.</param>
        /// <param name="check">If set to <c>true</c> [check].</param>
        [Obsolete("SetValue(string field, bool check) is deprecated, please use SetValue(TwoOption option) instead.")]
        public new BrowserCommandResult<bool> SetValue(string field, bool check)
        {
            return this.Execute(GetOptions($"Set Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id("int_" + field)))
                {
                    var input = driver.FindElement(By.Id("int_" + field));

                    if (bool.Parse(input.FindElement(By.TagName("input")).GetAttribute("checked")) && !check)
                        input.FindElement(By.TagName("input")).Click();
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Checkbox/TwoOption field on a QuickCreate form.
        /// </summary>
        /// <param name="option">TwoOption Name and Value</param>
        public new BrowserCommandResult<bool> SetValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Set QuickCreate TwoOption Value: {option.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));

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
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' on the QuickCreate form. Please verify the TwoOption field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        [Obsolete("SetValue(string field, DateTime date) is deprecated, please use SetValue(DateTimeControl date) instead.")]
        public new BrowserCommandResult<bool> SetValue(string field, DateTime date)
        {
            return this.Execute(GetOptions($"Set QuickCreate DateTime Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var input = driver.ClickWhenAvailable(By.Id(field));

                    //Check to see if focus is on field already
                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])) != null)
                        input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();
                    else
                        input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.ValueClass])).Click();

                    input.FindElement(By.TagName("input")).SendKeys(date.ToShortDateString());
                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();

                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date Field on a QuickCreate form.
        /// </summary>
        /// <param name="date">DateTimecontrol Name and Value.</param>
        public new BrowserCommandResult<bool> SetValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Set QuickCreate DateTime Value: {date.Name}"), driver =>
            {
                if (driver.HasElement(By.Id(date.Name)))
                {
                    var fieldElement = driver.ClickWhenAvailable(By.Id(date.Name));

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
                        input.SendKeys(Keys.Tab);
                    }
                    else
                    {
                        fieldElement.Click();
                        input.SendKeys(date.Value.ToShortDateString());
                        input.SendKeys(Keys.Enter);
                        input.SendKeys(Keys.Tab);
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' on the QuickCreate form. Please verify the DateTime field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Text/Description field on a QuickCreate form.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <param name="value">The value.</param>
        /// <example>xrmBrowser.QuickCreate.SetValue("lastname", "Contact");</example>
        public new BrowserCommandResult<bool> SetValue(string field, string value)
        {
            return this.Execute(GetOptions($"Set Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var input = driver.ClickWhenAvailable(By.Id(field));

                    //Check to see if focus is on field already
                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])) != null)
                        input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();
                    else
                        input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.ValueClass])).Click();

                    if (input.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        input.FindElement(By.TagName("textarea")).SendKeys(value);
                        input.FindElement(By.TagName("textarea")).SendKeys(Keys.Tab);
                    }
                    else
                    {
                        input.FindElement(By.TagName("input")).SendKeys(value);
                        input.FindElement(By.TagName("input")).SendKeys(Keys.Tab);
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of an OptionSet on a QuickCreate form.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        public new BrowserCommandResult<bool> SetValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Set QuickCreate OptionSet Value: {option.Name}"), driver =>
            {
                driver.WaitUntilVisible(By.Id(option.Name));

                if (driver.HasElement(By.Id(option.Name)))
                {
                    var input = driver.ClickWhenAvailable(By.Id(option.Name));
                    var select = input;

                    if (input.TagName != "select")
                        select = input.FindElement(By.TagName("select"));

                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.Text == option.Value || op.GetAttribute("value") == option.Value)
                            op.Click();
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate OptionSet field '{option.Name}' on the QuickCreate form. Please verify the OptionSet field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Composite control on a QuickCreate form.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        public BrowserCommandResult<bool> SetValue(CompositeControl control)
        {
            return this.Execute(GetOptions($"Set QuickCreate ConpositeControl Value: {control.Id}"), driver =>
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
                    throw new InvalidOperationException($"Unable to locate CompositeControl field '{control.Name}' on the QuickCreate form. Please verify the CompositeControl field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Lookup on a QuickCreate form.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.QuickCreate.SetValue(new Lookup { Name = "primarycontactid", Value = "Rene Valdes (sample)" });</example>
        public new BrowserCommandResult<bool> SetValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Set QuickCreate Lookup Value: {control.Name}"), driver =>
            {
                if (driver.HasElement(By.Id(control.Name)))
                {
                    driver.WaitUntilVisible(By.Id(control.Name));

                    var input = driver.ClickWhenAvailable(By.Id(control.Name));

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {control.Name} is not lookup");

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click();

                    var dialogName = $"Dialog_{control.Name}_IMenu";
                    var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;


                    if (control.Value != null)
                    {

                        if (!dialogItems.Exists(x => x.Title == control.Value))
                            throw new InvalidOperationException($"List does not have {control.Value}.");

                        var dialogItem = dialogItems.Where(x => x.Title == control.Value).First();
                        dialogItem.Element.Click();

                    }
                    else
                    {
                        if (dialogItems.Count < control.Index)
                            throw new InvalidOperationException($"List does not have {control.Index + 1} items.");

                        var dialogItem = dialogItems[control.Index];
                        dialogItem.Element.Click();
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate the Lookup field '{control.Name}' on the QuickCreate form. Please verify the Lookup field exists and try again.");

                return true;
            });
        }

    }
}