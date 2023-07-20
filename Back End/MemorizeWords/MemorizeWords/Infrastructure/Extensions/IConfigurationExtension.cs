using MemorizeWords.Infrastructure.Constants.AppSettings;
using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;
using MemorizeWords.Infrastructure.Utilities;

namespace MemorizeWords.Infrastructure.Extensions;

public static class IConfigurationExtension
{
    public static T GetSettingsValue<T>(this IConfiguration configuration, string settingsName)
    {
        var value = configuration.GetValue<T>(settingsName);
        var exceptionMessage = $"{settingsName} is not a valid value";
        ValidationTypeUtility.ThrowIfNullOrDefault(value, exceptionMessage);

        return value;
    }

    public static int GetSequentTrueAnswerCount(this IConfiguration configuration)
    {
        return configuration.GetSettingsValue<int>(AppSettingsConstants.SEQUENT_TRUE_ANSWER_COUNT);
    }

    public static int GetEnoughAnswerToMemorize(this IConfiguration configuration)
    {
        return configuration.GetSettingsValue<int>(AppSettingsConstants.ENOUGH_ANSWER_TO_MEMORIZE);
    }
}