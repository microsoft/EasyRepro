// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class AreasAndSubAreasUci : TestsBase
    {
        /// <summary>
        /// Test that only those areas that are expected appear on the are menu
        /// </summary>
        [TestCategory("Fail - ExpectedAssertion")]
        [TestMethod]
        public void TestExpectedAreasArePresent()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                List<string> expectedAreas = new List<string> { "Accounts", "Contacts" };

                Dictionary<string, IWebElement> areas = client.OpenMenu();

                List<string> actualAreas = new List<string>();

                foreach (var subAreaMenuItem in areas)
                {
                    actualAreas.Add(subAreaMenuItem.Key);
                }

                Assert.IsTrue(TwoListsAreTheSame(expectedAreas, actualAreas));
            }
        }

        /// <summary>
        /// Test that only expected subareas are visible to the user
        /// </summary>
        [TestCategory("Fail - ExpectedAssertion")]
        [TestMethod]
        public void TestExpectedSubAreasArePresent()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);

                List<string> expectedSubAreas = new List<string> { "Home","Recent","Pinned","Dashboards","Activities","Accounts","Contacts","Social Profiles","Cases","Queues","Knowledge Articles"};

                Dictionary<string, IWebElement> subAreaMenuItems = WebClient.GetSubAreaMenuItems(client.Browser.Driver);

                List<string> actualSubAreas = new List<string>();

                foreach (var subAreaMenuItem in subAreaMenuItems)
                {
                    actualSubAreas.Add(subAreaMenuItem.Key);
                }

                Assert.IsTrue(TwoListsAreTheSame(expectedSubAreas, actualSubAreas));
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