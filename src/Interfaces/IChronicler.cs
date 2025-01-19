using Chronicle.Builders;

namespace Chronicle.Interfaces
{
    public interface IChronicler
    {
        IChronicleBuilder Fatal();
        IChronicleBuilder Error();
        IChronicleBuilder Warn();
        IChronicleBuilder Info();
        IChronicleBuilder Debug();
        IChronicleBuilder Trace();
        
        void Log(ChronicleLevel level, string message, Dictionary<string, object> properties);
    }
}
