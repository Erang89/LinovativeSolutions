using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates.Factory;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.BlankTemplates
{
    public class BulkOperationExcelTemplateCommand : IRequest<Result>
    {
        public BulkOperationTemplateType Type { get; set; }
    }

    public class BulkOperationExcelTemplateHandler : IRequestHandler<BulkOperationExcelTemplateCommand, Result>
    {
        private readonly IBulkOperationTemplateFactory _bulkOperationFactory;
        public BulkOperationExcelTemplateHandler(IBulkOperationTemplateFactory factory)
        {
            _bulkOperationFactory = factory;
        }
        public async Task<Result> Handle(BulkOperationExcelTemplateCommand request, CancellationToken ct)
        {
            var ms = await _bulkOperationFactory.GetService(request.Type).GetTemplateMemoryStream();
            return Result.OK(ms);
        }
    }
}
