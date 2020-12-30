
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Syncfusion.UI.Xaml.TreeGrid;
using Windows.UI;
using System;

namespace LandBankManagement.Converters
{
   public sealed class SfTreeGridAlternativeRowStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (((value as TreeGridRowControl).DataRow.RowIndex % 2 == 0) && ((value as TreeGridRowControl).DataRow.Level == 0))
                return new SolidColorBrush(Color.FromArgb(98, 187, 232, 251));
            else if (((value as TreeGridRowControl).DataRow.RowIndex % 2 != 0) && ((value as TreeGridRowControl).DataRow.Level == 0))
                return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            else
            if (((value as TreeGridRowControl).DataRow.RowIndex % 2 == 0) && ((value as TreeGridRowControl).DataRow.Level != 0))
                return new SolidColorBrush(Color.FromArgb(98, 187, 232, 251));
            else if (((value as TreeGridRowControl).DataRow.RowIndex % 2 != 0) && ((value as TreeGridRowControl).DataRow.Level != 0))
                return new SolidColorBrush(Color.FromArgb(255, 255, 255,255));
            else
                return new SolidColorBrush(Color.FromArgb(255, 112, 252, 160));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
