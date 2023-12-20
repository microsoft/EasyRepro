// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Lookup : Element
    {
        #region DTO
        public static class LookupReference
        {
            public static string RelatedEntityLabel = "//li[contains(@aria-label,'[NAME]') and contains(@data-id,'LookupResultsDropdown')]";
            public static string AdvancedLookupButton = "//button[.//label[text()='Advanced lookup']]";
            public static string ViewRows = "//li[contains(@data-id,'viewLineContainer')]";
            public static string LookupResultRows = "//li[contains(@data-id,'LookupResultsDropdown') and contains(@data-id,'resultsContainer')]";
            public static string NewButton = "//button[contains(@data-id,'addNewBtnContainer') and contains(@data-id,'LookupResultsDropdown')]";
            public static string RecordList = ".//div[contains(@id,'RecordList') and contains(@role,'presentation')]";
        }

        public static class AdvancedLookupReference
        {
            public static string Container = ".//div[contains(@data-lp-id, 'MscrmControls.FieldControls.AdvancedLookupControl')]";
            public static string SearchInput = "//input[@type='text' and @placeholder='Search']";
            public static string ResultRows = "//div[@ref='eLeftContainer']//div[@role='row']";
            public static string FilterTables = "//li[@role='listitem']//button";
            public static string FilterTable = "//li[@role='listitem']//button[.//*[text()='[NAME]']]";
            public static string AddNewTables = "//ul[@role='menu']//button";
            public static string DoneButton = "//button[.//*[text()='Done']]";
            public static string AddNewRecordButton = "//button[.//*[text()='Add new record']]";
            public static string AddNewButton = "//button[.//*[text()='Add new']]";
            public static string ViewSelectorCaret = ".//span[contains(@class, 'ms-Dropdown-caretDownWrapper')]";
            public static string ViewDropdownList = ".//div[contains(@class, 'dropdownItemsWrapper')]";
            public static string ViewDropdownListItem = "//button[.//span[text()='[NAME]']]";
        }
        #endregion
        private readonly WebClient _client;

        public Lookup(WebClient client) : base()
        {
            _client = client;
        }
        #region public
        /// <summary>
        /// Opens a record from a lookup control
        /// </summary>
        /// <param name="index">Index of the row to open</param>
        public void OpenRecord(int index)
        {
            this.OpenLookupRecord(index);
        }

        /// <summary>
        /// Clicks the New button in a lookup control
        /// </summary>
        public void New()
        {
            this.SelectLookupNewButton();
        }

        /// <summary>
        /// Searches records in a lookup control
        /// </summary>
        /// <param name="control">LookupItem with the name of the lookup field</param>
        /// <param name="searchCriteria">Criteria used for search</param>
        public void Search(LookupItem control, string searchCriteria)
        {
            this.SearchLookupField(control, searchCriteria);
        }

        /// <summary>
        /// Selects a related entity on a lookup control
        /// </summary>
        /// <param name="entityLabel">Name of the entity to select</param>
        public void SelectRelatedEntity(string entityLabel)
        {
            this.SelectLookupRelatedEntity(entityLabel);
        }

        /// <summary>
        /// Clicks the "Change View" button on a lookup control and selects the view provided
        /// </summary>
        /// <param name="viewName">Name of the view to select</param>
        public void SwitchView(string viewName)
        {
            this.SwitchLookupView(viewName);
        }
        #endregion
        #region Lookup 

        internal BrowserCommandResult<bool> OpenLookupRecord(int index)
        {
            return _client.Execute(_client.GetOptions("Select Lookup Record"), driver =>
            {
                driver.WaitForTransaction();

                ReadOnlyCollection<IWebElement> rows = null;
                if (driver.TryFindElement(By.XPath(AdvancedLookupReference.Container), out var advancedLookup))
                {
                    // Advanced Lookup
                    rows = driver.FindElements(By.XPath(AdvancedLookupReference.ResultRows));
                }
                else
                {
                    // Lookup
                    rows = driver.FindElements(By.XPath(LookupReference.LookupResultRows));
                }

                if (rows == null || !rows.Any())
                {
                    throw new NotFoundException("No rows found");
                }

                var row = rows.ElementAt(index);

                if (advancedLookup == null)
                {
                    row.Click();
                }
                else
                {
                    if (!row.GetAttribute<bool?>("aria-selected").GetValueOrDefault())
                    {
                        row.Click();
                    }

                    advancedLookup.FindElement(By.XPath(AdvancedLookupReference.DoneButton)).Click();
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SearchLookupField(LookupItem control, string searchCriteria)
        {
            return _client.Execute(_client.GetOptions("Search Lookup Record"), driver =>
            {
                driver.WaitForTransaction();

                if (driver.TryFindElement(By.XPath(AdvancedLookupReference.Container), out var advancedLookup))
                {
                    // Advanced lookup
                    var search = advancedLookup.FindElement(By.XPath(AdvancedLookupReference.SearchInput));
                    search.Click();
                    search.SendKeys(Keys.Control + "a");
                    search.SendKeys(Keys.Backspace);
                    search.SendKeys(searchCriteria);

                    driver.WaitForTransaction();

                    OpenLookupRecord(0);
                }
                else
                {
                    // Lookup
                    control.Value = searchCriteria;
                    _client.SetValue(control, FormContextType.Entity);
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupRelatedEntity(string entityName)
        {
            // Click the Related Entity on the Lookup Flyout
            return _client.Execute(_client.GetOptions($"Select Lookup Related Entity {entityName}"), driver =>
            {
                driver.WaitForTransaction();

                IWebElement relatedEntity = null;
                if (driver.TryFindElement(By.XPath(AdvancedLookupReference.Container), out var advancedLookup))
                {
                    // Advanced lookup
                    relatedEntity = advancedLookup.WaitUntilAvailable(
                        By.XPath(AdvancedLookupReference.FilterTable.Replace("[NAME]", entityName)),
                        2.Seconds());
                }
                else
                {
                    // Lookup 
                    relatedEntity = driver.WaitUntilAvailable(
                        By.XPath(LookupReference.RelatedEntityLabel.Replace("[NAME]", entityName)),
                        2.Seconds());
                }

                if (relatedEntity == null)
                {
                    throw new NotFoundException($"Lookup Entity {entityName} not found.");
                }

                relatedEntity.Click();
                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SwitchLookupView(string viewName)
        {
            return _client.Execute(_client.GetOptions($"Select Lookup View {viewName}"), driver =>
            {
                var advancedLookup = driver.WaitUntilAvailable(
                    By.XPath(AdvancedLookupReference.Container),
                    2.Seconds());

                if (advancedLookup == null)
                {
                    SelectLookupAdvancedLookupButton();
                    advancedLookup = driver.WaitUntilAvailable(
                        By.XPath(AdvancedLookupReference.Container),
                        2.Seconds(),
                        "Expected Advanced Lookup dialog but it was not found.");
                }

                advancedLookup
                    .FindElement(By.XPath(AdvancedLookupReference.ViewSelectorCaret))
                    .Click();

                driver
                    .WaitUntilAvailable(By.XPath(AdvancedLookupReference.ViewDropdownList))
                    .ClickWhenAvailable(
                     By.XPath(AdvancedLookupReference.ViewDropdownListItem.Replace("[NAME]", viewName)),
                     2.Seconds(),
                     $"The '{viewName}' view isn't in the list of available lookup views.");

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupAdvancedLookupButton()
        {
            return _client.Execute(_client.GetOptions("Click Advanced Lookup Button"), driver =>
            {
                driver.ClickWhenAvailable(
                    By.XPath(LookupReference.AdvancedLookupButton),
                    10.Seconds(),
                    "The 'Advanced Lookup' button was not found. Ensure a search has been performed in the lookup first.");

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupNewButton()
        {
            return _client.Execute(_client.GetOptions("Click New Lookup Button"), driver =>
            {
                driver.WaitForTransaction();

                if (driver.TryFindElement(By.XPath(AdvancedLookupReference.Container), out var advancedLookup))
                {
                    // Advanced lookup
                    if (advancedLookup.TryFindElement(By.XPath(AdvancedLookupReference.AddNewRecordButton), out var addNewRecordButton))
                    {
                        // Single table lookup
                        addNewRecordButton.Click();
                    }
                    else if (advancedLookup.TryFindElement(By.XPath(AdvancedLookupReference.AddNewButton), out var addNewButton))
                    {
                        // Composite lookup
                        var filterTables = advancedLookup.FindElements(By.XPath(AdvancedLookupReference.FilterTables)).ToList();
                        var tableIndex = filterTables.FindIndex(t => t.HasAttribute("aria-current"));

                        addNewButton.Click();
                        driver.WaitForTransaction();

                        var addNewTables = advancedLookup.FindElements(By.XPath(AdvancedLookupReference.AddNewTables));
                        addNewTables.ElementAt(tableIndex).Click();
                    }
                }
                else
                {
                    // Lookup
                    if (driver.HasElement(By.XPath(LookupReference.NewButton)))
                    {
                        var newButton = driver.FindElement(By.XPath(LookupReference.NewButton));

                        if (newButton.GetAttribute("disabled") == null)
                            driver.FindElement(By.XPath(LookupReference.NewButton)).Click();
                        else
                            throw new ElementNotInteractableException("New button is not enabled.  If this is a mulit-entity lookup, please use SelectRelatedEntity first.");
                    }
                    else
                        throw new NotFoundException("New button not found.");
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        internal BrowserCommandResult<IReadOnlyList<FormNotification>> GetFormNotifications()
        {
            return _client.Execute(_client.GetOptions($"Get all form notifications"), driver =>
            {
                List<FormNotification> notifications = new List<FormNotification>();

                // Look for notificationMessageAndButtons bar
                var notificationMessage = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormMessageBar), TimeSpan.FromSeconds(2));

                if (notificationMessage != null)
                {
                    IWebElement icon = null;

                    try
                    {
                        icon = driver.FindElement(By.XPath(Entity.EntityReference.FormMessageBarTypeIcon));
                    }
                    catch (NoSuchElementException)
                    {
                        // Swallow the exception
                    }

                    if (icon != null)
                    {
                        var notification = new FormNotification
                        {
                            Message = notificationMessage?.Text
                        };
                        string classes = icon.GetAttribute("class");
                        notification.SetTypeFromClass(classes);
                        notifications.Add(notification);
                    }
                }

                // Look for the notification wrapper, if it doesn't exist there are no notificatios
                var notificationBar = driver.WaitUntilVisible(By.XPath(Entity.EntityReference.FormNotifcationBar), TimeSpan.FromSeconds(2));
                if (notificationBar == null)
                    return notifications;
                else
                {
                    // If there are multiple notifications, the notifications must be expanded first.
                    if (notificationBar.TryFindElement(By.XPath(Entity.EntityReference.FormNotifcationExpandButton), out var expandButton))
                    {
                        if (!Convert.ToBoolean(notificationBar.GetAttribute("aria-expanded")))
                            expandButton.Click();

                        // After expansion the list of notifications are now in a different element
                        notificationBar = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormNotifcationFlyoutRoot), TimeSpan.FromSeconds(2), "Failed to open the form notifications");
                    }

                    var notificationList = notificationBar.FindElement(By.XPath(Entity.EntityReference.FormNotifcationList));
                    var notificationListItems = notificationList.FindElements(By.TagName("li"));

                    foreach (var item in notificationListItems)
                    {
                        var icon = item.FindElement(By.XPath(Entity.EntityReference.FormNotifcationTypeIcon));

                        var notification = new FormNotification
                        {
                            Message = item.Text
                        };
                        string classes = icon.GetAttribute("class");
                        notification.SetTypeFromClass(classes);
                        notifications.Add(notification);
                    }

                    if (notificationBar != null)
                    {
                        notificationBar = driver.WaitUntilVisible(By.XPath(Entity.EntityReference.FormNotifcationBar), TimeSpan.FromSeconds(2));
                        notificationBar.Click(true); // Collapse the notification bar
                    }
                    return notifications;
                }

            }).Value;
        }

        #endregion

    }
}
