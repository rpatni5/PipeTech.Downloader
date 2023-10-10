// <copyright file="EnumToBooleanConverter.cs" company="Industrial Technology Group">
// Copyright (c) Industrial Technology Group. All rights reserved.
// </copyright>

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace PipeTech.Downloader.Helpers;

/// <summary>
/// Enum to boolean converter class.
/// </summary>
public class ErrorCountConverter : IValueConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorCountConverter"/> class.
    /// </summary>
    public ErrorCountConverter()
    {
    }

    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int)
        {
            int count = (int)value;
            return count > 1 ? $"{count} Errors •  " : $"{count} Error • ";
        }
        return string.Empty;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    }
}
