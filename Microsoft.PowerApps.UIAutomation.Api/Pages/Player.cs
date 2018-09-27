// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  The Home page.
    ///  </summary>
    public class Player
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Player(InteractiveBrowser browser)
            : base(browser)
        {
            //The App opens in a new window.  Switch back to that window.
            browser.Driver.LastWindow();
        }
        
        /// <summary>
        /// Hide the Studio Welcome Dialog
        /// </summary>
        public BrowserCommandResult<bool> OpenApp(string id, Dictionary<string,string> parameters = null, bool openInNewTab = false)
        {
            return this.Execute(GetOptions("Open App By ID"), driver =>
            {
                var uri = new Uri(this.Browser.Driver.Url);
                var link = $"{uri.Scheme}://{uri.Authority}/webplayer/app?appId=/providers/Microsoft.PowerApps/apps/{id}";

                foreach (var param in parameters)
                {
                    link += $"&{param.Key}={param.Value}";
                }

                if (openInNewTab)
                {
                    driver.OpenNewTab();

                    Thread.Sleep(1000);
                    driver.LastWindow();

                    Thread.Sleep(1000);

                    driver.Navigate().GoToUrl(link);
                }
                else
                {
                    driver.Navigate().GoToUrl(link);
                }

                driver.WaitForPlayerToLoad();
                
                return true;
            });
        }
    }
}