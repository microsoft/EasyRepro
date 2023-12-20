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

        public static class Lookup
        {
            public static string RelatedEntityLabel = "Lookup_RelatedEntityLabel";
            public static string AdvancedLookupButton = "Lookup_AdvancedLookupButton";
            public static string ViewRows = "Lookup_ViewRows";
            public static string LookupResultRows = "Lookup_ResultRows";
            public static string NewButton = "Lookup_NewButton";
            public static string RecordList = "Lookup_RecordList";
        }

        public static class AdvancedLookup
        {
            public static string Container = "AdvancedLookup_Container";
            public static string SearchInput = "AdvancedLookup_SearchInput";
            public static string ResultRows = "AdvancedLookup_ResultRows";
            public static string FilterTables = "AdvancedLookup_FilterTables";
            public static string FilterTable = "AdvancedLookup_FilterTable";
            public static string AddNewTables = "AdvancedLookup_AddNewTables";
            public static string DoneButton = "AdvancedLookup_DoneButton";
            public static string AddNewRecordButton = "AdvancedLookup_AddNewRecordButton";
            public static string AddNewButton = "AdvancedLookup_AddNewButton";
            public static string ViewSelectorCaret = "AdvancedLookup_ViewSelectorCaret";
            public static string ViewDropdownList = "AdvancedLookup_ViewDropdownList";
            public static string ViewDropdownListItem = "AdvancedLookup_ViewDropdownListItem";
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

            //Navigation
            //{ "Nav_AreaButton"       , "//button[@id='areaSwitcherId']"},
            //{ "Nav_AreaMenu"       , "//*[@data-lp-id='sitemap-area-switcher-flyout']"},
            //{ "Nav_AreaMoreMenu"       , "//ul[@role=\"menubar\"]"},
            //{ "Nav_SubAreaContainer"       , "//*[@data-id=\"navbar-container\"]/div/ul"},
            //{ "Nav_WebAppMenuButton"       , "//*[@id=\"TabArrowDivider\"]/a"},
            //{ "Nav_UCIAppMenuButton"       , "//a[@data-id=\"appBreadCrumb\"]"},
            //{ "Nav_SiteMapLauncherButton", "//button[@data-lp-id=\"sitemap-launcher\"]" },
            //{ "Nav_SiteMapLauncherCloseButton", "//button[@data-id='navbutton']" },
            //{ "Nav_SiteMapAreaMoreButton", "//button[@data-lp-id=\"sitemap-areaBar-more-btn\"]" },
            //{ "Nav_SiteMapSingleArea", "//li[translate(@data-text,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = '[NAME]']" },
            //{ "Nav_AppMenuContainer"       , "//*[@id=\"taskpane-scroll-container\"]"},
            //{ "Nav_SettingsLauncherBar"       , "//button[@data-id='[NAME]Launcher']"},
            //{ "Nav_SettingsLauncher"       , "//ul[@data-id='[NAME]Launcher']"},
            //{ "Nav_AccountManagerButton", "//*[@id=\"mectrl_main_trigger\"]" },
            //{ "Nav_AccountManagerSignOutButton", "//*[@id=\"mectrl_body_signOut\"]" },
            //{ "Nav_GuidedHelp"       , "//*[@id=\"helpLauncher\"]/button"},
            ////{ "Nav_AdminPortal"       , "//*[@id=(\"id-5\")]"},
            //{ "Nav_AdminPortal"       , "//*[contains(@data-id,'officewaffle')]"},
            //{ "Nav_AdminPortalButton" , "//*[@id=(\"O365_AppTile_Admin\")]"},
            //{ "Nav_SearchButton" , "//*[@id=\"searchLauncher\"]/button"},
            //{ "Nav_Search",                "//*[@id=\"categorizedSearchInputAndButton\"]"},
            //{ "Nav_QuickLaunchMenu",                "//div[contains(@data-id,'quick-launch-bar')]"},
            //{ "Nav_QuickLaunchButton",                "//li[contains(@title, '[NAME]')]"},
            //{ "Nav_QuickCreateButton", "//button[contains(@data-id,'quickCreateLauncher')]" },
            //{ "Nav_QuickCreateMenuList", "//ul[contains(@id,'MenuSectionItemsquickCreate')]" },
            //{ "Nav_QuickCreateMenuItems", "//button[@role='menuitem']" },
            //{ "Nav_PinnedSitemapEntity","//li[contains(@data-id,'sitemap-entity-Pinned') and contains(@role,'treeitem')]"},
            //{ "Nav_SitemapMenuGroup", "//ul[@role=\"group\"]"},
            //{ "Nav_SitemapMenuItems", "//li[contains(@data-id,'sitemap-entity')]"},
            //{ "Nav_SitemapSwitcherButton", "//button[contains(@data-id,'sitemap-areaSwitcher-expand-btn')]"},
            //{ "Nav_SitemapSwitcherFlyout","//div[contains(@data-lp-id,'sitemap-area-switcher-flyout')]"},
            //{ "Nav_UCIAppContainer","//div[@id='AppLandingPageContentContainer']"},
            //{ "Nav_UCIAppTile", "//div[@data-type='app-title' and @title='[NAME]']"},

            
           
            

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


            //Related Grid
            { "Related_CommandBarButton", ".//button[contains(@aria-label, '[NAME]') and contains(@id,'SubGrid')]"},
            { "Related_CommandBarOverflowButton", ".//button[contains(@data-id, 'OverflowButton') and contains(@data-lp-id, 'Grid')]"},
            { "Related_CommandBarOverflowContainer", "//div[contains(@data-id, 'flyoutRootNode')]"},
            { "Related_CommandBarSubButton" ,".//button[contains(., '[NAME]')]"},
            { "Related_CommandBarButtonList" ,"//ul[contains(@data-lp-id, 'commandbar-SubGridAssociated')]"},
            { "Related_CommandBarFlyoutButtonList" ,"//ul[contains(@data-id, 'OverflowFlyout')]"},

            //Field
            {"Field_ReadOnly",".//*[@aria-readonly]" },
            {"Field_Required", ".//*[@aria-required]"},
            {"Field_RequiredIcon", ".//div[contains(@data-id, 'required-icon') or contains(@id, 'required-icon')]"},
			
            //QuickCreate
            { "QuickCreate_FormContext" , "//section[contains(@data-id,'quickCreateRoot')]" },
            { "QuickCreate_SaveButton" , "//button[contains(@id,'quickCreateSaveBtn')]" },
            { "QuickCreate_SaveAndCloseButton", "//button[contains(@id,'quickCreateSaveAndCloseBtn')]"},
            { "QuickCreate_CancelButton", "//button[contains(@id,'quickCreateCancelBtn')]"},

            //Lookup
            { "Lookup_RelatedEntityLabel", "//li[contains(@aria-label,'[NAME]') and contains(@data-id,'LookupResultsDropdown')]" },
            { "Lookup_AdvancedLookupButton", "//button[.//label[text()='Advanced lookup']]"},
            { "Lookup_ViewRows", "//li[contains(@data-id,'viewLineContainer')]"},
            { "Lookup_ResultRows", "//li[contains(@data-id,'LookupResultsDropdown') and contains(@data-id,'resultsContainer')]"},
            { "Lookup_NewButton", "//button[contains(@data-id,'addNewBtnContainer') and contains(@data-id,'LookupResultsDropdown')]" },
            { "Lookup_RecordList", ".//div[contains(@id,'RecordList') and contains(@role,'presentation')]" },

            //Advanced Lookup
            { "AdvancedLookup_Container", ".//div[contains(@data-lp-id, 'MscrmControls.FieldControls.AdvancedLookupControl')]" },
            { "AdvancedLookup_SearchInput", "//input[@type='text' and @placeholder='Search']" },
            { "AdvancedLookup_ViewSelectorCaret", ".//span[contains(@class, 'ms-Dropdown-caretDownWrapper')]" },
            { "AdvancedLookup_ViewDropdownList", ".//div[contains(@class, 'dropdownItemsWrapper')]" },
            { "AdvancedLookup_ViewDropdownListItem", "//button[.//span[text()='[NAME]']]"},
            { "AdvancedLookup_ResultRows", "//div[@ref='eLeftContainer']//div[@role='row']" },
            { "AdvancedLookup_FilterTables",  "//li[@role='listitem']//button" },
            { "AdvancedLookup_FilterTable",  "//li[@role='listitem']//button[.//*[text()='[NAME]']]" },
            { "AdvancedLookup_AddNewTables",  "//ul[@role='menu']//button" },
            { "AdvancedLookup_DoneButton", "//button[.//*[text()='Done']]" },
            { "AdvancedLookup_AddNewButton", "//button[.//*[text()='Add new']]" },
            { "AdvancedLookup_AddNewRecordButton", "//button[.//*[text()='Add new record']]" },

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
