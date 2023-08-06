using DailyPoetryM.Services;

namespace DailyPoetryM.UnitTest.Services;

public class TodayPoetryServiceTest
{
    [Fact]
    public async Task GetTodayPoetryAsync_Default()
    {
        TodayPoetryService todayPoetryService = new(null, null);
        var result = await todayPoetryService.GetTodayPoetryAsync();
        Assert.NotNull(result);
    }
}
