using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Configurations;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.CompanyDtos;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.DatabaseService
{
    public class AppDbContext : DbContext, IAppDbContext, IScoopService
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureDatabaseRelationship();
        }


        // Auth DbSets
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppUserCompany> UserCompanies { get; set; }
        public DbSet<AppUserApplication> UserApplications { get; set; }
        
        
        // Source DbSets
        public DbSet<Country> Countries { get; set; }
        public DbSet<AppTimeZone> TimeZones { get; set; }
        public DbSet<Currency> Currencies { get; set; }


        // Items DbSet
        public DbSet<ItemUnit> ItemUnits { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<CostumePriceTag> ItemPriceTags { get; set; }
        public DbSet<ItemCostumePrice> ItemCostumePrices { get; set; }


        // Companies DbSet
        public DbSet<Company> Companies { get; set; }


        // Functions
        public async Task<Result> SaveAsync(IActor actor, CancellationToken token = default)
        {
            await this.SaveChangesAsync(token);
            return Result.OK();
        }

        public Type? GetModelType(string modelName)
        {
            var type = this.Model
                .GetEntityTypes()
                .Where(x => x.ClrType.Name.ToLower() == modelName!.ToLower())
                .FirstOrDefault()?.ClrType;

            return type;
        }

        public string? GetTableName(string modelName)
        {
            var tableName = this.Model
                .GetEntityTypes()
                .Where(x => x.ClrType.Name.ToLower() == modelName.ToLower())
                .FirstOrDefault()?.GetTableName();


            return tableName;
        }

        public IQueryable GetAll<T>() where T : IDto
        {
            var queries = new Dictionary<Type, IQueryable>()
            {
                {typeof(CompanyDto), this.Set<Company>().Where(x => !x.IsDeleted)}
            };

            if (!queries.ContainsKey(typeof(T)))
                throw new Exception(string.Format("Query is not difined for the {0} yet", typeof(T).Name));

            return queries[typeof(T)];
        }
    }
}
