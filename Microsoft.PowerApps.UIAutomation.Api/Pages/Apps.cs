// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  The Home page.
    ///  </summary>
    public class Apps
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Apps(InteractiveBrowser browser)
            : base(browser)
        {
        }

        public BrowserCommandResult<bool> OpenApp(String appName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("LaunchApp: " + appName), driver =>
            {
                //Find the container with the list of apps
                var container = driver.FindElement(By.ClassName("apps-list"));

                //Find the apps on the home page
                container.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.AppLink].Replace("[NAME]", appName))).Click();

                //Wait for the main page to render
                //driver.LastWindow();

                var url = driver.Url;

                if (url.Contains("tip1"))
                {
                    driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Canvas.AppManagementPageTIP]),
                        new TimeSpan(0, 0, 20),
                        null,
                        e => { throw new Exception("Unable to Launch App"); }
                        );

                    return true;
                }
                else
                {
                    driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Canvas.AppManagementPagePreview]),
                        new TimeSpan(0, 0, 20),
                        null,
                        e => { throw new Exception("Unable to Launch App"); }
                        );

                    return true;
                }
            });
        }

        public BrowserCommandResult<bool> CreateBlankApp(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("CreateBlankApp"), driver =>
            {
                //click the create new app button. 
                driver.FindElement(By.Name(Reference.Navigation.CreateAnApp)).Click();
                //switch to the new window 
                driver.LastWindow();
                //wait for the create blank phone app button. 
                driver.WaitUntilVisible(By.Id(Reference.Navigation.CreateBlankAppPhoneButtonId));
                //click on it. 
                driver.WaitUntilClickable(By.Id(Reference.Navigation.CreateBlankAppPhoneButtonId));
                Browser.ThinkTime(thinkTime);
                driver.FindElement(By.Id(Reference.Navigation.CreateBlankAppPhoneButtonId)).Click();
                return true;
            });
        }
        public BrowserCommandResult<bool> CreateAppFromData(String sPOnlineURL, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("CreateBlankApp"), driver =>
            {
                //click the create new app button. 
                driver.FindElement(By.Name(Reference.Navigation.CreateAnApp)).Click();
                //switch to the new window 
                driver.LastWindow();
                //wait for the create blank phone app button. 
                driver.WaitUntilVisible(By.Id(Reference.Navigation.CreateAppFromSPOListButtonId));
                //click on it. 
                driver.WaitUntilClickable(By.Id(Reference.Navigation.CreateAppFromSPOListButtonId));
                Browser.ThinkTime(thinkTime);
                driver.FindElement(By.Id(Reference.Navigation.CreateAppFromSPOListButtonId)).Click();
                
                //sharepoint site url
                driver.WaitUntilVisible(By.ClassName(Reference.Navigation.CreateAppFromSPOListSPURLClassName));
                var textArea  = driver.WaitUntilAvailable(By.ClassName(Reference.Navigation.CreateAppFromSPOListSPURLClassName));
                textArea.Click();
                textArea.SendKeys(sPOnlineURL);

                driver.WaitUntilVisible(By.ClassName(Reference.Navigation.CreateAppFromDataNewSiteButtonClassName));
                driver.FindElement(By.ClassName(Reference.Navigation.CreateAppFromDataNewSiteButtonClassName)).Click();

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Navigation.CreateAppFromDataSPConnectedList]));
                driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.CreateAppFromDataSPConnectedList])).Click();
                
                driver.WaitUntilVisible(By.ClassName(Reference.Navigation.CreateAppFromDataSPConnectButtonClassName));
                driver.FindElement(By.ClassName(Reference.Navigation.CreateAppFromDataSPConnectButtonClassName)).Click();
                return true;
            });
        }

        public BrowserCommandResult<bool> CreateAppFromTemplate(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("CreateBlankApp"), driver =>
            {
                //click the create new app button. 
                driver.WaitUntilVisible(By.Name(Reference.Navigation.CreateAnApp));
                //click on it. 
                driver.WaitUntilClickable(By.Name(Reference.Navigation.CreateAnApp));
                driver.FindElement(By.Name(Reference.Navigation.CreateAnApp)).Click();
                //switch to the new window 
                driver.LastWindow();

                //wait for the create blank phone app button. 
                driver.WaitUntilVisible(By.Id(Reference.Navigation.CreateAppFromTemplateButtonId));
                //click on it. 
                driver.WaitUntilClickable(By.Id(Reference.Navigation.CreateAppFromTemplateButtonId));
                Browser.ThinkTime(thinkTime);
                driver.FindElement(By.Id(Reference.Navigation.CreateAppFromTemplateButtonId)).Click();

                //Browser.ThinkTime(thinkTime);
                driver.WaitUntilClickable(By.Id(Reference.Navigation.CreateAppFromTemplateServiceDeskButtonId));
                driver.WaitUntilVisible(By.Id(Reference.Navigation.CreateAppFromTemplateServiceDeskButtonId));
                driver.FindElement(By.Id(Reference.Navigation.CreateAppFromTemplateServiceDeskButtonId)).Click();

                //Browser.ThinkTime(thinkTime);
                driver.WaitUntilVisible(By.Id(Reference.Navigation.CreateAppFromTemplateChooseButtonId));
                driver.FindElement(By.Id(Reference.Navigation.CreateAppFromTemplateChooseButtonId)).Click();
                
                return true;
            });
        }

        public BrowserCommandResult<bool> OpenSharePointAppStudio(String appName, string appId, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Launch SharePoint App Studio: " + appName), driver =>
            {

                var uri = new Uri(this.Browser.Driver.Url);
                var link = $"{uri.Scheme}://{uri.Authority}/#action=sp-edit-formdata&app-id={appId}";

                try
                {
                    driver.Navigate().GoToUrl(link);
                }
                catch (UnhandledAlertException)
                {
                    var modalDialog = driver.WaitUntilVisible(By.XPath(Reference.Dialog.OverrideDialog), new TimeSpan(0, 0, 3));

                    if (modalDialog)
                    {
                        driver.ClickWhenAvailable(By.XPath(Reference.Dialog.OverrideDialogButton));
                    }

                    return true;
                }
                //Wait for the main page to render
                //driver.LastWindow();
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Canvas.CanvasMainPage]),
                    new TimeSpan(0, 0, 20),
                    null,
                    e => { throw new Exception("Unable to Launch App"); }
                    );

                return true;
            });
        }

        public BrowserCommandResult<bool> SharePointPermissionDialog(String appName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Allow SharePoint Access For: " + appName), driver =>
            {

                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialog.PermissionsDialog])
                                                , new TimeSpan(0, 0, 0, 30)
                                                , d =>
                                                {
                                                    var buttons = driver.FindElements(By.XPath(Elements.Xpath[Reference.Dialog.AllowDialog]));

                                                        foreach (var button in buttons)
                                                        {
                                                            if (button.Text == "Allow")
                                                                button.Click(true);

                                                        }
                                                });
                return true;
            });
        }

        public BrowserCommandResult<bool> Edit(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Edit App"), driver =>
            {

                var url = driver.Url;

                if (url.Contains("tip1"))
                {
                    driver.ClickWhenAvailable(By.XPath("//button[@name=\"Edit\"]"));
                }
                else //Else Scenario for when TIP1 is different
                {
                    driver.ClickWhenAvailable(By.XPath("//button[@name=\"Edit\"]"));
                }
                

                /*
                ClickButtonInAppHeader("Edit");

                driver.LastWindow();
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Navigation.CanvasPageLoaded]),
                    new TimeSpan(0, 0, 40),
                    null,
                    e => { throw new Exception(String.Format("Click Edit button failed")); }
                    );

                 */

                driver.LastWindow();
                return true;
            });
        }

        public BrowserCommandResult<bool> Play(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Play App"), driver =>
            {
                ClickButtonInAppHeader("Play");

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Navigation.CanvasPageLoaded]),
                    new TimeSpan(0, 0, 40),
                    null,
                    e => { throw new Exception(String.Format("Click Play button failed")); }
                    );

                return true;
            });
        }

        public BrowserCommandResult<bool> Delete(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Delete App"), driver =>
            {
                ClickButtonInAppHeader("Delete");

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Navigation.CanvasPageLoaded]),
                    new TimeSpan(0, 0, 40),
                    null,
                    e => { throw new Exception(String.Format("Click Delete button failed")); }
                    );

                return true;
            });
        }

        internal void ClickButtonInAppHeader(String buttonName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            //Find the container with the button
            //var container = Browser.Driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.AppCommandBarButton].Replace("[NAME]", buttonName)));

            Browser.Driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.AppCommandBarButton].Replace("[NAME]", buttonName)));

            //Click the svg
            //container.FindElement(By.TagName("svg")).Click();
        }
    }
}
