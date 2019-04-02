// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Xrm Notfication Page
    /// </summary>
    public class Notfication
        : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notfication"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Notfication(InteractiveBrowser browser)
            : base(browser)
        {
        }

        /// <summary>
        /// Closes the Notifications
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        /// <example>xrmBrowser.Notifications.CloseNotifications();</example>
        public BrowserCommandResult<bool> CloseNotifications(int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute("Close Notifications", driver =>
            {
                bool returnValue = false;

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Notification.AppMessageBar]), new TimeSpan(0, 0, 5), d =>
                {
                    var container = driver.FindElement(By.XPath(Elements.Xpath[Reference.Notification.AppMessageBar]));
                    var rows = container.FindElements(By.ClassName(Elements.CssClass[Reference.Notification.MessageBarRow]));

                    foreach(var row in rows)
                    {
                        var dismissButtonElement = row.FindElement(By.ClassName(Elements.CssClass[Reference.Notification.MessageBarButtonContainer]));
                        var dismissButton = dismissButtonElement.FindElement(By.TagName("a"));

                        dismissButton.Click();

                        returnValue = true;
                    }
                });

                return returnValue;
            });
        }

        public BrowserCommandResult<bool> Close(AppNotification notification)
        {
            return Close(notification.Index);
        }

        /// <summary>
        /// Dismiss App Notification
        /// </summary>
        /// <param name="index">The index</param>
        public BrowserCommandResult<bool> Close(Int32 index)
        {
            return this.Execute("Dismiss App Notification", driver =>
            {
                bool returnValue = false;

                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Notification.AppMessageBar]), new TimeSpan(0, 0, 5), d =>
                {
                    var container = driver.FindElement(By.XPath(Elements.Xpath[Reference.Notification.AppMessageBar]));
                    var rows = container.FindElements(By.ClassName(Elements.CssClass[Reference.Notification.MessageBarRow]));

                    if (rows.Count > index)
                    {
                        var row = rows[index];
                        var dismissButtonElement = row.FindElement(By.ClassName(Elements.CssClass[Reference.Notification.MessageBarButtonContainer]));
                        var dismissButton = dismissButtonElement.FindElement(By.TagName("a"));

                        dismissButton.Click();

                        returnValue = true;
                    }
                });

                return returnValue;
            });
        }

        /// <summary>
        /// Get App Messages
        /// </summary>
        public BrowserCommandResult<List<AppNotification>> Notifications
        {
            get
            {
                return this.Execute("Get App Messages", driver =>
                {
                    var returnList = new List<AppNotification>();
                    var index = 0;

                    driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Notification.AppMessageBar]), new TimeSpan(0, 0, 5), d =>
                    {
                        var container = driver.FindElement(By.XPath(Elements.Xpath[Reference.Notification.AppMessageBar]));
                        var rows = container.FindElements(By.ClassName(Elements.CssClass[Reference.Notification.MessageBarRow]));

                        foreach (var row in rows)
                        {
                            var titleElement = row.FindElement(By.ClassName(Elements.CssClass[Reference.Notification.MessageBarTitle]));
                            var messageElement = row.FindElement(By.ClassName(Elements.CssClass[Reference.Notification.MessageBarMessage]));
                            var dismissButtonElement = row.FindElement(By.ClassName(Elements.CssClass[Reference.Notification.MessageBarButtonContainer]));

                            var newItem = new AppNotification(this)
                            {
                                Index = index,
                                Title = titleElement.Text,
                                Message = messageElement.Text,
                                DismissButtonText = dismissButtonElement.Text
                            };

                            returnList.Add(newItem);

                            index++;
                        }
                    });

                    return returnList;
                });
            }
        }
    }
}