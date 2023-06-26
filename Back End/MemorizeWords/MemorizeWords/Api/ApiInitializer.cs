using MemorizeWords.Api.Apis;
using System.Reflection;

namespace MemorizeWords.Api
{
    public static class ApiInitializer
    {
        public static void InitializeApis(this WebApplication app)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var implementations = assembly.GetTypes()
                .Where(t => typeof(IInitializer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            // Register the implementations in the service collection
            foreach (var implementation in implementations)
            {
                Type implementationType = (Type)implementation; // Casting to System.Type

                // Create an instance of the implementation type
                IInitializer initializer = (IInitializer)Activator.CreateInstance(implementationType);

                // Call the interface method
                initializer.Initialize(app);
            }

        }
    }
}
