using Chronicle.Builders;
using Chronicle.Context;
using Chronicle.Formatting;
using Chronicle.Interfaces;

namespace Chronicle.Implementations
{
    public sealed class FileChronicler : IChronicler, IDisposable
    {
        public sealed class Options
        {
            /// <summary>
            /// Size of the write buffer in bytes. Default is 4KB.
            /// </summary>
            public int BufferSize { get; set; } = 4096;

            /// <summary>
            /// Whether to automatically flush after each write. Default is true.
            /// </summary>
            public bool AutoFlush { get; set; } = true;

            /// <summary>
            /// Maximum size of a log file before rolling. Default is 10MB. Set to 0 to disable.
            /// </summary>
            public long MaxFileSize { get; set; } = 10 * 1024 * 1024;

            /// <summary>
            /// Maximum number of rolling files to keep. Default is 5. Set to 0 to disable rolling.
            /// </summary>
            public int MaxRollingFiles { get; set; } = 5;
        }

        private readonly string _filePath;
        private readonly ILogFormatter _formatter;
        private readonly Options _options;
        private readonly object _lock = new();
        private bool _disposed;
        private readonly StreamWriter _writer;

        public FileChronicler(string filePath, ILogFormatter? formatter = null, Options? options = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _filePath = filePath;
            _formatter = formatter ?? ChronicleContext.Current.LogFormatter;
            _options = options ?? new Options();

            try
            {
                string? directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                _writer = new StreamWriter(
                    new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read),
                    bufferSize: _options.BufferSize
                ) { AutoFlush = _options.AutoFlush };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to initialize log file: {ex.Message}", ex);
            }
        }

        public IChronicleBuilder Fatal() => this.IsEnabled(ChronicleLevel.Fatal) ? new SingleChronicleBuilder(this, ChronicleLevel.Fatal) : null;
        public IChronicleBuilder Error() => this.IsEnabled(ChronicleLevel.Error) ? new SingleChronicleBuilder(this, ChronicleLevel.Error) : null;
        public IChronicleBuilder Warn() => this.IsEnabled(ChronicleLevel.Warn) ? new SingleChronicleBuilder(this, ChronicleLevel.Warn) : null;
        public IChronicleBuilder Info() => this.IsEnabled(ChronicleLevel.Info) ? new SingleChronicleBuilder(this, ChronicleLevel.Info) : null;
        public IChronicleBuilder Debug() => this.IsEnabled(ChronicleLevel.Debug) ? new SingleChronicleBuilder(this, ChronicleLevel.Debug) : null;
        public IChronicleBuilder Trace() => this.IsEnabled(ChronicleLevel.Trace) ? new SingleChronicleBuilder(this, ChronicleLevel.Trace) : null;

        public void Log(ChronicleLevel level, string message, Dictionary<string, object> properties)
        {
            if (!this.IsEnabled(level) || _disposed)
            {
                return;
            }

            string logLine = _formatter.FormatLogEntry(DateTime.UtcNow, level, message, properties);

            lock (_lock)
            {
                try
                {
                    _writer.WriteLine(logLine);
                    
                    if (_options.MaxFileSize > 0 && new FileInfo(_filePath).Length > _options.MaxFileSize)
                    {
                        RollFile();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to write log: {ex.Message}");
                }
            }
        }

        private void RollFile()
        {
            if (_options.MaxRollingFiles <= 0)
            {
                return;
            }

            _writer.Flush();

            for (int i = _options.MaxRollingFiles - 1; i >= 0; i--)
            {
                string sourceFile = i == 0 ? _filePath : $"{_filePath}.{i}";
                string targetFile = $"{_filePath}.{i + 1}";

                if (File.Exists(sourceFile))
                {
                    if (File.Exists(targetFile))
                    {
                        File.Delete(targetFile);
                    }
                    File.Move(sourceFile, targetFile);
                }
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _writer?.Dispose();
            _disposed = true;
        }
    }
}
