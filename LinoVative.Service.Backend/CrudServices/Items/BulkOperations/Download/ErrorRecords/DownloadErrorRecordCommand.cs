using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Enums;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperations.Download.ErrorRecords
{
    public class DownloadErrorRecordCommand : IRequest<Result>
    {
        public BulkOperationTypes? Type { get; set; }
    }

    public class DownloadErrorRecordHandler : IRequestHandler<DownloadErrorRecordCommand, Result>
    {
        private readonly IServiceProvider _serviceProvider;

        public DownloadErrorRecordHandler(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }


        private IDownloadErrorRecord GetProvider(BulkOperationTypes? type)
        {
            return type switch
            {
                BulkOperationTypes.ItemCreate => ActivatorUtilities.CreateInstance<DownloadErrorItemCreateService>(_serviceProvider),
                BulkOperationTypes.ItemUpdate => ActivatorUtilities.CreateInstance<DownloadErrorItemUpdateService>(_serviceProvider),
                BulkOperationTypes.ItemDelete => ActivatorUtilities.CreateInstance<DownloadErrorItemDeleteService>(_serviceProvider),

                BulkOperationTypes.GroupCreate => ActivatorUtilities.CreateInstance<DownloadErrorGroupCreateService>(_serviceProvider),
                BulkOperationTypes.GroupUpdate => ActivatorUtilities.CreateInstance<DownloadErrorGroupUpdateService>(_serviceProvider),
                BulkOperationTypes.GroupDelete => ActivatorUtilities.CreateInstance<DownloadErrorGroupDeleteService>(_serviceProvider),

                BulkOperationTypes.CategoryCreate => ActivatorUtilities.CreateInstance<DownloadErrorCategooryCreateService>(_serviceProvider),
                BulkOperationTypes.CategoryUpdate => ActivatorUtilities.CreateInstance<DownloadErrorCategoryUpdateService>(_serviceProvider),
                BulkOperationTypes.CategoryDelete => ActivatorUtilities.CreateInstance<DownloadErrorCategoryDeleteService>(_serviceProvider),

                BulkOperationTypes.UnitCreate => ActivatorUtilities.CreateInstance<DownloadErrorUnitCreateService>(_serviceProvider),
                BulkOperationTypes.UnitUpdate => ActivatorUtilities.CreateInstance<DownloadErrorUnitUpdateService>(_serviceProvider),
                BulkOperationTypes.UnitDelete => ActivatorUtilities.CreateInstance<DownloadErrorUnitDeleteService>(_serviceProvider),

                _ => throw new ArgumentException($"Unknown provider type: {type}")
            };
        }


        public async Task<Result> Handle(DownloadErrorRecordCommand request, CancellationToken ct)
            => Result.OK(await GetProvider(request.Type).DownloadRows());
    }
}
