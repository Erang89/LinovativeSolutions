using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Configurations;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Customers;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Payments;
using LinoVative.Service.Core.People;
using LinoVative.Service.Core.Shifts;
using LinoVative.Service.Core.Sources;
using LinoVative.Service.Core.Suppliers;
using LinoVative.Service.Core.Warehoses;
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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        
        // Source DbSets
        public DbSet<CountryRegion> CountryRegions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<AppTimeZone> TimeZones { get; set; }
        public DbSet<Currency> Currencies { get; set; }


        // Items DbSet
        public DbSet<Item> Items{ get; set; }
        public DbSet<ItemUnit> ItemUnits { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<PriceType> PriceTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ItemPriceType> ItemPriceTypes { get; set; }


        // Customers DbSets
        public DbSet<Customer> Customers { get; set; }

        // Order Type DbSets
        public DbSet<OrderType> OrderTypes { get; set; }


        // Outlet DbSets
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<OutletArea> OutletAreas { get; set; }
        public DbSet<OutletBankNote> OutletBankNotes { get; set; }
        public DbSet<OutletItemCategory> OutletItemCategories { get; set; }
        public DbSet<OutletItemGroup> OutletItemGroups { get; set; }
        public DbSet<OutletOrderType> OutletOrderTypes { get; set; }
        public DbSet<OutletPaymentMethod> OutletPaymentMethods { get; set; }
        public DbSet<OutletShift> OutletShifts { get; set; }
        public DbSet<OutletTable> OutletTables { get; set; }
        public DbSet<OutletUser> OutletUsers { get; set; }
        public DbSet<OutletItemExceptional> OutletItemExceptionals { get; set; }


        // DbSet Payments
        public DbSet<BankNote> BankNotes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentMethodGroup> PaymentMethodGroups { get; set; }

        // Shifts
        public DbSet<Shift> Shifts { get; set; }

        // DbSet Peoples
        public DbSet<Person> People { get; set; }


        // DbSet WhareHouses
        public DbSet<Warehouse> WareHouses { get; set; }


        // Accountings
        public DbSet<Account> Accounts { get; set; }
        public DbSet<COAGroup> CoaGroups { get; set; }
        public DbSet<SalesCOAMapping> SalesCOAMappings { get; set; }


        public DbSet<ItemGroupBulkUpload> ItemGroupBulkUploads { get; set; }
        public DbSet<ItemGroupBulkUploadDetail> ItemGroupBulkUploadDetails { get; set; }
        public DbSet<ItemCategoryBulkUpload> ItemCategoryBulkUploads { get; set; }
        public DbSet<ItemCategoryBulkUploadDetail> ItemCategoryBulkUploadDetails { get; set; }
        public DbSet<ItemUnitBulkUpload> ItemUnitBulkUploads { get; set; }
        public DbSet<ItemUnitBulkUploadDetail> ItemUnitBulkUploadDetails { get; set; }
        public DbSet<ItemBulkUpload> ItemBulkUploads { get; set; }
        public DbSet<ItemBulkUploadDetail> ItemBulkUploadDetails { get; set; }


        // Supplier
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierAddress> SupplierAddress { get; set; }
        public DbSet<SupplierContact> SupplierContact { get; set; }


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

        public EntityState GetEntityState<T>(T entity)
        {
            return this.Entry(entity!).State;
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
