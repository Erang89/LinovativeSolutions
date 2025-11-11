using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Services.Extensions
{
    public static class JsonLocalizerExtensions
    {
        public static string POSCashier(this IJsonLocalizer localizer, string key) => localizer[$"POSCashierGlobal.{key}"];
        public static string Setting(this IJsonLocalizer localizer, string key) => localizer[$"SettingGlobal.{key}"];
        public static string Accomodation(this IJsonLocalizer localizer, string key) => localizer[$"AccomodationGlobal.{key}"];
        public static string Inventory(this IJsonLocalizer localizer, string key) => localizer[$"InventoryGlobal.{key}"];
        public static string Accounting(this IJsonLocalizer localizer, string key) => localizer[$"AccountingGlobal.{key}"];
        public static string PosManagement(this IJsonLocalizer localizer, string key) => localizer[$"POSManagementGlobal.{key}"];
        public static string Global(this IJsonLocalizer localizer, string key) => localizer[$"Global.{key}"];
    }
}
