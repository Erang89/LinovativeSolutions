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
    public class DownloadItemCategoryForDeleteCommand : IRequest<Result>
    {
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class DownloadItemCategoryForDeleteHandler : QueryServiceBase<ItemCategory, DownloadItemCategoryForDeleteCommand>, IRequestHandler<DownloadItemCategoryForDeleteCommand, Result>
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;
        public DownloadItemCategoryForDeleteHandler(IBulkOperationTemplateFactory bulkOpFactory, IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) :
            base(dbContext, actor, mapper, appCache)
        {
            _bulkOperationFactory = bulkOpFactory;
        }


        public async Task<Result> Handle(DownloadItemCategoryForDeleteCommand request, CancellationToken ct)
        {
            var filter = request.Filter.FirstOrDefault(x => x.Field == "ItemGroup.Name");
            if (filter is not null) filter.Field = "Group.Name";

            var wb = await _bulkOperationFactory.GetService(BulkOperationTemplateType.Category_Delete).GetTemplate();
            var ws = wb.Worksheet(1);
            var categories = base.GetAll().ApplyFilters(request.Filter).Select(x => new { x.Id, x.Name, GroupName = x.Group!.Name }).ToList();

            ws.Column("A").Width = 45;
            ws.Column("B").Width = 70;
           
            var rowNumber = 2;
            foreach (var g in categories)
            {
                var row = ws.Row(rowNumber);
                row.Cell("A").Value = g.Id.ToString();
                row.Cell("B").Value = g.Name;
                rowNumber++;
            }

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return Result.OK(ms);
        }
    }
}
