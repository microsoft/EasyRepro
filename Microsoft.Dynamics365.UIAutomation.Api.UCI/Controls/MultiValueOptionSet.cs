// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class MultiValueOptionSet
    {
        public string Name { get; set; }
        public string[] Values { get; set; }
    }

    internal static class MultiSelect
    {
        public static string DivContainer = ".//div[contains(@data-id,\"[NAME]-FieldSectionItemContainer\")]";
        public static string InputSearch = ".//input[contains(@data-id,\"textInputBox\")]";
        public static string SelectedRecord = ".//li";
        //public string SelectedRecordButton = ".//button[contains(@data-id, \"delete\")]";
        public static string SelectedOptionDeleteButton = ".//button[contains(@data-id, \"delete\")]";
        public static string SelectedRecordLabel = ".//span[contains(@class, \"msos-selected-display-item-text\")]";
        public static string FlyoutCaret = "//button[contains(@class, \"msos-caret-button\")]";
        public static string FlyoutOption = "//li[label[contains(@title, \"[NAME]\")] and contains(@class,\"msos-option\")]";
        public static string FlyoutOptionCheckbox = "//input[contains(@class, \"msos-checkbox\")]";
        public static string ExpandCollapseButton = ".//button[contains(@class,\"msos-selecteditems-toggle\")]";
    }
}
