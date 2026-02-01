namespace BeghShare
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CoreInit();
            Application.Run(GetService<MainWindow>());
        }
    }
}
