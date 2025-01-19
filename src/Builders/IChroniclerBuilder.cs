namespace Chronicle.Builders
{
    public interface IChronicleBuilder
    {
        public IChronicleBuilder Message(string message);
        public IChronicleBuilder Property(string key, object value);
        public void Write();
    }
}
