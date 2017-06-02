using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.UnitTests.Sample
{
    public static class TestSettings
    {
        public static string AccountLogicalName = "account";
        public static string AccountId = "BD8AC246-2416-E711-8104-FC15B4282DF4";

        public static string LookupField = "primarycontactid";
        public static string LookupName = "Rene Valdes (sample)";

        public static BrowserOptions Options = new BrowserOptions
                                                {
                                                    BrowserType = BrowserType.Chrome,
                                                    PrivateMode = true,
                                                    FireEvents = true
                                                };
    }
}
