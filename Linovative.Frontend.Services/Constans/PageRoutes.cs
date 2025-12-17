namespace Linovative.Frontend.Services.Constans
{
    public static class PageRoutes
    {
        public const string Home = "/";
        public static class PostManagement
        {
            public const string IndexMaster = "pos/management";
            public static class ItemMaster
            {
                public const string IndexItems = $"{PostManagement.IndexMaster}/item-master";
                public const string Item = $"{IndexItems}/items";
                public const string Groups = $"{IndexItems}/groups";
                public const string Categories = $"{IndexItems}/categories";
                public const string Units = $"{IndexItems}/units";
                public const string BulkOperations = $"{IndexItems}/bulk-operations";
            }


            public static class PosSettings
            {
                public const string IndexPosSettings = $"{IndexMaster}/Settings";
                public static class Outlets
                {
                    public const string IndexOutlets = $"{PosSettings.IndexPosSettings}/outlets";
                    public const string OutletList = $"{IndexOutlets}/lists";
                    public const string OutletListAdd = $"{IndexOutlets}/add";
                    public const string OutletListUpdate = IndexOutlets + "/{Id}";
                    public const string WorkingShifts = $"{IndexOutlets}/shifts";
                    public const string OutletUsers = $"{IndexOutlets}/users";
                    public const string Table = $"{IndexOutlets}/tables";
                }

                public static class Trasactions
                {
                    public const string Index = $"{PosSettings.IndexPosSettings}/transactions";
                    public static class Payments
                    {
                        public const string PaymentMethod = $"{Trasactions.Index}/payment-methods";
                        public const string PaymentMethodGroup = $"{Trasactions.Index}/payment-method-groups";
                        public const string OrderTypes = $"{Trasactions.Index}/order-types";
                        public const string BankNotes = $"{Trasactions.Index}/bank-notes";
                        public const string DiscountLimits = $"{Trasactions.Index}/discount-limits";
                    }
                }

            }


            public static class Customers
            {
                public const string IndexCustomers = "Customers";
                public const string Groups = $"{IndexCustomers}/groups";
            }

        }

        public static class Settings
        {
            public const string IndexSettings = $"/Settings";
            public const string Users = $"{IndexSettings}/users";
            public const string CompanyProfile = $"{IndexSettings}/company/profile";
            public const string Warehouse = $"{IndexSettings}/warehouse";
            public const string Currency = $"{IndexSettings}/currency";
            public const string CurrencyRate = $"{IndexSettings}/currency/rate";
            public const string CurrencyGroup = $"{IndexSettings}/currency/group";
            public static class Accounting
            {
                public const string AccountingIndex = $"{IndexSettings}/accounting";
                public const string Coa = $"{AccountingIndex}/coa";
                public const string CoaGroup = $"{AccountingIndex}/coa/group";
            }


        }

        public static class Inventory
        {
            public const string Index = "/inventory";
            public static class Settings
            {
                public const string Index = $"{Inventory.Index}/settings";
                public const string WareHouse = $"{Settings.Index}/warehouse";
            }


        }

        public static class Accounting
        {
            public const string Index = "accounting";
            public static class Settings
            {
                public const string Index = $"{Accounting.Index}/settings";
                public const string Accounts = $"{Index}/accounts";
                public const string AccountGroup = $"{Accounts}/groups";
                public const string OutletSalesCoaMappings = $"{Index}/outlet-sales-coa-mapping";
            }
        }

        public static class Accomodation
        {
            public const string IndexAccomodation = "accomodation";
        }

        public static class POSCashier
        {
            public const string IndexPosCashier = "pos";
        }

        public const string Logout = "/logout";
        public const string Login = "/login";
    }
}
