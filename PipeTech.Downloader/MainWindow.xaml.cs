﻿// <copyright file="MainWindow.xaml.cs" company="Industrial Technology Group">
// Copyright (c) Industrial Technology Group. All rights reserved.
// </copyright>

using PipeTech.Downloader.Helpers;

using Windows.UI.ViewManagement;

namespace PipeTech.Downloader;

/// <summary>
/// Main window class.
/// </summary>
public sealed partial class MainWindow : WindowEx
{
    private readonly Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

    private readonly UISettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();

        this.AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        this.Content = null;
        this.Title = "AppDisplayName".GetLocalized();

        // Theme change code picked from https://github.com/microsoft/WinUI-Gallery/pull/1239
        this.dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        this.settings = new UISettings();
        this.settings.ColorValuesChanged += this.Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event
    }

    // this handles updating the caption button colors correctly when windows system theme is changed
    // while the app is open
    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        this.dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }
}
