namespace Chronicle.Builders
{
    public sealed class MultiChronicleBuilder : IChronicleBuilder
    {
        private readonly ChronicleCollection _collection;
        private readonly ChronicleLevel _level;
        private readonly Dictionary<string, object> _properties = [];

        private string _message = string.Empty;

        public MultiChronicleBuilder(ChronicleCollection collection, ChronicleLevel level)
        {
            _collection = collection;
            _level = level;
        }

        public IChronicleBuilder Message(string message)
        {
            _message = message;
            return this;
        }

        public IChronicleBuilder Property(string key, object value)
        {
            _properties[key] = value;
            return this;
        }

        public void Write() => _collection.Log(_level, _message, _properties);
    }
}
