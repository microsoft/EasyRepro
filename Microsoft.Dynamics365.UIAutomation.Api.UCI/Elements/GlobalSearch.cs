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
        public class GlobalSearchReference
        {
            public const string GlobalSearch = "GlobalSearch";
            #region private
            private string _CategorizedSearchButton = "//button[contains(@data-id,'search-submit-button')]";
            private string _RelevanceSearchButton = "//div[@aria-label=\"Search box\"]//button";
            private string _Text = "//input[@aria-label=\"Search box\"]";
            private string _Filter = "//select[@aria-label=\"Filter with\"]";
            private string _Results = "id(\"entityDiv_1\")";
            private string _Container = "//div[@id=\"searchResultList\"]";
            private string _EntityContainer = "//div[@id=\"View[NAME]\"]";
            private string _Records = "//li[@role=\"row\"]";
            private string _GroupContainer = "//label[contains(text(), '[NAME]')]/parent::div";
            private string _FilterValue = "//label[contains(text(), '[NAME]')]";
            private string _CategorizedResultsContainer = "//div[@id=\"searchResultList\"]";
            private string _CategorizedResults = "//ul[@aria-label='[ENTITY]']/li";
            private string _RelevanceSearchResultsSelectedTab = "//button[@aria-selected='true' and @role='tab']";
            private string _RelevanceSearchResultsTab = "//section[@id='searchComponent']//button[@name='[NAME]' and @role='tab']";
            private string _RelevanceSearchResultLinks = "//div[@role='rowgroup']/div[@role='row']/div[1]";
            #endregion
            #region prop
            public string CategorizedSearchButton { get => _CategorizedSearchButton; set { _CategorizedSearchButton = value; } }
            public string RelevanceSearchButton { get => _RelevanceSearchButton; set { _RelevanceSearchButton = value; } }
            public string Text { get => _Text; set { _Text = value; } }
            public string Filter { get => _Filter; set { _Filter = value; } }
            public string Results { get => _Results; set { _Results = value; } }
            public string Container { get => _Container; set { _Container = value; } }
            public string EntityContainer { get => _EntityContainer; set { _EntityContainer = value; } }
            public string Records { get => _Records; set { _Records = value; } }
            public string GroupContainer { get => _GroupContainer; set { _GroupContainer = value; } }
            public string FilterValue { get => _FilterValue; set { _FilterValue = value; } }
            public string CategorizedResultsContainer { get => _CategorizedResultsContainer; set { _CategorizedResultsContainer = value; } }
            public string CategorizedResults { get => _CategorizedResults; set { _CategorizedResults = value; } }
            public string RelevanceSearchResultsSelectedTab { get => _RelevanceSearchResultsSelectedTab; set { _RelevanceSearchResultsSelectedTab = value; } }
            public string RelevanceSearchResultsTab { get => _RelevanceSearchResultsTab; set { _RelevanceSearchResultsTab = value; } }
            public string RelevanceSearchResultLinks { get => _RelevanceSearchResultLinks; set { _RelevanceSearchResultLinks = value; } }
            #endregion
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
                        By.XPath(_client.ElementMapper.NavigationReference.SearchButton),
                        2.Seconds(),
                        "The Global Search (Categorized Search) button is not available.");

                    input = driver.WaitUntilVisible(
                        By.XPath(_client.ElementMapper.GlobalSearchReference.Text),
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

                if (driver.TryFindElement(By.XPath(_client.ElementMapper.GlobalSearchReference.Filter), out var filter))
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
                else if (driver.TryFindElement(By.XPath(_client.ElementMapper.GlobalSearchReference.RelevanceSearchResultsTab.Replace("[NAME]", entity)), out var entityTab))
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

                if (driver.TryFindElement(By.XPath(_client.ElementMapper.GlobalSearchReference.GroupContainer.Replace("[NAME]", filterBy)), out var entityPicker))
                {
                    // Categorized Search
                    entityPicker.ClickWhenAvailable(
                    By.XPath(_client.ElementMapper.GlobalSearchReference.FilterValue.Replace("[NAME]", value)),
                    $"Filter By Value '{value}' does not exist in the Filter options.");
                }
                else if (filterBy == "Record Type" && driver.TryFindElement(By.XPath(_client.ElementMapper.GlobalSearchReference.RelevanceSearchResultsTab.Replace("[NAME]", value)), out var entityTab))
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

                if (driver.TryFindElement(By.XPath(_client.ElementMapper.GlobalSearchReference.CategorizedResultsContainer), out var categorizedContainer))
                {
                    // Categorized Search
                    var records = categorizedContainer.FindElements(By.XPath(_client.ElementMapper.GlobalSearchReference.CategorizedResults.Replace("[ENTITY]", entity)));
                    if (index >= records.Count)
                    {
                        throw new InvalidOperationException($"There was less than {index + 1} records in the search result.");
                    }

                    records[index].Click(true);
                }
                else if (driver.TryFindElement(By.Id("resultsContainer-view"), out var relevanceContainer))
                {
                    // Relevance Search
                    var selectedTab = relevanceContainer.FindElement(By.XPath(_client.ElementMapper.GlobalSearchReference.RelevanceSearchResultsSelectedTab));
                    if (selectedTab.GetAttribute("name") != entity)
                    {
                        this.FilterWith(entity);
                    }

                    var links = relevanceContainer.FindElements(By.XPath(_client.ElementMapper.GlobalSearchReference.RelevanceSearchResultLinks));
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
