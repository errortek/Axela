using System.Diagnostics;
using System.Net;
using System.Reflection;
using App2.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.Media.PlayTo;
using Windows.Security.Cryptography.Core;
using Windows.System;

namespace App2.Views;

public sealed partial class MainPage : Microsoft.UI.Xaml.Controls.Page
{
    private string AxelaText;
    public string version;

    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
        version = fvi.FileVersion!;
    }

    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        AxelaText = axelabox.Text;
        AxelaText = AxelaText.ToLower();
        await ProcessSmartAnswer();
    }

    public List<string> Questions = new()
    {
        "Hello",
        "Hi",
        "Hey there",
        "Who is your developer?",
        "Bye",
        "It's my birthday!",
        "What's the time?",
        "Search for ... on Wikipedia",
        "How are you?",
        "Axela, Axela",
        "Cortana",
        "What's today's date?",
        "What day is it right now?",
        "What's the date?",
        "What is the date today?", 
        "What OS is this computer running?",
        "What Axela version am I running?"
    };

    public bool isRequestingWikipedia = false;

    public List<string> Answers = new()
    {
        $"Hello, {System.Security.Principal.WindowsIdentity.GetCurrent().Name}! 😊",
        $"Hello, {System.Security.Principal.WindowsIdentity.GetCurrent().Name}! 😊",
        $"Hello, {System.Security.Principal.WindowsIdentity.GetCurrent().Name}! 😊",
        "My developers are jpbandroid and Ivirius.",
        "Bye! Have a nice day! 😊",
        "Happy birthday! 😊\nI hope you have a great day!",
        $"The time right now is {DateTime.Now}.",
        "Please input your search query...",
        "I'm good. What about you? 😊",
        "I see that you're repeating my name. Is there something specific you would like to talk about? 😊",
        "Rest in peace to Cortana...",
        $"The date right now is {DateTime.Now.ToString("dd.M.yyyy")}",
        $"Today is {DateTime.Now.ToString("dddd")}",
        $"The date right now is {DateTime.Now.ToString("dd.M.yyyy")}",
        $"The date right now is {DateTime.Now.ToString("dd.M.yyyy")}",
        $"This computer is running {Environment.OSVersion}",
        $"You are running Axela v11.2405.1.0"
    };

    private async Task ProcessSmartAnswer()
    {
        if (isRequestingWikipedia == true)
        {
            var query = axelabox.Text.Replace(" ", "+");
            var uri = "https://en.wikipedia.org/wiki/Special:Search?go=Go&search=" + query + "&ns0=1";
            await Launcher.LaunchUriAsync(new Uri(uri));
            isRequestingWikipedia = false;
            return;
        }

        var similarityIndexer = new F23.StringSimilarity.Levenshtein();
        var preferredIndex = 0;
        double maxIndex = 99999;

        foreach (var question in Questions)
        {
            var similarityIndex = similarityIndexer.Distance(question, axelabox.Text);
            if (maxIndex > similarityIndex)
            {
                maxIndex = similarityIndex;
                preferredIndex = Questions.IndexOf(question);
            }
        }

        AxelaResponseText.Text = Answers[preferredIndex];

        if (Questions[preferredIndex] == "Search for ... on Wikipedia")
        {
            isRequestingWikipedia = true;
        }

        axelabox.Text = string.Empty;
    }

    public async void UserInit()
    {
        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


        //text1.Text = "Welcome, " + userName;
    }

    public class Result
    {
        public Query query
        {
            get; set;
        }
    }

    public class Query
    {
        public Dictionary<string, Page> pages
        {
            get; set;
        }
    }

    public class Page
    {
        public string extract
        {
            get; set;
        }
    }

    private async void axelabox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            AxelaText = axelabox.Text;
            AxelaText = AxelaText.ToLower();
            await ProcessSmartAnswer();
        }
    }
}