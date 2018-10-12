// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PowerApps.UIAutomation.Api
{
    public static class Elements
    {
        public static Dictionary<string, string> Xpath = new Dictionary<string, string>()
        {               
            //Login           
            { "Login_UserId", "//input[@type='email']"},
            { "Login_Password", "//input[@type='password']"},
            { "Login_SignIn", "id(\"cred_sign_in_button\")"},
            { "Login_MainPage", "//nav[@id=\"D365-navbar\"]"},
            { "Login_SharePointPage", "id(\"O365_NavHeader\")"},
            { "Login_StaySignedIn", "id(\"idSIButton9\")"},

            //Canvas
            { "Canvas_Shell", "//*[@id=\"shell-root\"]" },
            { "Canvas_AppManagementPage", "//section[contains(@class, 'app-management-page')]" },
            { "Canvas_Skip", "//*[@id=\"rootBody\"]/div[1]/div[2]/div[2]/div/button" },
            { "Canvas_ColorWhite", "//button[@title=\"RGBA(255, 255, 255, 1)\"]" },
            { "Canvas_ColorBlue", "//button[@title=\"RGBA(0, 120, 215, 1)\"]" },
            { "Canvas_ExistingColor", "//div[@class=\"appmagic-borderfill-container\"]" },
            { "Canvas_ClickButton", "//button[@aria-label=\"[NAME]\"]" },

            //Navigation
            { "Navigation_HomePage", "//*[@class=\"home-page\"]" },
            { "Navigation_ChangeEnvironmentButton", "//button[contains(@class, 'groups-menu-toggle')]" },
            { "Navigation_ChangeEnvironmentList", "//div[contains(@class, 'action-menu-panel')]" },

            //Backstage
            { "Backstage_MenuContainer","//div[contains(@class, 'backstage-nav')]" },
            { "Backstage_OpenFileMenu","//button[contains(@id, 'appmagic-file-tab')]" },
            { "Backstage_Description", "//textarea[@aria-label='Description']" },

            //Dialog
            {"Permission_Dialog", "//span[@id=\"app-permissions-dialog-message\"]" },
            {"Allow_Dialog", "//button[contains(@class, 'dialog-button')]" },
            {"Override_Dialog", "//button[contains(@class, 'dialog pa__dialog overlay')]" },
            {"Override_DialogButton", "//button[contains(text(),'Override')]"},

            //Sidebar
            { "Navigation_AppLink", "//a[contains(text(),'[NAME]')]"},
            { "Navigation_AppCommandBarButton", "//span[contains(@title, '[NAME]')]"},
            { "Navigation_CanvasPageLoaded", "//div[contains(@class,'ribbon-tab-bar-parent-container')]"},
            { "Navigation_SaveButton", "//button[contains(text(), 'Save')]"},
            { "Navigation_SaveButtonSuccess", "//div[contains(text(),'All changes are saved')]"},
            { "Navigation_PublishButton", "//button[contains(@aria-label,'Publish')]"},
            { "Navigation_PublishToSharePointButton", "//button[contains(@aria-label,'Publish to SharePoint')]"},
            { "Navigation_PublishToSharePointDialogButton", "//button[contains(text(),'Publish to SharePoint')]"},
            { "Navigation_PublishButtonSuccess", "//div[contains(text(),'All changes are saved')]"},
            { "Navigation_PublishVerifyButton", "//button[contains(text(), 'Publish this version')]"},
            { "Navigation_PublishVerifyButtonSuccess", "//div[contains(text(),'All changes are saved and published')]"},
            { "Navigation_SwitchDesignMode", "//div[contains(@class, 'react-sidebar-mode-switcher')]"},
            { "Navigation_DesignModeButton", "//button[contains(@title, '[NAME]')]"},


            //SharePoint
            { "Close_App", "//button[contains(@aria-label,'Close')]/i"},
            { "Click_Button", "//button[@name=\"[NAME]\"]" },
            { "Click_SubButton", "//button[@name=\"[NAME]\"]" },

            //CommandBar
            { "CommandBar_Container","//div[contains(@class,'ba-CommandBar')]"},
            { "CommandBar_SubButtonContainer", "//ul[contains(@class,'ms-ContextualMenu')]" },
            { "CommandBar_OverflowContainer", "//div[contains(@class,'ms-OverflowSet-overflowButton')]" },
            { "CommandBar_ContextualMenuList", "//ul[contains(@class,'ms-ContextualMenu-list')]" },
            { "CommandBar_GridSolutionNameColumn", "//div[@data-automation-key='name']"},
            { "CommandBar_GridSolutionStatusColumn", "//div[contains(@data-automation-key,'solutionChecker')]"},


            //ModelDrivenApps
            { "ModelDrivenApps_CellsContainer", "//div[contains(@data-automationid,'DetailsRowCell')]"}
        };

        public static Dictionary<string, string> ElementId = new Dictionary<string, string>()
        {

        };

        public static Dictionary<string, string> CssClass = new Dictionary<string, string>()
        {

        };

        public static Dictionary<string, string> Name = new Dictionary<string, string>()
        {

        };
    }

    public static class Reference
    {
        public static class Login
        {
            public static string UserId = "Login_UserId";
            public static string LoginPassword = "Login_Password";
            public static string SignIn = "Login_SignIn";
            public static string MainPage = "Login_MainPage";
            public static string SharePointPage = "Login_SharePointPage";
            public static string StaySignedIn = "Login_StaySignedIn";
            public static int SignInAttempts = 3;
        }
        public static class Canvas
        {
            public static string CanvasMainPage = "Canvas_Shell";
            public static string AppManagementPage = "Canvas_AppManagementPage";
            public static string CanvasSkipButton = "Canvas_Skip";
            public static string ColorWhite = "Canvas_ColorWhite";
            public static string ColorBlue = "Canvas_ColorBlue";
            public static string ExistingColor = "Canvas_ExistingColor";
            public static string ClickButton = "Canvas_ClickButton";
        }
        public static class Navigation
        {
            public static string AppLink = "Navigation_AppLink";
            public static string AppCommandBarButton = "Navigation_AppCommandBarButton";
            public static string CanvasPageLoaded = "Navigation_CanvasPageLoaded";
            public static string SaveButton = "Navigation_SaveButton";
            public static string SaveButtonSuccess = "Navigation_SaveButtonSuccess";
            public static string PublishButton = "Navigation_PublishButton";
            public static string PublishToSharePointButton = "Navigation_PublishToSharePointButton";
            public static string PublishToSharePointDialogButton = "Navigation_PublishToSharePointDialogButton";
            public static string PublishVerifyButton = "Navigation_PublishVerifyButton";
            public static string PublishVerifyButtonSuccess = "Navigation_PublishVerifyButtonSuccess";
            public static string SwitchDesignMode = "Navigation_SwitchDesignMode";
            public static string DesignModeButton = "Navigation_DesignModeButton";
            public static string HomePage = "Navigation_HomePage";
            public static string ChangeEnvironmentButton = "Navigation_ChangeEnvironmentButton";
            public static string ChangeEnvironmentList = "Navigation_ChangeEnvironmentList";
            public static string CreateAnApp = "Create an app";
            public static string CreateBlankAppPhoneButtonId = "blank-app-icon-container-id-phone";
            public static string CreateAppFromTemplateButtonId = "app-from-template-icon-container-id-phone";
            public static string CreateAppFromTemplateServiceDeskButtonId = "/providers/Microsoft.PowerApps/galleries/public/items/Microsoft.ServiceDeskNew.0.2.0";
            public static string CreateAppFromTemplateChooseButtonId = "templates-choose-button-id";
            public static string CreateAppFromSPOListButtonId = "shared_sharepointonline-phone";
            public static string CreateAppFromSPOListSPURLClassName = "new-site-text-input";
            public static string CreateAppFromDataNewSiteButtonClassName = "new-site-button";
            public static string CreateAppFromDataSPConnectButtonClassName = "data-sources-pane-connect-button";
            
        }
        public static class BackStage
        {
            public static string MenuContainer = "Backstage_MenuContainer";
            public static string OpenFileMenu = "Backstage_OpenFileMenu";
            public static string Description = "Backstage_Description";
        }

        public static class Dialog
        {
            public static string PermissionsDialog = "Permission_Dialog";
            public static string AllowDialog = "Allow_Dialog";
            public static string OverrideDialog = "Override_Dialog";
            public static string OverrideDialogButton = "Override_DialogButton";
        }

        public static class SharePoint
        {
            public static string CloseApp = "Close_App";
            public static string AllowDialog = "Allow_Dialog";
            public static string ClickButton = "Click_Button";
            public static string ClickSubButton = "Click_SubButton";
        }

        public static class CommandBar
        {
            public static string Container = "CommandBar_Container";
            public static string SubButtonContainer = "CommandBar_SubButtonContainer";
            public static string OverflowContainer = "CommandBar_OverflowContainer";
            public static string ContextualMenuList = "CommandBar_ContextualMenuList";
            public static string GridSolutionNameColumn = "CommandBar_GridSolutionNameColumn";
            public static string GridSolutionStatusColumn = "CommandBar_GridSolutionStatusColumn";
        }

        public static class ModelDrivenApps
        {
            public static string CellsContainer = "ModelDrivenApps_CellsContainer";
        }

    }
}

