// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App2.Views;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SysBackdropTest : Window
{
    BackdropType m_currentBackdrop;

    public SysBackdropTest()
    {
        this.InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(titleBar);
        SetBackdrop(BackdropType.Mica);
    }

    public enum BackdropType
    {
        Mica,
        MicaAlt,
        DesktopAcrylic,
        DefaultColor,
    }

    public void SetBackdrop(BackdropType type)
    {
        // Reset to default color. If the requested type is supported, we'll update to that.
        // Note: This sample completely removes any previous controller to reset to the default
        //       state. This is done so this sample can show what is expected to be the most
        //       common pattern of an app simply choosing one controller type which it sets at
        //       startup. If an app wants to toggle between Mica and Acrylic it could simply
        //       call RemoveSystemBackdropTarget() on the old controller and then setup the new
        //       controller, reusing any existing m_configurationSource and Activated/Closed
        //       event handlers.
        m_currentBackdrop = BackdropType.DefaultColor;
        tbCurrentBackdrop.Text = "None (default theme color)";
        tbChangeStatus.Text = "";
        this.SystemBackdrop = null;

        if (type == BackdropType.Mica)
        {
            if (TrySetMicaBackdrop(false))
            {
                tbCurrentBackdrop.Text = "Built-in Mica";
                m_currentBackdrop = type;
            }
            else
            {
                // Mica isn't supported. Try Acrylic.
                type = BackdropType.DesktopAcrylic;
                tbChangeStatus.Text += "  Mica isn't supported. Trying Acrylic.";
            }
        }
        if (type == BackdropType.MicaAlt)
        {
            if (TrySetMicaBackdrop(true))
            {
                tbCurrentBackdrop.Text = "Built-in MicaAlt";
                m_currentBackdrop = type;
            }
            else
            {
                // MicaAlt isn't supported. Try Acrylic.
                type = BackdropType.DesktopAcrylic;
                tbChangeStatus.Text += "  MicaAlt isn't supported. Trying Acrylic.";
            }
        }
        if (type == BackdropType.DesktopAcrylic)
        {
            if (TrySetAcrylicBackdrop())
            {
                tbCurrentBackdrop.Text = "Built-in Acrylic";
                m_currentBackdrop = type;
            }
            else
            {
                // Acrylic isn't supported, so take the next option, which is DefaultColor, which is already set.
                tbChangeStatus.Text += "  Acrylic isn't supported. Switching to default color.";
            }
        }

    }

    bool TrySetMicaBackdrop(bool useMicaAlt)
    {
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            Microsoft.UI.Xaml.Media.MicaBackdrop micaBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop();
            micaBackdrop.Kind = useMicaAlt ? Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt : Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;
            this.SystemBackdrop = micaBackdrop;
            return true;
        }

        return false; // Mica is not supported on this system
    }

    bool TrySetAcrylicBackdrop()
    {
        if (Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController.IsSupported())
        {
            this.SystemBackdrop = new Microsoft.UI.Xaml.Media.DesktopAcrylicBackdrop();
            return true;
        }

        return false; // Acrylic is not supported on this system
    }

    void ChangeBackdropButton_Click(object sender, RoutedEventArgs e)
    {
        BackdropType newType;

        switch (m_currentBackdrop)
        {
            case BackdropType.Mica: newType = BackdropType.MicaAlt; break;
            case BackdropType.MicaAlt: newType = BackdropType.DesktopAcrylic; break;
            case BackdropType.DesktopAcrylic: newType = BackdropType.DefaultColor; break;
            default:
            case BackdropType.DefaultColor: newType = BackdropType.Mica; break;
        }

        SetBackdrop(newType);
    }
}
