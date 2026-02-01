using SharpHook.Data;
namespace BeghShare.Core.Events.UserInputEvents
{
    public record MousePressedEvent
    {
        public MouseButton Button { get; set; }
    }
}
