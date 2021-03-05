using OutputColorizer;
using System;
using System.Diagnostics;

namespace BulkIssueCreatorCLI
{
    public class LoggingScope : IDisposable
    {
        private string _scopeName;
        private object[] _arguments;
        private Stopwatch _timer;

        public LoggingScope(string scopeName, params object[] arguments)
        {
            _scopeName = scopeName;
            _arguments = arguments;

            Colorizer.Write("[Magenta!Start] ");
            Colorizer.WriteLine(scopeName, arguments);
            _timer = new Stopwatch();
            _timer.Start();
        }


        public void Dispose()
        {
            _timer.Stop();

            Colorizer.Write("[Green!Done] ");
            Colorizer.Write(_scopeName, _arguments);
            Colorizer.WriteLine(" in [Cyan!{0}ms]", _timer.Elapsed.Milliseconds);
        }
    }
}
