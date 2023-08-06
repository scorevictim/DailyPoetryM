using DailyPoetryM.Services;
using DailyPoetryM.ViewModels;

namespace DailyPoetryM;

public class ServiceLocator
{
    private IServiceProvider serviceProvider;
    public ResultPageViewModel ResultPageViewModel => serviceProvider.GetService<ResultPageViewModel>();
    public TodayPageViewModel TodayPageViewModel => serviceProvider.GetService<TodayPageViewModel>();

    public ServiceLocator()
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
        serviceCollection.AddSingleton<IPreferenceStorage, PreferenceStorage>();

        serviceCollection.AddSingleton<ResultPageViewModel>();
        serviceCollection.AddSingleton<TodayPageViewModel>();

        serviceCollection.AddSingleton<ITodayPoetryService, TodayPoetryService>();
        serviceCollection.AddSingleton<IAlertService, AlertService>();
        serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
