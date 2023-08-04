﻿using DailyPoetryM.Models;
using System.Text.Json;

namespace DailyPoetryM.Services;

public class TodayPoetryService : ITodayPoetryService
{
    public Task<TodayPoetry> GetTodayPoetryAsync()
    {
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
        }catch (Exception )
        {
            return string.Empty;
        }

        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<JinrishiciToken>(json);
        return token.data;
    }
}

public class JinrishiciToken
{
    public string status { get; set; }
    public string data { get; set; }
}
