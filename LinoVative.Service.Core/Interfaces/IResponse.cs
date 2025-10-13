namespace LinoVative.Service.Core.Interfaces
{
    public interface IResponse
    {
        public int IsOk { get; set; }
        public object? Data { get; set; }
        public int MyProperty { get; set; }
    }
}
