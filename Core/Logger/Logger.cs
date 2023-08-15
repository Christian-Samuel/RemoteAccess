using Core.Logger.Interface;

namespace Core.Logger
{
    public class Logger : ILogger
    {
        private StreamWriter? _logWriter;

        private const string LogFolderName = "RemoteAccess";
        private const string LogSubfolderName = "Logs";
        private const string LogFileNameFormat = "Log_{0:yyyyMMdd_HHmmss}.txt";

        public Logger()
        {

        }

        public void WriteToLogFile(string message)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string logFolderPath = Path.Combine(documentsPath, LogFolderName, LogSubfolderName);

            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }

            string logFileName = string.Format(LogFileNameFormat, DateTime.Now);
            string logFilePath = Path.Combine(logFolderPath, logFileName);

            if (_logWriter is null)
            {
                _logWriter = new StreamWriter(logFilePath, true);
            }

            _logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");
            _logWriter.Flush();
        }

        public void CloseLogFile()
        {
            _logWriter?.Close();
            _logWriter = null;
        }
    }
}
