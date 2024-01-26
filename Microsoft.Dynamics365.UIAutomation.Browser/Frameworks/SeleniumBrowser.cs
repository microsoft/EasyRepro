using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Microsoft.Playwright;
using System.Xml.Linq;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Interactions;


namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    internal class SeleniumBrowser : IWebBrowser, IDisposable
    {
        string IWebBrowser.Url
        {
            get
            {
                return _driver.Url;
            }
            set
            {
                _driver.Url = value;
            }
        }
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

        public bool DoubleClick(string selector)
        {
            _driver.WaitUntilClickable(selector, "Can not click element");
            IWebElement element = GetElement(selector);

            if (element != null)
                _driver.DoubleClick(element);

            return true;
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
            //_driver.Navigate().GoToUrl(url);
            _driver.Navigate().GoToUrl(new Uri(url));
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


        //string IWebBrowser.Url { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private IElement? ConvertToElement(IWebElement element, string selector)
        {
            IElement rtnObject = new SeleniumElement(element);
            if (element == null) return null;
            try
            {
                //rtnObject.Text = element.Text;
                //rtnObject.Tag = element.TagName;
                //rtnObject.Selected = element.Selected;
                //rtnObject.Value = element.Text;
                //rtnObject.Id = element.GetAttribute("id");
                rtnObject.Locator = selector;
            }
            catch (StaleElementReferenceException staleEx)
            {
                return null;
                //throw;
            }


            return rtnObject;
        }
        private ICollection<IElement> ConvertToElements(ICollection<IWebElement> elements, string selector)
        {
            ICollection<IElement> rtnObject = new List<IElement>();
            foreach (var element in elements)
            {
                rtnObject.Add(ConvertToElement(element, selector));
            }
            return rtnObject;
        }
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

        public IElement FindElement(string selector)
        {
            IWebElement seleniumElement = _driver.FindElement(By.XPath(selector));
            return ConvertToElement(seleniumElement, selector);
        }

        public void Wait(TimeSpan? timeout = null)
        {
            SeleniumExtensions.WaitForTransaction(_driver, timeout);
        }

        public bool ClickWhenAvailable(string selector, TimeSpan timeToWait, string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScript(string selector, params object[] args)
        {
            throw new NotImplementedException();
        }

        public bool HasElement(string selector)
        {
            return _driver.HasElement(By.XPath(selector));
        }
        
        public IElement? WaitUntilAvailable(string selector)
        {
            IWebElement element = SeleniumExtensions.WaitUntilVisible(_driver, By.XPath(selector));
            //IWebElement element = _driver.WaitUntilAvailable(By.XPath(selector), null, null, WaitUntilAvailable(selector));
            return ConvertToElement(element, selector);
        }

        public IElement WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage)
        {
            IWebElement element = _driver.WaitUntilAvailable(By.XPath(selector), timeToWait, exceptionMessage);
            return ConvertToElement(element, selector);
        }

        public IElement WaitUntilAvailable(string selector, string exceptionMessage)
        {
            IWebElement element = _driver.WaitUntilAvailable(By.XPath(selector), exceptionMessage);
            return ConvertToElement(element, selector);
        }

        public bool ClickWhenAvailable(string selector)
        {
            
            try
            {
                IWebElement element = _driver.ClickIfVisible(By.XPath(selector), new TimeSpan(0, 0, 2));
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        bool IWebBrowser.ClickWhenAvailable(string selector, TimeSpan timeToWait, string? exceptionMessage)
        {
            try
            {
                IWebElement element = _driver.ClickWhenAvailable(By.XPath(selector), timeToWait, exceptionMessage);
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(exceptionMessage);
            }
           
        }

        public List<IElement>? FindElements(string selector)
        {
            ICollection<IWebElement> elements = _driver.FindElements(By.XPath(selector));
            return ConvertToElements(elements, selector).ToList();
        }

        public void SendKeys(string locator, string[] keys)
        {
            IWebElement element = _driver.FindElement(By.XPath(locator));
            SeleniumExtensions.SendKeys(element, string.Join("",keys));
        }

        public void SwitchToFrame(string locator)
        {
            if (int.TryParse(locator, out var frame))
            {
                if (frame == 0) { _driver.SwitchTo().ParentFrame(); }
                else _driver.SwitchTo().Frame(frame);
            }
            else
                _driver.SwitchTo().Frame(locator);
        }

        public void Wait(PageEvent pageEvent)
        {
            switch (pageEvent){
                case PageEvent.Load:
                    _driver.WaitForPageToLoad();
                    break;
                default:
                    break;
            }
            
        }

        public void TakeWindowScreenShot(string fileName, FileFormat fileFormat)
        {
            _driver.TakeScreenshot();
        }

        public void SendKey(string locator, string key)
        {
            Actions keyPress = new Actions(_driver);
            keyPress.SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
        }

        public string? FindElementAttribute(string selector, string attribute)
        {
            return _driver.FindElement(By.XPath(selector)).GetAttribute(attribute);
        }

        public IElement Test(string selector, string exceptionMessage)
        {
            _interfaceElement = new SeleniumElement(_driver.FindElement(By.XPath(selector)));
            _element = _driver.FindElement(By.XPath(selector)); 
            
            return _interfaceElement;
        }

        private IWebElement _element;
        private SeleniumElement _interfaceElement;

        //public bool DoubleClick(string selector)
        //{
        //    Actions act = new Actions(_driver);
        //    IWebElement element = _driver.FindElement(By.XPath(selector));
        //    act.DoubleClick(element);
        //    return true;
        //}

        #endregion Disposal / Finalization
    }
}
