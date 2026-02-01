using SharpHook.Data;
namespace BeghShare.Core.Events.UserInputEvents
{
    public record MouseReleasedEvent
    {
        public MouseButton Button { get; set; }
    }
}
