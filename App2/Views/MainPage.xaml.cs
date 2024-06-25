using System.Diagnostics;
using System.Net;
using System.Reflection;
using App2.ViewModels;
using Genbox.Wikipedia.Enums;
using Genbox.Wikipedia.Objects;
using Genbox.Wikipedia;
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

    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
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
        "What Axela version am I running?",
        "Say something random, for fun",
        "Tell me a fun fact"
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
        $"You are running Axela v11.2406.1.0",
        $"I can even respond to your questions offline, as I run on-device, not in the cloud",
        $"Windows XP is actually the longest-supported Windows version, being supported for 18 years (2001-2019) in its various forms, longer than that same metric for Windows 10, which has an ongoing 17 years of support (2015-2032) in its various forms."
    };

    private async Task ProcessSmartAnswer()
    {
        if (isRequestingWikipedia == true)
        {
            var query = axelabox.Text.Replace(" ", "+");
            using WikipediaClient client = new WikipediaClient();

            WikiSearchRequest req = new WikiSearchRequest(query);
            req.Limit = 1; //We would like 5 results
            req.WhatToSearch = WikiWhat.Text; //We would like to search inside the articles

            WikiSearchResponse resp = await client.SearchAsync(req);

            foreach (SearchResult s in resp.QueryResult.SearchResults)
            {
                AxelaResponseText.Text = ($"{s.Title}\n{s.Snippet}");
            }

            isRequestingWikipedia = false;
            return;
        }

        var similarityIndexer = new F23.StringSimilarity.Cosine();
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