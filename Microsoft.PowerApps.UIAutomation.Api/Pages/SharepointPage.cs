using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.PowerApps.UIAutomation.Api
{
    public class Sharepoint : AppPage
    {

        
        public Sharepoint(InteractiveBrowser browser)
            : base(browser)
        {
            //The App opens in a new window.  Switch back to that window.
            /*
            if (browser.Driver.WindowHandles.Count > 1)
            {
                browser.Driver.LastWindow();
            }
            */
        }
        

        public BrowserCommandResult<bool> ClickLeftNavigationItem(string menuItemName)
        {
            return this.Execute(GetOptions("Sharepoint Ribbon Button: " + menuItemName), driver =>
            {
                //driver.WaitUntilVisible(By.XPath("//button[contains(@title,'[NAME]')]".Replace("[NAME]", menuItemName)));

                driver.FindElement(By.XPath("//a[contains(@title,'[NAME]')]".Replace("[NAME]", menuItemName))).Click(driver,true);
                
                return true;
            }
            );
        }
        public BrowserCommandResult<bool> ClickGridItem(string recordTitle, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions("Sharepoint Ribbon Button: " + recordTitle), driver =>
            {
                Browser.ThinkTime(thinkTime);
                
                driver.WaitUntilVisible(By.XPath("//button[contains(@title,'[NAME]')]".Replace("[NAME]", recordTitle)));

                var button = driver.FindElement(By.XPath("//button[contains(@title,'[NAME]')]".Replace("[NAME]",recordTitle)));

                button.Click(driver, true);

                return true;
            }
            );
        }

        public BrowserCommandResult<bool> CloseApp()
        {
            return this.Execute(GetOptions("Close PowerApp"), driver =>
            {

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.SharePoint.CloseApp]));

                var link = driver.FindElement(By.XPath(Elements.Xpath[Reference.SharePoint.CloseApp]));

                link.Click(driver, true);

                return true;
            }
            );
        }

        /// <summary>
        /// Open a SharePoint form app direct
        /// </summary>
        public BrowserCommandResult<bool> OpenSharePointList(string listName)
        {
            return this.Execute(GetOptions("Open SharePoint Form App"), driver =>
            {
                var uri = new Uri(this.Browser.Driver.Url);
                var link = $"{uri.Scheme}://{uri.Authority}/Lists/{listName}/AllItems.aspx";

                driver.Navigate().GoToUrl(link);

                Thread.Sleep(3000);

                return true;
            });
        }

        public BrowserCommandResult<bool> ClickButton(string name, string subName, int thinkTime = 2000)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open SharePoint Command Button"), driver =>
            {
                var button = driver.FindElement(By.XPath(Elements.Xpath[Reference.SharePoint.ClickButton].Replace("[NAME]", name)));

                if (button == null)
                {
                    throw new InvalidOperationException($"Button: { name } does not exist");
                }
                else
                {
                    button.Click(true);

                    var subButton = driver.FindElement(By.XPath(Elements.Xpath[Reference.SharePoint.ClickSubButton].Replace("[NAME]", subName)));

                    if (subButton == null)
                    {
                        throw new InvalidOperationException($"Sub Menu Button: { subName } does not exist");
                    }
                    else
                    {
                        subButton.Click(true);
                    }
                }

                Thread.Sleep(3000);

                return true;
            });

        }

    }
}
