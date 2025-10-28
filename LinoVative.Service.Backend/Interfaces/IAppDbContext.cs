﻿using Linovative.Shared.Interface;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Customers;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Payments;
using LinoVative.Service.Core.People;
using LinoVative.Service.Core.Sources;
using LinoVative.Service.Core.Warehoses;
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
        public DbSet<OutletShift> OutletShifts{ get; set; }
        public DbSet<OutletTable> OutletTables{ get; set; }
        public DbSet<OutletUser> OutletUsers{ get; set; }


        // DbSet Payments
        public DbSet<BankNote> BankNotes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods{ get; set; }
        public DbSet<PaymentMethodGroup> PaymentMethodGroups{ get; set; }


        // DbSet Peoples
        public DbSet<Person> People { get; set; }


        // Companies DbSet
        public DbSet<Company> Companies { get; set; }


        // DbSet WhareHouses
        public DbSet<Warehouse> WareHouses { get; set; }


        // Functions

        public DbSet<T> Set<T>() where T : class;
        public Task<Result> SaveAsync(IActor actor, CancellationToken token = default);
        public Type GetModelType(string modelName);
        public string? GetTableName(string modelName);
    }
}
