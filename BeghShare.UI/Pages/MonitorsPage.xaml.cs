using BeghCore;
using BeghShare.Core.Services;
using BeghShare.UI.Attributes;
using BeghShare.UI.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BeghShare.UI.Pages
{
    /// <summary>
    /// Interaction logic for MonitorsPage.xaml
    /// </summary>
    [SideMenuItem(Title = "Monitors Page", Icon = "\xe163")]
    public partial class MonitorsPage : UserControl, ITransient
    {
        private MonitorsPageViewModel ViewModel;

        public MonitorsPage()
        {
            InitializeComponent();
            ViewModel = GetService<MonitorsPageViewModel>();
            DataContext = ViewModel;
        }

        private bool _isDragging;
        private Point _clickPosition;

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is BorderViewModel vm)
            {
                _isDragging = true;
                _clickPosition = e.GetPosition(element);
                element.CaptureMouse();
                ViewModel.StartDragging(vm);
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && sender is FrameworkElement element)
            {
                var canvas = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(element)) as UIElement;
                var currentPos = e.GetPosition(canvas);
                ViewModel.UpdatePosition(currentPos.X - _clickPosition.X, currentPos.Y - _clickPosition.Y);
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            ((FrameworkElement)sender).ReleaseMouseCapture();
            ViewModel.StopDragging();
        }
    }

    public partial class MonitorsPageViewModel : ObservableObject, ITransient
    {
        [ObservableProperty]
        private ObservableCollection<BorderViewModel> borders = [];

        private BorderViewModel? _currentDraggingBorder;

        public MonitorsPageViewModel()
        {
            var size = GetService<IScreenService>().GetResolution();
            Borders.Add(new() { Left = 50, Top = 70, Width = 150, Height = 100, TextInside = "Main_Monitor", Resolution = $"{size.Width}x{size.Height}", IsThisMonitor = true });
            //Borders.Add(new() { Left = 220, Top = 50, Width = 100, Height = 150, TextInside = "Other_Monitor" });
        }

        public void StartDragging(BorderViewModel border)
        {
            _currentDraggingBorder = border;
        }

        public void UpdatePosition(double x, double y)
        {
            if (_currentDraggingBorder != null)
            {
                double newLeft = x;
                double newTop = y;

                double validLeft = _currentDraggingBorder.Left;
                double validTop = _currentDraggingBorder.Top;

                // Try both axes
                if (!CheckCollision(newLeft, newTop, _currentDraggingBorder))
                {
                    validLeft = newLeft;
                    validTop = newTop;
                }
                else
                {
                    if (!CheckCollision(newLeft, validTop, _currentDraggingBorder))
                        validLeft = newLeft;
                    if (!CheckCollision(validLeft, newTop, _currentDraggingBorder))
                        validTop = newTop;
                }

                _currentDraggingBorder.Left = validLeft;
                _currentDraggingBorder.Top = validTop;
            }
        }

        public void StopDragging()
        {
            _currentDraggingBorder = null;
        }

        private bool CheckCollision(double left, double top, BorderViewModel currentBorder)
        {
            var rect1 = new Rect(left, top, currentBorder.Width, currentBorder.Height);

            foreach (var border in Borders)
            {
                if (border == currentBorder) continue;

                var rect2 = new Rect(border.Left, border.Top, border.Width, border.Height);
                if (rect1.IntersectsWith(rect2))
                    return true;
            }

            return false;
        }
    }
}