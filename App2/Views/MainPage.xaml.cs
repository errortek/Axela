using App2.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Windows.Media.PlayTo;
using Windows.Security.Cryptography.Core;
using Windows.System;

namespace App2.Views;

public sealed partial class MainPage : Microsoft.UI.Xaml.Controls.Page
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
        AxelaText = AxelaText.ToLower();
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
                if (AxelaText.Contains("get")) {
                    if (AxelaText.Contains("from wikipedia"))
                    {
                        string WikiArticle = string.Empty;
                        AxelaText = WikiArticle;
                        WikiArticle.Replace("get ", "");
                        WikiArticle.Replace("from wikipedia", "");

                    }
                }
                if (AxelaText.Contains("how are you"))
                {
                    AxelaResponseText.Text = "I'm great! :D\nBut I don't really have feelings, as I am an AI chatbot";
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
