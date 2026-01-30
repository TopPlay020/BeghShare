using BeghCore;

namespace BeghShare
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Core.CoreInit();
            Application.Run(Core.GetService<MainWindow>());
        }
    }
}
