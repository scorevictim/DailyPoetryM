namespace DailyPoetryM.Services;

public class RouteService : IRouteService
{
    private readonly Dictionary<string, string> routeDictionary = new()
    {
        [RootNavigationConstant.TodayPage] = RootNavigationConstant.TodayPage,
        [ContentNavigationConstant.TodayDetailPage] = $"{RootNavigationConstant.TodayPage}/{ContentNavigationConstant.TodayDetailPage}"
    };
    public string GetRoute(string pageKey)
    {
        return routeDictionary[pageKey];
    }
}
