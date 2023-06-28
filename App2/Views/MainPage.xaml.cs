using App2.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App2.Views;

public sealed partial class MainPage : Page
{
    private string AxelaText;

    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        AxelaText = axelabox.Text;
        tester.Text = AxelaText;
    }
}
