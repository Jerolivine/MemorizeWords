using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;

namespace MemorizeWords.Infrastructure.Utilities;

public class ValidationTypeUtility
{
    public static void ThrowIfNullOrDefault<T>(T value, string settingsName)
    {
        if (EqualityComparer<T>.Default.Equals(value, default(T)))
            throw new BusinessException($"{settingsName} is not a valid value");
    }
}