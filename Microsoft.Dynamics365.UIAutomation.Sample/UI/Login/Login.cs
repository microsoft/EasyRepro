using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    [TestClass]
    public class Login : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        [TestCategory("Login")]
        [TestMethod]
        public void MultiFactorLogin()
        {
           trace.Log("Login success");
        }

        [TestCategory("Login")]
        [TestMethod]
        public void MultiFactorLogin_NavigateToApp()
        {
            trace.Log("Login success");
            NavigateTo(AppName.Sales);
        }

        [TestCategory("Login")]
        [TestMethod]
        public void MultiFactorLogin_NavigateToApp_CustomerService()
        {
            trace.Log("Login success");

            NavigateTo(AppName.CustomerService);
            trace.Log("Open Customer Service Success");
        }

        [TestCategory("Login")]
        [TestMethod]
        public void MultiFactorLogin_NavigateToApp_ChangeApp()
        {
            trace.Log("Login success");
            NavigateTo(AppName.Sales);

            trace.Log("Open Sales Success");
            
            NavigateTo(AppName.CustomerService);
            trace.Log("Open Customer Service Success");
        }
    }
}