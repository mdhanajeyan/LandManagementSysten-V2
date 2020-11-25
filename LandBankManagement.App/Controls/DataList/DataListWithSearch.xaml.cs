﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LandBankManagement.Controls
{
    public sealed partial class DataListWithSearch : UserControl, INotifyExpressionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DataListWithSearch()
        {
            this.InitializeComponent();
            DependencyExpressions.Initialize(this);
        }
        static private readonly DependencyExpressions DependencyExpressions = new DependencyExpressions();

        #region NewLabel
        public string NewLabel
        {
            get { return (string)GetValue(NewLabelProperty); }
            set { SetValue(NewLabelProperty, value); }
        }

        public static readonly DependencyProperty NewLabelProperty = DependencyProperty.Register(nameof(NewLabel), typeof(string), typeof(DataListWithSearch), new PropertyMetadata("New"));
        #endregion

        #region ItemsSource*
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataListWithSearch;
            control.UpdateItemsSource(e.NewValue, e.OldValue);
            DependencyExpressions.UpdateDependencies(control, nameof(ItemsSource));
        }

        private void UpdateItemsSource(object newValue, object oldValue)
        {
            if (oldValue is INotifyCollectionChanged oldSource)
            {
                oldSource.CollectionChanged -= OnCollectionChanged;
            }
            if (newValue is INotifyCollectionChanged newSource)
            {
                newSource.CollectionChanged += OnCollectionChanged;
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(DataListWithSearch), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region ItemSecondaryActionInvokedCommand
        public ICommand ItemSecondaryActionInvokedCommand
        {
            get { return (ICommand)GetValue(ItemSecondaryActionInvokedCommandProperty); }
            set { SetValue(ItemSecondaryActionInvokedCommandProperty, value); }
        }

        public static readonly DependencyProperty ItemSecondaryActionInvokedCommandProperty = DependencyProperty.Register(nameof(ItemSecondaryActionInvokedCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region DefaultCommands
        public string DefaultCommands
        {
            get { return (string)GetValue(DefaultCommandsProperty); }
            set { SetValue(DefaultCommandsProperty, value); }
        }

        public static readonly DependencyProperty DefaultCommandsProperty = DependencyProperty.Register(nameof(DefaultCommands), typeof(string), typeof(DataListWithSearch), new PropertyMetadata("new,select,refresh,search"));
        #endregion

        #region SelectedItem
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region IsMultipleSelection*
        public bool IsMultipleSelection
        {
            get { return (bool)GetValue(IsMultipleSelectionProperty); }
            set { SetValue(IsMultipleSelectionProperty, value); }
        }

        private static void IsMultipleSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataListWithSearch;
            DependencyExpressions.UpdateDependencies(control, nameof(IsMultipleSelection));
        }

        public static readonly DependencyProperty IsMultipleSelectionProperty = DependencyProperty.Register(nameof(IsMultipleSelection), typeof(bool), typeof(DataListWithSearch), new PropertyMetadata(null, IsMultipleSelectionChanged));
        #endregion

        #region SelectedItemsCount*
        public int SelectedItemsCount
        {
            get { return (int)GetValue(SelectedItemsCountProperty); }
            set { SetValue(SelectedItemsCountProperty, value); }
        }

        private static void SelectedItemsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataListWithSearch;
            DependencyExpressions.UpdateDependencies(control, nameof(SelectedItemsCount));
        }

        public static readonly DependencyProperty SelectedItemsCountProperty = DependencyProperty.Register(nameof(SelectedItemsCount), typeof(int), typeof(DataListWithSearch), new PropertyMetadata(null, SelectedItemsCountChanged));
        #endregion


        #region Query
        public string Query
        {
            get { return (string)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public static readonly DependencyProperty QueryProperty = DependencyProperty.Register(nameof(Query), typeof(string), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region ItemsCount
        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }

        public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register(nameof(ItemsCount), typeof(int), typeof(DataListWithSearch), new PropertyMetadata(0));
        #endregion


        #region RefreshCommand
        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register(nameof(RefreshCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region QuerySubmittedCommand
        public ICommand QuerySubmittedCommand
        {
            get { return (ICommand)GetValue(QuerySubmittedCommandProperty); }
            set { SetValue(QuerySubmittedCommandProperty, value); }
        }

        public static readonly DependencyProperty QuerySubmittedCommandProperty = DependencyProperty.Register(nameof(QuerySubmittedCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region NewCommand
        public ICommand NewCommand
        {
            get { return (ICommand)GetValue(NewCommandProperty); }
            set { SetValue(NewCommandProperty, value); }
        }

        public static readonly DependencyProperty NewCommandProperty = DependencyProperty.Register(nameof(NewCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion


        #region StartSelectionCommand
        public ICommand StartSelectionCommand
        {
            get { return (ICommand)GetValue(StartSelectionCommandProperty); }
            set { SetValue(StartSelectionCommandProperty, value); }
        }

        public static readonly DependencyProperty StartSelectionCommandProperty = DependencyProperty.Register(nameof(StartSelectionCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region CancelSelectionCommand
        public ICommand CancelSelectionCommand
        {
            get { return (ICommand)GetValue(CancelSelectionCommandProperty); }
            set { SetValue(CancelSelectionCommandProperty, value); }
        }

        public static readonly DependencyProperty CancelSelectionCommandProperty = DependencyProperty.Register(nameof(CancelSelectionCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region SelectItemsCommand
        public ICommand SelectItemsCommand
        {
            get { return (ICommand)GetValue(SelectItemsCommandProperty); }
            set { SetValue(SelectItemsCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectItemsCommandProperty = DependencyProperty.Register(nameof(SelectItemsCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region DeselectItemsCommand
        public ICommand DeselectItemsCommand
        {
            get { return (ICommand)GetValue(DeselectItemsCommandProperty); }
            set { SetValue(DeselectItemsCommandProperty, value); }
        }

        public static readonly DependencyProperty DeselectItemsCommandProperty = DependencyProperty.Register(nameof(DeselectItemsCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion

        #region SelectRangesCommand
        public ICommand SelectRangesCommand
        {
            get { return (ICommand)GetValue(SelectRangesCommandProperty); }
            set { SetValue(SelectRangesCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectRangesCommandProperty = DependencyProperty.Register(nameof(SelectRangesCommand), typeof(ICommand), typeof(DataListWithSearch), new PropertyMetadata(null));
        #endregion


        public ListToolbarMode ToolbarMode => IsMultipleSelection ? (SelectedItemsCount > 0 ? ListToolbarMode.CancelDelete : ListToolbarMode.Cancel) : ListToolbarMode.Default;
        static DependencyExpression ToolbarModeExpression = DependencyExpressions.Register(nameof(ToolbarMode), nameof(IsMultipleSelection), nameof(SelectedItemsCount));

        public ListViewSelectionMode SelectionMode => IsMultipleSelection ? ListViewSelectionMode.Multiple : ListViewSelectionMode.Single;
        static DependencyExpression SelectionModeExpression = DependencyExpressions.Register(nameof(SelectionMode), nameof(IsMultipleSelection));

        public bool IsSingleSelection => !IsMultipleSelection;
        static DependencyExpression IsSingleSelectionExpression = DependencyExpressions.Register(nameof(IsSingleSelection), nameof(IsMultipleSelection));

        public bool IsDataAvailable => (ItemsSource?.Cast<object>().Any() ?? false);
        static DependencyExpression IsDataAvailableExpression = DependencyExpressions.Register(nameof(IsDataAvailable), nameof(ItemsSource));

        public bool IsDataUnavailable => !IsDataAvailable;
        static DependencyExpression IsDataUnavailableExpression = DependencyExpressions.Register(nameof(IsDataUnavailable), nameof(IsDataAvailable));

        public string DataUnavailableMessage => ItemsSource == null ? "Loading..." : "No items found.";
        static DependencyExpression DataUnavailableMessageExpression = DependencyExpressions.Register(nameof(DataUnavailableMessage), nameof(ItemsSource));

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!IsMultipleSelection)
            {
                if (ItemsSource is IList list)
                {
                    if (e.Action == NotifyCollectionChangedAction.Replace)
                    {
                        if (ItemsSource is ISelectionInfo selectionInfo)
                        {
                            if (selectionInfo.IsSelected(e.NewStartingIndex))
                            {
                                SelectedItem = list[e.NewStartingIndex];
                                System.Diagnostics.Debug.WriteLine("SelectedItem {0}", SelectedItem);
                            }
                        }
                    }
                }
            }
        }

        private void OnDoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            if (!IsMultipleSelection)
            {
                ItemSecondaryActionInvokedCommand?.TryExecute(listview.SelectedItem);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsMultipleSelection)
            {
                if (listview.SelectedItems != null)
                {
                    SelectedItemsCount = listview.SelectedItems.Count;
                }
                else if (listview.SelectedRanges != null)
                {
                    var ranges = listview.SelectedRanges;
                    SelectedItemsCount = ranges.IndexCount();
                    SelectRangesCommand?.TryExecute(ranges.GetIndexRanges().ToArray());
                }

                if (e.AddedItems != null)
                {
                    SelectItemsCommand?.TryExecute(e.AddedItems);
                }
                if (e.RemovedItems != null)
                {
                    DeselectItemsCommand?.TryExecute(e.RemovedItems);
                }
            }
        }

        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            QuerySubmittedCommand?.TryExecute(args.QueryText);
        }

        private void OnToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.New:
                    NewCommand?.TryExecute();
                    break;
                case ToolbarButton.Delete:
                    DeleteCommand?.TryExecute();
                    break;
                case ToolbarButton.Select:
                    StartSelectionCommand?.TryExecute();
                    break;
                case ToolbarButton.Refresh:
                    RefreshCommand?.TryExecute();
                    break;
                case ToolbarButton.Cancel:
                    CancelSelectionCommand?.TryExecute();
                    break;
            }
        }

        #region NotifyPropertyChanged
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
