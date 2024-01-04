using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class Element : IWebElement
    {
        //private InteractiveBrowser _browser;
        //public Element(InteractiveBrowser Browser)
        //{
        //    _browser = Browser;
        //}


        //public void ThinkTime(int milliseconds)
        //{
        //    _browser.ThinkTime(milliseconds);
        //}

        //public void ThinkTime(TimeSpan timespan)
        //{
        //    ThinkTime((int)timespan.TotalMilliseconds);
        //}


        //public void WaitForLoadArea(IWebDriver driver)
        //{
        //    driver.WaitForPageToLoad();
        //    driver.WaitForTransaction();
        //}

        //public void Dispose()
        //{
        //    _browser.Dispose();
        //}
        public string TagName => throw new NotImplementedException();

        public string Text => throw new NotImplementedException();

        public bool Enabled => throw new NotImplementedException();

        public bool Selected => throw new NotImplementedException();

        public Point Location => throw new NotImplementedException();

        public Size Size => throw new NotImplementedException();

        public bool Displayed => throw new NotImplementedException();

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            throw new NotImplementedException();
        }

        public IWebElement FindElement(By by)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string attributeName)
        {
            throw new NotImplementedException();
        }

        public string GetCssValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public string GetDomAttribute(string attributeName)
        {
            throw new NotImplementedException();
        }

        public string GetDomProperty(string propertyName)
        {
            throw new NotImplementedException();
        }

        public ISearchContext GetShadowRoot()
        {
            throw new NotImplementedException();
        }

        public void SendKeys(string text)
        {
            throw new NotImplementedException();
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }
    }
}
