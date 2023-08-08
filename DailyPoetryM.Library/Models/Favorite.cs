using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace DailyPoetryM.Models;

public class Favorite : ObservableObject
{
    [PrimaryKey]
    public int PoetryId { get; set; }
    private bool isFavorite;
    public virtual bool IsFavorite
    {
        get => isFavorite;
        set => SetProperty(ref isFavorite, value);
    }
    public long TimeStamp { get; set; }
}
