using BeghCore;
using BeghShare.Core.Services;
using BeghShare.UI.Dialog;
using System.Windows;

namespace BeghShare.UI.Services
{
    public class YesNoQustionService : IYesNoQustionService, ISingleton<IYesNoQustionService>
    {
        public bool Handle(string requestTitle, string requestBody)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var dialog = new YesNoDialog(requestTitle, requestBody);
                return dialog.ShowDialog() == true;
            });
        }

    }
}
