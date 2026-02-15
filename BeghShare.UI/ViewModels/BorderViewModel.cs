using CommunityToolkit.Mvvm.ComponentModel;

namespace BeghShare.UI.ViewModels
{
    public partial class BorderViewModel : ObservableObject
    {
        [ObservableProperty]
        public double left;
        [ObservableProperty]
        public double top;
        [ObservableProperty]
        public double width;
        [ObservableProperty]
        public double height;
        [ObservableProperty]
        public string textInside = string.Empty;

        [ObservableProperty]
        public bool isThisMonitor = false;
        [ObservableProperty]
        public string resolution = "Unknow";
    }
}
