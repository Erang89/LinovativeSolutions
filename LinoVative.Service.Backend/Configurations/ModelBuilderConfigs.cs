using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.Sources;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.Configurations
{
    internal static class ModelBuilderConfigs
    {
        public static void ConfigureDatabaseRelationship(this ModelBuilder modelBuilder)
        {
            // Mapping Auth
            modelBuilder.Entity<AppUser>(x =>
            {
                x.ToTable("Users");
            });
            modelBuilder.Entity<AppUserApplication>().ToTable("UserApplications");
            modelBuilder.Entity<AppUserCompany>().ToTable("UserCompanies");


            // Maping Items
            modelBuilder.Entity<ItemUnit>().ToTable("ItemUnits");
            modelBuilder.Entity<ItemGroup>().ToTable("ItemGroups");
            modelBuilder.Entity<CostumePriceTag>().ToTable("ItemPriceTags");

            modelBuilder.Entity<ItemCategory>(x =>
            {
                x.ToTable("ItemCategories");
                x.HasOne(x => x.Group).WithMany(x => x.Categories).HasForeignKey(x => x.GroupId).IsRequired();
            });

            modelBuilder.Entity<Item>(x =>
            {
                x.ToTable("Items");

            });

            modelBuilder.Entity<ItemCostumePrice>(x =>
            {
                x.ToTable("ItemCustomePrices");
                x.HasOne(x => x.CostumePriceTag).WithMany().HasForeignKey(x => x.CostumePriceTagId).IsRequired();
                x.HasOne(x => x.Item).WithMany(x => x.CostumePrices).HasForeignKey(x => x.ItemId).IsRequired();
            });


            // Mapping Sources
            modelBuilder.Entity<Country>(x => 
            { 
                x.ToTable("Countries");
                x.HasOne(x => x.Region).WithMany(x => x.Countries).HasPrincipalKey(x => x.Id);
            });
            modelBuilder.Entity<CountryRegion>().ToTable("CountryRegions");
            modelBuilder.Entity<Currency>().ToTable("Currencies");
            modelBuilder.Entity<AppTimeZone>().ToTable("TimeZones");

        }



    }
}
