// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    /// Encapsulates the list item in the text search dropdown/list
    /// </summary>
    public class ListItem
    {
        /// <summary>
        /// The title which shows up in the text search dropdown/list
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The id for the clickable id in the dropdown/list
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The clickable element underlying the title in the text search dropdown/list
        /// </summary>
        public IWebElement Element { get; set; }
    }
}