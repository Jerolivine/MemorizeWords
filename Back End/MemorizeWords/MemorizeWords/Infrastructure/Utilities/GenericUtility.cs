using System.Reflection;

namespace MemorizeWords.Infrastructure.Utilities
{
    public class GenericUtility
    {
        /// <summary>
        /// TType tipinden tureyen tum classlarin tiplerini doner
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static List<Type> GetImplementationsByType<TType>()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var implementations = assembly.GetTypes()
                .Where(t => typeof(TType).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            return implementations;

        }

    }
}
