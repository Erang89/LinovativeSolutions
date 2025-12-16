using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Category;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Group;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Item;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations.Unit;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations
{
    public class BulkOperationProcessCommand : IRequest<Result>
    {
        public BulkOperationTypes? Type { get; set; }
        public Dictionary<string, string> FieldMapping { get; set; } = new();
    }

    public class BulkOperationProcessHandler : IRequestHandler<BulkOperationProcessCommand, Result>
    {
        private readonly IServiceProvider _serviceProvider;

        public BulkOperationProcessHandler(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }

        private IBulkOperationProcess GetBulkOperationProvider(BulkOperationTypes? type)
        {
            return type switch
            {
                BulkOperationTypes.ItemCreate => ActivatorUtilities.CreateInstance<BulkCreateItemService>(_serviceProvider),
                BulkOperationTypes.ItemUpdate => ActivatorUtilities.CreateInstance<BulkUpdateItemService>(_serviceProvider),
                BulkOperationTypes.ItemDelete => ActivatorUtilities.CreateInstance<BulkDeleteItemService>(_serviceProvider),

                BulkOperationTypes.GroupCreate => ActivatorUtilities.CreateInstance<BulkCreateGroupService>(_serviceProvider),
                BulkOperationTypes.GroupUpdate => ActivatorUtilities.CreateInstance<BulkUpdateGroupService>(_serviceProvider),
                BulkOperationTypes.GroupDelete => ActivatorUtilities.CreateInstance<BulkDeleteItemGroupService>(_serviceProvider),

                BulkOperationTypes.CategoryCreate => ActivatorUtilities.CreateInstance<BulkCreateCategoryService>(_serviceProvider),
                BulkOperationTypes.CategoryUpdate => ActivatorUtilities.CreateInstance<BulkUpdateCategoryService>(_serviceProvider),
                BulkOperationTypes.CategoryDelete => ActivatorUtilities.CreateInstance<BulkCreateCategoryService>(_serviceProvider),

                BulkOperationTypes.UnitCreate => ActivatorUtilities.CreateInstance<BulkCreateUnitService>(_serviceProvider),
                BulkOperationTypes.UnitUpdate => ActivatorUtilities.CreateInstance<BulkUpdateUnitService>(_serviceProvider),
                BulkOperationTypes.UnitDelete => ActivatorUtilities.CreateInstance<BulkDeleteUnitService>(_serviceProvider),

                _ => throw new ArgumentException($"Unknown provider type: {type}")
            };
        }


        public Task<Result> Handle(BulkOperationProcessCommand request, CancellationToken ct) =>
            GetBulkOperationProvider(request.Type).Save(request.FieldMapping, ct);
    }
}
