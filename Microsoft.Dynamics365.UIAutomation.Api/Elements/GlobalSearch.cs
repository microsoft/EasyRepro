// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class GlobalSearch
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
                driver.Wait();

                IElement input;
                if (driver.HasElement("//*[@id='GlobalSearchBox']"))
                {
                    var globalSearch = driver.FindElement("//*[@id='GlobalSearchBox']");
                    input = globalSearch;
                }
                else
                {
                    driver.ClickWhenAvailable(
                        _client.ElementMapper.NavigationReference.SearchButton,
                        2.Seconds(),
                        "The Global Search (Categorized Search) button is not available.");

                    input = driver.WaitUntilAvailable(
                        _client.ElementMapper.GlobalSearchReference.Text,
                        2.Seconds(),
                        "The Categorized Search text field is not available.");
                }

                input.Click(_client);
                input.SendKeys(_client, new string[] { criteria });
                input.SendKeys(_client, new string[] { Keys.Enter });

                driver.Wait();

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
                driver.Wait();

                if (driver.HasElement(_client.ElementMapper.GlobalSearchReference.Filter))
                {
                    var filter = driver.FindElement(_client.ElementMapper.GlobalSearchReference.Filter);
                    // Categorized Search
                    var option = driver
                        .FindElements(filter + "//option")
                        .FirstOrDefault(x => x.Text == entity);

                    if (option == null)
                    {
                        throw new InvalidOperationException($"Entity '{entity}' does not exist in the Filter options.");
                    }

                    filter.Click(_client);
                    option.Click(_client);
                }
                else if (driver.HasElement(_client.ElementMapper.GlobalSearchReference.RelevanceSearchResultsTab.Replace("[NAME]", entity)))
                {
                    var entityTab = driver.FindElement(_client.ElementMapper.GlobalSearchReference.RelevanceSearchResultsTab.Replace("[NAME]", entity));
                    // Relevance Search
                    entityTab.Click(_client);
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
                driver.Wait();

                if (driver.HasElement(_client.ElementMapper.GlobalSearchReference.GroupContainer.Replace("[NAME]", filterBy)))
                {
                    var entityPicker = driver.FindElement(_client.ElementMapper.GlobalSearchReference.GroupContainer.Replace("[NAME]", filterBy));
                    // Categorized Search
                    driver.ClickWhenAvailable(
                    entityPicker.Locator + _client.ElementMapper.GlobalSearchReference.FilterValue.Replace("[NAME]", value), new TimeSpan(0,0,5),
                    $"Filter By Value '{value}' does not exist in the Filter options.");
                }
                else if (filterBy == "Record Type" && driver.HasElement(_client.ElementMapper.GlobalSearchReference.RelevanceSearchResultsTab.Replace("[NAME]", value)))
                {
                    var entityTab = driver.FindElement(_client.ElementMapper.GlobalSearchReference.RelevanceSearchResultsTab.Replace("[NAME]", value));
                    // Relevance Search
                    entityTab.Click(_client);
                }
                else
                {
                    throw new InvalidOperationException("Unable to filter global search results.");
                }

                driver.Wait();

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
                driver.Wait();

                if (driver.HasElement(_client.ElementMapper.GlobalSearchReference.CategorizedResultsContainer))
                {
                    var categorizedContainer = driver.FindElement(_client.ElementMapper.GlobalSearchReference.CategorizedResultsContainer);
                    // Categorized Search
                    var records = driver.FindElements(categorizedContainer.Locator + _client.ElementMapper.GlobalSearchReference.CategorizedResults.Replace("[ENTITY]", entity));
                    if (index >= records.Count)
                    {
                        throw new InvalidOperationException($"There was less than {index + 1} records in the search result.");
                    }

                    records[index].Click(_client);
                }
                else if (driver.HasElement("//*[@id='resultsContainer-view'"))
                {
                    var relevanceContainer = driver.FindElement("//*[@id='resultsContainer-view'");
                    // Relevance Search
                    var selectedTab = driver.FindElement(relevanceContainer.Locator + _client.ElementMapper.GlobalSearchReference.RelevanceSearchResultsSelectedTab);
                    if (selectedTab.GetAttribute(_client, "name") != entity)
                    {
                        this.FilterWith(entity);
                    }

                    var links = driver.FindElements(relevanceContainer.Locator + _client.ElementMapper.GlobalSearchReference.RelevanceSearchResultLinks);
                    if (index >= links.Count)
                    {
                        throw new InvalidOperationException($"There was less than {index + 1} records in the search result.");
                    }

                    foreach (IElement link in links) link.DoubleClick(_client, link.Locator);
                    //new Actions(driver).DoubleClick(links[index]).Perform();
                }
                else
                {
                    throw new KeyNotFoundException("Unable to locate global search results.");
                }

                driver.Wait();

                return true;
            });
        }

        #endregion

    }
}
