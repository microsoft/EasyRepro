// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    ///  Related Grid class.
    ///  </summary>
    public class RelatedGrid : Element
    {
        private readonly WebClient _client;
        public RelatedGrid(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Opens a record from a related grid
        /// </summary>
        /// <param name="index">Index of the record to open</param>
        public void OpenGridRow(int index)
        {
            _client.OpenRelatedGridRow(index);
        }

        /// <summary>
        /// Clicks a button in the Related grid menu
        /// </summary>
        /// <param name="name">Name of the button to click</param>
        /// <param name="subName">Name of the submenu button to click</param>
        public void ClickCommand(string name, string subName = null, string subSecondName = null)
        {
            _client.ClickRelatedCommand(name, subName, subSecondName);
        }
    }
}
