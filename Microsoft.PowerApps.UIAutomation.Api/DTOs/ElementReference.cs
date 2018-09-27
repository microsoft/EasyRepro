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
            { "Canvas_AppManagementPageTIP", "//section[contains(@class, 'app-management-page')]" },
            { "Canvas_AppManagementPagePreview", "//section[contains(@class, 'app-management-page')]" },
            { "Canvas_Skip", "//*[@id=\"rootBody\"]/div[1]/div[2]/div[2]/div/button" },
            { "Canvas_ColorWhite", "//button[@title=\"RGBA(255, 255, 255, 1)\"]" },
            { "Canvas_ColorBlue", "//button[@title=\"RGBA(0, 120, 215, 1)\"]" },
            { "Canvas_ExistingColor", "//div[@class=\"appmagic-borderfill-container\"]" },
            { "Canvas_ClickButton", "//button[@aria-label=\"[NAME]\"]" },

            //Navigation
            { "Navigation_HomePage", "//*[@class=\"home-page\"]" },
            { "Navigation_TIPApps", "//button[@title=\"Apps\"]" },
            { "Navigation_TIP_SPConnectedList", "//h3[@title=\"SPConnectedList\"]"},

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

            //SharePoint
            { "Close_App", "//button[contains(@aria-label,'Close')]/i"},
            { "Click_Button", "//button[@name=\"[NAME]\"]" },
            { "Click_SubButton", "//button[@name=\"[NAME]\"]" },

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
            public static string AppManagementPageTIP = "Canvas_AppManagementPageTIP";
            public static string AppManagementPagePreview = "Canvas_AppManagementPagePreview";
            public static string CanvasSkipButton = "Canvas_Skip";
            public static string ColorWhite = "Canvas_ColorWhite";
            public static string ColorBlue = "Canvas_ColorBlue";
            public static string ExistingColor = "Canvas_ExistingColor";
            public static string ClickButton = "Canvas_ClickButton";
        }
        public static class Navigation
        {
            public static string AppLink = "Navigation_AppLink";
            public static string TIPApps = "Navigation_TIPApps";
            public static string AppCommandBarButton = "Navigation_AppCommandBarButton";
            public static string CanvasPageLoaded = "Navigation_CanvasPageLoaded";
            public static string SaveButton = "Navigation_SaveButton";
            public static string SaveButtonSuccess = "Navigation_SaveButtonSuccess";
            public static string PublishButton = "Navigation_PublishButton";
            public static string PublishToSharePointButton = "Navigation_PublishToSharePointButton";
            public static string PublishToSharePointDialogButton = "Navigation_PublishToSharePointDialogButton";
            public static string PublishVerifyButton = "Navigation_PublishVerifyButton";
            public static string PublishVerifyButtonSuccess = "Navigation_PublishVerifyButtonSuccess";
            public static string HomePage = "Navigation_HomePage";
            public static string CreateAnApp = "Create an app";
            public static string CreateBlankAppPhoneButtonId = "blank-app-icon-container-id-phone";
            public static string CreateAppFromTemplateButtonId = "app-from-template-icon-container-id-phone";
            public static string CreateAppFromTemplateServiceDeskButtonId = "/providers/Microsoft.PowerApps/galleries/public/items/Microsoft.ServiceDeskNew.0.2.0";
            public static string CreateAppFromTemplateChooseButtonId = "templates-choose-button-id";

            public static string CreateAppFromSPOListButtonId = "shared_sharepointonline-phone";
            public static string CreateAppFromSPOListSPURLClassName = "new-site-text-input";
            public static string CreateAppFromDataNewSiteButtonClassName = "new-site-button";
            public static string CreateAppFromDataSPConnectedList = "Navigation_TIP_SPConnectedList";
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

    }
}
