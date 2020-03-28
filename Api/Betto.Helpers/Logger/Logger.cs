using Betto.Helpers.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace Betto.Helpers
{
    public class Logger : ILogger
    {
        private readonly LoggingConfiguration _configuration;
        private static readonly object _padlock = new object();

        public Logger(IOptions<LoggingConfiguration> configuration)
        {
            this._configuration = configuration.Value;
        }

        public void LogToFile(string filename, string content)
        {
            lock (_padlock)
            {
                var filePath = GetBackupFilePath(filename);
                File.WriteAllText(filePath, content);
            }
        }

        private string GetBackupFilePath(string filename)
            => string.Concat(_configuration.BackupDirectory, filename, DateTime.Now.ToString("yyyyMMdd_HHmmssfff"), ".txt");
    }
}
