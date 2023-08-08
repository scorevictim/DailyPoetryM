using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

public class TodayPageViewModel : ObservableObject
{
    private readonly ITodayPoetryService todayPoetryService;
    private readonly IContentNavigationService contentNavigationService;
    private readonly ITodayImageService todayImageService;

    public TodayPageViewModel(ITodayPoetryService todayPoetryService, 
                              IContentNavigationService contentNavigationService,
                              ITodayImageService todayImageService)
    {
        this.todayPoetryService = todayPoetryService;
        this.contentNavigationService = contentNavigationService;
        this.todayImageService = todayImageService;
        this.lazyLoadedCommand = new Lazy<RelayCommand>(() => new RelayCommand(LoadedCommandFunction));
        this.lazyShowDetailCommand = new Lazy<AsyncRelayCommand>(() => new AsyncRelayCommand(ShowDetailCommandFunction));
    }
    private readonly Lazy<RelayCommand> lazyLoadedCommand;
    public RelayCommand LoadedCommand => lazyLoadedCommand.Value;
    public void LoadedCommandFunction()
    {
        Task.Run(async () =>
        {

            IsLoading = true;
            TodayPoetry = await todayPoetryService.GetTodayPoetryAsync();
            IsLoading = false;
        });
        Task.Run(async () =>
        {

            TodayImage = await todayImageService.GetTodayImageAsync();
            var checkUpdateResult = await todayImageService.CheckUpdateAsync();
            if (checkUpdateResult.HasUpdate)
            {
                TodayImage = checkUpdateResult.TodayImage;
            }
        });

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
    private TodayImage todayImage;
    public TodayImage TodayImage
    {
        get => todayImage;
        set => SetProperty(ref todayImage, value);
    }

    public AsyncRelayCommand ShowDetailCommand => lazyShowDetailCommand.Value;
    private readonly Lazy<AsyncRelayCommand> lazyShowDetailCommand;
    public async Task ShowDetailCommandFunction()
    {
        await contentNavigationService.NavigateToAsync(ContentNavigationConstant.TodayDetailPage);
    }
}
