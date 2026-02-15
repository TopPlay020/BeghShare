using BeghCore;
using BeghShare.Core.Services;
using System.Runtime.InteropServices;

namespace BeghShare.UI.Services
{
    public class ScreenService : IScreenService,ISingleton<IScreenService>
    {
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        public (int Width, int Height) GetResolution()
        {
            int screenWidth = GetSystemMetrics(0);  // SM_CXSCREEN
            int screenHeight = GetSystemMetrics(1); // SM_CYSCREEN
            return (screenWidth,screenHeight);
        }

    }
}
