using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PipeTech.Downloader.Helpers;
public class StatusTextMultivalueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values != null && values.Count() > 0)
        {
            int count = (int)values[0];
            long progress = (int)values[1];
            long totalsize = (int)values[2];

            if (parameter != null && parameter.Equals("Complete"))
            {
                return $"{count} Inspections {GetTotalSize(totalsize)}";
            }

            return $"{count} Inspections {GetDownloadedPercentage(progress)} of {GetTotalSize(totalsize)}";
        }
        return string.Empty;

    }
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public string GetDownloadedPercentage(object? value)
    {
        if (value == null)
        {
            return null;
        }

        if (!decimal.TryParse(value.ToString(), out var d))
        {
            return null;
        }

        return $"{d * 100m:0.0}%";
    }

    private string GetTotalSize(object? value)
    {
        if (value == null)
        {
            return null;
        }

        if (!long.TryParse(value.ToString(), out var bytes))
        {
            return null;
        }

        if (bytes < Math.Pow(2, 20))
        {
            // 1024 * 1024
            // Return as kilo bytes
            return (bytes / Math.Pow(2, 10)).ToString("0.0 KB");
        }
        else if (bytes <= Math.Pow(2, 30) * 10)
        {
            // 1024 * 1024 * 1024
            // Return in MB
            return (bytes / Math.Pow(2, 20)).ToString("0.0 MB");
        }
        else
        {
            // Return in GB
            return (bytes / Math.Pow(2, 30)).ToString("0.0 GB");
        }
    }
}
