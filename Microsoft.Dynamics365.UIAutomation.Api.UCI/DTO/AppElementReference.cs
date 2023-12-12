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
       
        public static class Grid
        {
            public static string Container = "Grid_Container";
            public static string PcfContainer = "Grid_PCFContainer";
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
            public static string LegacyReadOnlyRows = "Grid_LegacyReadOnly_Rows";
            public static string Rows = "Grid_Rows";
            public static string Row = "Grid_Row";
            public static string LastRow = "Grid_LastRow";
            public static string Columns = "Grid_Columns";
            public static string Control = "Grid_Control";
            public static string ChartSelector = "Grid_ChartSelector";
            public static string ChartViewList = "Grid_ChartViewList";
            public static string GridSortColumn = "Grid_SortColumn";
            public static string Cells = "Grid_Cells";
            public static string CellContainer = "Grid_CellContainer";
            public static string ViewSelector = "Grid_ViewSelector";
            public static string ViewContainer = "Grid_ViewContainer";
            public static string ViewSelectorMenuItem = "Grid_ViewSelectorMenuItem";
            public static string SubArea = "Grid_SubArea";
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

        public static class Related
        {
            public static string CommandBarButton = "Related_CommandBarButton";
            public static string CommandBarSubButton = "Related_CommandBarSubButton";
            public static string CommandBarOverflowContainer = "Related_CommandBarOverflowContainer";
            public static string CommandBarOverflowButton = "Related_CommandBarOverflowButton";
            public static string CommandBarButtonList = "Related_CommandBarButtonList";
            public static string CommandBarFlyoutButtonList = "Related_CommandBarFlyoutButtonList";
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
            { "Nav_SettingsLauncher"       , "//ul[@data-id='[NAME]Launcher']"},
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
            { "Grid_Container"       , "//div[@data-id='data-set-body-container']"},
            { "Grid_PCFContainer"       , "//div[@ref='eViewport']"},
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
            { "Grid_LegacyReadOnly_Rows"           , "//div[@data-id='grid-container']//div[@class='ag-center-cols-container']//div[@role='row']"},
            { "Grid_Rows"           , "//div[@data-id='entity_control-powerapps_onegrid_control_container']//div[@class='ag-center-cols-container']//div[@role='row']"},
            { "Grid_Row"           , "//div[@class='ag-center-cols-container']//div[@row-index=\'[INDEX]\']"},
            { "Grid_LastRow"           , "//div[@data-id='entity_control-pcf_grid_control_container']//div[@ref='centerContainer']//div[@role='rowgroup']//div[contains(@class, 'ag last-row')]"},
            { "Grid_Control", "//div[contains(@data-lp-id, 'MscrmControls.Grid.PCFGridControl')]" },
            { "Grid_Columns"           , "//div[contains(@ref,'gridHeader')]"},
            { "Grid_ChartSelector"           , "//span[contains(@id,'ChartSelector')]"},
            { "Grid_ChartViewList"           , "//ul[contains(@role,'listbox')]"},
            //{ "Grid_SortColumn",            "//div[contains(@ref,'gridHeader')]//label[@title='[COLNAME]']"},
            { "Grid_SortColumn",            "//div[contains(@role,'columnheader')]//label[@title='[COLNAME]']"},
            { "Grid_Cells", ".//div[@role='gridcell']"},
            { "Grid_CellContainer"    ,"//div[@role='grid'][@data-id='grid-cell-container']"},
            { "Grid_ViewSelector"   , "//button[contains(@id,'ViewSelector')]" },
            //{ "Grid_ViewContainer"   , "//div[contains(@data-id,'ViewSelector')]//div[@role='group']//ul" },
            { "Grid_ViewContainer"   , "//div[contains(@aria-label,'Views')]" },
            //{ "Grid_ViewSelectorMenuItem", ".//span[contains(@class, 'ms-ContextualMenu-itemText')]" },
            { "Grid_ViewSelectorMenuItem", ".//li[contains(@class, 'ms-ContextualMenu-item')]" },
            { "Grid_SubArea"   , "//*[contains(@data-id,'[NAME]')]"},
            

            //Entity
            //{ "Entity_Assign"       , "//button[contains(@data-id,'Assign')]"},
            //{ "Entity_CloseOpportunityWin"       , "//button[contains(@data-id,'MarkAsWon')]"},
            //{ "Entity_CloseOpportunityLoss"       , "//button[contains(@data-id,'MarkAsLost')]"},
            //{ "Entity_Delete"       , "//button[contains(@data-id,'Delete')]"},
            //{ "Entity_FormContainer"       , "//*[@data-id='editFormRoot']"},
            //{ "Entity_FormSelector"       , "//*[@data-id='form-selector']"},
            //{ "Entity_HeaderTitle"       , "//*[@data-id='header_title']"},
            //{ "Entity_HeaderContext"       , ".//div[@data-id='headerFieldsFlyout']"},
            //{ "Entity_Process"       , "//button[contains(@data-id,'MBPF.ConvertTo')]"},
            //{ "Entity_Save"       , "//button[@role='menuitem' and .//*[text()='Save']]"},
            //{ "Entity_SwitchProcess"       , "//button[contains(@data-id,'SwitchProcess')]"},
            //{ "Entity_TextFieldContainer", ".//*[contains(@id, \'[NAME]-FieldSectionItemContainer\')]" },
            //{ "Entity_TextFieldLabel", ".//label[contains(@id, \'[NAME]-field-label\')]" },
            //{ "Entity_TextFieldValue", ".//input[contains(@data-id, \'[NAME].fieldControl\')]" },
            //{ "Entity_TextFieldLookup", ".//*[contains(@id, \'systemuserview_id.fieldControl-LookupResultsDropdown')]" },
            //{ "Entity_TextFieldLookupSearchButton", ".//button[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_search')]" },
            //{ "Entity_TextFieldLookupMenu", "//div[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]') and contains(@data-id,'tabContainer')]" },
            //{ "Entity_LookupFieldExistingValue", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag') and @role='link']" },
            //{ "Entity_LookupFieldDeleteExistingValue", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag_delete')]" },
            //{ "Entity_LookupFieldExpandCollapseButton", ".//button[contains(@data-id,'[NAME].fieldControl-LookupResultsDropdown_[NAME]_expandCollapse')]/descendant::label[not(text()='+0')]" },
            //{ "Entity_LookupFieldNoRecordsText", ".//*[@data-id=\'[NAME].fieldControl-LookupResultsDropdown_[NAME]_No_Records_Text']" },
            //{ "Entity_LookupFieldResultList", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_tab')]" },
            //{ "Entity_LookupFieldResultListItem", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_resultsContainer')]" },
            //{ "Entity_LookupFieldHoverExistingValue", ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_SelectedRecordList')]" },
            //{ "Entity_TextFieldLookupFieldContainer", ".//div[@data-id='[NAME].fieldControl-Lookup_[NAME]']" },
            //{ "Entity_RecordSetNavigatorOpen", "//button[contains(@data-lp-id, 'recordset-navigator')]" },
            //{ "Entity_RecordSetNavigator", "//button[contains(@data-lp-id, 'recordset-navigator')]" },
            //{ "Entity_RecordSetNavList", "//ul[contains(@data-id, 'recordSetNavList')]" },
            //{ "Entity_RecordSetNavCollapseIcon", "//*[contains(@data-id, 'recordSetNavCollapseIcon')]" },
            //{ "Entity_RecordSetNavCollapseIconParent", "//*[contains(@data-id, 'recordSetNavCollapseIcon')]" },
            //{ "Entity_TabList", ".//ul[contains(@id, \"tablist\")]" },
            //{ "Entity_Tab", ".//li[@title='{0}']" },
            //{ "Entity_MoreTabs", ".//div[@data-id='more_button']" },
            //{ "Entity_MoreTabsMenu", "//div[@id='__flyoutRootNode']" },
            //{ "Entity_SubTab", "//div[@id=\"__flyoutRootNode\"]//span[text()=\"{0}\"]" },
            //{ "Entity_FieldControlDateTimeContainer","//div[@data-id='[NAME]-FieldSectionItemContainer']" },
            //{ "Entity_FieldControlDateTimeInputUCI",".//*[contains(@data-id, '[FIELD].fieldControl-date-time-input')]" },
            //{ "Entity_FieldControlDateTimeTimeInputUCI",".//div[contains(@data-id,'[FIELD].fieldControl._timecontrol-datetime-container')]/div/div/input" },
            //{ "Entity_LookupResultsDropdown", "//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_tab')]" },
            //{ "Entity_SubGrid_Row"           , "//div[@data-id='[NAME]-pcf_grid_control_container']//div[@data-id='grid-container']//div[@data-list-index=\'[INDEX]\']"},
            //{ "Entity_SubGrid_LastRow"           , "//div[@ref='centerContainer']//div[@role='rowgroup']//div[contains(@class, 'ag-row-last')]"},
            //{ "Entity_SubGridTitle", "//div[contains(text(), '[NAME]')]" },
            //{ "Entity_SubGridContents", "//div[@id=\"dataSetRoot_[NAME]\"]" },
            //{ "Entity_SubGridList", "//div[@data-id='[NAME]-pcf_grid_control_container']//div[@data-id='grid-container']//div[@data-automationid='ListCell']" },
            //{ "Entity_SubGridListCells", ".//div[@class='ag-center-cols-viewport']//div[@role='rowgroup']//div[@row-index]" },
            //{ "Entity_SubGridViewPickerButton", ".//span[contains(@id, 'ViewSelector') and contains(@id, 'button')]" },
            //{ "Entity_SubGridViewPickerFlyout", "//div[contains(@id, 'ViewSelector') and contains(@flyoutroot, 'flyoutRootNode')]" },
            //{ "Entity_SubGridCommandBar", ".//ul[contains(@data-id, 'CommandBar')]" },
            //{ "Entity_SubGridCommandLabel", ".//button//span[text()=\"[NAME]\"]" },
            //{ "Entity_SubGridOverflowContainer", ".//div[contains(@data-id, 'flyoutRootNode')]" },
            { "Entity_SubGridCommandOverflowButton", ".//button[contains(@data-id, 'OverflowButton')]" },
            //{ "Entity_SubGridOverflowButton", ".//button[contains(@aria-label, '[NAME]')]" },
            //{ "Entity_SubGridHighDensityList", ".//div[contains(@data-lp-id, \"ReadOnlyGrid|[NAME]\") and contains(@class, 'editableGrid')]" },
            //{ "Entity_EditableSubGridList", ".//div[contains(@data-lp-id, \"[NAME]\") and contains(@class, 'editableGrid') and not(contains(@class, 'readonly'))]" },
            //{ "Entity_EditableSubGridListCells", ".//div[contains(@wj-part, 'cells') and contains(@class, 'wj-cells') and contains(@role, 'grid')]" },
            //{ "Entity_EditableSubGridListCellRows", ".//div[contains(@class, 'wj-row') and contains(@role, 'row')]" },
            //{ "Entity_EditableSubGridCells", ".//div[@role='gridcell']" },
            //{ "Entity_SubGridControl", "//div[contains(@data-lp-id, 'MscrmControls.Grid.PCFGridControl')]" },
            //{ "Entity_SubGridCells",".//div[@role='gridcell']"},
            //{ "Entity_SubGridRows",".//div[@role='row' and ./div[@role='gridcell']]"},
            //{ "Entity_SubGridRowsHighDensity",".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Data')]"},
            //{ "Entity_SubGridDataRowsEditable",".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Data')]"},
            //{ "Entity_SubGridHeaders",".//div[contains(@class,'grid-header-text')]"},
            //{ "Entity_SubGridHeadersHighDensity",".//div[contains(@class, 'wj-colheaders') and contains(@wj-part, 'chcells')]/div/div"},
            //{ "Entity_SubGridHeadersEditable",".//div[contains(@class,'wj-row') and contains(@role, 'row') and contains(@aria-label, 'Header')]/div"},
            //{ "Entity_SubGridRecordCheckbox","//div[contains(@data-id,'cell-[INDEX]-1') and contains(@data-lp-id,'[NAME]')]"},
            //{ "Entity_SubGridSearchBox",".//div[contains(@data-id, 'data-set-quickFind-container')]"},
            //{ "Entity_SubGridAddButton", "//button[contains(@data-id,'[NAME].AddNewStandard')]/parent::li/parent::ul[contains(@data-lp-id, 'commandbar-SubGridStandard:[NAME]')]" },
            //{ "Entity_FieldLookupButton","//button[contains(@data-id,'[NAME]_search')]" },
            //{ "Entity_SearchButtonIcon", "//span[contains(@data-id,'microsoftIcon_searchButton')]" },
            //{ "Entity_DuplicateDetectionWindowMarker","//div[contains(@data-id,'ManageDuplicates')]"},
            //{ "Entity_DuplicateDetectionGridRows", "//div[contains(@class,'data-selectable')]" },
            //{ "Entity_DuplicateDetectionIgnoreAndSaveButton", "//button[contains(@data-id,'ignore_save')]"},
            //{ "Entity_BooleanFieldRadioContainer", "//div[contains(@data-id, '[NAME].fieldControl-checkbox-container') and contains(@role,'radiogroup')]"},
            //{ "Entity_BooleanFieldRadioTrue", "//div[contains(@data-id, '[NAME].fieldControl-checkbox-containercheckbox-inner-second')]"},
            //{ "Entity_BooleanFieldRadioFalse", "//div[contains(@data-id, '[NAME].fieldControl-checkbox-containercheckbox-inner-first')]"},
            //{ "Entity_BooleanFieldCheckboxContainer", "//div[contains(@data-id, '[NAME].fieldControl-checkbox-container')]"},
            //{ "Entity_BooleanFieldCheckbox", "//input[contains(@data-id, '[NAME].fieldControl-checkbox-toggle')]"},
            //{ "Entity_BooleanFieldList", "//select[contains(@data-id, '[NAME].fieldControl-checkbox-select')]"},
            //{ "Entity_BooleanFieldFlipSwitchLink", "//div[contains(@data-id, '[NAME]-FieldSectionItemContainer')]"},
            //{ "Entity_BooleanFieldFlipSwitchContainer", "//div[@data-id= '[NAME].fieldControl_container']"},
            //{ "Entity_BooleanFieldButton", "//div[contains(@data-id, '[NAME].fieldControl_container')]"},
            //{ "Entity_BooleanFieldButtonTrue", ".//label[contains(@class, 'first-child')]"},
            //{ "Entity_BooleanFieldButtonFalse", ".//label[contains(@class, 'last-child')]"},
            //{ "Entity_BooleanFieldToggle", "//div[contains(@data-id, '[NAME].fieldControl-toggle-container')]"},
            //{ "Entity_OptionSetFieldContainer", ".//div[@data-id='[NAME].fieldControl-option-set-container']" },
            //{ "Entity_OptionsetStatusCombo", "//div[contains(@data-id, '[NAME].fieldControl-pickliststatus-comboBox')]"},
            //{ "Entity_OptionsetStatusComboButton", "//div[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_button')]"},
            //{ "Entity_OptionsetStatusComboList", "//ul[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_list')]"},
            //{ "Entity_OptionsetStatusTextValue", "//span[contains(@id, '[NAME].fieldControl-pickliststatus-comboBox_text-value')]"},
            //{ "Entity_FormMessageBar", "//*[@id=\"notificationMessageAndButtons\"]/div/div/span" },
            //{ "Entity_FormMessageBarTypeIcon", ".//span[contains(@data-id,'formReadOnlyIcon')]" },
            //{ "Entity_FormNotifcationBar", "//div[contains(@data-id, 'notificationWrapper')]" },
            //{ "Entity_FormNotifcationTypeIcon", ".//span[contains(@id,'notification_icon_')]" },
            //{ "Entity_FormNotifcationExpandButton", ".//span[@id='notificationExpandIcon']" },
            //{ "Entity_FormNotifcationFlyoutRoot", "//div[@id='__flyoutRootNode']" },
            //{ "Entity_FormNotifcationList", ".//ul[@data-id='notificationList']" },

            //Entity Header
            //{ "Entity_Header", "//div[contains(@data-id,'form-header')]"},
            //{ "Entity_Header_Flyout","//div[@data-id='headerFieldsFlyout']" },
            //{ "Entity_Header_FlyoutButton","//button[contains(@id,'headerFieldsExpandButton')]" },
            //{ "Entity_Header_LookupFieldContainer", "//div[@data-id='header_[NAME].fieldControl-Lookup_[NAME]']" },
            //{ "Entity_Header_TextFieldContainer", "//div[@data-id='header_[NAME].fieldControl-text-box-container']" },
            //{ "Entity_Header_OptionSetFieldContainer", "//div[@data-id='header_[NAME]']" },
            //{ "Entity_Header_DateTimeFieldContainer","//div[@data-id='header_[NAME]-FieldSectionItemContainer']" },
                        

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
