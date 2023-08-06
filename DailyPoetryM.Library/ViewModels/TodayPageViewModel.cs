using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

public class TodayPageViewModel : ObservableObject
{
    private readonly ITodayPoetryService todayPoetryService;

    public TodayPageViewModel(ITodayPoetryService todayPoetryService)
    {
        this.todayPoetryService = todayPoetryService;
        this.lazyLoadedCommand = new Lazy<AsyncRelayCommand>(new AsyncRelayCommand(LoadedCommandFunction));
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
}
