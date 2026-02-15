using System.Windows;

namespace BeghShare.UI.Dialog
{

    public partial class YesNoDialog : Window
    {
        public string Message { get; set; }
        public string DialogTitle { get; set; }
        public YesNoDialog(string title, string message)
        {
            InitializeComponent();
            Owner = GetService<MainWindow>();
            Message = message;
            DialogTitle = title;
            DataContext = this;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}
