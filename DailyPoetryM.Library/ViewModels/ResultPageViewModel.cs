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

    public ResultPageViewModel(IPoetryStorage poetryStorage)
    {
        Where = Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
            Expression.Parameter(typeof(Poetry), "p"));
        await poetryStorage.InitializeAsync();
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
    }
    private RelayCommand navigatedToCommand;
    public RelayCommand NavigatedToCommand => navigatedToCommand ??= new(async () =>
    {
        await NavigatedToCommandFunctionAsync();
    });
    public async Task NavigatedToCommandFunctionAsync()
    {
        Poetries.Clear();
        await Poetries.LoadMoreAsync();
    }
    private bool canLoadMore;
    public const string Loading = "Loading";
    public const string NoResult = "No Result";
    public const string NoMoreResult = "End";
    public const int PageSize = 20;
}
