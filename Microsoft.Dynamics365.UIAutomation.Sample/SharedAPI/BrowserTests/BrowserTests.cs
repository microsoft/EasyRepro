using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Sample.SharedAPI
{
    [TestClass]
    public class OpenWebBrowserTests
    {
        private readonly string _homePageUri = "http://www.bing.com";

        [TestMethod]
        public void SharedTestOpenInternetExplorer()
        {
            using (var browser = new InteractiveBrowser(TestSettings.Options))
            {
                browser.Driver.Navigate().GoToUrl(_homePageUri);
            }
        }

        [TestMethod]
        public void SharedTestOpenChrome()
        {
            using (var browser = new InteractiveBrowser(TestSettings.Options))
            {
                browser.Driver.Navigate().GoToUrl(_homePageUri);
            }
        }

        [TestMethod]
        public void SharedTestOpenFirefox()
        {
            using (var browser = new InteractiveBrowser(TestSettings.Options))
            {
                browser.Driver.Navigate().GoToUrl(_homePageUri);
            }
        }

        [TestMethod]
        public void SharedTestOpenEdge()
        {
            using (var browser = new InteractiveBrowser(TestSettings.Options))
            {
                browser.Driver.Navigate().GoToUrl(_homePageUri);
            }
        }
    }
}