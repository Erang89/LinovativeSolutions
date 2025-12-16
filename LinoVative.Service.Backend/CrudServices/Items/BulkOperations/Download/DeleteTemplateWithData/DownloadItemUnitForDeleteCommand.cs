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
    public class DownloadItemUnitForDeleteCommand : IRequest<Result>
    {
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class DownloadItemUnitForDeleteHandler : QueryServiceBase<ItemUnit, DownloadItemUnitForDeleteCommand>, IRequestHandler<DownloadItemUnitForDeleteCommand, Result>
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;
        public DownloadItemUnitForDeleteHandler(IBulkOperationTemplateFactory bulkOperationFactory, IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) :
            base(dbContext, actor, mapper, appCache)
        {
            _bulkOperationFactory = bulkOperationFactory;
        }


        public async Task<Result> Handle(DownloadItemUnitForDeleteCommand request, CancellationToken ct)
        {
            
            var wb = await _bulkOperationFactory.GetService(BulkOperationTemplateType.Unit_Delete).GetTemplate();
            var ws = wb.Worksheet(1);
            var groups = base.GetAll().ApplyFilters(request.Filter).Select(x => new { x.Id, x.Name }).ToList();

            ws.Column("A").Width = 45;
            ws.Column("B").Width = 70;

            var rowNumber = 2;
            foreach(var g in groups)
            {
                var row = ws.Row(rowNumber);
                row.Cell("A").Value = g.Id.ToString();
                row.Cell("B").Value = g.Name?.ToString();
                rowNumber++;
            }

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return Result.OK(ms);
        }
    }
}
