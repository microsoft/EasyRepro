// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class GlobalSearch : Element
    {
        #region DTO
        public static class GlobalSearchReference
        {
            public static string CategorizedSearchButton = "//button[contains(@data-id,'search-submit-button')]";
            public static string RelevanceSearchButton = "//div[@aria-label=\"Search box\"]//button";
            public static string Text = "//input[@aria-label=\"Search box\"]";
            public static string Filter = "//select[@aria-label=\"Filter with\"]";
            public static string Results = "id(\"entityDiv_1\")";
            public static string Container = "//div[@id=\"searchResultList\"]";
            public static string EntityContainer = "//div[@id=\"View[NAME]\"]";
            public static string Records = "//li[@role=\"row\"]";
            public static string GroupContainer = "//label[contains(text(), '[NAME]')]/parent::div";
            public static string FilterValue = "//label[contains(text(), '[NAME]')]";
            public static string CategorizedResultsContainer = "//div[@id=\"searchResultList\"]";
            public static string CategorizedResults = "//ul[@aria-label='[ENTITY]']/li";
            public static string RelevanceSearchResultsSelectedTab = "//button[@aria-selected='true' and @role='tab']";
            public static string RelevanceSearchResultsTab = "//section[@id='searchComponent']//button[@name='[NAME]' and @role='tab']";
            public static string RelevanceSearchResultLinks = "//div[@role='rowgroup']/div[@role='row']/div[1]";
        }
        #endregion
        private readonly WebClient _client;

        public GlobalSearch(WebClient client) : base()
        {
            _client = client;
        }
        #region public

        /// <summary>
        /// Changes the Global Search Type
        /// </summary>
        /// <param name="type">The type of search that you want to do </param>
        /// <param name="index">The index of the record you want to open.</param>
        /// <example>xrmBrowser.GlobalSearch.ChangeSearchType("Categorized Search");</example>
        public bool ChangeSearchType(string type)
        {
            return true;
            //return _client.ChangeSearchType(type);
        }
        #endregion

        #region GlobalSearch

        /// <summary>
        /// Search using Relevance Search or Categorized Search.
        /// </summary>
        /// <param name="criteria">Criteria to search for.</param>
        /// <returns></returns>
        public BrowserCommandResult<bool> Search(string criteria, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Global Search: {criteria}"), driver =>
            {
                driver.WaitForTransaction();

                IWebElement input;
                if (driver.TryFindElement(By.Id("GlobalSearchBox"), out var globalSearch))
                {
                    input = globalSearch;
                }
                else
                {
                    driver.ClickWhenAvailable(
                        By.XPath(Navigation.NavigationReference.SearchButton),
                        2.Seconds(),
                        "The Global Search (Categorized Search) button is not available.");

                    input = driver.WaitUntilVisible(
                        By.XPath(GlobalSearchReference.Text),
                        2.Seconds(),
                        "The Categorized Search text field is not available.");
                }

                input.Click();
                input.SendKeys(criteria, true);
                input.SendKeys(Keys.Enter);

                driver.WaitForTransaction();

                return true;
            });
        }

        /// <summary>
        /// Filter by entity in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to filter with.</param>
        /// <example>xrmBrowser.GlobalSearch.FilterWith("Account");</example>
        public BrowserCommandResult<bool> FilterWith(string entity, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Filter With: {entity}"), driver =>
            {
                driver.WaitForTransaction();

                if (driver.TryFindElement(By.XPath(GlobalSearchReference.Filter), out var filter))
                {
                    // Categorized Search
                    var option = filter
                        .FindElements(By.TagName("option"))
                        .FirstOrDefault(x => x.Text == entity);

                    if (option == null)
                    {
                        throw new InvalidOperationException($"Entity '{entity}' does not exist in the Filter options.");
                    }

                    filter.Click();
                    option.Click();
                }
                else if (driver.TryFindElement(By.XPath(GlobalSearchReference.RelevanceSearchResultsTab.Replace("[NAME]", entity)), out var entityTab))
                {
                    // Relevance Search
                    entityTab.Click();
                }
                else
                {
                    throw new InvalidOperationException($"Unable to filter global search results by the '{entity}' table.");
                }

                return true;
            });
        }

        /// <summary>
        /// Filter by value in the Global Search Results.
        /// </summary>
        /// <param name="filterBy">The Group you want to filter on.</param>
        /// <param name="value">The value you want to filter on.</param>
        /// <example>xrmBrowser.GlobalSearch.Filter("Record Type", "Accounts");</example>
        public BrowserCommandResult<bool> Filter(string filterBy, string value, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Filter With: {value}"), driver =>
            {
                driver.WaitForTransaction();

                if (driver.TryFindElement(By.XPath(GlobalSearchReference.GroupContainer.Replace("[NAME]", filterBy)), out var entityPicker))
                {
                    // Categorized Search
                    entityPicker.ClickWhenAvailable(
                    By.XPath(GlobalSearchReference.FilterValue.Replace("[NAME]", value)),
                    $"Filter By Value '{value}' does not exist in the Filter options.");
                }
                else if (filterBy == "Record Type" && driver.TryFindElement(By.XPath(GlobalSearchReference.RelevanceSearchResultsTab.Replace("[NAME]", value)), out var entityTab))
                {
                    // Relevance Search
                    entityTab.Click();
                }
                else
                {
                    throw new InvalidOperationException("Unable to filter global search results.");
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        /// <summary>
        /// Opens the specified record in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to open a record.</param>
        /// <param name="index">The index of the record you want to open.</param>
        /// <example>xrmBrowser.GlobalSearch.OpenRecord("Accounts",0);</example>
        public BrowserCommandResult<bool> OpenRecord(string entity, int index, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Open Global Search Record"), driver =>
            {
                driver.WaitForTransaction();

                if (driver.TryFindElement(By.XPath(GlobalSearchReference.CategorizedResultsContainer), out var categorizedContainer))
                {
                    // Categorized Search
                    var records = categorizedContainer.FindElements(By.XPath(GlobalSearchReference.CategorizedResults.Replace("[ENTITY]", entity)));
                    if (index >= records.Count)
                    {
                        throw new InvalidOperationException($"There was less than {index + 1} records in the search result.");
                    }

                    records[index].Click(true);
                }
                else if (driver.TryFindElement(By.Id("resultsContainer-view"), out var relevanceContainer))
                {
                    // Relevance Search
                    var selectedTab = relevanceContainer.FindElement(By.XPath(GlobalSearchReference.RelevanceSearchResultsSelectedTab));
                    if (selectedTab.GetAttribute("name") != entity)
                    {
                        this.FilterWith(entity);
                    }

                    var links = relevanceContainer.FindElements(By.XPath(GlobalSearchReference.RelevanceSearchResultLinks));
                    if (index >= links.Count)
                    {
                        throw new InvalidOperationException($"There was less than {index + 1} records in the search result.");
                    }

                    new Actions(driver).DoubleClick(links[index]).Perform();
                }
                else
                {
                    throw new NotFoundException("Unable to locate global search results.");
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        #endregion

    }
}
