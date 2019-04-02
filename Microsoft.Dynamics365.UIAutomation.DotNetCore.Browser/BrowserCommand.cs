// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public abstract class BrowserCommand<TReturn>
    {
        public BrowserCommandOptions Options { get; private set; }

        protected BrowserCommand()
        {
            this.Options = BrowserCommandOptions.Default;

            Trace = new TraceSource(this.Options.TraceSource);
        }

        protected BrowserCommand(BrowserCommandOptions options)
        {
            this.Options = options;            

            Trace = new TraceSource(this.Options.TraceSource);
        }

        protected TraceSource Trace { get; }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute(IWebDriver driver)
        {
            return Execute(driver, default(object), default(object), default(object), default(object), default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1>(IWebDriver driver, T1 p1)
        {
            return Execute(driver, p1, default(object), default(object), default(object), default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1, T2>(IWebDriver driver, T1 p1, T2 p2)
        {
            return Execute(driver, p1, p2, default(object), default(object), default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3>(IWebDriver driver, T1 p1, T2 p2, T3 p3)
        {
            return Execute(driver, p1, p2, p3, default(object), default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return Execute(driver, p1, p2, p3, p4, default(object), default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return Execute(driver, p1, p2, p3, p4, p5, default(object), default(object), default(object), default(object));
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5, T6>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return Execute(driver, p1, p2, p3, p4, p5, p6, default(object), default(object), default(object));
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5, T6, T7>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return Execute(driver, p1, p2, p3, p4, p5, p6, p7, default(object), default(object));
        }

        [DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5, T6, T7, T8>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return Execute(driver, p1, p2, p3, p4, p5, p6, p7, p8, default(object));
        }

        //[DebuggerNonUserCode()]
        public BrowserCommandResult<TReturn> Execute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IWebDriver driver, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            int retries = this.Options.RetryAttempts;            
            var result = new BrowserCommandResult<TReturn>();
            result.CommandName = this.Options.CommandName;

            System.Diagnostics.Trace.CorrelationManager.StartLogicalOperation();

            result.Start();

            while (retries-- >= 0)
            {
                try
                {
                    result.ExecutionAttempts++;

                    Trace.TraceEvent(TraceEventType.Information,
                        Constants.Tracing.CommandStartEventId,
                        "Command Start: {0} - Attempt {1}/{2}",
                        this.Options.CommandName,
                        result.ExecutionAttempts,
                        this.Options.RetryAttempts);

                    result.Value = ExecuteCommand(driver, p1, p2, p3, p4, p5, p6, p7, p8, p9);
                    result.Success = true;
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Exception = e;

                    if (this.Options.ExceptionTypes != null && this.Options.ExceptionTypes.Count > 0)
                    {
                        var isAssignable = this.Options.ExceptionTypes
                            .Any(t => t.IsAssignableFrom(e.GetType()));

                        if (!isAssignable || retries == 0)
                        {
                            Trace.TraceEvent(TraceEventType.Error,
                                Constants.Tracing.CommandErrorEventId,
                                "Command Error: {0} - Attempt {1}/{2} - {3} - {4}",
                                this.Options.CommandName,
                                result.ExecutionAttempts,
                                this.Options.RetryAttempts,
                                e.GetType().FullName, e.Message);

                            if (this.Options.ThrowExceptions)
                            {
                                throw;
                            }
                        }
                        else
                        {
                            if (this.Options.ExceptionAction != null)
                            {
                                Trace.TraceEvent(TraceEventType.Information,
                                    Constants.Tracing.CommandRetryEventId,
                                    "Command Error: {0} - Retry Action initiated",
                                    this.Options.CommandName);

                                this.Options.ExceptionAction(e);
                            }

                            if (this.Options.RetryDelay > 0)
                                Thread.Sleep(this.Options.RetryDelay);
                        }
                    }
                    else
                    {
                        Trace.TraceEvent(TraceEventType.Error,
                            Constants.Tracing.CommandErrorEventId,
                            "Command Error: {0} - Attempt {1}/{2} - {3} - {4}",
                            this.Options.CommandName,
                            result.ExecutionAttempts,
                            this.Options.RetryAttempts,
                            e.GetType().FullName, e.Message);

                        if (this.Options.ThrowExceptions)
                        {
                            throw;
                        }
                    }
                }
                finally
                {
                    if (result.Success.HasValue && result.Success.Value)
                    {
                        retries = -1;
                    }
                }
            }

            result.Stop();

            Trace.TraceEvent(TraceEventType.Information,
                Constants.Tracing.CommandStopEventId,
                "Command Stop: {0} - {1} attempts - total execution time {2}ms",
                this.Options.CommandName,
                result.ExecutionAttempts,
                result.ExecutionTime);

            System.Diagnostics.Trace.CorrelationManager.StopLogicalOperation();
            
            return result;
        }

        protected abstract TReturn ExecuteCommand(IWebDriver driver, params object[] @params);
    }
}