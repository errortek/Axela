using App2.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Windows.System;

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
        processText();
    }

    private async void processText()
    {
        if (AxelaText != null)
        {
            if (AxelaText.Length > 0)
            {
                if (AxelaText.Contains("hello")) {
                    string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    AxelaResponseText.Text = "Hello, " + userName;
                }
                if (AxelaText.Contains("who is your developer"))
                {
                    AxelaResponseText.Text = "My developer is jpb, sometimes referred to as jpbandroid :D";
                }
                if (AxelaText.Contains("bye"))
                {
                    string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    AxelaResponseText.Text = "Bye! :D\nHave a nice day, " + userName;
                }
                if (AxelaText.Contains("it's my birthday"))
                {
                    string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    AxelaResponseText.Text = "Happy birthday, " + userName;
                }
                if (AxelaText.Contains("what's the time"))
                {
                    AxelaResponseText.Text = "The time right now is: " + DateTime.Now.ToString();
                }
            }
        }
    }

    public async void UserInit()
    {
        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


        //text1.Text = "Welcome, " + userName;
    }
}
