using BeghCore;
using BeghCore.Attributes;
using BeghShare.Attributes;
using BeghShare.Core.Events.MessageEvents;
using BeghShare.Core.Models;
using BeghShare.Core.Services;

namespace BeghShare.UI.Pages
{
    [SideMenuItem(Title = "Peers Discovery")]
    public partial class DiscoveryPage : UserControl, ITransient
    {
        private readonly Dictionary<ListViewItem, PeerInfo> _peers = [];
        public DiscoveryPage()
        {
            InitializeComponent();
            foreach (var peer in GetService<DiscoveryService>().GetPeerInfos())
                OnPeerFoundEvent(new PeerFoundEvent(peer));
        }
        [EventHandler]
        private void OnPeerFoundEvent(PeerFoundEvent e)
        {
            //Ui Change
            if (PeersList.InvokeRequired)
            {
                PeersList.Invoke(new Action(() => OnPeerFoundEvent(e)));
                return;
            }

            var listItem = PeersList.Items.Add(e.PeerInfo.Name);
            listItem.SubItems.Add(e.PeerInfo.IP.ToString());
            listItem.SubItems.Add(e.PeerInfo.IsOnline ? "Online" : "Offline");
            _peers[listItem] = e.PeerInfo;
        }

        private async void DiscoveryClick(object sender, EventArgs e)
        {
            DiscoveryButton.Enabled = false;
            GetService<DiscoveryService>().Discover();
            DiscoveryButton.Enabled = true;
        }

        private void PeerItemDoubleClick(object sender, MouseEventArgs e)
        {
            var targetPeer = _peers[PeersList.SelectedItems[0]];
            var result = MessageBox.Show(
                $"Do you want to control {targetPeer.Name}:{targetPeer.IP} ?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SendEvent(new SendPeerControlRequestEvent(targetPeer));
            }
        }

        private void SearchClick(object sender, EventArgs e)
        {
            var ipAddressToSearch = SearchText.Text.Trim();
            if (!GetService<DiscoveryService>().Discover(ipAddressToSearch))
                MessageBox.Show("Invalid IP address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
