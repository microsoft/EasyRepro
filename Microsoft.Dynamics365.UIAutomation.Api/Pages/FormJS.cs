using System;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace Microsoft.Dynamics365.UIAutomation.Api.Pages
{
    public class FormJS : BrowserPage
    {
        public FormJS(InteractiveBrowser browser)
            : base(browser)
        { }

        public BrowserCommandOptions GetOptions(string commandName)
        {
            return new BrowserCommandOptions(Constants.DefaultTraceSource,
                commandName,
                0,
                0,
                null,
                true,
                typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        public bool SwitchToContent()
        {
            Browser.Driver.SwitchTo().DefaultContent();
            //wait for the content panel to render
            Browser.Driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Frames.ContentPanel]));

            //find the crmContentPanel and find out what the current content frame ID is - then navigate to the current content frame
            var currentContentFrame = Browser.Driver.FindElement(By.XPath(Elements.Xpath[Reference.Frames.ContentPanel]))
                .GetAttribute(Elements.ElementId[Reference.Frames.ContentFrameId]);

            Browser.Driver.SwitchTo().Frame(currentContentFrame);

            return true;
        }

        public BrowserCommandResult<T> GetAttributeValue<T>(string attributte)
        {
            var commandName = $"Get Attribute Value via Form JS: {attributte}";
            string code = $"return Xrm.Page.getAttribute('{attributte}').getValue()";

            return ExecuteJS<T>(commandName, code);
        }

        public bool SetAttributeValue<T>(string attributte, T value)
        {
            var commandName = $"Set Attribute Value via Form JS: {attributte}";
            string code = $"return Xrm.Page.getAttribute('{attributte}').setValue(arguments[0])";

            return ExecuteJS(commandName, code, value);
        }

        public bool Clear(string attribute)
        {
            var commandName = $"Clear Attribute via Form JS: {attribute}";
            string code = $"return Xrm.Page.getAttribute('{attribute}').setValue(null)";

            return ExecuteJS(commandName, code);
        }

        public BrowserCommandResult<Guid> GetEntityId()
        {
            var commandName = $"Get Entity Id via Form JS";
            string code = $"return Xrm.Page.data.entity.getId()";

            return ExecuteJS<string, Guid>(commandName, code, v =>
            {
                bool success = Guid.TryParse(v, out Guid result);
                return success ? result : default(Guid);
            });
        }

        public BrowserCommandResult<bool> IsControlVisible(string attributte)
        {
            var commandName = $"Get Control Visibility via Form JS: {attributte}";
            string code = $"return Xrm.Page.getControl('{attributte}').getVisible()";

            return ExecuteJS<bool>(commandName, code);
        }

        public BrowserCommandResult<bool> IsDirty(string attributte)
        {
            var commandName = $"Get Attribute IsDirty via Form JS: {attributte}";
            string code = $"return Xrm.Page.getAttribute('{attributte}').getIsDirty()";

            return ExecuteJS<bool>(commandName, code);
        }

        public enum RequiereLevel { Unknown = 0, None, Required, Recommended }

        public BrowserCommandResult<RequiereLevel> GetRequiredLevel(string attributte)
        {
            var commandName = $"Get Attribute RequiredLevel via Form JS: {attributte}";
            string code = $"return Xrm.Page.getAttribute('{attributte}').getRequiredLevel()";
            
            return ExecuteJS<string, RequiereLevel>(commandName, code,
                v =>
                {
                    bool success =  Enum.TryParse(v, true, out RequiereLevel result);
                    return success ? result : RequiereLevel.Unknown;
                });
        }


        public BrowserCommandResult<T> ExecuteJS<T>(string commandName, string code) => ExecuteJS<T, T>(commandName, code, result => result);

        public BrowserCommandResult<TResult> ExecuteJS<T, TResult>(string commandName, string code, Func<T, TResult> converter)
        {
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            return Execute(GetOptions(commandName), driver =>
            {
                SwitchToContent();

                T result = driver.ExecuteJavaScript<T>(code);
                return converter(result);
            });
        }
        
        public bool ExecuteJS(string commandName, string code, params object[] args)
        {
            return Execute(GetOptions(commandName), driver =>
            {
                SwitchToContent();

                driver.ExecuteJavaScript(code, args);
                return true;
            });
        }
    }
}
