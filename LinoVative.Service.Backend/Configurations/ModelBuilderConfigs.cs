using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.BulkUploads;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Customers;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Payments;
using LinoVative.Service.Core.People;
using LinoVative.Service.Core.Shifts;
using LinoVative.Service.Core.Sources;
using LinoVative.Service.Core.Warehoses;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.Configurations
{
    internal static class ModelBuilderConfigs
    {

        public static void ConfigureDatabaseRelationship(this ModelBuilder modelBuilder)
        {

            modelBuilder.ConfigureTempDataRelationship();

            // Mapping Auth
            modelBuilder.Entity<AppUser>(x =>
            {
                x.ToTable("Users");
            });
            modelBuilder.Entity<AppUserApplication>().ToTable("UserApplications");
            modelBuilder.Entity<AppUserCompany>().ToTable("UserCompanies");
            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.ToTable("RefreshTokens");
                e.Property(x => x.Token).IsRequired();
                e.Property(x => x.IPAddressLogin).IsRequired();
                e.Property(x => x.UserId).IsRequired();
            });

            // Company Mappings
            modelBuilder.Entity<Company>(x =>
            {
                x.ToTable("Companies");
                x.HasOne<AppTimeZone>().WithMany().HasForeignKey(x => x.TimeZoneId).IsRequired();
                x.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId).IsRequired();
                x.HasOne(x => x.Currency).WithMany().HasForeignKey(x => x.CurrencyId).IsRequired();
            });

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
                x.Property(x => x.SellPrice).HasPrecision(18, 4);
            });

            modelBuilder.Entity<ItemCostumePrice>(x =>
            {
                x.ToTable("ItemCustomePrices");
                x.HasOne(x => x.CostumePriceTag).WithMany().HasForeignKey(x => x.CostumePriceTagId).IsRequired();
                x.HasOne(x => x.Item).WithMany(x => x.CostumePrices).HasForeignKey(x => x.ItemId).IsRequired();
                x.Property(x => x.Price).HasPrecision(18, 4);
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


            // Person
            modelBuilder.Entity<Person>(x =>
            {
                x.ToTable("People");
                x.Property(x => x.Firstname).IsRequired();
                x.Property(x => x.Title).HasColumnType("varchar(10)").IsRequired();
            });


            // Shift
            modelBuilder.Entity<Shift>(x =>
            {
                x.ToTable("Shifts");
                x.Property(x => x.Name).IsRequired();
                x.Property(x => x.StartTime).IsRequired();
                x.Property(x => x.EndTime).IsRequired();
            });



            // Warehouse
            modelBuilder.Entity<Warehouse>(x =>
            {
                x.ToTable("Warehouses");
                x.Property(x => x.Name).IsRequired();
                x.Property(x => x.Code).IsRequired();
            });


            modelBuilder.ConfigurePaymentEntities();
            modelBuilder.ConfigureOutletEntities();
            modelBuilder.ConfigureOrderTypeEntities();
            modelBuilder.ConfigureCustomerEntities();
            modelBuilder.ConfigureAccountingEntities();

        }



        static void ConfigurePaymentEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankNote>(x =>
            {
                x.ToTable("BankNotes");
                x.Property(x => x.Note).HasPrecision(14, 2).IsRequired();
            });

            modelBuilder.Entity<PaymentMethodGroup>(x =>
            {
                x.ToTable("PaymentMethodGroups");
                x.Property(x => x.Name).IsRequired();
            });

            modelBuilder.Entity<PaymentMethod>(x =>
            {
                x.ToTable("PaymentMethods");
                x.Property(x => x.Name).IsRequired();
                x.Property(x => x.Type).HasColumnType("Varchar(20)");
                x.Property(x => x.PaymentMethodGroupId).IsRequired();
                x.HasOne(x => x.PaymentMethodGroup).WithMany(x => x.PaymentMethods).HasForeignKey(x => x.PaymentMethodGroupId);
            });

        }

        static void ConfigureOutletEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Outlet>(x =>
            {
                x.ToTable("Outlets");
                x.Property(x => x.Name).IsRequired();
                x.Property(x => x.DefaultServicePercent).HasPrecision(14, 2);
                x.Property(x => x.DefaultTaxPercent).HasPrecision(14, 2);
            });

            modelBuilder.Entity<OutletArea>(x =>
            {
                x.ToTable("OutletAreas");
                x.Property(x => x.Name).IsRequired();
                x.HasOne(x => x.Outlet).WithMany(x => x.Areas).HasForeignKey(x => x.OutletId).IsRequired();
            });


            modelBuilder.Entity<OutletUser>(x =>
            {
                x.ToTable("OutletUsers");
                x.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).IsRequired();
                x.HasOne(x => x.Outlet).WithMany(x => x.Users).HasForeignKey(x => x.OutletId).IsRequired();
            });

            modelBuilder.Entity<OutletTable>(x =>
            {
                x.ToTable("OutletTable");
                x.Property(x => x.Name).IsRequired();
                x.HasOne(x => x.Area).WithMany(x => x.Tables).HasForeignKey(x => x.AreaId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<OutletShift>(x =>
            {
                x.ToTable("OutletShifts");
                x.HasOne(x => x.Outlet).WithMany(x => x.Shifts).HasForeignKey(x => x.OutletId).IsRequired();
                x.HasOne(x => x.Shift).WithMany().HasForeignKey(x => x.ShiftId).IsRequired();
            });

            modelBuilder.Entity<OutletPaymentMethod>(x =>
            {
                x.ToTable("OutletPaymentMethods");
                x.HasOne(x => x.Outlet).WithMany(x => x.PaymentMethods).HasForeignKey(x => x.OutletId).IsRequired();
                x.HasOne(x => x.PaymentMethod).WithMany().HasForeignKey(x => x.PaymentMethodId).IsRequired();
            });

            modelBuilder.Entity<OutletOrderType>(x =>
            {
                x.ToTable("OutletOrderTypes");
                x.HasOne(x => x.Outlet).WithMany(x => x.OrderTypes).HasForeignKey(x => x.OutletId).IsRequired();
                x.HasOne(x => x.OrderType).WithMany().HasForeignKey(x => x.OrderTypeId).IsRequired();
                x.Property(x => x.TaxPercent).HasPrecision(14, 2);
                x.Property(x => x.ServicePercent).HasPrecision(14, 2);
            });


            modelBuilder.Entity<OutletItemGroup>(x =>
            {
                x.ToTable("OutletItemGroups");
                x.HasOne(x => x.Outlet).WithMany(x => x.ItemGroups).HasForeignKey(x => x.OutletId).IsRequired();
                x.HasOne(x => x.ItemGroup).WithMany().HasForeignKey(x => x.ItemGroupId).IsRequired();
            });


            modelBuilder.Entity<OutletItemCategory>(x =>
            {
                x.ToTable("OutletItemCategories");
                x.HasOne(x => x.Outlet).WithMany(x => x.ItemCategories).HasForeignKey(x => x.OutletId).IsRequired();
                x.HasOne(x => x.ItemCategory).WithMany().HasForeignKey(x => x.ItemCategoryId).IsRequired();
            });

            modelBuilder.Entity<OutletBankNote>(x =>
            {
                x.ToTable("OutletBankNotes");
                x.HasOne(x => x.Outlet).WithMany(x => x.BankNotes).HasForeignKey(x => x.OutletId).IsRequired();
                x.HasOne(x => x.BankNote).WithMany().HasForeignKey(x => x.BankNoteId).IsRequired();
            });


            modelBuilder.Entity<OutletArea>(x =>
            {
                x.ToTable("OutletAreas");
                x.HasOne(x => x.Outlet).WithMany(x => x.Areas).HasForeignKey(x => x.OutletId).IsRequired();
                x.Property(x => x.Name).IsRequired();
            });
        }

        static void ConfigureOrderTypeEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderType>(x =>
            {
                x.ToTable("OrderTypes");
                x.Property(x => x.Name).IsRequired();
            });
        }

        static void ConfigureCustomerEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(x =>
            {
                x.ToTable("Customers");
                x.HasOne(x => x.Person).WithOne().HasForeignKey<Customer>(x => x.PersonId);
            });
        }

        static void ConfigureAccountingEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(x =>
            {
                x.ToTable("Accounts");
                x.Property(x => x.Name).IsRequired();
                x.HasOne(x => x.Parent).WithMany(x => x.Childs).HasForeignKey(x => x.ParentAccountId);
            });

            modelBuilder.Entity<COAGroup>(x =>
            {
                x.ToTable("COAGroup");
                x.Property(x => x.Name).IsRequired();
                x.Property(x => x.Type).HasColumnType("Varchar(25)");
            });

            modelBuilder.Entity<SalesCOAMapping>(x =>
            {
                x.ToTable("SalesCoaMappings");
                x.HasOne(x => x.Outlet).WithMany().HasForeignKey(x => x.OutletId).IsRequired();
                x.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId).IsRequired();

            });
        }

        public static void ConfigureTempDataRelationship(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemGroupBulkUpload>(x =>
            {
                x.ToTable("ItemGroupBulkUpload", "temp");
                x.Property(x => x.headerColum1).HasColumnType("varchar(100)");
                x.Property(x => x.headerColum2).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<ItemGroupBulkUploadDetail>(x =>
            {
                x.ToTable("ItemGroupBulkUploadDetail", "temp");
                x.Property(x => x.Column1).HasColumnType("varchar(100)");
                x.Property(x => x.Column2).HasColumnType("varchar(100)");
                x.HasOne(x => x.ItemGroupBulkUpload).WithMany(x => x.Details).HasForeignKey(x => x.ItemGroupBulkUploadId).IsRequired();
            });


            modelBuilder.Entity<ItemCategoryBulkUpload>(x =>
            {
                x.ToTable("ItemCategoryBulkUpload", "temp");
                x.Property(x => x.headerColum1).HasColumnType("varchar(100)");
                x.Property(x => x.headerColum2).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<ItemCategoryBulkUploadDetail>(x =>
            {
                x.ToTable("ItemCategoryBulkUploadDetail", "temp");
                x.Property(x => x.Column1).HasColumnType("varchar(100)");
                x.Property(x => x.Column2).HasColumnType("varchar(100)");
                x.HasOne(x => x.ItemCategoryBulkUpload).WithMany(x => x.Details).HasForeignKey(x => x.ItemCategoryBulkUploadId).IsRequired();
            });

            modelBuilder.Entity<ItemUnitBulkUpload>(x =>
            {
                x.ToTable("ItemUnitBulkUpload", "temp");
                x.Property(x => x.headerColum1).HasColumnType("varchar(100)");
                x.Property(x => x.headerColum2).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<ItemUnitBulkUploadDetail>(x =>
            {
                x.ToTable("ItemUnitBulkUploadDetail", "temp");
                x.Property(x => x.Column1).HasColumnType("varchar(100)");
                x.Property(x => x.Column2).HasColumnType("varchar(100)");
                x.HasOne(x => x.ItemUnitBulkUpload).WithMany(x => x.Details).HasForeignKey(x => x.ItemUnitBulkUploadId).IsRequired();
            });

            modelBuilder.Entity<ItemBulkUpload>(x =>
            {
                x.ToTable("ItemBulkUpload", "temp");
                x.Property(x => x.headerColum1).HasColumnType("varchar(100)");
                x.Property(x => x.headerColum2).HasColumnType("varchar(100)");
                x.Property(x => x.headerColum3).HasColumnType("varchar(100)");
                x.Property(x => x.headerColum4).HasColumnType("varchar(100)");
                x.Property(x => x.headerColum5).HasColumnType("varchar(100)");
                x.Property(x => x.headerColum6).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<ItemBulkUploadDetail>(x =>
            {
                x.ToTable("ItemBulkUploadDetail", "temp");
                x.Property(x => x.Column1).HasColumnType("varchar(100)");
                x.Property(x => x.Column2).HasColumnType("varchar(100)");
                x.Property(x => x.Column3).HasColumnType("varchar(100)");
                x.Property(x => x.Column4).HasColumnType("varchar(100)");
                x.Property(x => x.Column5).HasColumnType("varchar(100)");
                x.Property(x => x.Column6).HasColumnType("varchar(100)");
                x.HasOne(x => x.ItemBulkUpload).WithMany(x => x.Details).HasForeignKey(x => x.ItemBulkUploadId).IsRequired();
            });

        }
    }
}
