﻿#if !DEBUG
using System.IO.Compression;
#endif
using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Application;
using KaVE.Model.Events;
using KaVE.Utils.Assertion;
using KaVE.VsFeedbackGenerator.MessageBus;
using KaVE.VsFeedbackGenerator.Utils.Json;

namespace KaVE.VsFeedbackGenerator
{
    [ShellComponent]
    class Initializer
    {
        internal const string LogFileExtension = ".log";
        private const string ProjectName = "KAVE";
        private static readonly string EventLogScopeName = typeof (Initializer).Assembly.GetName().Name;

        private readonly IMessageBus _messageChannel;

        public Initializer(IMessageBus messageBus)
        {
            _messageChannel = messageBus;
            _messageChannel.Subscribe<IDEEvent>(LogIDEEvent);
        }

        private void LogIDEEvent(IDEEvent ce)
        {
            lock (_messageChannel)
            {
                var logPath = GetSessionEventLogFilePath(ce);
                EnsureLogDirectoryExists(logPath);
                Debug.WriteLine("Logging IDE Events to: '" + logPath + "'");
                using (var logWriter = NewLogWriter(logPath))
                {
                    logWriter.Write(ce);
                }
            }
        }

        private static string GetSessionEventLogFilePath(IDEEvent evt)
        {
            return Path.Combine(EventLogsDirectory, evt.IDESessionUUID + LogFileExtension);
        }

        internal static string EventLogsDirectory
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appDataPath, ProjectName, EventLogScopeName);
            }
        }

        private static void EnsureLogDirectoryExists(string logPath)
        {
            var logDir = Path.GetDirectoryName(logPath);
            Asserts.NotNull(logDir, "could not determine log directly from path '{0}'", logPath);
            Directory.CreateDirectory(logDir);
        }

        private static JsonLogWriter NewLogWriter(string logFilePath)
        {
            Stream logStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);
            try
            {
#if !DEBUG
                logStream = new GZipStream(logStream, CompressionMode.Compress);
#endif
                return new JsonLogWriter(logStream);
            }
            catch (Exception)
            {
                logStream.Close();
                throw;
            }
        }
    }
}

