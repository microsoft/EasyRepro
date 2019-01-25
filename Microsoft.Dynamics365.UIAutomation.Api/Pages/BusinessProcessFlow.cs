// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Business process flow.
    /// </summary>
    public class BusinessProcessFlow
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessProcessFlow"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public BusinessProcessFlow(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToContent();
        }

        /// <summary>
        /// Clears the value of a Text field in a Business Process Flow
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmBrowser.BusinessProcessFlow.ClearValue("firstname", "Test");</example>
        public BrowserCommandResult<bool> ClearValue(string field)
        {
            return this.Execute(GetOptions($"Clear BPF Text Field Value: {field}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", field.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", field.ToLower())));

                    fieldElement.Click();

                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("textarea")).Clear();
                    }
                    else if (fieldElement.TagName == "textarea")
                    {
                        fieldElement.Clear();
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate field '{field}' in the Business Process Flow. Please verify the field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Checkbox/TwoOption field in a Business Process Flow.
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.BusinessProcessFlow.ClearValue(new TwoOption{ Name = "creditonhold"});</example>
        public BrowserCommandResult<bool> ClearValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Clear BPF Checkbox/TwoOption Value: {option.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));

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
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' in the Business Process Flow. Please verify the TwoOption field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of an option set / picklist in a Business Process Flow.
        /// </summary>
        /// <param name="option">The option you want to clear.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.ClearValue(new OptionSet { Name = "preferredcontactmethodcode"});</example>
        public BrowserCommandResult<bool> ClearValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Clear BPF OptionSet Value: {option.Name}"), driver =>
            {

                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.OptionSetFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));
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
                    throw new InvalidOperationException($"Unable to locate OptionSet '{option.Name}' in the Business Process Flow. Please verify the OptionSet exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a Lookup in a Business Process Flow.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.ClearValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        public BrowserCommandResult<bool> ClearValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Clear BPF Lookup Value: {control.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower())));

                    if (fieldElement.Text != "")
                    {
                        fieldElement.Hover(driver, true);

                        if (fieldElement.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.GetLookupSearchIcon].Replace("[NAME]", control.Name.ToLower()))) == null)
                            throw new InvalidOperationException($"Field: {control.Name} is not Lookup control");

                        driver.Manage().Window.Maximize();
                        var lookupSearch = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.GetLookupSearchIcon].Replace("[NAME]", control.Name.ToLower())));

                        if (!lookupSearch.Displayed)
                        {
                            driver.Manage().Window.Minimize();
                            driver.Manage().Window.Maximize();
                            fieldElement.Hover(driver, true);
                            lookupSearch = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.GetLookupSearchIcon].Replace("[NAME]", control.Name.ToLower())));
                        }

                        lookupSearch.Click(true);

                        var dialogName = $"Dialog_header_process_{control.Name}_IMenu";
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
                    throw new InvalidOperationException($"Unable to locate Lookup '{control.Name}' in the Business Process Flow. Please verify the Lookup exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Clears the value of a DateTime field in a Business Process Flow.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.BusinessProcessFlow.ClearValue(new DateTime {Name = "birthdate"}));</example>
        public BrowserCommandResult<bool> ClearValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Clear BPF DateTime Field: {date.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.DateFieldContainer].Replace("[NAME]", date.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.DateFieldContainer].Replace("[NAME]", date.Name.ToLower())));

                    // Check whether the DateTime field has an existing value
                    if (fieldElement.GetAttribute("title") != "Select to enter data")
                    {
                        fieldElement.Click(true);
                        var fieldInput = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.DateFieldInput].Replace("[NAME]", date.Name.ToLower())));
                        // Clear any existing values
                        fieldInput.Clear();
                        fieldElement.Click(true);
                        fieldElement.SendKeys(Keys.Enter);
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' in the Business Process Flow. Please verify the DateTime field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Placeholder: MultiValueOptionSets are not currently supported in BPFs.
        /// </summary>
        /// <param name="option">The option you want to clear.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.ClearValue(new MultiValueOptionSet { Name = "preferredcontactmethodcode"});</example>
        public BrowserCommandResult<bool> ClearValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Clear BPF MultiValueOptionSet Value: {option.Name}"), driver =>
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
        /// <example>xrmBrowser.BusinessProcessFlow.ClearValue(new CompositeControl() {Id = "fullname"});</example>
        public BrowserCommandResult<bool> ClearValue(CompositeControl control)
        {
            return this.Execute(GetOptions($"Clear BPF ConpositeControl Value: {control.Id}"), driver =>
            {
                /*
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

                */
                return true;
            });
        }

        /// <summary>
        /// If the BPF is at the end of the final stage, finishes the BPF.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.Finish();</example>
        public BrowserCommandResult<bool> Finish(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Finish BPF", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Finish])))
                    throw new Exception("Business Process Flow Finish Element does not exist");

                if (driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Finish])).GetAttribute("class").Contains("hidden"))
                    throw new Exception("The Business Process is already finished");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Finish]));

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.FinishedLabel]));

                if (!driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Finish])).GetAttribute("class").Contains("hidden"))
                    throw new Exception("The finish operation did not complete as expected.");

                return true;
            });
        }

        /// <summary>
        /// Gets the value of a Text/Description field in a Business Process Flow.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <returns>The value</returns>
        /// <example>xrmBrowser.BusinessProcessFlow.GetValue("mobilephone");</example>
        public BrowserCommandResult<string> GetValue(string field)
        {
            return this.Execute($"Get BPF Text Field Value: {field}", driver =>
            {
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", field.ToLower())));

                string text = string.Empty;
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", field.ToLower()))))
                {
                    var fieldElement = driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", field.ToLower())));

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
                    throw new InvalidOperationException($"Unable to locate field '{field}' in the Business Process Flow. Please verify the field exists and try again.");

                return text;
            });
        }

        /// <summary>
        /// Placeholder: CompositeControls are not currently supported in BPFs.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.GetValue(new CompositeControl { Id = "fullname", Fields = fields });</example>
        public  BrowserCommandResult<string> GetValue(CompositeControl control)
        {
            return this.Execute($"Get BPF CompositeControl Value: {control.Id}", driver =>
            {
                string text = string.Empty;

                /*
                driver.WaitUntilVisible(By.Id(control.Id));

                driver.ClickWhenAvailable(By.Id(control.Id));

                if (driver.HasElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut])))
                {
                    var compcntrl =
                        driver.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut]));

                    foreach (var field in control.Fields)
                    {
                        compcntrl.FindElement(By.Id(Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id)).Click();

                        var result = compcntrl.FindElements(By.TagName("input"))
                            .ToList()
                            .FirstOrDefault(i => i.GetAttribute("id").Contains(field.Id));
                        text += result.GetAttribute("value");
                    }

                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click();
                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");
                */
                return text;
            });
        }

        /// <summary>
        /// Gets the value of an option set / picklist in a Business Process Flow.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        public BrowserCommandResult<string> GetValue(OptionSet option)
        {
            return this.Execute($"Get BPF OptionSet Value: {option.Name}", driver =>
            {
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.OptionSetFieldContainer].Replace("[NAME]", option.Name.ToLower())));

                string text = string.Empty;
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.OptionSetFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var input = driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.OptionSetFieldContainer].Replace("[NAME]", option.Name.ToLower())));
                    text = input.Text;
                }
                else
                    throw new InvalidOperationException($"Unable to locate OptionSet '{option.Name}' in the Business Process Flow. Please verify the OptionSet exists and try again.");

                return text;
            });
        }

        /// <summary>
        /// Gets the value of a Lookup in a Business Process Flow.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.GetValue(new Lookup { Name = "primarycontactid" });</example>
        public BrowserCommandResult<string> GetValue(LookupItem control)
        {
            return this.Execute($"Get BPF Lookup Value: {control.Name}", driver =>
            {
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower())));

                string lookupValue = string.Empty;
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower()))))
                {
                    var input = driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower())));
                    lookupValue = input.Text;
                }
                else
                    throw new InvalidOperationException($"Unable to locate Lookup '{control.Name}' in the Business Process Flow. Please verify the Lookup exists and try again.");

                return lookupValue;
            });
        }

        /// <summary>
        /// Gets the value of a Checkbox/TwoOption field in a Business Process Flow.
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.BusinessProcessFlow.GetValue(new TwoOption {Name="creditonhold"});</example>
        public BrowserCommandResult<bool> GetValue(TwoOption option)
        {

            return this.Execute(GetOptions($"Get BPF Checkbox/TwoOption Value: {option.Name}"), driver =>
            {
                bool check = false;

                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));
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
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' in the Business Process Flow. Please verify the TwoOption field exists and try again.");

                return check;
            });
        }

        /// <summary>
        /// Gets the value of a DateTime field in a Business Process Flow.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.BusinessProcessFlow.GetValue(new DateTime {Name = "birthdate"));</example>
        public BrowserCommandResult<string> GetValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Get BPF DateTime Value: {date.Name}"), driver =>
            {
                string dateValue = "";
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.DateFieldContainer].Replace("[NAME]", date.Name.ToLower()))))
                {

                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.DateFieldContainer].Replace("[NAME]", date.Name.ToLower())));

                    // Check whether the DateTime field has an existing value
                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var label = fieldElement.FindElement(By.TagName("label"));
                        dateValue = label.Text;
                    }
                }
                else
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' in the Business Process Flow. Please verify the DateTime field exists and try again.");

                return dateValue;
            });
        }

        /// <summary>
        /// Hides the Business Process flow UI.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.Hide();</example>
        public BrowserCommandResult<bool> Hide(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Hide", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Hide])))
                    throw new Exception("Business Process Flow Hide Element does not exist");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Hide]));

                return true;
            });
        }

        /// <summary>
        /// Moves to the Next stage in the Business Process Flow.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.NextStage();</example>
        public BrowserCommandResult<bool> NextStage(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Next Stage", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.NextStage])))
                    throw new Exception("Business Process Flow Next Stage Element does not exist");

                if(driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.NextStage])).GetAttribute("class").Contains("disabled"))
                    throw new Exception("Business Process Flow Next Stage Element is not enabled");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.NextStage]));

                return true;
            });
        }

        /// <summary>
        /// Moves to the Next stage in the Business Process Flow.
        /// Useful when the 'Next Stage' button prompts to choose an entity value
        /// </summary>
        /// <param name="index">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.NextStage();</example>
        public BrowserCommandResult<bool> NextStage([System.ComponentModel.DataAnnotations.Range(0, 9)]int index, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Next Stage", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.NextStageMenu])))
                    throw new Exception("Business Process Flow Next Stage Element does not exist");

                if (driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.NextStageMenu])).GetAttribute("class").Contains("disabled"))
                    throw new Exception("Business Process Flow Next Stage Element is not enabled");

                driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.NextStageMenu])).Click(true);

                //div class=navigateMenuSection

                var recordMenu = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.NextStageMenuOptions]));

                var records = NextStageGetRecords(recordMenu).Value;

                if (records.Count < index)
                    throw new InvalidOperationException($"Next Stage Record List does not have {index + 1} items.");

                var recordItem = records[index];
                recordItem.Element.Click(true);

                return true;
            });
        }

        /// <summary>
        /// Gets records from the BPF Next Stage Menu
        /// </summary>
        /// <param name="dialog"></param>
        internal BrowserCommandResult<List<ListItem>> NextStageGetRecords(IWebElement dialog)
        {
            var list = new List<ListItem>();
            var dialogItems = dialog.FindElements(By.TagName("div"));

            foreach (var dialogItem in dialogItems)
            {
                if (dialogItem.GetAttribute("class") != null && dialogItem.GetAttribute("class").Contains("navigateMenuItem"))
                {
                    var title = dialogItem.Text;

                    list.Add(new ListItem()
                    {
                        Title = title,
                        Element = dialogItem
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// Moves to the Previous stage in the Business Process Flow.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.PreviousStage();</example>
        public BrowserCommandResult<bool> PreviousStage(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Previous Stage", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.PreviousStage])))
                    throw new Exception("Business Process Flow Previous Stage Element does not exist");

                if (driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.PreviousStage])).GetAttribute("class").Contains("disabled"))
                    throw new Exception("Business Process Flow Previous Stage Element is not enabled");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.PreviousStage]));

                return true;
            });
        }

        /// <summary>
        /// Selects the Business Process Flow from the Dialog.
        /// </summary>
        /// <param name="name">The name of the business process flow you want to select.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SelectBusinessProcessFlow("Opportunity Sales Process");</example>
        public BrowserCommandResult<bool> SelectBusinessProcessFlow(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Delete"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.ProcessFlowHeader]),
                    new TimeSpan(0, 0, 10),
                    "The Select Business Process Flow dialog is not available.");

                if (driver.FindElement(By.ClassName(Elements.CssClass[Reference.Dialogs.SwitchProcess.SelectedRadioButton])).Text.Split('\r')[0] != name)
                {
                    var processes = driver.FindElements(By.ClassName(Elements.CssClass[Reference.Dialogs.SwitchProcess.Process]));
                    IWebElement element = null;

                    foreach (var process in processes)
                    {
                        if (process.Text == name)
                        {
                            element = process;
                            break;
                        }
                    }

                    if (element != null)
                        element.Click();
                    else
                        throw new InvalidOperationException($"The Business Process with name: '{name}' does not exist");

                    driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.Ok]));
                }
                else
                {
                    throw new InvalidOperationException($"The Business Process with name: '{name}' is already selected");
                }
                return true;
            });
        }

        /// <summary>
        /// Selects the Business Process Flow stage.
        /// </summary>
        /// <param name="stagenumber">The stage number that you would like to select. The stages start with 0.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SelectStage(0);</example>
        public BrowserCommandResult<bool> SelectStage(int stagenumber, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Select Stage", driver =>
            {
                var xpath = Elements.Xpath[Reference.BusinessProcessFlow.SelectStage].Replace("[STAGENUM]", stagenumber.ToString());

               var stageButton = driver.FindElements(By.XPath(xpath));

                if (stageButton.Any())
                {
                    stageButton.First().Click(true);
                }
                else
                { throw new Exception("Business Process Flow Select Stage Element does not exist"); }
                
                return true;
            });
        }

        /// <summary>
        /// Sets the current selected Stage as Active.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetActive();</example>
        public BrowserCommandResult<bool> SetActive(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute("Set Active", driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.SetActive])))
                    throw new Exception("Business Process Flow Set Active Element does not exist");

                if (driver.FindElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.SetActive])).GetAttribute("class").Contains("hidden"))
                    throw new Exception("The Business Process is already Active");

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.SetActive]));

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Text field in a Business Process Flow
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue("firstname", "Test");</example>
        public new BrowserCommandResult<bool> SetValue(string field, string value)
        {
            return this.Execute(GetOptions($"Set BPF Text Field Value: {field}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", field.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", field.ToLower())));

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
                }
                else
                    throw new InvalidOperationException($"Unable to locate field '{field}' in the Business Process Flow. Please verify the field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Checkbox/TwoOption field in a Business Process Flow.
        /// </summary>
        /// <param name="option">The TwoOption field you want to set</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue(new TwoOption { Name = "creditonhold"});</example>
        public new BrowserCommandResult<bool> SetValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Set BPF Checkbox/TwoOption Value: {option.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));

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
                    throw new InvalidOperationException($"Unable to locate TwoOption field '{option.Name}' in the Business Process Flow. Please verify the TwoOption field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of an option set / picklist in a Business Process Flow.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public new BrowserCommandResult<bool> SetValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Set BPF OptionSet Value: {option.Name}"), driver =>
            {

                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.OptionSetFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.OptionSetFieldContainer].Replace("[NAME]", option.Name.ToLower())));
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
                    throw new InvalidOperationException($"Unable to locate OptionSet '{option.Name}' in the Business Process Flow. Please verify the OptionSet exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Lookup in a Business Process Flow.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        public new BrowserCommandResult<bool> SetValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Set BPF Lookup Value: {control.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower()))))
                {
                    driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower())));

                    var lookupInput = driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower())));

                    if (lookupInput.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {control.Name} is not Lookup control");

                    lookupInput.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click(true);

                    var dialogName = $"Dialog_header_process_{control.Name}_IMenu";

                    var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;


                    if (control.Value != null)
                    {

                        if (!dialogItems.Exists(x => x.Title.ToLower() == control.Value.ToLower()))
                            throw new InvalidOperationException($"List does not contain value {control.Value}.");

                        var dialogItem = dialogItems.Where(x => x.Title.ToLower() == control.Value.ToLower()).First();

                        dialogItem.Element.Hover(driver,true);
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
                    throw new InvalidOperationException($"Unable to locate Lookup '{control.Name}' in the Business Process Flow. Please verify the Lookup exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date field in a Business Process Flow.
        /// </summary>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.BusinessProcessFlow.SetValue(new DateTime {Name = "birthdate", Value= DateTime.Parse("11/1/1980"}));</example>
        public new BrowserCommandResult<bool> SetValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Set BPF DateTime Value: {date.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.DateFieldContainer].Replace("[NAME]", date.Name.ToLower()))))
                {
                    var fieldElement = driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.BusinessProcessFlow.DateFieldContainer].Replace("[NAME]", date.Name.ToLower())));

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
                    throw new InvalidOperationException($"Unable to locate DateTime field '{date.Name}' in the Business Process Flow. Please verify the DateTime field exists and try again.");

                return true;
            });
        }

        /// <summary>
        /// Placeholder: MultiValueOptionSets are not currently supported in BPFs.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public new BrowserCommandResult<bool> SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Set BPF MultiValueOptionSet Value: {option.Name}"), driver =>
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
        /// <param name="control">The Composite control values you want to set.</param>
        /// <example>xrmBrowser.BusinessProcessFlow.SetValue(new CompositeControl {Id = "fullname", Fields = fields});</example>
        public new BrowserCommandResult<bool> SetValue(CompositeControl control)
        {
            return this.Execute(GetOptions($"Set BPF CompositeControl Value: {control.Id}"), driver =>
            {
                /*
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

                */
                return true;
            });
        }
    }
}