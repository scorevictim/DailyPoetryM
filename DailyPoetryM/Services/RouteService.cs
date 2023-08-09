namespace DailyPoetryM.Services;

public class RouteService : IRouteService
{
    private readonly Dictionary<string, string> routeDictionary = new()
    {
        [RootNavigationConstant.TodayPage] = RootNavigationConstant.TodayPage,
        [RootNavigationConstant.QueryPage] = RootNavigationConstant.QueryPage,
        [RootNavigationConstant.FavoritePage] = RootNavigationConstant.FavoritePage,
        [ContentNavigationConstant.TodayDetailPage] = $"{RootNavigationConstant.TodayPage}/{ContentNavigationConstant.TodayDetailPage}",
        [ContentNavigationConstant.ResultPage] = ContentNavigationConstant.ResultPage,
        [ContentNavigationConstant.DetailPage] = $"{ContentNavigationConstant.ResultPage}/{ContentNavigationConstant.DetailPage}",
    };
    public string GetRoute(string pageKey)
    {
        return routeDictionary[pageKey];
    }
}
