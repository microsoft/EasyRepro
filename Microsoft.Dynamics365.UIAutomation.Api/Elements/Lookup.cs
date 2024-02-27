// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Api.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static Microsoft.Dynamics365.UIAutomation.Api.Entity;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class Lookup 
    {
        #region DTO
        public class LookupReference
        {
            public const string Lookup = "Lookup";
            private string _relatedEntityLabel = "//li[contains(@aria-label,'[NAME]') and contains(@data-id,'LookupResultsDropdown')]";
            private string _advancedLookupButton = "//button[.//label[text()='Advanced lookup']]";
            private string _viewRows = "//li[contains(@data-id,'viewLineContainer')]";
            private string _lookupResultRows = "//li[contains(@data-id,'LookupResultsDropdown') and contains(@data-id,'resultsContainer')]";
            private string _newButton = "//button[contains(@data-id,'addNewBtnContainer') and contains(@data-id,'LookupResultsDropdown')]";
            private string _recordList = ".//div[contains(@id,'RecordList') and contains(@role,'presentation')]";

            public string RelatedEntityLabel { get => _relatedEntityLabel; set { _relatedEntityLabel = value; } }
            public string AdvancedLookupButton { get => _advancedLookupButton; set { _advancedLookupButton = value; } }
            public string ViewRows { get => _viewRows; set { _viewRows = value; } }
            public string LookupResultRows { get => _lookupResultRows; set { _lookupResultRows = value; } }
            public string NewButton { get => _newButton; set { _newButton = value; } }
            public string RecordList { get => _recordList; set { _recordList = value; } }
        }

        public class AdvancedLookupReference
        {
            public const string AdvancedLookup = "AdvancedLookup";
            private string _container = ".//div[contains(@data-lp-id, 'MscrmControls.FieldControls.AdvancedLookupControl')]";
            private string _searchInput = "//input[@type='text' and @placeholder='Search']";
            private string _resultRows = "//div[@ref='eLeftContainer']//div[@role='row']";
            private string _filterTables = "//li[@role='listitem']//button";
            private string _filterTable = "//li[@role='listitem']//button[.//*[text()='[NAME]']]";
            private string _addNewTables = "//ul[@role='menu']//button";
            private string _doneButton = "//button[.//*[text()='Done']]";
            private string _addNewRecordButton = "//button[.//*[text()='Add new record']]";
            private string _addNewButton = "//button[.//*[text()='Add new']]";
            private string _viewSelectorCaret = ".//span[contains(@class, 'ms-Dropdown-caretDownWrapper')]";
            private string _viewDropdownList = ".//div[contains(@class, 'dropdownItemsWrapper')]";
            private string _viewDropdownListItem = "//button[.//span[text()='[NAME]']]";

            public string Container { get => _container; set { _container = value; } }
            public string SearchInput { get => _searchInput; set { _searchInput = value; } }
            public string ResultRows { get => _resultRows; set { _resultRows = value; } }
            public string FilterTables { get => _filterTables; set { _filterTables = value; } }
            public string FilterTable { get => _filterTable; set { _filterTable = value; } }
            public string AddNewTables { get => _addNewTables; set { _addNewTables = value; } }
            public string DoneButton { get => _doneButton; set { _doneButton = value; } }
            public string AddNewRecordButton { get => _addNewRecordButton; set { _addNewRecordButton = value; } }
            public string AddNewButton { get => _addNewButton; set { _addNewButton = value; } }
            public string ViewSelectorCaret { get => _viewSelectorCaret; set { _viewSelectorCaret = value; } }
            public string ViewDropdownList { get => _viewDropdownList; set { _viewDropdownList = value; } }
            public string ViewDropdownListItem { get => _viewDropdownListItem; set { _viewDropdownListItem = value; } }

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
        public void TrySetValue(IWebBrowser driver, IElement fieldContainer, LookupItem control)
        {
            IElement input;
            bool found = driver.HasElement(fieldContainer + "//input");
            input = driver.FindElement(fieldContainer + "//input");
            string value = control.Value?.Trim();
            if (found)
            {
                Field objField = new Field(_client);
                objField.SetInputValue(driver, input, value);
            }

            TrySetValue(driver, fieldContainer, control);
        }

        public void TryToSetValue(IWebBrowser driver, IElement fieldContainer, LookupItem[] controls)
        {
            IElement input;
            bool found = driver.HasElement(fieldContainer.Locator + "//input");
            input = driver.FindElement(fieldContainer.Locator + "//input");
            foreach (var control in controls)
            {
                var value = control.Value?.Trim();
                if (found)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        input.Click(_client);
                    else
                    {
                        input.SendKeys(_client, new string[] { value });
                        driver.Wait();
                        _client.ThinkTime(3.Seconds());
                        input.SendKeys(_client,new string[] { Keys.Tab });
                        input.SendKeys(_client,new string[] { Keys.Enter });
                    }
                }

                TrySetValue(fieldContainer, control);
            }

            input.SendKeys(_client, new string[] { Keys.Escape }); // IE wants to keep the flyout open on multi-value fields, this makes sure it closes
        }

        public void TrySetValue(IElement container, LookupItem control)
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
        private void SetLookUpByValue(IElement container, LookupItem control)
        {
            var controlName = control.Name;
            var xpathToText = _client.ElementMapper.EntityReference.LookupFieldNoRecordsText.Replace("[NAME]", controlName);
            var xpathToResultList = _client.ElementMapper.EntityReference.LookupFieldResultList.Replace("[NAME]", controlName);
            var bypathResultList = xpathToText + "|" + xpathToResultList;

            _client.Browser.Browser.WaitUntilAvailable(container.Locator + bypathResultList, TimeSpan.FromSeconds(10), "Cannot find lookup. XPath: '" + container.Locator + bypathResultList + "'");

            var byPathToFlyout = _client.ElementMapper.EntityReference.TextFieldLookupMenu.Replace("[NAME]", controlName);
            var flyoutDialog = _client.Browser.Browser.WaitUntilAvailable(container.Locator + byPathToFlyout);

            var items = GetListItems(flyoutDialog, control);

            if (items.Count == 0)
                throw new InvalidOperationException($"List does not contain a record with the name:  {control.Value}");

            int index = control.Index;
            if (index >= items.Count)
                throw new InvalidOperationException($"List does not contain {index + 1} records. Please provide an index value less than {items.Count} ");

            var selectedItem = items.ElementAt(index);
            selectedItem.Click(_client, true);
        }
        internal ICollection<IElement> GetListItems(IElement container, LookupItem control)
        {
            var name = control.Name;
            var xpathToItems = this._client.ElementMapper.EntityReference.LookupFieldResultListItem.Replace("[NAME]", name);

            //wait for complete the search
            var lookupResult = _client.Browser.Browser.WaitUntilAvailable(_client.ElementMapper.EntityReference.LookupFieldResultListItem, "Lookup Field Results not available with XPath: '" + xpathToItems + "'");
            if(lookupResult.Text.Contains(control.Value, StringComparison.OrdinalIgnoreCase) == true)
            {

            }
            //container.WaitUntil(d => d.FindVisible(xpathToItems)?.Text?.Contains(control.Value, StringComparison.OrdinalIgnoreCase) == true);
            ICollection<IElement> items = new List<IElement>();
            //ICollection<IElement> result = container.WaitUntil(
            //    d => d.FindElements(xpathToItems),
            //    failureCallback: () => throw new InvalidOperationException($"No Results Matching {control.Value} Were Found.")
            //    );
            return items;
        }
        private void SetLookupByIndex(IElement container, LookupItem control)
        {
            var controlName = control.Name;
            var xpathToControl = _client.ElementMapper.EntityReference.LookupResultsDropdown.Replace("[NAME]", controlName);
            var lookupResultsDialog = _client.Browser.Browser.WaitUntilAvailable(container.Locator + xpathToControl);

            var xpathFieldResultListItem = _client.ElementMapper.EntityReference.LookupFieldResultListItem.Replace("[NAME]", controlName);
            var lookupResult = _client.Browser.Browser.FindElements(container.Locator + xpathFieldResultListItem).Count;
            if (lookupResult > 0)
            {

            }
            //container.WaitUntil(d => d.FindElements(xpathFieldResultListItem).Count > 0);


            var items = GetListItems(lookupResultsDialog, control);
            if (items.Count == 0)
                throw new InvalidOperationException($"No results exist in the Recently Viewed flyout menu. Please provide a text value for {controlName}");

            int index = control.Index;
            if (index >= items.Count)
                throw new InvalidOperationException($"Recently Viewed list does not contain {index} records. Please provide an index value less than {items.Count}");

            var selectedItem = items.ElementAt(index);
            selectedItem.Click(_client, true);
        }
        public void TryRemoveLookupValue(IWebBrowser driver, IElement fieldContainer, LookupItem control, bool removeAll = true, bool isHeader = false)
        {
            Trace.TraceInformation("Lookup.TryRemoveLookupValue initated.");
            var controlName = control.Name;
            fieldContainer.Hover(_client, fieldContainer.Locator);

            Trace.TraceInformation("Lookup.TryRemoveLookupValue: Look for existing values.");
            var xpathDeleteExistingValues = _client.ElementMapper.EntityReference.LookupFieldDeleteExistingValue.Replace("[NAME]", controlName);
            var existingValues = driver.FindElements(fieldContainer.Locator + xpathDeleteExistingValues);

            if (driver.HasElement(fieldContainer.Locator + xpathDeleteExistingValues))
            {
                Trace.TraceInformation("Lookup.TryRemoveLookupValue: Existing values found for lookup " + control.Name);
            }

            var xpathToExpandButton = _client.ElementMapper.EntityReference.LookupFieldExpandCollapseButton.Replace("[NAME]", controlName);
            bool success = driver.HasElement(fieldContainer.Locator + xpathToExpandButton);
            var expandButton = driver.FindElement(fieldContainer.Locator + xpathToExpandButton);
            if (success)
            {
                Trace.TraceInformation("Lookup.TryRemoveLookupValue: Click expand button.");
                expandButton.Click(_client,true);

                var count = existingValues.Count;
                var removeLookupValue = driver.FindElements(fieldContainer.Locator + xpathDeleteExistingValues);
                if (removeLookupValue.Count < count) driver.Wait();
            }

            driver.WaitUntilAvailable(fieldContainer.Locator + _client.ElementMapper.EntityReference.TextFieldLookupSearchButton.Replace("[NAME]", controlName));

            existingValues = driver.FindElements(fieldContainer.Locator + xpathDeleteExistingValues);
            if (existingValues.Count == 0)
                return;

            if (removeAll)
            {
                Trace.TraceInformation("Lookup.TryRemoveLookupValue: Remove all selected items.");
                // Removes all selected items

                while (existingValues.Count > 0)
                {
                    foreach (var v in existingValues)
                        v.Click(_client,true);

                    existingValues = driver.FindElements(fieldContainer.Locator + xpathDeleteExistingValues);
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

                existingValues[index].Click(_client,true);
                return;
            }

            var existingValue = existingValues.FirstOrDefault(v => v.GetAttribute(_client,"aria-label").EndsWith(value));
            if (existingValue == null)
                throw new InvalidOperationException($"Field '{controlName}' does not contain a record with the name:  {value}");

            existingValue.Click(_client, true);
            driver.Wait();
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
                driver.Wait();

                IElement fieldContainer = null;
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
                driver.Wait();

                IElement fieldContainer = null;
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
                var fieldContainer = driver.WaitUntilAvailable(_client.ElementMapper.EntityReference.TextFieldLookupFieldContainer.Replace("[NAME]", controlName));
                TryRemoveLookupValue(driver, fieldContainer, control, removeAll);
                return true;
            });
        }

        internal BrowserCommandResult<bool> OpenLookupRecord(int index)
        {
            return _client.Execute(_client.GetOptions("Select Lookup Record"), driver =>
            {
                driver.Wait();

                List<IElement> rows = null;
                IElement advancedLookup = null;
                if (driver.HasElement(_client.ElementMapper.AdvancedLookupReference.Container))
                {
                    advancedLookup = driver.FindElement(_client.ElementMapper.AdvancedLookupReference.Container);
                    // Advanced Lookup
                    rows = driver.FindElements(_client.ElementMapper.AdvancedLookupReference.ResultRows);
                }
                else
                {
                    // Lookup
                    rows = driver.FindElements(_client.ElementMapper.LookupReference.LookupResultRows);
                }

                if (rows == null || !rows.Any())
                {
                    throw new KeyNotFoundException("No rows found");
                }

                var row = rows.ElementAt(index);

                if (advancedLookup == null)
                {
                    row.Click(_client);
                }
                else
                {
                    if (!Convert.ToBoolean(row.GetAttribute(_client,"aria-selected")))
                    {
                        row.Click(_client);
                    }

                    driver.FindElement(_client.ElementMapper.AdvancedLookupReference.Container + _client.ElementMapper.AdvancedLookupReference.DoneButton).Click(_client);
                }

                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SearchLookupField(LookupItem control, string searchCriteria)
        {
            return _client.Execute(_client.GetOptions("Search Lookup Record"), driver =>
            {
                driver.Wait();

                if (driver.HasElement(_client.ElementMapper.AdvancedLookupReference.Container))
                {
                    var advancedLookup = driver.FindElement(_client.ElementMapper.AdvancedLookupReference.Container);
                    // Advanced lookup
                    var search = driver.FindElement(advancedLookup.Locator + _client.ElementMapper.AdvancedLookupReference.SearchInput);
                    search.Click(_client);
                    search.SendKeys(_client,new string[] { Keys.Control, "a" });
                    search.SendKeys(_client, new string[] { Keys.Backspace });
                    search.SendKeys(_client, new string[] { searchCriteria });

                    driver.Wait();

                    OpenLookupRecord(0);
                }
                else
                {
                    // Lookup
                    control.Value = searchCriteria;
                    this.SetValue(control, FormContextType.Entity);
                }

                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupRelatedEntity(string entityName)
        {
            // Click the Related Entity on the Lookup Flyout
            return _client.Execute(_client.GetOptions($"Select Lookup Related Entity {entityName}"), driver =>
            {
            driver.Wait();

            IElement relatedEntity = null;
            if (driver.HasElement(_client.ElementMapper.AdvancedLookupReference.Container))
            {
                var advancedLookup = driver.FindElement(_client.ElementMapper.AdvancedLookupReference.Container);
                // Advanced lookup
                relatedEntity = driver.WaitUntilAvailable(
                    advancedLookup.Locator + _client.ElementMapper.AdvancedLookupReference.FilterTable.Replace("[NAME]", entityName),
                    2.Seconds(), "Cannot select lookup related entity. XPath: '" + advancedLookup.Locator + _client.ElementMapper.AdvancedLookupReference.FilterTable.Replace("[NAME]", entityName) + "'");
                }
                else
                {
                    // Lookup 
                    relatedEntity = driver.WaitUntilAvailable(
                        _client.ElementMapper.LookupReference.RelatedEntityLabel.Replace("[NAME]", entityName),
                        2.Seconds(), "Cannot select lookup related entity. XPath: '" + _client.ElementMapper.LookupReference.RelatedEntityLabel.Replace("[NAME]", entityName) + "'");
                }

                if (relatedEntity == null)
                {
                    throw new KeyNotFoundException($"Lookup Entity {entityName} not found.");
                }

                relatedEntity.Click(_client);
                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SwitchLookupView(string viewName)
        {
            return _client.Execute(_client.GetOptions($"Select Lookup View {viewName}"), driver =>
            {
                var advancedLookup = driver.WaitUntilAvailable(
                    _client.ElementMapper.AdvancedLookupReference.Container,
                    2.Seconds(), "Switch Lookup View is not available. XPath: '" + _client.ElementMapper.AdvancedLookupReference.Container + "'");

                if (advancedLookup == null)
                {
                    SelectLookupAdvancedLookupButton();
                    advancedLookup = driver.WaitUntilAvailable(
                        _client.ElementMapper.AdvancedLookupReference.Container,
                        2.Seconds(),
                        "Expected Advanced Lookup dialog but it was not found.");
                }   

                driver
                    .FindElement(_client.ElementMapper.AdvancedLookupReference.Container + _client.ElementMapper.AdvancedLookupReference.ViewSelectorCaret)
                    .Click(_client);

                var viewDropdownList = driver
                    .WaitUntilAvailable(_client.ElementMapper.AdvancedLookupReference.ViewDropdownList);

                    driver.ClickWhenAvailable(
                     _client.ElementMapper.AdvancedLookupReference.ViewDropdownListItem.Replace("[NAME]", viewName),
                     2.Seconds(),
                     $"The '{viewName}' view isn't in the list of available lookup views.");

                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupAdvancedLookupButton()
        {
            return _client.Execute(_client.GetOptions("Click Advanced Lookup Button"), driver =>
            {
                driver.ClickWhenAvailable(
                    _client.ElementMapper.LookupReference.AdvancedLookupButton,
                    10.Seconds(),
                    "The 'Advanced Lookup' button was not found. Ensure a search has been performed in the lookup first.");

                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<bool> SelectLookupNewButton()
        {
            return _client.Execute(_client.GetOptions("Click New Lookup Button"), driver =>
            {
                driver.Wait();

                if (driver.HasElement(_client.ElementMapper.AdvancedLookupReference.Container))
                {
                    var advancedLookup = driver.FindElement(_client.ElementMapper.AdvancedLookupReference.Container);
                    // Advanced lookup
                    if (driver.HasElement(advancedLookup.Locator + _client.ElementMapper.AdvancedLookupReference.AddNewRecordButton))
                    {
                        var addNewRecordButton = driver.FindElement(advancedLookup.Locator + _client.ElementMapper.AdvancedLookupReference.AddNewRecordButton);
                        // Single table lookup
                        addNewRecordButton.Click(_client);
                    }
                    else if (driver.HasElement(advancedLookup.Locator + _client.ElementMapper.AdvancedLookupReference.AddNewButton))
                    {
                        var addNewButton = driver.FindElement(advancedLookup.Locator + _client.ElementMapper.AdvancedLookupReference.AddNewButton);
                        // Composite lookup
                        var filterTables = driver.FindElements(advancedLookup.Locator + _client.ElementMapper.AdvancedLookupReference.FilterTables).ToList();
                        var tableIndex = filterTables.FindIndex(t => t.HasAttribute(_client,"aria-current"));

                        addNewButton.Click(_client);
                        driver.Wait();

                        var addNewTables = driver.FindElements(advancedLookup.Locator + _client.ElementMapper.AdvancedLookupReference.AddNewTables);
                        addNewTables.ElementAt(tableIndex).Click(_client);
                    }
                }
                else
                {
                    // Lookup
                    if (driver.HasElement(_client.ElementMapper.LookupReference.NewButton))
                    {
                        var newButton = driver.FindElement(_client.ElementMapper.LookupReference.NewButton);

                        if (newButton.GetAttribute(_client, "disabled") == null)
                            driver.FindElement(_client.ElementMapper.LookupReference.NewButton).Click(_client);
                        else
                            throw new Exception("New button is not enabled.  If this is a mulit-entity lookup, please use SelectRelatedEntity first.");
                    }
                    else
                        throw new KeyNotFoundException("New button not found.");
                }

                driver.Wait();

                return true;
            });
        }

        internal BrowserCommandResult<IReadOnlyList<FormNotification>> GetFormNotifications()
        {
            return _client.Execute(_client.GetOptions($"Get all form notifications"), driver =>
            {
                List<FormNotification> notifications = new List<FormNotification>();

                // Look for notificationMessageAndButtons bar
                var notificationMessage = driver.WaitUntilAvailable(_client.ElementMapper.EntityReference.FormMessageBar, TimeSpan.FromSeconds(2), "Cannot Get Form Notification. XPath: '" + _client.ElementMapper.EntityReference.FormMessageBar + "'");

                if (notificationMessage != null)
                {
                    IElement icon = null;

                    try
                    {
                        icon = driver.FindElement(_client.ElementMapper.EntityReference.FormMessageBarTypeIcon);
                    }
                    catch (Exception)
                    {
                        // Swallow the exception
                    }

                    if (icon != null)
                    {
                        var notification = new FormNotification
                        {
                            Message = notificationMessage?.Text
                        };
                        string classes = icon.GetAttribute(_client, "class");
                        notification.SetTypeFromClass(classes);
                        notifications.Add(notification);
                    }
                }

                // Look for the notification wrapper, if it doesn't exist there are no notificatios
                var notificationBar = driver.WaitUntilAvailable(_client.ElementMapper.EntityReference.FormNotifcationBar, TimeSpan.FromSeconds(2), "");
                if (notificationBar == null)
                    return notifications;
                else
                {
                    // If there are multiple notifications, the notifications must be expanded first.
                    if (driver.HasElement(notificationBar.Locator + _client.ElementMapper.EntityReference.FormNotifcationExpandButton))
                    {
                        var expandButton = driver.FindElement(notificationBar.Locator + _client.ElementMapper.EntityReference.FormNotifcationExpandButton);
                        if (!Convert.ToBoolean(notificationBar.GetAttribute(_client, "aria-expanded")))
                            expandButton.Click(_client);

                        // After expansion the list of notifications are now in a different IElement
                        notificationBar = driver.WaitUntilAvailable(_client.ElementMapper.EntityReference.FormNotifcationFlyoutRoot, TimeSpan.FromSeconds(2), "Failed to open the form notifications");
                    }

                    var notificationList = driver.FindElement(notificationBar.Locator + _client.ElementMapper.EntityReference.FormNotifcationList);
                    var notificationListItems = driver.FindElements(notificationList.Locator + "//li");

                    foreach (var item in notificationListItems)
                    {
                        var icon = driver.FindElement(item.Locator + _client.ElementMapper.EntityReference.FormNotifcationTypeIcon);

                        var notification = new FormNotification
                        {
                            Message = item.Text
                        };
                        string classes = icon.GetAttribute(_client, "class");
                        notification.SetTypeFromClass(classes);
                        notifications.Add(notification);
                    }

                    if (notificationBar != null)
                    {
                        notificationBar = driver.WaitUntilAvailable(_client.ElementMapper.EntityReference.FormNotifcationBar, TimeSpan.FromSeconds(2),"");
                        notificationBar.Click(_client,true); // Collapse the notification bar
                    }
                    return notifications;
                }

            }).Value;
        }

        #endregion

    }
}
