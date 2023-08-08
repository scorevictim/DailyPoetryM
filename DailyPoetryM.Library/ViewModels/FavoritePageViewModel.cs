using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using System.Collections.ObjectModel;

namespace DailyPoetryM.ViewModels;

public class FavoritePageViewModel : ObservableObject
{
    private IFavoriteStorage favoriteStorage;

    private IPoetryStorage poetryStorage;

    IContentNavigationService contentNavigationService;

    public FavoritePageViewModel(IFavoriteStorage favoriteStorage,
        IPoetryStorage poetryStorage,
        IContentNavigationService contentNavigationService)
    {
        this.favoriteStorage = favoriteStorage;
        this.poetryStorage = poetryStorage;
        this.contentNavigationService = contentNavigationService;

        this.favoriteStorage.Updated += FavoriteStorageOnUpdated;

        lazyLoadedCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(LoadedCommandFunction));
        layzPoetryTappedCommand = new Lazy<AsyncRelayCommand<PoetryFavorite>>(
            new AsyncRelayCommand<PoetryFavorite>(PoetryTappedCommandFunction));
    }

    public ObservableCollection<PoetryFavorite> PoetryFavoriteCollection
    {
        get;
    } = new();

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private bool _isLoading;

    public AsyncRelayCommand LoadedCommand => lazyLoadedCommand.Value;

    private Lazy<AsyncRelayCommand> lazyLoadedCommand;

    public async Task LoadedCommandFunction()
    {
        IsLoading = true;

        PoetryFavoriteCollection.Clear();
        var favoriteList = await favoriteStorage.GetFavoritesAsync();

        var list = (await Task.WhenAll(
            favoriteList.Select(p => Task.Run(async () => new PoetryFavorite
            {
                Poetry = await poetryStorage.GetPoetryAsync(p.PoetryId),
                Favorite = p
            })))).ToList();

        foreach (var item in list)
        {
            PoetryFavoriteCollection.Add(item);
        }

        IsLoading = false;
    }

    public AsyncRelayCommand<PoetryFavorite> PoetryTappedCommand =>
        layzPoetryTappedCommand.Value;

    private Lazy<AsyncRelayCommand<PoetryFavorite>> layzPoetryTappedCommand;

    public async Task
        PoetryTappedCommandFunction(PoetryFavorite poetryFavorite) =>
        await contentNavigationService.NavigateToAsync(
            ContentNavigationConstant.FavoriteDetailPage,
            poetryFavorite.Poetry);


    private async void FavoriteStorageOnUpdated(object? sender,
        FavoriteStorageUpdatedEventArgs e)
    {
        var favorite = e.UpdatedFavorite;
        PoetryFavoriteCollection.Remove(
            PoetryFavoriteCollection.FirstOrDefault(p =>
                p.Favorite.PoetryId == favorite.PoetryId));

        if (!favorite.IsFavorite)
        {
            return;
        }

        var poetryFavorite = new PoetryFavorite
        {
            Poetry = await poetryStorage.GetPoetryAsync(favorite.PoetryId),
            Favorite = favorite
        };

        var index = PoetryFavoriteCollection.IndexOf(
            PoetryFavoriteCollection.FirstOrDefault(p =>
                p.Favorite.TimeStamp < favorite.TimeStamp));
        if (index < 0)
        {
            index = PoetryFavoriteCollection.Count;
        }

        PoetryFavoriteCollection.Insert(index, poetryFavorite);
    }
}

public class PoetryFavorite
{
    public Poetry Poetry { get; set; }

    public Favorite Favorite { get; set; }
}
