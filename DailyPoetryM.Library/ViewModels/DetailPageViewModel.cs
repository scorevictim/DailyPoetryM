using CommunityToolkit.Mvvm.ComponentModel;
using DailyPoetryM.Models;

namespace DailyPoetryM.ViewModels;

public partial class DetailPageViewModel : ObservableObject
{
    [ObservableProperty]
    private Poetry poetry;
}
