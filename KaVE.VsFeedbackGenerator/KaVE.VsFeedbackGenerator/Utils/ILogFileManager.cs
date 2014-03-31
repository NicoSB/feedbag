﻿using System;
using System.Collections.Generic;

namespace KaVE.VsFeedbackGenerator.Utils
{
    public interface ILogFileManager<TMessage>
    {
        IEnumerable<string> GetLogFileNames();
        ILogWriter<TMessage> NewLogWriter(string logFileName);
        ILogReader<TMessage> NewLogReader(string logFileName);
        void DeleteLogs(params string[] logFileNames);
        void DeleteLogsOlderThan(DateTime time);
        string GetLogFileName(string filename, string extension = null);

        string BaseLocation { get; }
        string DefaultExtension { get; }
    }
}
