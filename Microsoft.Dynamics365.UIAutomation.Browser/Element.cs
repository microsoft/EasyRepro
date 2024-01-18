﻿using Microsoft.Playwright;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text.Json;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class Element //: IWebElement, IElementHandle
    {
        #region properties
        public string Locator { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsClickable { get; set; }
        public bool Selected { get; set; }
        public string Text { get; set; }
        #endregion

        public Element()
        {
        }

        #region methods
        public void Clear(BrowserPage page, string key)
        {
            page.Execute(new BrowserCommandOptions(), browser =>
            {
                return true;
            });
        }

        public void Click(BrowserPage page, bool? click = true)
        {

        }
        public void DoubleClick(BrowserPage page, string key)
        {

        }
        public string GetAttribute(BrowserPage page, string attributeName)
        {
            return "";
        }
        public bool HasAttribute(BrowserPage page, string attributeName)
        {
            return true;
        }
        public void Focus(BrowserPage page, string key)
        {

        }
        public void SendKeys(BrowserPage page, string key)
        {

        }
        public virtual void SetValue(BrowserPage page, string value)
        {
            page.Execute(new BrowserCommandOptions(), browser =>
            {
                return true;
            });
        }
        #endregion
    }

}
