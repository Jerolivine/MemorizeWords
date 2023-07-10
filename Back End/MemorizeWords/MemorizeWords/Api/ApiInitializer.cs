using MemorizeWords.Infrastructure.Utilities;
using System.Reflection;

namespace MemorizeWords.Api
{
    public static class ApiInitializer
    {
        public static void InitializeApis(this WebApplication app)
        {
            var implementations = GenericUtility.GetImplementationsByType<IInitializer>();

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
