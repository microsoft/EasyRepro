// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    ///  Xrm Entity page.
    ///  </summary>
    public class Entity
        : XrmPage 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>              
        public Entity(InteractiveBrowser browser)
            : base(browser)
        {
            SwitchToContent();
        }

        private readonly string _navigateDownCssSelector = "img.recnav-down.ms-crm-ImageStrip-Down_Enabled_proxy";
        private readonly string _navigateUpCssSelector = "img.recnav-up.ms-crm-ImageStrip-Up_Enabled_proxy";

        /// <summary>
        /// Opens the Entity
        /// </summary>
        /// <param name="entityName">The Entity Name you want to open</param>
        /// <param name="id">The Guid</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.OpenEntity(TestSettings.AccountLogicalName, Guid.Parse(TestSettings.AccountId));</example>
        public BrowserCommandResult<bool> OpenEntity(string entityName, Guid id, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open: {entityName} {id}"), driver =>
            {
                var uri = new Uri(this.Browser.Driver.Url);
                var link = $"{uri.Scheme}://{uri.Authority}/main.aspx?etn={entityName}&pagetype=entityrecord&id=%7B{id:D}%7D";
                return OpenEntity(new Uri(link)).Value;
            });
        }

        /// <summary>
        /// Opens the Entity
        /// </summary>
        /// <param name="uri">The uri</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> OpenEntity(Uri uri, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open: {uri}"), driver =>
            {

                driver.Navigate().GoToUrl(uri);

                DismissAlertIfPresent();

                // driver.WaitFor(d => d.ExecuteScript(XrmPerformanceCenterPage.GetAllMarkersJavascriptCommand).ToString().Contains("AllSubgridsLoaded"));
                SwitchToContent();
                driver.WaitForPageToLoad();
                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Entity.Form]),
                                            new TimeSpan(0, 0, 30),
                                            null,
                                            d => { throw new Exception("CRM Record is Unavailable or not finished loading. Timeout Exceeded"); }
                                        );

                return true;
            });
        }

        /// <summary>
        /// Navigate Down the record
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.NavigateDown();</example>
        public BrowserCommandResult<bool> NavigateDown(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Navigate Down"), driver =>
            {
                SwitchToDefault();
                if (!driver.HasElement(By.CssSelector(_navigateDownCssSelector)))
                    return false;

                var buttons = driver.FindElements(By.CssSelector(_navigateDownCssSelector));

                buttons[0].Click();

                //driver.WaitFor(d => d.ExecuteScript(XrmPerformanceCenterPage.GetAllMarkersJavascriptCommand).ToString().Contains("AllSubgridsLoaded"));
                driver.WaitForPageToLoad();

                return true;
            });
        }

        /// <summary>
        /// Navigate Up the record
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.NavigateUp();</example>
        public BrowserCommandResult<bool> NavigateUp(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Navigate Up"), driver =>
            {
                SwitchToDefault();
                if (!driver.HasElement(By.CssSelector(_navigateUpCssSelector)))
                    return false;

                var buttons = driver.FindElements(By.CssSelector(_navigateUpCssSelector));

                buttons[0].Click();

                //driver.WaitFor(d => d.ExecuteScript(XrmPerformanceCenterPage.GetAllMarkersJavascriptCommand).ToString().Contains("AllSubgridsLoaded"));
                driver.WaitForPageToLoad();
                
                return true;
            });
        }

        /// <summary>
        /// Selects the Form
        /// </summary>
        /// <param name="name">The name of the form</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.SelectForm("Details");</example>
        public BrowserCommandResult<bool> SelectForm(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"SelectForm: {name}"), driver =>
            { 
                var form = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.SelectForm]) ,"The Select Form option is not available.");
                form.Click();

                var items = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.SelectFormContentTable])).FindElements(By.TagName("a"));
                items.Where(x => x.Text == name).FirstOrDefault()?.Click();

                return true;

            });
        }

        /// <summary>
        /// Selects the Form Section
        /// </summary>
        /// <param name="name">The name of the form section</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.SelectFormSection("Details");</example>
        public BrowserCommandResult<bool> SelectFormSection(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"SelectFormSection: {name}"), driver =>
            {
                var form = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.SelectFormSection]), "The Select Form Section option is not available.");
                form.Click();

                var items = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.FormSectionContentTable])).FindElements(By.TagName("a"));
                items.Where(x => x.Text == name).FirstOrDefault()?.Click();

                return true;

            });
        }

        /// <summary>
        /// Selects the tab and clicks. If the tab is expanded it will collapse it. If the tab is collapsed it will expand it. 
        /// </summary>
        /// <param name="name">The name of the tab.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.SelectTab("Details");</example>
        public BrowserCommandResult<bool> SelectTab(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"SelectTab: {name}"), driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name))))
                {
                    throw new InvalidOperationException($"Tab with name '{name}' does not exist.");
                }
                var tab = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name)));

                tab?.Click();

                return true;
            });
        }

        /// <summary>
        /// Collapses the Tab on a CRM Entity form.
        /// </summary>
        /// <param name="name">The name of the Tab.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.CollapseTab("Summary");</example>
        public BrowserCommandResult<bool> CollapseTab(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Collapse Tab: {name}"), driver =>
            {
                if(!driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name))))
                {
                    throw new InvalidOperationException($"Tab with name '{name}' does not exist.");
                }
                var tab = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]",name)));

                if (tab.GetAttribute("title").Contains("Collapse"))
                    tab?.Click();

                return true;
            });
        }

        /// <summary>
        /// Expands the Tab on a CRM Entity form.
        /// </summary>
        /// <param name="name">The name of the Tab.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.ExpandTab("Summary");</example>
        public BrowserCommandResult<bool> ExpandTab(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Expand Tab: {name}"), driver =>
            {
                if (!driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name))))
                {
                    throw new InvalidOperationException($"Tab with name '{name}' does not exist.");
                }
                var tab = driver.FindElement(By.XPath(Elements.Xpath[Reference.Entity.Tab].Replace("[NAME]", name)));

                if (tab.GetAttribute("title").Contains("Expand"))
                    tab?.Click();

                return true;
            });
        }

        /// <summary>
        /// Popout the form
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.Popout();</example>
        public BrowserCommandResult<bool> Popout(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Popout"), driver =>
            {
                SwitchToDefault();
                driver.ClickWhenAvailable(By.ClassName(Elements.CssClass[Reference.Entity.Popout]));

                return true;
            });
        }

        /// <summary>
        /// Click add button of subgridName
        /// </summary>
        /// <param name="subgridName">The SubgridName</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.ClickSubgridAddButton("Stakeholders");</example>
        public BrowserCommandResult<bool> ClickSubgridAddButton(string subgridName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Click add button of {subgridName}"), driver =>
            {
                driver.FindElement(By.Id($"{subgridName}_addImageButton"))?.Click();

                return true;
            });
        }

        /// <summary>
        /// Click GridView button of subgridName
        /// </summary>
        /// <param name="subgridName">The subgridName</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> ClickSubgridGridViewButton(string subgridName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Click GridView button of {subgridName}"), driver =>
            {
                driver.FindElement(By.Id($"{subgridName}_openAssociatedGridViewImageButton"))?.Click();

                return true;
            });
        }

        /// <summary>
        /// Set Lookup Value for Subgrid subgridName
        /// </summary>
        /// <param name="subgridName">The SubgridName</param>
        /// <param name="value"></param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", "Alex Wu");</example>
        public BrowserCommandResult<bool> SelectSubgridLookup(string subgridName, string value, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Set Lookup Value for Subgrid {subgridName}"), driver =>
            {
                if (driver.HasElement(By.Id($"inlineLookupControlForSubgrid_{subgridName}")))
                {
                    var input = driver.ClickWhenAvailable(By.Id($"inlineLookupControlForSubgrid_{subgridName}"));
                    
                    var lookupIcon = input.FindElement(By.ClassName(Elements.CssClass[Reference.Entity.LookupRender]));
                    lookupIcon.Click();

                    var dialogName = $"Dialog_lookup_{subgridName}_i_IMenu";
                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if (!dialogItems.Exists(x => x.Title == value))
                        throw new InvalidOperationException($"List does not have {value}.");

                    var dialogItem = dialogItems.Where(x => x.Title == value).First();
                    dialogItem.Element.Click();
                }

                return true;
            });
        }

        /// <summary>
        /// Set Lookup Value for Subgrid
        /// </summary>
        /// <param name="subgridName">The subgridName</param>
        /// <param name="index">The Index</param>
        /// <example>xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", 3);</example>
        public BrowserCommandResult<bool> SelectSubgridLookup(string subgridName, [Range(0, 9)]int index)
        {
            return this.Execute(GetOptions($"Set Lookup Value for Subgrid {subgridName}"), driver =>
            {
                if (driver.HasElement(By.Id($"inlineLookupControlForSubgrid_{subgridName}")))
                {
                    var input = driver.ClickWhenAvailable(By.Id($"inlineLookupControlForSubgrid_{subgridName}"));

                    input.FindElement(By.ClassName(Elements.CssClass[Reference.Entity.LookupRender])).Click();

                    var dialogName = $"Dialog_lookup_{subgridName}_i_IMenu";
                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    if (dialogItems.Count < index)
                        throw new InvalidOperationException($"List does not have {index + 1} items.");

                    var dialogItem = dialogItems[index];
                    dialogItem.Element.Click();
                }

                return true;
            });
        }

        /// <summary>
        /// Set Lookup Value for Subgrid
        /// </summary>
        /// <param name="subgridName">The SubgridName</param>
        /// <param name="openLookupPage"></param>
        /// <example>xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", true);</example>
        public BrowserCommandResult<bool> SelectSubgridLookup(string subgridName, bool openLookupPage = true)
        {
            return this.Execute(GetOptions($"Set Lookup Value for Subgrid {subgridName}"), driver =>
            {
                if (driver.HasElement(By.Id($"inlineLookupControlForSubgrid_{subgridName}")))
                {
                    var input = driver.ClickWhenAvailable(By.Id($"inlineLookupControlForSubgrid_{subgridName}"));
                    
                    input.FindElement(By.ClassName(Elements.CssClass[Reference.Entity.LookupRender])).Click();

                    var dialogName = $"Dialog_lookup_{subgridName}_i_IMenu";

                    driver.WaitUntilVisible(By.Id(dialogName), new TimeSpan(0, 0, 2));

                    var dialog = driver.FindElement(By.Id(dialogName));

                    var dialogItems = OpenDialog(dialog).Value;

                    var dialogItem = dialogItems.Last();
                    
                    if (this.Browser.Options.BrowserType == BrowserType.Firefox)
                    {
                        var id = dialog.FindElements(By.TagName("li")).Last().GetAttribute("id");
                       
                        driver.ExecuteScript($"document.getElementById('{id}').childNodes[1].click();");
                    }
                    else
                        dialogItem.Element.Click();
                }

                return true;
            });
        }

        /// <summary>
        /// Closes the current entity record you are on.
        /// </summary>
        /// <example>xrmBrowser.Entity.CloseEntity();</example>
        public BrowserCommandResult<bool> CloseEntity(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Close Entity"), driver =>
            {
                SwitchToDefault();

                var filter = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.Close]),
                    "Close Buttton is not available");

                filter?.Click();

                return true;
            });
        }

        /// <summary>
        /// Saves the specified entity record.
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.Save();</example>
        public BrowserCommandResult<bool> Save(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Save Entity"), driver =>
            {
                var save = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.Save]),
                    "Save Buttton is not available");

                save?.Click();

                return true;
            });
        }

        /// <summary>
        /// Dismiss the Alert If Present
        /// </summary>
        /// <param name="stay"></param>
        public BrowserCommandResult<bool> DismissAlertIfPresent(bool stay = false)
        {

            return this.Execute(GetOptions("Dismiss Confirm Save Alert"), driver =>
            {
                if (driver.AlertIsPresent())
                {
                    if (stay)
                        driver.SwitchTo().Alert().Dismiss();
                    else
                        driver.SwitchTo().Alert().Accept();
                }

                return true;
            });
        }
    }
}