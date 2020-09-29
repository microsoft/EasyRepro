// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public static class SeleniumExtensions
    {
        #region Click

        public static void Click(this IWebElement element, bool ignoreStaleElementException)
        {
            try
            {
                element.Click();
            }
            catch (StaleElementReferenceException)
            {
                if (!ignoreStaleElementException)
                    throw;
            }
        }

        public static IWebElement ClickIfVisible(this ISearchContext driver, By by, TimeSpan? timeout = null)
            => WaitUntilClickable(driver, by, timeout ?? TimeSpan.FromSeconds(1), e => e.Click());

        public static IWebElement ClickWhenAvailable(this ISearchContext driver, By by, TimeSpan? timeout = null, string errorMessage = null)
            => WaitUntilClickable(driver, by, timeout, e => e.Click(), errorMessage ?? "Unable to click element.");

        public static IWebElement ClickWhenAvailable(this ISearchContext driver, By by, string errorMessage)
            => WaitUntilClickable(driver, by, null, e => e.Click(), errorMessage ?? "Unable to click element.");

        public static IWebElement ClickAndWait(this IWebDriver driver, By by, TimeSpan timeout)
        {
            var element = driver.FindElement(by);
            if (element == null)
                return null;

            element.Click();
            System.Threading.Thread.Sleep((int) timeout.TotalMilliseconds);

            return element;
        }

        public static void Hover(this IWebElement element, IWebDriver driver, bool ignoreStaleElementException = true)
        {
            try
            {
                Actions action = new Actions(driver);
                action.MoveToElement(element).Perform();
            }
            catch (StaleElementReferenceException)
            {
                if (!ignoreStaleElementException)
                    throw;
            }
        }

        #endregion Click

        public static void Click(this IWebDriver driver, IWebElement element, Func<Point> offsetFunc = null, bool ignoreStaleElementException = true)
            => driver.Perform(a => a.Click(), element, offsetFunc, ignoreStaleElementException);

        #region Double Click

        public static void DoubleClick(this IWebDriver driver, IWebElement element, Func<Point> offsetFunc = null, bool ignoreStaleElementException = true)
            => driver.Perform(a => a.DoubleClick(), element, offsetFunc, ignoreStaleElementException);

        public static void Perform(this IWebDriver driver, Func<Actions, Actions> action, IWebElement element, Func<Point> offsetFunc = null, bool ignoreStaleElementException = true)
        {
            try
            {
                var actions = new Actions(driver);
                if (offsetFunc == null)
                    actions = actions.MoveToElement(element);
                else
                {
                    var offset = offsetFunc();
                    actions = actions.MoveToElement(element, offset.X, offset.Y);
                }
                action(actions).Perform();
            }
            catch (StaleElementReferenceException)
            {
                if (!ignoreStaleElementException)
                    throw;
            }
        }

        
        #endregion

        #region Script Execution

        [DebuggerNonUserCode]
        public static object ExecuteScript(this IWebDriver driver, string script, params object[] args)
        {
            var scriptExecutor = driver as IJavaScriptExecutor;

            if (scriptExecutor == null)
                throw new InvalidOperationException(
                    $"The driver type '{driver.GetType().FullName}' does not support Javascript execution.");

            return scriptExecutor.ExecuteScript(script, args);
        }

        [DebuggerNonUserCode]
        public static JObject GetJsonObject(this IWebDriver driver, string @object)
        {
            @object = SanitizeReturnStatement(@object);

            var results = ExecuteScript(driver, $"return JSON.stringify({@object});").ToString();

            return JObject.Parse(results);
        }

        [DebuggerNonUserCode]
        public static JArray GetJsonArray(this IWebDriver driver, string @object)
        {
            @object = SanitizeReturnStatement(@object);

            var results = ExecuteScript(driver, $"return JSON.stringify({@object});").ToString();

            return JArray.Parse(results);
        }

        [DebuggerNonUserCode]
        public static T GetJsonObject<T>(this IWebDriver driver, string @object)
        {
            @object = SanitizeReturnStatement(@object);

            var results = ExecuteScript(driver, $"return JSON.stringify({@object});").ToString();
            var jsSerializer = new JavaScriptSerializer();

            jsSerializer.RegisterConverters(new[] {new DynamicJsonConverter()});

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

        [DebuggerNonUserCode]
        public static void ResetZoom(this IWebDriver driver)
        {
            IWebElement element = driver.FindElement(By.TagName("body"));
            element.SendKeys(Keys.Control + "0");
        }

        #endregion Browser Options

        #region Screenshot

        [DebuggerNonUserCode]
        public static Screenshot TakeScreenshot(this IWebDriver driver)
        {
            var screenshotDriver = driver as ITakesScreenshot;

            if (screenshotDriver == null)
                throw new InvalidOperationException(
                    $"The driver type '{driver.GetType().FullName}' does not support taking screenshots.");

            return screenshotDriver.GetScreenshot();
        }

        [DebuggerNonUserCode]
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

        public static bool HasAttribute(this IWebElement element, string attributeName)
            => element.GetAttribute(attributeName) != null;

        public static T GetAttribute<T>(this IWebElement element, string attributeName)
        {
            string value = element.GetAttribute(attributeName) ?? string.Empty;
            return (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
        }

        public static string GetAuthority(this IWebDriver driver)
        {
            string url = driver.Url; // get the current URL (full)
            Uri currentUri = new Uri(url); // create a Uri instance of it
            string baseUrl = currentUri.Authority; // just get the "base" bit of the URL

            return baseUrl;
        }

        public static string GetBodyText(this IWebDriver driver)
        {
            return driver.FindElement(By.TagName("body")).Text;
        }

        public static void SendKeys(this IWebElement element, string value, bool clear = true)
        {
            if (clear)
                element.Clear();

            element.SendKeys(value);
        }

        public static bool AlertIsPresent(this IWebDriver driver)
        {
            return AlertIsPresent(driver, new TimeSpan(0, 0, 2));
        }

        public static bool AlertIsPresent(this IWebDriver driver, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            try
            {
                wait.Until(ExpectedConditions.AlertIsPresent());
                return true;
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException) { }
            return false;
        }

        public static IWebDriver LastWindow(this IWebDriver driver)
            => driver.SwitchTo().Window(driver.WindowHandles.Last());


        /// <summary>
        /// Clears the focus from all elements.
        /// TODO: this implementation of the ClearFocus is clicking somewhere on the body, that may happen in some unwanted point, changing the results of the test.
        /// Clicking on the label of the current control may have a better result.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public static void ClearFocus(this IWebDriver driver)
        {
            driver.FindElement(By.TagName("body")).Click();
        }

        #endregion Elements

        #region Waits

        public static bool WaitForPageToLoad(this IWebDriver driver, TimeSpan? timeout = null)
        {
            string state = string.Empty;
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, timeout ?? Constants.DefaultTimeout);

                //Checks every 500 ms whether predicate returns true if returns exit otherwise keep trying till it returns ture
                wait.Until(d =>
                {
                    try
                    {
                        state = ((IJavaScriptExecutor) driver).ExecuteScript(@"return document.readyState").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        //Ignore
                    }
                    catch (NoSuchWindowException)
                    {
                        //when popup is closed, switch to last windows
                        driver.LastWindow();
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

                state = ((IJavaScriptExecutor) driver).ExecuteScript(@"return document.readyState").ToString();
                if (!(state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase)))
                    throw;
            }

            return true;
        }

        public static bool WaitForTransaction(this IWebDriver driver, TimeSpan? timeout = null)
        {
            bool state = false;
            //Poll every half second to see if UCI is idle
            var wait = new WebDriverWait(driver, timeout ?? Constants.DefaultTimeout);
            wait.IgnoreExceptionTypes(typeof(TimeoutException), typeof(NullReferenceException));
            try
            {
                state = wait.Until(d => (bool) driver.ExecuteScript("return window.UCWorkBlockTracker.isAppIdle()")); // Check to see if UCI is idle
            }
            catch (Exception)
            {
                // ignored
            }
            return state;
        }

        public static object WaitForScript(this IWebDriver driver, string script, TimeSpan? timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout ?? Constants.DefaultTimeout);

            wait.Until(d =>
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

        public static bool HasElement(this ISearchContext driver, By by)
            => driver.FindElements(by).Count > 0;

        public static IWebElement FindAvailable(this ISearchContext driver, By locator)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(locator);
            int? count = elements?.Count;
            if (count == null || count == 0)
                return null;

            var result = count > 1
                ? elements.FirstOrDefault(x => x?.Displayed == true)
                : elements.First(x => x != null);

            return result;
        }

        public static bool IsClickable(this IWebElement element) => element.IsVisible() && element.IsEnable();
        public static bool IsEnable(this IWebElement element) => element?.Enabled == true;
        public static bool IsVisible(this IWebElement element) => element?.Displayed == true;

        public static bool IsVisible(this ISearchContext driver, By locator)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(locator);
            bool result = elements.Any(IsVisible);
            return result;
        }

        public static void SetVisible(this IWebDriver driver, By by, bool visible)
        {
            IWebElement element = driver.FindElement(by);
            var visibility = visible ? "inline" : "none";
            driver.ExecuteScript($"document.getElementById('{element.GetAttribute("Id")}').setAttribute('style', 'display: {visibility};')");
        }

        public static IWebElement FindVisible(this ISearchContext driver, By locator)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(locator);
            IWebElement result = elements.FirstOrDefault(IsVisible);
            return result;
        }

        public static IWebElement FindClickable(this ISearchContext driver, By locator)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(locator);
            IWebElement result = elements.FirstOrDefault(IsClickable);
            return result;
        }

        public static bool TryFindElement(this ISearchContext context, By by, out IWebElement element)
        {
            ReadOnlyCollection<IWebElement> elements = null;

            try
            {
               elements = context.FindElements(by);
            }
            catch (NullReferenceException)
            {
                // Do nothing
            }

            var success = elements?.Count > 0;
            element = success ? elements[0] : null;
            return success;
        }


        public static IWebElement WaitUntilAvailable(this ISearchContext driver, By by, string exceptionMessage)
            => WaitUntilAvailable(driver, by, null, null, exceptionMessage);

        public static IWebElement WaitUntilAvailable(this ISearchContext driver, By by, Action<IWebElement> successCallback, string exceptionMessage)
            => WaitUntilAvailable(driver, by, null, successCallback, exceptionMessage);

        public static IWebElement WaitUntilAvailable(this ISearchContext driver, By by, TimeSpan timeout, string exceptionMessage)
            => WaitUntilAvailable(driver, by, timeout, null, exceptionMessage);

        public static IWebElement WaitUntilAvailable(this ISearchContext driver, By by,
            TimeSpan? timeout,
            Action<IWebElement> successCallback,
            string exceptionMessage)
        {
            if (string.IsNullOrWhiteSpace(exceptionMessage))
                exceptionMessage = $"Unable to find any element by: {by}";
            return WaitUntilAvailable(driver, by, timeout, successCallback, () => throw new InvalidOperationException(exceptionMessage));
        }

        public static IWebElement WaitUntilAvailable(this ISearchContext driver, By by,
            TimeSpan? timeout = null,
            Action<IWebElement> successCallback = null,
            Action failureCallback = null)
        {
            return WaitUntil(driver, d => d.FindAvailable(by), timeout,
                successCallback,
                failureCallback
            );
        }

        public static IWebElement WaitUntilVisible(this ISearchContext driver, By by, string exceptionMessage)
            => WaitUntilVisible(driver, by, null, null, exceptionMessage);

        public static IWebElement WaitUntilVisible(this ISearchContext driver, By by, Action<IWebElement> successCallback, string exceptionMessage)
            => WaitUntilVisible(driver, by, null, successCallback, exceptionMessage);

        public static IWebElement WaitUntilVisible(this ISearchContext driver, By by, TimeSpan timeout, string exceptionMessage)
            => WaitUntilVisible(driver, by, timeout, null, exceptionMessage);

        public static IWebElement WaitUntilVisible(this ISearchContext driver, By by,
            TimeSpan? timeout,
            Action<IWebElement> successCallback,
            string exceptionMessage)
        {
            if (string.IsNullOrWhiteSpace(exceptionMessage))
                exceptionMessage = $"Unable to find any visible element by: {by}";
            return WaitUntilVisible(driver, by, timeout, successCallback, () => throw new InvalidOperationException(exceptionMessage));
        }

        public static IWebElement WaitUntilVisible(this ISearchContext driver, By by,
            TimeSpan? timeout = null,
            Action<IWebElement> successCallback = null,
            Action failureCallback = null)
        {
            return WaitUntil(driver, d => d.FindVisible(by), timeout, successCallback, failureCallback);
        }


        public static IWebElement WaitUntilClickable(this ISearchContext driver, By by, string exceptionMessage)
            => WaitUntilClickable(driver, by, null, null, exceptionMessage);

        public static IWebElement WaitUntilClickable(this ISearchContext driver, By by, Action<IWebElement> successCallback, string exceptionMessage)
            => WaitUntilClickable(driver, by, null, successCallback, exceptionMessage);

        public static IWebElement WaitUntilClickable(this ISearchContext driver, By by, TimeSpan timeout, string exceptionMessage)
            => WaitUntilClickable(driver, by, timeout, null, exceptionMessage);

        public static IWebElement WaitUntilClickable(this ISearchContext driver, By by, TimeSpan? timeout, Action<IWebElement> successCallback, string exceptionMessage)
        {
            if (string.IsNullOrWhiteSpace(exceptionMessage))
                exceptionMessage = $"Unable to find any clickable element by: {by}";
            return WaitUntilClickable(driver, by, timeout, successCallback, () => throw new InvalidOperationException(exceptionMessage));
        }

        public static IWebElement WaitUntilClickable(this ISearchContext driver, By by,
            TimeSpan? timeout = null,
            Action<IWebElement> successCallback = null,
            Action failureCallback = null)
        {
            return WaitUntil(driver, d => d.FindClickable(by), timeout,
                successCallback,
                failureCallback
            );
        }

        public static bool WaitUntil(this ISearchContext driver, Predicate<ISearchContext> predicate,
            TimeSpan? timeout = null,
            Action successCallback = null, Action failureCallback = null)
        {
            var wait = new DefaultWait<ISearchContext>(driver) {Timeout = timeout ?? Constants.DefaultTimeout};
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            bool success = false;
            try
            {
                success = wait.Until(d => predicate(d));
            }
            catch (WebDriverTimeoutException) { }

            if (success)
                successCallback?.Invoke();
            else
                failureCallback?.Invoke();

            return success;
        }

        public static IWebElement WaitUntil(this ISearchContext driver, Func<ISearchContext, IWebElement> searchFunc,
            TimeSpan? timeout = null,
            Action<IWebElement> successCallback = null, Action failureCallback = null)
        {
            var wait = new DefaultWait<ISearchContext>(driver)
            {
                Timeout = timeout ?? Constants.DefaultTimeout
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            bool success = false;
            IWebElement element = null;
            try
            {
                element = wait.Until(searchFunc);
                success = element != null;
            }
            catch (WebDriverTimeoutException)
            {
            }

            if (success)
                successCallback?.Invoke(element);
            else
                failureCallback?.Invoke();

            return element;
        }

        public static ICollection<IWebElement> WaitUntil(this ISearchContext driver, Func<ISearchContext, ICollection<IWebElement>> searchFunc,
            TimeSpan? timeout = null,
            Action<ICollection<IWebElement>> successCallback = null, Action failureCallback = null)
        {
            ICollection<IWebElement> elements = null;
            Predicate<ISearchContext> condition = d =>
            {
                elements = searchFunc(d);
                return elements != null && elements.Count > 0;
            };

            bool success = driver.WaitUntil(condition);
            if (success)
                successCallback?.Invoke(elements);
            else
                failureCallback?.Invoke();

            return elements;
        }

        public static bool RepeatUntil(this IWebDriver driver, Action action, Predicate<IWebDriver> predicate,
            TimeSpan? timeout = null,
            int attemps = Constants.DefaultRetryAttempts,
            Action successCallback = null, Action failureCallback = null)
        {
            timeout = timeout ?? Constants.DefaultTimeout;
            var waittime = new TimeSpan(timeout.Value.Ticks / attemps);

            WebDriverWait wait = new WebDriverWait(driver, waittime);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            bool success = predicate(driver);
            while (!success && attemps > 0)
            {
                try
                {
                    action();
                    attemps--;
                    success = wait.Until(d => predicate(d));
                }
                catch (WebDriverTimeoutException)
                {
                }
            }

            if (success)
                successCallback?.Invoke();
            else
                failureCallback?.Invoke();

            return success;
        }

        #endregion Waits

        #region Args / Tracing

        public static string ToTraceString(this FindElementEventArgs e)
        {
            var method = e.FindMethod.ToString();
            try
            {
                if (e.Element != null)
                {
                    var element = $"[{e.Element.Location.X},{e.Element.Location.Y}] - <{e.Element.TagName}>{e.Element.Text}</{e.Element.TagName}>";
                    return method + " - " + element;
                }
            }
            catch (Exception)
            {
                /* ignore */
            }

            return method;
        }

        #endregion Args / Tracing
    }
}