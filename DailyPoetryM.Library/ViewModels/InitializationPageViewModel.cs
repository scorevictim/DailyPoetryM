using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

public class InitializationPageViewModel : ObservableObject
{
    private IPoetryStorage poetryStorage;

    private IFavoriteStorage favoriteStorage;

    private IInitializationNavigationService initializationNavigationService;

    public InitializationPageViewModel(IPoetryStorage poetryStorage,
        IFavoriteStorage favoriteStorage,
        IInitializationNavigationService initializationNavigationService)
    {
        this.poetryStorage = poetryStorage;
        this.favoriteStorage = favoriteStorage;
        this.initializationNavigationService = initializationNavigationService;
        lazyLoadedCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(LoadedCommandFunction));
    }

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private string _status = string.Empty;

    public AsyncRelayCommand LoadedCommand => lazyLoadedCommand.Value;

    private Lazy<AsyncRelayCommand> lazyLoadedCommand;

    public async Task LoadedCommandFunction()
    {
        if (!poetryStorage.IsInitialized)
        {
            Status = "正在初始化诗词数据库";
            await poetryStorage.InitializeAsync();
        }

        if (!favoriteStorage.IsInitialized)
        {
            Status = "正在初始化收藏数据库";
            await favoriteStorage.InitializeAsync();
        }

        Status = "所有初始化已完成";
        await Task.Delay(1000);

        initializationNavigationService.NavigateToAppShell();
    }
}