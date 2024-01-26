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
        public string Url { get; set; }

        IElement FindElement(string selector);
        List<IElement>? FindElements(string selector);
        object ExecuteScript(string selector, params object[] args);
        bool HasElement(string selector);
        void Navigate(string url);
        void SendKeys(string locator, string[] keys);
        void SendKey(string locator, string key);
        void SwitchToFrame(string locator);
        void TakeWindowScreenShot(string fileName, FileFormat fileFormat);
        //bool TryFindElement(string selector, out Element element);
        void Wait(TimeSpan? timeout = null);
        void Wait(PageEvent pageEvent);
        IElement? WaitUntilAvailable(string selector);
        IElement WaitUntilAvailable(string selector, TimeSpan timeToWait, string exceptionMessage);
        IElement WaitUntilAvailable(string selector, string exceptionMessage);
        bool ClickWhenAvailable(string selector);//element
        bool ClickWhenAvailable(string selector, TimeSpan timeToWait, string? exceptionMessage = null);//element
    }
}
