using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class Login : TestsBase
    {
        [TestInitialize]
        public override void InitTest() => base.InitTest();

        [TestCleanup]
        public override void FinishTest() => base.FinishTest();

        [TestMethod]
        public void MultiFactorLogin()
        {
           trace.Log("Login success");
        }

        [TestMethod]
        public void MultiFactorLogin_NavigateToApp()
        {
            trace.Log("Login success");
            NavigateTo(UCIAppName.Sales);
        }
        
        [TestMethod]
        public void MultiFactorLogin_NavigateToApp_CustomerService()
        {
            trace.Log("Login success");

            NavigateTo(UCIAppName.CustomerService);
            trace.Log("Open Customer Service Success");
        }

        [TestMethod]
        public void MultiFactorLogin_NavigateToApp_ChangeApp()
        {
            trace.Log("Login success");
            NavigateTo(UCIAppName.Sales);

            trace.Log("Open Sales Success");
            
            NavigateTo(UCIAppName.CustomerService);
            trace.Log("Open Customer Service Success");
        }
    }
}