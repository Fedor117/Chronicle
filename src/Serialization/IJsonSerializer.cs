namespace Chronicle.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object value);
    }
}