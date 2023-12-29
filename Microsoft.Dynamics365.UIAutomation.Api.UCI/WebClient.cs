﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OtpNet;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.BusinessProcessFlow;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.Entity;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.Grid;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.OnlineLogin;
using static Microsoft.Dynamics365.UIAutomation.Api.UCI.QuickCreate;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class WebClient : BrowserPage, IDisposable
    {
        public List<ICommandResult> CommandResults => Browser.CommandResults;
        public Guid ClientSessionId;

        public WebClient(BrowserOptions options)
        {
            Browser = new InteractiveBrowser(options);
            ClientSessionId = Guid.NewGuid();
        }
        internal BrowserCommandOptions GetOptions(string commandName)
        {
            return new BrowserCommandOptions(Constants.DefaultTraceSource,
                commandName,
                Constants.DefaultRetryAttempts,
                Constants.DefaultRetryDelay,
                null,
                true,
                typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        #region PageWaits
        internal bool WaitForMainPage(TimeSpan timeout, string errorMessage)
            => WaitForMainPage(timeout, null, () => throw new InvalidOperationException(errorMessage));

        internal bool WaitForMainPage(TimeSpan? timeout = null, Action<IWebElement> successCallback = null, Action failureCallback = null)
        {
            IWebDriver driver = Browser.Driver;
            timeout = timeout ?? Constants.DefaultTimeout;
            successCallback = successCallback ?? (
                                  _ =>
                                  {
                                      bool isUCI = driver.HasElement(By.XPath(LoginReference.CrmUCIMainPage));
                                      if (isUCI)
                                          driver.WaitForTransaction();
                                  });

            var xpathToMainPage = By.XPath(LoginReference.CrmMainPage);
            var element = driver.WaitUntilAvailable(xpathToMainPage, timeout, successCallback, failureCallback);
            return element != null;
        }
        #endregion


        #region FormContextType

        // Used by SetValue methods to determine the field context
        public IWebElement ValidateFormContext(IWebDriver driver, FormContextType formContextType, string field, IWebElement fieldContainer)
        {
            if (formContextType == FormContextType.QuickCreate)
            {
                // Initialize the quick create form context
                // If this is not done -- element input will go to the main form due to new flyout design
                var formContext = driver.WaitUntilAvailable(By.XPath(QuickCreateReference.QuickCreateFormContext));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.Entity)
            {
                // Initialize the entity form context
                var formContext = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.FormContext));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.BusinessProcessFlow)
            {
                // Initialize the Business Process Flow context
                var formContext = driver.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.BusinessProcessFlowFormContext));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(BusinessProcessFlowReference.TextFieldContainer.Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.Header)
            {
                // Initialize the Header context
                var formContext = driver.WaitUntilAvailable(By.XPath(Entity.EntityReference.HeaderContext));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.Dialog)
            {
                // Initialize the Dialog context
                driver.WaitForTransaction();
                var formContext = driver
                    .FindElements(By.XPath(Dialogs.DialogsReference.DialogContext))
                    .LastOrDefault() ?? throw new NotFoundException("Unable to find a dialog.");
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(Entity.EntityReference.TextFieldContainer.Replace("[NAME]", field)));
            }

            return fieldContainer;
        }

        #endregion

        internal void ThinkTime(int milliseconds)
        {
            Browser.ThinkTime(milliseconds);
        }

        internal void ThinkTime(TimeSpan timespan)
        {
            ThinkTime((int)timespan.TotalMilliseconds);
        }

        public void Dispose()
        {
            Browser.Dispose();
        }
    }
}
