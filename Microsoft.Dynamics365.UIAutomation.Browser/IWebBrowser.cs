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
        string Url { get; set; }
        Element FindElement(string selector);
        List<Element>? FindElements(string selector);
        object ExecuteScript(string script, params object[] args);
        void Navigate(string url);
        void SendKeys(string selector, string[] keys);
        void SendKey(string selector, string key);
        void SwitchToFrame(string name);
        void TakeWindowScreenShot(string fileName);
        bool HasElement(string selector);
        bool IsAvailable(string selector);
        void Wait(TimeSpan timeout);
        void Wait(int milliseconds);
        Element WaitUntilAvailable(string selector);
        Element WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage);
        Element WaitUntilAvailable(string selector, string exceptionMessage);
    }
}
