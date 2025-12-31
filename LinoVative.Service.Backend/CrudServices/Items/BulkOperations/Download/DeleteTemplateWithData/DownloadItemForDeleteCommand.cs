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

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.DeleteTemplateWithData
{
    public class DownloadItemForDeleteCommand : IRequest<Result>
    {
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class DownloadItemForDeleteHandler : QueryServiceBase<Item, DownloadItemForDeleteCommand>, IRequestHandler<DownloadItemForDeleteCommand, Result>
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;
        public DownloadItemForDeleteHandler(IBulkOperationTemplateFactory bulkOperationFactory, IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) :
            base(dbContext, actor, mapper, appCache)
        {
            _bulkOperationFactory = bulkOperationFactory;
        }


        public async Task<Result> Handle(DownloadItemForDeleteCommand request, CancellationToken ct)
        {
            
            var wb = await _bulkOperationFactory.GetService(BulkOperationTemplateType.Item_Delete).GetTemplate();
            var ws = wb.Worksheet(1);
           
            ws.Column("A").Width = 45;
            ws.Column("B").Width = 30;
            ws.Column("C").Width = 50;

            var groups = base.GetAll().ApplyFilters(request.Filter).Select(x =>
                new {
                    x.Id,
                    x.Code,
                    x.Name,
                }).ToList();


            var rowNumber = 2;
            foreach(var g in groups)
            {
                var row = ws.Row(rowNumber);
                row.Cell("A").Value = g.Id.ToString();
                row.Cell("B").Value = g.Code;
                row.Cell("C").Value = g.Name;
                rowNumber++;
            }

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return Result.OK(ms);
        }
    }
}
