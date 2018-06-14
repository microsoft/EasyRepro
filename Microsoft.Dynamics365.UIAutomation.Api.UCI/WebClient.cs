// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class WebClient : BrowserPage
    {
        public List<ICommandResult> CommandResults => Browser.CommandResults;

        public WebClient(BrowserOptions options)
        {
            Browser = new InteractiveBrowser(options);
            OnlineDomains = Constants.Xrm.XrmDomains;
        }

        internal BrowserCommandOptions GetOptions(string commandName)
        {
            return new BrowserCommandOptions(Constants.DefaultTraceSource,
                commandName,
                0,
                0,
                null,
                true,
                typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        public string[] OnlineDomains { get; set; }

        #region Login
        internal BrowserCommandResult<LoginResult> Login(Uri orUri, SecureString username, SecureString password)
        {
            return this.Execute(GetOptions("Login"), this.Login, orUri, username, password, default(Action<LoginRedirectEventArgs>));
        }

        internal BrowserCommandResult<LoginResult> Login(Uri orgUri, SecureString username, SecureString password, Action<LoginRedirectEventArgs> redirectAction)
        {
            return this.Execute(GetOptions("Login"), this.Login, orgUri, username, password, redirectAction);
        }
        
        private LoginResult Login(IWebDriver driver, Uri uri, SecureString username, SecureString password,
            Action<LoginRedirectEventArgs> redirectAction)
        {
            var redirect = false;
            bool online = !(this.OnlineDomains != null && !this.OnlineDomains.Any(d => uri.Host.EndsWith(d)));
            driver.Navigate().GoToUrl(uri);

            if (online)
            {
                if (driver.IsVisible(By.Id("use_another_account_link")))
                    driver.ClickWhenAvailable(By.Id("use_another_account_link"));

                driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]),
                    $"The Office 365 sign in page did not return the expected result and the user '{username}' could not be signed in.");

                driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(username.ToUnsecureString());
                driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(Keys.Tab);
                driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(Keys.Enter);

                Thread.Sleep(1000);

                if (driver.IsVisible(By.Id("aadTile")))
                {
                    driver.FindElement(By.Id("aadTile")).Click(true);
                }

                Thread.Sleep(1000);

                //If expecting redirect then wait for redirect to trigger
                if (redirectAction != null)
                {
                    //Wait for redirect to occur.
                    Thread.Sleep(3000);

                    redirectAction?.Invoke(new LoginRedirectEventArgs(username, password, driver));

                    redirect = true;
                }
                else
                {
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(password.ToUnsecureString());
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(Keys.Tab);
                    driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).Submit();

                    if (driver.IsVisible(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])))
                    {
                        driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]));
                        driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])).Submit();
                    }

                    driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.CrmMainPage])
                        , new TimeSpan(0, 0, 60),
                        e => {
                            e.WaitForPageToLoad();
                            e.SwitchTo().Frame(0);
                            e.WaitForPageToLoad();
                        },
                        f => { throw new Exception("Login page failed."); });
                }
            }

            return redirect ? LoginResult.Redirect : LoginResult.Success;
        }
        #endregion

        #region Navigation
        internal BrowserCommandResult<bool> OpenApp(string appName,int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open App"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.AppMenuButton]));

                var container = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.AppMenuContainer]));

                var buttons = container.FindElements(By.TagName("button"));

                var button = buttons.FirstOrDefault(x => x.Text == appName);

                if (button != null)
                    button.Click();
                else
                    throw new InvalidOperationException($"App Name {appName} not found.");

                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Application.Shell]));
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Navigation.SiteMapLauncherButton]));
                driver.WaitForPageToLoad();

                return true;
            });
        }

        internal BrowserCommandResult<bool> OpenSubArea(string area, string subarea, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Sub Area"), driver =>
            {
                area = area.ToLowerString();
                subarea = subarea.ToLowerString();

                var areas = OpenMenu().Value;

                if (!areas.ContainsKey(area))
                {
                    throw new InvalidOperationException($"No area with the name '{area}' exists.");
                }

                //Added for Bug
                IWebElement menuItem = null;
                bool foundMenuItem = areas.TryGetValue(area, out menuItem);
                if (foundMenuItem)
                {
                    //For some reason, ClickWhenAvailable isn't ignoring StaleElementExceptions
                    try
                    {
                        driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.SubArea].Replace("[NAME]", menuItem.GetAttribute("data-id"))));
                    }
                    catch (StaleElementReferenceException)
                    {
                    }
                }
                Browser.ThinkTime(500);
                //End Added For Bug

                var subAreas = OpenSubMenu(subarea).Value;

                if (!subAreas.ContainsKey(subarea))
                {
                    throw new InvalidOperationException($"No subarea with the name '{subarea}' exists inside of '{area}'.");
                }

                subAreas[subarea].Click();

                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));
                driver.WaitForPageToLoad();

                return true;
            });
        }
        public BrowserCommandResult<Dictionary<string, IWebElement>> OpenMenu(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Menu"), driver =>
            {
                var dictionary = new Dictionary<string, IWebElement>();

                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.AreaButton]));

                driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.AreaMenu]),
                                            new TimeSpan(0,0,2),
                                            d =>
                                            {
                                                var menu = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.AreaMenu]));
                                                var menuItems = menu.FindElements(By.TagName("li"));
                                                foreach (var item in menuItems)
                                                {
                                                    dictionary.Add(item.Text.ToLowerString(), item);
                                                }
                                            },
                                            e =>
                                            {
                                                throw new InvalidOperationException("The Main Menu is not available.");
                                            });


                return dictionary;
            });
        }
        internal BrowserCommandResult<Dictionary<string, IWebElement>> OpenSubMenu(string subarea, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions($"Open Sub Menu: {subarea}"), driver =>
            {
                var dictionary = new Dictionary<string, IWebElement>();

                var menuContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.SubAreaContainer]));
                
                var subItems = menuContainer.FindElements(By.TagName("li"));

                foreach (var subItem in subItems)
                {
                    dictionary.Add(subItem.GetAttribute("title").ToLowerString(), subItem);
                }

                return dictionary;
            });
        }

        internal BrowserCommandResult<bool> OpenSettingsOption(string command, string dataId, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions($"Open " + command + " " + dataId), driver =>
            {
                var cmdButtonBar = AppElements.Xpath[AppReference.Navigation.SettingsLauncherBar].Replace("[NAME]", command);
                var cmdLauncher  = AppElements.Xpath[AppReference.Navigation.SettingsLauncher].Replace("[NAME]", command);

                if (!driver.IsVisible(By.XPath(cmdLauncher)))
                {
                    driver.ClickWhenAvailable(By.XPath(cmdButtonBar));

                    Thread.Sleep(1000);

                    driver.SetVisible(By.XPath(cmdLauncher), true);
                    driver.WaitUntilVisible(By.XPath(cmdLauncher));
                }

                var menuContainer = driver.FindElement(By.XPath(cmdLauncher));
                var menuItems = menuContainer.FindElements(By.TagName("button"));
                var button = menuItems.FirstOrDefault(x => x.GetAttribute("data-id").Contains(dataId));

                if (button != null)
                {
                    button.Click();
                }
                else
                {
                    throw new InvalidOperationException($"No command with the exists inside of the Command Bar.");
                }

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
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.GuidedHelp]));

                return true;
            });
        }

        /// <summary>
        /// Opens the Admin Portal
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenAdminPortal();</example>
        internal BrowserCommandResult<bool> OpenAdminPortal(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);
            return this.Execute(GetOptions("Open Admin Portal"), driver =>
            {
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Application.Shell]));
                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.AdminPortal]))?.Click();
                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.AdminPortalButton]))?.Click();
                return true;
            });
        }

        /// <summary>
        /// Open Global Search
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Navigation.OpenGlobalSearch();</example>
        internal BrowserCommandResult<bool> OpenGlobalSearch(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Global Search"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Navigation.SearchButton]),
                new TimeSpan(0, 0, 5),
                d => { driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.SearchButton])); },
                d => { throw new InvalidOperationException("The Global Search button is not available."); });
                return true;
            });
        }
        internal BrowserCommandResult<bool> ClickQuickLaunchButton(string toolTip, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Quick Launch: {toolTip}"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Navigation.QuickLaunchMenu]));
                
                //Text could be in the crumb bar.  Find the Quick launch bar buttons and click that one.
                var buttons = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.QuickLaunchMenu]));
                var launchButton = buttons.FindElement(By.XPath(AppElements.Xpath[AppReference.Navigation.QuickLaunchButton].Replace("[NAME]", toolTip)));
                launchButton.Click();

                return true;
            });
        }

        #endregion

        #region Dialogs
        internal bool SwitchToDialog(int frameIndex = 0)
        {
            var index = "";
            if (frameIndex > 0)
                index = frameIndex.ToString();

            Browser.Driver.SwitchTo().DefaultContent();

            // Check to see if dialog is InlineDialog or popup
            var inlineDialog = Browser.Driver.HasElement(By.XPath(Elements.Xpath[Reference.Frames.DialogFrame].Replace("[INDEX]", index)));
            if (inlineDialog)
            {
                //wait for the content panel to render
                Browser.Driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.DialogFrame].Replace("[INDEX]", index)),
                                                  new TimeSpan(0, 0, 2),
                                                  d => { Browser.Driver.SwitchTo().Frame(Elements.ElementId[Reference.Frames.DialogFrameId].Replace("[INDEX]", index)); });
                return true;
            }
            else
            {
                // need to add this functionality
                //SwitchToPopup();
            }
            return true;
        }
        internal BrowserCommandResult<bool> CloseWarningDialog()
        {
            return this.Execute(GetOptions($"Close Warning Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Dialogs.WarningFooter]));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(Elements.Xpath[Reference.Dialogs.WarningCloseButton])).Count >
                          0)) return true;
                    var closeBtn = dialogFooter.FindElement(By.XPath(Elements.Xpath[Reference.Dialogs.WarningCloseButton]));
                    closeBtn.Click();
                }
                return true;
            });
        }
        internal BrowserCommandResult<bool> ConfirmationDialog(bool ClickConfirmButton)
        {
            //Passing true clicks the confirm button.  Passing false clicks the Cancel button.
            return this.Execute(GetOptions($"Confirm or Cancel Confirmation Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    //Wait until the buttons are available to click
                    var dialogFooter = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.ConfirmButton]));

                    if (
                        !(dialogFooter?.FindElements(By.XPath(AppElements.Xpath[AppReference.Dialogs.ConfirmButton])).Count >
                          0)) return true;

                    //Click the Confirm or Cancel button
                    IWebElement buttonToClick;
                    if (ClickConfirmButton)
                        buttonToClick = dialogFooter.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.ConfirmButton]));
                    else
                        buttonToClick = dialogFooter.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.CancelButton]));

                    buttonToClick.Click();
                }
                return true;
            });
        }
        internal BrowserCommandResult<bool> AssignDialog(string userOrTeamName)
        {
            return this.Execute(GetOptions($"Assign to User or Team Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();
                if (inlineDialog)
                {
                    //Click the Option to Assign to User Or Team
                    driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Dialogs.AssignDialogToggle]));

                    var toggleButton = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.AssignDialogToggle]),"Me/UserTeam toggle button unavailable");
                    if (toggleButton.Text == "Me")
                        toggleButton.Click();

                    //Set the User Or Team
                    var userOrTeamField = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookup]), "User field unavailable");

                    if (userOrTeamField.FindElements(By.TagName("input")).Count > 0)
                    {
                        var input = userOrTeamField.FindElement(By.TagName("input"));
                        if (input != null)
                        {
                            input.Click();
                            input.SendKeys(userOrTeamName, true);
                        }
                    }

                    //Pick the User from the list
                    driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.AssignDialogUserTeamLookupResults]));
                    //Remove this line
                    driver.WaitUntilVisible(By.XPath("//label[text()='[NAME]']".Replace("[NAME]", userOrTeamName)));

                    //WaitUntilVisible is too fast, adding a wait to allow CRM to catch up.  This needs to be removed.
                    Browser.ThinkTime(5000);
                    var container = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.AssignDialogUserTeamLookupResults]));
                    var records = container.FindElements(By.TagName("li"));
                    foreach (var record in records)
                    {
                        if (record.Text.StartsWith(userOrTeamName, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                record.Click();
                                break;
                            }
                            catch (StaleElementReferenceException)
                            {
                                continue;
                            }
                        }
                    }

                    //Click Assign
                    var buttonToClick = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.AssignDialogOKButton]));
                    try
                    {
                        buttonToClick.Click();
                    }
                    catch(StaleElementReferenceException)
                    {
                    }
                }
                return true;
            });
        }
        internal BrowserCommandResult<bool> SwitchProcessDialog(string processToSwitchTo)
        {
            return this.Execute(GetOptions($"Switch Process Dialog"), driver =>
            {
                //Wait for the Grid to load
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.ActiveProcessGridControlContainer]));

                //Select the Process
                var popup = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.SwitchProcessContainer]));
                var labels = popup.FindElements(By.TagName("label"));
                foreach (var label in labels)
                {
                    if (label.Text.Equals(processToSwitchTo, StringComparison.OrdinalIgnoreCase))
                    {
                        label.Click();
                        break;
                    }
                }

                //Click the OK button
                var okBtn = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.SwitchProcessDialogOK]));
                okBtn.Click();

                return true;
            });
        }
        internal BrowserCommandResult<bool> CloseOpportunityDialog(bool clickOK)
        {
            return this.Execute(GetOptions($"Assign to User or Team Dialog"), driver =>
            {
                var inlineDialog = this.SwitchToDialog();

                if (inlineDialog)
                {
                    //Close Opportunity
                    var xPath = AppElements.Xpath[AppReference.Dialogs.CloseOpportunityOKButton];

                    //Cancel
                    if (!clickOK)
                        xPath = AppElements.Xpath[AppReference.Dialogs.CloseOpportunityCancelButton];

                    driver.WaitUntilClickable(By.XPath(xPath),
                        new TimeSpan(0, 0, 5),
                        d => { driver.ClickWhenAvailable(By.XPath(xPath)); },
                        d => { throw new InvalidOperationException("The Close Opportunity dialog is not available."); });
                }
                return true;
            });
        }
        internal BrowserCommandResult<bool> HandleSaveDialog()
        {
            //If you click save and something happens, handle it.  Duplicate Detection/Errors/etc...
            //Check for Dialog and figure out which type it is and return the dialog type.

            return this.Execute(GetOptions($"Validate Save"), driver =>
            {
                //Is it Duplicate Detection?
                if(driver.HasElement(By.XPath("//div[contains(@data-id,'ManageDuplicates')]")))
                {
                    //Select the first record in the grid
                    driver.FindElements(By.XPath("//div[contains(@class,'data-selectable')]"))[0].Click();

                    //Click Ignore and Save
                    driver.FindElement(By.XPath("//button[contains(@data-id,'ignore_save')]")).Click();
                    //driver.FindElement(By.XPath("//button[contains(@data-id,'close_dialog')]")).Click();

                }

                return true;
            });
        }

        /// <summary>
        /// Opens the dialog
        /// </summary>
        /// <param name="dialog"></param>
        public BrowserCommandResult<List<ListItem>> OpenDialog(IWebElement dialog)
        {
            var list = new List<ListItem>();
            var dialogItems = dialog.FindElements(By.XPath(".//li"));

            foreach (var dialogItem in dialogItems)
            {
                var titleLinks = dialogItem.FindElements(By.XPath(".//label/span"));
                var divLinks = dialogItem.FindElements(By.XPath(".//div"));

                if (titleLinks != null && titleLinks.Count > 0 && divLinks != null && divLinks.Count > 0)
                {
                    var title = titleLinks[0].GetAttribute("innerText");
                    var divId = divLinks[0].GetAttribute("id");

                    list.Add(new ListItem()
                    {
                        Title = title,
                        Id = divId,
                    });
                }
            }

            return list;
        }
        #endregion

        #region CommandBar
        internal BrowserCommandResult<bool> ClickCommand(string name, string subname = "", bool moreCommands = false, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Set Value"), driver =>
            {
                bool success = false;
                IWebElement ribbon = null;

                //Try to click the command bar from inside an entity record.  If that fails, try to click the Command Bar from within a Grid.
                try
                {
                    ribbon = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container]));
                    success = true;
                }
                catch(Exception)
                {
                    success = false;
                }

                try
                {
                    ribbon = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid]));
                    success = true;
                }
                catch(Exception)
                {
                    success = false;
                }

                var items = ribbon.FindElements(By.TagName("li"));

                IWebElement button; 

                button = subname == "" ? items.FirstOrDefault(x => x.GetAttribute("aria-label") == name) : items.FirstOrDefault(x => x.GetAttribute("aria-label") == name + " More Commands");

                if (button != null)
                {
                    button.Click();
                }
                else
                {
                    throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");
                }

                if (!string.IsNullOrEmpty(subname))
                {
                    var submenu = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.MoreCommandsMenu]));

                    var subbutton = submenu.FindElements(By.TagName("button")).FirstOrDefault(x=>x.Text==subname);

                    if (subbutton != null)
                    {
                        subbutton.Click();
                    }
                    else
                        throw new InvalidOperationException($"No sub command with the name '{subname}' exists inside of Commandbar.");

                }
                return true;
            });
        }
        #endregion

        #region Grid
        public BrowserCommandResult<Dictionary<string, string>> OpenViewPicker(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open View Picker"), driver =>
            {
                var dictionary = new Dictionary<string, string>();

                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelector]),
                    new TimeSpan(0, 0, 20),
                    d => { d.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelector])); },
                    d => { throw new Exception("Unable to click the View Picker"); });

                //driver.WaitUntilVisible(By.ClassName(AppElements.Xpath[AppReference.Grid.ViewSelector]),
                //    new TimeSpan(0, 0, 20),
                //    null,
                //    d =>
                //    {
                //        //Fix for Firefox not clicking the element in the event above. Issue with the driver. 
                //        d.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelector]));
                //        driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Grid.ViewSelector]), new TimeSpan(0, 0, 3), null, e => { throw new Exception("View Picker menu is not avilable"); });

                //    });

                var viewContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.ViewContainer]));
                var viewItems = viewContainer.FindElements(By.TagName("li"));

                foreach (var viewItem in viewItems)
                {
                    if (viewItem.GetAttribute("role") != null && viewItem.GetAttribute("role") == "option")
                    {
                        dictionary.Add(viewItem.Text, viewItem.GetAttribute("id"));
                    }
                }

                return dictionary;
            });
        }
        internal BrowserCommandResult<bool> SwitchView(string viewName, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Switch View"), driver =>
            {
                var views = OpenViewPicker().Value;
                Thread.Sleep(500);
                if (!views.ContainsKey(viewName))
                {
                    throw new InvalidOperationException($"No view with the name '{viewName}' exists.");
                }

                var viewId = views[viewName];
                driver.ClickWhenAvailable(By.Id(viewId));

                return true;
            });
        }
        internal BrowserCommandResult<bool> OpenRecord(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Grid Record"), driver =>
            {
                var currentindex = 0;
                var control = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));

                var rows = driver.FindElements(By.ClassName("wj-row"));
                
                //TODO: The grid only has a small subset of records. Need to load them all
                foreach (var row in rows)
                {
                    if (!string.IsNullOrEmpty(row.GetAttribute("data-lp-id")))
                    {
                        if (currentindex == index)
                        {
                            row.FindElement(By.TagName("a")).Click();
                            break;
                        }

                        currentindex++;
                    }
                }

                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Entity.Form]));
                driver.WaitForPageToLoad();

                return true;
            });
        }
        internal BrowserCommandResult<bool> Search(string searchCriteria, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Search"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Grid.QuickFind]));
                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.QuickFind])).SendKeys(searchCriteria);
                driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.QuickFind])).SendKeys(Keys.Enter);

                //driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<List<GridItem>> GetGridItems(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Get Grid Items"), driver =>
            {
                var returnList = new List<GridItem>();

                driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));

                var rows = driver.FindElements(By.ClassName("wj-row"));
                var columnGroup = driver.FindElement(By.ClassName("wj-colheaders"));
                
                foreach (var row in rows)
                {
                    if (!string.IsNullOrEmpty(row.GetAttribute("data-lp-id")) && !string.IsNullOrEmpty(row.GetAttribute("role")))
                    {
                        //MscrmControls.Grid.ReadOnlyGrid|entity_control|account|00000000-0000-0000-00aa-000010001001|account|cc-grid|grid-cell-container
                        var datalpid = row.GetAttribute("data-lp-id").Split('|');
                        var cells = row.FindElements(By.ClassName("wj-cell"));
                        var currentindex = 0;
                        var link =
                           $"{new Uri(driver.Url).Scheme}://{new Uri(driver.Url).Authority}/main.aspx?etn={datalpid[2]}&pagetype=entityrecord&id=%7B{datalpid[3]}%7D";

                        var item = new GridItem
                        {
                            EntityName = datalpid[2],
                            Url = new Uri(link)
                        };

                        foreach (var column in columnGroup.FindElements(By.ClassName("wj-row")))
                        {
                            var rowHeaders = column.FindElements(By.TagName("div"))
                                .Where(c => !string.IsNullOrEmpty(c.GetAttribute("title")) && !string.IsNullOrEmpty(c.GetAttribute("id")));

                            foreach (var header in rowHeaders)
                            {
                                var id = header.GetAttribute("id");
                                var className = header.GetAttribute("class");
                                var cellData = cells[currentindex + 1].GetAttribute("title");

                                if (!string.IsNullOrEmpty(id)
                                    && className.Contains("wj-cell")
                                    && !string.IsNullOrEmpty(cellData)
                                    && cells.Count > currentindex
                                )
                                {
                                    item[id] = cellData.Replace("-", "");
                                }
                                currentindex++;
                            }
                            returnList.Add(item);
                        }
                    }
                }
                return returnList;
            });
        }
        internal BrowserCommandResult<bool> NextPage(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Next Page"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.NextPage]));

                driver.WaitForTransaction();

                return true;
            });
        }
        internal BrowserCommandResult<bool> PreviousPage(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Previous Page"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.PreviousPage]));

                driver.WaitForTransaction();

                return true;
            });
        }
        internal BrowserCommandResult<bool> FirstPage(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"First Page"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.FirstPage]));

                driver.WaitForTransaction();

                return true;
            });
        }
        internal BrowserCommandResult<bool> SelectAll(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Select All"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.SelectAll]));

                driver.WaitForTransaction();

                return true;
            });
        }
        public BrowserCommandResult<bool> ShowChart(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Show Chart"), driver =>
            {
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Grid.ShowChart])))
                {
                    driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.ShowChart]));

                    driver.WaitForTransaction();
                }
                else
                {
                    throw new Exception("The Show Chart button does not exist.");
                }

                return true;
            });
        }
        public BrowserCommandResult<bool> HideChart(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Hide Chart"), driver =>
            {
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Grid.HideChart])))
                {
                    driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.HideChart]));

                    driver.WaitForTransaction();
                }
                else
                {
                    throw new Exception("The Hide Chart button does not exist.");
                }

                return true;
            });
        }
        public BrowserCommandResult<bool> FilterByLetter(char filter, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            if (!Char.IsLetter(filter) && filter != '#')
                throw new InvalidOperationException("Filter criteria is not valid.");

            return this.Execute(GetOptions("Filter by Letter"), driver =>
            {
                var jumpBar = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.JumpBar]));
                var link = jumpBar.FindElement(By.Id(filter + "_link"));

                if (link != null)
                {
                    link.Click();

                    driver.WaitForTransaction();
                }
                else
                {
                    throw new Exception($"Filter with letter: {filter} link does not exist");
                }

                return true;
            });
        }
        public BrowserCommandResult<bool> FilterByAll(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Filter by All Records"), driver =>
            {
                var jumpBar = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.JumpBar]));
                var link = jumpBar.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.FilterByAll]));

                if (link != null)
                {
                    link.Click();

                    driver.WaitForTransaction();
                }
                else
                {
                    throw new Exception($"Filter by All link does not exist");
                }

                return true;
            });
        }
        public BrowserCommandResult<bool> SelectRecord(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Select Grid Record"), driver =>
            {
                var container = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.RowsContainer]),
                                                        $"Grid Container does not exist.");

                var row = container.FindElement(By.Id("id-cell-" + index + "-1"));

                if (row != null)
                    row.Click();
                else
                    throw new Exception($"Row with index: {index} does not exist.");

                return false;
            });
        }
        public BrowserCommandResult<bool> SwitchChart(string chartName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            if (!Browser.Driver.IsVisible(By.XPath(AppElements.Xpath[AppReference.Grid.ChartSelector])))
                ShowChart();

            Browser.ThinkTime(1000);

            return this.Execute(GetOptions("Switch Chart"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Grid.ChartSelector]));

                var list = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.ChartViewList]));

                driver.ClickWhenAvailable(By.XPath("//li[contains(@title,'" + chartName + "')]"));

                return true;
            });
        }
        public BrowserCommandResult<bool> Sort(string columnName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Sort by {columnName}"), driver =>
            {
                var sortCol = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.GridSortColumn].Replace("[COLNAME]", columnName)));
                
                if (sortCol == null)
                    throw new InvalidOperationException($"Column: {columnName} Does not exist");
                else
                    sortCol.Click();
                return true;
            });
        }
        #endregion

        #region RelatedGrid
        /// <summary>
        /// Opens the grid record.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> OpenGridRow(int index, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Grid Item"), driver =>
            {
                var grid = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.Container]));
                var gridCellContainer = grid.FindElement(By.XPath(AppElements.Xpath[AppReference.Grid.CellContainer]));
                var rowCount = gridCellContainer.GetAttribute("data-row-count");
                var count = 0;

                if (rowCount == null || !int.TryParse(rowCount, out count) || count <= 0) return true;
                var link =
                    gridCellContainer.FindElement(
                        By.XPath("//div[@role='gridcell'][@header-row-number='" + index + "']/following::div"));

                if (link != null)
                {
                    link.Click();
                }
                else
                {
                    throw new InvalidOperationException($"No record with the index '{index}' exists.");
                }
                return true;
            });
        }
        #endregion

        #region Entity
        /// <summary>
        /// Open Entity
        /// </summary>
        /// <param name="entityName">The entity name</param>
        /// <param name="id">The Id</param>
        /// <param name="thinkTime">The think time</param>
        internal BrowserCommandResult<bool> OpenEntity(string entityName, Guid id, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open: {entityName} {id}"), driver =>
            {
                var uri = new Uri(this.Browser.Driver.Url);
                var link = $"{uri.Scheme}://{uri.Authority}/main.aspx?etn={entityName}&pagetype=entityrecord&id=%7B{id:D}%7D";

                driver.Navigate().GoToUrl(uri);

                //SwitchToContent();
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
        /// Saves the entity
        /// </summary>
        /// <param name="thinkTime"></param>
        internal BrowserCommandResult<bool> Save(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Save"), driver =>
            {
                var save = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.Save]),
                    "Save Buttton is not available");

                save?.Click();

                return true;
            });
        }

        /// <summary>
        /// Open record set and navigate record index.
        /// This method supersedes Navigate Up and Navigate Down outside of UCI 
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Entity.OpenRecordSetNavigator();</example>
        public BrowserCommandResult<bool> OpenRecordSetNavigator(int index = 0, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Open Record Set Navigator"), driver =>
            {
                // check if record set navigator parent div is set to open
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavigatorOpen])))
                {
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavigator])).Click();
                }

                var navList = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavList]));
                var links = navList.FindElements(By.TagName("li"));
                try
                {
                    links[index].Click();
                }
                catch
                {
                    throw new InvalidOperationException($"No record with the index '{index}' exists.");
                }

                driver.WaitForPageToLoad();

                return true;
            });
        }

        /// <summary>
        /// Close Record Set Navigator
        /// </summary>
        /// <param name="thinkTime"></param>
        /// <example>xrmApp.Entity.CloseRecordSetNavigator();</example>
        public BrowserCommandResult<bool> CloseRecordSetNavigator(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Close Record Set Navigator"), driver =>
            {
                var closeSpan =
                    driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavCollapseIcon]));

                if (closeSpan)
                {
                    driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.RecordSetNavCollapseIconParent])).Click();
                }

                return true;
            });
        }

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.Entity.SetValue("firstname", "Test");</example>
        internal BrowserCommandResult<bool> SetValue(string field, string value)
        {
            return this.Execute(GetOptions($"Set Value"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));

                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    var input = fieldContainer.FindElement(By.TagName("input"));
                    if (input != null)
                    {
                        input.Click();
                        input.SendKeys(value, true);
                    }
                }
                else if (fieldContainer.FindElements(By.TagName("textarea")).Count > 0)
                {
                    fieldContainer.FindElement(By.TagName("textarea")).Click();
                    fieldContainer.FindElement(By.TagName("textarea")).Clear();
                    fieldContainer.FindElement(By.TagName("textarea")).SendKeys(value);
                }
                else
                {
                    throw new Exception($"Field with name {field} does not exist.");
                }

                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        internal BrowserCommandResult<bool> SetValue(LookupItem control)
        {
            return this.Execute(GetOptions($"Set Lookup Value: {control.Name}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupFieldContainer].Replace("[NAME]", control.Name)));

                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    var input = fieldContainer.FindElement(By.TagName("input"));
                    if (input != null)
                    {
                        input.Click();
                        input.SendKeys(control.Value, true);
                    }
                }

                var flyoutDialog = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupMenu].Replace("[NAME]", control.Name)));
                var dialogItems = OpenDialog(flyoutDialog).Value;

                if (control.Value != null)
                {
                    if (!dialogItems.Exists(x => x.Title == control.Value))
                        throw new InvalidOperationException($"List does not have {control.Value}.");

                    var dialogItem = dialogItems.Where(x => x.Title == control.Value).First();
                    driver.ClickWhenAvailable(By.Id(dialogItem.Id));
                }
                else
                {
                    if (dialogItems.Count < control.Index)
                        throw new InvalidOperationException($"List does not have {control.Index + 1} items.");

                    var dialogItem = dialogItems[control.Index];
                    driver.ClickWhenAvailable(By.Id(dialogItem.Id));
                }
                return true;
            });
        }

        /// <summary>
        /// Sets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public BrowserCommandResult<bool> SetValue(OptionSet option)
        {
            return this.Execute(GetOptions($"Set Value: {option.Name}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", option.Name)));

                if (fieldContainer.FindElements(By.TagName("select")).Count > 0)
                {
                    var select = fieldContainer.FindElement(By.TagName("select"));
                    var options = select.FindElements(By.TagName("option"));

                    foreach (var op in options)
                    {
                        if (op.Text != option.Value && op.GetAttribute("value") != option.Value) continue;
                        op.Click();
                        break;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");
                }
                return true;
            });
        }

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">The field id or name.</param>
        /// <param name="date">DateTime value.</param>
        /// <param name="format">DateTime format</param>
        /// <example> xrmBrowser.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        public BrowserCommandResult<bool> SetValue(string field, DateTime date, string format = "MM dd yyyy")
        {
            return this.Execute(GetOptions($"Set Value: {field}"), driver =>
            {
                var dateField = AppElements.Xpath[AppReference.Entity.FieldControlDateTimeInput].Replace("[FIELD]",field);

                if (driver.HasElement(By.XPath(dateField)))
                {
                    var fieldElement = driver.ClickWhenAvailable(By.XPath(dateField));

                    if (fieldElement.GetAttribute("value").Length > 0)
                    {
                        fieldElement.Click();
                        fieldElement.SendKeys(date.ToString(format));
                        fieldElement.SendKeys(Keys.Enter);
                    }
                    else
                    {
                        fieldElement.SendKeys(date.ToString(format));
                        fieldElement.SendKeys(Keys.Enter);
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
        }
        internal BrowserCommandResult<string> GetValue(string field)
        {
            return this.Execute(GetOptions($"Get Value"), driver =>
            {
                string text = string.Empty;
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));

                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    var input = fieldContainer.FindElement(By.TagName("input"));
                    if (input != null)
                    {
                        text = input.GetAttribute(field);
                    }
                }
                else if (fieldContainer.FindElements(By.TagName("textarea")).Count > 0)
                {
                    text = fieldContainer.FindElement(By.TagName("textarea")).GetAttribute(field);
                }
                else
                {
                    throw new Exception($"Field with name {field} does not exist.");
                }

                return text;
            });
        }

        /// <summary>
        /// Gets the value of a Lookup.
        /// </summary>
        /// <param name="control">The lookup field name of the lookup.</param>
        /// <example>xrmApp.Entity.GetValue(new Lookup { Name = "primarycontactid" });</example>
        public BrowserCommandResult<string> GetValue(LookupItem control)
        {
            return this.Execute($"Get Lookup Value: {control.Name}", driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupFieldContainer].Replace("[NAME]", control.Name)));

                string lookupValue = string.Empty;
                try
                {
                    if (fieldContainer.FindElements(By.TagName("input")).Any())
                    {
                        lookupValue = fieldContainer.FindElement(By.TagName("input")).GetAttribute("value");
                    }
                    else if(fieldContainer.FindElements(By.XPath(".//label")).Any())
                    {
                        var label = fieldContainer.FindElement(By.XPath(".//label"));
                        lookupValue = fieldContainer.FindElement(By.XPath(".//label")).GetAttribute("innerText");
                    }
                    else
                    {
                        throw new InvalidOperationException($"Field: {control.Name} Does not exist");
                    }
                }
                catch (Exception exp)
                {
                    throw new InvalidOperationException($"Field: {control.Name} Does not exist",exp);
                }

                return lookupValue;
            });
        }

        /// <summary>
        /// Gets the value of a picklist.
        /// </summary>
        /// <param name="option">The option you want to set.</param>
        /// <example>xrmBrowser.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"}); </example>
        internal BrowserCommandResult<string> GetValue(OptionSet option)
        {
            return this.Execute($"Get Value: {option.Name}", driver =>
            {
                var text = string.Empty;
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", option.Name)));

                if (fieldContainer.FindElements(By.TagName("select")).Count > 0)
                {
                    var select = fieldContainer.FindElement(By.TagName("select"));
                    var options = select.FindElements(By.TagName("option"));
                    foreach (var op in options)
                    {
                        if (!op.Selected) continue;
                        text = op.Text;
                        break;
                    }
                }else
                {
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");
                }
                return text;
               
            });
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        /// <returns>True on success</returns>
        internal BrowserCommandResult<bool> SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            return this.Execute(GetOptions($"Set Value: {option.Name}"), driver =>
            {
                if(removeExistingValues)
                {
                    RemoveMultiOptions(option);
                }
                else
                {
                    AddMultiOptions(option);
                }
                return true;
            });
        }

        /// <summary>
        /// Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be removed</param>
        /// <returns></returns>
        private BrowserCommandResult<bool> RemoveMultiOptions(MultiValueOptionSet option)
        {
            return this.Execute(GetOptions($"Remove Multi Select Value: {option.Name}"), driver =>
            {
                string xpath = AppElements.Xpath[AppReference.MultiSelect.SelectedRecord].Replace("[NAME]", Elements.ElementId[option.Name]);
                // If there is already some pre-selected items in the div then we must determine if it
                // actually exists and simulate a set focus event on that div so that the input textbox
                // becomes visible.
                var listItems = driver.FindElements(By.XPath(xpath));
                if (listItems.Any())
                {
                    listItems.First().SendKeys("");
                }

                // If there are large number of options selected then a small expand collapse 
                // button needs to be clicked to expose all the list elements.
                xpath = AppElements.Xpath[AppReference.MultiSelect.ExpandCollapseButton].Replace("[NAME]", Elements.ElementId[option.Name]);
                var expandCollapseButtons = driver.FindElements(By.XPath(xpath));
                if (expandCollapseButtons.Any())
                {
                    expandCollapseButtons.First().Click(true);
                }

                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.InputSearch].Replace("[NAME]", Elements.ElementId[option.Name])));
                foreach (var optionValue in option.Values)
                {
                    xpath = String.Format(AppElements.Xpath[AppReference.MultiSelect.SelectedRecordButton].Replace("[NAME]", Elements.ElementId[option.Name]), optionValue);
                    var listItemObjects = driver.FindElements(By.XPath(xpath));
                    var loopCounts = listItemObjects.Any() ? listItemObjects.Count : 0;

                    for (int i = 0; i < loopCounts; i++)
                    {
                        // With every click of the button, the underlying DOM changes and the
                        // entire collection becomes stale, hence we only click the first occurance of
                        // the button and loop back to again find the elements and anyother occurance
                        driver.FindElements(By.XPath(xpath)).First().Click(true);
                    }
                }
                return true;
            });
        }

        /// <summary>
        /// Sets the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set</param>
        /// <returns></returns>
        private BrowserCommandResult<bool> AddMultiOptions(MultiValueOptionSet option)
        {
            return this.Execute(GetOptions($"Add Multi Select Value: {option.Name}"), driver =>
            {
                string xpath = AppElements.Xpath[AppReference.MultiSelect.SelectedRecord].Replace("[NAME]", Elements.ElementId[option.Name]);
                // If there is already some pre-selected items in the div then we must determine if it
                // actually exists and simulate a set focus event on that div so that the input textbox
                // becomes visible.
                var listItems = driver.FindElements(By.XPath(xpath));
                if (listItems.Any())
                {
                    listItems.First().SendKeys("");
                }

                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.InputSearch].Replace("[NAME]", Elements.ElementId[option.Name])));
                foreach (var optionValue in option.Values)
                {
                    xpath = String.Format(AppElements.Xpath[AppReference.MultiSelect.FlyoutList].Replace("[NAME]", Elements.ElementId[option.Name]), optionValue);
                    var flyout = driver.FindElements(By.XPath(xpath));
                    if (flyout.Any())
                    {
                        flyout.First().Click(true);
                    }
                }

                // Click on the div containing textbox so that the floyout collapses or else the flyout
                // will interfere in finding the next multiselect control which by chance will be lying
                // behind the flyout control.
                //driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", Elements.ElementId[option.Name])));
                xpath = AppElements.Xpath[AppReference.MultiSelect.DivContainer].Replace("[NAME]", Elements.ElementId[option.Name]);
                var divElements = driver.FindElements(By.XPath(xpath));
                if(divElements.Any())
                {
                    divElements.First().Click(true);
                }
                return true;
            });
        }

        /// <summary>
        /// Gets the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field</param>
        /// <returns>MultiValueOptionSet object where the values field contains all the contact names</returns>
        internal BrowserCommandResult<MultiValueOptionSet> GetValue(MultiValueOptionSet option)
        {
            return this.Execute(GetOptions($"Get Multi Select Value: {option.Name}"), driver =>
            {
                // If there are large number of options selected then a small expand collapse 
                // button needs to be clicked to expose all the list elements.
                string xpath = AppElements.Xpath[AppReference.MultiSelect.ExpandCollapseButton].Replace("[NAME]", Elements.ElementId[option.Name]);
                var expandCollapseButtons = driver.FindElements(By.XPath(xpath));
                if (expandCollapseButtons.Any())
                {
                    expandCollapseButtons.First().Click(true);
                }

                var returnValue = new MultiValueOptionSet();
                returnValue.Name = option.Name;

                xpath = AppElements.Xpath[AppReference.MultiSelect.SelectedRecordLabel].Replace("[NAME]", Elements.ElementId[option.Name]);
                var labelItems = driver.FindElements(By.XPath(xpath));
                if(labelItems.Any())
                {
                    returnValue.Values = labelItems.Select(x => x.Text).ToArray();
                }

                return returnValue;
            });
        }

        #endregion

        #region Timeline

        /// <summary>
        /// This method opens the popout menus in the Dynamics 365 pages. 
        /// This method uses a thinktime since after the page loads, it takes some time for the 
        /// widgets to load before the method can find and popout the menu.
        /// </summary>
        /// <param name="popoutName">The By Object of the Popout menu</param>
        /// <param name="popoutItemName">The By Object of the Popout Item name in the popout menu</param>
        /// <param name="thinkTime">Amount of time(milliseconds) to wait before this method will click on the "+" popout menu.</param>
        /// <returns>True on success, False on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(By menuName, By menuItemName, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute($"Open menu", driver =>
            {
                driver.ClickWhenAvailable(menuName);
                try
                {
                    driver.ClickWhenAvailable(menuItemName);
                }
                catch
                {
                    // Element is stale reference is thrown here since the HTML components 
                    // get destroyed and thus leaving the references null. 
                    // It is expected that the components will be destroyed and the next 
                    // action should take place after it and hence it is ignored.
                    return false;
                }
                return true;
            });
        }
        internal BrowserCommandResult<bool> Delete(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Delete Entity"), driver =>
            {
                var deleteBtn = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.Delete]),
    "Delete Button is not available");

                deleteBtn?.Click();
                ConfirmationDialog(true);

                return true;
            });
        }
        internal BrowserCommandResult<bool> Assign(string userOrTeamToAssign, int thinkTime = Constants.DefaultThinkTime)
        {
            //Click the Assign Button on the Entity Record
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Assign Entity"), driver =>
            {
                var assignBtn = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.Assign]),
    "Assign Button is not available");

                assignBtn?.Click();
                AssignDialog(userOrTeamToAssign);

                return true;
            });
            //Add the User or Team to the field

            //Click the Assign Button on the dialog
        }
        internal BrowserCommandResult<bool> SwitchProcess(string processToSwitchTo, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Switch BusinessProcessFlow"), driver =>
            {
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Entity.ProcessButton]), new TimeSpan(0, 0, 5));
                var processBtn = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.ProcessButton]));
                processBtn?.Click();

                try
                {
                    driver.WaitUntilAvailable(
                        By.XPath(AppElements.Xpath[AppReference.Entity.SwitchProcess]),
                        new TimeSpan(0, 0, 5),
                        d => { driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.SwitchProcess])); },
                        d => { throw new InvalidOperationException("The Switch Process Button is not available."); }
                            );
                }
                catch(StaleElementReferenceException)
                {
                    Console.WriteLine("ignoring stale element exceptions");
                }
                //switchProcessBtn?.Click();

                SwitchProcessDialog(processToSwitchTo);

                return true;
            });
        }
        internal BrowserCommandResult<bool> CloseOpportunity(bool closeAsWon, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);
            string xPathQuery = String.Empty;

            if (closeAsWon)
                xPathQuery = AppElements.Xpath[AppReference.Entity.CloseOpportunityWin];
            else
                xPathQuery = AppElements.Xpath[AppReference.Entity.CloseOpportunityLoss];

            return this.Execute(GetOptions($"Close Opportunity"), driver =>
            {
                var closeBtn = driver.WaitUntilAvailable(By.XPath(xPathQuery), "Opportunity Close Button is not available");

                closeBtn?.Click();
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunityOKButton]));
                CloseOpportunityDialog(true);

                return true;
            });
        }

        /// <summary>
        /// This method opens the popout menus in the Dynamics 365 pages. 
        /// This method uses a thinktime since after the page loads, it takes some time for the 
        /// widgets to load before the method can find and popout the menu.
        /// </summary>
        /// <param name="popoutName">The name of the Popout menu</param>
        /// <param name="popoutItemName">The name of the Popout Item name in the popout menu</param>
        /// <param name="thinkTime">Amount of time(milliseconds) to wait before this method will click on the "+" popout menu.</param>
        /// <returns>True on success, False on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(string popoutName, string popoutItemName, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.OpenAndClickPopoutMenu(By.XPath(Elements.Xpath[popoutName]), By.XPath(Elements.Xpath[popoutItemName]), thinkTime);
        }


        /// <summary>
        /// Provided a By object which represents a HTML Button object, this method will
        /// find it and click it.
        /// </summary>
        /// <param name="by">The object of Type By which represents a HTML Button object</param>
        /// <returns>True on success, False/Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> ClickButton(By by)
        {
            return this.Execute($"Open Timeline Add Post Popout", driver =>
            {
                var button = driver.WaitUntilAvailable(by);
                if (button.TagName.Equals("button"))
                {
                    try
                    {
                        driver.ClickWhenAvailable(by);
                    }
                    catch
                    {
                        // Element is stale reference is thrown here since the HTML components 
                        // get destroyed and thus leaving the references null. 
                        // It is expected that the components will be destroyed and the next 
                        // action should take place after it and hence it is ignored.
                    }
                    return true;
                }
                else if (button.FindElements(By.TagName("button")).Any())
                {
                    button.FindElements(By.TagName("button")).First().Click();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"Control does not exist");
                }
            });
        }

        /// <summary>
        /// Provided a fieldname as a XPath which represents a HTML Button object, this method will
        /// find it and click it.
        /// </summary>
        /// <param name="fieldNameXpath">The field as a XPath which represents a HTML Button object</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> ClickButton(string fieldNameXpath)
        {
            try
            {
                return ClickButton(By.XPath(fieldNameXpath));
            }
            catch(Exception e)
            {
                throw new InvalidOperationException($"Field: {fieldNameXpath} with Does not exist", e);
            }
        }

        /// <summary>
        /// Generic method to help click on any item which is clickable or uniquely discoverable with a By object.
        /// </summary>
        /// <param name="by">The xpath of the HTML item as a By object</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> SelectTab(string tabName, string subTabName = "")
        {
            return this.Execute($"Select Tab", driver =>
            {
                driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TabList]));

                ClickTab(AppElements.Xpath[AppReference.Entity.Tab], tabName);

                //Click Sub Tab if provided
                if (!String.IsNullOrEmpty(subTabName))
                {
                    ClickTab(AppElements.Xpath[AppReference.Entity.SubTab], subTabName);
                }

                return true;
            });
        }

        internal void ClickTab(string xpath, string name)
        {
            if (this.Browser.Driver.HasElement(By.XPath(String.Format(xpath, name))))
            {
                this.Browser.Driver.FindElement(By.XPath(String.Format(xpath, name))).Click(true);
            }
            else
            {
                throw new Exception($"The tab with name: {name} does not exist");
            }
        }

        /// <summary>
        /// A generic setter method which will find the HTML Textbox/Textarea object and populate
        /// it with the value provided. The expected tag name is to make sure that it hits
        /// the expected tag and not some other object with the similar fieldname.
        /// </summary>
        /// <param name="fieldName">The name of the field representing the HTML Textbox/Textarea object</param>
        /// <param name="value">The string value which will be populated in the HTML Textbox/Textarea</param>
        /// <param name="expectedTagName">Expected values - textbox/textarea</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> SetValue(string fieldName, string value, string expectedTagName)
        {
            return this.Execute($"Open Timeline Add Post Popout", driver =>
            {
                var inputbox = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[fieldName]));
                if (expectedTagName.Equals(inputbox.TagName,StringComparison.InvariantCultureIgnoreCase))
                {
                    inputbox.Click();
                    inputbox.Clear();
                    inputbox.SendKeys(value);
                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"Field: {fieldName} with tagname {expectedTagName} Does not exist");
                }
            });
        }

        #endregion

        #region BusinessProcessFlow
        internal BrowserCommandResult<bool> NextStage(string stageName, Field businessProcessFlowField = null, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Next Stage"), driver =>
            {
                //Find the Business Process Stages
                var processStages = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.NextStage_UCI]));

                foreach (var processStage in processStages)
                {
                    var labels = processStage.FindElements(By.TagName("label"));

                    //Click the Label of the Process Stage if found
                    foreach (var label in labels)
                    {
                        if(label.Text.Equals(stageName,StringComparison.OrdinalIgnoreCase))
                        {
                            label.Click();
                        }
                    }
                }

                var flyoutFooterControls = driver.FindElements(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.Flyout_UCI]));
                
                foreach (var control in flyoutFooterControls)
                {
                    //If there's a field to enter, fill it out
                    if(businessProcessFlowField != null)
                    {
                        var bpfField = control.FindElement(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.BusinessProcessFlowFieldName].Replace("[NAME]", businessProcessFlowField.Name)));

                        if (bpfField != null)
                        {
                            bpfField.Click();
                            for (int i = 0; i < businessProcessFlowField.Value.Length; i++)
                            {
                                bpfField.SendKeys(businessProcessFlowField.Value.Substring(i, 1));
                            }
                        }
                    }

                    //Click the Next Stage Button
                    var nextButton = control.FindElement(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.NextStageButton]));
                    nextButton.Click();
                }
               
                return true;
            });
        }
        #endregion

        #region GlobalSearch
        /// <summary>
        /// Searches for the specified criteria in Global Search.
        /// </summary>
        /// <param name="criteria">Search criteria.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param> time.</param>
        /// <example>xrmBrowser.GlobalSearch.Search("Contoso");</example>
        internal BrowserCommandResult<bool> GlobalSearch(string criteria, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Global Search: {criteria}"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Navigation.SearchButton]),
                new TimeSpan(0, 0, 5),
                d => { driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Navigation.SearchButton])); },
                d => { throw new InvalidOperationException("The Global Search button is not available."); });


                if (driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Text])))
                {
                    var button = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Button]));
                    var input = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Text]));

                    if (button != null && input != null)
                    {
                        input.SendKeys(criteria, true);
                        button.Click();
                    }
                    else
                    {
                        throw new InvalidOperationException("The Global Search text field is not available.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("The Global Search is not available.");
                }
                return true;
            });
        }

        /// <summary>
        /// Filter by entity in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to filter with.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.GlobalSearch.FilterWith("Account");</example>
        public BrowserCommandResult<bool> FilterWith(string entity, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Filter With: {entity}"), driver =>
            {
                if (!driver.HasElement(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Filter])))
                    throw new InvalidOperationException("Filter With picklist is not available");

                var picklist = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Filter]));
                var options = picklist.FindElements(By.TagName("option"));

                picklist.Click();

                IWebElement option = options.FirstOrDefault(x => x.Text == entity);

                if (option == null)
                    throw new InvalidOperationException($"Entity '{entity}' does not exist in the Filter options.");

                option.Click();

                return true;
            });
        }

        /// <summary>
        /// Opens the specified record in the Global Search Results.
        /// </summary>
        /// <param name="entity">The entity you want to open a record.</param>
        /// <param name="index">The index of the record you want to open.</param>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param> time.</param>
        /// <example>xrmBrowser.GlobalSearch.OpenRecord("Accounts",0);</example>
        public BrowserCommandResult<bool> OpenGlobalSearchRecord(string entity, int index, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Open Global Search Record"), driver =>
            {
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Container]),
                                        Constants.DefaultTimeout,
                                        null,
                                        d => { throw new InvalidOperationException("Search Results is not available"); });

                                
                var resultsContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Container]));

                var entityContainer = resultsContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.EntityContainer].Replace("[NAME]", entity)));

                if (entityContainer == null)
                    throw new InvalidOperationException($"Entity {entity} was not found in the results");

                var records = entityContainer.FindElements(By.XPath(AppElements.Xpath[AppReference.GlobalSearch.Records]));

                if (records == null)
                    throw new InvalidOperationException($"No records found for entity {entity}");

                records[index].Click();
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Entity.Form]),
                    new TimeSpan(0, 0, 30),
                    null,
                    d => { throw new Exception("CRM Record is Unavailable or not finished loading. Timeout Exceeded"); }
                );

                return true;
            });
        }
        #endregion
        #region Dashboard
        internal BrowserCommandResult<bool> SelectDashboard(string dashboardName, int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions($"Select Dashboard"), driver =>
            {
                //Click the drop-down arrow
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dashboard.DashboardSelector]));
                //Select the dashboard
                driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dashboard.DashboardItemUCI].Replace("[NAME]", dashboardName)));

                return true;
            });
        }
        #endregion

        internal void ThinkTime(int milliseconds)
        {
            Browser.ThinkTime(milliseconds);
        }
        internal void Dispose()
        {
            Browser.Dispose();
        }
    }
}

