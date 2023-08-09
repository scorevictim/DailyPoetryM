using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using System.Linq.Expressions;
using TheSalLab.MauiInfiniteScrolling;

namespace DailyPoetryM.ViewModels;

public class ResultPageViewModel : ObservableObject
{
    private Expression<Func<Poetry, bool>> where;
    public Expression<Func<Poetry, bool>> Where
    {
        get => where;
        set
        {
            //if(value != where)
            //{
            //    canLoadMore = true;
            //}
            canLoadMore = SetProperty(ref where, value);
        }
    }
    private string status;
    public string Status
    {
        get => status;
        set => SetProperty(ref status, value);
    }
    public MauiInfiniteScrollCollection<Poetry> Poetries { get; }

    public ResultPageViewModel(IPoetryStorage poetryStorage, IContentNavigationService contentNavigationService)
    {
        Where = Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
            Expression.Parameter(typeof(Poetry), "p"));

        Poetries = new()
        {
            OnCanLoadMore = () => canLoadMore,
            OnLoadMore = async () =>
            {
                Status = Loading;
                var poetries = (await poetryStorage.GetPoetriesAsync(Where,
                    Poetries.Count, PageSize)).ToList();
                Status = string.Empty;

                if (poetries.Count < PageSize)
                {
                    // NoMoreResult
                    canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (poetries.Count == 0 && Poetries.Count == 0)
                {
                    // NoResult
                    canLoadMore = false;
                    Status = NoResult;
                }

                return poetries;
            }
        };
        this.poetryStorage = poetryStorage;
        this.contentNavigationService = contentNavigationService;
    }
    private RelayCommand navigatedToCommand;
    public RelayCommand NavigatedToCommand => navigatedToCommand ??= new(async () =>
    {
        await NavigatedToCommandFunctionAsync();
    });
    public async Task NavigatedToCommandFunctionAsync()
    {
        if(poetryStorage.IsInitialized is false)
        {
            await poetryStorage.InitializeAsync();
        }
        Poetries.Clear();
        await Poetries.LoadMoreAsync();
    }
    private bool canLoadMore;
    public const string Loading = "Loading";
    public const string NoResult = "No Result";
    public const string NoMoreResult = "End";
    public const int PageSize = 20;
    private readonly IPoetryStorage poetryStorage;
    private readonly IContentNavigationService contentNavigationService;
    private RelayCommand testCommand;
    public RelayCommand TestCommand => testCommand ??= new(async () =>
    {
        await contentNavigationService.NavigateToAsync(ContentNavigationConstant.DetailPage, Poetries[0]);
    });
    private RelayCommand<Poetry> poetryTappedCommand;
    public RelayCommand<Poetry> PoetryTappedCommand => poetryTappedCommand ??= new(async (poetry) =>
    {
        await contentNavigationService.NavigateToAsync(ContentNavigationConstant.DetailPage, poetry);
    });
}
