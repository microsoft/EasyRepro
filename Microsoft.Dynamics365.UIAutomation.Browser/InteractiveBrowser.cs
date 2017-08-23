// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public class InteractiveBrowser
        : IDisposable
    {
        #region Constructor(s)

        /// <summary>
        /// Constructs a new instance of an Interactive InteractiveBrowser.
        /// </summary>
        /// <param name="type">The type of browser instance to create.</param>
        public InteractiveBrowser(BrowserType type)
        {
            this.Options = new BrowserOptions
            {
                BrowserType = type
            };

            Trace = new TraceSource(this.Options.TraceSource);
        }

        public InteractiveBrowser(BrowserOptions options)
        {
            this.Options = options;

            Trace = new TraceSource(this.Options.TraceSource);
        }

        public InteractiveBrowser(IWebDriver driver)
        {
            this.driver = driver;

            lock (syncRoot)
            {
                this.disposeOfDriver = false;
            }

            Trace = new TraceSource(Constants.DefaultTraceSource);

            OnBrowserInitialized(new BrowserInitializeEventArgs(BrowserInitiationSource.Inline));
        }

        #endregion Constructor(s)

        #region Private Members

        private BackgroundRecorder recorder;
        private Thread recorderThread;

        private readonly bool disposeOfDriver = true;
        private bool disposing = false;
        private IWebDriver driver;
        private readonly object syncRoot = new object();

        #endregion Private Members

        #region Properties

        /// <summary>
        /// Gets the browser options that are used in this session.
        /// </summary>
        public BrowserOptions Options { get; private set; }

        /// <summary>
        /// Gets a reference to the underlying selenium driver for this browser.
        /// </summary>
        public IWebDriver Driver
        {
            get
            {
                if (this.driver == null)
                {
                    this.driver = BrowserDriverFactory.CreateWebDriver(this.Options);

                    if (this.Options.FireEvents || this.Options.EnableRecording)
                    {
                        // Wire events
                        var eventDriver = driver as EventFiringWebDriver;

                        if (eventDriver == null)
                            throw new InvalidOperationException(
                                $"Driver of type '{driver.GetType().FullName}' is not an event firing web driver, and one was expected.");

                        eventDriver.ElementClicked += EventDriver_ElementClicked;
                        eventDriver.ElementValueChanged += EventDriver_ElementValueChanged;
                        eventDriver.ExceptionThrown += EventDriver_ExceptionThrown;
                        eventDriver.Navigated += EventDriver_Navigated;

                        if (this.Options.FireEvents)
                        {
                            eventDriver.ElementClicking += EventDriver_ElementClicking;
                            eventDriver.ElementValueChanging += EventDriver_ElementValueChanging;
                            eventDriver.FindElementCompleted += EventDriver_FindElementCompleted;
                            eventDriver.FindingElement += EventDriver_FindingElement;
                            eventDriver.NavigatedBack += EventDriver_NavigatedBack;
                            eventDriver.NavigatedForward += EventDriver_NavigatedForward;
                            eventDriver.Navigating += EventDriver_Navigating;
                            eventDriver.NavigatingBack += EventDriver_NavigatingBack;
                            eventDriver.NavigatingForward += EventDriver_NavigatingForward;
                            eventDriver.ScriptExecuted += EventDriver_ScriptExecuted;
                            eventDriver.ScriptExecuting += EventDriver_ScriptExecuting;
                        }
                    }

                    OnBrowserInitialized(new BrowserInitializeEventArgs(BrowserInitiationSource.NewBrowser));
                }

                return driver;
            }
        }

        public bool IsRecording => this.recorder != null;
        protected TraceSource Trace { get; }
        public List<ICommandResult> CommandResults = new List<ICommandResult>();
        public int TotalThinkTime = 0;
        public string ActiveFrameId = "";
        internal Dictionary<int, int> CommandThinkTimes = new Dictionary<int, int>();
        internal DateTime? LastCommandEndTime;
        public int Depth = 1;

        #endregion Properties

        #region Methods

        public void ThinkTime(int milliseconds)
        {
            if(!CommandThinkTimes.ContainsKey((Depth)))
                CommandThinkTimes.Add(Depth,milliseconds);
            else if(Depth==1)
                CommandThinkTimes[Depth]+= milliseconds;
            else
                CommandThinkTimes[Depth] = milliseconds;

            TotalThinkTime += milliseconds;
            Thread.Sleep(milliseconds);
        }
        public void TakeWindowScreenShot(string path, ScreenshotImageFormat fileFormat)
        {

            this.Driver.TakeScreenshot().SaveAsFile(path, fileFormat);
        }

        public T GetPage<T>()
            where T : BrowserPage
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { this });
        }

        public void Navigate(Uri uri)
        {
            this.Driver.Navigate().GoToUrl(uri);
        }

        public void Navigate(string uri)
        {
            this.Driver.Navigate().GoToUrl(uri);
        }

        public void Navigate(NavigationOperation operation)
        {
            switch (operation)
            {
                case NavigationOperation.NavigateBack:
                    this.Driver.Navigate().Back();

                    break;
                case NavigationOperation.NavigateForward:
                    this.Driver.Navigate().Forward();

                    break;
                case NavigationOperation.Reload:
                    this.Driver.Navigate().Refresh();

                    break;
                default:
                    throw new InvalidOperationException($"The navigation operation '{operation}' is invalid.");
            }
        }

        internal class BackgroundRecorder
        {
            internal BackgroundRecorder(IWebDriver driver, IBrowserActionLogger logger, TimeSpan scanInterval)
            {
                this.Driver = driver;
                this.ScanInterval = scanInterval;
                this.Logger = logger;
                this.IsRecording = false;
            }

            internal IBrowserActionLogger Logger { get; set; }
            internal bool IsRecording { get; private set; }
            internal TimeSpan ScanInterval { get;set; }
            private IWebDriver Driver { get; set; }

            internal void Start()
            {
                //do the initial injection
                this.Driver.SwitchTo().DefaultContent();

                this.InjectRecordingScript();
                this.InjectEventCollectionScript();

                this.IsRecording = true;
            }

            internal void Stop()
            {
                this.IsRecording = false;
            }

            internal void DoWork()
            {
                while (this.IsRecording)
                {
                    this.Driver.SwitchTo().DefaultContent();

                    this.CheckRecordingScripts();

                    this.GetRecordedEvents();

                    Thread.Sleep(this.ScanInterval);
                }

                //The Recording has stopped and we get any remaining events in the queue.
                this.GetRecordedEvents();

            }

            private void CheckRecordingScripts()
            {
                this.Driver.SwitchTo().DefaultContent();

                foreach (var handle in this.Driver.WindowHandles)
                {
                    this.Driver.SwitchTo().Window(handle);
                    InjectRecordingScript();
                }

                CheckFrames(this.Driver);
                this.Driver.SwitchTo().DefaultContent();
            }

            private void GetRecordedEvents()
            {
                List<BrowserRecordingEvent> events;

                // If this script errors out, we'll get an InvalidOperationException.  This may happen if the script is reloading
                // or not yet available.  That's no big deal, as this timer will run again shortly and pick up any missed events.
                try
                {
                    events = this.Driver.GetJsonObject<List<BrowserRecordingEvent>>(Constants.Browser.Recording.GetRecordedEventsCommand);
                }
                catch (InvalidOperationException)
                {
                    return;
                }

                if (events?.Count > 0)
                {
                    this.Logger.LogEvents(events);

                    this.Driver.SwitchTo().DefaultContent();
                    this.Driver.ExecuteScript(Constants.Browser.Recording.RemoveEventsScript.Replace("{0}",
                        events.Count.ToString()));
                }
            }

            private void SwitchToContentFrame(IWebDriver driver)
            {
                var options = new BrowserCommandOptions
                {
                    RetryAttempts = 1,
                    ExceptionTypes = { typeof(StaleElementReferenceException), typeof(NoSuchFrameException) },
                    ExceptionAction = null
                };
                var command = new DelegateBrowserCommand<bool>(options, d =>
                {
                    //wait for the content panel to render
                    driver.WaitUntilAvailable(By.Id("crmContentPanel"));

                    //find the crmContentPanel and find out what the current content frame ID is - then navigate to the current content frame
                    var currentContentFrame = driver.FindElement(By.Id("crmContentPanel")).GetAttribute("currentcontentid");

                    driver.SwitchTo().Frame(currentContentFrame);

                    return true;
                });
            }
            private void CheckFrames(IWebDriver driver)
            {
                // Setup our retry options first.
                var options = new BrowserCommandOptions
                {
                    RetryAttempts = 1,
                    ExceptionTypes = { typeof(StaleElementReferenceException), typeof(NoSuchFrameException) },
                    ExceptionAction = null
                };
                var command = new DelegateBrowserCommand<bool>(options, d =>
                {
                    var iframes = GetFrameElements(d);

                    foreach (IWebElement frame in iframes)
                    {
                        var currentFrameId = frame.GetAttribute("id");

                        driver.WaitUntilAvailable(By.Id(currentFrameId));
                        d.SwitchTo().Frame(currentFrameId);
                        InjectRecordingScript();
                        //CheckFrames(driver); //Child Frames
                    }

                    return true;
                });

                command.Execute(driver);
            }

            private IWebElement[] GetFrameElements(IWebDriver driver)
            {
                return driver.FindElements(By.CssSelector(@"frame, iframe")).ToArray();
            }

            private void InjectEventCollectionScript()
            {
                //check parent window for event collection
                if (!bool.Parse(this.Driver.ExecuteScript(Constants.Browser.Recording.CheckIfEventCollectionExistsScript).ToString()))
                    this.Driver.ExecuteScript(Constants.Browser.Recording.GlobalEventCollectionScript);
            }

            internal void InjectRecordingScript()
            {
                if (!bool.Parse(this.Driver.ExecuteScript(Constants.Browser.Recording.CheckIfScriptExistsScript).ToString()))
                    this.Driver.ExecuteScript(Properties.Resources.Recorder);
            }
        }


        public void Record(IBrowserActionLogger logger)
        {
            if (this.IsRecording)
                return;

            this.recorder = new BackgroundRecorder(this.Driver, logger, Options.RecordingScanInterval);
            this.recorderThread = new Thread(recorder.DoWork)
            {
                IsBackground = true,
                Name = "BrowserRecorder"
            };

            this.recorder.Start();
            this.recorderThread.Start();
        }

        public void StopRecording()
        {
            if (!this.IsRecording)
                return;

            this.recorder.Stop();
            this.recorderThread.Join(Options.RecordingScanInterval);

            if (this.recorderThread.ThreadState != ThreadState.Stopped)
                this.recorderThread.Abort();

            this.recorderThread = null;
            this.recorder = null;
        }

        internal void CalculateResults(ICommandResult result)
        {
            //Calculate Transition time from the previous command end time. Set the LastCommandEndTime to the current command Stop Time
            if (Depth == 1)
            {
                if (result.StartTime.HasValue && LastCommandEndTime.HasValue && Depth == 1)
                {
                    result.TransitionTime = (((result.StartTime.Value - LastCommandEndTime.Value).Seconds * 1000) + (result.StartTime.Value - LastCommandEndTime.Value).Milliseconds) - CommandThinkTimes[Depth];
                }
                LastCommandEndTime = result.StopTime;
            }


            //Calculate ThinkTime for the Current Command. Reset the CurrentCommandThinkTime to 0 when finished
            result.ThinkTime = CommandThinkTimes.ContainsKey(Depth) ? CommandThinkTimes[Depth] : 0;
            result.Depth = Depth;
            CommandResults.Add(result);
            CommandThinkTimes[Depth] = 0;

        }

        #endregion Methods

        #region Event Model

        public EventHandler<BrowserInitializeEventArgs> BrowserInitialized;
        public EventHandler<EventArgs> BrowserDisposing;

        public EventHandler<WebElementEventArgs> ElementClicked;
        public EventHandler<WebElementEventArgs> ElementClicking;
        public EventHandler<WebElementEventArgs> ElementValueChanged;
        public EventHandler<WebElementEventArgs> ElementValueChanging;
        public EventHandler<WebDriverExceptionEventArgs> ExceptionThrown;
        public EventHandler<FindElementEventArgs> FindElementCompleted;
        public EventHandler<FindElementEventArgs> FindingElement;
        public EventHandler<WebDriverNavigationEventArgs> Navigated;
        public EventHandler<WebDriverNavigationEventArgs> NavigatedBack;
        public EventHandler<WebDriverNavigationEventArgs> NavigatedForward;
        public EventHandler<WebDriverNavigationEventArgs> Navigating;
        public EventHandler<WebDriverNavigationEventArgs> NavigatingBack;
        public EventHandler<WebDriverNavigationEventArgs> NavigatingForward;
        public EventHandler<WebDriverScriptEventArgs> ScriptExecuted;
        public EventHandler<WebDriverScriptEventArgs> ScriptExecuting;

        [DebuggerNonUserCode()]
        protected virtual void OnBrowserInitialized(BrowserInitializeEventArgs e)
        {
            var level = e.Source == BrowserInitiationSource.NewBrowser ? TraceEventType.Information : TraceEventType.Verbose;

            Trace.TraceEvent(level,
                Constants.Tracing.BrowserEventFiringEventId,
                "BrowserInitialized invoked.");

            this.BrowserInitialized?.Invoke(this, e);

            Trace.TraceEvent(level,
                Constants.Tracing.BrowserEventFiredEventId,
                "BrowserInitialized completed.");
        }

        [DebuggerNonUserCode()]
        protected virtual void OnBrowserDisposing(EventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Information,
                Constants.Tracing.BrowserEventFiringEventId,
                "BrowserDisposing invoked.");

            this.BrowserDisposing?.Invoke(this, e);

            Trace.TraceEvent(TraceEventType.Information,
                Constants.Tracing.BrowserEventFiredEventId,
                "BrowserDisposing completed.");
        }

        [DebuggerNonUserCode()]
        private void EventDriver_ElementClicked(object sender, WebElementEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserElementClickedEventId,
                string.Format("ElementClicked - [{0},{1}] - <{2}>{3}</{2}>", e.Element.Location.X, e.Element.Location.Y, e.Element.TagName, e.Element.Text));

            OnElementClicked(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnElementClicked(WebElementEventArgs e)
        {
            this.ElementClicked?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_ElementClicking(object sender, WebElementEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserElementClickingEventId,
                string.Format("ElementClicking - [{0},{1}] - <{2}>{3}</{2}>", e.Element.Location.X, e.Element.Location.Y, e.Element.TagName, e.Element.Text));

            OnElementClicking(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnElementClicking(WebElementEventArgs e)
        {
            this.ElementClicking?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserElementValueChangedEventId,
                string.Format("ElementValueChanged - [{0},{1}] - <{2}>{3}</{2}>", e.Element?.Location.X, e.Element?.Location.Y, e.Element?.TagName, e.Element?.Text));

            OnElementValueChanged(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnElementValueChanged(WebElementEventArgs e)
        {
            this.ElementValueChanged?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_ElementValueChanging(object sender, WebElementEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserElementValueChangingEventId,
                string.Format("ElementValueChanging - [{0},{1}] - <{2}>{3}</{2}>", e.Element.Location.X, e.Element.Location.Y, e.Element.TagName, e.Element.Text));

            OnElementValueChanging(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnElementValueChanging(WebElementEventArgs e)
        {
            this.ElementValueChanging?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Error,
                Constants.Tracing.BrowserExceptionThrownEventId,
                $"ExceptionThrown - [{e.ThrownException.GetType().Name}] - {e.ThrownException.Message} (HResult = {e.ThrownException.HResult}, InnerException = {e.ThrownException.InnerException?.Message})");

            OnExceptionThrown(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnExceptionThrown(WebDriverExceptionEventArgs e)
        {
            this.ExceptionThrown?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_FindElementCompleted(object sender, FindElementEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserFindElementCompletedEventId,
                $"FindElementCompleted - {e.ToTraceString()}");

            OnFindElementCompleted(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnFindElementCompleted(FindElementEventArgs e)
        {
            this.FindElementCompleted?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_FindingElement(object sender, FindElementEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserFindingElementEventId,
                $"FindingElement - {e.ToTraceString()}");

            OnFindingElement(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnFindingElement(FindElementEventArgs e)
        {
            this.FindingElement?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_Navigated(object sender, WebDriverNavigationEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserNavigatedEventId,
                $"Navigated - {e.Url}");

            if (Options.EnableRecording)
            {
                driver.ExecuteScript(Properties.Resources.Recorder);
                driver.ExecuteScript("document.eventCollection = new Array();");
            }

            OnNavigated(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnNavigated(WebDriverNavigationEventArgs e)
        {
            this.Navigated?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_NavigatedBack(object sender, WebDriverNavigationEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserNavigatedBackEventId,
                $"NavigatedBack - {e.Url}");

            OnNavigatedBack(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnNavigatedBack(WebDriverNavigationEventArgs e)
        {
            this.NavigatedBack?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_NavigatedForward(object sender, WebDriverNavigationEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserNavigatedForwardEventId,
                $"NavigatedForward - {e.Url}");

            OnNavigatedForward(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnNavigatedForward(WebDriverNavigationEventArgs e)
        {
            this.NavigatedForward?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserNavigatingEventId,
                $"Navigating - {e.Url}");

            OnNavigating(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnNavigating(WebDriverNavigationEventArgs e)
        {
            this.Navigating?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_NavigatingBack(object sender, WebDriverNavigationEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserNavigatingBackEventId,
                $"NavigatingBack - {e.Url}");

            OnNavigatingBack(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnNavigatingBack(WebDriverNavigationEventArgs e)
        {
            this.NavigatingBack?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_NavigatingForward(object sender, WebDriverNavigationEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserNavigatingForwardEventId,
                $"NavigatingForward - {e.Url}");

            OnNavigatingForward(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnNavigatingForward(WebDriverNavigationEventArgs e)
        {
            this.NavigatingForward?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_ScriptExecuted(object sender, WebDriverScriptEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserScriptExecutedEventId,
                $"ScriptExecuted - {e.Script}");

            OnScriptExecuted(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnScriptExecuted(WebDriverScriptEventArgs e)
        {
            this.ScriptExecuted?.Invoke(this, e);
        }

        [DebuggerNonUserCode()]
        private void EventDriver_ScriptExecuting(object sender, WebDriverScriptEventArgs e)
        {
            Trace.TraceEvent(TraceEventType.Verbose,
                Constants.Tracing.BrowserScriptExecutingEventId,
                $"ScriptExecuting - {e.Script}");

            OnScriptExecuting(e);
        }

        [DebuggerNonUserCode()]
        protected virtual void OnScriptExecuting(WebDriverScriptEventArgs e)
        {
            this.ScriptExecuting?.Invoke(this, e);
        }

        #endregion Event Model

        #region Disposal / Finalization

        public void Dispose()
        {
            bool isDisposing;

            lock (this.syncRoot)
            {
                isDisposing = disposing;
            }

            if (!isDisposing)
            {
                lock (this.syncRoot)
                {
                    disposing = true;
                }

                if (recorderThread != null && recorderThread.ThreadState != ThreadState.Aborted)
                {
                    recorderThread.Abort();
                }

                if (driver != null && disposeOfDriver)
                {
                    try
                    {
                        OnBrowserDisposing(EventArgs.Empty);
                    }
                    catch (Exception) { }
                    finally
                    {
                        try
                        {
                            driver.Close();
                        }
                        // If the user has closed the window aleady, this exception would be thrown.
                        catch (NoSuchWindowException) { }
                        finally
                        {
                            try
                            {
                                driver.Quit();
                            }
                            finally
                            {
                                driver = null;
                            }
                        }
                    }
                }
            }
        }

        #endregion Disposal / Finalization
    }
}