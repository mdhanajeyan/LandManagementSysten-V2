using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.UI.Xaml.Grid.Cells;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Common
{
    public class ComboBoxRenderer : GridCellComboBoxRenderer
    {
        protected override void OnEditElementLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (sender as ComboBox).IsDropDownOpen = true;
            base.OnEditElementLoaded(sender, e);
        }
    }
}
