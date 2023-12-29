// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.Entity;

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

        internal BrowserCommandResult<bool> RemoveValues(LookupItem[] controls)
        {
            return _client.Execute(_client.GetOptions($"Remove values {controls.First().Name}"), driver =>
            {
                foreach (var control in controls)
                    this.ClearValue(control, FormContextType.Entity, false);

                return true;
            });
        }
        public void TrySetValue(IWebDriver driver, IWebElement fieldContainer, LookupItem control)
        {
            IWebElement input;
            bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);
            string value = control.Value?.Trim();
            if (found)
                Field.SetInputValue(driver, input, value);

            TrySetValue(driver, control);
        }

        public void TryToSetValue(IWebDriver driver, ISearchContext fieldContainer, LookupItem[] controls)
        {
            IWebElement input;
            bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);

            foreach (var control in controls)
            {
                var value = control.Value?.Trim();
                if (found)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        input.Click();
                    else
                    {
                        input.SendKeys(value, true);
                        driver.WaitForTransaction();
                        _client.ThinkTime(3.Seconds());
                        input.SendKeys(Keys.Tab);
                        input.SendKeys(Keys.Enter);
                    }
                }

                TrySetValue(fieldContainer, control);
            }

            input.SendKeys(Keys.Escape); // IE wants to keep the flyout open on multi-value fields, this makes sure it closes
        }

        public void TrySetValue(ISearchContext container, LookupItem control)
        {
            string value = control.Value;
            if (value == null)
                control.Value = string.Empty;
            // throw new InvalidOperationException($"No value has been provided for the LookupItem {control.Name}. Please provide a value or an empty string and try again.");

            if (control.Value == string.Empty)
                SetLookupByIndex(container, control);
            else
                SetLookUpByValue(container, control);
        }
        private void SetLookUpByValue(ISearchContext container, LookupItem control)
        {
            var controlName = control.Name;
            var xpathToText = EntityReference.LookupFieldNoRecordsText.Replace("[NAME]", controlName);
            var xpathToResultList = EntityReference.LookupFieldResultList.Replace("[NAME]", controlName);
            var bypathResultList = By.XPath(xpathToText + "|" + xpathToResultList);

            container.WaitUntilAvailable(bypathResultList, TimeSpan.FromSeconds(10));

            var byPathToFlyout = By.XPath(EntityReference.TextFieldLookupMenu.Replace("[NAME]", controlName));
            var flyoutDialog = container.WaitUntilClickable(byPathToFlyout);

            var items = GetListItems(flyoutDialog, control);

            if (items.Count == 0)
                throw new InvalidOperationException($"List does not contain a record with the name:  {control.Value}");

            int index = control.Index;
            if (index >= items.Count)
                throw new InvalidOperationException($"List does not contain {index + 1} records. Please provide an index value less than {items.Count} ");

            var selectedItem = items.ElementAt(index);
            selectedItem.Click(true);
        }
        private void SetLookupByIndex(ISearchContext container, LookupItem control)
        {
            var controlName = control.Name;
            var xpathToControl = By.XPath(EntityReference.LookupResultsDropdown.Replace("[NAME]", controlName));
            var lookupResultsDialog = container.WaitUntilVisible(xpathToControl);

            var xpathFieldResultListItem = By.XPath(EntityReference.LookupFieldResultListItem.Replace("[NAME]", controlName));
            container.WaitUntil(d => d.FindElements(xpathFieldResultListItem).Count > 0);


            var items = GetListItems(lookupResultsDialog, control);
            if (items.Count == 0)
                throw new InvalidOperationException($"No results exist in the Recently Viewed flyout menu. Please provide a text value for {controlName}");

            int index = control.Index;
            if (index >= items.Count)
                throw new InvalidOperationException($"Recently Viewed list does not contain {index} records. Please provide an index value less than {items.Count}");

            var selectedItem = items.ElementAt(index);
            selectedItem.Click(true);
        }
        public void TryRemoveLookupValue(IWebDriver driver, IWebElement fieldContainer, LookupItem control, bool removeAll = true, bool isHeader = false)
        {
            var controlName = control.Name;
            fieldContainer.Hover(driver);

            var xpathDeleteExistingValues = By.XPath(Entity.EntityReference.LookupFieldDeleteExistingValue.Replace("[NAME]", controlName));
            var existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);

            var xpathToExpandButton = By.XPath(Entity.EntityReference.LookupFieldExpandCollapseButton.Replace("[NAME]", controlName));
            bool success = fieldContainer.TryFindElement(xpathToExpandButton, out var expandButton);
            if (success)
            {
                expandButton.Click(true);

                var count = existingValues.Count;
                fieldContainer.WaitUntil(x => x.FindElements(xpathDeleteExistingValues).Count > count);
            }

            fieldContainer.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldLookupSearchButton.Replace("[NAME]", controlName)));

            existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);
            if (existingValues.Count == 0)
                return;

            if (removeAll)
            {
                // Removes all selected items

                while (existingValues.Count > 0)
                {
                    foreach (var v in existingValues)
                        v.Click(true);

                    existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);
                }

                return;
            }

            // Removes an individual item by value or index
            var value = control.Value;
            if (value == null)
                throw new InvalidOperationException($"No value or index has been provided for the LookupItem {controlName}. Please provide an value or an empty string or an index and try again.");

            if (value == string.Empty)
            {
                var index = control.Index;
                if (index >= existingValues.Count)
                    throw new InvalidOperationException($"Field '{controlName}' does not contain {index + 1} records. Please provide an index value less than {existingValues.Count}");

                existingValues[index].Click(true);
                return;
            }

            var existingValue = existingValues.FirstOrDefault(v => v.GetAttribute("aria-label").EndsWith(value));
            if (existingValue == null)
                throw new InvalidOperationException($"Field '{controlName}' does not contain a record with the name:  {value}");

            existingValue.Click(true);
            driver.WaitForTransaction();
        }
        /// <summary>
        /// Sets the value of a Lookup, Customer, Owner or ActivityParty Lookup which accepts only a single value.
        /// </summary>
        /// <param name="control">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new Lookup { Name = "prrimarycontactid", Value = "Rene Valdes (sample)" });</example>
        /// The default index position is 0, which will be the first result record in the lookup results window. Suppy a value > 0 to select a different record if multiple are present.
        internal BrowserCommandResult<bool> SetValue(LookupItem control, FormContextType formContextType)
        {
            return _client.Execute(_client.GetOptions($"Set Lookup Value: {control.Name}"), driver =>
            {
                driver.WaitForTransaction();

                IWebElement fieldContainer = null;
                fieldContainer = _client.ValidateFormContext(driver, formContextType, control.Name, fieldContainer);

                TryRemoveLookupValue(driver, fieldContainer, control);
                TrySetValue(driver, fieldContainer, control);

                return true;
            });
        }
        /// <summary>
        /// Sets the value of an ActivityParty Lookup.
        /// </summary>
        /// <param name="controls">The lookup field name, value or index of the lookup.</param>
        /// <example>xrmApp.Entity.SetValue(new Lookup[] { Name = "to", Value = "Rene Valdes (sample)" }, { Name = "to", Value = "Alpine Ski House (sample)" } );</example>
        /// The default index position is 0, which will be the first result record in the lookup results window. Suppy a value > 0 to select a different record if multiple are present.
        internal BrowserCommandResult<bool> SetValue(LookupItem[] controls, FormContextType formContextType = FormContextType.Entity, bool clearFirst = true)
        {
            var control = controls.First();
            var controlName = control.Name;
            return _client.Execute(_client.GetOptions($"Set ActivityParty Lookup Value: {controlName}"), driver =>
            {
                driver.WaitForTransaction();

                IWebElement fieldContainer = null;
                fieldContainer = _client.ValidateFormContext(driver, formContextType, controlName, fieldContainer);

                if (clearFirst)
                   TryRemoveLookupValue(driver, fieldContainer, control);

                TryToSetValue(driver, fieldContainer, controls);

                return true;
            });
        }

        internal BrowserCommandResult<bool> AddValues(LookupItem[] controls)
        {
            return _client.Execute(_client.GetOptions($"Add values {controls.First().Name}"), driver =>
            {
                this.SetValue(controls, FormContextType.Entity, false);

                return true;
            });
        }


        internal BrowserCommandResult<bool> ClearValue(LookupItem control, FormContextType formContextType, bool removeAll = true)
        {
            var controlName = control.Name;
            return _client.Execute(_client.GetOptions($"Clear Field {controlName}"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldLookupFieldContainer.Replace("[NAME]", controlName)));
                TryRemoveLookupValue(driver, fieldContainer, control, removeAll);
                return true;
            });
        }

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
                    this.SetValue(control, FormContextType.Entity);
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
