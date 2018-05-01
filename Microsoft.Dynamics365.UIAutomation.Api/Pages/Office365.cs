// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class Office365
        : XrmPage
    {
        public Office365(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToPopup();
        }

        private static BrowserCommandOptions NavigationRetryOptions
        {
            get
            {
                return new BrowserCommandOptions(
                    Constants.DefaultTraceSource,
                    "Add User",
                    0,
                    1000,
                    null,
                    true,
                    typeof(StaleElementReferenceException));
            }
        }

        public BrowserCommandResult<bool> CreateUser(string firstname, string lastname, string displayname, string username, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(NavigationRetryOptions, driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Office365.AddUser]));

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Office365.FirstName])).SendKeys(firstname, true);
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Office365.LastName])).SendKeys(lastname, true);
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Office365.DisplayName])).SendKeys(displayname,true);
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Office365.UserName])).SendKeys(username, true);

                //Click the Microsoft Dynamics CRM Online Professional License
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Office365.License]));

                //Click Add
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Office365.Add]));

                driver.LastWindow().Close();
                driver.LastWindow();
                
            return true;
            });
        }
    }
}