// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using System;
using System.Diagnostics;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class BrowserPage
    {
        public BrowserPage(InteractiveBrowser browser)
        {
            Browser = browser;
            Browser.Depth = 1;
        }

        public BrowserPage()
        {
        }

        public InteractiveBrowser Browser;

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult>(BrowserCommand<TResult> cmd)
        {
            Browser.Depth++;
            var command = cmd.Execute(Browser.Driver);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult>(DelegateBrowserCommand<TResult> @delegate)
        {
            Browser.Depth++;
            var command = @delegate.Execute(Browser.Driver);

            Browser.CalculateResults(command);
            Browser.Depth--;
            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult>(string commandName, Func<IWebDriver, TResult> @delegate)
        {
            Browser.Depth++;
            var command =  new DelegateBrowserCommand<TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver);
            Browser.Depth--;
            Browser.CalculateResults(command);


            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1>(string commandName, Func<IWebDriver, T1, TResult> @delegate, T1 p1)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver, p1);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2>(string commandName, Func<IWebDriver, T1, T2, TResult> @delegate, T1 p1, T2 p2)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver, p1, p2);


            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3>(string commandName, Func<IWebDriver, T1, T2, T3, TResult> @delegate, T1 p1, T2 p2, T3 p3)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver, p1, p2, p3);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4>(string commandName, Func<IWebDriver, T1, T2, T3, T4, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            Browser.Depth++;
            var command =  new DelegateBrowserCommand<T1, T2, T3, T4, TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver, p1, p2, p3, p4);
            Browser.Depth--;
            Browser.CalculateResults(command);


            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5>(string commandName, Func<IWebDriver, T1, T2, T3, T4, T5, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver, p1, p2, p3, p4, p5);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5, T6>(string commandName, Func<IWebDriver, T1, T2, T3, T4, T5, T6, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver, p1, p2, p3, p4, p5, p6);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5, T6, T7>(string commandName, Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, T7, TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver, p1, p2, p3, p4, p5, p6, p7);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5, T6, T7, T8>(string commandName, Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver, p1, p2, p3, p4, p5, p6, p7, p8);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string commandName, Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(new BrowserCommandOptions(Constants.DefaultTraceSource, commandName), @delegate)
                .Execute(Browser.Driver, p1, p2, p3, p4, p5, p6, p7, p8, p9);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult>(BrowserCommandOptions options, Func<IWebDriver, TResult> @delegate)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<TResult>(options, @delegate).Execute(Browser.Driver);
            Browser.Depth--;
            Browser.CalculateResults(command);


            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1>(BrowserCommandOptions options, Func<IWebDriver, T1, TResult> @delegate, T1 p1)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, TResult>(options, @delegate).Execute(Browser.Driver, p1);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2>(BrowserCommandOptions options, Func<IWebDriver, T1, T2, TResult> @delegate, T1 p1, T2 p2)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, TResult>(options, @delegate).Execute(Browser.Driver, p1, p2);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3>(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, TResult> @delegate, T1 p1, T2 p2, T3 p3)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, TResult>(options, @delegate).Execute(Browser.Driver, p1, p2, p3);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4>(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, TResult>(options, @delegate).Execute(Browser.Driver, p1, p2, p3, p4);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5>(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, TResult>(options, @delegate).Execute(Browser.Driver, p1, p2, p3, p4, p5);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5, T6>(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, T6, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, TResult>(options, @delegate).Execute(Browser.Driver, p1, p2, p3, p4, p5, p6);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5, T6, T7>(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, T7, TResult>(options, @delegate).Execute(Browser.Driver, p1, p2, p3, p4, p5, p6, p7);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5, T6, T7, T8>(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(options, @delegate).Execute(Browser.Driver, p1, p2, p3, p4, p5, p6, p7, p8);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TResult> Execute<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9>(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @delegate, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            Browser.Depth++;
            var command = new DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(options, @delegate).Execute(Browser.Driver, p1, p2, p3, p4, p5, p6, p7, p8, p9);

            Browser.CalculateResults(command);
            Browser.Depth--;

            return command;
        }
    }
}