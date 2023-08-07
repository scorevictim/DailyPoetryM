using DailyPoetryM.Services;
using DailyPoetryM.ViewModels;

namespace DailyPoetryM;

public class ServiceLocator
{
    private IServiceProvider serviceProvider;
    public ResultPageViewModel ResultPageViewModel => serviceProvider.GetService<ResultPageViewModel>();
    public TodayPageViewModel TodayPageViewModel => serviceProvider.GetService<TodayPageViewModel>();
    public IRouteService RouteService => serviceProvider.GetRequiredService<IRouteService>();

    public ServiceLocator()
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
        serviceCollection.AddSingleton<IPreferenceStorage, PreferenceStorage>();

        serviceCollection.AddSingleton<ResultPageViewModel>();
        serviceCollection.AddSingleton<TodayPageViewModel>();

        serviceCollection.AddSingleton<ITodayPoetryService, TodayPoetryService>();
        serviceCollection.AddSingleton<IAlertService, AlertService>();
        serviceCollection.AddSingleton<IRouteService, RouteService>();
        serviceCollection.AddSingleton<IContentNavigationService, ContentNavigationService>();
        serviceCollection.AddSingleton<IRootNavigationService, RootNavigationService>();

        serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
