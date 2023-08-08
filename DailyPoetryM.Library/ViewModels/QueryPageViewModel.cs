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
        FilterViewModelCollection.Add(new FilterViewModel(this));
        FilterViewModelCollection.Add(new FilterViewModel(this));
        FilterViewModelCollection.Add(new FilterViewModel(this));
    }
    public virtual void AddFilterViewModel(FilterViewModel filterViewModel)
    {
        FilterViewModelCollection.Insert(FilterViewModelCollection.IndexOf(filterViewModel) + 1, new(this));
    }
    public virtual void RemoveFilterViewModel(FilterViewModel filterViewModel)
    {
        FilterViewModelCollection.Remove(filterViewModel);
        if (FilterViewModelCollection.Count == 0)
        {
            FilterViewModelCollection.Add(new(this));
        }
    }
}

public class FilterViewModel : ObservableObject
{
    public FilterViewModel(QueryPageViewModel queryPageViewModel)
    {
        lazyAddCommand = new(new RelayCommand(AddCommandFunction));
        lazyRemoveCommand = new(new RelayCommand(RemoveCommandFunction));
        this.queryPageViewModel = queryPageViewModel;
    }
    public string Content
    {
        get => content;
        set => SetProperty(ref content, value);
    }
    private string content;
    private Lazy<RelayCommand> lazyAddCommand;
    private Lazy<RelayCommand> lazyRemoveCommand;
    private readonly QueryPageViewModel queryPageViewModel;

    public void AddCommandFunction()
    {
        queryPageViewModel.AddFilterViewModel(this);
    }
    public void RemoveCommandFunction()
    {
        queryPageViewModel.RemoveFilterViewModel(this);
    }
    public RelayCommand AddCommand => lazyAddCommand.Value;
    public RelayCommand RemoveCommand => lazyRemoveCommand.Value;
}
