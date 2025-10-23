using Linovative.Shared.Interface;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.Interfaces
{
    public interface IAppDbContext
    {


        // Auth DbSets
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppUserCompany> UserCompanies { get; set; }
        public DbSet<AppUserApplication> UserApplications { get; set; }


        // Source DbSets
        public DbSet<CountryRegion> CountryRegions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<AppTimeZone> TimeZones { get; set; }
        public DbSet<Currency> Currencies { get; set; }


        // Items DbSet
        public DbSet<Item> Items{ get; set; }
        public DbSet<ItemUnit> ItemUnits { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<CostumePriceTag> ItemPriceTags { get; set; }
        public DbSet<ItemCostumePrice> ItemCostumePrices { get; set; }


        // Companies DbSet
        public DbSet<Company> Companies { get; set; }

        public DbSet<T> Set<T>() where T : class;
        public Task<Result> SaveAsync(IActor actor, CancellationToken token = default);
        public Type GetModelType(string modelName);
        public string? GetTableName(string modelName);
    }
}
