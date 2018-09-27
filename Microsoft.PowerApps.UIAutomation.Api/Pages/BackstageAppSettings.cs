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
    public class BackStageAppSettings
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Backstage"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public BackStageAppSettings(InteractiveBrowser browser)
            : base(browser)
        {
        }

        public BrowserCommandResult<bool> UpdateDescription(String descriptionText, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Update App Description"), driver =>
            {
                //Find the container with the button
                var textArea = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.BackStage.Description]),
                                                          "The description field is not avialable");

                textArea.Click();
                textArea.SendKeys($"Updating Description: {DateTime.Now.ToLongTimeString()}",true);

                return true;
            });
        }

        public BrowserCommandResult<bool> Save(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Save App"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.SaveButton]));

                //Wait until the response is visible
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Navigation.SaveButtonSuccess]),
                    new TimeSpan(0, 0, 20),
                    null,
                    e => { throw new Exception("Click Save Button failed"); }
                    );
                return true;
            });
        }

        public BrowserCommandResult<bool> Publish(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Publish App"), driver =>
            {
                //Find the container with the button
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.PublishButton]));

                //Find the container with the button
                var menuContainer = driver.FindElement(By.XPath("//div[contains(@class, 'backstage-nav')]"));
                //Elements.Xpath[Reference.BackStage.MenuContainer]

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.PublishVerifyButton]));
                

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Navigation.PublishVerifyButtonSuccess]),
                    new TimeSpan(0, 0, 20),
                    null,
                    e => { throw new Exception("Click Publish/Verify Button failed"); }
                    );

                return true;
            });
        }

        public BrowserCommandResult<bool> PublishToSharePoint(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Publish To SharePoint"), driver =>
            {
                //Find the container with the button
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.PublishToSharePointButton]));

                //Interact with the Publish to SharePoint Dialog
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.PublishToSharePointDialogButton]));


                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Navigation.PublishVerifyButtonSuccess]),
                    new TimeSpan(0, 0, 20),
                    null,
                    e => { throw new Exception("Click Publish/Verify Button failed"); }
                    );

                return true;
            });
        }
    }
}
