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
    }
}
