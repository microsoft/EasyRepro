using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    // credit:_ Arseni Mourzenko - https://stackoverflow.com/users/240613/arseni-mourzenko
    // Why are messages sent to trace source missing from all but the first unit test?
    // https://stackoverflow.com/questions/36327531/why-are-messages-sent-to-trace-source-missing-from-all-but-the-first-unit-test
    public class ConsoleTraceListener : TextWriterTraceListener
    {
        public ConsoleTraceListener() : base(new ConsoleTextWriter()) { }

        public override void Close() { }
    }

    public class ConsoleTextWriter : TextWriter
    {
        public override Encoding Encoding => Console.Out.Encoding;

        public override void Write(string value) => Console.Out.Write(value);

        public override void WriteLine(string value) => Console.Out.WriteLine(value);
    }
}