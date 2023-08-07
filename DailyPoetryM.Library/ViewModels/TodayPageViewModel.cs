using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

public class TodayPageViewModel : ObservableObject
{
    private readonly ITodayPoetryService todayPoetryService;
    private readonly IContentNavigationService contentNavigationService;

    public TodayPageViewModel(ITodayPoetryService todayPoetryService, IContentNavigationService contentNavigationService)
    {
        this.todayPoetryService = todayPoetryService;
        this.contentNavigationService = contentNavigationService;
        this.lazyLoadedCommand = new Lazy<AsyncRelayCommand>(() => new AsyncRelayCommand(LoadedCommandFunction));
        this.lazyShowDetailCommand = new Lazy<AsyncRelayCommand>(() => new AsyncRelayCommand(ShowDetailCommandFunction));
    }
    private readonly Lazy<AsyncRelayCommand> lazyLoadedCommand;
    public AsyncRelayCommand LoadedCommand => lazyLoadedCommand.Value;
    public async Task LoadedCommandFunction()
    {
        IsLoading = true;
        TodayPoetry = await todayPoetryService.GetTodayPoetryAsync();
        IsLoading = false;
    }

    private bool isLoading;
    public bool IsLoading
    {
        get { return isLoading; }
        set => SetProperty(ref isLoading, value);
    }

    private TodayPoetry? todayPoetry;
    public TodayPoetry? TodayPoetry
    {
        get => todayPoetry;
        set => SetProperty(ref todayPoetry, value);
    }

    public AsyncRelayCommand ShowDetailCommand => lazyShowDetailCommand.Value;
    private readonly Lazy<AsyncRelayCommand> lazyShowDetailCommand;
    public async Task ShowDetailCommandFunction()
    {
        await contentNavigationService.NavigateToAsync(ContentNavigationConstant.TodayDetailPage);
    }
}
