using System.Windows;
using System.Windows.Controls;
using Core;
using Microsoft.Extensions.DependencyInjection;

using ViewModel.ViewModel.Info;

namespace View;

public partial class InfoPage : Page
{
    public InfoPage()
    {
        InitializeComponent();
        DataContext = new InfoPageModel(App.ServiceProvider.GetRequiredService<IAtmService>());
    }

    private void OnGetBackClick(object sender, RoutedEventArgs e)
    {
        NavigationService!.GoBack();
    }
}