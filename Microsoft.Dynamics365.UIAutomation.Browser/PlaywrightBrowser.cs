using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Playwright;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
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
        private string _url = "";

        public PlaywrightBrowser(IBrowser browser, BrowserOptions options)
        {
            _options = options;
            _browser = browser;

            _page = _browser.NewPageAsync().GetAwaiter().GetResult();
        }

        public BrowserOptions Options { get { return _options; } set { _options = value; } }

        public string Url { get { return _url; } set { _url = value; } }

        #region FindElement
        public Element FindElement(string selector)
        {
            return GetElement(selector);
        }
        public List<Element> FindElements(string selector)
        {
            return GetElements(selector);
        }
        #endregion

        #region ExecuteScript

        public object ExecuteScript(string script, params object[] args)
        {
            return _page.EvaluateAsync(script, args);
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

        #region SendKeys

        public void SendKey(string selector, string key)
        {
            _page.Keyboard.TypeAsync(key);
        }

        public void SendKeys(string selector, string[] keys)
        {
            _page.Keyboard.TypeAsync(keys.ToString());
        }
        #endregion

        #region SwitchToFrame

        public void SwitchToFrame(string name)
        {
            _page.Frame(name).WaitForLoadStateAsync();
        }


        #endregion

        #region TakeWindowScreenShot

        public void TakeWindowScreenShot(string filename)
        {
            _page.ScreenshotAsync(new()
            {
                Path = filename,
                FullPage = true,
            });
        }
        #endregion

        #region HasElement
        public bool HasElement(string selector)
        {
            var hasElement = GetElement(selector);
            return (hasElement != null) ? true : false;
        }
        #endregion

        #region IsAvailable
        public bool IsAvailable(string selector)
        {
            var hasElement = GetElement(selector);
            return (hasElement != null) ? true : false;
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

        #region WaitUntilAvailable
        internal void WaitUntilAvailable(string selector)
        {
            WaitForSelector(selector);
        }
        #endregion

        #region IsAvailable
        public bool IsAvailable(string selector)
        {
            var isAvailable = _page.WaitForSelectorAsync(selector).GetAwaiter().GetResult();
            return (isAvailable != null) ? true : false;
        }
        #endregion

        #region WaitUntilVailable
        Element IWebBrowser.WaitUntilAvailable(string selector)
        {
            WaitForSelector(selector);
            return GetElement(selector);
        }

        Element IWebBrowser.WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage)
        {

            try
            {
                PageWaitForSelectorOptions options = new PageWaitForSelectorOptions();
                options.Timeout = (float)timeToWait.TotalMilliseconds;
                WaitForSelector(selector, options);
            }
            catch(Exception e)
            {
                throw new Exception(exceptionMessage);
            }
            return GetElement(selector);
        }

        Element IWebBrowser.WaitUntilAvailable(string selector, string exceptionMessage)
        {
            try
            {
                WaitForSelector(selector);
            }
            catch (Exception e)
            {
                throw new Exception(exceptionMessage);
            }
            return GetElement(selector);
        }
        #endregion

        #region WaitForSelector
        internal void WaitForSelector(string selector)
        {
            _page.WaitForLoadStateAsync(LoadState.NetworkIdle).GetAwaiter().GetResult();
            //_page.WaitForLoadStateAsync(LifecycleEvent.Networkidle).GetAwaiter().GetResult();
            _page.WaitForSelectorAsync(selector,).GetAwaiter().GetResult();
        }

        internal void WaitForSelector(string selector, PageWaitForSelectorOptions options)
        {
            _page.WaitForLoadStateAsync(LoadState.NetworkIdle).GetAwaiter().GetResult();
            //_page.WaitForLoadStateAsync(LifecycleEvent.Networkidle).GetAwaiter().GetResult();
            _page.WaitForSelectorAsync(selector, options).GetAwaiter().GetResult();
        }

        internal async Task WaitForSelectorAsync(string selector)
        {
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync(selector);
        }
        #endregion

        #region GetElement
        private Element GetElement(string selector)
        {
            IElementHandle playWrightElement = _page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
            if (playWrightElement == null) { throw new PlaywrightException(String.Format("Could not find element using selector '{0}'", selector)); }
            return (Element)playWrightElement;
        }
        private List<Element> GetElements(string selector)
        {
            IList<IElementHandle> playWrightElements = (IList<IElementHandle>)_page.QuerySelectorAllAsync(selector).GetAwaiter().GetResult();
            if (playWrightElements == null) { throw new PlaywrightException(String.Format("Could not find element using selector '{0}'", selector)); }
            return new List<Element>(playWrightElements.Select(x => (Element)x));
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
