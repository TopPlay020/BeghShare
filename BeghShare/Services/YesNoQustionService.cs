using BeghCore;
using BeghShare.Core.Events;
using BeghShare.Core.Services;

namespace BeghShare.View.Services
{
    public class YesNoQustionService : IYesNoQustionService, ISingleton<IYesNoQustionService>
    {
        public bool Handle(string requestTitle, string requestBody)
        {
            var mainWindow = GetService<MainWindow>();
            bool result = false;

            mainWindow.Invoke(() =>
            {
                result = MessageBox.Show(
                    mainWindow,
                    requestBody,
                    requestTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                ) == DialogResult.Yes;
            });

            return result;
        }

    }
}
