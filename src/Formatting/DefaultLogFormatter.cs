using Chronicle.Context;

namespace Chronicle.Formatting
{
    public class DefaultLogFormatter : ILogFormatter
    {
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private const string MessageFormat = "{0}|{1}|{2}|{3}";

        public string FormatLogEntry(DateTime timestamp, ChronicleLevel level, string message, Dictionary<string, object> properties)
        {
            string timeString = timestamp.ToString(DateFormat);
            string propertiesString = properties.Any()
                ? " | " + ChronicleContext.Current.JsonSerializer.Serialize(properties)
                : "";

            string logEntry = string.Format(MessageFormat, timeString, level, message, propertiesString);
            return logEntry;
        }
    }
}
