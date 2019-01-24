// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    ///  The Xrm Navigation page.
    ///  </summary>
    public class Navigation
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Navigation"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Navigation(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToDefault();
        }

        /// <summary>
        /// Global Search
        /// </summary>
        /// <param name="searchText">The SearchText</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.GlobalSearch("Contoso");</example>
        public BrowserCommandResult<bool> GlobalSearch(string searchText, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Global Search: {searchText}"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Navigation.SearchButton]),
                    new TimeSpan(0, 0, 5),
                    d => { driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.SearchButton])); },
                    d => { throw new InvalidOperationException("The Global Search button is not available."); });


                if (driver.IsVisible(By.XPath(Elements.Xpath[Reference.Navigation.SearchLabel])))
                {
                    driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.SearchLabel]));
                }

                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Navigation.Search]),
                    new TimeSpan(0, 0, 5),
                    d =>
                    {
                        driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.Search])).SendKeys(searchText, true);
                        Thread.Sleep(500);
                        driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.Search])).SendKeys(Keys.Enter);
                    },
                    d => { throw new InvalidOperationException("The Global Search text field is not available."); });

                return true;
            });
        }

        /// <summary>
        /// Opens About from navigation bar
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenAbout();</example>
        public BrowserCommandResult<bool> OpenAbout(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open About"), driver =>
            {
                OpenSettingsOption(driver, Elements.Xpath[Reference.Navigation.About]);

                return true;
            });
        }

        /// <summary>
        /// Opens the Admin Portal
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenAdminPortal();</example>
        public BrowserCommandResult<bool> OpenAdminPortal(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Admin Portal"), driver =>
            {
                driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.AdminPortal]))?.Click();

                return true;
            });
        }

        /// <summary>
        /// Open the Advanced Find
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenAdvancedFind();</example>
        public BrowserCommandResult<bool> OpenAdvancedFind(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Advanced Find"), driver =>
            {
                //Narrow down the scope to the Search Tab when looking for the search input
                var navBar = driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.AdvFindSearch]));

                navBar.FindElement(By.ClassName("navTabButtonLink")).Click();

                return true;
            });
        }

        /// <summary>
        /// Open Apps for Dynamics
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenAppsForDynamicsCRM();</example>
        public BrowserCommandResult<bool> OpenAppsForDynamicsCRM(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Apps for Dynamics"), driver =>
            {
                OpenSettingsOption(driver, Elements.Xpath[Reference.Navigation.AppsForCRM]);

                return true;
            });
        }

        /// <summary>
        /// Opens the Guided Help
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenGuidedHelp();</example>
        public BrowserCommandResult<bool> OpenGuidedHelp(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Guided Help"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.GuidedHelp]));

                return true;
            });
        }

        public BrowserCommandResult<bool> OpenHomePage(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            //TODO: Implement HomePage logic
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens the Menu
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<Dictionary<string, IWebElement>> OpenMenu(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Menu Menu"), driver =>
            {
                var dictionary = new Dictionary<string, IWebElement>();

                var topItem = driver.FindElements(By.ClassName(Elements.CssClass[Reference.Navigation.TopLevelItem])).FirstOrDefault();
                topItem?.FindElement(By.Name(Elements.Name[Reference.Navigation.HomeTab])).Click();

                //  driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.HomeTab]));

                Thread.Sleep(1000);

                var element = driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.ActionGroup]));
                var subItems = element.FindElements(By.ClassName(Elements.CssClass[Reference.Navigation.ActionButtonContainer]));

                foreach (var subItem in subItems)
                {
                    dictionary.Add(subItem.GetAttribute("title").ToLowerString(), subItem);
                }

                return dictionary;
            });
        }

        public BrowserCommandResult<List<Link>> OpenMruMenu(int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions("Open MRU Menu"), driver =>
            {
                var list = new List<Link>();

                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.TabGlobalMruNode]));


                var navContainer = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Navigation.Shuffle]));
                var links = navContainer.FindElements(By.TagName("a"));

                foreach (var link in links)
                {
                    if (!string.IsNullOrEmpty(link.Text))
                    {
                        var newItem = new Link();

                        newItem.Uri = new Uri(link.GetAttribute("href"));
                        newItem.PageType = newItem.Uri.ToPageType();
                        newItem.LinkText = link.Text;

                        list.Add(newItem);
                    }
                }

                return list;
            });
        }

        /// <summary>
        /// Opens the Options
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenOptions();</example>
        public BrowserCommandResult<bool> OpenOptions(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Options"), driver =>
            {
                OpenSettingsOption(driver, Elements.Xpath[Reference.Navigation.Options]);

                return true;
            });
        }

        /// <summary>
        /// Opens OptOut Learning Path
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenOptOutLearningPath();</example>
        public BrowserCommandResult<bool> OpenOptOutLearningPath(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Opt out of Learning Path"), driver =>
            {
                OpenSettingsOption(driver, Elements.Xpath[Reference.Navigation.OptOutLP]);

                return true;
            });
        }

        /// <summary>
        /// Opens the Print Preview
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenPrintPreview();</example>
        public BrowserCommandResult<bool> OpenPrintPreview(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open PrintPreview"), driver =>
            {
                OpenSettingsOption(driver, Elements.Xpath[Reference.Navigation.PrintPreview]);

                return true;
            });
        }

        /// <summary>
        /// Opens the Privacy Statement
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenPrivacyStatement();</example>
        public BrowserCommandResult<bool> OpenPrivacyStatement(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Privacy Statement"), driver =>
            {
                OpenSettingsOption(driver, Elements.Xpath[Reference.Navigation.Privacy]);

                return true;
            });
        }

        /// <summary>
        /// Opens the Related Menu
        /// </summary>
        /// <param name="relatedArea">The Related area</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenRelated("Cases");</example>
        public BrowserCommandResult<bool> OpenRelated(string relatedArea, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Related Menu"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.TabNode]));

                var element = driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.ActionGroup]));
                var subItems = element.FindElements(By.ClassName("nav-rowBody"));

                var related = subItems.Where(x => x.Text == relatedArea).FirstOrDefault();
                if (related == null)
                {
                    throw new InvalidOperationException($"No relatedarea with the name '{relatedArea}' exists.");
                }

                Browser.ActiveFrameId = related.GetAttribute("id").Replace("Node_nav", "area");
                related?.Click();

                return true;
            });
        }

        private static void OpenSettingsOption(IWebDriver driver, string settingPath)
        {
            driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.Settings]));
            Thread.Sleep(1000);

            //Bug: 563823 
            //Issue with CRM Style where the Options menu is not visible in IE. Works in Chrome but going to set the style to Display: Block so that we can click it.  
            driver.SetVisible(By.XPath(settingPath), true);
            //End bug fix

            driver.WaitUntilVisible(By.XPath(settingPath));
            driver.ClickWhenAvailable(By.XPath(settingPath));
        }

        /// <summary>
        /// Opens the Sub Area
        /// </summary>
        /// <param name="area">The area you want to open</param>
        /// <param name="subArea">The subarea you want to open</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenSubArea("Sales", "Opportunities");</example>
        public BrowserCommandResult<bool> OpenSubArea(string area, string subArea, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($": {area} > {subArea}"), driver =>
            {
                area = area.ToLower();
                subArea = subArea.ToLower();

                var areas = OpenMenu().Value;

                if (!areas.ContainsKey(area))
                {
                    throw new InvalidOperationException($"No area with the name '{area}' exists.");
                }

                var subAreas = OpenSubMenu(areas[area]).Value;

                if (!subAreas.Any(x => x.Key==subArea))
                {
                    throw new InvalidOperationException($"No subarea with the name '{subArea}' exists inside of '{area}'.");
                }

                subAreas.FirstOrDefault(x => x.Key==subArea).Value.Click(true);

                SwitchToContent();
                driver.WaitForPageToLoad();

                return true;
            });
        }

        internal BrowserCommandResult<List<KeyValuePair<string,IWebElement>>> OpenSubMenu(IWebElement area)
        {
            return this.Execute(GetOptions($"Open Sub Menu: {area}"), driver =>
            {
                var list = new List<KeyValuePair<string, IWebElement>>();

                driver.WaitUntilVisible(By.Id(area.GetAttribute("Id")));

                area.Click();

                Thread.Sleep(1000);

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Navigation.SubActionGroupContainer]));

                var subNavElement = driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.SubActionGroupContainer]));

                var subItems = subNavElement.FindElements(By.ClassName(Elements.CssClass[Reference.Navigation.SubActionElementClass]));

                foreach (var subItem in subItems)
                {
                    if (!string.IsNullOrEmpty(subItem.Text))
                        list.Add(new KeyValuePair<string, IWebElement>(subItem.Text.ToLowerString(), subItem));
                }

                return list;
            });
        }

        /// <summary>
        /// Opens Welcome Screen from navigation bar
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenWelcomeScreen();</example>
        public BrowserCommandResult<bool> OpenWelcomeScreen(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Welcome Screen"), driver =>
            {
                OpenSettingsOption(driver, Elements.Xpath[Reference.Navigation.WelcomeScreen]);

                return true;
            });
        }

        /// <summary>
        /// Open Quick Create
        /// </summary>
        /// <param name="entity">The entity name</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.QuickCreate("Account");</example>
        public BrowserCommandResult<bool> QuickCreate(string entity, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Quick Create"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Navigation.GlobalCreate]));

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.QuickCreate.EntityContainer]), new TimeSpan(0, 0, 2));

                var area = driver.FindElement(By.XPath(Elements.Xpath[Reference.QuickCreate.EntityContainer]));
                var items = area.FindElements(By.TagName("a"));

                var item = items.FirstOrDefault(x => x.Text == entity);

                if (item == null)
                {
                    throw new InvalidOperationException($"No Entity with the name '{entity}' exists inside QuickCreate.");
                }

                item.Click(true);

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.QuickCreate.Container]));

                return true;
            });
        }

        /// <summary>
        /// SignOut
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.SignOut();</example>
        public BrowserCommandResult<bool> SignOut(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"SignOut"), driver =>
            {
                var userInfo = driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.UserInfo]));
                userInfo?.Click();
                var signOut = driver.FindElement(By.XPath(Elements.Xpath[Reference.Navigation.SignOut]));
                signOut?.Click();
                return true;
            });
        }
    }
}