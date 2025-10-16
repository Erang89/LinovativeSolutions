using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.CompanyDtos;
using Mapster;

namespace LinoVative.Service.Backend.Configurations
{
    public class MapsterConfigs : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // IdCode Mapping
            config.NewConfig<Currency, IdWithCodeDto>();
            config.NewConfig<Country, IdWithCodeDto>();
            config.NewConfig<AppTimeZone, IdWithCodeDto>()
                .Map(des => des.Code, src => src.TimeZone)
                .Map(des => des.Name, src => src.Name);


            // Companies Mapping
            config.NewConfig<Company, CompanyDto>();

        }
    }
}
