using LinoVative.Shared.Dto.ItemDtos;
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
            
            return modelBuilder.GetEdmModel();
        }
    }
}
