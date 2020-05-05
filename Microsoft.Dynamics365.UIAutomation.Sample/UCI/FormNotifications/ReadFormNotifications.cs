using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass] 
    public class ReadFormNotifications : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        public override void NavigateToHomePage() => _xrmApp.Navigation.OpenSubArea("Sales", "Leads");

        [TestCategory("Fail - Bug")]
        [TestMethod]
        public void UCITestReadFormNotifications()
        {
            _xrmApp.CommandBar.ClickCommand("New");
            
            _xrmApp.Entity.SetValue("lastname", "Vong (sample)");
            
            _xrmApp.ThinkTime(5000);
            _xrmApp.Entity.Save();

            var notifications = _xrmApp.Entity.GetFormNotifications();

            Assert.AreEqual(1, notifications.Count);
            Assert.AreEqual(FormNotificationType.Error, notifications[0].Type);
            Assert.AreEqual("Topic : Required fields must be filled in.", notifications[0].Message);
            
        }
    }
}
