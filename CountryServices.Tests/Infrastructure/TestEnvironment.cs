using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace CountryServices.Tests.Infrastructure;

internal static class TestEnvironment
{
    /// <summary>
    /// Configures the real REST Countries API using the student's personal key from User Secrets or environment.
    /// </summary>
    public static void RequireRealApiKey()
    {
        Environment.SetEnvironmentVariable("RESTCOUNTRIES_API_BASE_URL", null);

        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("RESTCOUNTRIES_API_KEY")))
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<CountryCurrencyServiceTests>(optional: true)
                .Build();

            var apiKey = configuration["RESTCOUNTRIES_API_KEY"];
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                Environment.SetEnvironmentVariable("RESTCOUNTRIES_API_KEY", apiKey);
            }
        }

        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("RESTCOUNTRIES_API_KEY")))
        {
            Assert.Fail(
                "Local tests require your personal REST Countries API key. " +
                "Register at https://restcountries.com/sign-up and run: " +
                "dotnet user-secrets set RESTCOUNTRIES_API_KEY \"your_key\" --project CountryServices.Tests");
        }
    }
}
