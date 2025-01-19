namespace Chronicle
{

    /// <summary>
    /// Defines available chronicle levels.
    /// </summary>
    public sealed class ChronicleLevel : IComparable<ChronicleLevel>, IComparable, IEquatable<ChronicleLevel>
    {
        /// <summary>
        /// Trace level (Ordinal = 0). Most verbose level. Used for development and seldom enabled in production.
        /// </summary>
        public static readonly ChronicleLevel Trace = new("Trace", 0);

        /// <summary>
        /// Debug level (Ordinal = 1). Debugging the application behavior from internal events of interest.
        /// </summary>
        public static readonly ChronicleLevel Debug = new("Debug", 1);

        /// <summary>
        /// Info level (Ordinal = 2). Information that highlights progress or application lifetime events.
        /// </summary>
        public static readonly ChronicleLevel Info = new("Info", 2);

        /// <summary>
        /// Warn level (Ordinal = 3). Warnings about validation issues or temporary failures that can be recovered.
        /// </summary>
        public static readonly ChronicleLevel Warn = new("Warn", 3);

        /// <summary>
        /// Error level (Ordinal = 4). Errors where functionality has failed or exceptions have been caught.
        /// </summary>
        public static readonly ChronicleLevel Error = new("Error", 4);

        /// <summary>
        /// Fatal level (Ordinal = 5). Most critical level. Application is about to abort.
        /// </summary>
        public static readonly ChronicleLevel Fatal = new("Fatal", 5);

        /// <summary>
        /// Off level (Ordinal = 6). Used to disable logging.
        /// </summary>
        public static readonly ChronicleLevel Off = new("Off", 6);

        private static readonly IList<ChronicleLevel> s_allLevels =
            new List<ChronicleLevel> { Trace, Debug, Info, Warn, Error, Fatal, Off }.AsReadOnly();

        private static readonly IList<ChronicleLevel> s_allLoggingLevels =
            new List<ChronicleLevel> { Trace, Debug, Info, Warn, Error, Fatal }.AsReadOnly();

        public static IEnumerable<ChronicleLevel> AllLevels => s_allLevels;
        public static IEnumerable<ChronicleLevel> AllLoggingLevels => s_allLoggingLevels;

        internal static ChronicleLevel MaxLevel => Fatal;
        internal static ChronicleLevel MinLevel => Trace;

        public string Name { get; }
        public int Ordinal { get; }

        private ChronicleLevel(string name, int ordinal)
        {
            Name = name;
            Ordinal = ordinal;
        }

        public static bool operator ==(ChronicleLevel level1, ChronicleLevel level2)
        {
            if (ReferenceEquals(level1, level2))
                return true;
            return (level1 ?? Off).Equals(level2);
        }

        public static bool operator !=(ChronicleLevel level1, ChronicleLevel level2)
        {
            if (ReferenceEquals(level1, level2))
                return false;
            return !(level1 ?? Off).Equals(level2);
        }

        public static bool operator >(ChronicleLevel level1, ChronicleLevel level2)
        {
            if (ReferenceEquals(level1, level2))
                return false;
            return (level1 ?? Off).CompareTo(level2) > 0;
        }

        public static bool operator >=(ChronicleLevel level1, ChronicleLevel level2)
        {
            if (ReferenceEquals(level1, level2))
                return true;
            return (level1 ?? Off).CompareTo(level2) >= 0;
        }

        public static bool operator <(ChronicleLevel level1, ChronicleLevel level2)
        {
            if (ReferenceEquals(level1, level2))
                return false;
            return (level1 ?? Off).CompareTo(level2) < 0;
        }

        public static bool operator <=(ChronicleLevel level1, ChronicleLevel level2)
        {
            if (ReferenceEquals(level1, level2))
                return true;
            return (level1 ?? Off).CompareTo(level2) <= 0;
        }

        public static ChronicleLevel FromOrdinal(int ordinal)
        {
            return ordinal switch
            {
                0 => Trace,
                1 => Debug,
                2 => Info,
                3 => Warn,
                4 => Error,
                5 => Fatal,
                6 => Off,
                _ => throw new ArgumentException($"Unknown chronicle level: {ordinal}", nameof(ordinal)),
            };
        }

        public static ChronicleLevel FromString(string levelName)
        {
            if (string.IsNullOrEmpty(levelName))
                throw new ArgumentNullException(nameof(levelName));

            if (levelName.Equals("Trace", StringComparison.OrdinalIgnoreCase))
                return Trace;
            if (levelName.Equals("Debug", StringComparison.OrdinalIgnoreCase))
                return Debug;
            if (levelName.Equals("Info", StringComparison.OrdinalIgnoreCase))
                return Info;
            if (levelName.Equals("Warn", StringComparison.OrdinalIgnoreCase))
                return Warn;
            if (levelName.Equals("Error", StringComparison.OrdinalIgnoreCase))
                return Error;
            if (levelName.Equals("Fatal", StringComparison.OrdinalIgnoreCase))
                return Fatal;
            if (levelName.Equals("Off", StringComparison.OrdinalIgnoreCase))
                return Off;

            throw new ArgumentException($"Unknown chronicle level: {levelName}", nameof(levelName));
        }

        public override string ToString() => Name;

        public override int GetHashCode() => Ordinal;

        public override bool Equals(object? obj) => Equals(obj as ChronicleLevel);

        public bool Equals(ChronicleLevel? other) => Ordinal == other?.Ordinal;

        public int CompareTo(object? obj) => CompareTo(obj as ChronicleLevel);

        public int CompareTo(ChronicleLevel? other) => Ordinal - (other ?? Off).Ordinal;
    }
}