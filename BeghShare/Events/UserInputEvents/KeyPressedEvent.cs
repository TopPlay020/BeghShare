using SharpHook;
using SharpHook.Data;
using System.Windows.Forms;

namespace BeghShare.Events.UserInputEvents
{
    public class KeyPressedEvent
    {
        public required KeyCode keyCode { get; init; }
    }
}
