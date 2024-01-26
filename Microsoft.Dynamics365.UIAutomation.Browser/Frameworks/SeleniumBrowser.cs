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


        //string IWebBrowser.Url { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private Element? ConvertToElement(IWebElement element, string selector)
        {
            Element rtnObject = new Element();
            if (element == null) return null;
            try
            {
                rtnObject.Text = element.Text;
                rtnObject.Tag = element.TagName;
                rtnObject.Selected = element.Selected;
                rtnObject.Value = element.Text;
                rtnObject.Id = element.GetAttribute("id");
                rtnObject.Locator = selector;
            }
            catch (StaleElementReferenceException staleEx)
            {
                return null;
                //throw;
            }

        #endregion

            return rtnObject;
        }
        private ICollection<Element> ConvertToElements(ICollection<IWebElement> elements, string selector)
        {
            ICollection<Element> rtnObject = new List<Element>();
            foreach (var element in elements)
            {
                rtnObject.Add(ConvertToElement(element, selector));
            }
            return rtnObject;
        }
        public void Dispose()
        {
            bool isDisposing;

        #region HasElement
        public bool HasElement(string selector)
        {
            var hasElement = _driver.WaitUntilClickable(selector);
            return (hasElement != null) ? true : false;
        }
        #endregion

        #region IsAvailable

        public Element FindElement(string selector)
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
        
        public Element? WaitUntilAvailable(string selector)
        {
            IWebElement element = _driver.WaitUntilAvailable(By.XPath(selector));
            return ConvertToElement(element, selector);
        }

        public Element WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage)
        {
            return (Element)_driver.WaitUntilAvailable(GetSelectorType(selector), exceptionMessage);
        }
        #endregion

        public Element WaitUntilAvailable(string selector, string exceptionMessage)
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

                throw new Exception(exceptionMessage);
            }
           
        }

        public List<Element>? FindElements(string selector)
        {
            bool isDisposing;

        public void SendKeys(string locator, string[] keys)
        {
            IWebElement element = _driver.FindElement(By.XPath(locator));
            SeleniumExtensions.SendKeys(element, string.Join("",keys));
        }

            if (!isDisposing)
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
            _interfaceElement = new SeleniumElement(_driver, _driver.FindElement(By.XPath(selector)));
            
            return _interfaceElement;
        }

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
