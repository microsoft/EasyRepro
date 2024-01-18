﻿using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public interface IWebBrowser
    {
        public string Url { get; set; }
        Element ClickWhenAvailable(string selector);
        Element ClickWhenAvailable(string selector, TimeSpan timeToWait, string? exceptionMessage = null);
        Element FindElement(string selector);
        List<Element>? FindElements(string selector);
        object ExecuteScript(string selector, params object[] args);
        bool HasElement(string selector);
        void Navigate(string url);
        void SendKeys(string[] keys);
        void SwitchToFrame(string locator);
        //bool TryFindElement(string selector, out Element element);
        void Wait(TimeSpan? timeout = null);
        void Wait(PageEvent pageEvent);
        Element WaitUntilAvailable(string selector);
        Element WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage);
        Element WaitUntilAvailable(string selector, string exceptionMessage);
    }
}