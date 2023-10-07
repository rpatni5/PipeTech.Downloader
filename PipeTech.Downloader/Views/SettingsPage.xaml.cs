﻿// <copyright file="SettingsPage.xaml.cs" company="Industrial Technology Group">
// Copyright (c) Industrial Technology Group. All rights reserved.
// </copyright>

using Microsoft.UI.Xaml.Controls;

using PipeTech.Downloader.ViewModels;

namespace PipeTech.Downloader.Views;

/// <summary>
/// Setting page class.
/// </summary>
public sealed partial class SettingsPage : Page
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsPage"/> class.
    /// </summary>
    public SettingsPage()
    {
        this.DataContext = this.ViewModel = App.GetService<SettingsViewModel>();
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets the view model.
    /// </summary>
    public SettingsViewModel ViewModel
    {
        get;
    }
}
