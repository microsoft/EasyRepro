// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api
{

    /// <summary>
    /// Xrm Page
    /// </summary>
    public class XrmPage : BrowserPage
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="XrmPage"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public XrmPage(InteractiveBrowser browser) : base(browser)
        {
        }

        /// <summary>
        /// Sets the value of a Checkbox field.
        /// </summary>
        /// <param name="field">Field name or ID.</param>
        /// <param name="check">If set to <c>true</c> [check].</param>
        /// <example>xrmBrowser.Entity.SetValue("creditonhold",true);</example>
        public BrowserCommandResult<bool> SetValue(string field, bool check)
        {
            //return this.Execute($"Set Value: {field}", SetValue, field, check);
            return this.Execute(GetOptions($"Set Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var input = driver.FindElement(By.Id(field));
                    var checkBox = input.FindElement(By.TagName("input"));
                    var bCheck = checkBox.GetAttribute("value") == "1";

                    if (bCheck != check)
                    {
                        checkBox.Click();
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        public BrowserCommandResult<bool> SetValue(string field, DateTime date)
        {
            //return this.Execute($"Set Value: {field}", SetValue, field, date);
            return this.Execute(GetOptions($"Set Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var fieldElement = driver.ClickWhenAvailable(By.Id(field));

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
                        input.SendKeys(date.ToShortDateString());
                        input.SendKeys(Keys.Enter);
                    }
                    else
                    {
                        input.SendKeys(date.ToShortDateString());
                        input.SendKeys(Keys.Enter);
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Text/Description field.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <param name="value">The value.</param>
        /// <example>xrmBrowser.Entity.SetValue("name", "Test API Account");</example>
        public BrowserCommandResult<bool> SetValue(string field, string value)
        {
            //return this.Execute($"Set Value: {field}", SetValue, field, value);
            var returnval = this.Execute(GetOptions($"Set Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    driver.WaitUntilVisible(By.Id(field));

                    var fieldElement = driver.FindElement(By.Id(field));
                    if (fieldElement.IsVisible(By.TagName("a")))
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        var element = fieldElement.FindElement(By.TagName("a"));
                        js.ExecuteScript("arguments[0].setAttribute('style', 'pointer-events: none; cursor: default')", element);
                    }
                    fieldElement.Click();

                    try
                    { 
                        //Check to see if focus is on field already
                        if (fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])) != null)
                            fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();
                        else
                            fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.ValueClass])).Click();
                    }
                    catch (NoSuchElementException) { }

                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("textarea")).Clear();
                        fieldElement.FindElement(By.TagName("textarea")).SendKeys(value);
                    }
                    else if(fieldElement.TagName =="textarea")
                    {
                        fieldElement.Clear();
                        fieldElement.SendKeys(value);
                    }
                    else
                    {
                        //BugFix - Setvalue -The value is getting erased even after setting the value ,might be due to recent CSS changes.
                        //driver.ExecuteScript("Xrm.Page.getAttribute('" + field + "').setValue('')");
                        fieldElement.FindElement(By.TagName("input")).SendKeys(value, true);
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
            return returnval;
        }

        /// <summary>
        /// Sets the value of a Field.
        /// </summary>
        /// <param name="field">The field .</param>
        public BrowserCommandResult<bool> SetValue(Field field)
        {
            return this.Execute(GetOptions($"Set Value: {field.Name}"), driver =>
            {
                driver.WaitUntilVisible(By.Id(field.Id));

                if (driver.HasElement(By.Id(field.Id)))
                {
                    var fieldElement = driver.ClickWhenAvailable(By.Id(field.Id));

                    //Check to see if focus is on field already
                    if (fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])) != null)
                        fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();
                    else
                        fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.ValueClass])).Click();


                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("textarea")).Clear();
                        fieldElement.FindElement(By.TagName("textarea")).SendKeys(field.Value);
                    }
                    else
                    {
                        fieldElement.FindElement(By.TagName("input")).Clear();
                        fieldElement.FindElement(By.TagName("input")).SendKeys(field.Value);
                    }

                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public BrowserCommandResult<bool> SetValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Set Value: {option.Name}"), driver =>
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
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a multi-value picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public BrowserCommandResult<bool> SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Set Value: {option.Name}"), driver =>
            {
                driver.WaitUntilVisible(By.Id(option.Name));

                if (driver.HasElement(By.Id(option.Name)))
                {
                    var container = driver.ClickWhenAvailable(By.Id(option.Name));
                    
                    if(removeExistingValues)
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
        /// Sets the value of a Composite control.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <example>xrmBrowser.Entity.SetValue(new CompositeControl() {Id = "fullname", Fields = fields});</example>
        public BrowserCommandResult<bool> SetValue(CompositeControl control)
        {
            return this.Execute(GetOptions($"Set Conposite Control Value: {control.Id}"), driver =>
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
        /// Sets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.Entity.SetValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        public BrowserCommandResult<bool> SetValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Set Lookup Value: {control.Name}"), driver =>
            {
                if (driver.HasElement(By.Id(control.Name)))
                {
                    driver.WaitUntilVisible(By.Id(control.Name));

                    var input = driver.ClickWhenAvailable(By.Id(control.Name));

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {control.Name} is not lookup");

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click();

                    var dialogName = $"Dialog_{control.Name}_IMenu";
                    var dialog = driver.FindElement(By.Id(dialogName));

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
                    throw new InvalidOperationException($"Field: {control.Name} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Gets the value of a Text/Description field.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <returns>The value</returns>
        /// <example>xrmBrowser.Entity.GetValue("mobilephone");</example>
        public BrowserCommandResult<string> GetValue(string field)
        {
            return this.Execute($"Get Value: {field}", driver =>
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
        /// Gets the value of a Field.
        /// </summary>
        /// <param name="field">The field .</param>
        /// <returns>The value</returns>
        public BrowserCommandResult<bool> GetValue(Field field)
        {
            return this.Execute($"Get Value: {field.Name}", driver =>
            {
                var text = string.Empty;

                driver.WaitUntilVisible(By.Id(field.Id));

                if (driver.HasElement(By.Id(field.Id)))
                {
                    var fieldElement = driver.ClickWhenAvailable(By.Id(field.Id));

                    //Check to see if focus is on field already
                    if (fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])) != null)
                        fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();
                    else
                        fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.ValueClass])).Click();


                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("textarea")).GetAttribute("value");
                    }
                    else
                    {
                        fieldElement.FindElement(By.TagName("input")).GetAttribute("value");
                    }

                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Gets the value of a Composite control.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <example>xrmBrowser.Entity.GetValue(new CompositeControl() { Id = "fullname", Fields = fields });</example>
        public BrowserCommandResult<string> GetValue(CompositeControl control)
        {
            return this.Execute($"Get Conposite Control Value: {control.Id}", driver =>
            {
                string text = string.Empty;

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

                return text;
            });
        }

        /// <summary>
        /// Gets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        public BrowserCommandResult<string> GetValue(OptionSet option)
        {
            return this.Execute($"Get Value: {option.Name}", driver =>
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
        /// Gets the value of a Lookup.
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
        /// Switches to content frame in the CRM application.
        /// </summary>
        public bool SwitchToContentFrame()
        {
            return this.Execute("Switch to content frame", driver => SwitchToContent());
        }

        internal bool SwitchToContent()
        {
            Browser.Driver.SwitchTo().DefaultContent();
            //wait for the content panel to render
            Browser.Driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.ContentPanel]));

            //find the crmContentPanel and find out what the current content frame ID is - then navigate to the current content frame
            var currentContentFrame = Browser.Driver.FindElement(By.XPath(Elements.Xpath[Reference.Frames.ContentPanel]))
                .GetAttribute(Elements.ElementId[Reference.Frames.ContentFrameId]);

            Browser.Driver.SwitchTo().Frame(currentContentFrame);

            return true;
        }

        /// <summary>
        /// Switches to dialog frame in the CRM application.
        /// </summary>

        public bool SwitchToDialogFrame()
        {
            return this.Execute("Switch to dialog frame", driver => SwitchToDialog(0));
        }

        internal bool SwitchToDialog(int frameIndex = 0)
        {
            var index = "";
            if (frameIndex > 0)
                index = frameIndex.ToString();

            Browser.Driver.SwitchTo().DefaultContent();
            
            // Check to see if dialog is InlineDialog or popup
            var inlineDialog = Browser.Driver.HasElement(By.XPath(Elements.Xpath[Reference.Frames.DialogFrame].Replace("[INDEX]", index)));
            if (inlineDialog)
            {
                //wait for the content panel to render
                Browser.Driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.DialogFrame].Replace("[INDEX]", index)),
                                                  new TimeSpan(0, 0, 2),
                                                  d => { Browser.Driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.DialogFrameId].Replace("[INDEX]", index)); });
            }
            else
            {
                SwitchToPopup();
            }
            return true;
        }

        /// <summary>
        /// Switches to Quick Find frame in the CRM application.
        /// </summary>
        public bool SwitchToQuickCreateFrame()
        {
            return this.Execute("Switch to Quick Create Frame", driver => SwitchToQuickCreate());
        }

        internal bool SwitchToQuickCreate()
        {
            Browser.Driver.SwitchTo().DefaultContent();
            //wait for the content panel to render
            Browser.Driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.QuickCreateFrame]));

            Browser.Driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.QuickCreateFrameId]);

            return true;

        }

        /// <summary>
        /// Switches to related frame in the CRM application.
        /// </summary>
        public bool SwitchToRelatedFrame()
        {

            return this.Execute("Switch to Related Frame", driver => SwitchToRelated());

        }

        internal bool SwitchToRelated()
        {
            SwitchToContent();

            Browser.Driver.WaitUntilAvailable(By.Id(Browser.ActiveFrameId));

            Browser.Driver.SwitchTo().Frame(Browser.ActiveFrameId + "Frame");

            return true;
        }


        /// <summary>
        /// SwitchToDefaultContent
        /// </summary>
        public bool SwitchToDefaultContent()
        {
            return this.Execute("Switch to Default Content", driver => SwitchToDefault());
        }

        internal bool SwitchToDefault()
        {
            Browser.Driver.SwitchTo().DefaultContent();

            return true;
        }

        /// <summary>
        /// Switches to Wizard frame in the CRM application.
        /// </summary>
        public bool SwitchToWizardFrame()
        {

            return this.Execute("Switch to Wizard Frame", driver => SwitchToWizard());

        }

        internal bool SwitchToWizard()
        {
            SwitchToDialog();

            Browser.Driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.WizardFrame]));

            Browser.Driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.WizardFrameId]);

            return true;
        }

        /// <summary>
        /// Switches to Wizard frame in the CRM application.
        /// </summary>
        public bool SwitchToPopupWindow()
        {

            return this.Execute("Switch to Pop Up Window", driver => SwitchToPopup());

        }

        internal bool SwitchToPopup()
        {
            Browser.Driver.LastWindow().SwitchTo().ActiveElement();

            return true;
        }

        public bool SwitchToViewFrame()
        {
            return this.Execute("Switch to View frame", driver => SwitchToView());
        }

        internal bool SwitchToView()
        {
            Browser.Driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.ViewFrameId]);

            return true;
        }


        internal BrowserCommandOptions GetOptions(string commandName)
        {
            return new BrowserCommandOptions(Constants.DefaultTraceSource,
                commandName,
                0,
                0,
                null,
                true,
                typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        /// <summary>
        /// Gets the Commands
        /// </summary>
        /// <param name="moreCommands">The MoreCommands</param>
        /// <example></example>
        public BrowserCommandResult<ReadOnlyCollection<IWebElement>> GetCommands(bool moreCommands = false)
        {
            return this.Execute(GetOptions("Get Command Bar Buttons"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.CommandBar.RibbonManager]), new TimeSpan(0, 0, 5));

                IWebElement ribbon = null;
                if (moreCommands)
                    ribbon = driver.FindElement(By.XPath(Elements.Xpath[Reference.CommandBar.List]));
                else
                    ribbon = driver.FindElement(By.XPath(Elements.Xpath[Reference.CommandBar.RibbonManager]));

                var items = ribbon.FindElements(By.TagName("li"));

                return items;//.Where(item => item.Text.Length > 0).ToDictionary(item => item.Text, item => item.GetAttribute("id"));
            });
        }

        /// <summary>
        /// Clicks the  Command Button on the menu
        /// </summary>
        /// <param name="name">The Name of the command</param>
        /// <param name="subName">The SubName</param>
        /// <param name="moreCommands">The MoreCommands</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.CommandBar.ClickCommand("New");</example>
        internal bool ClickCommandButton(string name, string subName = "", bool moreCommands = false, int thinkTime = Constants.DefaultThinkTime)
        {
            var driver = Browser.Driver;

            var buttons = GetCommands(false).Value;
            var button = buttons.FirstOrDefault(x => x.Text.Split('\r')[0].ToLowerString() == name.ToLowerString());

            if (button == null)
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.CommandBar.MoreCommands]));
                buttons = GetCommands(true).Value;
                button = buttons.FirstOrDefault(x => x.Text.Split('\r')[0].ToLowerString() == name.ToLowerString());
            }

            if (button == null)
            {
                throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");
            }

            if (string.IsNullOrEmpty(subName))
            {
                button.Click(true);
            }
            else
            {

                button.FindElement(By.ClassName(Elements.CssClass[Reference.CommandBar.FlyoutAnchorArrow])).Click();

                var flyoutId = button.GetAttribute("id").Replace("|", "_").Replace(".", "_") + "Menu";
                var subButtons = driver.FindElement(By.Id(flyoutId)).FindElements(By.ClassName("ms-crm-CommandBar-Menu"));
                var item = subButtons.FirstOrDefault(x => x.Text.ToLowerString() == subName.ToLowerString());
                if (item == null) { throw new InvalidOperationException($"The sub menu item '{subName}' is not found."); }

                item.Click();
            }

            driver.WaitForPageToLoad();
            return true;
        }

        /// <summary>
        /// Clicks the  Command
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="subName">The subName</param>
        /// <param name="moreCommands">The moreCommands</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Related.ClickCommand("ADD NEW CASE");</example>
        public BrowserCommandResult<bool> ClickCommand(string name, string subName = "", bool moreCommands = false, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Click Command"), driver =>
            {
                ClickCommandButton(name, subName, moreCommands, thinkTime);
                return true;
            });
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        /// <example></example>
        public BrowserCommandResult<bool> Refresh(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Refresh"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Grid.Refresh]));

                return true;
            });
        }

        /// <summary>
        /// Firsts the page.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Grid.FirstPage();</example>
        public BrowserCommandResult<bool> FirstPage(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("FirstPage"), driver =>
            {
                var firstPageIcon = driver.FindElement(By.XPath(Elements.Xpath[Reference.Grid.FirstPage]));

                if (firstPageIcon.GetAttribute("disabled") != null)
                    return false;
                else
                    firstPageIcon.Click();
                return true;
            });
        }

        /// <summary>
        /// Nexts the page.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Grid.NextPage();</example>
        public BrowserCommandResult<bool> NextPage(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Next"), driver =>
            {
                var nextIcon = driver.FindElement(By.XPath(Elements.Xpath[Reference.Grid.NextPage]));

                if (nextIcon.GetAttribute("disabled") != null)
                    return false;
                else
                    nextIcon.Click();
                return true;
            });
        }
        
        /// <summary>
        /// Previouses the page.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Grid.PreviousPage();</example>
        public BrowserCommandResult<bool> PreviousPage(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("PreviousPage"), driver =>
            {
                var previousIcon = driver.FindElement(By.XPath(Elements.Xpath[Reference.Grid.PreviousPage]));

                if (previousIcon.GetAttribute("disabled") != null)
                    return false;
                else
                    previousIcon.Click();
                return true;
            });
        }

        /// <summary>
        /// Toggles the select all.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Grid.SelectAllRecords();</example>
        public BrowserCommandResult<bool> SelectAllRecords(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("ToggleSelectAll"), driver =>
            {
                // We can check if any record selected by using
                // driver.FindElements(By.ClassName("ms-crm-List-SelectedRow")).Count == 0
                // but this function doesn't check it.
                var selectAll = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Grid.ToggleSelectAll]),
                          "The Toggle SelectAll is not available.");

                selectAll.Click();

                return true;
            });
        }

        /// <summary>
        /// Get the Grid Items
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<List<GridItem>> GetGridItems(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Get Grid Items"), driver =>
            {
                var returnList = new List<GridItem>();

                var itemsTable = driver.FindElement(By.XPath(@"//*[@id=""gridBodyTable""]/tbody"));
                var columnGroup = driver.FindElement(By.XPath(@"//*[@id=""gridBodyTable""]/colgroup"));

                var rows = itemsTable.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    if (!string.IsNullOrEmpty(row.GetAttribute("oid")))
                    {
                        Guid id = Guid.Parse(row.GetAttribute("oid"));
                        var link =
                            $"{new Uri(driver.Url).Scheme}://{new Uri(driver.Url).Authority}/main.aspx?etn={row.GetAttribute("otypename")}&pagetype=entityrecord&id=%7B{id:D}%7D";

                        var item = new GridItem
                        {
                            EntityName = row.GetAttribute("otypename"),
                            Id = id,
                            Url = new Uri(link)
                        };

                        var cells = row.FindElements(By.TagName("td"));
                        var idx = 0;

                        foreach (var column in columnGroup.FindElements(By.TagName("col")))
                        {
                            var name = column.GetAttribute<string>("name");

                            if (!string.IsNullOrEmpty(name)
                                && column.GetAttribute("class").Contains(Elements.CssClass[Reference.Grid.DataColumn])
                                && cells.Count > idx)
                            {
                                item[name] = cells[idx].Text;
                            }

                            idx++;
                        }

                        returnList.Add(item);
                    }
                }

                return returnList;
            });
        }

        /// <summary>
        /// Set Lookup Value for the field
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="index">The Index</param>
        /// <example>xrmBrowser.Entity.SelectLookup("customerid", 0);</example>
        public BrowserCommandResult<bool> SelectLookup(string field, [Range(0, 9)]int index)
        {
            return this.Execute(GetOptions($"Set Lookup Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var input = driver.ClickWhenAvailable(By.Id(field));

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {field} is not lookup");

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click();

                    var dialogName = $"Dialog_{field}_IMenu";
                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if (dialogItems.Count < index)
                        throw new InvalidOperationException($"List does not have {index + 1} items.");

                    var dialogItem = dialogItems[index];
                    dialogItem.Element.Click();
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Set Lookup Value for the field
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="value">The Lookup value</param>
        public BrowserCommandResult<bool> SelectLookup(string field, string value)
        {
            return this.Execute(GetOptions($"Set Lookup Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var input = driver.ClickWhenAvailable(By.Id(field));

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {field} is not lookup");

                    var lookupIcon = input.FindElement(By.ClassName("Lookup_RenderButton_td"));
                    lookupIcon.Click();

                    var dialogName = $"Dialog_{field}_IMenu";
                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if (!dialogItems.Exists(x => x.Title == value))
                        throw new InvalidOperationException($"List does not have {value}.");

                    var dialogItem = dialogItems.Where(x => x.Title == value).First();
                    dialogItem.Element.Click();
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Set Lookup Value for the field
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="openLookupPage">The Open Lookup Page</param>
        public BrowserCommandResult<bool> SelectLookup(string field, bool openLookupPage = true)
        {
            return this.Execute(GetOptions($"Set Lookup Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var input = driver.ClickWhenAvailable(By.Id(field));

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {field} is not lookup");

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click();

                    var dialogName = $"Dialog_{field}_IMenu";
                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if (dialogItems.Any())
                    {
                        var dialogItem = dialogItems.Last();
                        dialogItem.Element.Click();
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Opens the dialog
        /// </summary>
        /// <param name="dialog"></param>
        public BrowserCommandResult<List<ListItem>> OpenDialog(IWebElement dialog)
        {
            var list = new List<ListItem>();
            var dialogItems = dialog.FindElements(By.TagName("li"));

            foreach (var dialogItem in dialogItems)
            {
                if (dialogItem.GetAttribute("role") != null && dialogItem.GetAttribute("role") == "menuitem")
                {
                    var links = dialogItem.FindElements(By.TagName("a"));

                    if (links != null && links.Count > 1)
                    {
                        var title = links[1].GetAttribute("title");

                        list.Add(new ListItem()
                        {
                            Title = title,
                            Element = links[1]
                        });
                    }
                }
            }

            return list;
        }


    }
}
