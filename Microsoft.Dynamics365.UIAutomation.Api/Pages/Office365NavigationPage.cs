// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class Office365NavigationPage
        : XrmPage
    {
        public Office365NavigationPage(InteractiveBrowser browser)
            : base(browser)
        {
        }

        private static BrowserCommandOptions NavigationRetryOptions
        {
            get
            {
                return new BrowserCommandOptions(
                    Constants.DefaultTraceSource,
                    "Open Waffle Menu",
                    5,
                    1000,
                    null,
                    false,
                    typeof(StaleElementReferenceException));
            }
        }

        public BrowserCommandResult<Dictionary<string, Uri>> OpenWaffleMenu()
        {
            return this.Execute(NavigationRetryOptions, driver =>
            {
                var dictionary = new Dictionary<string, Uri>();

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Office365Navigation.NavMenu]));

                var element = driver.FindElement(By.ClassName(Elements.CssClass[Reference.Office365Navigation.MenuTabContainer]));
                var subItems = element.FindElements(By.ClassName(Elements.CssClass[Reference.Office365Navigation.AppItem]));

                foreach (var subItem in subItems)
                {
                    var link = subItem.FindElement(By.TagName("a"));

                    if (link != null)
                    {
                        dictionary.Add(link.Text, new Uri(link.GetAttribute("href")));
                    }
                }

                return dictionary;
            });
        }
    }
}