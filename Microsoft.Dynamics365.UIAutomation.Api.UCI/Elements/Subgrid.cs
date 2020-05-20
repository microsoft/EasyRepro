// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    ///  SubGrid Grid class.
    ///  </summary>
    public class SubGrid : Element
    {
        private readonly WebClient _client;
        public SubGrid(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Clicks a button in the subgrid menu
        /// </summary>
        /// <param name="name">Name of the button to click</param>
        /// <param name="subName">Name of the submenu button to click</param>
        public void ClickCommand(string subGridName, string name, string subName = null, string subSecondName = null)
        {
            _client.ClickSubGridCommand(subGridName, name, subName, subSecondName);
        }

        /// <summary>
        /// Select all records in a subgrid
        /// </summary>
        /// <param name="subGridName">Schema name of the subgrid to select all</param>
        /// <param name="thinkTime">Additional time to wait, if required</param>
        public void ClickSubgridSelectAll(string subGridName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ClickSubgridSelectAll(subGridName, thinkTime);
        }

        /// <summary>
        /// Retrieve the items from a subgrid
        /// </summary>
        /// <param name="subgridName">schemaName of the SubGrid control to retrieve items from</param>
        public List<GridItem> GetSubGridItems(string subgridName)
        {
            return _client.GetSubGridItems(subgridName);
        }

        /// <summary>
        /// Retrieves the number of rows from a subgrid
        /// </summary>
        /// <param name="subgridName">schemaName of the SubGrid control</param>
        /// <returns></returns>
        public int GetSubGridItemsCount(string subgridName)
        {
            return _client.GetSubGridItemsCount(subgridName);
        }

        /// <summary>
        /// Open a record on a subgrid
        /// </summary>
        /// <param name="subgridName">schemaName of the SubGrid control</param>
        /// <param name="index">Index of the record to open</param>
        public void OpenSubGridRecord(string subgridName, int index = 0)
        {
            _client.OpenSubGridRecord(subgridName, index);
        }

        /// <summary>
        /// Performs a Search on the subgrid
        /// </summary>
        /// <param name="searchCriteria">Search term</param>
        /// <param name="clearByDefault">Determines whether to clear the search field before supplying the search term</param>
        public void Search(string subGridName, string searchCriteria, bool clearField = false)
        {
            _client.SearchSubGrid(subGridName, searchCriteria, clearField);
        }

        /// <summary>
        /// Switches the View on a SubGrid
        /// </summary>
        /// <param name="subgridName">schemaName of the SubGrid control</param>
        /// <param name="viewName">Name of the view to select</param>
        public void SwitchView(string subgridName, string viewName)
        {
            _client.SwitchSubGridView(subgridName, viewName);
        }
    }
}
