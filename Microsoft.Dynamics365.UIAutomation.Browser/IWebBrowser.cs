using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public interface IWebBrowser
    {
        void Clear(string selector);
        void Click(string selector);
        void ClickWhenAvailable(string selector, TimeSpan timeToWait, string exceptionMessage);
        void DoubleClick(string selector);
        Element FindElement(string selector);
        void Focus(string selector);
        object ExecuteScript(string selector, params object[] args);
        bool HasElement(string selector);
        bool IsAvailable(string selector);
        void Navigate(string url);
        void SendKeys(string selector, string key);
        void SetValue(string selector, string value);
        bool TryFindElement(string selector, out Element element);
        void Wait(TimeSpan? timeout = null);
        Element WaitUntilAvailable(string selector);
        Element WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage);
        Element WaitUntilAvailable(string selector, string exceptionMessage);
    }
}
