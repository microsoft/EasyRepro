// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class BrowserCommandOptions : ICloneable
    {
        public static BrowserCommandOptions Default => new BrowserCommandOptions();
        public static BrowserCommandOptions NoRetry => new BrowserCommandOptions(Constants.DefaultTraceSource, "", 1, 0, null, true, null);
        public static BrowserCommandOptions InfiniteRetry => new BrowserCommandOptions(Constants.DefaultTraceSource, "", Int32.MaxValue, Constants.DefaultRetryDelay, null, false, null);

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// Gets an action to execute when an exception is thrown by the command.
        /// </summary>
        public Action<Exception> ExceptionAction { get; set; }

        /// <summary>
        /// Gets or sets a list of exception types to ignore and retry the command.
        /// </summary>
        public List<Type> ExceptionTypes { get; set; }

        /// <summary>
        /// Number of retry attempts to execute this command.
        /// </summary>
        public int RetryAttempts { get; set; }

        /// <summary>
        /// Delay (in miliseconds) to wait between retry attempts.
        /// </summary>
        public int RetryDelay { get; set; }

        /// <summary>
        /// Gets the trace source that will be used for tracing/diagnostics.
        /// </summary>
        public string TraceSource { get; set; }

        /// <summary>
        /// Boolean indicating if exceptions should be thrown during command execution.
        /// </summary>
        public bool ThrowExceptions { get; set; }

        /// <summary>
        /// Constructs a new instance of the BrowserCommand using defaults where parameters are omitted.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelay">The retry delay.</param>
        /// <param name="exceptionAction">The exception action.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <param name="exceptions">The exceptions.</param>
        public BrowserCommandOptions(
            string traceSource = Constants.DefaultTraceSource,
            string commandName = "",
            int retryAttempts = Constants.DefaultRetryAttempts,
            int retryDelay = Constants.DefaultRetryDelay,
            Action<Exception> exceptionAction = null,
            bool throwExceptions = true,
            params Type[] exceptions)
        {
            this.TraceSource = traceSource;
            this.CommandName = commandName;
            this.RetryAttempts = retryAttempts;
            this.RetryDelay = retryDelay;
            this.ExceptionAction = exceptionAction;
            this.ThrowExceptions = throwExceptions;

            if (exceptions != null && exceptions.Length > 0)
            {
                this.ExceptionTypes = exceptions.ToList();
            }
            else
            {
                this.ExceptionTypes = new List<Type>();
            }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A copy of this instance.</returns>
        public object Clone()
        {
            return new BrowserCommandOptions(
                this.TraceSource,
                this.CommandName,
                this.RetryAttempts,
                this.RetryDelay,
                this.ExceptionAction,
                this.ThrowExceptions,
                this.ExceptionTypes.ToArray());
        }

        /// <summary>
        /// Clones the current BrowserCommandOptions with a given command name.
        /// </summary>
        /// <param name="commandName">Name of the command to use with the cloned BrowserCommandOptions.</param>
        /// <returns>
        /// A clonse of the current instance of BrowserCommandOptions with 
        /// the new commandName.
        /// </returns>
        public BrowserCommandOptions CloneAs(string commandName)
        {
            return new BrowserCommandOptions(
                this.TraceSource,
                commandName,
                this.RetryAttempts,
                this.RetryDelay,
                this.ExceptionAction,
                this.ThrowExceptions,
                this.ExceptionTypes.ToArray());
        }
    }
}