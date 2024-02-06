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
using System.Diagnostics;


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

        public string Url { get { return _driver.Url; } set { _url = value; } }

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





        private ICollection<IElement> ConvertToElements(ICollection<IWebElement> elements, string selector)
        {
            ICollection<IElement> rtnObject = new List<IElement>();
            foreach (var element in elements)
            {
                rtnObject.Add(element.ToElement(_driver, selector));
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
            Trace.TraceInformation("[Selenium] Browser has element initated. XPath: " + selector);
            return _driver.HasElement(By.XPath(selector));
        }
        #endregion

        #region GetElement/GetSelectorType
        private IWebElement GetElement(string selector)
        {
            //IWebElement element = GetElement(selector);
            return _driver.FindElement(GetSelectorType(selector));
        }
        private By GetSelectorType(string selector)
        {
            By type = null;

            if (selector.StartsWith("#"))
                type = By.CssSelector(selector);
            else if (selector.StartsWith("//") || selector.StartsWith(".//"))
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
            Trace.TraceInformation("[Selenium] Browser find element initated. XPath: " + selector);
            return GetElement(selector).ToElement(_driver, selector);
        }
        List<IElement>? IWebBrowser.FindElements(string selector)
        {
            Trace.TraceInformation("[Selenium] Browser find elements initated. XPath: " + selector);
            var elements = _driver.FindElements(By.XPath(selector));
            return elements.ToElements(_driver, selector);
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

        #region WaitUntilAvailable

        public IElement? WaitUntilAvailable(string selector)
        {
            Trace.TraceInformation("[Selenium] Browser wait until available initated. XPath: " + selector);
            return _driver.WaitUntilAvailable(By.XPath(selector)).ToElement(_driver, selector);
        }

        public IElement WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage)
        {
            Trace.TraceInformation("[Selenium] Browser wait until available initated. XPath: " + selector);
            return _driver.WaitUntilAvailable(GetSelectorType(selector), exceptionMessage).ToElement(_driver, selector);
        }
        public IElement WaitUntilAvailable(string selector, string exceptionMessage)
        {
            Trace.TraceInformation("[Selenium] Browser wait until available initated. XPath: " + selector);
            IWebElement element = SeleniumExtensions.WaitUntilVisible(_driver, By.XPath(selector));
            //IWebElement element = _driver.WaitUntilAvailable(By.XPath(selector), null, null, WaitUntilAvailable(selector));
            return element.ToElement(_driver, selector);
        }
        #endregion

        #region SwitchFrame
        public void SwitchToFrame(string locator)
        {
            if (int.TryParse(locator, out var frame))
            {
                if (frame == 0) { _driver.SwitchTo().ParentFrame(); }
                else _driver.SwitchTo().Frame(frame);
            }
            else
            {
                _driver.WaitUntilAvailable(By.Id(locator));
                _driver.SwitchTo().Frame(locator);
            }
            //_driver.WaitForTransaction();
            
        }
        #endregion

        #region TakeScreenshot
        public void TakeWindowScreenShot(string fileName, FileFormat fileFormat)
        {
            _driver.TakeScreenshot();
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

                return false;
            }
            return true;
        }

        public bool ClickWhenAvailable(string selector, TimeSpan timeToWait, string? exceptionMessage = null)
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
        #endregion
    }
}
    






    
