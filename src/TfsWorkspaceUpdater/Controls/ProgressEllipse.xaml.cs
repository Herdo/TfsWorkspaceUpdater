namespace TfsWorkspaceUpdater.Controls
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using Shared.Data;
    using Xceed.Wpf.Toolkit;

    /// <summary>
    /// Interaction logic for ProgressEllipse.xaml
    /// </summary>
    public partial class ProgressEllipse
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Dependency Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(ObservableCollection<UpdateableWorkingFolder>),
            typeof(ProgressEllipse),
            new FrameworkPropertyMetadata
            {
                DefaultValue = null,
                PropertyChangedCallback = ItemsSourcePropertyChangedCallback,
                BindsTwoWayByDefault = false
            });

        public ObservableCollection<UpdateableWorkingFolder> ItemsSource
        {
            get { return GetValue(ItemsSourceProperty) as ObservableCollection<UpdateableWorkingFolder>; }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as ProgressEllipse;
            if (c == null) return;

            var oldValue = e.OldValue as ObservableCollection<UpdateableWorkingFolder>;
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= c.ItemsSource_CollectionChanged;
            }

            var newValue = e.NewValue as ObservableCollection<UpdateableWorkingFolder>;
            if (newValue == null)
            {
                c.PieGrid.Children.Clear();
                return;
            }
            
            newValue.CollectionChanged += c.ItemsSource_CollectionChanged;
            c.CreateElements();
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CreateElements();
        }

        #endregion

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Fields

        private readonly IDictionary<UpdateableWorkingFolder, Pie> _elementMapping;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        public ProgressEllipse()
        {
            _elementMapping = new Dictionary<UpdateableWorkingFolder, Pie>();

            InitializeComponent();
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private void CreateElements()
        {
            foreach (var updateableWorkingFolder in _elementMapping.Keys)
            {
                updateableWorkingFolder.PropertyChanged -= UpdateableWorkingFolder_PropertyChanged;
            }
            _elementMapping.Clear();
            PieGrid.Children.Clear();

            if (ItemsSource == null
             || ItemsSource.Count == 0) return;

            var total = ItemsSource.Count;
            var degreePerSlive = (double)360 / total;
            var percentage = (double)100 / total / 100;
            
            for (var i = 0; i < total; i++)
            {
                var item = ItemsSource[i];
                item.PropertyChanged += UpdateableWorkingFolder_PropertyChanged;
                var pie = new Pie
                {
                    StrokeThickness = 1,
                    Fill = Brushes.LightGray,
                    Stroke = Brushes.Black,
                    Stretch = Stretch.UniformToFill,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new RotateTransform(-90 + degreePerSlive * i),
                    Slice = percentage
                };
                
                if (!item.MayGet)
                {
                    pie.Fill = Brushes.DimGray;
                    pie.ToolTip = $"Working folder \"{item.LocalPath}\" is cloaked.";
                }

                _elementMapping.Add(item, pie);

                PieGrid.Children.Add(pie);
            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Event Handler

        private void UpdateableWorkingFolder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var s = sender as UpdateableWorkingFolder;
            if (s == null) return;
            var pie = _elementMapping[s];
            if (s.Started && !s.Done)
            {
                pie.Fill = Brushes.PaleGoldenrod;
                pie.ToolTip = "Getting working folder...";
                ProgressInfo.Text = $"Updating \"{s.LocalPath}\" ...";
            }
            else if (s.Done)
            {
                ProgressInfo.Text = string.Empty;
                if (s.NumFailures > 0)
                {
                    pie.Fill = Brushes.DarkRed;
                }
                else if (s.NumConflicts > 0)
                {
                    pie.Fill = Brushes.OrangeRed;
                }
                else
                {
                    if (s.NumFiles > 0
                     || s.NumUpdated > 0)
                    {
                        pie.Fill = Brushes.GreenYellow;
                    }
                    else
                    {
                        pie.Fill = Brushes.ForestGreen;
                    }
                }

                pie.ToolTip = $@"{s.LocalPath}
Files: {s.NumFiles}
Updated: {s.NumUpdated}
Conflicts: {s.NumConflicts}
Failures: {s.NumFailures}";
            }
        }

        #endregion
    }
}
