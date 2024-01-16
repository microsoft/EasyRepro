using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public static class ElementExtensions
    {
        public static Element FindElement(this Element element, string selector)
        {
            throw new NotImplementedException("TBD");
        }
        public static Element? FindElements(this Element? element, string selector)
        {
            throw new NotImplementedException("TBD");
        }
        public static Element TryFindElement(this Element element, string selector, ref Element parentElement)
        {
            throw new NotImplementedException("TBD");
        }
        public static Element WaitUntilAvailable(this Element element, string selector)
        {
            throw new NotImplementedException("TBD");
        }
        public static Element WaitUntilAvailable(this Element element, string selector, string exceptionMessage)
        {
            throw new NotImplementedException("TBD");
        }
    }
}
