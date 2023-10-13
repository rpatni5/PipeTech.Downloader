// <copyright file="EnumToBooleanConverter.cs" company="Industrial Technology Group">
// Copyright (c) Industrial Technology Group. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using PipeTech.Downloader.Models;

namespace PipeTech.Downloader.Helpers;

/// <summary>
/// Enum to boolean converter class.
/// </summary>
public class CollectionToVisibilityConverter : IValueConverter
{
    /// <summary>    CollectionToVisibilityConverter    /// </summary>
    public CollectionToVisibilityConverter()
    {
    }

    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is "Invert")
        {
            return GetCount(value) == 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        else
        {
            return GetCount(value) == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    private int GetCount(object value)
    {
        if (value is ObservableCollection<ProjectGroup>)
        {
            return (value as ObservableCollection<ProjectGroup>).Count;
        }
        return 0;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    }
}
