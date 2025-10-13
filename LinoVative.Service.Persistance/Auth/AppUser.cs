namespace LinoVative.Service.Persistance.Auth
{
    public class AppUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool HasConfirmed { get; set; }
    }
}
