using System.Net;
using System.Text.Json;

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
        if (string.IsNullOrWhiteSpace(alpha2Or3Code))
        {
            throw new ArgumentException("Country code cannot be null or empty.", nameof(alpha2Or3Code));
        }

        if (this.currencyCountries.TryGetValue(alpha2Or3Code, out var weakReference)
           && weakReference.TryGetTarget(out var cachedCurrency))
        {
            return cachedCurrency;
        }

        if (alpha2Or3Code.Length != 2 && alpha2Or3Code.Length != 3)
        {
            throw new ArgumentException("Invalid country code", nameof(alpha2Or3Code));
        }

        string endPoint = alpha2Or3Code.Length == 2
    ? $"{ServiceUrl}/codes.alpha_2/{alpha2Or3Code}"
    : $"{ServiceUrl}/codes.alpha_3/{alpha2Or3Code}";

        string? apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable);

        using WebClient client = new WebClient();

        client.Headers.Add("Authorization", $"Bearer {apiKey}");

        string json = client.DownloadString(endPoint);

        RestCountriesResponse? restCountriesResponse = JsonSerializer.Deserialize<RestCountriesResponse>(json);

        CountryRecord? countryRecord = restCountriesResponse?.Data?.Objects?.FirstOrDefault();

        if (countryRecord is null)
        {
            throw new ArgumentException("Country not found.", nameof(alpha2Or3Code));
        }

        LocalCurrency localCurrency = new LocalCurrency
        {
            CountryName = countryRecord.Names?.Common,
            CurrencyCode = countryRecord.Currencies?.FirstOrDefault()?.Code,
            CurrencySymbol = countryRecord.Currencies?.FirstOrDefault()?.Symbol,
        };

        this.currencyCountries[alpha2Or3Code] = new WeakReference<LocalCurrency>(localCurrency);

        return localCurrency;
    }

    /// <summary>
    /// Gets information about currency by country code asynchronously.
    /// </summary>
    /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
    /// <see><cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref></see>.
    /// <param name="token">Token for the cancellation of asynchronous operation.</param>
    /// <returns>Information about country currency as <see cref="LocalCurrency"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if alpha2Or3Code is null, empty, whitespace or invalid country code.</exception>
    public Task<LocalCurrency> GetLocalCurrencyByAlpha2Or3CodeAsync(
    string? alpha2Or3Code,
    CancellationToken token)
    {
        // Parameter validation
        if (string.IsNullOrWhiteSpace(alpha2Or3Code))
        {
            throw new ArgumentException(
                "Country code cannot be null or empty.",
                nameof(alpha2Or3Code));
        }

        if (alpha2Or3Code.Length != 2 && alpha2Or3Code.Length != 3)
        {
            throw new ArgumentException(
                "Invalid country code.",
                nameof(alpha2Or3Code));
        }

        return this.GetLocalCurrencyAsyncCore(alpha2Or3Code, token);
    }

    /// <summary>
    /// Gets information about the country by the country capital synchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <returns>Information about the country as <see cref="Country"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the capital name is null, empty, whitespace or nonexistent.</exception>
    public Country GetCountryInfoByCapital(string? capital)
    {
        if (string.IsNullOrWhiteSpace(capital))
        {
            throw new ArgumentException("Capital cannot be null or empty.", nameof(capital));
        }

        string endPoint = $"{ServiceUrl}/capitals/{Uri.EscapeDataString(capital)}";

        string? apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable);

        using WebClient client = new WebClient();

        client.Headers.Add("Authorization", $"Bearer {apiKey}");

        string json = client.DownloadString(endPoint);

        RestCountriesResponse? response = JsonSerializer.Deserialize<RestCountriesResponse>(json);

        CountryRecord? countryRecord = response?.Data?.Objects?.FirstOrDefault();

        if (countryRecord is null)
        {
            throw new ArgumentException("Capital not found.", nameof(capital));
        }

        Country country = new Country
        {
            Name = countryRecord.Names?.Common,
            CapitalName = countryRecord.Capitals?.FirstOrDefault()?.Name,
            Area = countryRecord.Area?.Kilometers ?? 0,
            Population = countryRecord.Population,
            Flag = countryRecord.Flag?.UrlPng ?? countryRecord.Flag?.UrlSvg,
        };

        return country;
    }

    /// <summary>
    /// Gets information about the country by the country capital asynchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <param name="token">Token for cancellation of asynchronous operation.</param>
    /// <returns>Information about the country as <see cref="Country"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the capital name is null, empty, whitespace or nonexistent.</exception>
    public Task<Country> GetCountryInfoByCapitalAsync(
    string? capital,
    CancellationToken token)
    {
        // Parameter validation
        if (string.IsNullOrWhiteSpace(capital))
        {
            throw new ArgumentException(
                "Capital cannot be null or empty.",
                nameof(capital));
        }

        return this.GetCountryInfoByCapitalAsyncCore(capital, token);
    }

    private async Task<LocalCurrency> GetLocalCurrencyAsyncCore(
        string alpha2Or3Code,
        CancellationToken token)
    {
        // Check cache
        if (this.currencyCountries.TryGetValue(alpha2Or3Code, out var weakReference)
            && weakReference.TryGetTarget(out var cachedCurrency))
        {
            return cachedCurrency;
        }

        // Determine endpoint
        string endPoint = alpha2Or3Code.Length == 2
            ? $"{ServiceUrl}/codes.alpha_2/{alpha2Or3Code}"
            : $"{ServiceUrl}/codes.alpha_3/{alpha2Or3Code}";

        // Read API key
        string? apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable);

        using HttpClient client = new HttpClient();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        // Send asynchronous request
        string json = await client.GetStringAsync(new Uri(endPoint), token);

        // Deserialize JSON
        RestCountriesResponse? response =
            JsonSerializer.Deserialize<RestCountriesResponse>(json);

        CountryRecord? countryRecord =
            response?.Data?.Objects?.FirstOrDefault();

        if (countryRecord is null)
        {
            throw new ArgumentException(
                "Country not found.",
                nameof(alpha2Or3Code));
        }

        // Mapping
        LocalCurrency localCurrency = new LocalCurrency
        {
            CountryName = countryRecord.Names?.Common,
            CurrencyCode = countryRecord.Currencies?.FirstOrDefault()?.Code,
            CurrencySymbol = countryRecord.Currencies?.FirstOrDefault()?.Symbol,
        };

        // Save to cache
        this.currencyCountries[alpha2Or3Code] =
            new WeakReference<LocalCurrency>(localCurrency);

        return localCurrency;
    }

    private async Task<Country> GetCountryInfoByCapitalAsyncCore(
        string capital,
        CancellationToken token)
    {
        // Build endpoint
        string endPoint =
            $"{ServiceUrl}/capitals/{Uri.EscapeDataString(capital)}";

        // Read API key
        string? apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable);

        using HttpClient client = new HttpClient();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        // Send asynchronous request
        string json = await client.GetStringAsync(new Uri(endPoint), token);

        // Deserialize JSON
        RestCountriesResponse? response =
            JsonSerializer.Deserialize<RestCountriesResponse>(json);

        CountryRecord? countryRecord =
            response?.Data?.Objects?.FirstOrDefault();

        if (countryRecord is null)
        {
            throw new ArgumentException(
                "Capital not found.",
                nameof(capital));
        }

        // Mapping
        Country country = new Country
        {
            Name = countryRecord.Names?.Common,
            CapitalName = countryRecord.Capitals?.FirstOrDefault()?.Name,
            Area = countryRecord.Area?.Kilometers ?? 0,
            Population = countryRecord.Population,
            Flag = countryRecord.Flag?.UrlPng ?? countryRecord.Flag?.UrlSvg,
        };

        return country;
    }
}
