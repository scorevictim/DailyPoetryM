namespace DailyPoetryM.Services;

public interface IRootNavigationService
{
    Task NavigateToAsync(string pageKey);
    Task NavigateToAsync(string pageKey, object parameter);
}
