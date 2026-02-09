using BeghCore;
using BeghShare.Attributes;
namespace BeghShare.View.Pages
{
    [SideMenuItem(Title = "Monitor Manager", Order = 2)]
    public partial class MonitorManagerPage : UserControl, ITransient
    {
        public MonitorManagerPage()
        {
            InitializeComponent();
        }
    }
}
