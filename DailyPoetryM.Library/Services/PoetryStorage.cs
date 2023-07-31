using DailyPoetryM.Models;
using SQLite;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace DailyPoetryM.Services;

public class PoetryStorage : IPoetryStorage
{
    private readonly IPreferenceStorage preferenceStorage;
    public const string DbName = "poetrydb.sqlite3";
    public static readonly string PoetryDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);
    private SQLiteAsyncConnection? connection;
    private SQLiteAsyncConnection Connection => connection ??= new SQLiteAsyncConnection(PoetryDbPath);

    public PoetryStorage(IPreferenceStorage preferenceStorage)
    {
        this.preferenceStorage = preferenceStorage;
    }
    public bool IsInitialized => preferenceStorage.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;

    public async Task<IEnumerable<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take)
    {
        return await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<Poetry> GetPoetryAsync(int id)
    {
        return await Connection.Table<Poetry>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CloseAsync() => await Connection.CloseAsync();

    public async Task InitializeAsync()
    {
        await using var dbFileStream = new FileStream(PoetryDbPath, FileMode.OpenOrCreate);

        await using var dbAssetStream = typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName) ?? throw new Exception();

        await dbAssetStream.CopyToAsync(dbFileStream);

        preferenceStorage.Set(PoetryStorageConstant.VersionKey, PoetryStorageConstant.Version);
    }
}

public static class PoetryStorageConstant
{
    public const int Version = 1;
    public const string VersionKey = $"{nameof(PoetryStorageConstant)}.{nameof(Version)}";
}
