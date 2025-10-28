using LinoVative.Service.Core.EntityBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinoVative.Service.Core.Accountings
{
    public class Account : AuditableEntityUnderCompany
    {
        public string? AccountCode { get; set; }
        public string Name { get; set; } = default!;
        public Guid GroupId { get; set; }
        public COAGroup? Group { get; set; }
        public Guid? ParentAccountId { get; set; }
        public Account? Parent { get; set; }
        public List<Account> Childs { get; set; } = new();
    }
}
