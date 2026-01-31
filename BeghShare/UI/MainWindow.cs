using BeghCore;
using BeghCore.Attributes;
using BeghShare.Attributes;
using BeghShare.Events;
using System.Reflection;
namespace BeghShare
{
    public partial class MainWindow : Form, ISingleton, IGUIAutoStart
    {
        private Dictionary<TreeNode, Type> SideMenuClickMap = default!;
        public MainWindow()
        {
            InitializeComponent();
            InitUi();
            Show();
        }

        private void InitUi()
        {
            var nodes = SideMenu.Nodes;
            SideMenuClickMap = new Dictionary<TreeNode, Type>();
            foreach (var page in Core.GetAssemblyTypes().Where(t => t.GetCustomAttributes(typeof(SideMenuItemAttribute), false).Any()))
            {
                var attr = page.GetCustomAttribute<SideMenuItemAttribute>()!;
                var node = nodes.Add(attr.Title);
                SideMenuClickMap[node] = page;
            }
        }
        private void SideMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MainFrame.Controls.Clear();
            var newPage = (UserControl)Core.GetService(SideMenuClickMap[e.Node!]);
            newPage.Dock = DockStyle.Fill;
            MainFrame.Controls.Add(newPage);
        }

        [EventHandler]
        private void OnMouseMove(MouseMoveEvent e)
        {
            if (MouseX.InvokeRequired)
                MouseX.Invoke(() => MouseX.Text = e.X.ToString());
            else
                MouseX.Text = e.X.ToString();
            if (MouseY.InvokeRequired)
                MouseY.Invoke(() => MouseY.Text = e.Y.ToString());
            else
                MouseY.Text = e.Y.ToString();
        }
        [EventHandler]
        private void OnKeyDownEvent(KeyDownEvent e)
        {
            if (KeyDownText.InvokeRequired)
                KeyDownText.Invoke(() => KeyDownText.Text = e.keyCode.ToString());
            else
                KeyDownText.Text = e.keyCode.ToString();
        }
        [EventHandler]
        private void OnOnlineComputerCountChangedEvent(OnlineComputerCountChangedEvent e)
        {
            if (OnlineText.InvokeRequired)
                OnlineText.Invoke(() => OnlineText.Text = e.Count.ToString());
            else
                OnlineText.Text = e.Count.ToString();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Core.SendEvent(new MainWindowCloseEvent());
        }
    }
}
