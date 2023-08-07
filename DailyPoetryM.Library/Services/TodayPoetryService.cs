using DailyPoetryM.Models;
using System.Linq.Expressions;
using System.Text.Json;

namespace DailyPoetryM.Services;

public class TodayPoetryService : ITodayPoetryService
{
    private readonly IAlertService alertService;
    private readonly IPoetryStorage poetryStorage;
    private readonly IPreferenceStorage preferenceStorage;

    public TodayPoetryService(IAlertService alertService, IPoetryStorage poetryStorage, IPreferenceStorage preferenceStorage)
    {
        this.alertService = alertService;
        this.poetryStorage = poetryStorage;
        this.preferenceStorage = preferenceStorage;
    }
    public async Task<TodayPoetry> GetTodayPoetryAsync()
    {
        string token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            return await GetRandomPoetryAsync();
        }

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-User-Token", token);
        HttpResponseMessage response;
        try
        {
            response = await httpClient.GetAsync("https://v2.jinrishici.com/sentence");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            alertService.Alert("Error", e.Message, "OK");
            return await GetRandomPoetryAsync();
        }

        var json = await response.Content.ReadAsStringAsync();
        JinrishiciSentence jinrishiciSentence;
        try
        {
            jinrishiciSentence = JsonSerializer.Deserialize<JinrishiciSentence>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new JsonException();
        }
        catch (Exception e)
        {
            alertService.Alert("Error", e.Message, "OK");
            return await GetRandomPoetryAsync();
        }
        return new()
        {
            Snippet = jinrishiciSentence.Data?.Content ?? throw new JsonException(),
            Name = jinrishiciSentence.Data.Origin?.Title ?? throw new JsonException(),
            Dynasty = jinrishiciSentence.Data.Origin.Dynasty ?? throw new JsonException(),
            Author = jinrishiciSentence.Data.Origin.Author ?? throw new JsonException(),
            Content = string.Join("\n", jinrishiciSentence.Data.Origin.Content ?? throw new JsonException()),
            Source = TodayPoetrySources.Jinrishici
        };
    }
    private const string TokenKey = nameof(TodayPoetryService) + ".Token";
    public async Task<string> GetTokenAsync()
    {
        var localToken = preferenceStorage.Get(TokenKey, string.Empty);
        if(string.IsNullOrWhiteSpace(localToken) is false)
        {
            return localToken;
        }
        using var httpClient = new HttpClient();
        HttpResponseMessage response;
        try
        {
            response = await httpClient.GetAsync("https://v2.jinrishici.com/token");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            alertService.Alert("Error", e.Message, "OK");
            return string.Empty;
        }

        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<JinrishiciToken>(json);
        preferenceStorage.Set(TokenKey, token.data);
        return token.data;
    }
    public async Task<TodayPoetry> GetRandomPoetryAsync()
    {
        var poetries = await poetryStorage.GetPoetriesAsync(
            Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
            Expression.Parameter(typeof(Poetry), "p")), new Random().Next(30), 1);
        var poetry = poetries.First();
        return new()
        {
            Snippet = poetry.Snippet,
            Name = poetry.Name,
            Dynasty = poetry.Dynasty,
            Author = poetry.Author,
            Content = poetry.Content,
            Source = TodayPoetrySources.Local
        };
    }
}

public class JinrishiciToken
{
    public string status { get; set; }
    public string data { get; set; }
}

public class JinrishiciOrigin
{
    public string? Title { get; set; } = string.Empty;
    public string? Dynasty { get; set; } = string.Empty;
    public string? Author { get; set; } = string.Empty;
    public List<string> Content { get; set; } = new();
    public List<string> Translate { get; set; } = new();
}

public class JinrishiciData
{
    public string? Content { get; set; } = string.Empty;
    public JinrishiciOrigin? Origin { get; set; } = new();
}

public class JinrishiciSentence
{
    public JinrishiciData? Data { get; set; } = new();
}