namespace LinoVative.Shared.Dto.Attributes
{

    [AttributeUsage(AttributeTargets.All | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LocalizerKeyAttribute : Attribute
    {
        public string Key { get; set; }
        public LocalizerKeyAttribute(string key)
        {
            Key = key;
        }
    }
}
