namespace LinoVative.Shared.Dto.Attributes
{

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class IgnoreEntityIDValidationAttribute : Attribute
    {
        public List<IgnoreProperty> IgnoreList { get; private set; } = new();
        public IgnoreEntityIDValidationAttribute()
        {
            
        }

        public IgnoreEntityIDValidationAttribute(string[] ignoreList)
        {
            foreach(var item in ignoreList)
            {
                var classAndProperties = item.Split(".");
                if (classAndProperties.Count() != 2)
                    throw new ArgumentException($"Invalid {nameof(ignoreList)}");

                IgnoreList.Add(new IgnoreProperty() { ClassName = classAndProperties[0], PropertyName = classAndProperties[1] });
            }
        }
    }

    public class IgnoreProperty
    {
        public string? ClassName { get; set; }
        public string? PropertyName { get; set; }
    }
}
