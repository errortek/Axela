using App2.Helpers;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.ViewManagement;
namespace App2;

public sealed partial class MainWindow : WindowEx
{
    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

    private UISettings settings;

    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        SystemBackdrop = new MicaBackdrop()
        {
            Kind = MicaKind.BaseAlt
        };
        var appWindow = AppWindow;
        var titleBar = appWindow.TitleBar;
        titleBar.ExtendsContentIntoTitleBar = true;
        bool isTallTitleBar = true;
        if (AppWindowTitleBar.IsCustomizationSupported() && appWindow.TitleBar.ExtendsContentIntoTitleBar)
        {
            AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            AppWindow.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(25, 255, 255, 255);
            AppWindow.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(25, 200, 200, 200);
            if (isTallTitleBar)
            {
                appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            }
            else
            {
                appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Standard;
            }
        }

        // Theme change code picked from https://github.com/microsoft/WinUI-Gallery/pull/1239
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event
    }

    // this handles updating the caption button colors correctly when indows system theme is changed
    // while the app is open
    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }
}
