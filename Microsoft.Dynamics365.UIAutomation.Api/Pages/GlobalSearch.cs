// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    ///  Xrm global search page.
    ///  </summary>
    public class GlobalSearch
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalSearch"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public GlobalSearch(InteractiveBrowser browser)
            : base(browser)
        {
            this.SwitchToContent();
        }

        /// <summary>
        /// Filter by entity in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to filter with.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.GlobalSearch.FilterWith("Account");</example>
        public BrowserCommandResult<bool> FilterWith(string entity, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Filter With: {entity}"), driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.GlobalSearch.Filter])))
                    throw new InvalidOperationException("Filter With picklist is not available");

                var picklist = driver.FindElement(By.XPath(Elements.Xpath[Reference.GlobalSearch.Filter]));
                var options = driver.FindElements(By.TagName("option"));

                picklist.Click();

                IWebElement select = options.FirstOrDefault(x => x.Text == entity);

                if (select == null)
                    throw new InvalidOperationException($"Entity '{entity}' does not exist in the Filter options.");

                select.Click();

                return true;
            });
        }

        /// <summary>
        /// Opens the specified record in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to open a record.</param>
        /// <param name="index">The index of the record you want to open.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param> time.</param>
        /// <example>xrmBrowser.GlobalSearch.OpenRecord("Accounts",0);</example>
        public BrowserCommandResult<bool> OpenRecord(string entity, int index, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Global Search Record for Entity: {entity} and record position {index}"), driver =>
            {
                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.GlobalSearch.SearchResults]));

                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.GlobalSearch.SearchResults])))
                    throw new InvalidOperationException("Search Results is not available");

                var results = driver.FindElement(By.XPath(Elements.Xpath[Reference.GlobalSearch.SearchResults]));
                var resultsContainer = results.FindElement(By.XPath(Elements.Xpath[Reference.GlobalSearch.Container]));
                var entityContainers = resultsContainer.FindElements(By.Id(Elements.ElementId[Reference.GlobalSearch.EntityContainersId]));
                var entityContainer = entityContainers.FirstOrDefault(x => x.FindElement(By.Id(Elements.ElementId[Reference.GlobalSearch.EntityNameId])).Text.Trim().ToLower() == entity.ToLower());

                if (entityContainer == null)
                    throw new InvalidOperationException($"Entity {entity} was not found in the results");

                var records = entityContainer?.FindElements(By.Id(Elements.ElementId[Reference.GlobalSearch.RecordNameId]));

                if (records == null)
                    throw new InvalidOperationException($"No records found for entity {entity}");

                if (records.Count < index)
                    throw new InvalidOperationException($"Search Results List does not have {index + 1} items.");

                records[index].Click(true);

                SwitchToContent();
                driver.WaitForPageToLoad();
                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Entity.Form]),
                    TimeSpan.FromSeconds(60),
                    "CRM Record is Unavailable or not finished loading. Timeout Exceeded"
                );

                return true;
            });
        }

        /// <summary>
        /// Searches for the specified criteria in Global Search.
        /// </summary>
        /// <param name="criteria">Search criteria.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param> time.</param>
        /// <example>xrmBrowser.GlobalSearch.Search("Contoso");</example>
        public BrowserCommandResult<bool> Search(string criteria, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Global Search"), driver =>
            {
                var searchText = driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.GlobalSearch.SearchText]), 
                                                                TimeSpan.FromSeconds(10), 
                                                                "Search Text Box is not available");
                searchText.SendKeys(criteria, true);

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.GlobalSearch.SearchButton]));
                return true;
            });
        }
    }
}
