using DailyPoetryM.Models;
using DailyPoetryM.UnitTest.Services;
using DailyPoetryM.ViewModels;
using System.Linq.Expressions;

namespace DailyPoetryM.UnitTest.ViewModels;

public class ResultPageViewModelTest : IDisposable
{
    public ResultPageViewModelTest() => PoetryStorageTest.RemoveDatabaseFile();

    public void Dispose() => PoetryStorageTest.RemoveDatabaseFile();

    [Fact]
    public async Task Poetries_Default()
    {
        var where = Expression.Lambda<Func<Poetry, bool>>(
            Expression.Constant(true),
            Expression.Parameter(typeof(Poetry), "p"));

        var poetryStorage = await PoetryStorageTest.GetInitializedPoetryStorage();

        ResultPageViewModel resultPageViewModel = new(poetryStorage)
        {
            Where = where
        };

        var statusList = new List<string>();
        resultPageViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ResultPageViewModel.Status))
            {
                statusList.Add(resultPageViewModel.Status);
            }
        };

        await resultPageViewModel.NavigatedToCommandFunctionAsync();
        Assert.Equal(ResultPageViewModel.PageSize, resultPageViewModel.Poetries.Count);
        Assert.Equal(2, statusList.Count);
        Assert.Equal(ResultPageViewModel.Loading, statusList[0]);
        Assert.Equal(string.Empty, statusList[1]);

        await poetryStorage.CloseAsync();
    }
}
