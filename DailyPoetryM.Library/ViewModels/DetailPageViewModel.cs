using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

public partial class DetailPageViewModel : ObservableObject
{
    [ObservableProperty]
    private Poetry poetry;
    [ObservableProperty]
    private Favorite favorite;
    private Lazy<AsyncRelayCommand> lazyNavigatedToCommand;
    private Lazy<AsyncRelayCommand> lazyFavoriteToggledCommand;
    private readonly IFavoriteStorage favoriteStorage;
    private bool isFavorite;
    public AsyncRelayCommand NavigatedToCommand => lazyNavigatedToCommand.Value;
    public AsyncRelayCommand FavoriteToggledCommand => lazyFavoriteToggledCommand.Value;
    public DetailPageViewModel(IFavoriteStorage favoriteStorage)
    {
        lazyNavigatedToCommand = new(new AsyncRelayCommand(NavigatedToCommandFunction));
        lazyFavoriteToggledCommand = new(new AsyncRelayCommand(FavoriteToggledCommandFunction));
        this.favoriteStorage = favoriteStorage;
    }
    public async Task NavigatedToCommandFunction()
    {
        var favorite = await favoriteStorage.GetFavoriteAsync(Poetry.Id) ?? new() { PoetryId = Poetry.Id };
        isFavorite = favorite.IsFavorite;
        Favorite = favorite;
    }
    public async Task FavoriteToggledCommandFunction()
    {
        if (isFavorite == Favorite.IsFavorite)
        {
            return;
        }
         await favoriteStorage.SaveFavoriteAsync(Favorite);
    }
}
