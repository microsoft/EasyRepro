using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.IO;


namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    internal class SeleniumBrowser : IWebBrowser, IDisposable
    {
        private BrowserOptions _options;
        private IWebDriver _driver;
        private string _url = "";

        public SeleniumBrowser(IWebDriver driver, BrowserOptions options)
        {
            _options = options;
            _driver = driver;
        }

        public BrowserOptions Options { get { return _options; } set { _options = value; } }

        public string Url { get { return _url; } set { _url = value; } }

        #region FindElement

        public Element FindElement(string selector)
        {
            return (Element)_driver.FindElement(By.XPath(selector);
        }
        public List<Element> FindElements(string selector)
        {
            return new List<Element>(_driver.FindElements(By.XPath(selector)).Select(x => (Element)x));
        }
        #endregion

        #region ExecuteScript

        public object ExecuteScript(string script, params object[] args)
        {
            return _driver.ExecuteScript(script, args);
        }
        #endregion

        #region Navigate

        public void Navigate(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }
        #endregion

        #region SendKeys

        public void SendKey(string selector, string key)
        {
            GetElement(selector).SendKeys(key);
        }

        public void SendKeys(string selector, string[] keys)
        {
            GetElement(selector).SendKeys(keys.ToString());
        }
        #endregion

        #region SwitchToFrame

        public void SwitchToFrame(string name)
        {
            _driver.SwitchTo().Frame(name);
        }
        //Commenting these out as Playwright doesn't do frame # or Element. Just want to save these just in case. 
        //public void SwitchToFrame(int frame)
        //{
        //    _driver.SwitchTo().Frame(frame);
        //}
        //public void SwitchToFrame(Element frameElement)
        //{
        //    _driver.SwitchTo().Frame((IWebElement)frameElement);
        //}

        #endregion

        #region TakeWindowScreenShot

        public void TakeWindowScreenShot(string filename)
        {           
            _driver.TakeScreenshot().SaveAsFile(filename);
        }
        #endregion

        #region HasElement
        public bool HasElement(string selector)
        {
            var hasElement = _driver.WaitUntilClickable(selector);
            return (hasElement != null) ? true : false;
        }
        #endregion

        #region IsAvailable

        public bool IsAvailable(string selector)
        {
            var isAvailable = _driver.WaitUntilClickable(selector);
            return (isAvailable != null) ? true : false;
        }
        #endregion

        #region Wait
        public void Wait(TimeSpan timeSpan)
        {
            ThinkTime((int)timeSpan.TotalMilliseconds);
        }
        public void Wait(int milliseconds)
        {
            ThinkTime(milliseconds);
        }
        private void ThinkTime(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
        #endregion

        #region WaitUntilVailable
        Element IWebBrowser.WaitUntilAvailable(string selector)
        {
            return (Element)_driver.WaitUntilAvailable(GetSelectorType(selector));
        }

        Element IWebBrowser.WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage)
        {
            return (Element)_driver.WaitUntilAvailable(GetSelectorType(selector),timeToWait,exceptionMessage);
        }

        Element IWebBrowser.WaitUntilAvailable(string selector, string exceptionMessage)
        {
            return (Element)_driver.WaitUntilAvailable(GetSelectorType(selector), exceptionMessage);
        }
        #endregion

        #region GetElement/GetSelectorType
        private IWebElement GetElement(string selector)
        {
            return _driver.FindElement(GetSelectorType(selector));
        }
        private By GetSelectorType(string selector)
        {
            By type = null;

            if (selector.StartsWith("#"))
                type = By.CssSelector(selector);
            else if (selector.StartsWith("//"))
                type = By.XPath(selector);
            else if (_driver.FindElement(By.Id(selector)) != null)
                type = By.Id(selector);
            else if (_driver.FindElement(By.Name(selector)) != null)
                type = By.Name(selector);
            else if (_driver.FindElement(By.ClassName(selector)) != null)
                type = By.ClassName(selector);
            else if (_driver.FindElement(By.LinkText(selector)) != null)
                type = By.LinkText(selector);

            if(type == null) throw new Exception(String.Format("Invalid Selctor Type. Can't determine the selector type for selector '{0}'", selector));

            return type;
        }
        #endregion

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

        #endregion Disposal / Finalization
    }
}
