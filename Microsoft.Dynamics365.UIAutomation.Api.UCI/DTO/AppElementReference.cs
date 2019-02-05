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
        public static class Navigation
        {
            public static string AreaButton = "Nav_AreaButton";
            public static string AreaMenu = "Nav_AreaMenu";
            public static string AreaMoreMenu = "Nav_AreaMoreMenu";
            public static string SubAreaContainer = "Nav_SubAreaContainer";
            public static string AppMenuButton = "Nav_AppMenuButton";
            public static string SiteMapLauncherButton = "Nav_SiteMapLauncherButton";
            public static string SiteMapLauncherCloseButton = "Nav_SiteMapLauncherCloseButton";
            public static string SiteMapAreaMoreButton = "Nav_SiteMapAreaMoreButton";
            public static string SiteMapSingleArea = "Nav_SiteMapSingleArea";
            public static string AppMenuContainer = "Nav_AppMenuContainer";
            public static string SettingsLauncherBar = "Nav_SettingsLauncherBar";
            public static string SettingsLauncher = "Nav_SettingsLauncher";
            public static string GuidedHelp = "Nav_GuidedHelp";
            public static string AdminPortal = "Nav_AdminPortal";
            public static string AdminPortalButton = "Nav_AdminPortalButton";
            public static string SearchButton = "Nav_SearchButton";
            public static string Search = "Nav_Search";
            public static string QuickLaunchMenu = "Nav_QuickLaunchMenu";
            public static string QuickLaunchButton = "Nav_QuickLaunchButton";
			public static string QuickCreateButton = "Nav_QuickCreateButton";
            public static string QuickCreateMenuList = "Nav_QuickCreateMenuList";
            public static string QuickCreateMenuItems = "Nav_QuickCreateMenuItems";
            public static string PinnedSitemapEntity = "Nav_PinnedSitemapEntity";
            public static string SitemapMenuItems = "Nav_SitemapMenuItems";
            public static string SitemapSwitcherButton = "Nav_SitemapSwitcherButton";
            public static string SitemapSwitcherFlyout = "Nav_SitemapSwitcherFlyout";
        }

        public static class Grid
        {
            public static string Container = "Grid_Container";
            public static string QuickFind = "Grid_QuickFind";
            public static string FirstPage = "Grid_FirstPage";
            public static string NextPage = "Grid_NextPage";
            public static string PreviousPage = "Grid_PreviousPage";
            public static string SelectAll = "Grid_SelectAll";
            public static string ShowChart = "Grid_ShowChart";
            public static string HideChart = "Grid_HideChart";
            public static string JumpBar = "Grid_JumpBar";
            public static string FilterByAll = "Grid_FilterByAll";
            public static string RowsContainer = "Grid_RowsContainer";
            public static string Rows = "Grid_Rows";
            public static string ChartSelector = "Grid_ChartSelector";
            public static string ChartViewList = "Grid_ChartViewList";
            public static string GridSortColumn = "Grid_SortColumn";
            public static string CellContainer = "Grid_CellContainer";
            public static string ViewSelector = "Grid_ViewSelector";
            public static string ViewContainer = "Grid_ViewContainer";
            public static string SubArea = "Grid_SubArea";
        }

        public static class Entity
        {
            public static string Form = "Entity_FormContainer";
            public static string Save = "Entity_Save";
            public static string TextFieldContainer = "Entity_TextFieldContainer";
            public static string TextFieldValue = "Entity_TextFieldValue";
            public static string TextFieldLookup = "Entity_TextFieldLookup";
            public static string TextFieldLookupMenu = "Entity_TextFieldLookupMenu";
            public static string LookupFieldDeleteExistingValue = "Entity_LookupFieldDeleteExistingValue";
            public static string LookupFieldHoverExistingValue = "Entity_LookupFieldHoverExistingValue";
            public static string LookupResultsDropdown = "Entity_LookupResultsDropdown";
            public static string TextFieldLookupFieldContainer = "Entity_TextFieldLookupFieldContainer";
            public static string RecordSetNavigator = "Entity_RecordSetNavigator";
            public static string RecordSetNavigatorOpen = "Entity_RecordSetNavigatorOpen";
            public static string RecordSetNavList = "Entity_RecordSetNavList";
            public static string RecordSetNavCollapseIcon = "Entity_RecordSetNavCollapseIcon";
            public static string RecordSetNavCollapseIconParent = "Entity_RecordSetNavCollapseIconParent";
            public static string FieldControlDateTimeInput = "Entity_FieldControlDateTimeInput";
            public static string FieldControlDateTimeInputUCI = "Entity_FieldControlDateTimeInputUCI";
            public static string Delete = "Entity_Delete";
            public static string Assign = "Entity_Assign";
            public static string SwitchProcess = "Entity_SwitchProcess";
            public static string CloseOpportunityWin = "Entity_CloseOpportunityWin";
            public static string CloseOpportunityLoss = "Entity_CloseOpportunityLoss";
            public static string ProcessButton = "Entity_Process";
            public static string SwitchProcessDialog = "Entity_SwitchProcessDialog";
            public static string TabList = "Entity_TabList";
            public static string Tab = "Entity_Tab";
            public static string SubTab = "Entity_SubTab";
            public static string EntityFooter = "Entity_Footer";
            public static string SubGridTitle = "Entity_SubGridTitle";
            public static string SubGridContents = "Entity_SubGridContents";
            public static string SubGridCells = "Entity_SubGridCells";
            public static string SubGridRows = "Entity_SubGridRows";
            public static string SubGridHeaders = "Entity_SubGridHeaders";
            public static string SubGridRecordCheckbox = "Entity_SubGridRecordCheckbox";
            public static string FieldLookupButton = "Entity_FieldLookupButton";
            public static string SearchButtonIcon = "Entity_SearchButtonIcon";
            public static string EntityHeader = "Entity_Header";
            public static string DuplicateDetectionWindowMarker = "Entity_DuplicateDetectionWindowMarker";
            public static string DuplicateDetectionGridRows = "Entity_DuplicateDetectionGridRows";
            public static string DuplicateDetectionIgnoreAndSaveButton = "Entity_DuplicateDetectionIgnoreAndSaveButton";
            public static string FooterStatusValue = "Entity_FooterStatusField";

        }

        public static class CommandBar
        {
            public static string Container = "Cmd_Container";
            public static string ContainerGrid = "Cmd_ContainerGrid";
            public static string MoreCommandsMenu = "Cmd_MoreCommandsMenu";
            public static string Button = "Cmd_Button";
        }

        public static class Timeline
        {
            public static string SaveAndClose = "Timeline_SaveAndClose";
        }

        public static class Dashboard
        {
            public static string DashboardSelector = "Dashboard_Selector";
            public static string DashboardItem = "Dashboard_Item";
            public static string DashboardItemUCI = "Dashboard_Item_UCI";
        }

        public static class MultiSelect
        {
            public static string DivContainer = "MultiSelect_DivContainer";
            public static string InputSearch = "MultiSelect_InputSearch";
            public static string SelectedRecord = "MultiSelect_SelectedRecord";
            public static string SelectedRecordButton = "MultiSelect_SelectedRecord_Button";
            public static string SelectedRecordLabel = "MultiSelect_SelectedRecord_Label";
            public static string FlyoutList = "MultiSelect_FlyoutList";
            public static string ExpandCollapseButton = "MultiSelect_ExpandCollapseButton";
        }
        
        public static class GlobalSearch
        {
            public static string Button = "Search_Button";
            public static string Text = "Search_Text";
            public static string Filter = "Search_Filter";
            public static string Results = "Search_Result";
            public static string Container = "Search_Container";
            public static string EntityContainer = "Search_EntityContainer";
            public static string Records = "Search_Records";
        }

        public static class BusinessProcessFlow
        {
            public static string NextStage_UCI = "BPF_NextStage_UCI";
            public static string Flyout_UCI = "BPF_Flyout_UCI";
            public static string NextStageButton = "BPF_NextStageButton_UCI";
            public static string SetActiveButton = "BPF_SetActiveButton";
            public static string BusinessProcessFlowFieldName = "BPF_FieldName_UCI";
            public static string TextFieldContainer = "BPF_TextFieldContainer";
            public static string BooleanFieldContainer = "BPF_BooleanFieldContainer";
            public static string DateTimeFieldContainer = "BPF_DateTimeFieldContainer";
            public static string FieldControlDateTimeInputUCI = "BPF_FieldControlDateTimeInputUCI";
            public static string PinStageButton = "BPF_PinStageButton";
            public static string CloseStageButton = "BPF_CloseStageButton";
        }
        public static class Dialogs
        {
            public static class CloseOpportunity
            {
                public static string Ok = "CloseOpportunityDialog_OKButton";
                public static string Cancel = "CloseOpportunityDialog_CancelButton";
                public static string ActualRevenueId = "Dialog_ActualRevenue";
                public static string CloseDateId = "Dialog_CloseDate";
                public static string DescriptionId = "Dialog_Description";
            }
            public static string AssignDialogUserTeamLookupResults = "AssignDialog_UserTeamLookupResults";
            public static string AssignDialogOKButton = "AssignDialog_OKButton";
            public static string AssignDialogToggle = "AssignDialog_ToggleField";
            public static string ConfirmButton = "Dialog_ConfirmButton";
            public static string SwitchProcessDialog = "Entity_SwitchProcessDialog";
            public static string SwitchProcessDialogOK = "Entity_SwitchProcessDialogOK";
            public static string ActiveProcessGridControlContainer = "Entity_ActiveProcessGridControlContainer";
            public static string CancelButton = "Dialog_CancelButton";
            public static string SwitchProcessContainer = "Dialog_SwitchProcessContainer";
        }

		public static class QuickCreate
        {
            public static string SaveButton = "QuickCreate_SaveButton";
            public static string SaveAndCloseButton = "QuickCreate_SaveAndCloseButton";
            public static string CancelButton = "QuickCreate_CancelButton";
        }

        public static class Lookup
        {
            public static string RelatedEntityLabel = "Lookup_RelatedEntityLabel";
            public static string ChangeViewButton = "Lookup_ChangeViewButton";
            public static string ViewRows = "Lookup_ViewRows";
            public static string LookupResultRows = "Lookup_ResultRows";
            public static string NewButton = "Lookup_NewButton";
        }

        public static class Related
        {
            public static string CommandBarButton = "Related_CommandBarButton";
            public static string CommandBarSubButton = "Related_CommandBarSubButton";
        }

        public static class Field
        {
            public static string ReadOnly = "Field_ReadOnly";
        }
    }

    public static class AppElements
    {
        public static Dictionary<string, string> Xpath = new Dictionary<string, string>()
        {
            //Application 
            { "App_Shell"    , "//*[@id=\"ApplicationShell\"]"},

            //Navigation
            { "Nav_AreaButton"       , "//button[contains(@data-lp-id,'sitemap-areaBar-more-btn')]"},
            { "Nav_AreaMenu"       , "//*[@data-lp-id=\"sitemap-areabar-overflow-flyout\"]"},
            { "Nav_AreaMoreMenu"       , "//ul[@role=\"menubar\"]"},
            { "Nav_SubAreaContainer"       , "//*[@data-id=\"navbar-container\"]/div/ul"},
            { "Nav_AppMenuButton"       , "//*[@id=\"TabArrowDivider\"]/a"},
            { "Nav_SiteMapLauncherButton", "//button[@data-lp-id=\"sitemap-launcher\"]" },
            { "Nav_SiteMapLauncherCloseButton", "//button[@aria-label=\"Close Site Map\"]" },
            { "Nav_SiteMapAreaMoreButton", "//button[@data-lp-id=\"sitemap-areaBar-more-btn\"]" },
            { "Nav_SiteMapSingleArea", "//li[translate(@data-text,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = '[NAME]']" },
            { "Nav_AppMenuContainer"       , "//*[@id=\"taskpane-scroll-container\"]"},
            { "Nav_SettingsLauncherBar"       , "//*[@id=\"[NAME]Launcher_buttonaction-bar\"]"},
            { "Nav_SettingsLauncher"       , "//*[@id=\"[NAME]Launcher\"]"},
            { "Nav_GuidedHelp"       , "//*[@id=\"helpLauncher\"]/button"},
            //{ "Nav_AdminPortal"       , "//*[@id=(\"id-5\")]"},
            { "Nav_AdminPortal"       , "//*[contains(@data-id,'officewaffle')]"},
            { "Nav_AdminPortalButton" , "//*[@id=(\"O365_AppTile_Admin\")]"},
            { "Nav_SearchButton" , "//*[@id=\"searchLauncher\"]/button"},
            { "Nav_Search",                "//*[@id=\"categorizedSearchInputAndButton\"]"},
            { "Nav_QuickLaunchMenu",                "//div[contains(@data-id,'quick-launch-bar')]"},
            { "Nav_QuickLaunchButton",                "//li[contains(@title, '[NAME]')]"},
			{ "Nav_QuickCreateButton", "//button[contains(@data-id,'quickCreateLauncher')]" },
            { "Nav_QuickCreateMenuList", "//ul[contains(@id,'MenuSectionItemsquickCreate')]" },
            { "Nav_QuickCreateMenuItems", "//li[@role='menuitem']" },
            { "Nav_PinnedSitemapEntity","//li[contains(@data-id,'sitemap-entity-Pinned') and contains(@role,'treeitem')]"},
            { "Nav_SitemapMenuItems", "//li[contains(@data-id,'sitemap-entity')]"},
            { "Nav_SitemapSwitcherButton", "//button[contains(@data-id,'sitemap-areaSwitcher-expand-btn')]"},
            { "Nav_SitemapSwitcherFlyout","//div[contains(@data-lp-id,'sitemap-area-switcher-flyout')]"},

            
            //Grid
            { "Grid_Container"       , "//*[@id=\"_outer\"]/div/div[3][@data-type=\"Grid\"]"},
            { "Grid_QuickFind"       , "//*[contains(@id, \'quickFind_text\')]"},
            { "Grid_NextPage"       , "//button[contains(@data-id,'moveToNextPage')]"},
            { "Grid_PreviousPage"       , "//button[contains(@data-id,'moveToPreviousPage')]"},
            { "Grid_FirstPage"       , "//button[contains(@data-id,'loadFirstPage')]"},
            { "Grid_SelectAll"       , "//button[contains(@title,'Select All')]"},
            { "Grid_ShowChart"       , "//button[contains(@aria-label,'Show Chart')]"},
            { "Grid_JumpBar"       , "//*[@id=\"JumpBarItemsList\"]"},
            { "Grid_FilterByAll"       , "//*[@id=\"All_link\"]"},
            { "Grid_RowsContainer"       , "//div[contains(@role,'grid')]"},
            { "Grid_Rows"           , "//div[contains(@role,'row')]"},
            { "Grid_ChartSelector"           , "//span[contains(@id,'ChartSelector')]"},
            { "Grid_ChartViewList"           , "//ul[contains(@role,'listbox')]"},
            { "Grid_SortColumn",            "//div[@id='btnheaderselectcolumn']/parent::*//div[text()='[COLNAME]'"},
            { "Grid_CellContainer"    ,"//div[@role='grid'][@data-id='grid-cell-container']"},
            { "Grid_ViewSelector"   , "//span[contains(@id,'ViewSelector')]" },
            { "Grid_ViewContainer"   , "//ul[contains(@id,'ViewSelector')]" },
            { "Grid_SubArea"   , "//*[contains(@data-id,'[NAME]')]"},
            

            //Entity
            { "Entity_Assign"       , "//button[contains(@data-id,'Assign')]"},
            { "Entity_CloseOpportunityWin"       , "//button[contains(@data-id,'MarkAsWon')]"},
            { "Entity_CloseOpportunityLoss"       , "//button[contains(@data-id,'MarkAsLost')]"},
            { "Entity_Delete"       , "//button[contains(@data-id,'Delete')]"},
            { "Entity_FormContainer"       , "//*[@id=\"tab-section\"]"},
            { "Entity_Process"       , "//button[contains(@data-id,'MBPF.ConvertTo')]"},
            { "Entity_Save"       , "//button[contains(@data-id, 'form-save-btn')]"},
            { "Entity_SwitchProcess"       , "//button[contains(@data-id,'SwitchProcess')]"},
            { "Entity_TextFieldContainer", "//*[contains(@id, \'[NAME]-FieldSectionItemContainer\')]" },
            { "Entity_TextFieldValue", "//input[contains(@data-id, \'[NAME].fieldControl\')]" },
            { "Entity_TextFieldLookup", "//*[contains(@id, \'systemuserview_id.fieldControl-LookupResultsDropdown')]" },
            { "Entity_TextFieldLookupMenu", "//div[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]') and contains(@data-id,'tabContainer')]" },
            { "Entity_LookupFieldDeleteExistingValue", "//*[contains(@data-id, \'[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag_delete')]" },
            { "Entity_LookupFieldHoverExistingValue", "//*[contains(@data-id, \'[NAME].fieldControl-LookupResultsDropdown_[NAME]_SelectedRecordList')]" },
            { "Entity_TextFieldLookupFieldContainer", "//*[contains(@data-id, '[NAME].fieldControl-Lookup_[NAME]')]" },
            { "Entity_RecordSetNavigatorOpen", "//button[contains(@data-lp-id, 'recordset-navigator')]" },
            { "Entity_RecordSetNavigator", "//button[contains(@data-lp-id, 'recordset-navigator')]" },
            { "Entity_RecordSetNavList", "//ul[contains(@data-id, 'recordSetNaveList')]" },
            { "Entity_RecordSetNavCollapseIcon", "//*[contains(@data-id, 'recordSetNavCollapseIcon')]" },
            { "Entity_RecordSetNavCollapseIconParent", "//*[contains(@data-id, 'recordSetNavCollapseIcon')]" },
            { "Entity_TabList", "//ul[@id=\"tablist\"]" },
            { "Entity_Tab", "//li[@title=\"{0}\"]" },
            { "Entity_SubTab", "//div[@id=\"__flyoutRootNode\"]//span[text()=\"{0}\"]" },
            { "Entity_FieldControlDateTimeInput","//input[contains(@id,'[FIELD].fieldControl-date-time-input')]" },
            { "Entity_FieldControlDateTimeInputUCI","//input[contains(@data-id,'[FIELD].fieldControl-date-time-input')]" },
            { "Entity_LookupResultsDropdown", "//*[contains(@data-id, \'[NAME].fieldControl-LookupResultsDropdown_[NAME]_tab')]" },
            { "Entity_Footer", "//div[contains(@id,'footerWrapper')]" },
            { "Entity_SubGridTitle", "//div[contains(text(), '[NAME]')]"},
            { "Entity_SubGridContents", "//div[contains(text(), '[NAME]')]/parent::div/parent::div/parent::div"},
            { "Entity_SubGridCells",".//div[contains(@role,'gridcell')]"},
            { "Entity_SubGridRows",".//div[contains(@class,'wj-row')]"},
            { "Entity_SubGridHeaders",".//div[contains(@class,'grid-header-text')]"},
            { "Entity_SubGridRecordCheckbox","//div[contains(@data-id,'cell-[INDEX]-1') and contains(@data-lp-id,'[NAME]')]"},
            { "Entity_FieldLookupButton","//button[contains(@data-id,'[NAME]_search')]" },
            { "Entity_SearchButtonIcon", "//span[contains(@data-id,'microsoftIcon_searchButton')]" },
            { "Entity_Header", "//div[contains(@data-id,'form-header')]"},
            { "Entity_DuplicateDetectionWindowMarker","//div[contains(@data-id,'ManageDuplicates')]"},
            { "Entity_DuplicateDetectionGridRows", "//div[contains(@class,'data-selectable')]" },
            { "Entity_DuplicateDetectionIgnoreAndSaveButton", "//button[contains(@data-id,'ignore_save')]"},
            { "Entity_FooterStatusField",".//span[contains(@role,'status')]"},
			
                        
            //CommandBar
            { "Cmd_Container"       , "//ul[contains(@data-lp-id,\"commandbar-Form\")]"},
            { "Cmd_ContainerGrid"       , "//ul[contains(@data-lp-id,\"commandbar-HomePageGrid\")]"},
            { "Cmd_MoreCommandsMenu"       , "//*[@id=\"__flyoutRootNode\"]"},
            { "Cmd_Button", "//*[contains(text(),'[NAME]')]"},

            //GlobalSearch
            { "Search_Button"       , "//div[@id=\"categorizedSearchHeader\"]//button[contains(@data-id,'search-submit-button')]" },
            { "Search_Text"       , "//input[@aria-label=\"Search box\"]" },
            { "Search_Filter"       , "//select[@aria-label=\"Filter with\"]"},
            { "Search_Container"    , "//div[@id=\"searchResultList\"]"},
            { "Search_EntityContainer"    , "//div[@id=\"View[NAME]\"]"},
            { "Search_Records"    , "//li[@role=\"row\"]" },

            //Timeline
            { "Timeline_SaveAndClose", "//button[contains(@data-id,\"[NAME].SaveAndClose\")]" },

            //MultiSelect
            { "MultiSelect_DivContainer",     "//div[contains(@data-id,\"[NAME]-FieldSectionItemContainer\")]/div/div/div" },
            { "MultiSelect_InputSearch",     "//div[contains(@data-id,\"[NAME].fieldControl-LookupResultsDropdown_[NAME]_InputSearch\")]" },
            { "MultiSelect_SelectedRecord",  "//ul[contains(@data-id,\"[NAME].fieldControl-LookupResultsDropdown_[NAME]_SelectedRecordList\")]//li" },
            { "MultiSelect_SelectedRecord_Button",  "//ul[contains(@data-id,\"[NAME].fieldControl-LookupResultsDropdown_[NAME]_SelectedRecordList\")]//li[descendant::label[text()=\"{0}\"]]/descendant::button" },
            { "MultiSelect_SelectedRecord_Label",  "//ul[contains(@data-id,\"[NAME].fieldControl-LookupResultsDropdown_[NAME]_SelectedRecordList\")]/descendant::label" },
            { "MultiSelect_FlyoutList",      "//div[contains(@id,\"[NAME].fieldControl|__flyoutRootNode_SimpleLookupControlFlyout\")]//li[descendant::label[text()=\"{0}\"]]" },
            { "MultiSelect_ExpandCollapseButton", "//button[contains(@data-id,\"[NAME].fieldControl-LookupResultsDropdown_[NAME]_expandCollapse\")]/descendant::label[not(text()=\"+0\")]" },

            //Dashboard
            { "Dashboard_Selector"       , "//span[contains(@id, 'Dashboard_Selector')]"},
            { "Dashboard_Item"       , "//li[contains(@title, '[NAME]')]"},
            { "Dashboard_Item_UCI"       , "//li[contains(@data-text, '[NAME]')]"},

            //Business Process Flow
            { "BPF_NextStage_UCI"     , "//li[contains(@id,'processHeaderStage')]" },
            { "BPF_Flyout_UCI"     , "//div[contains(@id,'businessProcessFlowFlyoutHeaderContainer')]" },
            { "BPF_NextStageButton_UCI"     , "//button[contains(@data-id,'nextButtonContainer')]" },
            { "BPF_SetActiveButton", "//button[contains(@data-id,'setActiveButton')]" },
            { "BPF_FieldName_UCI"     , "//input[contains(@id,'[NAME]')]" },
            { "BPF_TextFieldContainer", "//div[contains(@data-lp-id, \'header_process_[NAME]\')]" },
            { "BPF_BooleanFieldContainer", "//input[contains(@data-id, \'header_process_[NAME].fieldControl-checkbox-toggle\')]" },
            { "BPF_DateTimeFieldContainer", "//input[contains(@data-id, \'[NAME].fieldControl-date-time-input\')]" },
            { "BPF_FieldControlDateTimeInputUCI","//input[contains(@data-id,'[FIELD].fieldControl-date-time-input')]" },
            { "BPF_PinStageButton","//button[contains(@id,'stageDockModeButton')]"},
            { "BPF_CloseStageButton","//button[contains(@id,'stageContentClose')]"},

            //Related Grid
            { "Related_CommandBarButton", "//button[contains(., '[NAME]') and contains(@data-id,'SubGridAssociated')]"},
            { "Related_CommandBarSubButton" ,"//button[contains(., '[NAME]')]"},

            //Field
            {"Field_ReadOnly",".//*[@aria-readonly]" },

            //Dialogs
            { "AssignDialog_ToggleField" , "//label[contains(@data-id,'rdoMe_id.fieldControl-checkbox-inner-first')]" },
            { "AssignDialog_UserTeamLookupResults" , "//ul[contains(@data-id,'systemuserview_id.fieldControl-LookupResultsDropdown_systemuserview_id_tab')]" },
            { "AssignDialog_OKButton" , "//button[contains(@data-id, 'ok_id')]" },
            { "CloseOpportunityDialog_OKButton" , "//button[contains(@data-id, 'ok_id')]" },
            { "CloseOpportunityDialog_CancelButton" , "//button[contains(@data-id, 'cancel_id')]" },
            { "Dialog_ActualRevenue", "//input[contains(@data-id,'actualrevenue_id')]" },
            { "Dialog_CloseDate", "//input[contains(@data-id,'closedate_id')]" },
            { "Dialog_DescriptionId", "//input[contains(@data-id,'description_id')]" },
            { "Dialog_ConfirmButton" , "//*[@id=\"confirmButton\"]" },
            { "Dialog_CancelButton" , "//*[@id=\"cancelButton\"]" },
            { "Dialog_SwitchProcessContainer" , "//div[contains(@id,'switchProcess_id-FieldSectionItemContainer')]" },
            { "Entity_ActiveProcessGridControlContainer"       , "//div[contains(@data-lp-id,'activeProcessGridControlContainer')]"},
            { "Entity_SwitchProcessDialogOK"       , "//button[contains(@data-id,'ok_id')]"},
            { "SwitchProcess_Container" , "//section[contains(@id, 'popupContainer')]" },
			
            //QuickCreate 
            { "QuickCreate_SaveButton" , "//button[contains(@id,'quickCreateSaveBtn')]" },
            { "QuickCreate_SaveAndCloseButton", "//button[contains(@id,'quickCreateSaveAndCloseBtn')]"},
            { "QuickCreate_CancelButton", "//button[contains(@id,'quickCreateCancelBtn')]"},

            //Lookup
            { "Lookup_RelatedEntityLabel", "//li[contains(@title,'[NAME]') and contains(@data-id,'LookupResultsDropdown')]" },
            { "Lookup_ChangeViewButton", "//button[contains(@data-id,'changeViewBtn')]"},
            { "Lookup_ViewRows", "//li[contains(@data-id,'viewLineContainer')]"},
            { "Lookup_ResultRows", "//li[contains(@data-id,'LookupResultsDropdown') and contains(@data-id,'resultsContainer')]"},
            { "Lookup_NewButton", "//button[contains(@data-id,'addNewBtnContainer') and contains(@data-id,'LookupResultsDropdown')]" }
        };
    }

    public enum LoginResult
    {
        Success,
        Failure,
        Redirect
    }
}




