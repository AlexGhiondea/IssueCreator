using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueCreator.Logging
{
    public class FileLogger
    {
        private string _logPath;
        public FileLogger(string logPath)
        {
            if (string.IsNullOrEmpty(logPath))
            {
                throw new ArgumentNullException(nameof(logPath));
            }

            _logPath = logPath;
        }

        public LogScope CreateScope(string message)
        {
            return new LogScope(this, message);
        }

        internal void Log(string message)
        {
            lock (_logPath)
            {
                File.AppendAllText(_logPath, $"{DateTime.Now:s}: {message} {Environment.NewLine}");
            }
        }
    }
}
