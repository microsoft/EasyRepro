﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Grid : Element
    {
        private readonly WebClient _client;

        public Grid(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Switches the view to the view supplied
        /// </summary>
        /// <param name="viewName">Name of the view to select</param>
        public void SwitchView(string viewName, string subViewName = null)
        {
            _client.SwitchView(viewName, subViewName);
        }

        /// <summary>
        /// Opens a record in the grid
        /// </summary>
        /// <param name="index">The index of the row to open</param>
        public void OpenRecord(int index)
        {
            _client.OpenRecord(index);
        }

        /// <summary>
        /// Checks / Ticks a record in the grid
        /// </summary>
        /// <param name="index">The index of the row to open</param>
        public void HighLightRecord(int index)
        {
            _client.OpenRecord(index, checkRecord: true);
        }

        /// <summary>
        /// Performs a Quick Find on the grid
        /// </summary>
        /// <param name="searchCriteria">Search term</param>
        /// <param name="clearByDefault">Determines whether to clear the quick find field before supplying the search term</param>
        public void Search(string searchCriteria, bool clearByDefault = true)
        {
            _client.Search(searchCriteria, clearByDefault);
        }

        /// <summary>
        /// Clears the quick find field
        /// </summary>
        public void ClearSearch()
        {
            _client.ClearSearch();
        }

        /// <summary>
        /// Returns a list of items from the current grid
        /// </summary>
        /// <returns></returns>
        public List<GridItem> GetGridItems()
        {
            return _client.GetGridItems();
        }

        /// <summary>
        /// Sorts the grid by the column provided
        /// </summary>
        /// <param name="columnName">Label of the column name</param>
        /// <param name="sortOptionButtonText">Sort option button text</param>
        public void Sort(string columnName, string sortOptionButtonText)
        {
            _client.Sort(columnName, sortOptionButtonText);
        }

        /// <summary>
        /// Filter the grid by the Character provided
        /// </summary>
        /// <param name="filter">Label of footer filter - View records that starts with this letter</param>
        public void FilterByLetter(char filter)
        {
            _client.FilterByLetter(filter);
        }
    }
}