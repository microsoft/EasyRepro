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
            public static string WebAppMenuButton = "Nav_WebAppMenuButton";
            public static string UCIAppMenuButton = "Nav_UCIAppMenuButton";
            public static string SiteMapLauncherButton = "Nav_SiteMapLauncherButton";
            public static string SiteMapLauncherCloseButton = "Nav_SiteMapLauncherCloseButton";
            public static string SiteMapAreaMoreButton = "Nav_SiteMapAreaMoreButton";
            public static string SiteMapSingleArea = "Nav_SiteMapSingleArea";
            public static string AppMenuContainer = "Nav_AppMenuContainer";
            public static string SettingsLauncherBar = "Nav_SettingsLauncherBar";
            public static string SettingsLauncher = "Nav_SettingsLauncher";
            public static string AccountManagerButton = "Nav_AccountManagerButton";
            public static string AccountManagerSignOutButton = "Nav_AccountManagerSignOutButton";
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
            public static string SitemapMenuGroup = "Nav_SitemapMenuGroup";
            public static string SitemapMenuItems = "Nav_SitemapMenuItems";
            public static string SitemapSwitcherButton = "Nav_SitemapSwitcherButton";
            public static string SitemapSwitcherFlyout = "Nav_SitemapSwitcherFlyout";
            public static string UCIAppContainer = "Nav_UCIAppContainer";
            public static string UCIAppTile = "Nav_UCIAppTile";
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
            public static string RowsContainerCheckbox = "Grid_RowsContainerCheckbox";
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
            public static string FormContext = "Entity_FormContainer";
            public static string FormSelector = "Entity_FormSelector";
            public static string HeaderTitle = "Entity_HeaderTitle";
            public static string HeaderContext = "Entity_HeaderContext";
            public static string Save = "Entity_Save";
            public static string TextFieldContainer = "Entity_TextFieldContainer";
            public static string TextFieldLabel = "Entity_TextFieldLabel";
            public static string TextFieldValue = "Entity_TextFieldValue";
            public static string TextFieldLookup = "Entity_TextFieldLookup";
            public static string TextFieldLookupSearchButton = "Entity_TextFieldLookupSearchButton";
            public static string TextFieldLookupMenu = "Entity_TextFieldLookupMenu";
            public static string LookupFieldExistingValue = "Entity_LookupFieldExistingValue";
            public static string LookupFieldDeleteExistingValue = "Entity_LookupFieldDeleteExistingValue";
            public static string LookupFieldExpandCollapseButton = "Entity_LookupFieldExpandCollapseButton";
            public static string LookupFieldNoRecordsText = "Entity_LookupFieldNoRecordsText";
            public static string LookupFieldResultList = "Entity_LookupFieldResultList";
            public static string LookupFieldResultListItem = "Entity_LookupFieldResultListItem";
            public static string LookupFieldHoverExistingValue = "Entity_LookupFieldHoverExistingValue";
            public static string LookupResultsDropdown = "Entity_LookupResultsDropdown";
            public static string OptionSetFieldContainer = "Entity_OptionSetFieldContainer";
            public static string TextFieldLookupFieldContainer = "Entity_TextFieldLookupFieldContainer";
            public static string RecordSetNavigator = "Entity_RecordSetNavigator";
            public static string RecordSetNavigatorOpen = "Entity_RecordSetNavigatorOpen";
            public static string RecordSetNavList = "Entity_RecordSetNavList";
            public static string RecordSetNavCollapseIcon = "Entity_RecordSetNavCollapseIcon";
            public static string RecordSetNavCollapseIconParent = "Entity_RecordSetNavCollapseIconParent";
            public static string FieldControlDateTimeContainer = "Entity_FieldControlDateTimeContainer";
            public static string FieldControlDateTimeInputUCI = "Entity_FieldControlDateTimeInputUCI";
            public static string FieldControlDateTimeTimeInputUCI = "Entity_FieldControlDateTimeTimeInputUCI";
            public static string Delete = "Entity_Delete";
            public static string Assign = "Entity_Assign";
            public static string SwitchProcess = "Entity_SwitchProcess";
            public static string CloseOpportunityWin = "Entity_CloseOpportunityWin";
            public static string CloseOpportunityLoss = "Entity_CloseOpportunityLoss";
            public static string ProcessButton = "Entity_Process";
            public static string SwitchProcessDialog = "Entity_SwitchProcessDialog";
            public static string TabList = "Entity_TabList";
            public static string Tab = "Entity_Tab";
            public static string MoreTabs = "Entity_MoreTabs";
            public static string MoreTabsMenu = "Entity_MoreTabsMenu";
            public static string SubTab = "Entity_SubTab";
            public static string EntityFooter = "Entity_Footer";
            public static string SubGridTitle = "Entity_SubGridTitle";
            public static string SubGridContents = "Entity_SubGridContents";
            public static string SubGridList = "Entity_SubGridList";
            public static string SubGridListCells = "Entity_SubGridListCells";
            public static string SubGridViewPickerButton = "Entity_SubGridViewPickerButton";
            public static string SubGridViewPickerFlyout = "Entity_SubGridViewPickerFlyout";
            public static string SubGridCommandBar = "Entity_SubGridCommandBar";
            public static string SubGridCommandLabel = "Entity_SubGridCommandLabel";
            public static string SubGridOverflowContainer = "Entity_SubGridOverflowContainer";
            public static string SubGridOverflowButton = "Entity_SubGridOverflowButton";
            public static string SubGridHighDensityList = "Entity_SubGridHighDensityList";
            public static string EditableSubGridList = "Entity_EditableSubGridList";
            public static string EditableSubGridListCells = "Entity_EditableSubGridListCells";
            public static string EditableSubGridListCellRows = "Entity_EditableSubGridListCellRows";
            public static string SubGridCells = "Entity_SubGridCells";
            public static string SubGridRows = "Entity_SubGridRows";
            public static string SubGridRowsHighDensity = "Entity_SubGridRowsHighDensity";
            public static string SubGridDataRowsEditable = "Entity_SubGridDataRowsEditable";
            public static string SubGridHeaders = "Entity_SubGridHeaders";
            public static string SubGridHeadersHighDensity = "Entity_SubGridHeadersHighDensity";
            public static string SubGridHeadersEditable = "Entity_SubGridHeadersEditable";
            public static string SubGridRecordCheckbox = "Entity_SubGridRecordCheckbox";
            public static string SubGridSearchBox = "Entity_SubGridSearchBox";
            public static string SubGridAddButton = "Entity_SubGridAddButton";
            public static string FieldLookupButton = "Entity_FieldLookupButton";
            public static string SearchButtonIcon = "Entity_SearchButtonIcon";
            public static string DuplicateDetectionWindowMarker = "Entity_DuplicateDetectionWindowMarker";
            public static string DuplicateDetectionGridRows = "Entity_DuplicateDetectionGridRows";
            public static string DuplicateDetectionIgnoreAndSaveButton = "Entity_DuplicateDetectionIgnoreAndSaveButton";
            public static string FooterStatusValue = "Entity_FooterStatusField";
            public static string FooterMessageValue = "Entity_FooterMessage";
            public static string EntityBooleanFieldRadioContainer = "Entity_BooleanFieldRadioContainer";
            public static string EntityBooleanFieldRadioTrue = "Entity_BooleanFieldRadioTrue";
            public static string EntityBooleanFieldRadioFalse = "Entity_BooleanFieldRadioFalse";
            public static string EntityBooleanFieldButtonContainer = "Entity_BooleanFieldButton";
            public static string EntityBooleanFieldButtonTrue = "Entity_BooleanFieldButtonTrue";
            public static string EntityBooleanFieldButtonFalse = "Entity_BooleanFieldButtonFalse";
            public static string EntityBooleanFieldCheckboxContainer = "Entity_BooleanFieldCheckboxContainer";
            public static string EntityBooleanFieldCheckbox = "Entity_BooleanFieldCheckbox";
            public static string EntityBooleanFieldList = "Entity_BooleanFieldList";
            public static string EntityBooleanFieldFlipSwitchLink = "Entity_BooleanFieldFlipSwitchLink";
            public static string EntityBooleanFieldFlipSwitchContainer = "Entity_BooleanFieldFlipSwitchContainer";
            public static string EntityBooleanFieldToggle = "Entity_BooleanFieldToggle";
            public static string EntityOptionsetStatusCombo = "Entity_OptionsetStatusCombo";
            public static string EntityOptionsetStatusComboButton = "Entity_OptionsetStatusComboButton";
            public static string EntityOptionsetStatusComboList = "Entity_OptionsetStatusComboList";
            public static string EntityOptionsetStatusTextValue = "Entity_OptionsetStatusTextValue";
            public static string FormMessageBar = "Entity_FormMessageBar";
            public static string FormMessageBarTypeIcon = "Entity_FormMessageBarTypeIcon";
            public static string FormNotifcationBar = "Entity_FormNotifcationBar";
            public static string FormNotifcationExpandButton = "Entity_FormNotifcationExpandButton";
            public static string FormNotifcationFlyoutRoot = "Entity_FormNotifcationFlyoutRoot";
            public static string FormNotifcationList = "Entity_FormNotifcationList";
            public static string FormNotifcationTypeIcon = "Entity_FormNotifcationTypeIcon";

            public static class Header
            {
                public static string Container = "Entity_Header";
                public static string Flyout = "Entity_Header_Flyout";
                public static string FlyoutButton = "Entity_Header_FlyoutButton";
                public static string LookupFieldContainer = "Entity_Header_LookupFieldContainer";
                public static string TextFieldContainer = "Entity_Header_TextFieldContainer";
                public static string OptionSetFieldContainer = "Entity_Header_OptionSetFieldContainer";
                public static string DateTimeFieldContainer = "Entity_Header_DateTimeFieldContainer";
            }
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
            public static string SelectedOptionDeleteButton = "MultiSelect_SelectedRecord_DeleteButton";
            public static string SelectedRecordLabel = "MultiSelect_SelectedRecord_Label";
            public static string FlyoutCaret = "MultiSelect_FlyoutCaret";
            public static string FlyoutOption = "MultiSelect_FlyoutOption";
            public static string FlyoutOptionCheckbox = "MultiSelect_FlyoutOptionCheckbox";
            public static string ExpandCollapseButton = "MultiSelect_ExpandCollapseButton";
        }

        public static class GlobalSearch
        {
            public static string CategorizedSearchButton = "Search_CategorizedSearchButton";
            public static string RelevanceSearchButton = "Search_RelevanceSearchButton";
            public static string Text = "Search_Text";
            public static string Filter = "Search_Filter";
            public static string Results = "Search_Result";
            public static string Container = "Search_Container";
            public static string EntityContainer = "Search_EntityContainer";
            public static string Records = "Search_Records";
            public static string Type = "Search_Type";
            public static string GroupContainer = "Search_GroupContainer";
            public static string FilterValue = "Search_FilterValue";
            public static string RelevanceResultsContainer = "Search_RelevanceResultsContainer";
            public static string RelevanceResults = "Search_RelevanceResults";

        }

        public static class BusinessProcessFlow
        {
            public static string NextStage_UCI = "BPF_NextStage_UCI";
            public static string Flyout_UCI = "BPF_Flyout_UCI";
            public static string NextStageButton = "BPF_NextStageButton_UCI";
            public static string SetActiveButton = "BPF_SetActiveButton";
            public static string BusinessProcessFlowFieldName = "BPF_FieldName_UCI";
            public static string BusinessProcessFlowFormContext = "BPF_FormContext";
            public static string TextFieldContainer = "BPF_TextFieldContainer";
            public static string FieldSectionItemContainer = "BPF_FieldSectionItemContainer";
            public static string TextFieldLabel = "BPF_TextFieldLabel";
            public static string BooleanFieldContainer = "BPF_BooleanFieldContainer";
            public static string BooleanFieldSelectedOption = "BPF_BooleanFieldSelectedOption";
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
            public static class CloseActivity
            {
                public static string Close = "CloseActivityDialog_CloseButton";
                public static string Cancel = "CloseActivityDialog_CancelButton";
            }
            public static string AssignDialogUserTeamLookupResults = "AssignDialog_UserTeamLookupResults";
            public static string AssignDialogOKButton = "AssignDialog_OKButton";
            public static string AssignDialogToggle = "AssignDialog_ToggleField";
            public static string ConfirmButton = "Dialog_ConfirmButton";
            public static string CancelButton = "Dialog_CancelButton";
            public static string DuplicateDetectionIgnoreSaveButton = "DuplicateDetectionDialog_IgnoreAndSaveButton";
            public static string DuplicateDetectionCancelButton = "DuplicateDetectionDialog_CancelButton";
            public static string PublishConfirmButton = "Dialog_PublishConfirmButton";
            public static string PublishCancelButton = "Dialog_PublishCancelButton";
            public static string SetStateDialog = "Dialog_SetStateDialog";
            public static string SetStateActionButton = "Dialog_SetStateActionButton";
            public static string SetStateCancelButton = "Dialog_SetStateCancelButton";
            public static string SwitchProcessDialog = "Entity_SwitchProcessDialog";
            public static string SwitchProcessDialogOK = "Entity_SwitchProcessDialogOK";
            public static string ActiveProcessGridControlContainer = "Entity_ActiveProcessGridControlContainer";
            public static string DialogContext = "Dialog_DialogContext";
            public static string SwitchProcessContainer = "Dialog_SwitchProcessContainer";
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
            public static string ChangeViewButton = "Lookup_ChangeViewButton";
            public static string ViewRows = "Lookup_ViewRows";
            public static string LookupResultRows = "Lookup_ResultRows";
            public static string NewButton = "Lookup_NewButton";
            public static string RecordList = "Lookup_RecordList";
        }

        public static class Related
        {
            public static string CommandBarButton = "Related_CommandBarButton";
            public static string CommandBarSubButton = "Related_CommandBarSubButton";
            public static string CommandBarOverflowContainer = "Related_CommandBarOverflowContainer";
            public static string CommandBarOverflowButton = "Related_CommandBarOverflowButton";
            public static string CommandBarButtonList = "Related_CommandBarButtonList";
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
    }

    public static class AppElements
    {
        public static Dictionary<string, string> Xpath = new Dictionary<string, string>()
        {
            //Application 
            { "App_Shell"    , "//*[@id=\"ApplicationShell\"]"},

            //Navigation
            { "Nav_AreaButton"       , "//button[@id='areaSwitcherId']"},
            { "Nav_AreaMenu"       , "//*[@data-lp-id='sitemap-area-switcher-flyout']"},
            { "Nav_AreaMoreMenu"       , "//ul[@role=\"menubar\"]"},
            { "Nav_SubAreaContainer"       , "//*[@data-id=\"navbar-container\"]/div/ul"},
            { "Nav_WebAppMenuButton"       , "//*[@id=\"TabArrowDivider\"]/a"},
            { "Nav_UCIAppMenuButton"       , "//a[@data-id=\"appBreadCrumb\"]"},
            { "Nav_SiteMapLauncherButton", "//button[@data-lp-id=\"sitemap-launcher\"]" },
            { "Nav_SiteMapLauncherCloseButton", "//button[@data-id='navbutton']" },
            { "Nav_SiteMapAreaMoreButton", "//button[@data-lp-id=\"sitemap-areaBar-more-btn\"]" },
            { "Nav_SiteMapSingleArea", "//li[translate(@data-text,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = '[NAME]']" },
            { "Nav_AppMenuContainer"       , "//*[@id=\"taskpane-scroll-container\"]"},
            { "Nav_SettingsLauncherBar"       , "//button[@data-id='[NAME]Launcher']"},
            { "Nav_SettingsLauncher"       , "//div[@id='[NAME]Launcher']"},
            { "Nav_AccountManagerButton", "//*[@id=\"mectrl_main_trigger\"]" },
            { "Nav_AccountManagerSignOutButton", "//*[@id=\"mectrl_body_signOut\"]" },
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
            { "Nav_QuickCreateMenuItems", "//button[@role='menuitem']" },
            { "Nav_PinnedSitemapEntity","//li[contains(@data-id,'sitemap-entity-Pinned') and contains(@role,'treeitem')]"},
            { "Nav_SitemapMenuGroup", "//ul[@role=\"group\"]"},
            { "Nav_SitemapMenuItems", "//li[contains(@data-id,'sitemap-entity')]"},
            { "Nav_SitemapSwitcherButton", "//button[contains(@data-id,'sitemap-areaSwitcher-expand-btn')]"},
            { "Nav_SitemapSwitcherFlyout","//div[contains(@data-lp-id,'sitemap-area-switcher-flyout')]"},
            { "Nav_UCIAppContainer","//div[@id='AppLandingPageContentContainer']"},
            { "Nav_UCIAppTile", "//div[@data-type='app-title' and @title='[NAME]']"},

            
            //Grid
            { "Grid_Container"       , "//div[@data-type=\"Grid\"]"},
            { "Grid_QuickFind"       , "//*[contains(@id, \'quickFind_text\')]"},
            { "Grid_NextPage"       , "//button[contains(@data-id,'moveToNextPage')]"},
            { "Grid_PreviousPage"       , "//button[contains(@data-id,'moveToPreviousPage')]"},
            { "Grid_FirstPage"       , "//button[contains(@data-id,'loadFirstPage')]"},
            { "Grid_SelectAll"       , "//button[contains(@title,'Select All')]"},
            { "Grid_ShowChart"       , "//button[contains(@aria-label,'Show Chart')]"},
            { "Grid_JumpBar"       , "//*[@id=\"JumpBarItemsList\"]"},
            { "Grid_FilterByAll"       , "//*[@id=\"All_link\"]"},
            { "Grid_RowsContainerCheckbox"  ,   "//div[@role='checkbox']" },
            { "Grid_RowsContainer"       , "//div[contains(@role,'grid')]"},
            { "Grid_Rows"           , "//div[contains(@role,'row')]"},
            { "Grid_ChartSelector"           , "//span[contains(@id,'ChartSelector')]"},
            { "Grid_ChartViewList"           , "//ul[contains(@role,'listbox')]"},
            { "Grid_SortColumn",            "//div[@data-type='Grid']//div[@title='[COLNAME]']//div[contains(@class,'header')]"},
            { "Grid_CellContainer"    ,"//div[@role='grid'][@data-id='grid-cell-container']"},
            { "Grid_ViewSelector"   , "//span[contains(@id,'ViewSelector')]" },
            { "Grid_ViewContainer"   , "//ul[contains(@id,'ViewSelector')]" },
            { "Grid_SubArea"   , "//*[contains(@data-id,'[NAME]')]"},
            

            //Entity
            { "Entity_Assign"       , "//button[contains(@data-id,'Assign')]"},
            { "Entity_CloseOpportunityWin"       , "//button[contains(@data-id,'MarkAsWon')]"},
            { "Entity_CloseOpportunityLoss"       , "//button[contains(@data-id,'MarkAsLost')]"},
            { "Entity_Delete"       , "//button[contains(@data-id,'Delete')]"},
            { "Entity_FormContainer"       , "//*[@data-id='editFormRoot']"},
            { "Entity_FormSelector"       , "//*[@data-id='form-selector']"},
            { "Entity_HeaderTitle"       , "//*[@data-id='header_title']"},
            { "Entity_HeaderContext"       , ".//div[@data-id='headerFieldsFlyout']"},
            { "Entity_Process"       , "//button[contains(@data-id,'MBPF.ConvertTo')]"},
            { "Entity_Save"       , "//button[contains(@id, 'SavePrimary')]"},
            { "Entity_SwitchProcess"       , "//button[contains(@data-id,'SwitchProcess')]"},
            { "Entity_TextFieldContainer", ".//*[contains(@id, \'[NAME]-FieldSectionItemContainer\')]" },
            { "Entity_TextFieldLabel", ".//label[contains(@id, \'[NAME]-field-label\')]" },
            { "Entity_TextFieldValue", ".//input[contains(@data-id, \'[NAME].fieldControl\')]" },
            { "Entity_TextFieldLookup", ".//*[contains(@id, \'systemuserview_id.fieldControl-LookupResultsDropdown')]" },
            { "Entity_TextFieldLookupSearchButton", ".//button[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_search')]" },
            { "Entity_TextFieldLookupMenu", "//div[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]') and contains(@data-id,'tabContainer')]" },
            { "Entity_LookupFieldExistingValue", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag') and @role='link']" },
            { "Entity_LookupFieldDeleteExistingValue", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag_delete')]" },
            { "Entity_LookupFieldExpandCollapseButton", ".//button[contains(@data-id,'[NAME].fieldControl-LookupResultsDropdown_[NAME]_expandCollapse')]/descendant::label[not(text()='+0')]" },
            { "Entity_LookupFieldNoRecordsText", ".//*[@data-id=\'[NAME].fieldControl-LookupResultsDropdown_[NAME]_No_Records_Text']" },
            { "Entity_LookupFieldResultList", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_tab')]" },
            { "Entity_LookupFieldResultListItem", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_resultsContainer')]" },
            { "Entity_LookupFieldHoverExistingValue", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_SelectedRecordList')]" },
            { "Entity_TextFieldLookupFieldContainer", ".//div[@data-id='[NAME].fieldControl-Lookup_[NAME]']" },
            { "Entity_RecordSetNavigatorOpen", "//button[contains(@data-lp-id, 'recordset-navigator')]" },
            { "Entity_RecordSetNavigator", "//button[contains(@data-lp-id, 'recordset-navigator')]" },
            { "Entity_RecordSetNavList", "//ul[contains(@data-id, 'recordSetNavList')]" },
            { "Entity_RecordSetNavCollapseIcon", "//*[contains(@data-id, 'recordSetNavCollapseIcon')]" },
            { "Entity_RecordSetNavCollapseIconParent", "//*[contains(@data-id, 'recordSetNavCollapseIcon')]" },
            { "Entity_TabList", ".//ul[contains(@id, \"tablist\")]" },
            { "Entity_Tab", ".//li[@title='{0}']" },
            { "Entity_MoreTabs", ".//div[@data-id='more_button']" },
            { "Entity_MoreTabsMenu", "//div[@id='__flyoutRootNode']" },
            { "Entity_SubTab", "//div[@id=\"__flyoutRootNode\"]//span[text()=\"{0}\"]" },
            { "Entity_FieldControlDateTimeContainer","//div[@data-id='[NAME]-FieldSectionItemContainer']" },
            { "Entity_FieldControlDateTimeInputUCI",".//*[contains(@data-id, '[FIELD].fieldControl-date-time-input')]" },
            { "Entity_FieldControlDateTimeTimeInputUCI",".//div[contains(@data-id,'[FIELD].fieldControl._timecontrol-datetime-container')]/div/div/input" },
            { "Entity_LookupResultsDropdown", "//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_tab')]" },
            { "Entity_Footer", "//div[contains(@id,'footerWrapper')]" },
            { "Entity_SubGridTitle", "//div[contains(text(), '[NAME]')]" },
            { "Entity_SubGridContents", "//div[@id=\"dataSetRoot_[NAME]\"]" },
            { "Entity_SubGridList", ".//ul[contains(@id, \"[NAME]-GridList\")]" },
            { "Entity_SubGridListCells", ".//div[contains(@wj-part, 'cells') and contains(@class, 'wj-cells') and contains(@role, 'grid')]" },
            { "Entity_SubGridViewPickerButton", ".//span[contains(@id, 'ViewSelector') and contains(@id, 'button')]" },
            { "Entity_SubGridViewPickerFlyout", "//div[contains(@id, 'ViewSelector') and contains(@flyoutroot, 'flyoutRootNode')]" },
            { "Entity_SubGridCommandBar", ".//ul[contains(@data-id, 'CommandBar')]" },
            { "Entity_SubGridCommandLabel", ".//button//span[text()=\"[NAME]\"]" },
            { "Entity_SubGridOverflowContainer", ".//div[contains(@data-id, 'flyoutRootNode')]" },
            { "Entity_SubGridOverflowButton", ".//button[contains(@aria-label, '[NAME]')]" },
            { "Entity_SubGridHighDensityList", ".//div[contains(@data-lp-id, \"ReadOnlyGrid|[NAME]\") and contains(@class, 'editableGrid')]" },
            { "Entity_EditableSubGridList", ".//div[contains(@data-lp-id, \"[NAME]\") and contains(@class, 'editableGrid') and not(contains(@class, 'readonly'))]" },
            { "Entity_EditableSubGridListCells", ".//div[contains(@wj-part, 'cells') and contains(@class, 'wj-cells') and contains(@role, 'grid')]" },
            { "Entity_EditableSubGridListCellRows", ".//div[contains(@class, 'wj-row') and contains(@role, 'row')]" },
            { "Entity_SubGridCells",".//div[contains(@role,'gridcell')]"},
            { "Entity_SubGridRows",".//div[contains(@class,'wj-row')]"},
            { "Entity_SubGridRowsHighDensity",".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Data')]"},
            { "Entity_SubGridDataRowsEditable",".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Data')]"},
            { "Entity_SubGridHeaders",".//div[contains(@class,'grid-header-text')]"},
            { "Entity_SubGridHeadersHighDensity",".//div[contains(@class, 'wj-colheaders') and contains(@wj-part, 'chcells')]/div/div"},
            { "Entity_SubGridHeadersEditable",".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Header')]/div"},
            { "Entity_SubGridRecordCheckbox","//div[contains(@data-id,'cell-[INDEX]-1') and contains(@data-lp-id,'[NAME]')]"},
            { "Entity_SubGridSearchBox",".//div[contains(@data-id, 'data-set-quickFind-container')]"},
            { "Entity_SubGridAddButton", "//button[contains(@data-id,'[NAME].AddNewStandard')]/parent::li/parent::ul[contains(@data-lp-id, 'commandbar-SubGridStandard:[NAME]')]" },
            { "Entity_FieldLookupButton","//button[contains(@data-id,'[NAME]_search')]" },
            { "Entity_SearchButtonIcon", "//span[contains(@data-id,'microsoftIcon_searchButton')]" },
            { "Entity_DuplicateDetectionWindowMarker","//div[contains(@data-id,'ManageDuplicates')]"},
            { "Entity_DuplicateDetectionGridRows", "//div[contains(@class,'data-selectable')]" },
            { "Entity_DuplicateDetectionIgnoreAndSaveButton", "//button[contains(@data-id,'ignore_save')]"},
            { "Entity_FooterStatusField",".//span[contains(@role,'status')]"},
            { "Entity_FooterMessage",".//span[contains(@data-id,'footer-message')]"},
            { "Entity_BooleanFieldRadioContainer", "//div[contains(@data-id, '[NAME].fieldControl-checkbox-container') and contains(@role,'radiogroup')]"},
            { "Entity_BooleanFieldRadioTrue", "//div[contains(@data-id, '[NAME].fieldControl-checkbox-containercheckbox-inner-second')]"},
            { "Entity_BooleanFieldRadioFalse", "//div[contains(@data-id, '[NAME].fieldControl-checkbox-containercheckbox-inner-first')]"},
            { "Entity_BooleanFieldCheckboxContainer", "//div[contains(@data-id, '[NAME].fieldControl-checkbox-container')]"},
            { "Entity_BooleanFieldCheckbox", "//input[contains(@data-id, '[NAME].fieldControl-checkbox-toggle')]"},
            { "Entity_BooleanFieldList", "//select[contains(@data-id, '[NAME].fieldControl-checkbox-select')]"},
            { "Entity_BooleanFieldFlipSwitchLink", "//div[contains(@data-id, '[NAME]-FieldSectionItemContainer')]"},
            { "Entity_BooleanFieldFlipSwitchContainer", "//div[@data-id= '[NAME].fieldControl_container']"},
            { "Entity_BooleanFieldButton", "//div[contains(@data-id, '[NAME].fieldControl_container')]"},
            { "Entity_BooleanFieldButtonTrue", ".//label[contains(@class, 'first-child')]"},
            { "Entity_BooleanFieldButtonFalse", ".//label[contains(@class, 'last-child')]"},
            { "Entity_BooleanFieldToggle", "//div[contains(@data-id, '[NAME].fieldControl-toggle-container')]"},
            { "Entity_OptionSetFieldContainer", ".//div[@data-id='[NAME].fieldControl-option-set-container']" },
            { "Entity_OptionsetStatusCombo", "//div[contains(@data-id, '[NAME].fieldControl-pickliststatus-comboBox')]"},
            { "Entity_OptionsetStatusComboButton", "//div[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_button')]"},
            { "Entity_OptionsetStatusComboList", "//ul[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_list')]"},
            { "Entity_OptionsetStatusTextValue", "//span[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_text-value')]"},
            { "Entity_FormMessageBar", "//*[@id=\"notificationMessageAndButtons\"]/div/div/span" },
            { "Entity_FormMessageBarTypeIcon", ".//span[contains(@data-id,'formReadOnlyIcon')]" },
            { "Entity_FormNotifcationBar", "//div[contains(@data-id, 'notificationWrapper')]" },
            { "Entity_FormNotifcationTypeIcon", ".//span[contains(@id,'notification_icon_')]" },
            { "Entity_FormNotifcationExpandButton", ".//span[@id='notificationExpandIcon']" },
            { "Entity_FormNotifcationFlyoutRoot", "//div[@id='__flyoutRootNode']" },
            { "Entity_FormNotifcationList", ".//ul[@data-id='notificationList']" },

            //Entity Header
            { "Entity_Header", "//div[contains(@data-id,'form-header')]"},
            { "Entity_Header_Flyout","//div[@data-id='headerFieldsFlyout']" },
            { "Entity_Header_FlyoutButton","//button[contains(@id,'headerFieldsExpandButton')]" },
            { "Entity_Header_LookupFieldContainer", "//div[@data-id='header_[NAME].fieldControl-Lookup_[NAME]']" },
            { "Entity_Header_TextFieldContainer", "//div[@data-id='header_[NAME].fieldControl-text-box-container']" },
            { "Entity_Header_OptionSetFieldContainer", "//div[@data-id='header_[NAME]']" },
            { "Entity_Header_DateTimeFieldContainer","//div[@data-id='header_[NAME]-FieldSectionItemContainer']" },
                        
            //CommandBar
            { "Cmd_Container"       , ".//ul[contains(@data-lp-id,\"commandbar-Form\")]"},
            { "Cmd_ContainerGrid"       , "//ul[contains(@data-lp-id,\"commandbar-HomePageGrid\")]"},
            { "Cmd_MoreCommandsMenu"       , "//*[@id=\"__flyoutRootNode\"]"},
            { "Cmd_Button", "//*[contains(text(),'[NAME]')]"},

            //GlobalSearch
            { "Search_RelevanceSearchButton"       , "//div[@aria-label=\"Search box\"]//button" },
            { "Search_CategorizedSearchButton"       , "//button[contains(@data-id,'search-submit-button')]" },
            { "Search_Text"       , "//input[@aria-label=\"Search box\"]" },
            { "Search_Filter"       , "//select[@aria-label=\"Filter with\"]"},
            { "Search_Container"    , "//div[@id=\"searchResultList\"]"},
            { "Search_EntityContainer"    , "//div[@id=\"View[NAME]\"]"},
            { "Search_Records"    , "//li[@role=\"row\"]" },
            { "Search_Type"       , "//select[contains(@data-id,\"search-root-selector\")]"},
            { "Search_GroupContainer", "//label[contains(text(), '[NAME]')]/parent::div"},
            { "Search_FilterValue", "//label[contains(text(), '[NAME]')]"},
            { "Search_RelevanceResultsContainer"       , "//div[@aria-label=\"Search Results\"]"},
            { "Search_RelevanceResults"       , "//li//label[contains(text(), '[ENTITY]')]"},

            //Timeline
            { "Timeline_SaveAndClose", "//button[contains(@data-id,\"[NAME].SaveAndClose\")]" },

            //MultiSelect
            { "MultiSelect_DivContainer",     ".//div[contains(@data-id,\"[NAME]-FieldSectionItemContainer\")]" },
            { "MultiSelect_InputSearch",     ".//input[contains(@class,\"msos-input\")]" },
            { "MultiSelect_SelectedRecord",  ".//li[contains(@class, \"msos-selected-display-item\")]" },
            { "MultiSelect_SelectedRecord_DeleteButton", ".//button[contains(@class, \"msos-quick-delete\")]" },
            { "MultiSelect_SelectedRecord_Label",  ".//span[contains(@class, \"msos-selected-display-item-text\")]" },
            { "MultiSelect_FlyoutOption",      "//li[label[contains(@title, \"[NAME]\")] and contains(@class,\"msos-option\")]" },
            { "MultiSelect_FlyoutOptionCheckbox", "//input[contains(@class, \"msos-checkbox\")]" },
            { "MultiSelect_FlyoutCaret", "//button[contains(@class, \"msos-caret-button\")]" },
            { "MultiSelect_ExpandCollapseButton", ".//button[contains(@class,\"msos-selecteditems-toggle\")]" },

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
            { "BPF_FieldSectionItemContainer", ".//div[contains(@id, \'header_process_[NAME]-FieldSectionItemContainer\')]" },
            { "BPF_FormContext"     , "//div[contains(@id, \'ProcessStageControl-processHeaderStageFlyoutInnerContainer\')]" },
            { "BPF_TextFieldContainer", ".//div[contains(@data-lp-id, \'header_process_[NAME]\')]" },
            { "BPF_TextFieldLabel", "//label[contains(@id, \'header_process_[NAME]-field-label\')]" },
            { "BPF_BooleanFieldContainer", ".//div[contains(@data-id, \'header_process_[NAME].fieldControl-checkbox-container\')]" },
            { "BPF_BooleanFieldSelectedOption", ".//div[contains(@data-id, \'header_process_[NAME].fieldControl-checkbox-container\') and contains(@aria-checked, \'true\')]" },
            { "BPF_DateTimeFieldContainer", ".//input[contains(@data-id, \'[NAME].fieldControl-date-time-input\')]" },
            { "BPF_FieldControlDateTimeInputUCI",".//input[contains(@data-id,'[FIELD].fieldControl-date-time-input')]" },
            { "BPF_PinStageButton","//button[contains(@id,'stageDockModeButton')]"},
            { "BPF_CloseStageButton","//button[contains(@id,'stageContentClose')]"},

            //Related Grid
            { "Related_CommandBarButton", ".//button[contains(@aria-label, '[NAME]') and contains(@id,'SubGrid')]"},
            { "Related_CommandBarOverflowContainer", "//div[contains(@data-id, 'flyoutRootNode')]"},
            { "Related_CommandBarOverflowButton", ".//button[contains(@data-id, 'OverflowButton') and contains(@data-lp-id, 'SubGridAssociated')]"},
            { "Related_CommandBarSubButton" ,".//button[contains(., '[NAME]')]"},
            { "Related_CommandBarButtonList" ,"//ul[contains(@data-lp-id, 'commandbar-SubGridAssociated')]"},

            //Field
            {"Field_ReadOnly",".//*[@aria-readonly]" },
            {"Field_Required", ".//*[@aria-required]"},
            {"Field_RequiredIcon", ".//div[contains(@data-id, 'required-icon') or contains(@id, 'required-icon')]"},

            //Dialogs
            { "AssignDialog_ToggleField" , "//label[contains(@data-id,'rdoMe_id.fieldControl-checkbox-inner-first')]" },
            { "AssignDialog_UserTeamLookupResults" , "//ul[contains(@data-id,'systemuserview_id.fieldControl-LookupResultsDropdown_systemuserview_id_tab')]" },
            { "AssignDialog_OKButton" , "//button[contains(@data-id, 'ok_id')]" },
            { "CloseOpportunityDialog_OKButton" , "//button[contains(@data-id, 'ok_id')]" },
            { "CloseOpportunityDialog_CancelButton" , "//button[contains(@data-id, 'cancel_id')]" },
            { "CloseActivityDialog_CloseButton" , ".//button[contains(@data-id, 'ok_id')]" },
            { "CloseActivityDialog_CancelButton" , ".//button[contains(@data-id, 'cancel_id')]" },
            { "Dialog_DialogContext", "//div[contains(@role, 'dialog')]" },
            { "Dialog_ActualRevenue", "//input[contains(@data-id,'actualrevenue_id')]" },
            { "Dialog_CloseDate", "//input[contains(@data-id,'closedate_id')]" },
            { "Dialog_DescriptionId", "//input[contains(@data-id,'description_id')]" },
            { "Dialog_ConfirmButton" , "//*[@id=\"confirmButton\"]" },
            { "Dialog_CancelButton" , "//*[@id=\"cancelButton\"]" },
            { "DuplicateDetectionDialog_IgnoreAndSaveButton" , "//button[contains(@data-id, 'ignore_save')]" },
            { "DuplicateDetectionDialog_CancelButton" , "//button[contains(@data-id, 'close_dialog')]" },
            { "Dialog_SetStateDialog" , "//div[@data-id=\"SetStateDialog\"]" },
            { "Dialog_SetStateActionButton" , ".//button[@data-id=\"ok_id\"]" },
            { "Dialog_SetStateCancelButton" , ".//button[@data-id=\"cancel_id\"]" },
            { "Dialog_PublishConfirmButton" , "//*[@data-id=\"ok_id\"]" },
            { "Dialog_PublishCancelButton" , "//*[@data-id=\"cancel_id\"]" },
            { "Dialog_SwitchProcessContainer" , "//div[contains(@id,'switchProcess_id-FieldSectionItemContainer')]" },
            { "Entity_ActiveProcessGridControlContainer"       , "//div[contains(@data-lp-id,'activeProcessGridControlContainer')]"},
            { "Entity_SwitchProcessDialogOK"       , "//button[contains(@data-id,'ok_id')]"},
            { "SwitchProcess_Container" , "//section[contains(@id, 'popupContainer')]" },
			
            //QuickCreate
            { "QuickCreate_FormContext" , "//section[contains(@data-id,'quickCreateRoot')]" },
            { "QuickCreate_SaveButton" , "//button[contains(@id,'quickCreateSaveBtn')]" },
            { "QuickCreate_SaveAndCloseButton", "//button[contains(@id,'quickCreateSaveAndCloseBtn')]"},
            { "QuickCreate_CancelButton", "//button[contains(@id,'quickCreateCancelBtn')]"},

            //Lookup
            { "Lookup_RelatedEntityLabel", "//li[contains(@aria-label,'[NAME]') and contains(@data-id,'LookupResultsDropdown')]" },
            { "Lookup_ChangeViewButton", "//button[contains(@data-id,'changeViewBtn')]"},
            { "Lookup_ViewRows", "//li[contains(@data-id,'viewLineContainer')]"},
            { "Lookup_ResultRows", "//li[contains(@data-id,'LookupResultsDropdown') and contains(@data-id,'resultsContainer')]"},
            { "Lookup_NewButton", "//button[contains(@data-id,'addNewBtnContainer') and contains(@data-id,'LookupResultsDropdown')]" },
            { "Lookup_RecordList", ".//div[contains(@id,'RecordList') and contains(@role,'presentation')]" },

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
