// <copyright file="DownloadDetailViewModel.cs" company="Industrial Technology Group">
// Copyright (c) Industrial Technology Group. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using PipeTech.Downloader.Contracts.Services;
using PipeTech.Downloader.Contracts.ViewModels;
using PipeTech.Downloader.Models;
using Windows.System;

namespace PipeTech.Downloader.ViewModels;

/// <summary>
/// Download Detail page view model class.
/// </summary>
public partial class DownloadDetailViewModel : BindableRecipient, INavigationAware
{
    private readonly INavigationService? navigationService;
    private readonly ILogger<DownloadDetailViewModel>? logger;
    private readonly IDownloadService downloadService;
    private readonly IBackgroundJobClientV2 jobClient;
    private ContentDialog? dialog = null;

    public DownloadDetailViewModel(INavigationService navigationService,
         IDownloadService downloadService,
        IBackgroundJobClientV2 jobClient,
        ILogger<DownloadDetailViewModel>? logger = null)
    {
        this.navigationService = navigationService;
        this.logger = logger;
        this.downloadService = downloadService;
        this.jobClient = jobClient;
        project = new Project();
        this.ScreenTitle = "Details";
        this.IsChildScreen = true;
    }
    public void OnNavigatedFrom()
    {
    }

    [ObservableProperty]
    private Project project;

    /// <inheritdoc/>
    public void OnNavigatedTo(object parameter)
    {
        this.Project = parameter as Project;
    }


    [RelayCommand]
    private async Task NavigateBack(object? param)
    {
        if (this.navigationService.CanGoBack)
        {
            this.navigationService.GoBack();
        }
        else
        {
            this.navigationService.NavigateTo(typeof(DownloadsViewModel).FullName!, clearNavigation: true);
        }
        await Task.CompletedTask;
        return;
    }

    [RelayCommand]
    private async Task OpenFolder(object? param)
    {
        if (param is not Project p ||
            string.IsNullOrEmpty(p.DownloadPath) ||
            !Directory.Exists(p.DownloadPath))
        {
            await Task.CompletedTask;
            return;
        }

        try
        {
            await Launcher.LaunchFolderPathAsync(p.DownloadPath);
        }
        catch (Exception ex)
        {
            this.logger?.LogError(ex, $"Unable to open folder [{p.DownloadPath}]");
        }
    }

    [RelayCommand]
    private async Task PauseProject(object? param)
    {
        if (param is not Project p || p.Inspections is null)
        {
            return;
        }

        // Find jobs for the project and cancel them.
        var jobs = this.downloadService.FindJobForProject(p);
        while (jobs is not null && jobs.Any())
        {
            foreach (var j in jobs)
            {
                this.jobClient.Delete(j.Key);
            }

            await Task.Delay(500);
            jobs = this.downloadService.FindJobForProject(p);
        }

        // Set the inspection state of each non-completed inspection to staged
        foreach (var i in p.Inspections)
        {
            if (i.Inspection is not null &&
            i.Inspection.State != DownloadInspection.States.Complete)
            {
                i.Inspection.State = DownloadInspection.States.Paused;
            }
        }
    }

    [RelayCommand]
    private async Task ResumeProject(object? param)
    {

        if (param is not Project p || p.Inspections is null)
        {
            return;
        }

        // Find jobs for the project and cancel them.
        var jobs = this.downloadService.FindJobForProject(p);
        while (jobs is not null && jobs.Any())
        {
            foreach (var j in jobs)
            {
                this.jobClient.Delete(j.Key);
            }

            await Task.Delay(500);
            jobs = this.downloadService.FindJobForProject(p);
        }

        // Set the inspection state of each non-completed inspection to staged
        foreach (var i in p.Inspections)
        {
            if (i.Inspection is not null &&
            i.Inspection.State != DownloadInspection.States.Complete)
            {
                i.Inspection.State = DownloadInspection.States.Staged;
            }
        }

        // Create a job
        await this.downloadService.CreateJobForProject(p);
    }

    [RelayCommand]
    private async Task ShowDetails(object? param)
    {
        if (param is not DownloadInspection dl ||
            string.IsNullOrEmpty(dl.LastError))
        {
            await Task.CompletedTask;
            return;
        }

        try
        {
            this.dialog?.Hide();

            this.dialog = new ContentDialog()
            {
                XamlRoot = App.MainWindow.Content.XamlRoot,
                Title = string.Empty,
                Content = dl.LastError,
                CloseButtonText = "Close",
            };

            await this.dialog.ShowAsync();
        }
        catch (Exception)
        {
        }
    }

}
