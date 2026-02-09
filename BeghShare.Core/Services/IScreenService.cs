namespace BeghShare.Core.Services
{
    public interface IScreenService
    {
        (int Width, int Height) GetResolution();
    }
}
