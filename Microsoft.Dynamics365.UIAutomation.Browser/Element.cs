using Microsoft.Playwright;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text.Json;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class Element //: IWebElement, IElementHandle
    {
        private IWebBrowser _browser;
        private BrowserPage _page;
        #region properties
        public string Id { get; set; }
        public string Value { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsClickable { get; set; }
        #endregion

        public Element(BrowserPage page)
        {
            _page = page;
        }

        #region methods
        public void Clear()
        {
            
            _page.Execute(new BrowserCommandOptions(), browser =>
            {
                return true;
            });
        }

        public void Click()
        {

        }
        public void DoubleClick()
        {

        }
        public string GetAttribute(string attributeName)
        {
            return "";
        }
        public void Focus()
        {

        }
        public void SendKeys()
        {

        }
        public void SetValue()
        {

        }
        #endregion
    }

}
