// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.PowerApps.UIAutomation.Api
{

    /// <summary>
    ///  The Home page.
    ///  </summary>
    public class Canvas
        : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public Canvas(InteractiveBrowser browser)
            : base(browser)
        {
            //The App opens in a new window.  Switch back to that window.
            browser.Driver.LastWindow();
        }

        /// <summary>
        /// Navigate To a section of the page
        /// </summary>
        /// <param name="thinkTime">Used to simulate a wait time between human interactions. The Default is 2 seconds.</param>
        public BrowserCommandResult<bool> Navigate(string AppName, int thinkTime = Constants.DefaultThinkTime)
        {
            return true;
        }
        /// <summary>
        /// Hide the Studio Welcome Dialog
        /// </summary>
        public BrowserCommandResult<bool> HideStudioWelcome(int thinkTime = Constants.DefaultThinkTime)
        {

            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Hide Studio Welcome"), driver =>
            {

                //Wait for the Skip button to be visible
                driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Canvas.CanvasSkipButton]), new TimeSpan(0, 0, 30), d =>
                  {

                    //Click the Skip button
                    var buttons = driver.FindElements(By.ClassName("button-strip"));

                      foreach (var button in buttons)
                      {
                          var thisButton = button.FindElement(By.TagName("button"));
                          if (string.Equals(thisButton.Text, "Skip", System.StringComparison.OrdinalIgnoreCase))
                          {
                              thisButton.Click();
                          }
                      }
                  }
                );

                return true;
            }
            );
        }
        public BrowserCommandResult<bool> ClickTab(string tabName, int thinkTime = Constants.DefaultThinkTime)
        {
            Browser.ThinkTime(thinkTime);

            return this.Execute(GetOptions("Canvas Click Tab: " + tabName), driver =>
            { 
                //Wait for the element to become available
                var header = driver.WaitUntilAvailable(By.XPath("//*[@id=\"appmagic-center-top-bar\"]"),new TimeSpan(0,0,30),
                                                        $"Tab Header is not available. Unable to click tab {tabName}");

                var button = header.FindElement(By.XPath($"//span[text()='{tabName}']"));
                
                button?.Click(true);

                //TODO: Add wait for Marker

                return true;
            }
            );
        }

        public BrowserCommandResult<bool> ClickRibbon(string buttonName)
        {
            return this.Execute(GetOptions("Canvas Ribbon Button: " + buttonName), driver =>
            {
                var header = driver.FindElements(By.ClassName("ribbon-tab-bar-container"))[0];
                ClickButton(header, buttonName, driver);
                return true;
            }
            );
        }
        public BrowserCommandResult<bool> ClickRibbon(string buttonName, string subMenuName= "")
        {
            return this.Execute(GetOptions("DropDownList MenuItem"), driver =>
            {
                var header = driver.FindElements(By.ClassName("ribbon-tab-bar-container"))[0];
                ClickButton(header, buttonName, driver);

                //Lookup Div Tag
                //Text Button and Forms are special cases
                if (buttonName.Equals("Forms", StringComparison.OrdinalIgnoreCase))
                    buttonName = "form";

                var subMenuContainer = driver.FindElements(By.TagName("ribbon-category-flyout")).FirstOrDefault();
                ClickButton(subMenuContainer, subMenuName, driver);

                return true;
            }
);
        }

        public BrowserCommandResult<bool> ChangeTheme(int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions("Change Canvas Color"), driver =>
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Canvas.ColorBlue]));
                
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Canvas.ColorWhite]));

                return true;
            }
            );
        }

        public BrowserCommandResult<bool> ClickButton(string name, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.Execute(GetOptions("Change Canvas Color"), driver =>
            {

                //Find the button to Click
                var button = driver.FindElement(By.XPath(Elements.Xpath[Reference.Canvas.ClickButton].Replace("[NAME]", name)));

                button.Click(driver, true);

                return true;
            }
            );
        }

        public BrowserCommandResult<bool> AddControl(CanvasControl control)
        {
            return this.Execute(GetOptions("Add Control: " + control.Name), driver =>
            {
                //Get list of controls before clicking the button
                List<IWebElement> before = GetControlsOnCanvas(driver.FindElements(By.XPath("//div[contains(@class, 'canvas')]"))[0]);

                //Click the button
                if (control.ControlType.Contains(":"))
                {
                    var name = control.ControlType.Split(':');
                    ClickRibbon(name[0],name[1]);
                }
                else
                {
                    ClickRibbon(control.ControlType);
                }

                driver.WaitUntilVisible(By.XPath("//div[contains(@class, 'contentContainer')]"));

                //Get list of controls after clicking the button
                List<IWebElement> after = GetControlsOnCanvas(driver.FindElements(By.XPath("//div[contains(@class, 'canvas')]"))[0]);
                //Find the control we just added
                List<IWebElement> addedControl = after.Except(before).ToList();

                //Change the properties of the control
                if (addedControl.Count > 0)
                {
                    var sidebar = driver.FindElements(By.Id("right-side-bar"))[0];
                    var inputs = sidebar.FindElements(By.TagName("input"));
                    var textAreas = sidebar.FindElements(By.TagName("textarea"));


                    //string X = control.ControlProperties.Find(x => x.Label.Equals("X", StringComparison.OrdinalIgnoreCase)).Value.ToString();
                    //string Y = control.ControlProperties.Find(x => x.Label.Equals("Y", StringComparison.OrdinalIgnoreCase)).Value.ToString();
                    //string Width = control.ControlProperties.Find(x => x.Label.Equals("Width", StringComparison.OrdinalIgnoreCase)).Value.ToString();
                    //string Height = control.ControlProperties.Find(x => x.Label.Equals("Height", StringComparison.OrdinalIgnoreCase)).Value.ToString();
                    //string Text = control.ControlProperties.Find(x => x.Label.Equals("Height", StringComparison.OrdinalIgnoreCase)).Value.ToString();


                    foreach (var prop in control.ControlProperties)
                    {
                        var input = inputs.FirstOrDefault(x => x.GetAttribute("title").StartsWith(prop.Label));

                        if (input != null)
                        {
                            input.Click();
                            input.Clear();
                            input.SendKeys(prop.Value.ToString());
                            input.SendKeys(Keys.Tab);
                            continue;
                        }

                        var textArea = textAreas.FirstOrDefault(x => x.GetAttribute("title").StartsWith(prop.Label));

                        if (textArea != null)
                        {
                            textArea.Click();
                            textArea.Clear();
                            textArea.SendKeys(prop.Value.ToString());
                            textArea.SendKeys(Keys.Tab);
                            continue;
                        }
                    }
                    //foreach (var inputControl in inputs)
                    //{
                    //    if (inputControl.GetAttribute("title").StartsWith("X"))
                    //    {
                    //        inputControl.Click();
                    //        inputControl.Clear();
                    //        inputControl.SendKeys(X.ToString());
                    //        inputControl.SendKeys(Keys.Tab);
                    //    }

                    //    if (inputControl.GetAttribute("title").StartsWith("Y"))
                    //    {
                    //        inputControl.Click();
                    //        inputControl.Clear();
                    //        inputControl.SendKeys(Y.ToString());
                    //        inputControl.SendKeys(Keys.Tab);
                    //    }

                    //    if (inputControl.GetAttribute("title").StartsWith("Width"))
                    //    {
                    //        inputControl.Click();
                    //        inputControl.Clear();
                    //        inputControl.SendKeys(Width.ToString());
                    //        inputControl.SendKeys(Keys.Tab);
                    //    }

                    //    if (inputControl.GetAttribute("title").StartsWith("Height"))
                    //    {
                    //        inputControl.Click();
                    //        inputControl.Clear();
                    //        inputControl.SendKeys(Height.ToString());
                    //        inputControl.SendKeys(Keys.Tab);
                    //    }
                    //}

                    /*
                    //Change the properties
                    string controlName = addedControl[0].GetAttribute("appmagic-content-control-name");

                    var sidebar = driver.FindElements(By.Id("right-side-bar"))[0];
                    var propertyGroups = sidebar.FindElements(By.XPath("//div[starts-with(@class,'propertyGroup')]]"));

                    foreach(var propertyGroup in propertyGroups)
                    {
                        var rows = propertyGroup.FindElements(By.XPath("//div/@*[starts-with(.,'row')]"));
                    }
                    */
                }

                return true;
            });


        }
        public BrowserCommandResult<bool> DragAndDrop(string controlName, int X, int Y)
        {
            return this.Execute(GetOptions("Drag and Drop"), driver =>
            {
                //FindElement?
                var canvas = driver.FindElements(By.XPath("//div[contains(@class, 'canvas')]"))[0];
                var controlsContainer = canvas.FindElements(By.XPath("//div[contains(@class, 'controlContainer')]"))[0];
                var contentContainer = controlsContainer.FindElements(By.XPath("//div[contains(@class, 'contentContainer')]"))[0];
                var controls = contentContainer.FindElements(By.XPath(String.Format("//div[contains(@appmagic-content-control-name, '{0}')]", controlName)));

                foreach(var control in controls)
                {
                    control.DragAndDrop(driver, 80, 80);


                    //find the x and y fields on the right hand pane
                    var sidebar = driver.FindElements(By.Id("right-side-bar"))[0];
                    var inputs = sidebar.FindElements(By.TagName("input"));

                    foreach(var inputControl in inputs)
                    {
                        if(inputControl.GetAttribute("title").StartsWith("X"))
                        {
                            inputControl.Click();
                            inputControl.Clear();
                            inputControl.SendKeys(X.ToString());
                            inputControl.SendKeys(Keys.Tab);
                        }

                        if (inputControl.GetAttribute("title").StartsWith("Y"))
                        {
                            inputControl.Click();
                            inputControl.Clear();
                            inputControl.SendKeys(Y.ToString());
                            inputControl.SendKeys(Keys.Tab);
                        }
                    }

                }

                return true;
            });
        }
        internal void ClickButton(IWebElement container, string name, IWebDriver driver)
        {
            var buttons = container.FindElements(By.TagName("button"));

            foreach (var button in buttons)
            {
                //Find the Label of the control
                string buttonText = button.FindElement(By.TagName("span")).Text;

                if (string.Equals(buttonText, name, StringComparison.OrdinalIgnoreCase))
                {
                    button.Click();
                    break;
                }
            }
        }
        internal List<IWebElement> GetControlsOnCanvas(IWebElement canvas)
        {
            List<IWebElement> controlList = new List<IWebElement>();

            var controlsContainer = canvas.FindElements(By.XPath("//div[contains(@class, 'controlContainer')]"))[0];
            var contentContainer = controlsContainer.FindElements(By.XPath("//div[contains(@class, 'contentContainer')]"));
            if (contentContainer.Count > 0)
            {
                //There's already controls on the Canvas.  If = 0, then this is the first control
                var controls = controlsContainer.FindElements(By.XPath("//div[@*[contains(name(), 'appmagic-content-control-name')]]"));
                foreach (var control in controls)
                {
                    controlList.Add(control);
                }
            }

            return controlList;
        }
    }
}