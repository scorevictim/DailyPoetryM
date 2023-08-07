namespace DailyPoetryM.Services;

public class RootNavigationService : IRootNavigationService
{
    private readonly IRouteService routeService;

    public RootNavigationService(IRouteService routeService)
    {
        this.routeService = routeService;
    }
    public async Task NavigateToAsync(string pageKey)
    {
        await Shell.Current.GoToAsync($"//{routeService.GetRoute(pageKey)}");
        throw new NotImplementedException();
    }

    public Task NavigateToAsync(string pageKey, object parameter)
    {
        throw new NotImplementedException();
    }
}
