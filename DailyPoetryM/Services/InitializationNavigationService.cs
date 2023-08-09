using DailyPoetryM.Pages;

namespace DailyPoetryM.Services;

public class InitializationNavigationService : IInitializationNavigationService
{
    private Lazy<InitializationPage> lazyInitializationPage =
       new(() => new InitializationPage());

    private Lazy<AppShell> lazyAppShell = new(() => new AppShell());

    public void NavigateToInitializationPage() =>
        Application.Current!.MainPage = lazyInitializationPage.Value;

    public void NavigateToAppShell() =>
        Application.Current!.MainPage = lazyAppShell.Value;
}
