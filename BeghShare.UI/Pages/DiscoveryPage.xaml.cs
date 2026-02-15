using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Events.MessageEvents;
using BeghShare.Core.Services;
using BeghShare.UI.Attributes;
using BeghShare.UI.Services;
using BeghShare.UI.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BeghShare.UI.Pages
{
    [SideMenuItem(Title = "Discovery Page", Icon = "\xf002")]
    public partial class DiscoveryPage : UserControl, ITransient
    {
        DiscoveryPageViewModel viewModel;
        public DiscoveryPage()
        {
            viewModel = GetService<DiscoveryPageViewModel>();
            DataContext = viewModel;
            InitializeComponent();
        }
    }

    public partial class DiscoveryPageViewModel : ObservableObject, ITransient
    {
        [ObservableProperty]
        public string? searchText;

        [ObservableProperty]
        public ObservableCollection<PeerTableItemViewModel> peerTableItems = [];

        [ObservableProperty]
        private bool canDiscover = true;

        public DiscoveryPageViewModel()
        {
            foreach (var peer in GetService<DiscoveryService>().GetPeerInfos())
            {
                PeerTableItems.Add(new PeerTableItemViewModel()
                {
                    Name = peer.Name,
                    IP = peer.IP.MapToIPv4().ToString(),
                    peerInfo = peer
                });
            }
        }

        [EventHandler]
        private void OnPeerFoundEvent(PeerFoundEvent e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var peer = e.PeerInfo;
                PeerTableItems.Add(new PeerTableItemViewModel()
                {
                    Name = peer.Name,
                    IP = peer.IP.MapToIPv4().ToString(),
                    peerInfo = peer
                });
            });
        }
        [RelayCommand]
        private void OnDiscovery()
        {
            CanDiscover = false;
            GetService<DiscoveryService>().Discover();
            CanDiscover = true;
        }
        [RelayCommand]
        private void OnSearch()
        {
            var ipAddressToSearch = SearchText?.Trim();
            if (!GetService<DiscoveryService>().Discover(ipAddressToSearch))
                MessageBox.Show("Invalid IP address.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        [RelayCommand]
        private void OnConnect(PeerTableItemViewModel peer)
        {
            var result = GetService<IYesNoQustionService>().Handle("Confirm", $"Do you want to control {peer.Name}:{peer.IP} ?");
            if (result)
                SendEvent(new SendPeerControlRequestEvent(peer.peerInfo));
        }
    }
}
