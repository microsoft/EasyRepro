using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web.Navigation
{
    [TestClass]
    public class AreasAndSubAreas
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"]);

        [TestMethod]
        public void CheckSubAreasAvailableWithinSalesArea()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.ThinkTime(500);

                List<string> expectedSubAreas = new List<string> { "leads", "opportunities", "competitors" };

                List<KeyValuePair<string, IWebElement>> subareas = xrmBrowser.Navigation.GetSubAreas("Sales");
                List<string> actualSubAreas = new List<string>();
                foreach (var subarea in subareas)
                {
                  actualSubAreas.Add(subarea.Key);  
                }

                Assert.IsTrue(TwoListsAreTheSame(expectedSubAreas, actualSubAreas));
            }
        }

        [TestMethod]
        public void CheckAreasAvailableOnMenu()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.ThinkTime(500);

                Dictionary<string, IWebElement> areas = xrmBrowser.Navigation.GetAreas();

                List<string> expectedAreas = new List<string> {"sales", "workplace", "training", "settings"};

                List<string> actualAreas = new List<string>();

                foreach (var subarea in areas)
                {
                    actualAreas.Add(subarea.Key);
                }

                Assert.IsTrue(TwoListsAreTheSame(expectedAreas, actualAreas));
            }
        }

        private bool TwoListsAreTheSame(List<string> expectedList, List<string> actualList)
        {
            bool result = true;

            if (actualList.Except(expectedList).Any())
            {
                result = false;
                Console.WriteLine(@"These items were in the expected list but not the actual list.");
                
                foreach (var listItem in expectedList.Except(actualList))
                {
                    Console.WriteLine(listItem);
                }
            }

            if (actualList.Except(expectedList).Any())
            {
                result = false;
                Console.WriteLine(@"These items were in the actual list but not the expected list.");

                foreach (var listItem in actualList.Except(expectedList))
                {
                    Console.WriteLine(listItem);
                }
            }
            return result;
        }
    }
}