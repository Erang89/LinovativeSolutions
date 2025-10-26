namespace LinoVative.Shared.Dto.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }

        public EnumDisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
