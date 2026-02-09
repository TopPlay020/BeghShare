using BeghCore;
using BeghShare.Core.Services;

namespace BeghShare.View.Services
{
    public class ScreenService : IScreenService, ISingleton<IScreenService>
    {
        public (int Width, int Height) GetResolution()
        {
            var width = Screen.PrimaryScreen!.Bounds.Width;
            var height = Screen.PrimaryScreen!.Bounds.Height;
            return (width, height);
        }
    }
}
