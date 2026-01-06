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

    public class DownloadItemForUpdateHandler : QueryServiceBase<Item, DownloadItemForUpdateCommand>, IRequestHandler<DownloadItemForUpdateCommand, Result>
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
           
            ws.Column("A").Width = 45;
            ws.Column("B").Width = 30;
            ws.Column("C").Width = 50;
            ws.Column("D").Width = 25;
            ws.Column("E").Width = 25;
            ws.Column("F").Width = 25;
            ws.Column("G").Width = 25;

            var groups = base.GetAll().ApplyFilters(request.Filter).Select(x =>
                new {
                    x.Id,
                    x.Name,
                    //UnitName = x.Unit!.Name,
                    //GroupName = x.Category!.Group!.Name,
                    //CategoryName = x.Category.Name,
                    //x.SellPrice,
                }).ToList();


            var rowNumber = 2;
            foreach(var g in groups)
            {
                var row = ws.Row(rowNumber);
                row.Cell("A").Value = g.Id.ToString();
                row.Cell("C").Value = g.Name;
                //row.Cell("D").Value = g.UnitName;
                //row.Cell("E").Value = g.GroupName;
                //row.Cell("F").Value = g.CategoryName;
                //row.Cell("G").Value = g.SellPrice;
                rowNumber++;
            }

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return Result.OK(ms);
        }
    }
}
