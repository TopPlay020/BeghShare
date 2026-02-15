using BeghShare.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
namespace BeghShare.UI.ViewModels
{
#pragma warning disable CS8618
    public partial class PeerTableItemViewModel : ObservableObject
    {
        [ObservableProperty]
        public string iP;
        [ObservableProperty]
        public string name;

        public PeerInfo peerInfo;
    }
}
