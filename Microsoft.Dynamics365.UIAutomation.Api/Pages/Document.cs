// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// The XRM Document Page that provides methods to interact with the browswer DOM. 
    /// </summary>
    /// <seealso cref="Microsoft.Dynamics365.UIAutomation.Browser.BrowserPage" />
    public class Document : XrmPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="browser">The Interactive Browser session created with the API.</param>
        public Document(InteractiveBrowser browser)
            : base(browser)
        {
           
        }

        /// <summary>
        /// Returns the document element that has the ID attribute with the specified value.
        /// </summary>
        /// <param name="id">The identifier.</param>        
        public IWebElement getElementById(string id)
        {
            return this.Execute(GetOptions($"Element Search by ID: {id}"), driver => driver.FindElement(By.Id(id))).Value;
        }

        /// <summary>
        /// Returns the document element that has the CSS attribute with the specified value.
        /// </summary>
        /// <param name="css">The CSS.</param>       
        public IWebElement getElementByCss(string css)
        {
            return this.Execute(GetOptions($"Element Search by ID: {css}"), driver => driver.FindElement(By.CssSelector(css))).Value;
        }

        /// <summary>
        /// Returns the document element that has the specified XPath value.
        /// </summary>
        /// <param name="xpath">The xpath that is used to references nodes in the document.</param>       
        public IWebElement getElementByXPath(string xpath)
        {
            return this.Execute(GetOptions($"Element Search by ID: {xpath}"), driver => driver.FindElement(By.XPath(xpath))).Value;
        }





    }
}
