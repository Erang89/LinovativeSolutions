using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Companies
{
    public class CompanySetting : AuditableEntityUnderCompany
    {
        public bool RequiredProvinceForCustomerAddress { get; set; }
        public bool RequiredCityForCustomerAddress { get; set; }
        public bool RequiredPostalCodeForCustomerAddress { get; set; }
        public bool RequiredRegencyForCustomerAddress { get; set; }
    }
}
