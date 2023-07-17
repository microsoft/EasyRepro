// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public abstract class BrowserCommand<TReturn>
    {
        public BrowserCommandOptions Options { get; private set; }

        protected BrowserCommand()
        {
            Options = BrowserCommandOptions.Default;

            Trace = new TraceSource(Options.TraceSource);
        }

        protected BrowserCommand(BrowserCommandOptions options)
        {
            Options = options;

            Trace = new TraceSource(Options.TraceSource);
        }

        protected TraceSource Trace { get; }

        [DebuggerNonUserCode]
        public BrowserCommandResult<TReturn> Execute(IWebDriver driver)
        {
            return Execute(driver, default(object), default(object), default(object), default(object), default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode]
        public BrowserCommandResult<TReturn> Execute<T1>(IWebDriver driver, T1 p1)
        {
            return Execute(driver, p1, default(object), default(object), default(object), default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode]
        public BrowserCommandResult<TReturn> Execute<T1, T2>(IWebDriver driver, T1 p1, T2 p2)
        {
            return Execute(driver, p1, p2, default(object), default(object), default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3>(IWebDriver driver, T1 p1, T2 p2, T3 p3)
        {
            return Execute(driver, p1, p2, p3, default(object), default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return Execute(driver, p1, p2, p3, p4, default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return Execute(driver, p1, p2, p3, p4, p5, default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5, T6>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return Execute(driver, p1, p2, p3, p4, p5, p6, default(object), default(object), default(object));
        }

        [DebuggerNonUserCode]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5, T6, T7>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return Execute(driver, p1, p2, p3, p4, p5, p6, p7, default(object), default(object));
        }

        [DebuggerNonUserCode]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5, T6, T7, T8>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return Execute(driver, p1, p2, p3, p4, p5, p6, p7, p8, default(object));
        }

        //[DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            int retries = 1;
            int maxRetryAttempts = Options.RetryAttempts;
            int maxRetryAttemptsNotIgnoredExceptions = Options.RetryAttemptsNotIgnoredExceptions;

            var result = new BrowserCommandResult<TReturn> { CommandName = Options.CommandName };

            System.Diagnostics.Trace.CorrelationManager.StartLogicalOperation();

            result.Start();
            TraceExecutionStart(retries, maxRetryAttempts);
            while (true)
            {
                try
                {
                    result.Value = ExecuteCommand(driver, p1, p2, p3, p4, p5, p6, p7, p8, p9);
                    result.Success = true;
                }
                catch (Exception e)
                {
                    bool canRetry = false;
                    if (retries < maxRetryAttempts)
                        if (retries < maxRetryAttemptsNotIgnoredExceptions)
                            canRetry = true;
                        else
                            canRetry = Options.ExceptionTypes?.Any(t => t.IsInstanceOfType(e)) ?? false;

                    if (canRetry)
                    {
                        retries++;
                        TraceExecutionRetry(e, retries, maxRetryAttempts);
                        Options.ExceptionAction?.Invoke(e);

                        if (Options.RetryDelay > 0)
                            Thread.Sleep(Options.RetryDelay);

                        continue;
                    }
                    result.Exception = e;
                    result.Success = false;
                }
                break;
            }
            result.ExecutionAttempts = retries;
            result.Stop();
            TraceFinish(result);

            System.Diagnostics.Trace.CorrelationManager.StopLogicalOperation();

            if (result.Success == false && Options.ThrowExceptions)
                throw result.Exception;

            return result;
        }

        private void TraceExecutionRetry(Exception exception, int retries, int maxRetryAttempts)
        {
            Trace.TraceEvent(TraceEventType.Information,
                Constants.Tracing.CommandRetryEventId,
                "Command Retry: {0} - {1}: {2} - Retry {3}/{4} initiated",
                Options.CommandName, exception.GetType(), exception.Message, retries, maxRetryAttempts);
        }

        private void TraceExecutionStart(int retries, int maxRetryAttempts)
        {
            Trace.TraceEvent(TraceEventType.Information,
                Constants.Tracing.CommandStartEventId,
                "Command Start: {0} - Attempt {1}/{2}",
                Options.CommandName,
                retries,
                maxRetryAttempts);
        }

        private void TraceFinish(BrowserCommandResult<TReturn> result)
        {
            bool success = result.Success == true;
            var eventId = success ? Constants.Tracing.CommandStopEventId : Constants.Tracing.CommandErrorEventId;

            var attempts = result.ExecutionAttempts;
            var time = result.ExecutionTime;
            var strResult = success ? "Success" : "Failure";

            var value = success ?
                result.Value?.ToString() : 
                result.Exception?.ToString();

            string message = $"Command {strResult}: {Options.CommandName}" +
                             $" - {attempts} attempts - total execution time {time}ms" +
                             $" - Return Result: {value ?? "null"}";
            Trace.TraceEvent(TraceEventType.Information, eventId, message);
        }

        protected abstract TReturn ExecuteCommand(IWebDriver driver, params object[] @params);
    }
}