using Linovative.Shared.Interface;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Service.Core.Auth
{
    public class RefreshToken : IEntityId
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string IPAddressLogin { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public Guid? Token { get; set; }
    }
}
