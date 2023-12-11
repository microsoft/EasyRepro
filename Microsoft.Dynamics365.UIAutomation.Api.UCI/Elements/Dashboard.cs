// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Dashboard : Element
    {

        #region DTO
        public static class DashboardReference
        {
            public static string DashboardSelector = "//span[contains(@id, 'Dashboard_Selector')]";
            public static string DashboardItem = "//li[contains(@title, '[NAME]')]";
            public static string DashboardItemUCI = "//li[contains(@data-text, '[NAME]')]";
        }
        #endregion

        private readonly WebClient _client;

        public SubGrid SubGrid => this.GetElement<SubGrid>(_client);

        public Dashboard(WebClient client)
        {
            _client = client;
        }

        #region public
        public T GetElement<T>(WebClient client)
            where T : Element
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { client });
        }

        /// <summary>
        /// Selects the Dashboard provided
        /// </summary>
        /// <param name="dashboardName">Name of the dashboard to select</param>
        public void SelectDashboard(string dashboardName)
        {
            this.SelectDashboard(dashboardName);
        }
        #endregion

        #region Dashboard

        internal BrowserCommandResult<bool> SelectDashboard(string dashboardName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute(_client.GetOptions($"Select Dashboard"), driver =>
            {
                //Click the drop-down arrow
                driver.ClickWhenAvailable(By.XPath(DashboardReference.DashboardSelector));
                //Select the dashboard
                driver.ClickWhenAvailable(By.XPath(DashboardReference.DashboardItemUCI.Replace("[NAME]", dashboardName)));

                // Wait for Dashboard to load
                driver.WaitForTransaction();

                return true;
            });
        }

        #endregion
    }
}
