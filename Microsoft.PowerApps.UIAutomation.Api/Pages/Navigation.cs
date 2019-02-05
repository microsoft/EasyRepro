// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  The Sidebar page.
    ///  </summary>
    public class Navigation
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Navigation"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Navigation(InteractiveBrowser browser)
            : base(browser)
        {
        }

        public BrowserCommandResult<bool> Homepage()
        {
            return this.Execute(GetOptions("Home Page"), driver =>
            {
                var uri = new Uri(this.Browser.Driver.Url);
                var link = $"{uri.Scheme}://{uri.Authority}/home";
                
                driver.Navigate().GoToUrl(link);

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Navigation.HomePage])
                    , new TimeSpan(0, 2, 0),
                    e => { e.WaitForPageToLoad(); },
                    f => { throw new Exception("Home page load failed."); });

                return true;
            });
        }

        public BrowserCommandResult<string> ChangeEnvironment(string environmentName)
        {
            return this.Execute(GetOptions($"Change Environment to: {environmentName}"), driver =>
            {
                var chosenEnvironment = "";
                var environmentButton = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Navigation.ChangeEnvironmentButton]));
                var environmentButtonName = environmentButton.FindElements(By.TagName("span"));
                chosenEnvironment = environmentButtonName[1].Text;

                if (chosenEnvironment == environmentName)
                {
                    return chosenEnvironment;
                }
                else
                {
                    environmentButton.Click(true);

                    var environments = driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.ChangeEnvironmentList]));
                    var environmentsList = environments.FindElements(By.TagName("li"));


                    if (environmentsList != null)
                    {
                        var environmentFound = false;

                        foreach (var environmentListItem in environmentsList)
                        {
                            var titleLinks = environmentListItem.FindElements(By.XPath(".//div/div"));

                            if (titleLinks != null && titleLinks.Count > 0)
                            {
                                var title = titleLinks[0].GetAttribute("innerText");

                                if (title.ToLower().Contains(environmentName.ToLower()))
                                {
                                    environmentListItem.Click(true);
                                    environmentFound = true;                                    
                                }
                            }
                        }

                        if (!environmentFound)
                            throw new InvalidOperationException($"Environment {environmentName} does not exist in the list of environments. Please verify the environment exists, or that the provided name is correct and try again.");
                    }
                    else
                    {
                        throw new InvalidOperationException($"Environment List contains no values. Please create an environment via the PowerApps Admin Center.");
                    }

                    environmentButtonName = environmentButton.FindElements(By.TagName("span"));
                    chosenEnvironment = environmentButtonName[1].Text;

                    return chosenEnvironment;
                }
            });
        }

    }
}
