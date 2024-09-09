using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Core;
using Microsoft.Extensions.DependencyInjection;
using ViewModel.ViewModel.Withdrawal;

namespace View;

public partial class WithdrawalPage : Page
{
    public WithdrawalPage()
    {
        InitializeComponent();
        DataContext = new WithdrawalPageModel(App.ServiceProvider.GetRequiredService<IAtmService>());
    }
    
    private void OnGiveMoneyClick(object sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as WithdrawalPageModel;
        if (viewModel?.GiveBanknotesCommand.CanExecute(null) == true)
        {
            viewModel.GiveBanknotesCommand.Execute(null);
        }
        else
        {
            MessageBox.Show("Невозможно снять данную сумму");
            return;
        }
        NavigationService!.Navigate(new PostWithdrawalPage(viewModel!));
    }
    
    private void NumberValidation(object sender, TextChangedEventArgs  e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox == null)
            return;
        string text = textBox.Text;
        Regex regex = new Regex(@"^(0|[1-9][0-9]{0,5}|500000)$");
        if (regex.IsMatch(text)) return;
        textBox.Text = "";
        textBox.CaretIndex = 0;
    }
    
    private void OnGetBackClick(object sender, RoutedEventArgs e)
    {
        NavigationService!.GoBack();
    }
}