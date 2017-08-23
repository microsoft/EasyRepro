// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class DelegateBrowserCommand<TReturn>
        : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            return Delegate(driver);
        }
    }

    public class DelegateBrowserCommand<T1, TReturn>
        : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, T1, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, T1, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, T1, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            var p1 = @params != null && @params.Length >= 1 ? (T1)@params[0] : default(T1);

            return Delegate(driver, p1);
        }
    }

    public class DelegateBrowserCommand<T1, T2, TReturn>
        : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, T1, T2, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, T1, T2, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, T1, T2, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            var p1 = @params != null && @params.Length >= 1 ? (T1)@params[0] : default(T1);
            var p2 = @params != null && @params.Length >= 2 ? (T2)@params[1] : default(T2);

            return Delegate(driver, p1, p2);
        }
    }

    public class DelegateBrowserCommand<T1, T2, T3, TReturn>
       : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, T1, T2, T3, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, T1, T2, T3, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            var p1 = @params != null && @params.Length >= 1 ? (T1)@params[0] : default(T1);
            var p2 = @params != null && @params.Length >= 2 ? (T2)@params[1] : default(T2);
            var p3 = @params != null && @params.Length >= 3 ? (T3)@params[2] : default(T3);

            return Delegate(driver, p1, p2, p3);
        }
    }

    public class DelegateBrowserCommand<T1, T2, T3, T4, TReturn>
       : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, T1, T2, T3, T4, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, T1, T2, T3, T4, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            var p1 = @params != null && @params.Length >= 1 ? (T1)@params[0] : default(T1);
            var p2 = @params != null && @params.Length >= 2 ? (T2)@params[1] : default(T2);
            var p3 = @params != null && @params.Length >= 3 ? (T3)@params[2] : default(T3);
            var p4 = @params != null && @params.Length >= 4 ? (T4)@params[3] : default(T4);

            return Delegate(driver, p1, p2, p3, p4);
        }
    }

    public class DelegateBrowserCommand<T1, T2, T3, T4, T5, TReturn>
       : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, T1, T2, T3, T4, T5, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, T1, T2, T3, T4, T5, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            var p1 = @params != null && @params.Length >= 1 ? (T1)@params[0] : default(T1);
            var p2 = @params != null && @params.Length >= 2 ? (T2)@params[1] : default(T2);
            var p3 = @params != null && @params.Length >= 3 ? (T3)@params[2] : default(T3);
            var p4 = @params != null && @params.Length >= 4 ? (T4)@params[3] : default(T4);
            var p5 = @params != null && @params.Length >= 5 ? (T5)@params[4] : default(T5);

            return Delegate(driver, p1, p2, p3, p4, p5);
        }
    }

    public class DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, TReturn>
      : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, T1, T2, T3, T4, T5, T6, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, T6, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, T1, T2, T3, T4, T5, T6, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            var p1 = @params != null && @params.Length >= 1 ? (T1)@params[0] : default(T1);
            var p2 = @params != null && @params.Length >= 2 ? (T2)@params[1] : default(T2);
            var p3 = @params != null && @params.Length >= 3 ? (T3)@params[2] : default(T3);
            var p4 = @params != null && @params.Length >= 4 ? (T4)@params[3] : default(T4);
            var p5 = @params != null && @params.Length >= 5 ? (T5)@params[4] : default(T5);
            var p6 = @params != null && @params.Length >= 6 ? (T6)@params[5] : default(T6);

            return Delegate(driver, p1, p2, p3, p4, p5, p6);
        }
    }

    public class DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, T7, TReturn>
       : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            var p1 = @params != null && @params.Length >= 1 ? (T1)@params[0] : default(T1);
            var p2 = @params != null && @params.Length >= 2 ? (T2)@params[1] : default(T2);
            var p3 = @params != null && @params.Length >= 3 ? (T3)@params[2] : default(T3);
            var p4 = @params != null && @params.Length >= 4 ? (T4)@params[3] : default(T4);
            var p5 = @params != null && @params.Length >= 5 ? (T5)@params[4] : default(T5);
            var p6 = @params != null && @params.Length >= 6 ? (T6)@params[5] : default(T6);
            var p7 = @params != null && @params.Length >= 7 ? (T7)@params[6] : default(T7);

            return Delegate(driver, p1, p2, p3, p4, p5, p6, p7);
        }
    }

    public class DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>
        : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            var p1 = @params != null && @params.Length >= 1 ? (T1)@params[0] : default(T1);
            var p2 = @params != null && @params.Length >= 2 ? (T2)@params[1] : default(T2);
            var p3 = @params != null && @params.Length >= 3 ? (T3)@params[2] : default(T3);
            var p4 = @params != null && @params.Length >= 4 ? (T4)@params[3] : default(T4);
            var p5 = @params != null && @params.Length >= 5 ? (T5)@params[4] : default(T5);
            var p6 = @params != null && @params.Length >= 6 ? (T6)@params[5] : default(T6);
            var p7 = @params != null && @params.Length >= 7 ? (T7)@params[6] : default(T7);
            var p8 = @params != null && @params.Length >= 8 ? (T8)@params[7] : default(T8);

            return Delegate(driver, p1, p2, p3, p4, p5, p6, p7, p8);
        }
    }

    public class DelegateBrowserCommand<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>
        : BrowserCommand<TReturn>
    {
        public DelegateBrowserCommand(Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> @delegate)
            : base()
        {
            this.Delegate = @delegate;
        }

        public DelegateBrowserCommand(BrowserCommandOptions options, Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> @delegate)
            : base(options)
        {
            this.Delegate = @delegate;
        }

        public Func<IWebDriver, T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> Delegate { get; protected set; }

        protected override TReturn ExecuteCommand(IWebDriver driver, params object[] @params)
        {
            var p1 = @params != null && @params.Length >= 1 ? (T1)@params[0] : default(T1);
            var p2 = @params != null && @params.Length >= 2 ? (T2)@params[1] : default(T2);
            var p3 = @params != null && @params.Length >= 3 ? (T3)@params[2] : default(T3);
            var p4 = @params != null && @params.Length >= 4 ? (T4)@params[3] : default(T4);
            var p5 = @params != null && @params.Length >= 5 ? (T5)@params[4] : default(T5);
            var p6 = @params != null && @params.Length >= 6 ? (T6)@params[5] : default(T6);
            var p7 = @params != null && @params.Length >= 7 ? (T7)@params[6] : default(T7);
            var p8 = @params != null && @params.Length >= 8 ? (T8)@params[7] : default(T8);
            var p9 = @params != null && @params.Length >= 9 ? (T9)@params[8] : default(T9);

            return Delegate(driver, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }
    }
}