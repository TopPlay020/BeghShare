
using System.Windows.Forms;

namespace BeghShare.Events
{
    public class KeyDownEvent
    {
        public KeyEventArgs e;
        public KeyDownEvent(KeyEventArgs e)
        {
            this.e = e;
        }
    }
}
