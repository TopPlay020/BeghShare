using BeghCore;
using BeghCore.Attributes;
using BeghShare.Attributes;
using BeghShare.Events;
using BeghShare.Models;
using BeghShare.Services;

namespace BeghShare.UI.Pages
{
    [SideMenuItem(Title = "Peers Discovery")]
    public partial class DiscoveryPage : UserControl, ITransient
    {
        private Dictionary<ListViewItem, PeerInfo> _peers = new();
        public DiscoveryPage()
        {
            InitializeComponent();
            foreach (var peer in Core.GetService<DiscoveryService>().Peers)
            {
                OnPeerFoundEvent(new PeerFoundEvent(peer));
            }
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
            Core.GetService<DiscoveryService>().Discover();
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
                Core.SendEvent(new SendPeerControlRequestEvent(targetPeer));
            }
        }
    }
}
