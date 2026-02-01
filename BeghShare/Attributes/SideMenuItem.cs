namespace BeghShare.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SideMenuItemAttribute : Attribute
    {
        public required string Title { get; set; }
    }
}
