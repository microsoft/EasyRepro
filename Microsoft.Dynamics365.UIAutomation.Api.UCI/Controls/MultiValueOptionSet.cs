// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.BusinessProcessFlow;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.QuickCreate;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class MultiValueOptionSet
    {
        public string Name { get; set; }
        public string[] Values { get; set; }
        private static Entity.EntityReference _entityReference;
        public MultiValueOptionSet() {
            _entityReference = new Entity.EntityReference();
        }
        internal BrowserCommandResult<bool> ClearValue(WebClient client, MultiValueOptionSet control, FormContextType formContextType)
        {
            return client.Execute(client.GetOptions($"Clear Field {control.Name}"), driver =>
            {
                RemoveMultiOptions(client, control, formContextType);

                return true;
            });
        }

        /// <summary>
        /// Sets/Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set/removed</param>
        /// <param name="removeExistingValues">False - Values will be set. True - Values will be removed</param>
        /// <returns>True on success</returns>
        internal static BrowserCommandResult<bool> SetValue(WebClient client, MultiValueOptionSet option, FormContextType formContextType = FormContextType.Entity, bool removeExistingValues = false)
        {
            return client.Execute(client.GetOptions($"Set MultiValueOptionSet Value: {option.Name}"), driver =>
            {
                if (removeExistingValues)
                {
                    RemoveMultiOptions(client, option, formContextType);
                }


                AddMultiOptions(client,option, formContextType);

                return true;
            });
        }

        /// <summary>
        /// Removes the value from the multselect type control
        /// </summary>
        /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be removed</param>
        /// <returns></returns>
        internal static BrowserCommandResult<bool> RemoveMultiOptions(WebClient client, MultiValueOptionSet option, FormContextType formContextType)
        {
            return client.Execute(client.GetOptions($"Remove Multi Select Value: {option.Name}"), driver =>
            {
                IWebElement fieldContainer = null;

                if (formContextType == FormContextType.QuickCreate)
                {
                    // Initialize the quick create form context
                    // If this is not done -- element input will go to the main form due to new flyout design
                    var formContext = driver.WaitUntilAvailable(By.XPath(QuickCreateReference.QuickCreateFormContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(MultiSelect.DivContainer.Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.Entity)
                {
                    // Initialize the entity form context
                    var formContext = driver.WaitUntilAvailable(By.XPath(_entityReference.FormContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(MultiSelect.DivContainer.Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.BusinessProcessFlow)
                {
                    // Initialize the Business Process Flow context
                    var formContext = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFormContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(MultiSelect.DivContainer.Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.Header)
                {
                    // Initialize the Header context
                    var formContext = driver.WaitUntilAvailable(By.XPath(_entityReference.HeaderContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(MultiSelect.DivContainer.Replace("[NAME]", option.Name)));
                }
                else if (formContextType == FormContextType.Dialog)
                {
                    // Initialize the Header context
                    var formContext = driver.WaitUntilAvailable(By.XPath(Dialogs.DialogsReference.DialogContext));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(MultiSelect.DivContainer.Replace("[NAME]", option.Name)));
                }

                fieldContainer.Hover(driver, true);

                var selectedRecordXPath = By.XPath(MultiSelect.SelectedRecord);
                //change to .//li
                var selectedRecords = fieldContainer.FindElements(selectedRecordXPath);

                var initialCountOfSelectedOptions = selectedRecords.Count;
                var deleteButtonXpath = By.XPath(MultiSelect.SelectedOptionDeleteButton);
                //button[contains(@data-id, 'delete')]
                for (int i = 0; i < initialCountOfSelectedOptions; i++)
                {
                    // With every click of the button, the underlying DOM changes and the
                    // entire collection becomes stale, hence we only click the first occurance of
                    // the button and loop back to again find the elements and anyother occurance
                    selectedRecords[0].FindElement(deleteButtonXpath).Click(true);
                    driver.WaitForTransaction();
                    selectedRecords = fieldContainer.FindElements(selectedRecordXPath);
                }

                return true;
            });
        }
            /// <summary>
            /// Sets the value from the multselect type control
            /// </summary>
            /// <param name="option">Object of type MultiValueOptionSet containing name of the Field and the values to be set</param>
            /// <returns></returns>
            public static BrowserCommandResult<bool> AddMultiOptions(WebClient client, MultiValueOptionSet option, FormContextType formContextType)
            {
                return client.Execute(client.GetOptions($"Add Multi Select Value: {option.Name}"), driver =>
                {
                    IWebElement fieldContainer = null;

                    if (formContextType == FormContextType.QuickCreate)
                    {
                        // Initialize the quick create form context
                        // If this is not done -- element input will go to the main form due to new flyout design
                        var formContext = driver.WaitUntilAvailable(By.XPath(QuickCreateReference.QuickCreateFormContext));
                        fieldContainer = formContext.WaitUntilAvailable(By.XPath(MultiSelect.DivContainer.Replace("[NAME]", option.Name)));
                    }
                    else if (formContextType == FormContextType.Entity)
                    {
                        // Initialize the entity form context
                        var formContext = driver.WaitUntilAvailable(By.XPath(_entityReference.FormContext));
                        fieldContainer = formContext.WaitUntilAvailable(By.XPath(MultiSelect.DivContainer.Replace("[NAME]", option.Name)));
                    }
                    else if (formContextType == FormContextType.BusinessProcessFlow)
                    {
                        // Initialize the Business Process Flow context
                        var formContext = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFormContext));
                        fieldContainer = formContext.WaitUntilAvailable(By.XPath(MultiSelect.DivContainer.Replace("[NAME]", option.Name)));
                    }
                    else if (formContextType == FormContextType.Header)
                    {
                        // Initialize the Header context
                        var formContext = driver.WaitUntilAvailable(By.XPath(_entityReference.HeaderContext));
                        fieldContainer = formContext.WaitUntilAvailable(By.XPath(MultiSelect.DivContainer.Replace("[NAME]", option.Name)));
                    }
                    else if (formContextType == FormContextType.Dialog)
                    {
                        // Initialize the Header context
                        var formContext = driver.WaitUntilAvailable(By.XPath(Dialogs.DialogsReference.DialogContext));
                        fieldContainer = formContext.WaitUntilAvailable(By.XPath(Dialogs.DialogsReference.DialogContext.Replace("[NAME]", option.Name)));
                    }

                    var inputXPath = By.XPath(MultiSelect.InputSearch);
                    fieldContainer.FindElement(inputXPath).SendKeys(string.Empty);

                    var flyoutCaretXPath = By.XPath(MultiSelect.FlyoutCaret);
                    fieldContainer.FindElement(flyoutCaretXPath).Click();

                    foreach (var optionValue in option.Values)
                    {
                        var flyoutOptionXPath = By.XPath(MultiSelect.FlyoutOption.Replace("[NAME]", optionValue));
                        if (fieldContainer.TryFindElement(flyoutOptionXPath, out var flyoutOption))
                        {
                            var ariaSelected = flyoutOption.GetAttribute<string>("aria-selected");
                            var selected = !string.IsNullOrEmpty(ariaSelected) && bool.Parse(ariaSelected);

                            if (!selected)
                            {
                                flyoutOption.Click();
                            }
                        }
                    }

                    return true;
                });
            }
        }
    

    internal static class MultiSelect
    {
        public static string DivContainer = ".//div[contains(@data-id,\"[NAME]-FieldSectionItemContainer\")]";
        public static string InputSearch = ".//input[contains(@data-id,\"textInputBox\")]";
        public static string SelectedRecord = ".//li";
        //public string SelectedRecordButton = ".//button[contains(@data-id, \"delete\")]";
        public static string SelectedOptionDeleteButton = ".//button[contains(@data-id, \"delete\")]";
        public static string SelectedRecordLabel = ".//span[contains(@class, \"msos-selected-display-item-text\")]";
        public static string FlyoutCaret = "//button[contains(@class, \"msos-caret-button\")]";
        public static string FlyoutOption = "//li[label[contains(@title, \"[NAME]\")] and contains(@class,\"msos-option\")]";
        public static string FlyoutOptionCheckbox = "//input[contains(@class, \"msos-checkbox\")]";
        public static string ExpandCollapseButton = ".//button[contains(@class,\"msos-selecteditems-toggle\")]";
    }




}
