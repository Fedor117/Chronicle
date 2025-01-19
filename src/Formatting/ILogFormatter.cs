namespace Chronicle.Formatting
{
    public interface ILogFormatter
    {
        string FormatLogEntry(DateTime timestamp, ChronicleLevel level, string message, Dictionary<string, object> properties);
    }
}