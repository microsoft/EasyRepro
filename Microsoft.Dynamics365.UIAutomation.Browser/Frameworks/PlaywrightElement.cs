using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    internal class PlaywrightElement : IElement
    {
        private ILocator _element;
        public PlaywrightElement(ILocator element) {
            _element = element;

        }
        public string Tag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Locator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsAvailable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsClickable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Selected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Clear(BrowserPage page, string key)
        {
            throw new NotImplementedException();
        }

        public void Click(BrowserPage page, bool? click = true)
        {
            throw new NotImplementedException();
        }

        public bool ClickWhenAvailable(BrowserPage page, string key)
        {
            throw new NotImplementedException();
        }

        public bool ClickWhenAvailable(BrowserPage page, string key, TimeSpan timeToWait, string? exceptionMessage = null)
        {
            throw new NotImplementedException();
        }

        public void DoubleClick(BrowserPage page, string key)
        {
            throw new NotImplementedException();
        }

        public void Focus(BrowserPage page, string key)
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(BrowserPage page, string attributeName)
        {
            return this._element.GetAttributeAsync(attributeName).GetAwaiter().GetResult();
        }

        public bool HasAttribute(BrowserPage page, string attributeName)
        {
            throw new NotImplementedException();
        }

        public void Hover(BrowserPage page, string key)
        {
            throw new NotImplementedException();
        }

        public void SendKeys(BrowserPage page, string[] keys)
        {
            throw new NotImplementedException();
        }

        public void SetValue(BrowserPage page, string value)
        {
            throw new NotImplementedException();
        }

        public void Test(BrowserPage page, string value)
        {
            throw new NotImplementedException();
        }
    }
}
