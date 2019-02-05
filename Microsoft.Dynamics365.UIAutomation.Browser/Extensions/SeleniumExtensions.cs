// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public static class SeleniumExtensions
    {
        #region Click

        public static IWebDriver ClickndWait(this IWebDriver driver, By by, TimeSpan timeout)
        {
            var element = driver.FindElement(by);

            if (element != null)
            {
                element.Click();
                System.Threading.Thread.Sleep((int)timeout.TotalMilliseconds);
            }

            return driver;
        }

        public static void Click(this IWebElement element, bool ignoreStaleElementException = true)
        {
            try
            {
                element.Click();
            }
            catch(StaleElementReferenceException ex)
            {
                if (!ignoreStaleElementException)
                    throw ex;
            }
        }

        public static void Hover(this IWebElement Element, IWebDriver driver, bool ignoreStaleElementException = true)
        {
            try
            {
                Actions action = new Actions(driver);
                action.MoveToElement(Element).Build().Perform();
            }
            catch (StaleElementReferenceException)
            {
                if (!ignoreStaleElementException)
                    throw;
            }
        }

        public static IWebElement ClickWhenAvailable(this IWebDriver driver, By by)
        {
            return ClickWhenAvailable(driver, by, Constants.DefaultTimeout);
        }

        public static IWebElement ClickWhenAvailable(this IWebDriver driver, By by, TimeSpan timeout)
        {
            var element = driver.FindElement(by);

            WaitUntilClickable(driver,
                                by,
                                timeout,
                                d => { element.Click(true); },
                                e => { throw new InvalidOperationException($"Unable to click element."); });



            return element;
        }

        #endregion Click

        #region Double Click

        public static void DoubleClick(this IWebDriver driver, IWebElement element, bool ignoreStaleElementException = false)
        {
            try
            {
                Actions actions = new Actions(driver);
                actions.DoubleClick(element).Perform();
            }
            catch (StaleElementReferenceException ex)
            {
                if (!ignoreStaleElementException)
                    throw ex;
            }
        }

        public static void DoubleClick(this IWebDriver driver, By by, bool ignoreStaleElementException = false)
        {
            try
            {
                var element = driver.FindElement(by);
                driver.DoubleClick(element, ignoreStaleElementException);
            }
            catch (StaleElementReferenceException ex)
            {
                if (!ignoreStaleElementException)
                    throw ex;
            }
        }

        #endregion

        #region Script Execution

        [DebuggerNonUserCode()]
        public static object ExecuteScript(this IWebDriver driver, string script, params object[] args)
        {
            var scriptExecutor = (driver as IJavaScriptExecutor);

            if (scriptExecutor == null)
                throw new InvalidOperationException(
                    $"The driver type '{driver.GetType().FullName}' does not support Javascript execution.");

            return scriptExecutor.ExecuteScript(script, args);
        }

        [DebuggerNonUserCode()]
        public static JObject GetJsonObject(this IWebDriver driver, string @object)
        {
            @object = SanitizeReturnStatement(@object);

            var results = ExecuteScript(driver, $"return JSON.stringify({@object});").ToString();

            return JObject.Parse(results);
        }

        [DebuggerNonUserCode()]
        public static JArray GetJsonArray(this IWebDriver driver, string @object)
        {
            @object = SanitizeReturnStatement(@object);

            var results = ExecuteScript(driver, $"return JSON.stringify({@object});").ToString();

            return JArray.Parse(results);
        }

        [DebuggerNonUserCode()]
        public static T GetJsonObject<T>(this IWebDriver driver, string @object)
        {
            @object = SanitizeReturnStatement(@object);

            var results = ExecuteScript(driver, $"return JSON.stringify({@object});").ToString();
            var jsSerializer = new JavaScriptSerializer();

            jsSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            var jsonObj = new JavaScriptSerializer().Deserialize<T>(results);

            return jsonObj;
        }

        private static string SanitizeReturnStatement(string script)
        {
            if (script.EndsWith(";"))
            {
                script = script.TrimEnd(script[script.Length - 1]);
            }

            if (script.StartsWith("return "))
            {
                script = script.TrimStart("return ".ToCharArray());
            }

            return script;
        }

        #endregion Script Execution

        #region Browser Options

        [DebuggerNonUserCode()]
        public static void ResetZoom(this IWebDriver driver)
        {
            IWebElement element = driver.FindElement(By.TagName("body"));
            element.SendKeys(Keys.Control + "0");
        }

        #endregion Browser Options

        #region Screenshot

        [DebuggerNonUserCode()]
        public static Screenshot TakeScreenshot(this IWebDriver driver)
        {
            var screenshotDriver = (driver as ITakesScreenshot);

            if (screenshotDriver == null)
                throw new InvalidOperationException(
                    $"The driver type '{driver.GetType().FullName}' does not support taking screenshots.");

            return screenshotDriver.GetScreenshot();
        }

        [DebuggerNonUserCode()]
        public static Bitmap TakeScreenshot(this IWebDriver driver, By by)
        {
            var screenshot = TakeScreenshot(driver);
            var bmpScreen = new Bitmap(new MemoryStream(screenshot.AsByteArray));

            // Measure the location of a specific element
            IWebElement element = driver.FindElement(by);
            var crop = new Rectangle(element.Location, element.Size);

            return bmpScreen.Clone(crop, bmpScreen.PixelFormat);
        }

        #endregion Screenshot

        #region Elements

        public static T GetAttribute<T>(this IWebElement element, string attributeName)
        {
            string value = element.GetAttribute(attributeName) ?? string.Empty;

            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
        }

        public static string GetAuthority(this IWebDriver driver)
        {
            string url = driver.Url;                // get the current URL (full)
            Uri currentUri = new Uri(url);          // create a Uri instance of it
            string baseUrl = currentUri.Authority;  // just get the "base" bit of the URL

            return baseUrl;
        }

        public static string GetBodyText(this IWebDriver driver)
        {
            return driver.FindElement(By.TagName("body")).Text;
        }

        public static bool HasAttribute(this IWebElement element, string attributeName)
        {
            return element.GetAttribute(attributeName) == null ? false : true;
        }

        public static bool HasElement(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElements(by).Count > 0;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static bool HasElement(this IWebElement element, By by)
        {
            try
            {
                return element.FindElements(by).Count > 0;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static bool IsVisible(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static bool IsVisible(this IWebElement element, By by)
        {
            try
            {
                return element.FindElement(by).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static void SetVisible(this IWebDriver driver, By by, bool visible)
        {
            IWebElement element = driver.FindElement(by);
            if(visible)
                driver.ExecuteScript($"document.getElementById('{element.GetAttribute("Id")}').setAttribute('style', 'display: inline;')");
            else
                driver.ExecuteScript($"document.getElementById('{element.GetAttribute("Id")}').setAttribute('style', 'display: none;')");
        }

        public static void SendKeys(this IWebElement element, string value, bool clear)
        {
            if (clear)
            {
                element.Clear();
                element.SendKeys(value);
            }
            else
            {
                element.SendKeys(value);
            }
        }

        public static bool AlertIsPresent(this IWebDriver driver)
        {
            return AlertIsPresent(driver, new TimeSpan(0, 0, 2));
        }

        public static bool AlertIsPresent(this IWebDriver driver, TimeSpan timeout)
        {
            var returnvalue = false;

            WebDriverWait wait = new WebDriverWait(driver, timeout);

            try
            {
                wait.Until(ExpectedConditions.AlertIsPresent());

                returnvalue = true;
            }
            catch (NoSuchElementException)
            {
                returnvalue = false;
            }
            catch (WebDriverTimeoutException)
            {
                returnvalue = false;
            }

            return returnvalue;

        }

        public static IWebDriver LastWindow(this IWebDriver driver)
        {
            return driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        #endregion Elements

        #region Waits

        public static bool WaitFor(this IWebDriver driver, Predicate<IWebDriver> predicate)
        {
            return WaitFor(driver, predicate, Constants.DefaultTimeout);
        }

        public static bool WaitFor(this IWebDriver driver, Predicate<IWebDriver> predicate, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);

            var result = wait.Until(d => predicate(d));

            return result;
        }

        public static bool WaitForPageToLoad(this IWebDriver driver)
        {
            return WaitForPageToLoad(driver, Constants.DefaultTimeout.Seconds);
        }

        public static bool WaitForTransaction(this IWebDriver driver)
        {
            return WaitForTransaction(driver, Constants.DefaultTimeout.Seconds);
        }

        //public static bool WaitForPageToLoad(this IWebDriver driver, TimeSpan timeout)
        //{
        //    object readyState = WaitForScript(driver, "if (document.readyState) return document.readyState;", timeout);

        //    if (readyState != null)
        //        return readyState.ToString().ToLower() == "complete";

        //    return false;
        //}

        public static bool WaitForPageToLoad(this IWebDriver driver, int maxWaitTimeInSeconds)
        {
            string state = string.Empty;
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(maxWaitTimeInSeconds));

                //Checks every 500 ms whether predicate returns true if returns exit otherwise keep trying till it returns ture
                wait.Until(d => {

                    try
                    {
                        state = ((IJavaScriptExecutor)driver).ExecuteScript(@"return document.readyState").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        //Ignore
                    }
                    catch (NoSuchWindowException)
                    {
                        //when popup is closed, switch to last windows
                        driver.SwitchTo().Window(driver.WindowHandles.Last(driver));
                    }
                    //In IE7 there are chances we may get state as loaded instead of complete
                    return (state.Equals("complete", StringComparison.InvariantCultureIgnoreCase));

                });
            }
            catch (TimeoutException)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw;
            }
            catch (NullReferenceException)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw;
            }
            catch (WebDriverException)
            {
                if (driver.WindowHandles.Count == 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                }
                state = ((IJavaScriptExecutor)driver).ExecuteScript(@"return document.readyState").ToString();
                if (!(state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase)))
                    throw;
            }
            return true;
        }

        public static bool WaitForTransaction(this IWebDriver driver, int maxWaitTimeInSeconds)
        {
            bool state = false;
            try
            {
                //Poll every half second to see if UCI is idle
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(500));
                wait.Until(d =>
                {
                    try
                    {
                        //Check to see if UCI is idle
                        state = (bool)driver.ExecuteScript("return window.UCWorkBlockTracker.isAppIdle()", "");
                    }
                    catch (TimeoutException)
                    {

                    }
                    catch (NullReferenceException)
                    {

                    }

                    return state;
                });
            }
            catch(Exception)
            {

            }
           
            return state;
        }
        public static string Last(this System.Collections.ObjectModel.ReadOnlyCollection<string> handles, IWebDriver driver)
        {
            return handles[handles.Count - 1];
        }
        public static object WaitForScript(this IWebDriver driver, string script)
        {
            return WaitForScript(driver, script, Constants.DefaultTimeout);
        }

        public static object WaitForScript(this IWebDriver driver, string script, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);

            wait.Until((d) =>
            {
                try
                {
                    object returnValue = ExecuteScript(driver, script);

                    return returnValue;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
                catch (WebDriverException)
                {
                    return null;
                }
            });

            return null;
        }

        public static IWebElement WaitUntilAvailable(this IWebDriver driver, By by)
        {
            return WaitUntilAvailable(driver, by, Constants.DefaultTimeout, null, null);
        }

        public static IWebElement WaitUntilAvailable(this IWebDriver driver, By by, TimeSpan timeout)
        {
            return WaitUntilAvailable(driver, by, timeout, null, null);
        }

        public static IWebElement WaitUntilAvailable(this IWebDriver driver, By by, string exceptionMessage)
        {
            return WaitUntilAvailable(driver, by, Constants.DefaultTimeout, null, d =>
            {
                throw new InvalidOperationException(exceptionMessage);
            });
        }

        public static IWebElement WaitUntilAvailable(this IWebDriver driver, By by, TimeSpan timeout, string exceptionMessage)
        {
            return WaitUntilAvailable(driver, by, timeout, null, d =>
            {
                throw new InvalidOperationException(exceptionMessage);
            });
        }

        public static IWebElement WaitUntilAvailable(this IWebDriver driver, By by, TimeSpan timeout, Action<IWebDriver> successCallback)
        {
            return WaitUntilAvailable(driver, by, timeout, successCallback, null);
        }

        public static IWebElement WaitUntilAvailable(this IWebDriver driver, By by, TimeSpan timeout, Action<IWebDriver> successCallback, Action<IWebDriver> failureCallback)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            bool? success;
            IWebElement returnElement = null;

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            try
            {
                returnElement = wait.Until(d => d.FindElement(by));

                success = true;
            }
            catch (NoSuchElementException)
            {
                success = false;
            }
            catch (WebDriverTimeoutException)
            {
                success = false;
            }

            if (success.HasValue && success.Value && successCallback != null)
                successCallback(driver);
            else if (success.HasValue && !success.Value && failureCallback != null)
                failureCallback(driver);

            return returnElement;
        }

        public static bool WaitUntilVisible(this IWebDriver driver, By by)
        {
            return WaitUntilVisible(driver, by, Constants.DefaultTimeout, null, null);
        }

        public static bool WaitUntilVisible(this IWebDriver driver, By by, TimeSpan timeout)
        {
            return WaitUntilVisible(driver, by, timeout, null, null);
        }

        public static bool WaitUntilVisible(this IWebDriver driver, By by, TimeSpan timeout, Action<IWebDriver> successCallback)
        {
            return WaitUntilVisible(driver, by, timeout, successCallback, null);
        }

        public static bool WaitUntilVisible(this IWebDriver driver, By by, TimeSpan timeout, Action<IWebDriver> successCallback, Action<IWebDriver> failureCallback)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            bool? success;

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(by));

                success = true;
            }
            catch (NoSuchElementException)
            {
                success = false;
            }
            catch (WebDriverTimeoutException)
            {
                success = false;
            }

            if (success.HasValue && success.Value && successCallback != null)
                successCallback(driver);
            else if (success.HasValue && !success.Value && failureCallback != null)
                failureCallback(driver);

            return success.Value;
        }

        public static bool WaitUntilClickable(this IWebDriver driver, By by)
        {
            return WaitUntilClickable(driver, by, Constants.DefaultTimeout, null, null);
        }

        public static bool WaitUntilClickable(this IWebDriver driver, By by, TimeSpan timeout)
        {
            return WaitUntilClickable(driver, by, timeout, null, null);
        }

        public static bool WaitUntilClickable(this IWebDriver driver, By by, TimeSpan timeout, Action<IWebDriver> successCallback)
        {
            return WaitUntilClickable(driver, by, timeout, successCallback, null);
        }

        public static bool WaitUntilClickable(this IWebDriver driver, By by, TimeSpan timeout, Action<IWebDriver> successCallback, Action<IWebDriver> failureCallback)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            bool? success;

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(by));

                success = true;
            }
            catch (NoSuchElementException)
            {
                success = false;
            }
            catch (WebDriverTimeoutException)
            {
                success = false;
            }

            if (success.HasValue && success.Value && successCallback != null)
                successCallback(driver);
            else if (success.HasValue && !success.Value && failureCallback != null)
                failureCallback(driver);

            return success.Value;
        }
        #endregion Waits

        #region Args / Tracing

        public static string ToTraceString(this FindElementEventArgs e)
        {
            try
            {
                if (e.Element != null)
                {
                    return string.Format("{4} - [{0},{1}] - <{2}>{3}</{2}>", e.Element.Location.X, e.Element.Location.Y, e.Element.TagName, e.Element.Text, e.FindMethod);
                }
                else
                {
                    return e.FindMethod.ToString();
                }
            }
            catch(Exception)
            {
                return e.FindMethod.ToString();
            }
        }

        #endregion Args / Tracing
    }
}