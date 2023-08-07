namespace DailyPoetryM.Services;

public class ContentNavigationService : IContentNavigationService
{
    private readonly IRouteService routeService;

    public ContentNavigationService(IRouteService routeService)
    {
        this.routeService = routeService;
    }
    public async Task NavigateToAsync(string pageKey)
    {
        await Shell.Current.GoToAsync(routeService.GetRoute(pageKey));
    }

    public async Task NavigateToAsync(string pageKey, object parameter)
    {
        await Shell.Current.GoToAsync($"{routeService.GetRoute(pageKey)}", new Dictionary<string, object> { ["parameter"] = parameter });
    }
}
