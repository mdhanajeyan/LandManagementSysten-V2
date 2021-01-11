using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LandBankManagement.Styles
{
    public class ListViewStyleSelector : StyleSelector
    {
        public Style OddStyle { get; set; }
        public Style EvenStyle { get; set; }
        protected override Style SelectStyleCore(object item,DependencyObject container) {
            var itm = (ListViewItem)item;
            return OddStyle;

           // return base.SelectStyle(item, container);
        }
    }
}
