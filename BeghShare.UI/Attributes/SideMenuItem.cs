namespace BeghShare.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SideMenuItemAttribute : Attribute
    {
        public required string Title { get; set; }
        public required string Icon { get; set; }
        public int Order { get; set; } = 0;
    }
}
