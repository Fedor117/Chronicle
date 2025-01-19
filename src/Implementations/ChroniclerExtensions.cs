using Chronicle.Context;
using Chronicle.Interfaces;

namespace Chronicle.Implementations
{
    public static class ChroniclerExtensions
    {
        public static bool IsEnabled(this IChronicler chronicler, ChronicleLevel level)
        {
            return level >= ChronicleContext.Current.MinimumLevel;
        }
    }
}