using Linovative.Dto.MasterData.People;
using LinoVative.Shared.Dto.BulkUploads;
using LinoVative.Shared.Dto.ItemDtos;
using LinoVative.Shared.Dto.MasterData.Accountings;
using LinoVative.Shared.Dto.MasterData.Payments;
using LinoVative.Shared.Dto.MasterData.Shifts;
using LinoVative.Shared.Dto.MasterData.Warehouses;
using LinoVative.Shared.Dto.OrderTypes;
using LinoVative.Shared.Dto.Outlets;
using LinoVative.Shared.Dto.Sources;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace LinoVative.Web.Api.Extensions
{
    public static class ODataExtensions
    {
        public static IMvcBuilder ConfigurePrivateOData(this IMvcBuilder builder, IServiceCollection services)
        {
            builder.AddOData(option => {
                option.Select()
                    .Filter()
                    .Count()
                    .OrderBy()
                    .Expand()
                    .SetMaxTop(100)
                    .EnableQueryFeatures()
                    .AddRouteComponents("api/v1/odata", GetPrivateEdmModel(), serviceProvider =>
                    {
                        
                    });
            });

            return builder;
        }


        static IEdmModel GetPrivateEdmModel()
        {

            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EnableLowerCamelCase();

            // Master Datas
            modelBuilder.EntitySet<ItemUnitDto>("ItemUnits");
            modelBuilder.EntitySet<PriceTypeDto>("PriceTypes");
            modelBuilder.EntitySet<ItemGroupDto>("ItemGroups");
            modelBuilder.EntitySet<ItemCategoryViewDto>("ItemCategories");
            modelBuilder.EntitySet<ItemViewDto>("Items");
            modelBuilder.EntitySet<OutletViewDto>("Outlets");
            modelBuilder.EntitySet<OutletAreaViewDto>("OutletAreas");
            modelBuilder.EntitySet<ShiftViewDto>("Shifts");
            modelBuilder.EntitySet<PersonViewDto>("People");
            modelBuilder.EntitySet<PaymentMethodGroupViewDto>("PaymentMethodGroups");
            modelBuilder.EntitySet<PaymentMethodViewDto>("PaymentMethods");
            modelBuilder.EntitySet<BankNoteDto>("BankNotes");
            modelBuilder.EntitySet<OrderTypeViewDto>("OrderTypes");
            modelBuilder.EntitySet<WarehouseDto>("Warehouses");
            modelBuilder.EntitySet<COAGroupDto>("COAGroups");
            modelBuilder.EntitySet<AccountViewDto>("Accounts");
            modelBuilder.EntitySet<SalesCOAMappingViewDto>("SalesCOAMappings");

            // Bulk Uploads Data
            modelBuilder.EntitySet<BulkUploadItemGroupDto>("BulkOperationItemGroups");
            modelBuilder.EntitySet<BulkUploadItemGroupDetailDto>("BulkOperationItemGroupDetails");
            modelBuilder.EntitySet<BulkUploadItemCategoryDto>("BulkOperationItemCategories");
            modelBuilder.EntitySet<BulkUploadItemCategoryDetailDto>("BulkOperationItemCategoryDetails");
            modelBuilder.EntitySet<BulkUploadItemUnitDto>("BulkOperationItemUnits");
            modelBuilder.EntitySet<BulkUploadItemUnitDetailDto>("BulkOperationItemUnitDetails");
            modelBuilder.EntitySet<BulkUploadItemDto>("BulkOperationItems");
            modelBuilder.EntitySet<BulkUploadItemDetailDto>("BulkOperationItemDetails");

            

            return modelBuilder.GetEdmModel();
        }

        public static IMvcBuilder ConfigurePublicOData(this IMvcBuilder builder, IServiceCollection services)
        {
            builder.AddOData(option => {
                option.Select()
                    .Filter()
                    .Count()
                    .OrderBy()
                    .Expand()
                    .SetMaxTop(100)
                    .EnableQueryFeatures()
                    .AddRouteComponents("public/api/v1/odata", GetPublicteEdmModel(), serviceProvider =>
                    {

                    });
            });

            return builder;
        }

        static IEdmModel GetPublicteEdmModel()
        {

            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EnableLowerCamelCase();

            modelBuilder.EntitySet<CurrencyDto>("Currencies");
            modelBuilder.EntitySet<CountryDto>("Countries");
            modelBuilder.EntitySet<TimezoneDto>("Timezones");

            return modelBuilder.GetEdmModel();
        }
    }
}
