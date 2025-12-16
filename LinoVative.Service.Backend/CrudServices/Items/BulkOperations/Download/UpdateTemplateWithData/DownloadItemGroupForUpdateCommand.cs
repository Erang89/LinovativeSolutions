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
    public class DownloadItemGroupForUpdateCommand : IRequest<Result>
    {
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class DownloadItemGroupForUpdateHandler : QueryServiceBase<ItemGroup, DownloadItemGroupForUpdateCommand>, IRequestHandler<DownloadItemGroupForUpdateCommand, Result>
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;

        public DownloadItemGroupForUpdateHandler(IBulkOperationTemplateFactory bulkOperationFactory, IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) :
            base(dbContext, actor, mapper, appCache)
        {
            _bulkOperationFactory = bulkOperationFactory;
        }


        public async Task<Result> Handle(DownloadItemGroupForUpdateCommand request, CancellationToken ct)
        {
            var wb = await _bulkOperationFactory.GetService(BulkOperationTemplateType.Group_Update).GetTemplate();  
            var ws = wb.Worksheet(1);
            var groups = base.GetAll().ApplyFilters(request.Filter).Select(x => new { x.Id, x.Name }).ToList();

            var rowNumber = 2;
            foreach (var g in groups)
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
