using Chronicle.Interfaces;

namespace Chronicle.Builders
{
    public sealed class SingleChronicleBuilder : IChronicleBuilder
    {
        private readonly IChronicler _chronicler;
        private readonly ChronicleLevel _level;
        private readonly Dictionary<string, object> _properties = [];

        private string _message = string.Empty;

        public SingleChronicleBuilder(IChronicler chronicler, ChronicleLevel level)
        {
            _chronicler = chronicler;
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

        public void Write() => _chronicler.Log(_level, _message, _properties);
    }
}