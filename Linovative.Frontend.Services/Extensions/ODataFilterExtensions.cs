using Linovative.Frontend.Services.Models;

namespace Linovative.Frontend.Services.Extensions
{
    public static class ODataFilterExtensions
    {
        public static string GetODataUrl(this ODataFilter? oDataFilter, string prefix)
        {
            if (oDataFilter is null)
                return $"oData/{prefix}?$count=true";

            var pageNumber = oDataFilter.PageNumber < 1 ? 1 : oDataFilter.PageNumber;
            var skip = (pageNumber - 1) * oDataFilter.PageSize;
            var url = $"oData/{prefix}?$count=true&$top={oDataFilter.PageSize}&$skip={skip}";
            if (oDataFilter.Options is not null)
                url += $"&{oDataFilter.Options}";

            if (oDataFilter.OrderFiled is not null && oDataFilter.SortDirection is not null)
            {
                url += $"&$orderby={oDataFilter.OrderFiled} {oDataFilter.SortDirection}";
            }

            return url;
        }

    }

}
