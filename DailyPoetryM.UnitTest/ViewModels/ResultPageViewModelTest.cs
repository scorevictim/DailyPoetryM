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

        await resultPageViewModel.NavigatedToCommandFunctionAsync();
        Assert.Equal(ResultPageViewModel.PageSize, resultPageViewModel.Poetries.Count);


        await poetryStorage.CloseAsync();
    }
}
