using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// Xrm Pending Email Reminder Page
    /// </summary>
    public class PendingEmail : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PendingEmail"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public PendingEmail(InteractiveBrowser browser) : base(browser)
        {
            SwitchToDefault();
        }

        public bool IsShown
        {
            get
            {
                var inlineDialog = this.Browser.Driver.FindElements(By.Id(Elements.ElementId[Reference.PendingEmail.InlineDialog]));

                if (inlineDialog.Any())
                {
                    String dialogSrc = inlineDialog.First().GetAttribute("src");

                    return dialogSrc != null && dialogSrc.EndsWith(Elements.ElementId[Reference.PendingEmail.InlineDialogUri]);
                }

                return false;
            }
        }

        /// <summary>
        /// Closes the Guided Help
        /// </summary>
        /// <example>xrmBrowser.PendingEmail.Close();</example>
        public BrowserCommandResult<bool> Close(int thinkTime = Constants.DefaultThinkTime)
        {
            this.Browser.ThinkTime(thinkTime);
            return this.Execute(GetOptions("Close Pending Email Reminder"), driver =>
            {
                bool returnValue = false;

                if (IsShown)
                {
                    var inlineDialog = driver.FindElement(By.Id(Elements.ElementId[Reference.PendingEmail.InlineDialog]));
                    driver.SwitchTo().Frame(inlineDialog);

                    driver.WaitUntilClickable(By.Id(Elements.ElementId[Reference.PendingEmail.ButBegin]), new TimeSpan(0, 0, 5), d =>
                    {
                        var element = d.FindElement(By.Id(Elements.ElementId[Reference.PendingEmail.ButBegin]));
                        element.Click(true);

                        returnValue = true;
                    });

                    driver.SwitchTo().DefaultContent();
                }

                return returnValue;
            });
        }
    }
}
