// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class Link
    {
        public PageType PageType { get; set; }
        public string LinkText { get; set; }
        public Uri Uri { get; set; }
    }

    public enum PageType
    {
        Unknown,
        Dashboard,
        View,
        Entity
    }

    public static class PageTypeExtensions
    {
        public static PageType ToPageType(this Uri uri)
        {
            var query = uri.Query;

            if (!string.IsNullOrEmpty(query))
            {
                if (string.Compare(query, "pageType=Dashboard", true) == 0)
                    return PageType.Dashboard;
                else if (string.Compare(query, "pageType=EntityList", true) == 0)
                    return PageType.View;
                else if (string.Compare(query, "pageType=EntityRecord", true) == 0)
                    return PageType.Entity;
            }

            return PageType.Unknown;
        }
    }
}