using DailyPoetryM.Services;
using DailyPoetryM.ViewModels;

namespace DailyPoetryM;

public class ServiceLocator
{
    private IServiceProvider serviceProvider;
    public ResultPageViewModel ResultPageViewModel => serviceProvider.GetService<ResultPageViewModel>();
    public TodayPageViewModel TodayPageViewModel => serviceProvider.GetService<TodayPageViewModel>();
    public DetailPageViewModelProxy DetailPageViewModelProxy => serviceProvider.GetService<DetailPageViewModelProxy>();
    public QueryPageViewModel QueryPageViewModel => serviceProvider.GetService<QueryPageViewModel>();
    public FavoritePageViewModel FavoritePageViewModel => serviceProvider.GetService<FavoritePageViewModel>();
    public IRouteService RouteService => serviceProvider.GetService<IRouteService>();

    public ServiceLocator()
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
        serviceCollection.AddSingleton<IPreferenceStorage, PreferenceStorage>();

        serviceCollection.AddSingleton<ResultPageViewModel>();
        serviceCollection.AddSingleton<TodayPageViewModel>();
        serviceCollection.AddSingleton<DetailPageViewModelProxy>();
        serviceCollection.AddSingleton<QueryPageViewModel>();
        serviceCollection.AddSingleton<FavoritePageViewModel>();

        serviceCollection.AddSingleton<ITodayPoetryService, TodayPoetryService>();
        serviceCollection.AddSingleton<IAlertService, AlertService>();
        serviceCollection.AddSingleton<IRouteService, RouteService>();
        serviceCollection.AddSingleton<IContentNavigationService, ContentNavigationService>();
        serviceCollection.AddSingleton<IRootNavigationService, RootNavigationService>();
        serviceCollection.AddSingleton<ITodayImageService, BingImageService>();
        serviceCollection.AddSingleton<ITodayImageStorage, TodayImageStorage>();
        serviceCollection.AddSingleton<IFavoriteStorage, FavoriteStorage>();

        serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
