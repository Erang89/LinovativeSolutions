namespace LinoVative.Service.Core.Interfaces
{
    public interface IActor
    {
        public Guid UserId { get; }
        public string EmailAddress { get; } 
        public Guid? CompanyId { get; }
        public List<Guid> PrivilegesId { get;}
        public string? TimeZone { get; set; }

    }

    public interface ISystemAdministrator : IActor { }
}
