using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;


namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    internal class SeleniumBrowser : IWebBrowser, IDisposable
    {
        private BrowserOptions _options;
        private IWebDriver _driver;

        public SeleniumBrowser(IWebDriver driver, BrowserOptions options)
        {
            _options = options;
            _driver = driver;
        }

        public void Click(string selector)
        {
            _driver.WaitUntilClickable(selector, "Can not click element");
            IWebElement element = GetElement(selector);

            if (element != null)
                element.Click(true);
        }

        public void DoubleClick(string selector)
        {
            _driver.WaitUntilClickable(selector, "Can not click element");
            IWebElement element = GetElement(selector);

            if (element != null)
                _driver.DoubleClick(element);
        }

        public void Focus(string selector)
        {
            _driver.WaitUntilClickable(selector, "Element not available");
            IWebElement element = GetElement(selector);

            if (element != null)
                element.Click();
        }

        public void Navigate(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void SetValue(string selector, string value)
        {
            _driver.WaitUntilClickable(selector, "Element not available");
            IWebElement element = GetElement(selector);

            if (element != null)
            {
                element.Click();
                element.SendKeys(value);
            }
        }

        public bool IsAvailable(string selector)
        {
            var isAvailable = _driver.WaitUntilClickable(selector);
            return (isAvailable != null) ? true : false;
        }

        private IWebElement GetElement(string selector)
        {
            IWebElement element = null;

            if (selector.StartsWith("#"))
                element = _driver.FindElement(By.CssSelector(selector));
            else if(selector.StartsWith("//"))
                element = _driver.FindElement(By.XPath(selector));
            else if (_driver.FindElement(By.Id(selector)) != null)
                element = _driver.FindElement(By.Id(selector));
            else if (_driver.FindElement(By.Name(selector)) != null)
                element = _driver.FindElement(By.Name(selector));
            else if (_driver.FindElement(By.ClassName(selector)) != null)
                element = _driver.FindElement(By.ClassName(selector));
            else if (_driver.FindElement(By.LinkText(selector)) != null)
                element = _driver.FindElement(By.LinkText(selector));

            return element;
        }
        #region Disposal / Finalization

        private readonly object syncRoot = new object();
        private readonly bool disposeOfDriver = true;
        private bool disposing = false;

        public void Dispose()
        {
            bool isDisposing;

            lock (this.syncRoot)
            {
                isDisposing = disposing;
            }

            if (!isDisposing)
            {
                lock (this.syncRoot)
                {
                    disposing = true;
                }
            }
        }

        public Element FindElement(string selector)
        {
            IWebElement seleniumElement = _driver.FindElement(By.XPath(selector));
            return (Element) seleniumElement;
        }

        public void Wait(TimeSpan? timeout = null)
        {
            SeleniumExtensions.WaitForTransaction(_driver, timeout);
        }

        #endregion Disposal / Finalization
    }
}
