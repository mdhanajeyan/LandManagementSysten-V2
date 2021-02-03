using LandBankManagement.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Views
{
    public sealed partial class RolePermissionDetails : UserControl
    {
        public RolePermissionDetails()
        {
            this.InitializeComponent();
        }
        public RolePermissionDetailsViewModel ViewModel
        {
            get { return (RolePermissionDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(RolePermissionDetailsViewModel), typeof(RolePermissionDetails), new PropertyMetadata(null));


        public void SetFocus()
        {
            details.SetFocus();
        }

        private void RoleDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = ((ComboBox)sender).SelectedValue;
            if (val == null || val == "0")
                return;
            var id =Convert.ToInt32( val);
            if(id!=0)
            ViewModel.GetRolePermissionForRole(id);
        }
    }
}
