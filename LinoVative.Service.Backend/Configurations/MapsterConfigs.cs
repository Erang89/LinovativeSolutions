using Linovative.Dto.MasterData.People;
using LinoVative.Service.Backend.CrudServices.Items.Items;
using LinoVative.Service.Backend.CrudServices.Outlets.Outlets;
using LinoVative.Service.Core.Accountings;
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
using LinoVative.Shared.Dto.MasterData.Accountings;
using LinoVative.Shared.Dto.MasterData.Payments;
using LinoVative.Shared.Dto.MasterData.Shifts;
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
            config.NewConfig<CreateItemCommand, Item>();
            config.NewConfig<UpdateItemCommand, Item>();
            config.NewConfig<Item, ItemViewDto>();

            config.NewConfig<ItemUnit, ItemUnitDto>();
            config.NewConfig<ItemUnit, IdWithNameDto>();

            config.NewConfig<ItemGroup, ItemGroupDto>();
            config.NewConfig<ItemGroup, IdWithNameDto>();
            
            config.NewConfig<ItemCategory, ItemCategoryDto>();
            config.NewConfig<ItemCategory, IdWithNameDto>();
            config.NewConfig<ItemCategory, ItemCategoryViewDto>()
                .Map(x => x.ItemGroup, src => src.Group);

            // Mapping Outlets
            config.NewConfig<Outlet, OutletViewDto>();
            config.NewConfig<UpdateOutletCommand, Outlet>()
                .Ignore(x => x.Shifts)
                .Ignore(x => x.BankNotes)
                .Ignore(x => x.PaymentMethods)
                .Ignore(x => x.OrderTypes);

            config.NewConfig<CreateOutletCommand, Outlet>()
                .Ignore(x => x.Shifts)
                .Ignore(x => x.BankNotes)
                .Ignore(x => x.PaymentMethods)
                .Ignore(x => x.OrderTypes);

            config.NewConfig<OutletAreaCreateDto, OutletArea>()
                .Ignore(x => x.Tables);
            config.NewConfig<OutletArea, OutletAreaDto>();
            config.NewConfig<OutletBankNote, OutletBankNoteDto>();
            config.NewConfig<OutletItemCategory, OutletItemCategoryViewDto>();
            config.NewConfig<OutletItemGroup, OutletItemGroupViewDto>();
            config.NewConfig<OutletOrderType, OutletOrderTypeViewDto>();
            config.NewConfig<OutletPaymentMethod, OutletPaymentMethodViewDto>();
            config.NewConfig<OutletShift, OutletShiftViewDto>();
            config.NewConfig<OutletTable, OutletTableViewDto>();
            config.NewConfig<OutletUser, OutletUserViewDto>();
            config.NewConfig<OutletTableDto, OutletTable>();

            // Mapping Order Types
            config.NewConfig<OrderType, OrderTypeViewDto>();
            
            
            // Mapping paymens
            config.NewConfig<BankNote, BankNoteDto>();
            config.NewConfig<PaymentMethod, PaymentMethodViewDto>()
                .Map(x => x.PaymentMethodGroup, src => src.PaymentMethodGroup);
            config.NewConfig<PaymentMethodGroup, PaymentMethodGroupViewDto>();
            
            // Mapping People
            config.NewConfig<Person, PersonViewDto>();
            
            // Mapping shift
            config.NewConfig<Shift, ShiftViewDto>();

            // Mapping Users
            config.NewConfig<AppUser, UserViewDto>();

            // Mapping Warehouse
            config.NewConfig<Warehouse, WarehouseDto>();


            // Mapping COA
            config.NewConfig<Account, AccountViewDto>()
                .Map(x => x.Group, src => src.Group);

            config.NewConfig<COAGroup, COAGroupDto>();


        }
    }
}
