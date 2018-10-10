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

                var url = driver.Url;
              
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Canvas.AppManagementPage]),
                    new TimeSpan(0, 0, 20),
                    null,
                    e => { throw new Exception("Unable to Launch App"); }
                    );

                return true;
               
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

                driver.ClickWhenAvailable(By.XPath("//button[@name=\"Edit\"]"));
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
            Browser.Driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.AppCommandBarButton].Replace("[NAME]", buttonName)));
        }
    }
}
