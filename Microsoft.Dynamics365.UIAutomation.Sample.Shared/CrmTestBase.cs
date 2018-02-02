using System.Globalization;
using System.Threading;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Shared
{
    public abstract class CrmTestBase
    {
        protected CrmTestBase()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pl-PL");
        }
    }
}