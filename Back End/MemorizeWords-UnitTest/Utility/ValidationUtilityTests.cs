using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;
using MemorizeWords.Infrastructure.Utilities;
using Microsoft.Extensions.Configuration;

namespace MemorizeWords_UnitTest.Utility;

public class ValidationUtilityTests
{
    private readonly IConfiguration _configuration;

    public ValidationUtilityTests()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();
    }

    [Fact]
    public void IS_DEFAULT_TEST_SHOULD_DONE()
    {
        // Arrange
        var settingsName = "SomeSetting";

        // Act & Assert
        Assert.Throws<BusinessException>(() =>
            ValidationTypeUtility.ThrowIfNullOrDefault(default(int), settingsName));
        Assert.Throws<BusinessException>(() =>
            ValidationTypeUtility.ThrowIfNullOrDefault(default(string), settingsName));
        Assert.Throws<BusinessException>(() =>
            ValidationTypeUtility.ThrowIfNullOrDefault(default(object), settingsName));
        Assert.Throws<BusinessException>(() =>
            ValidationTypeUtility.ThrowIfNullOrDefault(default(decimal), settingsName));
        Assert.Throws<BusinessException>(() =>
            ValidationTypeUtility.ThrowIfNullOrDefault(default(bool), settingsName));
        Assert.Throws<BusinessException>(() =>
            ValidationTypeUtility.ThrowIfNullOrDefault(default(Customer), settingsName));
        
    }
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}

