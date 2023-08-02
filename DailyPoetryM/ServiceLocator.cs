using DailyPoetryM.Services;
using DailyPoetryM.ViewModels;

namespace DailyPoetryM;

public class ServiceLocator
{
    private IServiceProvider serviceProvider;
    public ResultPageViewModel ResultPageViewModel => serviceProvider.GetService<ResultPageViewModel>();

    public ServiceLocator()
    {
        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
        serviceCollection.AddSingleton<ResultPageViewModel>();

        serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
