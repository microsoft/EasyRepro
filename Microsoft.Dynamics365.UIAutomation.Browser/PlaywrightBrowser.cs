using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Playwright;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

//using PlaywrightSharp;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    internal class PlaywrightBrowser : IWebBrowser, IDisposable
    {
        private BrowserOptions _options;
        private IBrowser _browser;
        private IPage _page; 

        public PlaywrightBrowser(IBrowser browser, BrowserOptions options)
        {
            _options = options;
            _browser = browser;

            _page = _browser.NewPageAsync().GetAwaiter().GetResult();
        }

        public BrowserOptions Options { get { return _options; } set { _options = value; } }

        string IWebBrowser.Url
        {
            get
            {
                return _page.Url;
            }
            set
            {
                throw new NotImplementedException("Cannot perform in Playwright");
            }
        }


        private Element? ConvertToElement(ILocator element, string selector)
        {
            Element rtnObject = new Element();
            if (element == null) return null;
            try
            {
                rtnObject.Text = element.InnerTextAsync().Result;
                //rtnObject.Tag = element.;
                //rtnObject.Selected = element.IsCheckedAsync().Result;
                rtnObject.Value = element.InnerTextAsync().Result;
                rtnObject.Id = element.GetAttributeAsync("id").Result;
                rtnObject.Locator = selector;
            }
            catch (StaleElementReferenceException staleEx)
            {
                return null;
                //throw;
            }


            return rtnObject;
        }

        private ICollection<Element> ConvertToElements(IReadOnlyCollection<ILocator> elements, string selector)
        {
            ICollection<Element> rtnObject = new List<Element>();
            foreach (var element in elements)
            {
                rtnObject.Add(ConvertToElement(element, selector));
            }
            return rtnObject;
        }

        #region DoubleClick
        public void DoubleClick(string selector)
        {
            WaitForSelector(selector);
            var element = _page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            element.DblClickAsync();
        }
        public async void DoubleClickAsync(string selector)
        {
            await WaitForSelectorAsync(selector);
            var element = await _page.QuerySelectorAsync(selector);
            await element.DblClickAsync();
        }
        #endregion

        #region Click
        public void Click(string selector)
        {
            WaitForSelector(selector);
            var element =  _page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            element.ClickAsync();
        }
        public async void ClickAsync(string selector)
        {
            await WaitForSelectorAsync(selector);
            var element = await _page.QuerySelectorAsync(selector);
            await element.ClickAsync();
        }
        #endregion

        #region SetValue

        public void SetValue(string selector, string value)
        {
            Focus(selector);
            _page.TypeAsync(selector, value).GetAwaiter().GetResult();
        }
        public async void SetValueAsync(string selector, string value)
        {
            await FocusAsync(selector);
            await _page.TypeAsync(selector, value);
        }

        #endregion

        #region Focus
        public void Focus(string selector)
        {
            WaitForSelector(selector);
            _page.FocusAsync(selector).GetAwaiter().GetResult();
        }
        public async Task FocusAsync(string selector)
        {
            await _page.WaitForSelectorAsync(selector);
            await _page.FocusAsync(selector);
        }
        #endregion

        #region WaitForPageState

        #endregion

        #region WaitForSelector
        internal void WaitForSelector(string selector)
        {
            _page.WaitForLoadStateAsync(LoadState.NetworkIdle).GetAwaiter().GetResult();
            //_page.WaitForLoadStateAsync(LifecycleEvent.Networkidle).GetAwaiter().GetResult();
            _page.WaitForSelectorAsync(selector).GetAwaiter().GetResult();
        }
        internal async Task WaitForSelectorAsync(string selector)
        {
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync(selector);
        }
        #endregion 

        #region Navigate
        public void Navigate(string url)
        {
            _page.GotoAsync(url).GetAwaiter().GetResult();
        }
        public async void NavigateAsync(string url)
        {
            await _page.GotoAsync(url);
        }
        #endregion

        public bool IsAvailable(string selector)
        {
            var isAvailable = _page.WaitForSelectorAsync(selector).GetAwaiter().GetResult();
            return (isAvailable != null) ? true : false;
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
            ILocator element = _page.Locator(selector);
            IElementHandle playWrightElement =  _page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            if (playWrightElement == null) { throw new PlaywrightException(String.Format("Could not find element using selector '{0}'", selector)); }
            return ConvertToElement(element, selector);
        }

        public void Wait(TimeSpan? timeout = null)
        {
            timeout = timeout ?? Constants.DefaultTimeout;
            _page.WaitForTimeoutAsync((float)timeout.Value.TotalMilliseconds);
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
            ILocator locator = _page.Locator(selector);
            IElementHandle playWrightElement = _page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            bool isAvailable = false;
            if (playWrightElement != null) _page.WaitForSelectorAsync(selector).GetAwaiter().GetResult();
            //locator.WaitForAsync().GetAwaiter().GetResult();
            return (playWrightElement != null) ? true : false;
        }

        public Element? WaitUntilAvailable(string selector)
        {
            ILocator locator = _page.Locator(selector);
            locator.WaitForAsync().GetAwaiter().GetResult();
            return ConvertToElement(locator, selector);
        }

        public Element WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage)
        {
            //IElementHandle playWrightElement = _page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            ILocator locator = _page.Locator(selector);
            locator.WaitForAsync().GetAwaiter().GetResult();
            //if (playWrightElement != null) 
            return ConvertToElement(locator, selector);
        }

        public Element WaitUntilAvailable(string selector, string exceptionMessage)
        {
            //IElementHandle playWrightElement = _page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            ILocator locator = _page.Locator(selector);
            locator.WaitForAsync().GetAwaiter().GetResult();
            if (locator == null) throw new Exception(exceptionMessage);
            return ConvertToElement(locator, selector);
        }

        public bool ClickWhenAvailable(string selector)
        {
            //throw new NotImplementedException();
            ILocator locator = _page.Locator(selector);
            try
            {
                locator.WaitForAsync(new LocatorWaitForOptions()
                {
                    Timeout = (float)Constants.DefaultTimeout.TotalMilliseconds
                }).GetAwaiter().GetResult();
                locator.ClickAsync(new LocatorClickOptions()
                {

                }).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        bool IWebBrowser.ClickWhenAvailable(string selector, TimeSpan timeToWait, string? exceptionMessage)
        {
            //throw new NotImplementedException();
            ILocator locator = _page.Locator(selector);
            locator.ClickAsync(new LocatorClickOptions()
            {
                
            }).GetAwaiter().GetResult();
            return true;
        }

        public List<Element>? FindElements(string selector)
        {
            //IElementHandle playWrightElement = _page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            ILocator locator = _page.Locator(selector);
            //locator.WaitForAsync().GetAwaiter().GetResult();
            IReadOnlyCollection<ILocator> elements = locator.AllAsync().GetAwaiter().GetResult();
            //if (playWrightElement != null) 
            return ConvertToElements(elements, selector).ToList();
        }



        public void SendKeys(string locator, string[] keys)
        {
            throw new NotImplementedException();
        }

        public void SwitchToFrame(string locator)
        {
            //IFrame frame = _page.Frame(locator);
            //if (int.TryParse(locator, out int frameIndex)== 0) _page.MainFrame
            //IFrameLocator frameLocator = frame.FrameLocator(locator);
            
        }

        public void Wait(PageEvent pageEvent)
        {
            _page.WaitForLoadStateAsync(LoadState.Load).GetAwaiter().GetResult();
        }

        public void TakeWindowScreenShot(string fileName, FileFormat fileFormat )
        {
            throw new NotImplementedException();
        }

        public void SendKey(string locator, string key)
        {

            _page.Keyboard.PressAsync(key);
        }

        bool IWebBrowser.DoubleClick(string selector)
        {
            _page.DblClickAsync(selector).GetAwaiter().GetResult();
            return true;
        }

        #endregion Disposal / Finalization
    }
}
