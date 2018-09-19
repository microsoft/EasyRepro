using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    public static class TestSettings
    {
        public static BrowserOptions Options = new BrowserOptions
                                                {
                                                    BrowserType = BrowserType.Chrome,
                                                    PrivateMode = true,
                                                    FireEvents = true
                                                };
    }
}
