using DailyPoetryM.Pages;
using DailyPoetryM.Services;

namespace DailyPoetryM;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        var serviceLocatorName = nameof(ServiceLocator);
        var serviceLocator = (ServiceLocator)Application.Current.Resources.MergedDictionaries.First(p => p.ContainsKey(serviceLocatorName))[serviceLocatorName];
        var routeService = serviceLocator.RouteService;
        Items.Add(new FlyoutItem
        {
            Title = nameof(TodayPage),
            Route = routeService.GetRoute(RootNavigationConstant.TodayPage),
            Items =
            {
                new ShellContent
                {
                    ContentTemplate = new(typeof(TodayPage))
                }
            }
        });
        Routing.RegisterRoute(routeService.GetRoute(nameof(RootNavigationConstant.TodayPage)), typeof(TodayDetailPage));
        Items.Add(new FlyoutItem
        {
            Title = nameof(ResultPage),
            Route = routeService.GetRoute(nameof(ContentNavigationConstant.ResultPage)),
            Items =
            {
                new ShellContent
                {
                    ContentTemplate = new(typeof(ResultPage))
                }
            }
        });
        Routing.RegisterRoute(routeService.GetRoute(ContentNavigationConstant.DetailPage), typeof(DetailPage));
    }
}
