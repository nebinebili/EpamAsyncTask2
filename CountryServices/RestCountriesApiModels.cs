using System.Text.Json.Serialization;

namespace CountryServices;

/// <summary>
/// Root JSON envelope returned by the REST Countries API v5.
/// </summary>
internal sealed record RestCountriesResponse
{
    [JsonPropertyName("data")]
    public RestCountriesData? Data { get; init; }
}

/// <summary>
/// Container for country objects in an API v5 response.
/// </summary>
internal sealed record RestCountriesData
{
    [JsonPropertyName("objects")]
    public CountryRecord[]? Objects { get; init; }
}

/// <summary>
/// Country payload used for currency and capital lookups.
/// </summary>
internal sealed record CountryRecord
{
    [JsonPropertyName("names")]
    public NamesRecord? Names { get; init; }

    [JsonPropertyName("currencies")]
    public CurrencyRecord[]? Currencies { get; init; }

    [JsonPropertyName("capitals")]
    public CapitalRecord[]? Capitals { get; init; }

    [JsonPropertyName("area")]
    public AreaRecord? Area { get; init; }

    [JsonPropertyName("population")]
    public int Population { get; init; }

    [JsonPropertyName("flag")]
    public FlagRecord? Flag { get; init; }
}

/// <summary>
/// Country naming fields from an API v5 response.
/// </summary>
internal sealed record NamesRecord
{
    [JsonPropertyName("common")]
    public string? Common { get; init; }
}

/// <summary>
/// Currency fields from an API v5 response.
/// </summary>
internal sealed record CurrencyRecord
{
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }
}

/// <summary>
/// Capital fields from an API v5 response.
/// </summary>
internal sealed record CapitalRecord
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

/// <summary>
/// Area fields from an API v5 response.
/// </summary>
internal sealed record AreaRecord
{
    [JsonPropertyName("kilometers")]
    public double Kilometers { get; init; }
}

/// <summary>
/// Flag URL fields from an API v5 response.
/// </summary>
internal sealed record FlagRecord
{
    [JsonPropertyName("url_png")]
    public string? UrlPng { get; init; }

    [JsonPropertyName("url_svg")]
    public string? UrlSvg { get; init; }
}
