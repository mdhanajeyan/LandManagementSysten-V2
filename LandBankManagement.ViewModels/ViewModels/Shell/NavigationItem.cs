using System;
using System.Collections.ObjectModel;
using LandBankManagement.Models;


namespace LandBankManagement.ViewModels
{
    public class NavigationItem : ObservableObject
    {
        public NavigationItem(string label)
        {
            Label = label;
            CanInvoke = false;
        }

        public NavigationItem(string label, int glyph)
        {
            Label = label;
            Glyph = char.ConvertFromUtf32(glyph).ToString();
            CanInvoke = false;

        }
        public NavigationItem(Type viewModel)
        {
            ViewModel = viewModel;
        }
        public NavigationItem(int glyph, string label, Type viewModel) : this(viewModel)
        {
            Label = label;
            Glyph = char.ConvertFromUtf32(glyph).ToString();
        }

        public readonly string Glyph;
        public readonly string Label;
        public readonly Type ViewModel;
        
        public ObservableCollection<NavigationItem> Children { get; set; }
        public bool CanInvoke { get; private set; } = true;
        public string IconColor { get; set; } = "Black";

        private string _badge = null;
        public string Badge
        {
            get => _badge;
            set => Set(ref _badge, value);
        }
    }
}
