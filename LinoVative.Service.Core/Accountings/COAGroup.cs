using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Service.Core.Accountings
{
    public class COAGroup : AuditableEntityUnderCompany
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public COATypes? Type { get; set; }
    }
}
