# Country Service. Async I/O bound operations

An advanced level task for practicing asynchronous programming for I/O-bound work with the [REST Countries API v5](https://restcountries.com/docs).

Estimated time: **4h**. Requirements: .NET 10 SDK.

## Start here

1. Get a free API key at [restcountries.com/sign-up](https://restcountries.com/sign-up) and save it (see [Personal API key](#personal-api-key-required-for-tests-in-this-repo)).
2. Open **`CountryServices/CountryService.cs`** — implement the four methods (stubs throw `NotImplementedException`).
3. Run `dotnet test` from the repository root until all tests pass.
4. Submit your solution according to your course workflow.

You mainly change **one file**: `CountryService.cs`. Do not rewrite the provided models, interface, or tests.

## Learning goals

- Compare a **synchronous** HTTP call with an **asynchronous** `async`/`await` call for the same operation.
- Call a real authenticated HTTP API, map JSON to domain types, and validate input.
- Read base URL and API key from environment variables on every request (grading may inject a mock base URL).

## What you implement

| Already provided (do not rewrite) | You implement |
|---|---|
| `ICountryService`, `Country`, `LocalCurrency` | HTTP + JSON mapping in `CountryService.cs` |
| `RestCountriesApiModels.cs` (v5 JSON types) | Sync methods with **`WebClient`** |
| Constants, `ServiceUrl`, `currencyCountries` | Async methods with **`HttpClient`** + `async`/`await` |
| Tests in `CountryServices.Tests` | `JsonSerializer`, input validation, currency cache |

**Four methods:**

| Method | Client | Returns |
|---|---|---|
| `GetLocalCurrencyByAlpha2Or3Code` | `WebClient` | `LocalCurrency` |
| `GetLocalCurrencyByAlpha2Or3CodeAsync` | `HttpClient` | `Task<LocalCurrency>` |
| `GetCountryInfoByCapital` | `WebClient` | `Country` |
| `GetCountryInfoByCapitalAsync` | `HttpClient` | `Task<Country>` |

Pass `CancellationToken` into the async HTTP calls. Invalid input (null, empty, whitespace, unknown code/capital) → **`ArgumentException`**.

### Sync vs async clients

`WebClient` is **required for sync methods** as a teaching contrast (blocking I/O).  
`HttpClient` is **required for async methods** — the approach you should prefer in new code. Implement both so you can compare them.

### Currency cache

The skeleton already has:

```csharp
private readonly Dictionary<string, WeakReference<LocalCurrency>> currencyCountries = [];
```

In **both currency methods**:

1. If a live `LocalCurrency` is already in the dictionary, return it (skip the HTTP call).
2. After a successful API call, store the result as `WeakReference<LocalCurrency>`.

Automated checks typically verify that a repeated lookup reuses the same instance while the reference is alive. You do **not** need to cache country-by-capital results.

`WeakReference` avoids keeping objects forever if nothing else uses them. In production you would more often use `IMemoryCache` with size/expiration; here use the provided field.

## REST Countries API v5

Default base URL: `https://api.restcountries.com/countries/v5`  
(override with `RESTCOUNTRIES_API_BASE_URL` when set).

| Need | Request |
|---|---|
| Currency by ISO code | `GET {baseUrl}/codes.alpha_2/{code}` if the code length is **2**; otherwise `GET {baseUrl}/codes.alpha_3/{code}` |
| Country by capital | `GET {baseUrl}/capitals/{capital}` |
| Auth | Header `Authorization: Bearer {api_key}` on every request |

Deserialize with types from **`RestCountriesApiModels.cs`**, then map into `LocalCurrency` / `Country`. Older docs may show `/alpha/` — do **not** use those paths in this assignment.

## Configuration

Read on **every request** (do not cache in `static readonly` / static constructor):

| Variable | Role |
|---|---|
| `RESTCOUNTRIES_API_KEY` | Bearer token (required for real API calls) |
| `RESTCOUNTRIES_API_BASE_URL` | Optional base URL override |

Do **not** hardcode the key or URL. Skeleton constants are variable **names**. Use the provided `ServiceUrl` for the base URL; read the key with `Environment.GetEnvironmentVariable` each time.

If grading sets `RESTCOUNTRIES_API_BASE_URL` to `http://localhost:…` and changes the port between fixtures, a cached URL causes connection errors.

## Personal API key (required for tests in this repo)

Local `dotnet test` in **this** repository calls the **real** API — a personal key is **mandatory** for that practice step. Platform grading uses a **separate** test suite with a mock and does **not** need your key.

```powershell
dotnet user-secrets set RESTCOUNTRIES_API_KEY "your_api_key_here" --project CountryServices.Tests
dotnet test
```

Alternatives: Windows user/system env var `RESTCOUNTRIES_API_KEY` (restart IDE after), or for one PowerShell session:

```powershell
$env:RESTCOUNTRIES_API_KEY = "your_api_key_here"
dotnet test
```

Without a key, tests fail with setup instructions until the key is configured.

## How verification works

| | **This repository** | **Course platform grading** |
|---|---|---|
| What runs | Your library + tests shipped here | Your library + instructor tests (you do not edit those) |
| API | Real REST Countries v5 | Usually a mock on `localhost` |
| Your API key | Required | Not required |

Implement the contract correctly so **both** real-API tests here and mock-based grading can pass.

## Running tests

```powershell
dotnet test
```

Or run `CountryServices.Tests` from Visual Studio / Rider. Tests call the API sequentially to respect the free-tier limit.

## Learning materials

- [Async/await in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/)
- [WebClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.webclient) — sync (legacy; comparison only)
- [HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient) — preferred HTTP client
- [`WeakReference<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.weakreference-1)
- [JsonSerializer](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializer)
- [REST Countries API docs](https://restcountries.com/docs) — **v5** endpoints above
- [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
