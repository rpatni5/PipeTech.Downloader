using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using PipeTech.Downloader.Core.Contracts;

namespace PipeTech.Downloader.Models;

public partial class ProjectGroup : BindableRecipient
{

    private string groupTitle;
    public string GroupTitle
    {
        get => groupTitle;
        set => SetProperty(ref groupTitle, value);
    }
    private List<Project> projects;
    public List<Project> Projects
    {
        get => projects;
        set => SetProperty(ref projects, value);
    }

    public ProjectGroup(string groupTitle, List<Project> projects)
    {

        this.GroupTitle = groupTitle;
        this.Projects = projects;
    }
}
