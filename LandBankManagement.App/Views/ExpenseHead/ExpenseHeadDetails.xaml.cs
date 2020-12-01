using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using LandBankManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace LandBankManagement.Views
{
    public sealed partial class ExpenseHeadDetails : UserControl
    {
        public ExpenseHeadDetails()
        {
            this.InitializeComponent();
        }
        public ExpenseHeadDetailsViewModel ViewModel
        {
            get { return (ExpenseHeadDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ExpenseHeadDetailsViewModel), typeof(ExpenseHeadDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }
    }
}
