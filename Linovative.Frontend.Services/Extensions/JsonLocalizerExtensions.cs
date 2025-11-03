using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Services.Extensions
{
    public static class JsonLocalizerExtensions
    {
        public static string PosManagement(this IJsonLocalizer localizer, string key) => localizer[$"POSManagementGlobal.{key}"];
        public static string Global(this IJsonLocalizer localizer, string key) => localizer[$"Global.{key}"];
    }
}
