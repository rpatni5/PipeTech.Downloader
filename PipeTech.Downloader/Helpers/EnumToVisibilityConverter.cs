// <copyright file="EnumToBooleanConverter.cs" company="Industrial Technology Group">
// Copyright (c) Industrial Technology Group. All rights reserved.
// </copyright>

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using static PipeTech.Downloader.Models.DownloadInspection;

namespace PipeTech.Downloader.Helpers;

/// <summary>
/// Enum to boolean converter class.
/// </summary>
public class EnumToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumToVisibilityConverter"/> class.
    /// </summary>
    public EnumToVisibilityConverter()
    {
    }

    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string enumString)
        {
            if (!Enum.IsDefined(typeof(States), value))
            {
                throw new ArgumentException("ExceptionEnumToBooleanConverterValueMustBeAnEnum");
            }

            var enumValue = Enum.Parse(typeof(States), enumString);

            return enumValue.Equals(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string enumString)
        {
            return Enum.Parse(typeof(States), enumString);
        }

        throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    }
}
