using DailyPoetryM.Models;
using System.Globalization;
using System.Text.Json;

namespace DailyPoetryM.Services;

public class BingImageService : ITodayImageService
{
    private readonly ITodayImageStorage todayImageStorage;

    private readonly IAlertService alertService;

    private const string Server = "必应每日图片服务器";

    public BingImageService(ITodayImageStorage todayImageStorage,
        IAlertService alertService)
    {
        this.todayImageStorage = todayImageStorage;
        this.alertService = alertService;
    }

    public async Task<TodayImage> GetTodayImageAsync() =>
        await todayImageStorage.GetTodayImageAsync(true);

    public async Task<TodayImageServiceCheckUpdateResult> CheckUpdateAsync()
    {
        var todayImage = await todayImageStorage.GetTodayImageAsync(false);
        if (todayImage.ExpiresAt > DateTime.Now)
        {
            return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
        }

        using var httpClient = new HttpClient();
        HttpResponseMessage response;
        try
        {
            response = await httpClient.GetAsync(
                "https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=zh-CN");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            alertService.Alert("Error", e.Message, "OK");
            return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
        }

        var json = await response.Content.ReadAsStringAsync();
        string bingImageUrl;
        try
        {
            var bingImage = JsonSerializer.Deserialize<BingImageOfTheDay>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })
                ?.Images?.FirstOrDefault() ?? throw new JsonException();

            var bingImageFullStartDate = DateTime.ParseExact(
                bingImage.FullStartDate ?? throw new JsonException(),
                "yyyyMMddHHmm", CultureInfo.InvariantCulture);
            var todayImageFullStartDate = DateTime.ParseExact(
                todayImage.FullStartDate, "yyyyMMddHHmm",
                CultureInfo.InvariantCulture);

            if (bingImageFullStartDate <= todayImageFullStartDate)
            {
                todayImage.ExpiresAt = DateTime.Now.AddHours(2);
                await todayImageStorage.SaveTodayImageAsync(todayImage, true);
                return new TodayImageServiceCheckUpdateResult
                {
                    HasUpdate = false
                };
            }

            todayImage = new TodayImage
            {
                FullStartDate = bingImage.FullStartDate,
                ExpiresAt = bingImageFullStartDate.AddDays(1),
                Copyright = bingImage.Copyright ?? throw new JsonException(),
                CopyrightLink = bingImage.CopyrightLink ??
                    throw new JsonException()
            };

            bingImageUrl = bingImage.Url ?? throw new JsonException();
        }
        catch (Exception e)
        {
            alertService.Alert("Error", e.Message, "OK");
            return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
        }

        try
        {
            response =
                await httpClient.GetAsync("https://www.bing.com" +
                    bingImageUrl);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            alertService.Alert("Error", e.Message, "OK");
            return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
        }

        todayImage.ImageBytes = await response.Content.ReadAsByteArrayAsync();
        await todayImageStorage.SaveTodayImageAsync(todayImage, false);
        return new TodayImageServiceCheckUpdateResult
        {
            HasUpdate = true,
            TodayImage = todayImage
        };
    }
}

public class BingImageOfTheDay
{
    public List<BingImageOfTheDayImage>? Images { get; set; }
}

public class BingImageOfTheDayImage
{
    public string? StartDate { get; set; }

    public string? FullStartDate { get; set; }

    public string? EndDate { get; set; }

    public string? Url { get; set; }

    public string? Copyright { get; set; }

    public string? CopyrightLink { get; set; }
}
