using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueCreator.Logging
{
    public class LogScope : IDisposable
    {
        FileLogger _log;
        string _message;
        public LogScope(FileLogger log, string message)
        {
            _log = log;
            _message = message;
            _log.Log($"BEGIN {_message}");
        }

        public void Dispose()
        {
            _log.Log($"END {_message}");
        }
    }
}
