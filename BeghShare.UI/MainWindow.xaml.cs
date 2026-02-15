using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Events.UserInputEvents;
using BeghShare.UI.Attributes;
using BeghShare.UI.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace BeghShare.UI
{
    public partial class MainWindow : Window, IGUIAutoStart, ISingleton
    {
        private readonly MainWindowViewModel viewModel;
        public MainWindow()
        {
            viewModel = GetService<MainWindowViewModel>();
            DataContext = viewModel;
            InitializeComponent();
            Show();
        }
    }

    public partial class MainWindowViewModel : ObservableObject, ISingleton
    {
        [ObservableProperty]
        ObservableCollection<SideMenuItemViewModel> sideMenuItems = [];

        [ObservableProperty]
        object? currentPage;

        [ObservableProperty]
        int mouseX;
        [ObservableProperty]
        int mouseY;
        [ObservableProperty]
        string keyDownText = "None";

        public MainWindowViewModel()
        {
            var pages = GetAssemblyTypes()
                .Where(t => t.GetCustomAttributes(typeof(SideMenuItemAttribute), false).Length != 0)
                .Select(t => new { Type = t, Attr = t.GetCustomAttribute<SideMenuItemAttribute>()! })
                .OrderBy(x => x.Attr.Order);

            foreach (var page in pages)
                SideMenuItems.Add(new SideMenuItemViewModel()
                {
                    Title = page.Attr.Title,
                    Icon = page.Attr.Icon,
                    Order = page.Attr.Order,
                    PageType = page.Type,
                    Command = ChangePageCommand
                });

            OnChangePage(SideMenuItems.First().PageType);
        }
        [RelayCommand]
        public void OnChangePage(Type pageType)
        {
            CurrentPage = GetService(pageType);
        }
        [EventHandler]
        private void OnMouseMove(MouseMoveEvent e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MouseX = e.X;
                MouseY = e.Y;
            });

        }
        [EventHandler]
        private void OnKeyDownEvent(KeyPressedEvent e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                KeyDownText = e.keyCode.ToString();
            });
        }
    }
}