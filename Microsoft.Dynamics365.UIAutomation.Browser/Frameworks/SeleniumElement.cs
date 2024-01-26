using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    internal class SeleniumElement : IElement
    {
        private IWebDriver _driver;
        private IWebElement _element;
        public SeleniumElement(IWebDriver driver, IWebElement element) {
            _driver = driver;
            _element = element;

        }
        private string _tag;
        private string _locator;
        private string _id;
        private string _value;
        private bool _isAvaiable;
        private bool _isClickable;
        private bool _selected;
        private string _text;
        public string Tag { get => _element.TagName; set => throw new NotImplementedException(); }
        public string Locator { get => _locator; set => _locator = value; }
        public string Id { get => _element.GetAttribute("id"); set => throw new NotImplementedException(); }
        public string Value { get => _element.Text; set => throw new NotImplementedException(); }
        public bool IsAvailable { get => _element.Enabled; set => throw new NotImplementedException(); }
        public bool IsClickable { get => _element.IsClickable(); set => throw new NotImplementedException(); }
        public bool Selected { get => _element.Selected; set => throw new NotImplementedException(); }
        public string Text { get => _element.Text; set => throw new NotImplementedException(); }

        public void Clear(BrowserPage page, string key)
        {
            _element.Clear();
        }

        public void Click(BrowserPage page, bool? click = true)
        {
            _element.Click();
        }

        public bool ClickWhenAvailable(BrowserPage page, string key)
        {
            throw new NotImplementedException();
            //return _element.ClickWhenAvailable(By.XPath(key));
        }

        public bool ClickWhenAvailable(BrowserPage page, string key, TimeSpan timeToWait, string? exceptionMessage = null)
        {
            throw new NotImplementedException();
        }

        public void DoubleClick(BrowserPage page, string key)
        {
            page.Execute(new BrowserCommandOptions(), browser =>
            {
                _driver.DoubleClick(_element);
                return true;
            });
        }

        public void Focus(BrowserPage page, string key)
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(BrowserPage page, string attributeName)
        {
            return this._element.GetAttribute(attributeName);
        }

        public bool HasAttribute(BrowserPage page, string attributeName)
        {
            return _element.HasAttribute(attributeName);
        }

        public void Hover(BrowserPage page, string key)
        {
            _element.Hover(_driver);
            
        }

        public void SendKeys(BrowserPage page, string[] keys)
        {
            _element.SendKeys(string.Join("", keys));
        }

        public void SetValue(BrowserPage page, string value)
        {
            _element.SendKeys(value);
        }

    }
}
