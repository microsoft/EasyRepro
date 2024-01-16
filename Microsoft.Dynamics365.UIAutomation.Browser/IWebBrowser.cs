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
        void ClickWhenAvailable(string selector, TimeSpan timeToWait, string exceptionMessage);
        Element FindElement(string selector);
        object ExecuteScript(string selector, params object[] args);
        bool HasElement(string selector);
        void Navigate(string url);
        bool TryFindElement(string selector, out Element element);
        void Wait(TimeSpan? timeout = null);
        Element WaitUntilAvailable(string selector);
        Element WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage);
        Element WaitUntilAvailable(string selector, string exceptionMessage);
    }
}
