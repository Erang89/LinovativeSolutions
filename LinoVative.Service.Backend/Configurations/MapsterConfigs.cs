using Linovative.Dto.MasterData.People;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Payments;
using LinoVative.Service.Core.People;
using LinoVative.Service.Core.Shifts;
using LinoVative.Service.Core.Sources;
using LinoVative.Service.Core.Warehoses;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.CompanyDtos;
using LinoVative.Shared.Dto.ItemDtos;
using LinoVative.Shared.Dto.MasterData.Outlets;
using LinoVative.Shared.Dto.MasterData.Payments;
using LinoVative.Shared.Dto.MasterData.Users;
using LinoVative.Shared.Dto.MasterData.Warehouses;
using LinoVative.Shared.Dto.OrderTypes;
using LinoVative.Shared.Dto.Outlets;
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

            // Mapping Outlets
            config.NewConfig<Outlet, OutletViewDto>();
            config.NewConfig<OutletArea, OutletAreaDto>();
            config.NewConfig<OutletBankNote, OutletBankNoteDto>();
            config.NewConfig<OutletItemCategory, OutletItemCategoryViewDto>();
            config.NewConfig<OutletItemGroup, OutletItemGroupViewDto>();
            config.NewConfig<OutletOrderType, OutletOrderTypeViewDto>();
            config.NewConfig<OutletPaymentMethod, OutletPaymentMethodViewDto>();
            config.NewConfig<OutletShift, OutletShiftViewDto>();
            config.NewConfig<OutletTable, OutletTableViewDto>();
            config.NewConfig<OutletUser, OutletUserViewDto>();

            // Mapping Order Types
            config.NewConfig<OrderType, OrderTypeViewDto>();
            
            
            // Mapping paymens
            config.NewConfig<BankNote, BankNoteDto>();
            config.NewConfig<PaymentMethod, PaymentMethodDto>();
            config.NewConfig<PaymentMethodGroup, PaymentMethodDto>();
            
            // Mapping People
            config.NewConfig<Person, PersonViewDto>();
            
            // Mapping shift
            config.NewConfig<Shift, ShiftViewDto>();

            // Mapping Users
            config.NewConfig<AppUser, UserViewDto>();

            // Mapping Warehouse
            config.NewConfig<Warehouse, WarehouseDto>();



        }
    }
}
