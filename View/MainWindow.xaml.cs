using System.Windows;
using ViewModel.ViewModel.Base;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            MainFrame.Navigate(new StartPage());
        }

    }
}