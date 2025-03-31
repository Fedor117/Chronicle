using Chronicle.Formatting;
using Chronicle.Serialization;

namespace Chronicle.Context
{
    public sealed class ChronicleContext
    {
        private static ChronicleContext _current = new();
        
        public static ChronicleContext Current
        {
            get => _current;
            set => _current = value ?? throw new ArgumentNullException(nameof(value));
        }

        public ChronicleLevel MinimumLevel { get; set; } = ChronicleLevel.Info;
        public IJsonSerializer JsonSerializer { get; set; } = new DefaultJsonSerializer();
        public ILogFormatter LogFormatter { get; set; } = new DefaultLogFormatter();

        public static void Reset()
        {
            _current = new ChronicleContext();
        }
    }
}
