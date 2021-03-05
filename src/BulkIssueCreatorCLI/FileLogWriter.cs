using OutputColorizer;
using System;
using System.IO;

namespace BulkIssueCreatorCLI
{
    public class FileLogWriter : IOutputWriter, IDisposable
    {
        private string _fileName;
        private StreamWriter _sw;
        private ConsoleWriter _cw;
        public FileLogWriter(string fileName)
        {
            _fileName = fileName;
            _cw = new ConsoleWriter();
            _cw.ForegroundColor = ConsoleColor.Gray;

            _sw = new StreamWriter(_fileName, true);
        }

        public ConsoleColor ForegroundColor
        {
            get => _cw.ForegroundColor;
            set => _cw.ForegroundColor = value;
        }

        public void Dispose()
        {
            _sw.Flush();
            _sw.Dispose();
        }

        public void Write(string text)
        {
            _sw.Write(text);
            _cw.Write(text);
        }

        public void WriteLine(string text)
        {
            _sw.WriteLine(text);
            _cw.WriteLine(text);
        }
    }
}
