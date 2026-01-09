using ClosedXML.Excel;
using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Factory;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.UpdateTemplateWithData
{
    public class DownloadItemForUpdateCommand : IRequest<Result>
    {
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class DownloadItemForUpdateHandler : QueryServiceBase<SKUItem, DownloadItemForUpdateCommand>, IRequestHandler<DownloadItemForUpdateCommand, Result>
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;
        public DownloadItemForUpdateHandler(IBulkOperationTemplateFactory bulkOperationFactory, IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) :
            base(dbContext, actor, mapper, appCache)
        {
            _bulkOperationFactory = bulkOperationFactory;
        }


        public async Task<Result> Handle(DownloadItemForUpdateCommand request, CancellationToken ct)
        {
            
            var wb = await _bulkOperationFactory.GetService(BulkOperationTemplateType.Item_Update).GetTemplate();
            var ws = wb.Worksheet(1);
           
            var items = base.GetAll().ApplyFilters(request.Filter).Select(x =>
                new {
                    x.Id,
                    ItemName = x.Item.Name,
                    CategoryName = x.Item.Category.Name,
                    x.SKU,
                    x.VarianName,
                    UnitName = x.Unit.Name,
                    x.IsActive,
                    x.Item.CanBePurchased,
                    x.Item.CanBeSell,
                    x.SalePrice,
                    x.Item.DefaltPurchaseQty,
                    x.Item.DefaultMinimumStock,
                    DetailDefaultPurchaseQty = x.DefaultPurchaseQty,
                    DetailDefaultMinimumStock = x.MinimumStockQty,
                    x.Item.Notes,
                    
                }).ToList();


            var rowNumber = 2;
            foreach(var item in items)
            {
                var row = ws.Row(rowNumber);
                row.Cell("A").Value = item.Id.ToString();
                row.Cell("B").Value = item.ItemName;
                row.Cell("C").Value = item.CategoryName;
                row.Cell("D").Value = item.SKU;
                row.Cell("E").Value = item.VarianName;
                row.Cell("F").Value = item.UnitName;
                row.Cell("G").Value = item.IsActive;
                row.Cell("H").Value = item.CanBePurchased;
                row.Cell("I").Value = item.CanBeSell;
                row.Cell("J").Value = item.SalePrice;
                row.Cell("K").Value = item.DetailDefaultPurchaseQty?? item.DefaltPurchaseQty;
                row.Cell("L").Value = item.DetailDefaultMinimumStock ?? item.DefaultMinimumStock;
                row.Cell("M").Value = item.Notes;
                rowNumber++;
            }

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return Result.OK(ms);
        }
    }
}
