using DailyPoetryM.Models;
using System.Linq.Expressions;
using System.Text.Json;

namespace DailyPoetryM.Services;

public class TodayPoetryService : ITodayPoetryService
{
    private readonly IAlertService alertService;
    private readonly IPoetryStorage poetryStorage;

    public TodayPoetryService(IAlertService alertService, IPoetryStorage poetryStorage)
    {
        this.alertService = alertService;
        this.poetryStorage = poetryStorage;
    }
    public async Task<TodayPoetry> GetTodayPoetryAsync()
    {
        string token = await GetTokenAsync();
        if(string.IsNullOrWhiteSpace(token))
        {
            return await GetRandomPoetryAsync();
        }
        throw new NotImplementedException();
    }

    public async Task<string> GetTokenAsync()
    {
        using var httpClient = new HttpClient();
        HttpResponseMessage response;
        try
        {
            response = await httpClient.GetAsync("https://v2.jinrishici.com/token");
            response.EnsureSuccessStatusCode();
        }catch (Exception e)
        {
            alertService.Alert("Error", e.Message, "OK");
            return string.Empty;
        }

        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<JinrishiciToken>(json);
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
