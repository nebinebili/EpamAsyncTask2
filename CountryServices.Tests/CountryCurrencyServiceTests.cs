using CountryServices.Tests.Infrastructure;
using NUnit.Framework;

namespace CountryServices.Tests;

/// <summary>
/// Local verification against the real REST Countries API using the student's personal API key.
/// Getting and configuring the key is a required part of this assignment.
/// </summary>
[TestFixture]
public class CountryCurrencyServiceTests : CountryCurrencyServiceTestsBase
{
    [OneTimeSetUp]
    public void FixtureSetup()
    {
        TestEnvironment.RequireRealApiKey();
        this.InitializeService();
    }
}
