// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium.Support.Events;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    using System;
    using System.Linq;
    using System.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Dynamics365.UIAutomation.Api;
    using Microsoft.Dynamics365.UIAutomation.Browser;

    public abstract class TestBase
    {
        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"]);
        protected Browser XrmTestBrowser;

        public bool HasData { get; set; }

        protected TestBase()
        {
            XrmTestBrowser = new Browser(TestSettings.Options);
            XrmTestBrowser.LoginPage.Login(_xrmUri, _username, _password);
            XrmTestBrowser.GuidedHelp.CloseGuidedHelp();
        }

        public abstract void TestSetup();

        [TestCleanup]
        public void TestCleanup()
        {
            XrmTestBrowser.Dispose();
        }

        public void OpenEntity(string area, string subArea, string view = null)
        {
            XrmTestBrowser.ThinkTime(500);
            XrmTestBrowser.Navigation.OpenSubArea(area, subArea);
            if (view != null) XrmTestBrowser.Grid.SwitchView(view);
        }

        public bool OpenEntityGrid()
        {
            var entityGrid = XrmTestBrowser.Grid.GetGridItems(20).Value;
            if (entityGrid?.FirstOrDefault() != null)
            {
                XrmTestBrowser.Entity.OpenEntity(entityGrid[0].EntityName, entityGrid[0].Id);
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
