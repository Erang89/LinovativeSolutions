using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.CompanyDtos;
using LinoVative.Shared.Dto.ItemDtos;
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


            // Items
            config.NewConfig<ItemUnit, ItemUnitDto>();
            config.NewConfig<ItemGroup, ItemGroupDto>();
            config.NewConfig<ItemCategory, ItemCategoryDto>();
            config.NewConfig<ItemCategory, ItemCategoryViewDto>();

        }
    }
}
