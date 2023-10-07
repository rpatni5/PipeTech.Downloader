using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace PipeTech.Downloader.Helpers;
public class AlternatingRowListView : ListView
{
    public AlternatingRowListView()
    {
        DefaultStyleKey = typeof(ListView);
    }
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);
        var listViewItem = element as ListViewItem;
        if (listViewItem != null)
        {
            var index = IndexFromContainer(element);

            if (index % 2 == 0)
            {
                listViewItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 249, 249, 249));
            }
            else
            {
                listViewItem.Background = new SolidColorBrush(Colors.White);
            }
        }

    }
}
