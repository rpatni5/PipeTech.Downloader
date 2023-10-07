// <copyright file="SettingsViewModel.cs" company="Industrial Technology Group">
// Copyright (c) Industrial Technology Group. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.WindowsAPICodePack.Dialogs;
using PipeTech.Downloader.Contracts.Services;
using PipeTech.Downloader.Helpers;
using PipeTech.Downloader.Models;
using Windows.ApplicationModel;
using Windows.System;

namespace PipeTech.Downloader.ViewModels;

/// <summary>
/// Setting vide model class.
/// </summary>
public partial class SettingsViewModel : BindableRecipient
{
    private readonly IThemeSelectorService themeSelectorService;
    private readonly ILocalSettingsService localSettingsService;
    private readonly INavigationService navigationService;

    [ObservableProperty]
    private ElementTheme elementTheme;

    [ObservableProperty]
    private string versionDescription;

    [ObservableProperty]
    private string? dataFolder;

    [ObservableProperty]
    private ObservableCollection<string> themes;

    private string selectedTheme;

    public string SelectedTheme
    {
        get
        {
            return this.selectedTheme;
        }
        set
        {
            this.selectedTheme = value;
            this.UpdateTheme((ElementTheme)Enum.Parse(typeof(ElementTheme), this.selectedTheme, true));
            this.RaisePropertyChanged();
        }
    }

    private async void UpdateTheme(ElementTheme param)
    {
        if (this.ElementTheme != param)
        {
            this.ElementTheme = param;
            await this.themeSelectorService.SetThemeAsync(param);
        }
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
    /// </summary>
    /// <param name="themeSelectorService">Theme selector service.</param>
    /// <param name="localSettingsService">Local settings service.</param>
    /// <param name="navigationService">Navigation service.</param>
    public SettingsViewModel(
        IThemeSelectorService themeSelectorService,
        ILocalSettingsService localSettingsService,
        INavigationService navigationService)
    {
        this.ScreenTitle = "Downloads";
        this.IsChildScreen = false;

        this.themeSelectorService = themeSelectorService;
        this.navigationService = navigationService;
        this.localSettingsService = localSettingsService;
        this.versionDescription = GetVersionDescription();

        this.Themes = new ObservableCollection<string> { "Light", "Dark", "Default" };
        this.ElementTheme = this.themeSelectorService.Theme;
        this.selectedTheme = this.ElementTheme.ToString();

        this.EmailCommand = new AsyncRelayCommand(() =>
        {
            _ = Launcher.LaunchUriAsync(new Uri("mailto:support.pipetech.com"));
            return Task.CompletedTask;
        });

        this.SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (this.ElementTheme != param)
                {
                    this.ElementTheme = param;
                    await this.themeSelectorService.SetThemeAsync(param);
                }
            });

        this.CloseCommand = new RelayCommand(() =>
        {
            if (this.navigationService.CanGoBack)
            {
                this.navigationService.GoBack();
            }
            else
            {
                this.navigationService.NavigateTo(typeof(DownloadsViewModel).FullName!, clearNavigation: true);
            }
        });

        this.LogFolderCommand = new AsyncRelayCommand(async () =>
        {
            await Launcher.LaunchFolderPathAsync(this.DiagnosticLogFolder);
        });

        this.BrowseFolderCommand = new RelayCommand(() =>
        {
            var d = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Title = "Select Download Folder",
            };

            if (d.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.DataFolder = d.FileName;
                _ = this.localSettingsService.SaveSettingAsync<string?>(
                    ILocalSettingsService.LastDataFolderKey,
                    this.DataFolder);
            }
        });

        _ = Task.Run(async () =>
        {
            this.DataFolder = await this.localSettingsService.ReadSettingAsync<string?>(ILocalSettingsService.LastDataFolderKey);
        });
    }

    /// <summary>
    /// Gets the diagnostic log folder.
    /// </summary>
    public string DiagnosticLogFolder => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "PipeTech",
        "PipeTech.Downloader");

    /// <summary>
    /// Gets the diagnostic log folder content.
    /// </summary>
    public string DiagnosticLogFolderContent => $"Log folder: {this.DiagnosticLogFolder}";

    /// <summary>
    /// Gets the email command.
    /// </summary>
    public IAsyncRelayCommand EmailCommand
    {
        get;
    }

    /// <summary>
    /// Gets the log folder command.
    /// </summary>
    public IAsyncRelayCommand LogFolderCommand
    {
        get;
    }

    /// <summary>
    /// Gets the command to browse for a folder.
    /// </summary>
    public IRelayCommand BrowseFolderCommand
    {
        get;
    }

    /// <summary>
    /// Gets the switch theme command.
    /// </summary>
    public ICommand SwitchThemeCommand
    {
        get;
    }

    /// <summary>
    /// Gets the switch theme command.
    /// </summary>
    public ICommand CloseCommand
    {
        get;
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;
            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
