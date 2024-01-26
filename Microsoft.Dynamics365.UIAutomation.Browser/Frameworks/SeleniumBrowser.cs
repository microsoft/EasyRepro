using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.IO;
using OpenQA.Selenium.DevTools.V107.DOM;
using OpenQA.Selenium.Interactions;


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

        public List<IElement> FindElements(string selector)
        {
            return new List<IElement>(_driver.FindElements(By.XPath(selector)).Select(x => (IElement)x));
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
        public void SendKey(string locator, string key)
        {
            Actions keyPress = new Actions(_driver);
            keyPress.SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
        }
        //public void SendKey(string selector, string key)
        //{
        //    GetElement(selector).SendKeys(key);
        //}

        public void SendKeys(string selector, string[] keys)
        {
            //GetElement(selector).SendKeys(keys.ToString());

            Actions action = new Actions(_driver);
            //action.KeyDown(Keys.Control).SendKeys("S").Perform();
            int keyLength = keys.Length - 1;
            for (int x = 0; x < keyLength; x++)
            {
                action.KeyDown(keys[x].Select(t => $"U+{Convert.ToUInt16(t):X4} ").FirstOrDefault());
            }
            action.SendKeys(keys[keyLength]);
            action.Perform();
        }
        #endregion

        #region ConvertToElement


        //string IWebBrowser.Url { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private IElement? ConvertToElement(IWebElement element, string selector)
        {
            IElement rtnObject = new SeleniumElement(_driver, element); ;
            if (element == null) return null;
            try
            {
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

        #endregion

        #region Dispose
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
        #endregion

        #region HasElement
        public bool HasElement(string selector)
        {
            var hasElement = _driver.WaitUntilClickable(selector);
            return (hasElement != null) ? true : false;
        }
        #endregion

        #region GetElement/GetSelectorType
        private IWebElement GetElement(string selector)
        {
            IWebElement element = GetElement(selector);
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

            if (type == null) throw new Exception(String.Format("Invalid Selctor Type. Can't determine the selector type for selector '{0}'", selector));

            return type;
        }
        #endregion

        #region FindElement

        public IElement FindElement(string selector)
        {
            return ConvertToElement(GetElement(selector), selector);
        }
        List<IElement>? IWebBrowser.FindElements(string selector)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Wait
        public void Wait(PageEvent pageEvent)
        {
            switch (pageEvent)
            {
                case PageEvent.Load:
                    _driver.WaitForPageToLoad();
                    break;
                default:
                    break;
            }

        }
        public void Wait(TimeSpan? timeSpan = null)
        {
            ThinkTime((int)timeSpan.GetValueOrDefault(Constants.DefaultTimeout).TotalMilliseconds);
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
        IElement IWebBrowser.WaitUntilAvailable(string selector)
        {
            return (IElement)_driver.WaitUntilAvailable(GetSelectorType(selector));
        }

        public IElement? WaitUntilAvailable(string selector)
        {
           return _driver.WaitUntilAvailable(By.XPath(selector)).ToElement();
        }

        public IElement WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage)
        {
            return (IElement)_driver.WaitUntilAvailable(GetSelectorType(selector), exceptionMessage);
        }
        public IElement WaitUntilAvailable(string selector, string exceptionMessage)
        {
            IWebElement element = SeleniumExtensions.WaitUntilVisible(_driver, By.XPath(selector));
            //IWebElement element = _driver.WaitUntilAvailable(By.XPath(selector), null, null, WaitUntilAvailable(selector));
            return ConvertToElement(element, selector);
        }
        #endregion

        #region SwitchFrame
        public void SwitchToFrame(string locator)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region TakeScreenshot
        public void TakeWindowScreenShot(string fileName, FileFormat fileFormat)
        {
            _driver.TakeScreenshot();
        }
        #endregion
    }
}
    






    
