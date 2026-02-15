using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BeghShare.UI.ViewModels
{
#pragma warning disable CS8618
    public partial class SideMenuItemViewModel : ObservableObject
    {
        [ObservableProperty]
        public string title;
        [ObservableProperty]
        public string icon;
        [ObservableProperty]
        public int order;
        [ObservableProperty]
        public Type pageType;
        [ObservableProperty]
        public IRelayCommand<Type> command;
    }
}
