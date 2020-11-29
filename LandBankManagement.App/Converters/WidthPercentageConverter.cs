using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace LandBankManagement.Converters
{
    class WidthPercentageConverter : IValueConverter
    {
        public object Convert(object value,
        Type targetType,
        object parameter,
       string lang)
        {

            return System.Convert.ToDouble(value) *
                   System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            string lang)
        {
            throw new NotImplementedException();

        }
    }
}
