// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public static class AppReference
    {
        public static class Application
        {
            public static string Shell = "App_Shell";
        }
       



        public static class MultiSelect
        {
            public static string DivContainer = "MultiSelect_DivContainer";
            public static string InputSearch = "MultiSelect_InputSearch";
            public static string SelectedRecord = "MultiSelect_SelectedRecord";
            public static string SelectedRecordButton = "MultiSelect_SelectedRecord_Button";
            public static string SelectedOptionDeleteButton = "MultiSelect_SelectedRecord_DeleteButton";
            public static string SelectedRecordLabel = "MultiSelect_SelectedRecord_Label";
            public static string FlyoutCaret = "MultiSelect_FlyoutCaret";
            public static string FlyoutOption = "MultiSelect_FlyoutOption";
            public static string FlyoutOptionCheckbox = "MultiSelect_FlyoutOptionCheckbox";
            public static string ExpandCollapseButton = "MultiSelect_ExpandCollapseButton";
        }

        public static class QuickCreate
        {
            public static string QuickCreateFormContext = "QuickCreate_FormContext";
            public static string SaveButton = "QuickCreate_SaveButton";
            public static string SaveAndCloseButton = "QuickCreate_SaveAndCloseButton";
            public static string CancelButton = "QuickCreate_CancelButton";
        }





        public static class Field
        {
            public static string ReadOnly = "Field_ReadOnly";
            public static string Required = "Field_Required";
            public static string RequiredIcon = "Field_RequiredIcon";
        }
        public static class PerformanceWidget
        {
            public static string Container = "Performance_Widget";
            public static string Page = "Performance_WidgetPage";
        }
    
        public static class PowerApp
        {
            public static string ModelFormContainer = "PowerApp_ModalFormContainer";
            public static string Control = "PowerApp_Control";

        }
    }

    public static class AppElements
    {
        public static Dictionary<string, string> Xpath = new Dictionary<string, string>()
        {
            //Application 
            { "App_Shell"    , "//*[@id=\"ApplicationShell\"]"},

            //PowerApp
            { "PowerApp_ModalFormContainer"       , "//iframe[contains(@src,\'[NAME]\')]"},
            { "PowerApp_Control"       , "//div[@data-control-name=\'[NAME]\']"},           

            //Entity
            { "Entity_SubGridCommandOverflowButton", ".//button[contains(@data-id, 'OverflowButton')]" },
          

            //MultiSelect
            { "MultiSelect_DivContainer",     ".//div[contains(@data-id,\"[NAME]-FieldSectionItemContainer\")]" },
            { "MultiSelect_InputSearch",     ".//input[contains(@data-id,\"textInputBox\")]" },
            { "MultiSelect_SelectedRecord",  ".//li" },
            { "MultiSelect_SelectedRecord_DeleteButton", ".//button[contains(@data-id, \"delete\")]" },
            { "MultiSelect_SelectedRecord_Label",  ".//span[contains(@class, \"msos-selected-display-item-text\")]" },
            { "MultiSelect_FlyoutOption",      "//li[label[contains(@title, \"[NAME]\")] and contains(@class,\"msos-option\")]" },
            { "MultiSelect_FlyoutOptionCheckbox", "//input[contains(@class, \"msos-checkbox\")]" },
            { "MultiSelect_FlyoutCaret", "//button[contains(@class, \"msos-caret-button\")]" },

            { "MultiSelect_ExpandCollapseButton", ".//button[contains(@class,\"msos-selecteditems-toggle\")]" },


            //Field
            {"Field_ReadOnly",".//*[@aria-readonly]" },
            {"Field_Required", ".//*[@aria-required]"},
            {"Field_RequiredIcon", ".//div[contains(@data-id, 'required-icon') or contains(@id, 'required-icon')]"},
			
            //QuickCreate
            { "QuickCreate_FormContext" , "//section[contains(@data-id,'quickCreateRoot')]" },
            { "QuickCreate_SaveButton" , "//button[contains(@id,'quickCreateSaveBtn')]" },
            { "QuickCreate_SaveAndCloseButton", "//button[contains(@id,'quickCreateSaveAndCloseBtn')]"},
            { "QuickCreate_CancelButton", "//button[contains(@id,'quickCreateCancelBtn')]"},


            //Performance Width
            { "Performance_Widget","//div[@data-id='performance-widget']//*[text()='Page load']"},
            { "Performance_WidgetPage", "//div[@data-id='performance-widget']//span[contains(text(), '[NAME]')]" }
        };
    }

    public enum LoginResult
    {
        Success,
        Failure,
        Redirect
    }
}
