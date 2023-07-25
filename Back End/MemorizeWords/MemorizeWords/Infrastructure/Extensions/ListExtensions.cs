using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MemorizeWords.Infrastructure.Extensions
{
    public static class ListExtensions
    {
        public static string ToJson<T>(this List<T> list)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            return JsonSerializer.Serialize(list, options);
        }
    }
}
