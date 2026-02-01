using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Events;

namespace BeghShare.View.Services
{
    public class YesNoQustionHandler : ISingleton, IAutoStart
    {
        [EventHandler]
        public async void OnYesNoQustionRequestEvent(YesNoQustionRequestEvent e)
        {
            var mainWindow = GetService<MainWindow>();
            mainWindow.Invoke(() =>
            {
                var result = MessageBox.Show(
                mainWindow,
                e.RequestBody,
                e.RequestTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                SendEvent(new YesNoQustionResponseEvent()
                {
                    RequestId = e.RequestId,
                    Response = result == DialogResult.Yes
                });
            });
        }
    }
}
