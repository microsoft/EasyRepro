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
                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Checkbox field.
        /// </summary>
        /// <param name="field">Field name or ID.</param>
        /// <param name="check">If set to <c>true</c> [check].</param>
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
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        public new BrowserCommandResult<bool> SetValue(string field, DateTime date)
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

                    input.FindElement(By.TagName("input")).SendKeys(date.ToShortDateString());
                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();

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
                        input.FindElement(By.TagName("textarea")).SendKeys(value);
                    else
                        input.FindElement(By.TagName("input")).SendKeys(value);

                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Field.
        /// </summary>
        /// <param name="field">The field .</param>
        public new BrowserCommandResult<bool> SetValue(Field field)
        {
            return this.Execute(GetOptions(""), driver =>
            {
                if (driver.HasElement(By.Id(field.Id)))
                {
                    var input = driver.ClickWhenAvailable(By.Id(field.Id));

                    //Check to see if focus is on field already
                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])) != null)
                        input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();
                    else
                        input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.ValueClass])).Click();

                    if (input.FindElements(By.TagName("textarea")).Count > 0)
                        input.FindElement(By.TagName("textarea")).SendKeys(field.Value);
                    else
                        input.FindElement(By.TagName("input")).SendKeys(field.Value);

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
        public new BrowserCommandResult<bool> SetValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Set Value: {option.Name}"), driver =>
            {
                if (driver.HasElement(By.Id(option.Name)))
                {
                    var input = driver.ClickWhenAvailable(By.Id(option.Name));

                    var select = input.FindElement(By.TagName("select"));
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
        /// Sets the value of a Composite control.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        public new BrowserCommandResult<bool> SetValue(CompositeControl control)
        {
            return this.Execute(GetOptions($"Set Conposite Control Value: {control.Id}"), driver =>
            {
                if (!driver.HasElement(By.Id(control.Id)))
                    return false;

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

                        result?.SendKeys(field.Value);
                    }

                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click();
                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");

                return true;
            });
        }

    }
}