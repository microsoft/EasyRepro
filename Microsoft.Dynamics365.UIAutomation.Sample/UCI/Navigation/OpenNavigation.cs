// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenNavigationUci: TestsBase
    {
        public void ExecuteNavegationCommand(Action<Navigation> command)
        {
            var options = TestSettings.Options;
            options.UCIPerformanceMode = false;
            
            using (var xrmApp = CreateApp(options))
            {
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);
                command(xrmApp.Navigation);
                xrmApp.ThinkTime(3.Seconds());
            }
        }
        [TestMethod] public void UCITestOpenOptions() => ExecuteNavegationCommand(n => n.OpenOptions());
        [TestMethod] public void UCITestOpenPrivacy() => ExecuteNavegationCommand(n => n.OpenPrivacy());
        [TestMethod] public void UCITestSignOut() => ExecuteNavegationCommand(n => n.SignOut());
        [TestMethod] public void UCITestOptInForLearningPath() => ExecuteNavegationCommand(n => n.OpenOptInForLearningPath());
        [TestMethod] public void UCITestOpenGuidedHelp () => ExecuteNavegationCommand(n => n.OpenGuidedHelp());
        [TestMethod] public void UCITestOpenSoftwareLicensing () => ExecuteNavegationCommand(n => n.OpenSoftwareLicensing());
        [TestMethod] public void UCITestOpenToastNotifications () => ExecuteNavegationCommand(n => n.OpenToastNotifications());
        [TestMethod] public void UCITestOpenAbout () => ExecuteNavegationCommand(n => n.OpenAbout());
        [TestMethod] public void UCITest_AppSettings_PDFgeneration () => ExecuteNavegationCommand(n => n.OpenSubArea("App Settings", "PDF generation"));
    
        [TestMethod]
        public void UCITestOpenHelp()
        {
            using (var xrmApp = CreateApp())
            {
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);
                xrmApp.Navigation.OpenArea("Help and Support");
                xrmApp.Navigation.OpenSubArea("Help Center");
            }
        }

        [TestMethod]
        public void UCITestOpenRelatedCommonActivities()
        {
            using (var xrmApp = CreateApp())
            {
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenSubArea("Sales", "Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.Navigation.OpenMenu(Reference.MenuRelated.Related, Reference.MenuRelated.CommonActivities);
            }
        }

        [TestMethod]
        public void UCITestOpenGroupSubArea()
        {
            using (var xrmApp = CreateApp())
            {
                xrmApp.Navigation.OpenApp(UCIAppName.Sales);

                xrmApp.Navigation.OpenGroupSubArea("Customers", "Accounts");

                xrmApp.Grid.OpenRecord(0);

                xrmApp.ThinkTime(3000);
            }
        }
    }
}