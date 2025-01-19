using Chronicle.Builders;
using Chronicle.Interfaces;

namespace Chronicle
{
    public class ChronicleCollection : IChronicler
    {
        private readonly List<IChronicler> _chroniclers = [];
        private bool _disposed;
        
        public void AddChronicler(IChronicler chronicler) => _chroniclers.Add(chronicler);
        public void RemoveChronicler(IChronicler chronicler) => _chroniclers.Remove(chronicler);
        
        public IChronicleBuilder Fatal() => new MultiChronicleBuilder(this, ChronicleLevel.Fatal);
        public IChronicleBuilder Error() => new MultiChronicleBuilder(this, ChronicleLevel.Error);
        public IChronicleBuilder Warn() => new MultiChronicleBuilder(this, ChronicleLevel.Warn);
        public IChronicleBuilder Info() => new MultiChronicleBuilder(this, ChronicleLevel.Info);
        public IChronicleBuilder Debug() => new MultiChronicleBuilder(this, ChronicleLevel.Debug);
        public IChronicleBuilder Trace() => new MultiChronicleBuilder(this, ChronicleLevel.Trace);
        
        public void Log(ChronicleLevel level, string message, Dictionary<string, object> properties)
        {
            foreach (IChronicler chronicler in _chroniclers)
            {
                chronicler.Log(level, message, properties);
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            foreach (IChronicler chronicler in _chroniclers)
            {
                if (chronicler is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            _chroniclers.Clear();
            _disposed = true;
        }
    }
}
