// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
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
        /// Firsts the grid on a Grid or Related page.
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
        /// Dismiss the Alert If Present
        /// </summary>
        /// <param name="stay"></param>
        public BrowserCommandResult<bool> IsDialogFrameVisible(bool visible = false)
        {
            return this.Execute(GetOptions("Check If Dialog Frame Is Visible"), driver =>
            {
                driver.WaitUntilVisible(By.Id("InlineDialog"),
                    new TimeSpan(0, 0, 5),
                    e => visible = true,
                    () => visible = false);

                return visible;
            });
        }

        /// <summary>
        /// Nexts the grid on a Grid or Related page.
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
                
                nextIcon.Click();
                return true;
            });
        }

        /// <summary>
        /// Previouses the grid on a Grid or Related page.
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
        /// Refreshes the grid from a Grid or Related page.
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
        /// Opens the dialog
        /// </summary>
        /// <param name="dialog"></param>
        public BrowserCommandResult<List<ListItem>> OpenDialog(IWebElement dialog)
        {
            // Delay briefly to ensure we can get items from the dialog
            Browser.ThinkTime(500);

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
        /// Set Lookup Value for the field on an entity header
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="clearFieldValue">Remove Existing Field Value, if present. False = Click the existing value</param>
        /// <param name="openLookupPage">The Open Lookup Page</param>
        public BrowserCommandResult<bool> SelectHeaderLookup(LookupItem field, bool clearFieldValue = true, bool openLookupPage = true)
        {
            return this.Execute(GetOptions($"Select Header Lookup for: {field.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", field.Name))))
                {
                    var fieldContainer = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", field.Name)));

                    if (fieldContainer.Text != "" && clearFieldValue)
                    {
                        fieldContainer.SendKeys(Keys.Clear);
                    }
                    else if (fieldContainer.Text != "" && !clearFieldValue)
                    {
                        fieldContainer.Click();
                        return true;
                    }

                    var input = driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer_Header].Replace("[NAME]", field.Name)));

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {field.Name} is not lookup");

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click();

                    Browser.ThinkTime(1000);
                    var dialogName = $"Dialog_header_{field.Name}_IMenu";
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
        /// Set Lookup Value for the field
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="index">The Index</param>
        /// <example>xrmBrowser.Entity.SelectLookup(new LookupItem {Name = "lookupSchemaName", Index = 0 });</example>
        public BrowserCommandResult<bool> SelectLookup(LookupItem control)
        {
            return this.Execute(GetOptions($"Set Lookup Value: {control.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower())));
                    var dialogName = $"Dialog_{control.Name}_IMenu";
                    IWebElement dialog;
                    List<ListItem> dialogItems;
                    ListItem dialogItem;

                    // If field contains a value, clear the value and then set the new desired value
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

                        dialog = driver.WaitUntilAvailable(By.Id(dialogName));
                        dialogItems = OpenDialog(dialog).Value;

                        if (dialogItems.Any())
                        {
                            dialogItem = dialogItems.Last();
                            dialogItem.Element.Click();
                        }

                        SwitchToDialog();

                        Browser.ThinkTime(500);

                        driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.LookUp.Remove])).Click(true);

                        SwitchToContent();

                    }

                    fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.LookupFieldContainer].Replace("[NAME]", control.Name.ToLower())));
                    fieldElement.Hover(driver);
                    fieldElement.Click(true);

                    var lookupSearchIcon = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", control.Name)));

                    if (Browser.Options.BrowserType != BrowserType.Firefox)
                    {
                        var lookupIcon = fieldElement.FindElement(By.ClassName("Lookup_RenderButton_td"));
                        lookupSearchIcon = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", control.Name)));
                        lookupIcon.Hover(driver);
                        lookupIcon.Click(true);

                        /*
                        lookupSearchIcon.Hover(driver, true);
                        lookupSearchIcon.Click(true);

                        var lookupSearchIconImage = driver.FindElement(By.TagName("img"));
                        lookupSearchIconImage.Hover(driver, true);

                        Actions clickAction = new Actions(driver).SendKeys(Keys.Enter);
                        clickAction.Build().Perform();
                        */
                    }
                    else
                    {
                        var lookupIcon = fieldElement.FindElement(By.ClassName("Lookup_RenderButton_td"));
                        lookupSearchIcon = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", control.Name)));
                        lookupIcon.Hover(driver);
                        lookupIcon.Click(true);
                    }


                    dialog = driver.WaitUntilAvailable(By.Id(dialogName));
                    dialogItems = OpenDialog(dialog).Value;

                    if (dialogItems.Count < control.Index)
                        throw new InvalidOperationException($"List does not have {control.Index + 1} items.");

                    dialogItem = dialogItems[control.Index];
                    dialogItem.Element.Click();

                }
                else
                    throw new InvalidOperationException($"Field: {control.Name} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Set Lookup Value for the field
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="index">The Index</param>
        /// <example>xrmBrowser.Entity.SelectLookup("customerid", 0);</example>
        [Obsolete("This method has been deprecated. Please use the new SelectLookup(new LookupItem { Name = \"lookupSchemaName\", Index = 0})")]
        public BrowserCommandResult<bool> SelectLookup(string field, [Range(0, 9)]int index)
        {
            return this.Execute(GetOptions($"Set Lookup Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var input = driver.ClickWhenAvailable(By.Id(field));

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {field} is not lookup");

                    var lookupIcon = input.FindElement(By.ClassName("Lookup_RenderButton_td"));
                    lookupIcon.Hover(driver, true);
                    lookupIcon.Click(true);

                    var dialogName = $"Dialog_{field}_IMenu";
                    var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

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
        [Obsolete("This method has been deprecated. Please use the new SelectLookup(LookupItem field, bool clearFieldValue = true, bool openLookupPage = true)")]
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
                    var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

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
        /// DEPRECATED: Set Lookup Value for the field
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="clearFieldValue">Remove Existing Field Value, if present. False = Click the existing value</param>
        /// <param name="openLookupPage">The Open Lookup Page</param>
        [Obsolete("SelectLookup is deprecated, please use SelectLookup(LookupItem field, bool clearFieldValue = true, bool openLookupPage = true) instead.")]
        public BrowserCommandResult<bool> SelectLookup(string field, bool clearFieldValue = true, bool openLookupPage = true)
        {
            return this.Execute(GetOptions($"Select Lookup for: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    var fieldContainer = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.FieldContainer].Replace("[NAME]", field)));

                    if (fieldContainer.Text != "" && clearFieldValue)
                    {
                        fieldContainer.SendKeys(Keys.Clear);
                    }
                    else if (fieldContainer.Text != "" && !clearFieldValue)
                    {
                        fieldContainer.Click();
                        return true;
                    }

                    var input = driver.ClickWhenAvailable(By.Id(field));

                    if (input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {field} is not lookup");

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])).Click();

                    Browser.ThinkTime(1000);
                    var dialogName = $"Dialog_{field}_IMenu";
                    var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

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
        /// Set Lookup Value for the field
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="openLookupPage">The Open Lookup Page</param>
        /// <param name="clearFieldValue">Remove Existing Field Value, if present. False = Click the existing value</param>
        public BrowserCommandResult<bool> SelectLookup(LookupItem field, bool clearFieldValue = true, bool openLookupPage = true)
        {
            return this.Execute(GetOptions($"Select Lookup for: {field.Name}"), driver =>
            {
                if (driver.HasElement(By.Id(field.Name)))
                {
                    var fieldContainer = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.FieldContainer].Replace("[NAME]", field.Name)));

                    if (fieldContainer.Text != "" && clearFieldValue)
                    {
                        if (Browser.Options.BrowserType != BrowserType.Firefox)
                        {
                            fieldContainer.Hover(driver, true);
                            fieldContainer.Click(true);

                            var lookupSearchIcon = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", field.Name)));
                            lookupSearchIcon.Hover(driver, true);

                            var lookupSearchIconImage = driver.FindElement(By.TagName("img"));
                            lookupSearchIconImage.Hover(driver, true);

                            Actions clickAction = new Actions(driver).SendKeys(Keys.Enter);
                            clickAction.Build().Perform();
                        }
                        else if (Browser.Options.BrowserType == BrowserType.Firefox)
                        {
                            fieldContainer.Hover(driver, true);
                            var lookupIcon = fieldContainer.FindElement(By.Id("[NAME]_lookupSearchIconDiv".Replace("[NAME]", field.Name)));
                            lookupIcon.Hover(driver, true);
                            lookupIcon.Click(true);
                        }

                        var lookupMenuName = $"Dialog_{field.Name}_IMenu";
                        var lookupMenu = driver.WaitUntilAvailable(By.Id(lookupMenuName));

                        var lookupMenuItems = OpenDialog(lookupMenu).Value;

                        if (lookupMenuItems.Any())
                        {
                            var lookupMenuItem = lookupMenuItems.Last();
                            lookupMenuItem.Element.Click();
                        }

                        driver.WaitUntilAvailable(By.Id("InlineDialog"), new TimeSpan(0, 0, 5));
                        SwitchToDialog();
                        driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.LookUp.Remove]));
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.LookUp.Remove]));

                        SwitchToContent();

                        driver.WaitForPageToLoad();
                        driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Entity.Form]),
                                                    TimeSpan.FromSeconds(30),
                                                    "CRM Record is Unavailable or not finished loading. Timeout Exceeded"
                                                    );
                    }
                    else if (fieldContainer.Text != "" && !clearFieldValue)
                    {
                        fieldContainer.Click();
                        return true;
                    }

                    if (!openLookupPage)
                        return true;

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
        /// DEPRECATED: Sets the value of a Checkbox field.
        /// Please use the new TwoOption method ==> SetValue(TwoOption option)
        /// </summary>
        /// <param name="field">Field name or ID.</param>
        /// <param name="check">If set to <c>true</c> [check].</param>
        /// <example>xrmBrowser.Entity.SetValue("creditonhold",true);</example>
        [Obsolete("SetValue(string field, bool check) is deprecated, please use SetValue(TwoOption option) instead.")]
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
        /// Sets the value of a TwoOption field on an Entity form.
        /// </summary>
        /// <param name="option">Field name or ID.</param>
        /// <example>xrmBrowser.Entity.SetValue(new TwoOption{ Name = "creditonhold"});</example>
        public BrowserCommandResult<bool> SetValue(TwoOption option)
        {
            return this.Execute(GetOptions($"Set TwoOption Value: {option.Name}"), driver =>
            {
                var isBoolean = bool.TryParse(option.Value, out var optionValue);
                if (!isBoolean)
                    throw new ArgumentException($"Value {option.Value}: Cannot be converted to a boolean value");

                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));

                    var hasRadio = false;
                    var hasList = false;
                    var hasCheckbox = false;
                    string selectedValue = null;
                    ReadOnlyCollection<IWebElement> options = null;

                    if (fieldElement.HasAttribute("data-picklisttype"))
                    {
                        if (fieldElement.GetAttribute("data-picklisttype") == "0")
                            hasRadio = true;
                        else
                            hasList = true;

                        var radioTd = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.TwoOptionFieldTd].Replace("[NAME]", option.Name)));
                        selectedValue = radioTd.GetAttribute("title");
                        var radioList = fieldElement.FindElement(By.XPath(Elements.Xpath[Reference.Entity.TwoOptionFieldList].Replace("[NAME]", option.Name)));
                        options = radioList.FindElements(By.TagName("option"));
                    }
                    else
                    {
                        hasCheckbox = fieldElement.HasElement(By.XPath(Elements.Xpath[Reference.Entity.TwoOptionFieldCheckbox].Replace("[NAME]", option.Name)));
                    }

                    if (hasRadio)
                    {
                        if (optionValue && selectedValue == options.FirstOrDefault(a => a.GetAttribute("value") == "0")?.GetAttribute("title") ||
                           !optionValue && selectedValue == options.FirstOrDefault(a => a.GetAttribute("value") == "1")?.GetAttribute("title"))
                        {
                            driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));
                        }
                    }
                    else if (hasCheckbox)
                    {
                        var checkbox = fieldElement.FindElement(By.XPath(Elements.Xpath[Reference.Entity.TwoOptionFieldCheckbox].Replace("[NAME]", option.Name)));

                        if (optionValue && !checkbox.Selected || !optionValue && checkbox.Selected)
                        {
                            driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Entity.TwoOptionFieldCheckbox].Replace("[NAME]", option.Name)));
                        }
                    }
                    else if (hasList)
                    {
                        var num = string.Empty;
                        if (optionValue && selectedValue == options.FirstOrDefault(a => a.GetAttribute("value") == "0")?.GetAttribute("title"))
                        {
                            num = "1";
                        }
                        else if (!optionValue && selectedValue == options.FirstOrDefault(a => a.GetAttribute("value") == "1")?.GetAttribute("title"))
                        {
                            num = "0";
                        }

                        if (!string.IsNullOrEmpty(num))
                        {
                            fieldElement.Hover(driver);
                            driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));
                            driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Entity.TwoOptionFieldListOption].Replace("[NAME]", option.Name).Replace("[VALUE]", num)));
                        }
                    }
                    else
                        throw new InvalidOperationException($"Field: {option.Name} Is not a TwoOption field");
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// DEPRECATED: Sets the value of a Date Field.
        /// Please use the new DateTimeControl method ==> SetValue(DateTimeControl date)
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        [Obsolete("SetValue(string field, DateTime date) is deprecated, please use SetValue(DateTimeControl date) instead.")]
        public BrowserCommandResult<bool> SetValue(string field, DateTime date)
        {
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
        /// Sets the value of a Date Field on an Entity form.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        /// <example> xrmBrowser.Entity.SetValue(new DateTimeControl { Name = "birthdate", Value =  DateTime.Parse("11/1/1980")});</example>
        public BrowserCommandResult<bool> SetValue(DateTimeControl date)
        {
            return this.Execute(GetOptions($"Set DateTime Value: {date.Name}"), driver =>
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
                    }
                    else
                    {
                        input.SendKeys(date.Value.ToShortDateString());
                        input.SendKeys(Keys.Enter);
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {date.Name} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Text/Description field on an Entity form.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <param name="value">The value.</param>
        /// <example>xrmBrowser.Entity.SetValue("name", "Test API Account");</example>
        public BrowserCommandResult<bool> SetValue(string field, string value)
        {
            var returnval = this.Execute(GetOptions($"Set Text Field Value: {field}"), driver =>
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
                    else if (fieldElement.TagName == "textarea")
                    {
                        fieldElement.Clear();
                        fieldElement.SendKeys(value);
                        fieldElement.SendKeys(Keys.Enter);
                    }
                    else
                    {
                        //BugFix - Setvalue -The value is getting erased even after setting the value ,might be due to recent CSS changes.
                        //driver.ExecuteScript("Xrm.Page.getAttribute('" + field + "').setValue('')");
                        fieldElement.FindElement(By.TagName("input")).SendKeys(Keys.Control + "a");
                        fieldElement.FindElement(By.TagName("input")).SendKeys(Keys.Backspace);
                        fieldElement.FindElement(By.TagName("input")).SendKeys(value, true);
                        fieldElement.SendKeys(Keys.Enter);
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
            return returnval;
        }

        /// <summary>
        /// Sets the value of a picklist on an Entity form.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public BrowserCommandResult<bool> SetValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Set OptionSet Value: {option.Name}"), driver =>
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
        /// Sets the value of a multi-value picklist on an Entity form.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.SetValue(new MultiValueOptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public BrowserCommandResult<bool> SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Set MultiValueOptionSet Value: {option.Name}"), driver =>
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
        /// Sets the value of a Composite control on an Entity form.
        /// </summary>
        /// <param name="control">The Composite control values you want to set.</param>
        /// <param name="checkForDialog">Check for a dialog, e.g. Found Places </param>
        /// <example>xrmBrowser.Entity.SetValue(new CompositeControl {Id = "fullname", Fields = fields});</example>
        public BrowserCommandResult<bool> SetValue(CompositeControl control, bool checkForDialog = false, [Range(1, 10)]int index = 1)
        {
            return this.Execute(GetOptions($"Set ConpositeControl Value: {control.Id}"), driver =>
            {
                driver.WaitUntilVisible(By.Id(control.Id));

                if (!driver.HasElement(By.Id(control.Id)))
                    return false;

                driver.ClickWhenAvailable(By.Id(control.Id));

                if (driver.HasElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut])))
                {
                    var compcntrl =
                        driver.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut]));

                    // Initialize i, to correspond with current field position in control.Fields
                    int i = 0;

                    foreach (var field in control.Fields)
                    {
                        compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id)).Click(true);

                        var result = compcntrl.FindElements(By.TagName("input"))
                            .ToList()
                            .FirstOrDefault(x => x.GetAttribute("id").Contains(field.Id));

                        if (checkForDialog)
                        {
                            if (IsDialogFrameVisible())
                            {
                                if (control.Fields.Count == (i + 1))
                                {
                                    SwitchToDialog();

                                    var addressSuggestor = driver.FindElement(By.Id("ctrAddressSuggestor"));
                                    var suggestedAddresses = addressSuggestor.FindElements(By.TagName("li"));

                                    var targetAddress = suggestedAddresses[(index - 1)].FindElement(By.TagName("a"));

                                    targetAddress.Click(true);

                                    SwitchToContentFrame();
                                    compcntrl = driver.WaitUntilAvailable(By.Id(control.Id + Elements.ElementId[Reference.SetValue.FlyOut]));
                                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click(true);
                                    return true;
                                }
                                else
                                {
                                    SwitchToDialog();

                                    var closeFoundPlaces = driver.FindElement(By.XPath(Elements.Xpath[Reference.Dialogs.CloseFoundPlacesDialog]));
                                    closeFoundPlaces.Click(true);
                                    SwitchToContentFrame();
                                }

                            }
                            else
                                SwitchToContentFrame();
                        }

                        driver.WaitUntilAvailable(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id));
                        compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.CompositionLinkControl] + field.Id)).Click(true);
                        //BugFix - Setvalue -The value is getting erased even after setting the value ,might be due to recent CSS changes.
                        driver.ExecuteScript("document.getElementById('" + result?.GetAttribute("id") + "').value = ''");
                        result?.SendKeys(field.Value);

                        i++;
                    }

                    compcntrl.FindElement(By.Id(control.Id + Elements.ElementId[Reference.SetValue.Confirm])).Click(true);

                    // Repeat this check in the event the Found Places dialog appears after closing the Composite Control for Address
                    if (checkForDialog)
                    {
                        if (IsDialogFrameVisible())
                        {
                            if (control.Fields.Count == (i + 1))
                            {
                                SwitchToDialog();

                                var addressSuggestor = driver.FindElement(By.Id("ctrAddressSuggestor"));
                                var suggestedAddresses = addressSuggestor.FindElements(By.TagName("li"));

                                var targetAddress = suggestedAddresses[(index - 1)].FindElement(By.TagName("a"));

                                targetAddress.Click(true);

                                SwitchToContentFrame();

                            }
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Composite Control: {control.Id} Does not exist");

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Lookup on an Entity form.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmBrowser.Entity.SetValue(new Lookup { Name = "primarycontactid", Value = "Rene Valdes (sample)" });</example>
        public BrowserCommandResult<bool> SetValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Set Lookup Value: {control.Name}"), driver =>
            {
                if (driver.HasElement(By.Id(control.Name)))
                {
                    var fieldContainer = driver.WaitUntilAvailable(By.Id(control.Name));

                    if (fieldContainer.Text != "" && control.Value != null)
                        SelectLookup(control, true, false);

                    driver.WaitUntilVisible(By.Id(control.Name));

                    fieldContainer = driver.ClickWhenAvailable(By.Id(control.Name));

                    if (fieldContainer.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.LookupRenderClass])) == null)
                        throw new InvalidOperationException($"Field: {control.Name} is not lookup");

                    var lookupSearchIcon = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", control.Name)));

                    if (Browser.Options.BrowserType != BrowserType.Firefox)
                    {
                        var lookupIcon = fieldContainer.FindElement(By.ClassName("Lookup_RenderButton_td"));
                        lookupSearchIcon = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", control.Name)));
                        lookupIcon.Hover(driver);
                        lookupIcon.Click(true);

                        /*
                        lookupSearchIcon.Hover(driver, true);
                        lookupSearchIcon.Click(true);

                        var lookupSearchIconImage = driver.FindElement(By.TagName("img"));
                        lookupSearchIconImage.Hover(driver, true);

                        Actions clickAction = new Actions(driver).SendKeys(Keys.Enter);
                        clickAction.Build().Perform();
                        */
                    }
                    else
                    {
                        var lookupIcon = fieldContainer.FindElement(By.ClassName("Lookup_RenderButton_td"));
                        lookupSearchIcon = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.GetLookupSearchIcon].Replace("[NAME]", control.Name)));
                        lookupIcon.Hover(driver);
                        lookupIcon.Click(true);
                    }

                    var dialogName = $"Dialog_{control.Name}_IMenu";
                    var dialog = driver.WaitUntilAvailable(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if (dialogItems.Any())
                    {
                        var lookupMenuItem = dialogItems.Last();
                        lookupMenuItem.Element.Click();
                    }

                    if (control.Value != null)
                    {
                        SwitchToDialog();

                        driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Grid.FindCriteria]));
                        driver.FindElement(By.XPath(Elements.Xpath[Reference.Grid.FindCriteria])).Clear();
                        driver.FindElement(By.XPath(Elements.Xpath[Reference.Grid.FindCriteria])).SendKeys(control.Value);
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Grid.FindCriteriaImg]));

                        var itemsTable = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Grid.GridBodyTable]));

                        if (itemsTable.GetAttribute("totalrecordcount") == "0")
                        {
                            throw new InvalidOperationException($"No records are available in this view for the Search'{control.Value}'");
                        }
                        var tbody = itemsTable.FindElement(By.TagName("tbody"));
                        var items = tbody.FindElements(By.TagName("tr"));

                        foreach (var item in items)
                        {
                            var primary = item.FindElements(By.TagName("td"))[1];
                            if (primary.Text.Contains(control.Value))
                            {
                                var checkbox = item.FindElements(By.TagName("td"))[0];

                                if (item.GetAttribute("selected") != "true")
                                    checkbox.Click();
                                break;
                            }
                        }

                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.LookUp.Begin]));

                        SwitchToContent();

                    }
                    else
                        throw new InvalidOperationException($"Field: {control.Name} Does not exist");
                }
                return true;
            });
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
        /// Switches to content frame in the CRM application.
        /// </summary>
        public bool SwitchToContentFrame()
        {
            return this.Execute("Switch to content frame", driver => SwitchToContent());
        }

        internal bool SwitchToDefault()
        {
            Browser.Driver.SwitchTo().DefaultContent();

            return true;
        }

        /// <summary>
        /// SwitchToDefaultContent
        /// </summary>
        public bool SwitchToDefaultContent()
        {
            return this.Execute("Switch to Default Content", driver => SwitchToDefault());
        }

        /// <summary>
        /// Switches to dialog frame in the CRM application.
        /// </summary>   
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

        public bool SwitchToDialogFrame()
        {
            return this.Execute("Switch to dialog frame", driver => SwitchToDialog(0));
        }

        internal bool SwitchToPopup()
        {
            Browser.Driver.LastWindow().SwitchTo().ActiveElement();

            return true;
        }

        /// <summary>
        /// Switches to Wizard frame in the CRM application.
        /// </summary>
        public bool SwitchToPopupWindow()
        {

            return this.Execute("Switch to Pop Up Window", driver => SwitchToPopup());

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
        /// Switches to Quick Find frame in the CRM application.
        /// </summary>
        public bool SwitchToQuickCreateFrame()
        {
            return this.Execute("Switch to Quick Create Frame", driver => SwitchToQuickCreate());
        }

        internal bool SwitchToRelated()
        {
            SwitchToContent();

            Browser.Driver.WaitUntilAvailable(By.Id(Browser.ActiveFrameId));

            Browser.Driver.SwitchTo().Frame(Browser.ActiveFrameId + "Frame");

            return true;
        }

        /// <summary>
        /// Switches to related frame in the CRM application.
        /// </summary>
        public bool SwitchToRelatedFrame()
        {

            return this.Execute("Switch to Related Frame", driver => SwitchToRelated());

        }

        internal bool SwitchToView()
        {
            Browser.Driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.ViewFrameId]);

            return true;
        }

        public bool SwitchToViewFrame()
        {
            return this.Execute("Switch to View frame", driver => SwitchToView());
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
        public bool SwitchToWizardFrame()
        {

            return this.Execute("Switch to Wizard Frame", driver => SwitchToWizard());

        }

    }
}
