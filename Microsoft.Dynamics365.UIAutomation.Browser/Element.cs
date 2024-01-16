using Microsoft.Playwright;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text.Json;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class Element //: IWebElement, IElementHandle
    {
        /*
         * Properties: Location, Selecatable
         * Method: Click, Clear, Set
         * 
         * 
         * 
         */

        //private InteractiveBrowser _browser;
        //public Element(InteractiveBrowser Browser)
        //{
        //    _browser = Browser;
        //}


        //public void ThinkTime(int milliseconds)
        //{
        //    _browser.ThinkTime(milliseconds);
        //}

        //public void ThinkTime(TimeSpan timespan)
        //{
        //    ThinkTime((int)timespan.TotalMilliseconds);
        //}


        //public void WaitForLoadArea(IWebDriver driver)
        //{
        //    driver.WaitForPageToLoad();
        //    driver.WaitForTransaction();
        //}

        //public void Dispose()
        //{
        //    _browser.Dispose();
        //}
        public string TagName => throw new NotImplementedException();

        public string Text => throw new NotImplementedException();

        public bool Enabled => throw new NotImplementedException();

        public bool Selected => throw new NotImplementedException();

        public Point Location => throw new NotImplementedException();

        public Size Size => throw new NotImplementedException();

        public bool Displayed => throw new NotImplementedException();

        public IElementHandle? AsElement()
        {
            throw new NotImplementedException();
        }

        public Task<ElementHandleBoundingBoxResult?> BoundingBoxAsync()
        {
            throw new NotImplementedException();
        }

        public Task CheckAsync(ElementHandleCheckOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            throw new NotImplementedException();
        }

        public Task ClickAsync(ElementHandleClickOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IFrame?> ContentFrameAsync()
        {
            throw new NotImplementedException();
        }

        public Task DblClickAsync(ElementHandleDblClickOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task DispatchEventAsync(string type, object? eventInit = null)
        {
            throw new NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> EvalOnSelectorAllAsync<T>(string selector, string expression, object? arg = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> EvalOnSelectorAsync<T>(string selector, string expression, object? arg = null)
        {
            throw new NotImplementedException();
        }

        public Task<JsonElement?> EvalOnSelectorAsync(string selector, string expression, object arg = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> EvaluateAsync<T>(string expression, object? arg = null)
        {
            throw new NotImplementedException();
        }

        public Task<JsonElement?> EvaluateAsync(string expression, object arg = null)
        {
            throw new NotImplementedException();
        }

        public Task<IJSHandle> EvaluateHandleAsync(string expression, object? arg = null)
        {
            throw new NotImplementedException();
        }

        public Task FillAsync(string value, ElementHandleFillOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public IWebElement FindElement(By by)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            throw new NotImplementedException();
        }

        public Task FocusAsync()
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string attributeName)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetAttributeAsync(string name)
        {
            throw new NotImplementedException();
        }

        public string GetCssValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public string GetDomAttribute(string attributeName)
        {
            throw new NotImplementedException();
        }

        public string GetDomProperty(string propertyName)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, IJSHandle>> GetPropertiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IJSHandle> GetPropertyAsync(string propertyName)
        {
            throw new NotImplementedException();
        }

        public ISearchContext GetShadowRoot()
        {
            throw new NotImplementedException();
        }

        public Task HoverAsync(ElementHandleHoverOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> InnerHTMLAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> InnerTextAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> InputValueAsync(ElementHandleInputValueOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCheckedAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsDisabledAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEditableAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEnabledAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsHiddenAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsVisibleAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> JsonValueAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IFrame?> OwnerFrameAsync()
        {
            throw new NotImplementedException();
        }

        public Task PressAsync(string key, ElementHandlePressOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<IElementHandle>> QuerySelectorAllAsync(string selector)
        {
            throw new NotImplementedException();
        }

        public Task<IElementHandle?> QuerySelectorAsync(string selector)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ScreenshotAsync(ElementHandleScreenshotOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task ScrollIntoViewIfNeededAsync(ElementHandleScrollIntoViewIfNeededOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(string values, ElementHandleSelectOptionOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IElementHandle values, ElementHandleSelectOptionOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<string> values, ElementHandleSelectOptionOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(SelectOptionValue values, ElementHandleSelectOptionOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<IElementHandle> values, ElementHandleSelectOptionOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<SelectOptionValue> values, ElementHandleSelectOptionOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task SelectTextAsync(ElementHandleSelectTextOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public void SendKeys(string text)
        {
            throw new NotImplementedException();
        }

        public Task SetCheckedAsync(bool checkedState, ElementHandleSetCheckedOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task SetInputFilesAsync(string files, ElementHandleSetInputFilesOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task SetInputFilesAsync(IEnumerable<string> files, ElementHandleSetInputFilesOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task SetInputFilesAsync(FilePayload files, ElementHandleSetInputFilesOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task SetInputFilesAsync(IEnumerable<FilePayload> files, ElementHandleSetInputFilesOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }

        public Task TapAsync(ElementHandleTapOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<string?> TextContentAsync()
        {
            throw new NotImplementedException();
        }

        public Task TypeAsync(string text, ElementHandleTypeOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task UncheckAsync(ElementHandleUncheckOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task WaitForElementStateAsync(ElementState state, ElementHandleWaitForElementStateOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IElementHandle?> WaitForSelectorAsync(string selector, ElementHandleWaitForSelectorOptions? options = null)
        {
            throw new NotImplementedException();
        }
    }

}
