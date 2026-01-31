
using SharpHook;
using SharpHook.Data;
using System.Windows.Forms;

namespace BeghShare.Events
{
    public class KeyDownEvent
    {
        public required KeyCode keyCode { get; init; }
    }
}
