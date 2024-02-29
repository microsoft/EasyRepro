﻿using System.Collections.ObjectModel;
using System.Drawing;
using System.Text.Json;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public interface IElement
    {
        public string Tag { get; set; }
        public string Locator { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsClickable { get; set; }
        public bool Selected { get; set; }
        public string Text { get; set; }

        public void Clear(BrowserPage page, string key);
        //public bool ClickWhenAvailable(BrowserPage page, string key);
        //public bool ClickWhenAvailable(BrowserPage page, string key, TimeSpan timeToWait, string? exceptionMessage = null);
        public void Click(BrowserPage page, bool? click = true);
        public void DoubleClick(BrowserPage page, string key);
        public string GetAttribute(BrowserPage page, string attributeName);
        public bool HasAttribute(BrowserPage page, string attributeName);
        public void Hover(BrowserPage page, string key);
        public void Focus(BrowserPage page, string key);
        public void SendKeys(BrowserPage page, string[] keys);
        public void SetValue(BrowserPage page, string value);
    }
    //public class Element : IElement
    //{
    //    #region properties
    //    public string Tag { get; set; }
    //    public string Locator { get; set; }
    //    public string Id { get; set; }
    //    public string Value { get; set; }
    //    public bool IsAvailable { get; set; }
    //    public bool IsClickable { get; set; }
    //    public bool Selected { get; set; }
    //    public string Text { get; set; }
    //    #endregion

    //    //not recommended.

    //    public Element()
    //    {
    //    }

    //    #region methods
    //    public void Clear(BrowserPage page, string key)
    //    {
    //        page.Execute(new BrowserCommandOptions(), browser =>
    //        {
    //            return true;
    //        });
    //    }
    //    public bool ClickWhenAvailable(BrowserPage page, string key)
    //    {
    //        throw new  NotImplementedException("TBD");


    //    }
    //    public bool ClickWhenAvailable(BrowserPage page, string key, TimeSpan timeToWait, string? exceptionMessage = null)
    //    {
    //        throw new NotImplementedException("TBD");


    //    }
    //    public void Click(BrowserPage page, bool? click = true)
    //    {
    //        page.Execute(new BrowserCommandOptions(), browser =>
    //        {
    //            browser.ClickWhenAvailable(this.Locator);
    //            return true;
    //        });
    //    }
    //    public void DoubleClick(BrowserPage page, string key)
    //    {
    //        page.Execute(new BrowserCommandOptions(), browser =>
    //        {
    //            browser.DoubleClick(key);
    //            return true;
    //        });
    //    }
    //    public string GetAttribute(BrowserPage page, string attributeName)
    //    {
    //        page.Execute(new BrowserCommandOptions(), browser =>
    //        {
    //            return "";
    //            //return browser.FindElementAttribute(this.Locator, attributeName);

    //        });
    //        return "";
    //    }
    //    public bool HasAttribute(BrowserPage page, string attributeName)
    //    {
    //        return true;
    //    }
    //    public void Hover(BrowserPage page, string key) { }
    //    public void Focus(BrowserPage page, string key)
    //    {

    //    }
    //    public void SendKeys(BrowserPage page, string[] keys)
    //    {
    //        page.Execute(new BrowserCommandOptions(), browser =>
    //        {
    //            browser.SendKeys(this.Locator, keys);
    //            return true;
    //        });
    //    }
    //    public virtual void SetValue(BrowserPage page, string value)
    //    {
    //        page.Execute(new BrowserCommandOptions(), browser =>
    //        {
    //            browser.SetValue(this.Locator, value);
    //            return true;
    //        });
    //    }

    //    public void Test(BrowserPage page, string value)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    #endregion
    //}

}