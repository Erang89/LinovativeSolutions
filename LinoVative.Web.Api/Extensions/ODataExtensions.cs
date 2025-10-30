using Linovative.Dto.MasterData.People;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto.ItemDtos;
using LinoVative.Shared.Dto.MasterData.Accountings;
using LinoVative.Shared.Dto.MasterData.Outlets;
using LinoVative.Shared.Dto.MasterData.Payments;
using LinoVative.Shared.Dto.MasterData.Warehouses;
using LinoVative.Shared.Dto.OrderTypes;
using LinoVative.Shared.Dto.Outlets;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace LinoVative.Web.Api.Extensions
{
    public static class ODataExtensions
    {
        public static IMvcBuilder ConfigureOData(this IMvcBuilder builder, IServiceCollection services)
        {
            builder.AddOData(option => {
                option.Select()
                    .Filter()
                    .Count()
                    .OrderBy()
                    .Expand()
                    .SetMaxTop(100)
                    .EnableQueryFeatures()
                    .AddRouteComponents("api/odata", GetEdmModel(), serviceProvider =>
                    {
                        
                    });
            });

            return builder;
        }


        static IEdmModel GetEdmModel()
        {

            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EnableLowerCamelCase();


            modelBuilder.EntitySet<ItemUnitDto>("ItemUnits");
            modelBuilder.EntitySet<ItemGroupDto>("ItemGroups");
            modelBuilder.EntitySet<ItemCategoryViewDto>("ItemCategories");
            modelBuilder.EntitySet<OutletViewDto>("Outlets");

            var setShift = modelBuilder.EntitySet<ShiftViewDto>("Shifts");
            var shiftEntity = setShift.EntityType;
            shiftEntity.Ignore(s => s.StartTime);
            shiftEntity.Ignore(s => s.EndTime);
            shiftEntity.Property(s => s.StartTimeFormatted).Name = nameof(ShiftDto.StartTime);
            shiftEntity.Property(s => s.EndTimeFormatted).Name = nameof(ShiftDto.EndTime);


            modelBuilder.EntitySet<PersonViewDto>("People");
            modelBuilder.EntitySet<PaymentMethodGroupViewDto>("PaymentMethodGroups");
            modelBuilder.EntitySet<PaymentMethodViewDto>("PaymentMethods");
            modelBuilder.EntitySet<BankNoteDto>("BankNotes");
            modelBuilder.EntitySet<OrderTypeViewDto>("OrderTypes");
            modelBuilder.EntitySet<WarehouseDto>("Warehouses");
            modelBuilder.EntitySet<COAGroupDto>("COAGroups");
            modelBuilder.EntitySet<AccountViewDto>("Accounts");

            return modelBuilder.GetEdmModel();
        }
    }
}
