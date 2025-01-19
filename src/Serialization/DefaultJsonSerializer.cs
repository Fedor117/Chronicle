using System.Text.Json;

namespace Chronicle.Serialization
{
    public sealed class DefaultJsonSerializer : IJsonSerializer
    {
        private static readonly JsonSerializerOptions s_defaultOptions = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly JsonSerializerOptions _options;

        public DefaultJsonSerializer(JsonSerializerOptions? options = null)
        {
            _options = options ?? s_defaultOptions;
        }

        public string Serialize(object value)
        {
            try
            {
                return JsonSerializer.Serialize(value, _options);
            }
            catch
            {
                // If serialization fails, fallback to a simple string representation
                return value?.ToString() ?? "null";
            }
        }
    }
}