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

        private Point _canvasClickPosition;
        private bool _isDraggingCanvas;

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle || (e.ChangedButton == MouseButton.Left && Keyboard.Modifiers == ModifierKeys.Control))
            {
                _isDraggingCanvas = true;
                _canvasClickPosition = e.GetPosition(sender as UIElement);
                ((UIElement)sender).CaptureMouse();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDraggingCanvas)
            {
                var currentPos = e.GetPosition(sender as UIElement);
                var offsetX = currentPos.X - _canvasClickPosition.X;
                var offsetY = currentPos.Y - _canvasClickPosition.Y;

                ViewModel.MoveAllBorders(offsetX, offsetY);
                _canvasClickPosition = currentPos;
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle || e.ChangedButton == MouseButton.Left)
            {
                _isDraggingCanvas = false;
                ((UIElement)sender).ReleaseMouseCapture();
            }
        }

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

        private void MainItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.AddMainBorder(MainItemsControl.ActualWidth, MainItemsControl.ActualHeight);
        }
    }

    public partial class MonitorsPageViewModel : ObservableObject, ITransient
    {
        [ObservableProperty]
        private ObservableCollection<BorderViewModel> borders = [];

        [ObservableProperty]
        private bool snapToEdges = true;

        [ObservableProperty]
        private bool snapToCenters = true;

        private const double SnapThreshold = 10; // pixels

        private BorderViewModel? _currentDraggingBorder;

        private double _canvasWidth;
        private double _canvasHeight;

        public void AddMainBorder(double CanvasWidth, double CanvasHeight)
        {
            _canvasWidth = CanvasWidth;
            _canvasHeight = CanvasHeight;
            var (Width, Height) = GetService<IScreenService>().GetResolution();
            var renderedWidth = Width / 12;
            var renderedHeight = Height / 12;
            Borders.Add(new()
            {
                Left = (_canvasWidth - renderedWidth) / 2,
                Top = (_canvasHeight - renderedHeight) / 2,
                Width = renderedWidth,
                Height = renderedHeight,
                TextInside = "Main_Monitor",
                Resolution = $"{Width}x{Height}",
                IsThisMonitor = true
            });

            AddBorder(1920, 1080);
            AddBorder(1920, 1080);
            AddBorder(1920, 1080);
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

                if (SnapToEdges || SnapToCenters)
                {
                    (newLeft, newTop) = SnapToNearbyBorders(newLeft, newTop, _currentDraggingBorder);
                }

                double validLeft = _currentDraggingBorder.Left;
                double validTop = _currentDraggingBorder.Top;

                if (!CheckCollision(newLeft, newTop, _currentDraggingBorder) && IsConnected(newLeft, newTop, _currentDraggingBorder))
                {
                    validLeft = newLeft;
                    validTop = newTop;
                }
                else
                {
                    if (!CheckCollision(newLeft, validTop, _currentDraggingBorder) && IsConnected(newLeft, validTop, _currentDraggingBorder))
                        validLeft = newLeft;
                    if (!CheckCollision(validLeft, newTop, _currentDraggingBorder) && IsConnected(validLeft, newTop, _currentDraggingBorder))
                        validTop = newTop;
                }

                _currentDraggingBorder.Left = validLeft;
                _currentDraggingBorder.Top = validTop;
            }
        }

        private bool IsConnected(double left, double top, BorderViewModel current)
        {
            if (Borders.Count <= 1) return true;

            var testRect = new Rect(left, top, current.Width, current.Height);

            foreach (var border in Borders)
            {
                if (border == current) continue;

                var borderRect = new Rect(border.Left, border.Top, border.Width, border.Height);

                // Check if touching (edges align)
                bool touchingHorizontal = Math.Abs(testRect.Right - borderRect.Left) < 1 || Math.Abs(testRect.Left - borderRect.Right) < 1;
                bool touchingVertical = Math.Abs(testRect.Bottom - borderRect.Top) < 1 || Math.Abs(testRect.Top - borderRect.Bottom) < 1;
                bool overlapX = testRect.Left < borderRect.Right && testRect.Right > borderRect.Left;
                bool overlapY = testRect.Top < borderRect.Bottom && testRect.Bottom > borderRect.Top;

                if ((touchingHorizontal && overlapY) || (touchingVertical && overlapX))
                    return true;
            }

            return false;
        }

        private (double left, double top) SnapToNearbyBorders(double left, double top, BorderViewModel current)
        {
            double snappedLeft = left;
            double snappedTop = top;
            double minDistX = double.MaxValue;
            double minDistY = double.MaxValue;

            foreach (var border in Borders)
            {
                if (border == current) continue;

                if (SnapToEdges)
                {
                    // Snap to right edge
                    double distRight = Math.Abs(left - (border.Left + border.Width));
                    if (distRight < SnapThreshold && distRight < minDistX)
                    {
                        snappedLeft = border.Left + border.Width;
                        minDistX = distRight;
                    }

                    // Snap to left edge
                    double distLeft = Math.Abs((left + current.Width) - border.Left);
                    if (distLeft < SnapThreshold && distLeft < minDistX)
                    {
                        snappedLeft = border.Left - current.Width;
                        minDistX = distLeft;
                    }

                    // Snap to bottom edge
                    double distBottom = Math.Abs(top - (border.Top + border.Height));
                    if (distBottom < SnapThreshold && distBottom < minDistY)
                    {
                        snappedTop = border.Top + border.Height;
                        minDistY = distBottom;
                    }

                    // Snap to top edge
                    double distTop = Math.Abs((top + current.Height) - border.Top);
                    if (distTop < SnapThreshold && distTop < minDistY)
                    {
                        snappedTop = border.Top - current.Height;
                        minDistY = distTop;
                    }
                }

                if (SnapToCenters)
                {
                    double currentCenterX = left + current.Width / 2;
                    double currentCenterY = top + current.Height / 2;
                    double borderCenterX = border.Left + border.Width / 2;
                    double borderCenterY = border.Top + border.Height / 2;

                    // Snap center X
                    double distCenterX = Math.Abs(currentCenterX - borderCenterX);
                    if (distCenterX < SnapThreshold && distCenterX < minDistX)
                    {
                        snappedLeft = borderCenterX - current.Width / 2;
                        minDistX = distCenterX;
                    }

                    // Snap center Y
                    double distCenterY = Math.Abs(currentCenterY - borderCenterY);
                    if (distCenterY < SnapThreshold && distCenterY < minDistY)
                    {
                        snappedTop = borderCenterY - current.Height / 2;
                        minDistY = distCenterY;
                    }
                }
            }

            return (snappedLeft, snappedTop);
        }

        public void StopDragging()
        {
            _currentDraggingBorder = null;
        }

        public void AddBorder(int width, int height)
        {
            var renderedWidth = width / 12.0;
            var renderedHeight = height / 12.0;

            var (left, top) = FindValidPosition(renderedWidth, renderedHeight);

            Borders.Add(new()
            {
                Left = left,
                Top = top,
                Width = renderedWidth,
                Height = renderedHeight,
                TextInside = $"Monitor_{Borders.Count + 1}",
                Resolution = $"{width}x{height}",
                IsThisMonitor = false
            });
        }

        private (double left, double top) FindValidPosition(double width, double height)
        {
            if (Borders.Count == 0)
            {
                return ((_canvasWidth - width) / 2, (_canvasHeight - height) / 2);
            }

            // Try positions directly adjacent to each existing border
            foreach (var border in Borders)
            {
                var positions = new[]
                {
                    (border.Left + border.Width, border.Top), // Right
                    (border.Left - width, border.Top), // Left
                    (border.Left, border.Top - height), // Top
                    (border.Left, border.Top + border.Height) // Bottom
                };

                foreach (var (x, y) in positions)
                {
                    if (x >= 0 && y >= 0 && x + width <= _canvasWidth && y + height <= _canvasHeight)
                    {
                        if (!CheckCollision(x, y, width, height))
                            return (x, y);
                    }
                }
            }

            // If no position found, place to the right of the last border
            var lastBorder = Borders[^1];
            return (lastBorder.Left + lastBorder.Width, lastBorder.Top);
        }

        private bool CheckCollision(double left, double top, double width, double height)
        {
            var rect1 = new Rect(left, top, width, height);
            rect1.Inflate(-1, -1);

            foreach (var border in Borders)
            {
                var rect2 = new Rect(border.Left, border.Top, border.Width, border.Height);
                rect2.Inflate(-1, -1);
                if (rect1.IntersectsWith(rect2))
                    return true;
            }

            return false;
        }

        private bool CheckCollision(double left, double top, BorderViewModel currentBorder)
        {
            var rect1 = new Rect(left, top, currentBorder.Width, currentBorder.Height);

            foreach (var border in Borders)
            {
                if (border == currentBorder) continue;

                var rect2 = new Rect(border.Left, border.Top, border.Width, border.Height);

                // Allow touching - only block if truly overlapping (with 1px tolerance)
                rect1.Inflate(-1, -1);
                rect2.Inflate(-1, -1);

                if (rect1.IntersectsWith(rect2))
                    return true;
            }

            return false;
        }

        public void MoveAllBorders(double offsetX, double offsetY)
        {
            foreach (var border in Borders)
            {
                border.Left += offsetX;
                border.Top += offsetY;
            }
        }
    }
}