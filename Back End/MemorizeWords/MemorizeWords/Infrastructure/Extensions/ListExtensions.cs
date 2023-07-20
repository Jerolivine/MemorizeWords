using System.Text.Json;

namespace MemorizeWords.Infrastructure.Extensions
{
    public static class ListExtensions
    {
        public static string ToJson<T>(this List<T> list)
        {
            return JsonSerializer.Serialize(list);
        }
    }
}
