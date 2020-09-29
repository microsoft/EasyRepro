using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Microsoft.Dynamics365.UIAutomation.Browser.Logs
{
    public class TracingService
    {
        private readonly TraceSource _trace;
        private readonly string _loggerName;

        public TracingService(Type type, string traceSource)
            : this(type.Name, traceSource) { }

        public TracingService(string loggerName, string traceSource) 
        {
            _loggerName = loggerName ?? "<undefined>";
            _trace = new TraceSource(traceSource);
        }

        /// <summary>
        /// Log Message to Trace, include Caller Method Name (safe if trace is null)
        /// </summary>
        public void Log(TraceEventType eventType, object message = null, int stackTraceIndex = 1)
        {
            try
            {
                var stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(stackTraceIndex)?.GetMethod()?.Name;
                Write(eventType, $"{method}: {message.Format()}");
            }
            catch(Exception ex)
            {
                TraceFail(ex, nameof(Log));
            }
        }

        /// <summary>
        /// Log Message to Trace, include Caller Method Name (safe if trace is null)
        /// </summary>
        public void Log(TraceEventType eventType, string message, params object[] arguments)
        {
            try
            {
                var stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1)?.GetMethod()?.Name;
                if (arguments == null)
                    message = string.Format(message, "null");
                else
                {
                    arguments = arguments.Select(a => (object)a.Format()).ToArray();
                    message = string.Format(message, arguments);
                }
                Write(eventType, $"{method}: {message}");
            }
            catch(Exception ex)
            {
                TraceFail(ex, nameof(Log));
            }
        }

        /// <summary>
        /// Log Message to Trace, include Caller Method Name (safe if trace is null)
        /// </summary>
        public void Log(string message, params object[] arguments)
        {
            try
            {
                var stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1)?.GetMethod()?.Name;
                if (arguments == null)
                    message = string.Format(message, "null");
                else
                {
                    arguments = arguments.Select(a => (object)a.Format()).ToArray();
                    message = string.Format(message, arguments);
                }
                Write(TraceEventType.Information, $"{method}: {message}");
            }
            catch(Exception ex)
            {
                TraceFail(ex, nameof(Log));
            }
        }


        /// <summary>
        /// Log Message to Trace, include Caller Method Name (safe if trace is null)
        /// </summary>
        public void Log(TraceEventType eventType, string message, object argument)
        {
            try
            {
                var stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1)?.GetMethod()?.Name;
                Write(eventType, $"{method}: {message}{argument.Format()}");
            }
            catch(Exception ex)
            {
                TraceFail(ex, nameof(Log));
            }
        }
        
        protected void Write(TraceEventType eventType, string message)
        {
            var date = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffff");
            try
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var finalMessage = $"[{threadId, 5}] [{date}] [{_loggerName}] - {message}";
                _trace.TraceEvent(eventType, 0, finalMessage);
            }
            catch(Exception ex)
            {
                TraceFail(ex, nameof(Write));
            }
        }

        private static void TraceFail(Exception ex, string method)
        {
            var message = $"An error occur in {nameof(TracingService)}.{method}: {ex}";
          
            Trace.TraceError($"Trace Error => {message}");
            // ReSharper disable once LocalizableElement
            Console.WriteLine($"Console Log => {message}");
        }
    }
}
