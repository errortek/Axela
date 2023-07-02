using App2.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App2.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        SysBackdropTest sysbackdropwindow = new SysBackdropTest();
        sysbackdropwindow.Show();
    }
}
