using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DailyPoetryM.ViewModels;

public class QueryPageViewModel : ObservableObject
{
    public ObservableCollection<FilterViewModel> FilterViewModelCollection
    {
        get;
    } = new();

    public QueryPageViewModel()
    {
        FilterViewModelCollection.Add(new FilterViewModel());
        FilterViewModelCollection.Add(new FilterViewModel());
        FilterViewModelCollection.Add(new FilterViewModel());
    }
}

public class FilterViewModel : ObservableObject
{
    public FilterViewModel()
    {
        lazyAddCommand = new(new RelayCommand(AddCommandFunction));
        lazyRemoveCommand = new(new RelayCommand(RemoveCommandFunction));
    }
    public string Content
    {
        get => content;
        set => SetProperty(ref content, value);
    }
    private string content;
    private Lazy<RelayCommand> lazyAddCommand;
    private Lazy<RelayCommand> lazyRemoveCommand;
    public void AddCommandFunction() { }
    public void RemoveCommandFunction() { }
    public RelayCommand AddCommand => lazyAddCommand.Value;
    public RelayCommand RemoveCommand => lazyRemoveCommand.Value;
}
