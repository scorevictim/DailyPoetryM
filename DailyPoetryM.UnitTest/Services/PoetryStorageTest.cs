using DailyPoetryM.Models;
using DailyPoetryM.Services;
using Moq;
using System.Linq.Expressions;

namespace DailyPoetryM.UnitTest.Services;

public class PoetryStorageTest : IDisposable
{
    static void RemoveDatabaseFile() => File.Delete(PoetryStorage.PoetryDbPath);
    public PoetryStorageTest()
    {
        RemoveDatabaseFile();
    }

    public void Dispose()
    {
        RemoveDatabaseFile();
    }

    [Fact]
    public void IsInitialized_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();

        preferenceStorageMock
            .Setup(preferenceStorageMock => preferenceStorageMock.Get(PoetryStorageConstant.VersionKey, 0))
            .Returns(PoetryStorageConstant.Version);

        var poetryStorage = new PoetryStorage(preferenceStorageMock.Object);
        Assert.True(poetryStorage.IsInitialized);
    }

    [Fact]
    public async Task TestInitializeAsync_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var poetryStorage = new PoetryStorage(preferenceStorageMock.Object);

        Assert.False(File.Exists(PoetryStorage.PoetryDbPath));
        await poetryStorage.InitializeAsync();
        Assert.True(File.Exists(PoetryStorage.PoetryDbPath));

        preferenceStorageMock.Verify(p => p.Set(PoetryStorageConstant.VersionKey, PoetryStorageConstant.Version), Times.Once);
    }

    [Fact]
    public async Task GetPoetryAsync_Default()
    {
        var poetryStorage = await GetInitializedPoetryStorage();

        var poetry = await poetryStorage.GetPoetryAsync(10001);

        Assert.NotNull(poetry);
        // Assert.Equal("", poetry.Name);
        await poetryStorage.CloseAsync();
    }

    [Fact]
    public async Task GetPoetriesAsync_Default()
    {
        var poetryStorage = await GetInitializedPoetryStorage();

        var poetries = await poetryStorage.GetPoetriesAsync(
            Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
            Expression.Parameter(typeof(Poetry), "p")),
            0, int.MaxValue);

        Assert.NotNull(poetries);
        await poetryStorage.CloseAsync();
    }

    static async Task<PoetryStorage> GetInitializedPoetryStorage()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var poetryStorage = new PoetryStorage(preferenceStorageMock.Object);
        await poetryStorage.InitializeAsync();
        return poetryStorage;
    }
}
