using MemorizeWords.Infrastructure.Constants.AppSettings;
using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;
using MemorizeWords.Infrastructure.Utilities;

namespace MemorizeWords.Infrastructure.Extensions
{
    public static class IConfigurationExtension
    {
        public static T GetSettingsValue<T>(this IConfiguration configuration, string settingsName) 
        {
            var value = configuration.GetValue<T>(settingsName);

            
            // if (EqualityComparer<T>.Default.Equals(value, default(T)))
            // {
            //     throw new BusinessException($"{settingsName} is not an integer");
            // }
            
            ValidationTypeUtility.ThrowIfNullOrDefault(value, settingsName);

            return value;
        }

        public static int GetSequentTrueAnswerCount(this IConfiguration configuration) => configuration.GetSettingsValue<int>(AppSettingsConstants.SEQUENT_TRUE_ANSWER_COUNT);

        public static int GetEnoughAnswerToMemorize(this IConfiguration configuration) => configuration.GetSettingsValue<int>(AppSettingsConstants.ENOUGH_ANSWER_TO_MEMORIZE);
    }
}
