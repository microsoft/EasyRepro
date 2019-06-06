// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Xrm Lookup Page
    /// </summary>
    public class Lookup
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Lookup"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Lookup(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToDialog();
        }

        /// <summary>
        /// Opens the Entity Picker
        /// </summary>
        private BrowserCommandResult<Dictionary<string, IWebElement>> OpenEntityPicker()
        {
            return this.Execute(GetOptions("Open Entity Picker"), driver =>
            {
                var dictionary = new Dictionary<string, IWebElement>();

                var entitySelectorContainer = driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.LookUp.SelObjects]));

                Thread.Sleep(500);

                var entityItems = entitySelectorContainer.FindElements(By.TagName("option"));

                foreach (var entityItem in entityItems)
                {
                    var title = entityItem.GetAttribute("title");
                    dictionary.Add(title, entityItem);
                }

                return dictionary;
            });
        }

        /// <summary>
        /// Opens the View Picker
        /// </summary>
        private BrowserCommandResult<Dictionary<string, IWebElement>> OpenViewPicker()
        {
            return this.Execute(GetOptions("Open View Picker"), driver =>
            {
                var dictionary = new Dictionary<string, IWebElement>();

                var viewSelectorContainer = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.LookUp.SavedQuerySelector]));
                viewSelectorContainer.Click();

                Thread.Sleep(500);

                var viewItems = viewSelectorContainer.FindElements(By.TagName("option"));

                foreach (var viewItem in viewItems)
                {
                    var title = viewItem.GetAttribute("title");
                    dictionary.Add(title, viewItem);
                }

                return dictionary;
            });
        }

        /// <summary>
        /// Switch Entity
        /// </summary>
        /// <param name="entityName">The Entity Name</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> SwitchEntity(string entityName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Switch Entity"), driver =>
            {
                var entities = OpenEntityPicker().Value;

                if (!entities.ContainsKey(entityName))
                    return false;

                var entityItem = entities[entityName];
                entityItem.Click();

                return true;
            });
        }

        /// <summary>
        /// Switches the view
        /// </summary>
        /// <param name="viewName">The ViewName</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> SwitchView(string viewName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Switch View"), driver =>
            {
                var views = OpenViewPicker().Value;

                if (!views.ContainsKey(viewName))
                    return false;

                views[viewName].Click();

                return true;
            });
        }

        /// <summary>
        /// Searches based on searchCriteria in Lookup
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> Search(string searchCriteria, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Search"), driver =>
            {
                driver.FindElement(By.XPath(Elements.Xpath[Reference.Grid.FindCriteria])).Clear();
                driver.FindElement(By.XPath(Elements.Xpath[Reference.Grid.FindCriteria])).SendKeys(searchCriteria);
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Grid.FindCriteriaImg]));

                return true;
            });
        }

        /// <summary>
        /// Gets the Grid Items
        /// </summary>
        public BrowserCommandResult<List<GridItem>> GetGridItems()
        {
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
        /// Selects the Item
        /// </summary>
        /// <param name="index">The Index</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Lookup.SelectItem(0);</example>
        public BrowserCommandResult<bool> SelectItem(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Select Item"), driver =>
            {
                var itemsTable = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Grid.GridBodyTable]));
                var items = itemsTable.FindElements(By.TagName("tr"));

                var item = items[index + 1];
                var checkbox = item.FindElements(By.TagName("td"))[0];

                checkbox.Click();
             
                return true;
            });
        }

        /// <summary>
        /// Selects Item based on the value given
        /// </summary>
        /// <param name="value">Used to match where text contains the provided value.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Lookup.SelectItem("Alex Wu");</example>
        public BrowserCommandResult<bool> SelectItem(string value, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Select Item"), driver =>
            {
                var itemsTable = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Grid.GridBodyTable]));

                if(itemsTable.GetAttribute("totalrecordcount") == "0")
                {
                    throw new InvalidOperationException($"No Process records are available in this view for the Search'{value}'");
                }
                var tbody = itemsTable.FindElement(By.TagName("tbody"));
                var items = tbody.FindElements(By.TagName("tr"));

                foreach (var item in items)
                {
                    var primary = item.FindElements(By.TagName("td"))[1];
                    if (primary.Text.Contains(value))
                    {
                        var checkbox = item.FindElements(By.TagName("td"))[0];

                        if(item.GetAttribute("selected") != "true")
                            checkbox.Click();
                        break;
                    }
                }

                return true;
            });
        }
        /// <summary>
        /// Add Lookup
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Lookup.Add();</example>
        public BrowserCommandResult<bool> Add(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Add"), driver =>
            {               
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.LookUp.Begin]));

                return true;
            });
        }

        /// <summary>
        /// Select from subgrid
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Lookup.Select();</example>
        public BrowserCommandResult<bool> Select(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Select"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.LookUp.Add]));
                
                return true;
            });
        }

        public BrowserCommandResult<bool> Remove(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Remove"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.LookUp.Remove]));
                
                return true;
            });
        }

        public BrowserCommandResult<bool> New(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("New"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.LookUp.New]));

                return true;
            });
        }

        public BrowserCommandResult<bool> Cancel(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Cancel"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.LookUp.DialogCancel]));

                return true;
            });
        }
    }
}