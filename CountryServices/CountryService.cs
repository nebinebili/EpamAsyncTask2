namespace CountryServices;

/// <summary>
/// Provides information about country local currency from RESTful API
/// <see><cref>https://restcountries.com/docs</cref></see>.
/// </summary>
public class CountryService : ICountryService
{
    private const string DefaultServiceUrl = "https://api.restcountries.com/countries/v5";
    private const string ServiceUrlEnvironmentVariable = "RESTCOUNTRIES_API_BASE_URL";
    private const string ApiKeyEnvironmentVariable = "RESTCOUNTRIES_API_KEY";

    private readonly Dictionary<string, WeakReference<LocalCurrency>> currencyCountries = [];

    /// <summary>
    /// Current API base URL. Prefer this property over caching the URL in a static field —
    /// grading environments may change <c>RESTCOUNTRIES_API_BASE_URL</c> between test fixtures.
    /// </summary>
    private static string ServiceUrl =>
        Environment.GetEnvironmentVariable(ServiceUrlEnvironmentVariable)
        ?? DefaultServiceUrl;

    /// <summary>
    /// Gets information about currency by country code synchronously.
    /// </summary>
    /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
    /// <see><cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref></see>
    /// <returns>Information about country currency as <see cref="LocalCurrency"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if alpha2Or3Code is null, empty, whitespace or invalid country code.</exception>
    public LocalCurrency GetLocalCurrencyByAlpha2Or3Code(string? alpha2Or3Code)
    {
        // TODO: Use WebClient and JsonSerializer classes.
        // Use ServiceUrl and read RESTCOUNTRIES_API_KEY via Environment.GetEnvironmentVariable on each request (do not cache in static fields).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets information about currency by country code asynchronously.
    /// </summary>
    /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
    /// <see><cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref></see>.
    /// <param name="token">Token for the cancellation of asynchronous operation.</param>
    /// <returns>Information about country currency as <see cref="LocalCurrency"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if alpha2Or3Code is null, empty, whitespace or invalid country code.</exception>
    public Task<LocalCurrency> GetLocalCurrencyByAlpha2Or3CodeAsync(string? alpha2Or3Code, CancellationToken token)
    {
        // TODO: Use HttpClient and JsonSerializer classes. Notice the difference from WebClient class. In the future, in a similar situation, use only HttpClient.
        // Use ServiceUrl and read RESTCOUNTRIES_API_KEY via Environment.GetEnvironmentVariable on each request (do not cache in static fields).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets information about the country by the country capital synchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <returns>Information about the country as <see cref="Country"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the capital name is null, empty, whitespace or nonexistent.</exception>
    public Country GetCountryInfoByCapital(string? capital)
    {
        // TODO: Use WebClient and JsonSerializer classes.
        // Use ServiceUrl and read RESTCOUNTRIES_API_KEY via Environment.GetEnvironmentVariable on each request (do not cache in static fields).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets information about the country by the country capital asynchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <param name="token">Token for cancellation of asynchronous operation.</param>
    /// <returns>Information about the country as <see cref="Country"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the capital name is null, empty, whitespace or nonexistent.</exception>
    public Task<Country> GetCountryInfoByCapitalAsync(string? capital, CancellationToken token)
    {
        // TODO: Use HttpClient and JsonSerializer classes. Notice the difference from WebClient class. In the future, in a similar situation, use only HttpClient.
        // Use ServiceUrl and read RESTCOUNTRIES_API_KEY via Environment.GetEnvironmentVariable on each request (do not cache in static fields).
        throw new NotImplementedException();
    }
}
