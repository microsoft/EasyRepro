// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public static class Elements
    {
        public static Dictionary<string, string> Xpath = new Dictionary<string, string>()
        {
            //Business Process Flow
            { "BPF_NextStage"       , "id(\"stageAdvanceActionContainer\")/div"},
            { "BPF_NextStageMenu"       , "id(\"stageNavigateActionContainer\")/div"},
            { "BPF_NextStageMenuOptions"       , "//div[@class=\"navigateMenuSection\"]"},
            { "BPF_PreviousStage"   , "id(\"stageBackActionContainer\")/div"},
            { "BPF_Hide"            , "id(\"processControlCollapseButton\")" },
            { "BPF_SetActive"       , "id(\"stageSetActiveActionContainer\")" },
            { "BPF_Finish"       , "id(\"stageFinishActionContainer\")" },
            { "BPF_FinishedLabel"       , "//div[@class=\"finishLabelContainer\"]" },
            { "BPF_SelectStage"     , "id(\"stage_[STAGENUM]\")/div[2]/div/div/div/span" },
            { "BPF_Ok"     , "id(\"SwitchProcess-Select\")" },
            { "BPF_TextFieldContainer"     , "//div[@id=\"header_process_[NAME]\"]" },
            { "BPF_CheckboxFieldContainer"     , "//div[@id=\"header_process_[NAME]\"]" },
            { "BPF_OptionSetFieldContainer"     , "//div[@id=\"header_process_[NAME]\"]" },
            { "BPF_LookupFieldContainer"     , "//div[@id=\"header_process_[NAME]\"]" },
            { "BPF_DateFieldContainer"     , "//div[@id=\"header_process_[NAME]\"]" },
            { "BPF_DateFieldInput"     , "//input[@id=\"header_process_[NAME]_iDateInput\"]" },
            { "BPF_GetLookupSearchIcon"     , "//div[@id=\"header_process_[NAME]_lookupSearchIconDiv\"]" },

            //Dialogs
            { "Dialog_Header"       , "id(\"dialogHeaderTitle\")"},
            { "Dialog_DeleteIframe", "id(\"InlineDialog_Iframe\")" },
            { "Dialog_DeleteHeader"       , "id(\"tdDialogHeader\")"},
            { "Dialog_DuplicateWarningIframe", "id(\"InlineDialog_Iframe\")" },
            { "Dialog_DuplicateWarningHeader"       , "id(\"tdDialogHeader\")"},
            { "Dialog_WorkflowHeader", "id(\"DlgHdContainer\")" },
            { "Dialog_ProcessFlowHeader", "id(\"processSwitcherFlyout\")" },
            { "Dialog_CloseOpportunityOk"       , "id(\"ok_id\")"},
            { "Dialog_AssignOk"       , "id(\"ok_id\")"},
            { "Dialog_DeleteOk"       , "id(\"butBegin\")"},
            { "Dialog_DuplicateOk"       , "id(\"butBegin\")"},
            { "Dialog_DuplicateCancel"       , "id(\"cmdDialogCancel\")"},
            { "Dialog_QualifyContinue"       , "id(\"ok_id\")"},
            { "Dialog_QualifyCancel"       , "id(\"cancel_id\")"},
            { "Dialog_ConfirmWorkflow"       , "id(\"butBegin\")"},
            { "Dialog_ConfirmReport"       , "id(\"butBegin\")"},
            { "Dialog_AllRecords"       , "id(\"reportDefault\")"},
            { "Dialog_SelectedRecords"       , "id(\"reportSelected\")"},
            { "Dialog_ViewRecords"       , "id(\"reportView\")"},
            { "Dialog_AddUserHeader"       , "id(\"addUserDescription\")"},
            { "Dialog_AddUser"       , "id(\"buttonNext\")"},
            { "Dialog_AddConnectionHeader", "id(\"EntityTemplateTab.connection.NoRelationship.Form.Mscrm.Form.connection.MainTab-title\")" },
            { "Dialog_AddConnectionSave", "id(\"connection|NoRelationship|Form|Mscrm.Form.connection.SaveAndClose-Large\")" },
            { "Dialog_RoleLookupButton",  "id(\"record2roleid\")" },
            { "Dialog_RoleLookupTable",   "id(\"record2roleid_IMenu\")" },
            { "Dialog_WarningFooter" , "//*[@id=\"crmDialogFooter\"]" },
            { "Dialog_WarningCloseButton", "//*[@id=\"butBegin\"]" },
            { "Dialog_CloseFoundPlaces", "//span[contains(text(), 'Close')]"},
           

            //GuidedHelp
            { "GuidedHelp_MarsOverlay"       , "id(\"marsOverlay\")"},
            { "GuidedHelp_ButBegin"       , "id(\"butBegin\")"},
            { "GuidedHelp_ButtonClose", "id(\"buttonClose\")" },


            //Frames
            { "Frame_ContentPanel"       , "id(\"crmContentPanel\")"},
            { "Frame_ContentFrame"       , "id(\"currentcontentid\")"},
            { "Frame_DialogFrame"       , "id(\"InlineDialog[INDEX]\")"},
            { "Frame_QuickCreateFrame"       , "id(\"globalquickcreate_container_NavBarGloablQuickCreate\")"},
            { "Frame_WizardFrame"       , "id(\"wizardpageframe\")"},

            //Navigation
            { "Nav_ActionGroup"       , "id(\"actionGroupControl\")"},
            { "Nav_SubActionGroup"       , "id(\"actionGroupControl\")"},
            { "Nav_SubActionGroupContainer"       , "id(\"detailActionGroupControl\")"},
            { "Nav_GuidedHelp"       , "id(\"TabButtonHelpId\")/a"},
            { "Nav_AdminPortal"       , "id(\"TabAppSwitcherNode\")/a"},
            { "Nav_Settings"       , "id(\"TabButtonSettingsId\")/a"},
            { "Nav_Options"       , "id(\"navTabButtonSettingsOptionsId\")"},
            { "Nav_PrintPreview"       , "id(\"navTabButtonSettingsPrintPreviewId\")"},
            { "Nav_AppsForCrm"       , "id(\"navTabButtonSettingsNavAppsForCrmId\")"},
            { "Nav_WelcomeScreen"       , "id(\"navTabButtonSettingsNavTourId\")"},
            { "Nav_About"       , "id(\"navTabButtonSettingsAboutId\")"},
            { "Nav_AboutVersionText"       , "//span[contains(text(), 'Version')]"},
            { "Nav_OptOutLP"       , "id(\"navTabButtonSettingsGuidedHelpId\")"},
            { "Nav_Privacy"       , "id(\"NodeSettingsPrivacyStatementId\")"},
            { "Nav_UserInfo"       , "id(\"navTabButtonUserInfoLinkId\")"},
            { "Nav_SignOut"       , "id(\"navTabButtonUserInfoSignOutId\")"},
            { "Nav_TabGlobalMruNode"       , "id(\"TabGlobalMruNode\")"},
            { "Nav_GlobalCreate"       , "id(\"navTabGlobalCreateImage\")"},
            { "Nav_AdvFindSearch"       , "id(\"AdvFindSearch\")"},
            { "Nav_Shuffle"       , "id(\"nav-shuffle\")"},
            { "Nav_TabNode"       , "id(\"TabNode_tab0Tab\")"},
            { "Nav_TabSearch"       , "id(\"TabSearch\")"},
            { "Nav_SearchButton"       , "id(\"SearchNode\")/a"},
            { "Nav_SearchLabel"       , "id(\"findHintText\")"},
            { "Nav_Search"       , "id(\"search\")"},
            { "Nav_StartSearch"       , "id(\"findCriteriaButton\")"},
            { "Nav_OpenAppTabArrowDivider", "//*[@id=\"TabArrowDivider\"]/a" },
            { "Nav_OpenAppContainer", "//*[@id=\"taskpane-scroll-container\"]" },
            

            { "Nav_Tab_Summary",      "//li[contains(@data-id,\"tablist-SUMMARY_TAB\")]" },
            { "Nav_Tab_Details",      "//li[contains(@data-id,\"tablist-DETAILS_TAB\")]" },
            { "Nav_Tab_Perf",         "//li[contains(@data-id,\"tablist-tab_perf\")]" },
                  
            //Grid
            { "Grid_JumpBar"       , "id(\"crmGrid_JumpBar\")"},
            { "Grid_ShowAll"       , "id(\"crmGrid_JumpBar\")/tbody/tr/td[1]"},
            { "Grid_RowSelect"       , "id(\"gridBodyTable\")/tbody/tr[[INDEX]]/td[1]"},
            { "Grid_Filter"       , "id(\"filterButtonLink\")"},
            { "Grid_ChartList"       , "id(\"visualizationListLink\")"},
            { "Grid_ChartDialog"       , "id(\"Dialog_1\")"},
            { "Grid_OpenChartClass"       , "//div[contains(@id,\"crmGrid_crmVisualizationPaneStrip\")]"},
            { "Grid_CloseChartClass"       , "//span[contains(@id,\"runtimeChevron\")]"},
            { "Grid_ToggleSelectAll"   , "id(\"crmGrid_gridBodyTable_checkBox_Image_All\")" },
            { "Grid_FirstPage"       , "id(\"fastRewind\")"},
            { "Grid_NextPage"       , "id(\"_nextPageImg\")"},
            { "Grid_PreviousPage"   , "id(\"_prevPageImg\")" },
            { "Grid_FindCriteriaImg"       , "id(\"crmGrid_findCriteriaButton\")"},
            { "Grid_FindCriteria"       , "id(\"crmGrid_findCriteria\")"},
            { "Grid_GridBodyTable"   , "id(\"gridBodyTable\")" },
            { "Grid_GridBodyTable_Row", "tbody/tr" },
            { "Grid_GridBodyTable_RowCheckbox", "td[1]"},
            { "Grid_DefaultViewIcon"   , "id(\"defaultViewIcon\")" },
            { "Grid_ViewSelector"   , "id(\"crmGrid_SavedNewQuerySelector\")" },
            { "Grid_Refresh"   , "id(\"grid_refresh\")" },
            { "Grid_ViewSelectorContainer"   , "id(\"viewSelectorContainer\")" },
            { "Grid_FirstRow", "id(\"gridBodyTable\")/tbody/tr[1]"},

            //Entity
            { "Entity_Form"       , "id(\"tab0\")"},
            { "Entity_Close"       , "id(\"closeButton\")"},
            { "Entity_Save"       , "id(\"savefooter_statuscontrol\")"},
            { "Entity_FormSectionContentTable" , "id(\"flyoutFormSection_ContentTable\")" }, //GitHub Issue 56
            { "Entity_SelectFormContentTable" , "id(\"Dialog_0\")" }, //GitHub Issue 56
            { "Entity_SelectForm",  "id(\"header_crmFormSelector\")"}, //GitHub Issue #56 - Correcting reference point for SelectForm() method
            { "Entity_SelectFormSection",  "id(\"FormSecNavigationControl-Icon\")"}, //GitHub Issue 56
            { "Entity_TabId"       , "//a/*[contains(text(),'[NAME]')]/parent::a"}, //GitHub Issue 124
            { "Entity_FieldContainer"       , "//div[contains(@id,'[NAME]')]"},
            { "Entity_TextFieldContainer"     , "//div[@id=\"[NAME]\"]" },
            { "Entity_CheckboxFieldContainer"     , "//div[@id=\"[NAME]\"]" },
            { "Entity_TwoOptionFieldList", "//select[@id=\"[NAME]_i\"]" },
            { "Entity_TwoOptionFieldListOption", "//select[@id=\"[NAME]_i\"]//option[@value=\"[VALUE]\"]" },
            { "Entity_TwoOptionFieldTd", "//td[@id=\"[NAME]_d\"]" },
            { "Entity_TwoOptionFieldCheckbox", "//input[@id=\"[NAME]_i\"]" },
            { "Entity_OptionSetFieldContainer"     , "//div[@id=\"[NAME]\"]" },
            { "Entity_LookupFieldContainer"     , "//div[@id=\"[NAME]\"]" },
            { "Entity_LookupFieldTd"     , "//td[@id=\"[NAME]_d\"]" },
            { "Entity_LookupFieldInput"     , "//input[@id=\"[NAME]_ledit\"]" },
            { "Entity_DateFieldContainer"     , "//div[@id=\"[NAME]\"]" },
            { "Entity_DateFieldInput"     , "//input[@id=\"[NAME]_iDateInput\"]" },
            { "Entity_GetLookupSearchIcon"     , "//div[@id=\"[NAME]_lookupSearchIconDiv\"]" },
            { "Entity_Header_TextFieldContainer"     , "//div[@id=\"header_[NAME]\"]" },
            { "Entity_Header_CheckboxFieldContainer"     , "//div[@id=\"header_[NAME]\"]" },
            { "Entity_Header_TwoOptionFieldList", "//select[@id=\"header_[NAME]_i\"]" },
            { "Entity_Header_TwoOptionFieldListOption", "//select[@id=\"header_[NAME]_i\"]//option[@value=\"[VALUE]\"]" },
            { "Entity_Header_TwoOptionFieldDiv", "//div[@id=\"header_[NAME]_d\"]" },
            { "Entity_Header_TwoOptionFieldCheckbox", "//input[@id=\"header_[NAME]_i\"]" },
            { "Entity_Header_OptionSetFieldContainer"     , "//div[@id=\"header_[NAME]\"]" },
            { "Entity_Header_LookupFieldContainer"     , "//div[@id=\"header_[NAME]\"]" },
            { "Entity_Header_DateFieldContainer"     , "//div[@id=\"header_[NAME]\"]" },
            { "Entity_Header_DateFieldInput"     , "//input[@id=\"header_[NAME]_iDateInput\"]" },
            { "Entity_Header_GetLookupSearchIcon"     , "//div[@id=\"header_[NAME]_lookupSearchIconDiv\"]" },
            { "Entity_Footer_TextFieldContainer"     , "//div[@id=\"footer_[NAME]\"]" },
            { "Entity_Footer_CheckboxFieldContainer"     , "//div[@id=\"footer_[NAME]\"]" },
            { "Entity_Footer_TwoOptionFieldTd", "//td[@id=\"footer_[NAME]_d\"]" },
            { "Entity_Footer_TwoOptionFieldCheckbox", "//input[@id=\"footer_[NAME]_i\"]" },
            { "Entity_Footer_OptionSetFieldContainer"     , "//div[@id=\"footer_[NAME]\"]" },
            { "Entity_Footer_LookupFieldContainer"     , "//div[@id=\"footer_[NAME]\"]" },
            { "Entity_Footer_DateFieldContainer"     , "//div[@id=\"footer_[NAME]\"]" },
            { "Entity_Footer_DateFieldInput"     , "//input[@id=\"footer_[NAME]_iDateInput\"]" },
            { "Entity_Footer_GetLookupSearchIcon"     , "//div[@id=\"footer_[NAME]_lookupSearchIconDiv\"]" },
            { "Entity_SubGrid"                      ,"//span[@id='[NAME]_span' and @type='subgrid']" },
            { "Entity_SubGrid_HeaderCell"           ,"//div[@id='[NAME]_divDataArea']/div/table/thead/tr/th" },
            { "Entity_SubGrid_Row"                  ,"//div[@id='[NAME]_divDataArea']/div/table/tbody/tr" },
            { "Entity_SubGrid_Cell"                 ,"//div[@id='[NAME]_divDataArea']/div/table/tbody/tr/td" },
            { "Entity_SubGrid_RowDeleteButton"      ,"//a[contains(@id,'gridBodyTable_delete_[RECORDID]')]" },
            { "Entity_SubGrid_ItemsTotal"           ,"//span[@id='[NAME]_ItemsTotal']" },

            //Related MenuItems
            { "Related_Popout",                 "//li[contains(@data-id,\"tablist-tab_related\")]" },
            { "Related_Common_Activities",      "//div[contains(@data-id,\"form-tab-relatedEntity-navActivities\")]" },

            //Timeline
            { "Timeline_Add_Popout",               "//li[contains(@id,\"notescontrol-action_bar_add_command\")]" },
            { "Timeline_Add_Popout_Appointment",   "//li[contains(@id,\"notescontrol-appointment\")]" },
            { "Timeline_Add_Popout_Email",         "//li[contains(@id,\"notescontrol-email\")]" },
            { "Timeline_Add_Popout_PhoneCall",     "//li[contains(@id,\"notescontrol-phonecall\")]" },
            { "Timeline_Add_Popout_Task",          "//li[contains(@id,\"notescontrol-task\")]" },
            { "Timeline_Add_Popout_Note",          "//li[contains(@id,\"notescontrol-notes\")]" },
            { "Timeline_Add_Popout_Post",          "//li[contains(@id,\"notescontrol-post\")]" },

            { "Timeline_Post_Text",                "id(\"create_post_postText\")" },
            { "Timeline_Post_Add",                 "id(\"create_post_add_btn\")" },
            { "Timeline_Post_Cancel",              "id(\"create_post_cancel_btn\")" },

            { "Timeline_Note_Title",               "id(\"create_note_medium_title\")" },
            { "Timeline_Note_Text",                "id(\"create_note_notesText\")" },
            { "Timeline_Note_Add",                 "id(\"create_note_add_btn\")" },
            { "Timeline_Note_Cancel",              "id(\"create_note_cancel_btn\")" },

            //Global Search
            { "Search_Filter"       , "id(\"filterCombo\")"},
            { "Search_Text"       , "id(\"searchTextBox\")"},
            { "Search_Button"       , "id(\"SearchButton\")"},
            { "Search_Result"       , "//*[contains(@id,\"entityDiv\")]"},
            { "Search_Container"       , "id(\"panoramaContainer\")"},

            //DashBoard
            { "DashBoard_Selector"       , "id(\"dashboardSelector\")"},

            //CommandBar
            { "CommandBar_RibbonManager"       , "id(\"crmRibbonManager\")"},
            { "CommandBar_List"       , "id(\"moreCommandsList\")"},
            { "CommandBar_MoreCommands"       , "id(\"moreCommands\")"},
            
            //ActivityFeed
            { "Notes_NotesControl"       , "id(\"notescontrol\")"},
            { "Notes_NotesWall"       , "id(\"notesWall\")"},
            { "Notes_NotesText"       , "id(\"createNote_notesTextBox\")"},
            { "Notes_PostWall"       , "id(\"activityFeedsWall\")"},
            { "Notes_PostButton"       , "id(\"postButton\")"},
            { "Notes_PostText"       , "id(\"postTextBox\")"},
            { "Notes_ActivityWall"       , "id(\"notescontrolactivityContainer_notescontrol\")"},
            { "Notes_ActivityStatusFilter"       , "id(\"activityWallFilterButton\")"},
            { "Notes_ActivityStatusFilterDialog"       , "id(\"moreActivitiesList\")"},
            { "Notes_ActivityStatusAll"       , "id(\"AllActivitiesButton\")"},
            { "Notes_ActivityStatusOpen"       , "id(\"OpenActivitiesButton\")"},
            { "Notes_ActivityStatusOverdue"       , "id(\"OverdueActivitiesButton\")"},
            { "Notes_ActivityAssociatedView"       , "id(\"OpenAssociatedGridView\")"},
            { "Notes_ActivityPhoneCallOk"       , "id(\"save4210QuickCreateButton\")"},
            { "Notes_ActivityTaskOk", "id(\"save4212QuickCreateButton\")"},
            { "Notes_ActivityMoreActivities", "id(\"moreActivitiesButton\")"},
            { "Notes_ActivityAddEmail", "id(\"AddemailButton\")"},
            { "Notes_ActivityAddAppointment", "id(\"AddappointmentButton\")"},
            { "Notes_ActivityAddPhoneCall", "id(\"activityLabelinlineactivitybar4210\")"},
            { "Notes_ActivityAddTask", "id(\"activityLabelinlineactivitybar4212\")"},
            { "Notes_Done"                 , "id(\"doneSpacer\")"},
            { "Notes_VoiceMail"                 , "id(\"PhoneCallQuickformleftvoiceCheckBoxContol\")"},
            { "Notes_ActivityTaskScheduledEnd" , "id(\"quickCreateActivity4212controlId_scheduledend\")"},
            { "Notes_ActivityAddTaskDueTime"    ,"id(\"selectTable_Date\")"},
                      
            //Login           
            { "Login_UserId", "//input[@type='email']"},
            { "Login_Password", "//input[@type='password']"},
            { "Login_SignIn", "id(\"cred_sign_in_button\")"},
            { "Login_CrmMainPage", "id(\"crmTopBar\")"},
            { "Login_StaySignedIn", "id(\"idSIButton9\")"},
            { "Login_OneTimeCode", "//input[@name='otc']"},

            //Notification           
            { "Notification_AppMessageBar", "id(\"crmAppMessageBar\")"},
            { "Notification_Close", "id(\"crmAppMessageBarCloseButton\")"},

            //Office365Navigation           
            { "Office365Navigation_NavMenu", "id(\"TabAppSwitcherNode\")"},

            //QuickCreate           
            { "QuickCreate_Cancel", "id(\"globalquickcreate_cancel_button_NavBarGloablQuickCreate\")"},
            { "QuickCreate_Save", "id(\"globalquickcreate_save_button_NavBarGloablQuickCreate\")"},
            { "QuickCreate_EnittyContainer", "id(\"actionGroupControl\")"},
            { "QuickCreate_Container", "id(\"globalquickcreate_container_NavBarGloablQuickCreate\")"}, 
            
            
            //LookUp
            { "LookUp_SelObjects", "id(\"selObjects\")"},
            { "LookUp_SavedQuerySelector", "id(\"crmGrid_SavedQuerySelector\")"},
            { "LookUp_DialogCancel", "id(\"cmdDialogCancel\")"},
            { "LookUp_New", "id(\"btnNew\")"},
            { "LookUp_Remove", "id(\"btnRemoveValue\")"},
            { "LookUp_Add", "id(\"btnAdd\")"},
            { "LookUp_Begin", "id(\"butBegin\")"},
            { "LookUp_Search", "input(\"crmGrid_findCriteria\")"},

            //Reports
            { "Report_Close", "id(\"btnCancel\")"},
            { "Report_RunReport", "id(\"btnRun\")"},
   
            //Mobile
            { "Mobile_Page", "id(\"Content\")"},

            //Processes
            { "Process_Name", "id(\"txtWorkflowName\")"},
            { "Process_BlankWorkflow", "id(\"blankWorkflow\")"},
            { "Process_ButtonCreate", "id(\"_MBCreate\")"},
            { "Process_Activate", "id(\"_MBdoActioncrmGrid4703activate\")/span/a"},
            { "Process_Deactivate", "id(\"_MBdoActioncrmGrid4703deactivate\")/span/a"},
            { "Process_Delete", "id(\"_MBdoActioncrmGrid4703delete\")/span/a"},
            { "Process_Begin", "id(\"butBegin\")"},

            //O365
            { "O365_AddUser", "id(\"DashboardWidgetCommonActions\")/div[2]/div/ul/li[1]/a/div[2]"},
            { "O365_FirstName", "id(\"firstname\")"},
            { "O365_LastName", "id(\"lastname\")"},
            { "O365_DisplayName", "id(\"displayname\")"},
            { "O365_UserName", "id(\"username\")"},
            { "O365_License", "id(\"productsinfo\")/div[4]/div[1]/div/div[1]/div[2]/label"},
            { "O365_Add", "id(\"AddUserSubmit\")"},
    };

        public static Dictionary<string, string> ElementId = new Dictionary<string, string>()
        {
            //Frames
            { "Frame_ContentFrameId"       , "currentcontentid"},
            { "Frame_DialogFrameId"       , "InlineDialog[INDEX]_Iframe"},
            { "Frame_QuickCreateFrameId"       , "NavBarGloablQuickCreate"},
            { "Frame_WizardFrameId"       , "wizardpageframe"},
            { "Frame_ViewFrameId"       , "ViewArea"},

            //SetValue
            { "SetValue_ConfirmId"       , "_compositionLinkControl_flyoutLoadingArea-confirm"},
            { "SetValue_FlyOutId"       , "_compositionLinkControl_flyoutLoadingArea_flyOut"},
            { "SetValue_CompositionLinkControlId"       , "_compositionLinkControl_"},
                                
            //Dialogs
            { "Dialog_ActualRevenue"       , "actualrevenue_id"},
            { "Dialog_CloseDate"       , "closedate_id"},
            { "Dialog_Description"       , "description_id"},
            { "Dialog_UserOrTeamLookupId"       , "systemuserview_id"},

            //Add Connection
            { "Dialog_ConnectionDescription"  , "description"},

            //Entity
            { "Entity_TabId"       , "[NAME]_TAB_header_image_div"},

            //Timeline
            { "Timeline_Task_Subject",      "subject" },
            { "Timeline_Task_Description",  "description" },
            { "Timeline_Task_Duration",     "actualdurationminutes" },
            { "Timeline_Task",              "task" },

            { "Timeline_PhoneCall_Subject",      "subject" },
            { "Timeline_PhoneCall_Number",       "phonenumber" },
            { "Timeline_PhoneCall_Description",  "description" },
            { "Timeline_PhoneCall_Duration",     "actualdurationminutes" },
            { "Timeline_PhoneCall",              "phonecall" },

            { "Timeline_Email_Subject",      "subject" },
            { "Timeline_Email_To",           "to" },
            { "Timeline_Email_CC",           "cc" },
            { "Timeline_Email_Bcc",          "bcc" },
            { "Timeline_Email_Description",  "description" },
            { "Timeline_Email_Duration",     "actualdurationminutes" },
            { "Timeline_Email",              "email" },

            { "Timeline_Appointment_Subject",      "subject" },
            { "Timeline_Appointment_Location",     "location" },
            { "Timeline_Appointment_Description",  "description" },
            { "Timeline_Appointment_Duration",     "scheduleddurationminutes" },
            { "Timeline_Appointment",              "appointment" },


            //GuidedHelp
            { "GuidedHelp_Close"       , "closeButton"},

            //Grid
            { "Grid_PrimaryField"       , "gridBodyTable_primaryField_"},

            //Global Search
            { "Search_EntityNameId"       , "entityName"},
            { "Search_RecordNameId"       , "attribone"},
            { "Search_EntityContainersId"       , "entitypic"},

            //ActivityFeed
            { "Notes_ActivityPhoneCallDescId"       , "quickCreateActivity4210controlId_description"},
            { "Notes_ActivityPhoneCallVoiceMailId"       , "PhoneCallQuickformleftvoiceCheckBoxContol"},
            { "Notes_ActivityPhoneCallDirectionId"       , "quickCreateActivity4210controlId_directioncode_i"},
            { "Notes_ActivityTaskSubjectId"      , "quickCreateActivity4212controlId_subject"},
            { "Notes_ActivityTaskDescriptionId"      , "quickCreateActivity4212controlId_description"},
            { "Notes_ActivityTaskPriorityId"      , "quickCreateActivity4212controlId_prioritycode_i"},
            { "Notes_ActivityAddTaskDueTimeId"    ,"selectTable_Date"},
            { "Notes_ActivityAddTaskDueDateId"    ,"quickCreateActivity4212controlId_scheduledend"},

            //Process
            { "Process_Category", "WorkflowCategory"},
            { "Process_Entity", "PrimaryEntity"},

            //Login
            { "Login_TaggingId", "navTabAppSwitcherImage_TabAppSwitcherNode"},

    };

        public static Dictionary<string, string> CssClass = new Dictionary<string, string>()
        {
            //Navigation
            { "Nav_ActionButtonContainerClass"       , "navActionButtonContainer"},
            { "Nav_SubActionElementClass"       , "nav-rowBody"},
            { "Nav_TabButtonLinkClass"       , "navTabButtonLink"},
            { "Nav_ActionGroupContainerClass"       , "navActionGroupContainer"},
            { "Nav_RowLabelClass"       , "nav-rowLabel"},
            { "Nav_TopLevelItemClass"       , "navBarTopLevelItem"},
                      
            //Dialogs
            { "Dialog_SwitchProcessTitleClass"       , "ms-crm-ProcessSwitcher-ProcessTitle"},
            { "Dialog_SelectedRadioButton"       , "ms-crm-ProcessSwitcher-Process-Selected"},

            //SetValue
            { "SetValue_LookupRenderClass"       , "Lookup_RenderButton_td"},
            { "SetValue_EditClass"     , "ms-crm-Inline-Edit"},
            { "SetValue_ValueClass"       , "ms-crm-Inline-Value"},
            { "SetValue_MultiSelectPicklistDelete"       , "sol-quick-delete"},


            //DashBoard
            { "DashBoard_ViewContainerClass"       , "ms-crm-VS-Menu"},

            //CommandBar
            { "CommandBar_FlyoutAnchorArrowClass"       , "flyoutAnchorArrow"},

            //Grid
            { "Grid_ViewContainerClass"     , "ms-crm-VS-Menu"},
            { "Grid_OpenChartClass"       , "ms-crm-ImageStrip-navLeft_VisualRefresh"},
            { "Grid_CloseChartClass"       , "ms-crm-PaneChevron"},
            { "Grid_SortColumnClass"       , "ms-crm-List-Sortable"},
            { "Grid_DataColumnClass"       , "ms-crm-List-DataColumn"},

            //Entity
            { "Entity_LookupRenderClass"       , "Lookup_RenderButton_td"},
            { "Entity_PopoutClass"       , "ms-crm-ImageStrip-popout"},

            //Notifications
            { "Notification_MessageBarRowClass"     , "crmAppMessageBarRow"},
            { "Notification_MessageBarButtonContainerClass"       , "crmAppMessageBarButtonContainer"},
            { "Notification_MessageBarMessageClass"       , "crmAppMessageBarMessage"},
            { "Notification_MessageBarTitleClass"       , "crmAppMessageBarTitle"},

            //Office365Navigation
            { "Office365Navigation_MenuTabContainerClass"     , "o365cs-nav-navMenuTabContainer"},
            { "Office365Navigation_AppItemClass"       , "o365cs-nav-appItem"},

        };

        public static Dictionary<string, string> Name = new Dictionary<string, string>()
        {
            { "Dialog_ReportHeader", "crmDialog" },
            //Navigation
            { "Nav_HomeTab", "TabHome" },
        };
    }

    public static class Reference
    {
        public static class BusinessProcessFlow
        {
            public static string NextStage = "BPF_NextStage";
            public static string NextStageMenu = "BPF_NextStageMenu";
            public static string NextStageMenuOptions = "BPF_NextStageMenuOptions";
            public static string PreviousStage = "BPF_PreviousStage";
            public static string Hide = "BPF_Hide";
            public static string SetActive = "BPF_SetActive";
            public static string Finish = "BPF_Finish";
            public static string FinishedLabel = "BPF_FinishedLabel";
            public static string SelectStage = "BPF_SelectStage";
            public static string Ok = "BPF_Ok";
            public static string TextFieldContainer = "BPF_TextFieldContainer";
            public static string CheckboxFieldContainer = "BPF_CheckboxFieldContainer";
            public static string OptionSetFieldContainer = "BPF_OptionSetFieldContainer";
            public static string LookupFieldContainer = "BPF_LookupFieldContainer";
            public static string DateFieldContainer = "BPF_DateFieldContainer";
            public static string DateFieldInput = "BPF_DateFieldInput";
            public static string GetLookupSearchIcon = "BPF_GetLookupSearchIcon";
        }

        public static class Dialogs
        {
            public static string Header = "Dialog_Header";
            public static string DeleteIframe = "Dialog_DeleteIframe";
            public static string DeleteHeader = "Dialog_DeleteHeader";
            public static string DuplicateWarningIframe = "Dialog_DuplicateWarningIframe";
            public static string DuplicateWarningHeader = "Dialog_DuplicateWarningHeader";
            public static string WorkflowHeader = "Dialog_WorkflowHeader";
            public static string ProcessFlowHeader = "Dialog_ProcessFlowHeader";
            public static string AddConnectionHeader = "Dialog_AddConnectionHeader";
            public static string WarningFooter = "Dialog_WarningFooter";
            public static string WarningCloseButton = "Dialog_WarningCloseButton";
            public static string CloseFoundPlacesDialog = "Dialog_CloseFoundPlaces";

            public static class CloseOpportunity
            {
                public static string ActualRevenueId = "Dialog_ActualRevenue";
                public static string CloseDateId = "Dialog_CloseDate";
                public static string DescriptionId = "Dialog_Description";
                public static string Ok = "Dialog_CloseOpportunityOk";
            }

            public static class AddConnection
            {
                public static string DescriptionId = "Dialog_ConnectionDescription";
                public static string Save = "Dialog_AddConnectionSave";
                public static string RoleLookupButton = "Dialog_RoleLookupButton";
                public static string RoleLookupTable = "Dialog_RoleLookupTable";
            }
            public static class Assign
            {
                public static string Ok = "Dialog_AssignOk";
                public static string UserOrTeamLookupId = "Dialog_UserOrTeamLookupId";
            }
            public static class Delete
            {
                public static string Ok = "Dialog_DeleteOk";
            }
            public static class SwitchProcess
            {
                public static string Process = "Dialog_SwitchProcessTitleClass";
                public static string SelectedRadioButton = "Dialog_SelectedRadioButton";
            }
            public static class DuplicateDetection
            {
                public static string Save = "Dialog_DuplicateOk";
                public static string Cancel = "Dialog_DuplicateCancel";

            }

            public static class QualifyLead
            {
                public static string QualifyContinue = "Dialog_QualifyContinue";
                public static string QualifyCancel = "Dialog_QualifyCancel";
            }

            public static class RunWorkflow
            {
                public static string Confirm = "Dialog_ConfirmWorkflow";
            }
            public static class RunReport
            {
                public static string Header = "Dialog_ReportHeader";
                public static string Confirm = "Dialog_ConfirmReport";
                public static string Default = "Dialog_AllRecords";
                public static string Selected = "Dialog_SelectedRecords";
                public static string View = "Dialog_ViewRecords";
            }

            public static class AddUser
            {
                public static string Header = "Dialog_AddUserHeader";
                public static string Add = "Dialog_AddUser";
            }

}
        public static class SetValue
        {
            public static string LookupRenderClass = "SetValue_LookupRenderClass";
            public static string EditClass = "SetValue_EditClass";
            public static string ValueClass = "SetValue_ValueClass";
            public static string Confirm = "SetValue_ConfirmId";
            public static string FlyOut = "SetValue_FlyOutId";
            public static string CompositionLinkControl = "SetValue_CompositionLinkControlId";
            public static string Cancel = "SetValue_Cancel";
            public static string Save = "SetValue_Save";
            public static string MultiSelectPicklistDeleteClass = "SetValue_MultiSelectPicklistDelete";
        }

        public static class QuickCreate
        {
            public static string Cancel = "QuickCreate_Cancel";
            public static string Save = "QuickCreate_Save";
            public static string EntityContainer = "QuickCreate_EnittyContainer";
            public static string Container = "QuickCreate_Container";
        }
        public static class Office365Navigation
        {
            public static string NavMenu = "Office365Navigation_NavMenu";
            public static string MenuTabContainer = "Office365Navigation_MenuTabContainerClass";
            public static string AppItem = "Office365Navigation_AppItemClass";

        }

        public static class Frames
        {
            public static string ContentPanel = "Frame_ContentPanel";
            public static string ContentFrameId = "Frame_ContentFrameId";
            public static string DialogFrame = "Frame_DialogFrame";
            public static string DialogFrameId = "Frame_DialogFrameId";
            public static string QuickCreateFrame = "Frame_QuickCreateFrame";
            public static string QuickCreateFrameId = "Frame_QuickCreateFrameId";
            public static string WizardFrame = "Frame_WizardFrame";
            public static string WizardFrameId = "Frame_WizardFrameId";
            public static string ViewFrameId = "Frame_ViewFrameId";

        }

        public static class Navigation
        {
            public static string HomeTab = "Nav_HomeTab";
            public static string TopLevelItem = "Nav_TopLevelItemClass";
            public static string ActionGroup = "Nav_ActionGroup";
            public static string ActionButtonContainer = "Nav_ActionButtonContainerClass";
            public static string SubActionGroup = "Nav_SubActionGroup";
            public static string SubActionGroupContainer = "Nav_SubActionGroupContainer";
            public static string SubActionElementClass = "Nav_SubActionElementClass";
            public static string GuidedHelp = "Nav_GuidedHelp";
            public static string AdminPortal = "Nav_AdminPortal";
            public static string Settings = "Nav_Settings";
            public static string Options = "Nav_Options";
            public static string PrintPreview = "Nav_PrintPreview";
            public static string AppsForCRM = "Nav_AppsForCrm";
            public static string WelcomeScreen = "Nav_WelcomeScreen";
            public static string About = "Nav_About";
            public static string AboutVersionText = "Nav_AboutVersionText";
            public static string OptOutLP = "Nav_OptOutLP";
            public static string Privacy = "Nav_Privacy";
            public static string UserInfo = "Nav_UserInfo";
            public static string SignOut = "Nav_SignOut";
            public static string TabGlobalMruNode = "Nav_TabGlobalMruNode";
            public static string GlobalCreate = "Nav_GlobalCreate";
            public static string AdvFindSearch = "Nav_AdvFindSearch";
            public static string Shuffle = "Nav_Shuffle";
            public static string TabNode = "Nav_TabNode";
            public static string SearchLabel = "Nav_SearchLabel";
            public static string SearchButton = "Nav_SearchButton";
            public static string Search = "Nav_Search";
            public static string StartSearch = "Nav_StartSearch";
            public static string TabButtonLink = "Nav_TabButtonLinkClass";
            public static string ActionGroupContainer = "Nav_ActionGroupContainerClass";
            public static string RowLabel = "Nav_RowLabelClass";
            public static string SummaryTab = "Nav_Tab_Summary";
            public static string DetailsTab = "Nav_Tab_Details";
            public static string PerfTab = "Nav_Tab_Perf";
            public static string OpenAppTabDivider = "Nav_OpenAppTabArrowDivider";
            public static string OpenAppContainer = "Nav_OpenAppContainer";
        }
        public static class Grid
        {
            public static string JumpBar = "Grid_JumpBar";
            public static string ShowAll = "Grid_ShowAll";
            public static string RowSelect = "Grid_RowSelect";
            public static string Filter = "Grid_Filter";
            public static string ChartList = "Grid_ChartList";
            public static string ChartDialog = "Grid_ChartDialog";
            public static string ToggleSelectAll = "Grid_ToggleSelectAll";
            public static string FirstPage = "Grid_FirstPage";
            public static string NextPage = "Grid_NextPage";
            public static string PreviousPage = "Grid_PreviousPage";
            public static string DefaultViewIcon = "Grid_DefaultViewIcon";
            public static string FindCriteriaImg = "Grid_FindCriteriaImg";
            public static string GridBodyTable = "Grid_GridBodyTable";
            public static string GridBodyTableRow = "Grid_GridBodyTable_Row";
            public static string GridBodyTableRowCheckbox = "Grid_GridBodyTable_RowCheckbox";
            public static string FindCriteria = "Grid_FindCriteria";
            public static string ViewSelector = "Grid_ViewSelector";
            public static string Refresh = "Grid_Refresh";
            public static string ViewContainer = "Grid_ViewContainerClass";
            public static string OpenChart = "Grid_OpenChartClass";
            public static string CloseChart = "Grid_CloseChartClass";
            public static string SortColumn = "Grid_SortColumnClass";
            public static string PrimaryField = "Grid_PrimaryField";
            public static string DataColumn = "Grid_DataColumnClass";
            public static string ViewSelectorContainer = "Grid_ViewSelectorContainer";
            public static string FirstRow = "Grid_FirstRow";

        }

        public static class Entity
        {
            public static string Form = "Entity_Form";
            public static string Close = "Entity_Close";
            public static string Tab = "Entity_TabId";
            public static string Save = "Entity_Save";
            public static string SelectFormContentTable = "Entity_SelectFormContentTable"; //GitHub Issue 56
            public static string FormSectionContentTable = "Entity_FormSectionContentTable"; //GitHub Issue 56
            public static string SelectForm = "Entity_SelectForm";
            public static string SelectFormSection = "Entity_SelectFormSection"; //GitHub Issue 56
            public static string LookupRender = "Entity_LookupRenderClass";
            public static string Popout = "Entity_PopoutClass";
            public static string FieldContainer = "Entity_FieldContainer";
            public static string TextFieldContainer = "Entity_TextFieldContainer";
            public static string CheckboxFieldContainer = "Entity_CheckboxFieldContainer";
            public static string TwoOptionFieldList = "Entity_TwoOptionFieldList";
            public static string TwoOptionFieldListOption = "Entity_TwoOptionFieldListOption";
            public static string TwoOptionFieldTd = "Entity_TwoOptionFieldTd";
            public static string TwoOptionFieldCheckbox = "Entity_TwoOptionFieldCheckbox";
            public static string OptionSetFieldContainer = "Entity_OptionSetFieldContainer";
            public static string LookupFieldContainer = "Entity_LookupFieldContainer";
            public static string LookupFieldTd= "Entity_LookupFieldTd";
            public static string LookupFieldInput= "Entity_LookupFieldInput";
            public static string DateFieldContainer = "Entity_DateFieldContainer";
            public static string DateFieldInput = "Entity_DateFieldInput";
            public static string GetLookupSearchIcon = "Entity_GetLookupSearchIcon";
            public static string TextFieldContainer_Header = "Entity_Header_TextFieldContainer";
            public static string CheckboxFieldContainer_Header = "Entity_Header_CheckboxFieldContainer";
            public static string TwoOptionFieldList_Header = "Entity_Header_TwoOptionFieldList";
            public static string TwoOptionFieldListOption_Header = "Entity_Header_TwoOptionFieldListOption";
            public static string TwoOptionFieldDiv_Header = "Entity_Header_TwoOptionFieldDiv";
            public static string TwoOptionFieldCheckbox_Header = "Entity_Header_TwoOptionFieldCheckbox";
            public static string OptionSetFieldContainer_Header = "Entity_Header_OptionSetFieldContainer";
            public static string LookupFieldContainer_Header = "Entity_Header_LookupFieldContainer";
            public static string DateFieldContainer_Header = "Entity_Header_DateFieldContainer";
            public static string DateFieldInput_Header = "Entity_Header_DateFieldInput";
            public static string GetLookupSearchIcon_Header = "Entity_Header_GetLookupSearchIcon";
            public static string TextFieldContainer_Footer = "Entity_Footer_TextFieldContainer";
            public static string CheckboxFieldContainer_Footer = "Entity_Footer_CheckboxFieldContainer";
            public static string TwoOptionFieldTd_Footer = "Entity_Footer_TwoOptionFieldTd";
            public static string TwoOptionFieldCheckbox_Footer = "Entity_Footer_TwoOptionFieldCheckbox";
            public static string OptionSetFieldContainer_Footer = "Entity_Footer_OptionSetFieldContainer";
            public static string LookupFieldContainer_Footer = "Entity_Footer_LookupFieldContainer";
            public static string DateFieldContainer_Footer = "Entity_Footer_DateFieldContainer";
            public static string DateFieldInput_Footer = "Entity_Footer_DateFieldInput";
            public static string GetLookupSearchIcon_Footer = "Entity_Footer_GetLookupSearchIcon";
            public static string SubGrid = "Entity_SubGrid";
            public static string SubGridHeaderCell = "Entity_SubGrid_HeaderCell";
            public static string SubGridRow = "Entity_SubGrid_Row";
            public static string SubGridCell = "Entity_SubGrid_Cell";
            public static string SubGridRowDeleteButton = "Entity_SubGrid_RowDeleteButton";
            public static string SubGridItemsTotal = "Entity_SubGrid_ItemsTotal";
        }
        public static class MenuRelated
        {
            public static string Related = "Related_Popout";
            public static string CommonActivities = "Related_Common_Activities";
        }

        public static class Timeline
        {
            public static string Popout = "Timeline_Add_Popout";
            public static string PopoutAppointment = "Timeline_Add_Popout_Appointment";
            public static string PopoutEmail = "Timeline_Add_Popout_Email";
            public static string PopoutPhoneCall = "Timeline_Add_Popout_PhoneCall";
            public static string PopoutTask = "Timeline_Add_Popout_Task";
            public static string PopoutNote = "Timeline_Add_Popout_Note";
            public static string PopoutPost = "Timeline_Add_Popout_Post";

            public static string PostText = "Timeline_Post_Text";
            public static string PostAdd = "Timeline_Post_Add";
            public static string PostCancel = "Timeline_Post_Cancel";

            public static string NoteTitle = "Timeline_Note_Title";
            public static string NoteText = "Timeline_Note_Text";
            public static string NoteAdd = "Timeline_Note_Add";
            public static string NoteCancel = "Timeline_Note_Cancel";

            public static string TaskSubject = "Timeline_Task_Subject";
            public static string TaskDescription = "Timeline_Task_Description";
            public static string TaskDuration = "Timeline_Task_Duration";
            public static string Task = "Timeline_Task";

            public static string PhoneCallSubject = "Timeline_PhoneCall_Subject";
            public static string PhoneCallNumber = "Timeline_PhoneCall_Number";
            public static string PhoneCallDescription = "Timeline_PhoneCall_Description";
            public static string PhoneCallDuration = "Timeline_PhoneCall_Duration";
            public static string PhoneCall = "Timeline_PhoneCall";

            public static string EmailSubject = "Timeline_Email_Subject";
            public static string EmailTo = "Timeline_Email_To";
            public static string EmailCC = "Timeline_Email_CC";
            public static string EmailBcc = "Timeline_Email_Bcc";
            public static string EmailDescription = "Timeline_Email_Description";
            public static string EmailDuration = "Timeline_Email_Duration";
            public static string Email = "Timeline_Email";

            public static string AppointmentSubject = "Timeline_Appointment_Subject";
            public static string AppointmentLocation = "Timeline_Appointment_Location";
            public static string AppointmentDescription = "Timeline_Appointment_Description";
            public static string AppointmentDuration = "Timeline_Appointment_Duration";
            public static string Appointment = "Timeline";
        }

        public static class GlobalSearch
        {
            public static string Filter = "Search_Filter";
            public static string SearchText = "Search_Text";
            public static string SearchButton = "Search_Button";
            public static string SearchResults = "Search_Result";
            public static string Container = "Search_Container";
            public static string EntityContainersId = "Search_EntityContainersId";
            public static string EntityNameId = "Search_EntityNameId";
            public static string RecordNameId = "Search_RecordNameId";
        }

        public static class ActivityFeed
        {
            public static string NotesControl = "Notes_NotesControl";
            public static string NotesWall = "Notes_NotesWall";
            public static string NotesText = "Notes_NotesText";
            public static string NotesDone = "Notes_Done";
            public static string PostWall = "Notes_PostWall";
            public static string PostText = "Notes_PostText";
            public static string PostButton = "Notes_PostButton";
            public static string ActivityWall = "Notes_ActivityWall";
            public static string ActivityStatusFilter = "Notes_ActivityStatusFilter";
            public static string ActivityStatusFilterDialog = "Notes_ActivityStatusFilterDialog";
            public static string ActivityStatusAll = "Notes_ActivityStatusAll";
            public static string ActivityStatusOpen = "Notes_ActivityStatusOpen";
            public static string ActivitySTatusOverdue = "Notes_ActivityStatusOverdue";
            public static string ActivityAssociatedView = "Notes_ActivityAssociatedView";
            public static string ActivityPhoneCallDescriptionId = "Notes_ActivityPhoneCallDescId";
            public static string ActivityPhoneCallVoiceMailId = "Notes_ActivityPhoneCallVoiceMailId";
            public static string ActivityPhoneCallDirectionId = "Notes_ActivityPhoneCallDirectionId";
            public static string ActivityPhoneCallOk = "Notes_ActivityPhoneCallOk";
            public static string ActivityTaskOk = "Notes_ActivityTaskOk";
            public static string ActivityTaskSubjectId = "Notes_ActivityTaskSubjectId";
            public static string ActivityTaskDescriptionId = "Notes_ActivityTaskDescriptionId";
            public static string ActivityTaskPriorityId = "Notes_ActivityTaskPriorityId";
            public static string ActivityMoreActivities = "Notes_ActivityMoreActivities";
            public static string ActivityAddEmail = "Notes_ActivityAddEmail";
            public static string ActivityAddAppointment = "Notes_ActivityAddAppointment";
            public static string ActivityAddPhoneCall = "Notes_ActivityAddPhoneCall";
            public static string ActivityAddTask = "Notes_ActivityAddTask";
            public static string VoiceMail = "Notes_VoiceMail";
            public static string ActivityTaskScheduledEnd = "Notes_ActivityTaskScheduledEnd";
            public static string ActivityAddTaskDueTime = "Notes_ActivityAddTaskDueTime";
            public static string ActivityAddTaskDueTimeId = "Notes_ActivityAddTaskDueTimeId";
            public static string ActivityAddTaskDueDateId = "Notes_ActivityAddTaskDueDateId";


        }

        public static class DashBoard
        {
            public static string NotesControl = "Notes_NotesControl";
            public static string NotesWall = "Notes_NotesWall";
            public static string Selector = "DashBoard_Selector";
            public static string ViewContainerClass = "DashBoard_ViewContainerClass";

        }

        public static class CommandBar
        {
            public static string RibbonManager = "CommandBar_RibbonManager";
            public static string List = "CommandBar_List";
            public static string MoreCommands = "CommandBar_MoreCommands";
            public static string FlyoutAnchorArrow = "CommandBar_FlyoutAnchorArrowClass";

        }
        public static class GuidedHelp
        {
            public static string MarsOverlay = "GuidedHelp_MarsOverlay";
            public static string ButBegin = "GuidedHelp_ButBegin";
            public static string ButtonClose = "GuidedHelp_ButtonClose";
            public static string Close = "GuidedHelp_Close";

        }
        public static class Notification
        {
            public static string AppMessageBar = "Notification_AppMessageBar";
            public static string Close = "Notification_Close";
            public static string MessageBarRow = "Notification_MessageBarRowClass";
            public static string MessageBarButtonContainer = "Notification_MessageBarButtonContainerClass";
            public static string MessageBarMessage = "Notification_MessageBarMessageClass";
            public static string MessageBarTitle = "Notification_MessageBarTitleClass";

        }

        public static class LookUp
        {
            public static string SelObjects = "LookUp_SelObjects";
            public static string SavedQuerySelector = "LookUp_SavedQuerySelector";
            public static string DialogCancel = "LookUp_DialogCancel";
            public static string New = "LookUp_New";
            public static string Remove = "LookUp_Remove";
            public static string Add = "LookUp_Add";
            public static string Begin = "LookUp_Begin";
            public static string Search = "LookUp_Search";

        }
        public static class Login
        {
            public static string UserId = "Login_UserId";
            public static string LoginPassword = "Login_Password";
            public static string SignIn = "Login_SignIn";
            public static string CrmMainPage = "Login_CrmMainPage";
            public static string StaySignedIn = "Login_StaySignedIn";
            public static string TaggingId = "Login_TaggingId";
        }
        public static class Report
        {
            public static string Close = "Report_Close";
            public static string RunReport = "Report_RunReport";
        }
        public static class Mobile
        {
            public static string Page = "Mobile_Page";
        }
        public static class Office365
        {
            public static string AddUser = "O365_AddUser";
            public static string FirstName = "O365_FirstName";
            public static string LastName = "O365_LastName";
            public static string DisplayName = "O365_DisplayName";
            public static string UserName = "O365_UserName";
            public static string License = "O365_License";
            public static string Add = "O365_Add";
        }
        public static class Process
        {
            public static string Name = "Process_Name";
            public static string Category = "Process_Category";
            public static string Entity = "Process_Entity";
            public static string BlankWorkflow = "Process_BlankWorkflow";
            public static string Create = "Process_ButtonCreate";
            public static string Activate = "Process_Activate";
            public static string Deactivate = "Process_Deactivate";
            public static string Delete = "Process_Delete";
            public static string Begin = "Process_Begin";
        }
    }
}
