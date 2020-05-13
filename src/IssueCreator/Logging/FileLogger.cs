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
        public FileLogger(string logPath = "")
        {
            if (string.IsNullOrEmpty(logPath))
            {
                logPath = "log.txt";
            }

            _logPath = logPath;
        }

        public void Log(string message)
        {
            lock (_logPath)
            {
                File.AppendAllText(_logPath, $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}: {message} {Environment.NewLine}");
            }
        }
    }
}
